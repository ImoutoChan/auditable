using System.Threading.Tasks;

namespace Auditable.Collectors.Request;

public class NullRequestContextCollector : IRequestContextCollector
{
    public Task<RequestContext> Extract()
    {
        return Task.FromResult<RequestContext>(null);
    }
}