using System;

namespace Auditable.Collectors.EntityId;

public class NoIdValueException : Exception
{
    public NoIdValueException(Type entityType, string idAttributeName) : base(
        $"{entityType.Name}.{idAttributeName} the id property had no value")
    {
        EntityType = entityType;
        IdAttributeName = idAttributeName;
    }

    public Type EntityType { get; }
    public string IdAttributeName { get; }
}