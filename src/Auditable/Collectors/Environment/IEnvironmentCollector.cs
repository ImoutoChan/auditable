using System.Threading.Tasks;

namespace Auditable.Collectors.Environment;

/// <summary>
///     Grab information about the running environment
/// </summary>
public interface IEnvironmentCollector
{
    Task<Environment> Extract();
}

public record Environment(string Host, string Application);
