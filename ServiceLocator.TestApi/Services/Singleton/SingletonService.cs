using Microsoft.Extensions.DependencyInjection;

namespace ServiceLocator.TestApi.Services.Singleton;

[Service(ServiceLifetime.Singleton)]
public class SingletonService : ISingletonService
{
    private readonly List<string> _values = new();
    
    public void AddValue(string value) => _values.Add(value);
    public List<string> GetValues() => _values;
}
