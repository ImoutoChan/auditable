using System;
using Auditable.Collectors.EntityId;
using Auditable.Collectors.Environment;
using Auditable.Collectors.Initiator;
using Auditable.Collectors.Request;
using Auditable.Delta;
using Auditable.Infrastructure;
using Auditable.Parsing;
using Auditable.Writers;
using Auditable.Writers.Console;
using Microsoft.Extensions.DependencyInjection;

namespace Auditable.Configuration;

public static class ServiceCollectionExtensions
{
    /// <summary>
    ///     Setup Auditable with default setup.
    /// </summary>
    public static IServiceCollection AddAuditable(this IServiceCollection services)
    {
        //core
        services.AddTransient<IInternalAuditableContext, AuditableContext>();
        services.AddTransient<Func<IInternalAuditableContext>>(x => x.GetRequiredService<IInternalAuditableContext>);
        services.AddTransient<IAuditable, Auditable>();
        services.AddTransient<IDeltaCalculator, DeltaCalculator>();
        services.AddTransient<IAuditJsonSerializer, AuditJsonSerializer>();

        //collectors
        services.AddScoped<IInitiatorCollector, PrincipalInitiatorCollector>();
        services.AddScoped<IRequestContextCollector, NullRequestContextCollector>();
        services.AddTransient<IEnvironmentCollector, HostEnvironmentCollector>();
        services.AddTransient<IEntityIdCollector, EntityIdCollector>();

        //parse and write
        services.AddTransient<IParser, DefaultParser>();
        services.AddSingleton<IWriter, ConsoleWriter>();


        return services;
    }
}
