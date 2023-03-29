using System.Threading.Tasks;

namespace Auditable.Collectors.Request;

/// <summary>
///     Grab information about the request (tracing)
/// </summary>
public interface IRequestContextCollector
{
    Task<RequestContext?> Extract();
}

public record RequestContext(string SpanId, string TraceId, string ParentId);
