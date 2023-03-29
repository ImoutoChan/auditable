using System.Threading.Tasks;

namespace Auditable.Collectors.Request;

internal class NullRequestContextCollector : IRequestContextCollector
{
    public Task<RequestContext?> Extract() => Task.FromResult<RequestContext?>(null);
}
