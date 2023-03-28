using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auditable.Infrastructure;
using Auditable.Parsing;
using Auditable.Writers;

namespace Auditable.Tests
{
    public class TestWriter : IWriter
    {
        public TestWriter()
        {
            Entries = new List<LogEntry>();
        }

        public List<LogEntry> Entries { get; protected set; }

        public LogEntry First => Entries.FirstOrDefault();

        public Task Write(string id, string action, AuditableEntry entry)
        {
            Entries.Add(new LogEntry(new JsonSerializer().Serialize(entry)));
            return Task.CompletedTask;
        }
    }
}
