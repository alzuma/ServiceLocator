using Microsoft.Extensions.DependencyInjection;

namespace ServiceLocator.TestApi.Services.Multiple;

[Service(ServiceLifetime.Scoped)]
public class SameServiceTwo : ISameService
{
    public string GetName() => "SameServiceTwo";
}
