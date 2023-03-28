using System.IO;
using System.Threading.Tasks;
using Auditable.Infrastructure;
using Auditable.Parsing;
using Microsoft.Extensions.Options;

namespace Auditable.Writers.File;

public class FileWriter : IWriter
{
    private readonly JsonSerializer _jsonSerializer;
    private readonly IOptions<FileWriterOptions> _options;

    public FileWriter(IOptions<FileWriterOptions> options, JsonSerializer jsonSerializer)
    {
        Code.Require(() => options != null, nameof(options));
        _options = options;
        _jsonSerializer = jsonSerializer;
    }

    public Task Write(string id, string action, AuditableEntry entry)
    {
        Code.Require(() => !string.IsNullOrEmpty(id), nameof(id));
        Code.Require(() => !string.IsNullOrEmpty(action), nameof(action));

        var file = _options.Value.GetFileName(id, action);
        var folder = _options.Value.Folder;
        var path = Path.Combine(folder, file);
        System.IO.File.WriteAllText(path, _jsonSerializer.Serialize(entry));
        return Task.CompletedTask;
    }
}
