using System;

namespace Auditable.Collectors.EntityId;

public class NoIdAttributeException : Exception
{
    public NoIdAttributeException(Type entityType) : base($"{entityType.Name} does not have an ID attribute")
    {
        EntityType = entityType;
    }

    public Type EntityType { get; }
}