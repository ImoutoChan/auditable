using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Auditable.Collectors.EntityId;
using Auditable.Delta;
using Auditable.Infrastructure;
using Auditable.Parsing;
using Auditable.Writers;
using Newtonsoft.Json.Linq;

namespace Auditable;

internal class AuditableContext : IInternalAuditableContext
{
    private readonly IDeltaCalculator _engine;
    private readonly IEntityIdCollector _entityIdCollector;
    private readonly IParser _parser;
    private readonly TargetCollection _targets;
    private readonly IWriter _writer;
    private readonly IAuditJsonSerializer _auditJsonSerializer;
    private string _name;

    public AuditableContext(
        IParser parser,
        IEntityIdCollector entityIdCollector,
        IDeltaCalculator engine,
        IWriter writer,
        IAuditJsonSerializer auditJsonSerializer)
    {
        _parser = parser;
        _entityIdCollector = entityIdCollector;
        _engine = engine;
        _writer = writer;
        _auditJsonSerializer = auditJsonSerializer;
        _targets = new TargetCollection(_entityIdCollector);
        _name = "Unknown context";
    }

    public void Observe<T>(params T[] targets) where T : notnull
    {
        foreach (var instance in targets)
        {
            var copy = _auditJsonSerializer.Serialize(instance);
            var id = _entityIdCollector.Extract(instance);
            var type = typeof(T);

            var target = new Target
            {
                Type = type.FullName!,
                Id = id,
                Before = copy,
                Instance = instance,
                ActionStyle = ActionStyle.Observed,
                AuditType = AuditType.Modified
            };

            _targets.Add(instance, target);
        }
    }

    public void Created<T>(T target) where T : notnull => MarkCreated(target);

    public void Removed<T>(string id) where T : notnull => SetTargetAuditType<T>(id, AuditType.Removed);

    public void Removed<T>(T target) where T : notnull
    {
        var id = _entityIdCollector.Extract(target);
        SetTargetAuditType<T>(id, AuditType.Removed);
    }

    public void Read<T>(string id) where T : notnull => SetTargetAuditType<T>(id, AuditType.Read);

    public void Read<T>(T target) where T : notnull
    {
        var id = _entityIdCollector.Extract(target);
        SetTargetAuditType<T>(id, AuditType.Read);
    }

    public async Task Flush()
    {
        foreach (var target in _targets)
        {
            if (target.ActionStyle == ActionStyle.Explicit || target.Instance == null || target.Before == null)
                continue;

            var after = _auditJsonSerializer.Serialize(target.Instance);
            target.Delta = _engine.Calculate(target.Before, after);
        }

        var auditId = Guid.NewGuid().ToString();
        var entry = await _parser.Parse(auditId, _name, _targets);
        await _writer.Write(auditId, _name, entry);
    }

    public void SetName(string name) => _name = name;


    /// <summary>
    ///     This will write the audit log if the code did not throw an exception.
    /// </summary>
    void IDisposable.Dispose() => DisposeAsync().AsTask().Wait();

    /// <summary>
    ///     This will write the audit log if the code did not throw an exception.
    /// </summary>
    public async ValueTask DisposeAsync()
    {
        if (Marshal.GetExceptionPointers() == 0)
            await Flush();
    }

    private void SetTargetAuditType<T>(string id, AuditType action)
    {
        var type = typeof(T);

        if (!_targets.TryGet(type, id, out var target))
        {
            target = new Target
            {
                Type = type.FullName!,
                Id = id,
                AuditType = action,
                ActionStyle = ActionStyle.Explicit
            };

            _targets.Add(type, id, target);
        }

        target.ActionStyle = ActionStyle.Explicit;
        target.AuditType = action;
    }

    private void MarkCreated<T>(T target) where T : notnull
    {
        var id = _entityIdCollector.Extract(target);

        var type = typeof(T);

        var entry = _auditJsonSerializer.Serialize(target);
        var delta = JToken.Parse(entry);

        _targets.Add(type, id, new Target
        {
            Type = type.FullName!,
            Id = id,
            Delta = delta,
            ActionStyle = ActionStyle.Explicit,
            AuditType = AuditType.Created
        });
    }
}
