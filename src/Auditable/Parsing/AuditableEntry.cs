using System;
using System.Collections.Generic;
using Auditable.Collectors.Initiator;
using Auditable.Collectors.Request;
using Environment = Auditable.Collectors.Environment.Environment;

namespace Auditable.Parsing;

public class AuditableEntry
{
    public string Action { get; set; }
    public DateTime DateTime { get; set; }
    public Initiator Initiator { get; set; }
    public Environment Environment { get; set; }
    public RequestContext Request { get; set; }
    public IEnumerable<AuditableTarget> Targets { get; set; }
    public string Id { get; set; }
}