using Auditable.AspNetCore.Collectors;
using Auditable.AspNetCore.Middleware;
using Auditable.Collectors.Initiator;
using Auditable.Collectors.Request;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Auditable.AspNetCore;

public static class SetupAuditable
{
    /// <summary>
    ///     This sets up the ASPNET dependencies only, you will have to setup the AddAuditable
    ///     Consider using the ConfigureAuditable extension on the <seealso cref="IHostBuilder" />
    /// </summary>
    public static IServiceCollection AddAuditableForAspNetOnly(this IServiceCollection services)
    {
        //override or add ASP.NET components
        //collectors
        services.AddScoped<IInitiatorCollector, ClaimsInitiatorCollector>();
        services.AddScoped<IRequestContextCollector, RequestContextCollector>();

        return services;
    }

    /// <summary>
    ///     This sets up the Auditable with ASPNET dependencies
    ///     Consider using the ConfigureAuditable extension on the <seealso cref="IHostBuilder" />
    /// </summary>
    public static IServiceCollection AddAuditableForAspNet(this IServiceCollection services)
    {
        //setup the base auditbale
        Configuration.SetupAuditable.AddAuditable(services);

        //override or add ASP.NET components
        //collectors
        services.AddAuditableForAspNetOnly();

        return services;
    }

    /// <summary>
    ///     applies the required middleware for auditble to grab the correct information
    /// </summary>
    public static IApplicationBuilder UseAuditable(this IApplicationBuilder app)
    {
        app.UseMiddleware<RequestMiddleware>();
        app.UseMiddleware<InitiatorMiddleware>();
        return app;
    }
}