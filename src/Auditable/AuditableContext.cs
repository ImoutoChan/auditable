using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Auditable.Collectors.EntityId;
using Auditable.Delta;
using Auditable.Infrastructure;
using Auditable.Parsing;
using Auditable.Writers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace Auditable;

public class AuditableContext : IInternalAuditableContext
{
    private readonly IAuditIdGenerator _auditIdGenerator;
    private readonly IDifferenceEngine _engine;
    private readonly IEntityIdCollector _entityIdCollector;
    private readonly ILogger<AuditableContext> _logger;
    private readonly IParser _parser;
    private readonly JsonSerializer _serializer;
    private readonly TargetCollection _target;
    private readonly IWriter _writer;
    private string _name;

    public AuditableContext(
        IParser parser,
        IEntityIdCollector entityIdCollector,
        JsonSerializer serializer,
        IDifferenceEngine engine,
        IWriter writer,
        IAuditIdGenerator auditIdGenerator,
        ILogger<AuditableContext> logger
    )
    {
        _parser = parser;
        _entityIdCollector = entityIdCollector;
        _serializer = serializer;
        _engine = engine;
        _writer = writer;
        _auditIdGenerator = auditIdGenerator;
        _logger = logger;
        _target = new TargetCollection(_entityIdCollector);
    }

    public void WatchTargets(params object[] targets)
    {
        foreach (var instance in targets)
        {
            var copy = _serializer.Serialize(instance);
            var id = _entityIdCollector.Extract(instance);
            var type = instance.GetType();

            var target = new Target
            {
                Type = type.FullName,
                Id = id,
                Before = copy,
                Instance = instance,
                ActionStyle = ActionStyle.Observed
            };

            _target.Add(instance, target);
        }
    }

    public void Created<T>(T target) => MarkCreated<T>(target);

    public void Removed<T>(string id)
    {
        Code.Require(() => !string.IsNullOrEmpty(id), nameof(id));
        SetExplicateAction<T>(id, AuditType.Removed);
    }

    public void Removed<T>(T target)
    {
        var id = _entityIdCollector.Extract(target);
        SetExplicateAction<T>(id, AuditType.Removed);
    }

    public void Read<T>(string id)
    {
        Code.Require(() => !string.IsNullOrEmpty(id), nameof(id));
        SetExplicateAction<T>(id, AuditType.Read);
    }

    public void Read<T>(T target)
    {
        var id = _entityIdCollector.Extract(target);
        SetExplicateAction<T>(id, AuditType.Read);
    }

    public async Task WriteLog()
    {
        foreach (var target in _target)
        {
            if (target.ActionStyle == ActionStyle.Explicit) continue;

            var possibleChange = _serializer.Serialize(target.Instance);
            var diff = _engine.Differences(target.Before, possibleChange);
            target.Delta = diff;
        }

        var id = _auditIdGenerator.GenerateId();
        var parsedOutput = await _parser.Parse(id, _name, _target);
        await _writer.Write(id, _name, parsedOutput);
    }

    public void SetName(string name)
    {
        Code.Require(() => !string.IsNullOrEmpty(name), nameof(name));
        _name = name;
    }


    /// <summary>
    ///     this will write the audit log if the code did not throw an exception
    /// </summary>
    public void Dispose()
    {
        DisposeAsync().AsTask().Wait();
    }

    /// <summary>
    ///     this will write the audit log if the code did not throw an exception
    /// </summary>
    /// <remarks>
    ///     https://stackoverflow.com/questions/149609/c-sharp-using-syntax
    ///     https://ayende.com/blog/2577/did-you-know-find-out-if-an-exception-was-thrown-from-a-finally-block
    /// </remarks>
    public async ValueTask DisposeAsync()
    {
        if (Marshal.GetExceptionCode() == 0)
            await WriteLog();
        else
            _logger.LogDebug("code had an exception, will not write the audit entry");
    }

    private void SetExplicateAction<T>(string id, AuditType action)
    {
        Code.Require(() => !string.IsNullOrEmpty(id), nameof(id));

        var type = typeof(T);

        if (!_target.TryGet(type, id, out var target))
        {
            target = new Target
            {
                Type = type.FullName,
                Id = id
            };

            _target.Add(type, id, target);
        }

        target.ActionStyle = ActionStyle.Explicit;
        target.AuditType = action;
    }

    private void MarkCreated<T>(object target)
    {
        var id = _entityIdCollector.Extract(target);

        var type = typeof(T);

        var entry = _serializer.Serialize(target);
        var delta = JToken.Parse(entry);
        
        _target.Add(type, id, new Target
        {
            Type = type.FullName,
            Id = id,
            Delta = delta,
            ActionStyle = ActionStyle.Explicit,
            AuditType = AuditType.Created
        });
    }
}
