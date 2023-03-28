using System;

namespace Auditable.Configuration;

public interface ISetupOptions<out T> where T : class, new()
{
    void Setup(Action<T> options);
}