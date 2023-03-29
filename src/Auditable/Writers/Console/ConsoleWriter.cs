using System.Threading.Tasks;
using Auditable.Infrastructure;
using Auditable.Parsing;

namespace Auditable.Writers.Console;

internal class ConsoleWriter : IWriter
{
    private readonly IAuditJsonSerializer _auditJsonSerializer;

    public ConsoleWriter(IAuditJsonSerializer auditJsonSerializer) => _auditJsonSerializer = auditJsonSerializer;

    public Task Write(string auditId, string action, AuditableEntry entry)
    {
        System.Console.WriteLine(_auditJsonSerializer.Serialize(entry));
        return Task.CompletedTask;
    }
}
