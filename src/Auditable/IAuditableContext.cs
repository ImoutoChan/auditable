using System;
using System.Threading.Tasks;

namespace Auditable;

public interface IAuditableContext : IDisposable, IAsyncDisposable
{
    /// <summary>
    /// All changes in observed targets will be audited after auditable context is disposed.
    /// </summary>
    /// <param name="targets"></param>
    void Observe<T>(params T[] targets) where T : notnull;
    
    void Created<T>(T target) where T : notnull;
    
    void Removed<T>(string id) where T : notnull;
    
    void Removed<T>(T target) where T : notnull;
    
    void Read<T>(string id) where T : notnull;
    
    void Read<T>(T target) where T : notnull;
    
    Task Flush();
}
