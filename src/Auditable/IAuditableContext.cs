using System;
using System.Threading.Tasks;

namespace Auditable;

public interface IAuditableContext : IDisposable, IAsyncDisposable
{
    void WatchTargets(params object[] targets);
    void Removed<T>(string id);
    void Read<T>(string id);
    Task WriteLog();
}