using Microsoft.Extensions.DependencyInjection;

namespace ServiceLocator.TestApi.Services.Transient;

[Service(ServiceLifetime.Transient)]
public class TransientService : ITransientService
{
    private readonly List<string> _values = new();
    
    public void AddValue(string value) => _values.Add(value);
    public List<string> GetValues() => _values;
}
