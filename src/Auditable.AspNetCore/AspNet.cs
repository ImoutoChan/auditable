using Auditable.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Auditable.AspNetCore;

/// <summary>
///     this is recommended way to register the ASPNET dependencies.
/// </summary>
public class AspNet : IExtension
{
    public void RegisterServices(IServiceCollection services)
    {
        services.AddAuditableForAspNetOnly();
    }
}