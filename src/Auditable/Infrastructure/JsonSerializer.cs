using System;
using Newtonsoft.Json;

namespace Auditable.Infrastructure;

internal class AuditJsonSerializer : IAuditJsonSerializer
{
    private readonly Func<object, string> _serializer = JsonConvert.SerializeObject;
    
    public string Serialize<T>(T instance) where T : notnull => _serializer(instance);
}

public interface IAuditJsonSerializer
{
    string Serialize<T>(T instance) where T : notnull;
}
