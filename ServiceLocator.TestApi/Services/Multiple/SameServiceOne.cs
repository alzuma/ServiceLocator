using Microsoft.Extensions.DependencyInjection;

namespace ServiceLocator.TestApi.Services.Multiple;

[Service(ServiceLifetime.Scoped)]
public class SameServiceOne : ISameService
{
    public string GetName() => "SameServiceOne";
}
