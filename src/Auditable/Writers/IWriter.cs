using System.Threading.Tasks;
using Auditable.Parsing;

namespace Auditable.Writers;

public interface IWriter
{
    Task Write(string id, string action, AuditableEntry entry);
}
