using System;
using System.Threading.Tasks;

namespace Auditable;

public interface IAuditableContext : IDisposable, IAsyncDisposable
{
    void WatchTargets(params object[] targets);
    
    void Created<T>(T target);
    
    void Removed<T>(string id);
    
    void Removed<T>(T target);
    
    void Read<T>(string id);
    
    void Read<T>(T target);
    
    Task WriteLog();
}
