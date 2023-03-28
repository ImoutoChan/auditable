using System.Threading.Tasks;
using Auditable.Infrastructure;
using Auditable.Parsing;

namespace Auditable.Writers.Console;

public class ConsoleWriter : IWriter
{
    private readonly JsonSerializer _jsonSerializer;

    public ConsoleWriter(JsonSerializer jsonSerializer)
    {
        _jsonSerializer = jsonSerializer;
    }

    public Task Write(string id, string action, AuditableEntry entry)
    {
        System.Console.WriteLine(_jsonSerializer.Serialize(entry));
        return Task.CompletedTask;
    }
}
