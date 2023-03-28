using System.Threading.Tasks;

namespace Auditable.Collectors.Request;

/// <summary>
///     grab information about the request (tracing)
/// </summary>
public interface IRequestContextCollector
{
    Task<RequestContext> Extract();
}