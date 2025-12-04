using Microsoft.Extensions.DependencyInjection;

namespace ServiceLocator.TestApi.Services.Keyed;

[Service(ServiceLifetime.Singleton, Key = "small")]
public class SmallCache : ICache
{
    public string Get(string key) => $"Small cache: {key}";
}
