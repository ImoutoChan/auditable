using System.Threading.Tasks;
using Auditable.Collectors.Request;

namespace Auditable.AspNetCore.Collectors;

internal class RequestContextCollector : IRequestContextCollector
{
    public RequestContext RequestContext { get; set; }

    public Task<RequestContext> Extract()
    {
        return Task.FromResult(RequestContext);
    }
}