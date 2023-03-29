using System;

namespace Auditable.Collectors.EntityId;

public class NoIdValueException : Exception
{
    public NoIdValueException(Type entityType) 
        : base($"{entityType.Name} the id property had no value") 
        => EntityType = entityType;

    public Type EntityType { get; }
}
