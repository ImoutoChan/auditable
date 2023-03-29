using System.Threading.Tasks;
using Auditable.Infrastructure;
using Auditable.Parsing;

namespace Auditable.Writers.Console;

internal class ConsoleWriter : IWriter
{
    public Task Write(string auditId, string action, AuditableEntry entry)
    {
        System.Console.WriteLine(AuditJsonSerializer.Serialize(entry));
        return Task.CompletedTask;
    }
}
