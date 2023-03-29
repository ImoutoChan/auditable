using System.IO;
using System.Threading.Tasks;
using Auditable.Infrastructure;
using Auditable.Parsing;
using Microsoft.Extensions.Options;

namespace Auditable.Writers.File;

internal class FileWriter : IWriter
{
    private readonly IOptions<FileWriterOptions> _options;
    private readonly IAuditJsonSerializer _auditJsonSerializer;

    public FileWriter(IOptions<FileWriterOptions> options, IAuditJsonSerializer auditJsonSerializer)
    {
        _options = options;
        _auditJsonSerializer = auditJsonSerializer;
    }

    public Task Write(string auditId, string action, AuditableEntry entry)
    {
        var file = _options.Value.GetFileName(auditId, action);
        var folder = _options.Value.Folder;
        var path = Path.Combine(folder, file);
        System.IO.File.WriteAllText(path, _auditJsonSerializer.Serialize(entry));
        return Task.CompletedTask;
    }
}
