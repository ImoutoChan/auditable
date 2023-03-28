using System.Threading.Tasks;
using Auditable.Collectors.Initiator;

namespace Auditable.AspNetCore.Collectors;

internal class ClaimsInitiatorCollector : IInitiatorCollector
{
    public Initiator Initiator { get; set; }

    public Task<Initiator> Extract()
    {
        return Task.FromResult(Initiator);
    }
}