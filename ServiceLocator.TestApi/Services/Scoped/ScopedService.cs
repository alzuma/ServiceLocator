using Microsoft.Extensions.DependencyInjection;

namespace ServiceLocator.TestApi.Services.Scoped;

[Service(ServiceLifetime.Scoped)]
public class ScopedService : IScopedService
{
    private readonly List<string> _values = new();
    
    public void AddValue(string value) => _values.Add(value);
    public List<string> GetValues() => _values;
}
