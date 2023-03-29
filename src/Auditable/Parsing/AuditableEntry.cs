using System;
using System.Collections.Generic;
using Auditable.Collectors.Initiator;
using Auditable.Collectors.Request;
using Environment = Auditable.Collectors.Environment.Environment;

namespace Auditable.Parsing;

public class AuditableEntry
{
    public required string Id { get; set; }

    public required string Action { get; set; }

    public required DateTimeOffset DateTime { get; set; }

    public Initiator? Initiator { get; set; }

    public required Environment Environment { get; set; }

    public RequestContext? Request { get; set; }

    public required IEnumerable<AuditableTarget> Targets { get; set; }
}
