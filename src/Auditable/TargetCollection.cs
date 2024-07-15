using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Auditable.Collectors.EntityId;

namespace Auditable;

internal class TargetCollection : IEnumerable<Target>
{
    private readonly IEntityIdCollector _idCollector;
    private readonly Dictionary<string, Target> _idLookup = new();

    public TargetCollection(IEntityIdCollector idCollector) => _idCollector = idCollector;

    public IEnumerator<Target> GetEnumerator() => _idLookup.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public bool TryGet(Type type, string id, [NotNullWhen(true)] out Target? target)
    {
        var key = GetKey(type, id);
        return _idLookup.TryGetValue(key, out target);
    }

    public void Add(Type type, string id, Target target)
    {
        var key = GetKey(type, id);
        _idLookup.TryAdd(key, target);
    }


    public void Add<T>(T instance, Target target) where T : notnull
    {
        var id = _idCollector.Extract(instance);
        Add(typeof(T), id, target);
    }
    
    public void Clear() => _idLookup.Clear();
    
    public int Count => _idLookup.Count;

    private static string GetKey(Type type, string id) => $"{type.FullName}-{id}";
}
