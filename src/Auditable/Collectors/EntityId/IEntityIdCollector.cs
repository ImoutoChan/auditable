namespace Auditable.Collectors.EntityId;

public interface IEntityIdCollector
{
    string Extract<T>(T instance) where T : notnull;
}
