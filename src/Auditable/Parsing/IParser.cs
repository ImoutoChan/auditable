namespace Auditable.Parsing;

using System.Collections.Generic;
using System.Threading.Tasks;

public interface IParser
{
    Task<AuditableEntry> Parse(string id, string actionName, IEnumerable<Target> targets);
}
