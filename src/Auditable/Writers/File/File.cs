using System;
using Auditable.Configuration;
using Auditable.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Auditable.Writers.File;

public class File : IWriterProvider, ISetupOptions<FileWriterOptions>
{
    private Action<FileWriterOptions> _options = options => { };

    public void Setup(Action<FileWriterOptions> options)
    {
        Code.Require(() => options != null, nameof(options));
        _options = options;
    }

    public void RegisterServices(IServiceCollection services)
    {
        Code.Require(() => services != null, nameof(services));
        services.AddSingleton<IWriter, FileWriter>();
        services.Configure(_options);
    }
}