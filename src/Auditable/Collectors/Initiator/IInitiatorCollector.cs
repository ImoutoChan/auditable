using System.Threading.Tasks;

namespace Auditable.Collectors.Initiator;

/// <summary>
///     Grab information about the user who is acting the action
/// </summary>
public interface IInitiatorCollector
{
    Task<Initiator> Extract();
}