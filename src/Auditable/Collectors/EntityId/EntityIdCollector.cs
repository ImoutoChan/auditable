using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Auditable.Collectors.EntityId;

internal class EntityIdCollector : IEntityIdCollector
{
    private static readonly ConcurrentDictionary<Type, Func<object, string?>> IdExtractorCache = new();
    
    public string Extract<T>(T instance) where T : notnull
    {
        var type = typeof(T);

        var extractor = IdExtractorCache.GetOrAdd(type, inst =>
        {
            var fields = GetAllFields(GetAllTypes(type));

            var idFieldPatterns = new[]
            {
                "_id",
                GetPropertyBackingFieldName("Id"),
                GetPropertyBackingFieldName($"{type.Name}Id"),
                "id",
                "Id"
            }.ToList();

            var idField = fields.FirstOrDefault(x => idFieldPatterns.Contains(x.Name));

            if (idField == null) 
                throw new NoIdAttributeException(type);

            return i => idField.GetValue(i)?.ToString();
        });

        return extractor.Invoke(instance) ?? throw new NoIdValueException(type);
    }

    private static IEnumerable<FieldInfo> GetAllFields(IEnumerable<Type> typeTree) 
        => typeTree.SelectMany(x => x.GetTypeInfo().DeclaredFields);

    private static IEnumerable<Type> GetAllTypes(Type currentType)
    {
        if (currentType.BaseType != null)
            foreach (var type in GetAllTypes(currentType.BaseType))
                yield return type;

        yield return currentType;
    }

    private static string GetPropertyBackingFieldName(string propertyName) 
        => $"<{propertyName}>k__BackingField";
}
