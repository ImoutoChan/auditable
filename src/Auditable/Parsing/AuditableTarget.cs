using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace Auditable.Parsing;

public class AuditableTarget
{
    public required string Id { get; set; }

    public required string Type { get; set; }

    /// <summary>
    /// Can be null when type is read or removed.
    /// </summary>
    public JToken? Delta { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public ActionStyle Style { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public AuditType Audit { get; set; }
}
