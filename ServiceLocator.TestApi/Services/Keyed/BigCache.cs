using Microsoft.Extensions.DependencyInjection;

namespace ServiceLocator.TestApi.Services.Keyed;

[Service(ServiceLifetime.Singleton, Key = "big")]
public class BigCache : ICache
{
    public string Get(string key) => $"Big cache: {key}";
}
