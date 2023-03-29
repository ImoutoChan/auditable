using System.IO;
using System.Threading.Tasks;
using Auditable.Infrastructure;
using Auditable.Parsing;
using Microsoft.Extensions.Options;

namespace Auditable.Writers.File;

internal class FileWriter : IWriter
{
    private readonly IOptions<FileWriterOptions> _options;

    public FileWriter(IOptions<FileWriterOptions> options)
    {
        _options = options;
    }

    public Task Write(string auditId, string action, AuditableEntry entry)
    {
        var file = _options.Value.GetFileName(auditId, action);
        var folder = _options.Value.Folder;
        var path = Path.Combine(folder, file);
        System.IO.File.WriteAllText(path, AuditJsonSerializer.Serialize(entry));
        return Task.CompletedTask;
    }
}
