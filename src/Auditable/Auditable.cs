using System;

namespace Auditable;

internal class Auditable : IAuditable
{
    private readonly Func<IInternalAuditableContext> _contextCtor;

    public Auditable(Func<IInternalAuditableContext> contextCtor) => _contextCtor = contextCtor;

    public IAuditableContext CreateContext(string name, params object[] targets)
    {
        var context = _contextCtor();
        context.SetName(name);
        context.Observe(targets);
        return context;
    }
}
