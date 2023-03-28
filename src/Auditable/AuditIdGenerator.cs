using CSharpVitamins;

namespace Auditable;

internal class AuditIdGenerator : IAuditIdGenerator
{
    public string GenerateId()
    {
        return ShortGuid.NewGuid();
    }
}