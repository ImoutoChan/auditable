using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Auditable.Collectors.Environment;

internal class HostEnvironmentCollector : IEnvironmentCollector
{
    private readonly IHostEnvironment _hostEnvironment;

    public HostEnvironmentCollector(IHostEnvironment hostEnvironment) 
        => _hostEnvironment = hostEnvironment;

    public Task<Environment> Extract()
    {
        var application = _hostEnvironment.ApplicationName;
        var environment = _hostEnvironment.EnvironmentName;

        return Task.FromResult(new Environment(application, environment));
    }
}
