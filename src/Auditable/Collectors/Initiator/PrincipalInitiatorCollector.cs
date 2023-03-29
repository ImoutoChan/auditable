using System.Threading;
using System.Threading.Tasks;

namespace Auditable.Collectors.Initiator;

/// <summary>
///     Gets the current Principal, note this will need to be set, otherwise it will be null
/// </summary>
internal class PrincipalInitiatorCollector : IInitiatorCollector
{
    public Task<Initiator?> Extract()
    {
        var principal = Thread.CurrentPrincipal;
        if (principal == null) 
            return Task.FromResult<Initiator?>(null);

        var initiator = new Initiator(principal.Identity?.Name ?? "Unknown", "Unknown");

        return Task.FromResult<Initiator?>(initiator);
    }
}
