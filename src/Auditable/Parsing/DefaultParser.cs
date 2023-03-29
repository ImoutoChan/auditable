using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auditable.Collectors.EntityId;
using Auditable.Collectors.Environment;
using Auditable.Collectors.Initiator;
using Auditable.Collectors.Request;

namespace Auditable.Parsing;

internal class DefaultParser : IParser
{
    private readonly IEntityIdCollector _entityIdCollector;
    private readonly IEnvironmentCollector _environmentCollector;
    private readonly IInitiatorCollector _initiatorCollector;
    private readonly IRequestContextCollector _requestContextCollector;

    public DefaultParser(
        IInitiatorCollector initiatorCollector,
        IEnvironmentCollector environmentCollector,
        IRequestContextCollector requestContextCollector,
        IEntityIdCollector entityIdCollector)
    {
        _initiatorCollector = initiatorCollector;
        _environmentCollector = environmentCollector;
        _requestContextCollector = requestContextCollector;
        _entityIdCollector = entityIdCollector;
    }

    public async Task<AuditableEntry> Parse(string id, string actionName, IEnumerable<Target> targets)
    {
        var payload = new AuditableEntry
        {
            Id = id,
            Action = actionName,
            DateTime = DateTimeOffset.Now,

            Environment = await _environmentCollector.Extract(),
            Initiator = await _initiatorCollector.Extract(),
            Request = await _requestContextCollector.Extract(),
            Targets = targets.Select(x => new AuditableTarget
            {
                Delta = x.Delta,
                Id = x.Id,
                Type = x.Type,
                Style = x.ActionStyle,
                Audit = x.ActionStyle == ActionStyle.Explicit
                    ? x.AuditType
                    : x.Delta == null
                        ? AuditType.Read
                        : AuditType.Modified
            })
        };

        return payload;
    }
}
