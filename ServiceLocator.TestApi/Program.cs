using Microsoft.AspNetCore.Mvc;
using ServiceLocator;
using ServiceLocator.TestApi.Services.Keyed;
using ServiceLocator.TestApi.Services.Multiple;
using ServiceLocator.TestApi.Services.Scoped;
using ServiceLocator.TestApi.Services.Singleton;
using ServiceLocator.TestApi.Services.Transient;

var builder = WebApplication.CreateBuilder(args);

// Auto-register all services with [Service] attribute
builder.Services.AddServiceLocator<Program>();

var app = builder.Build();

// Test endpoint for scoped services
app.MapGet("/api/scoped", (IServiceProvider sp) =>
{
    var service1 = sp.GetRequiredService<IScopedService>();
    service1.AddValue("value1");
    var service2 = sp.GetRequiredService<IScopedService>();
    service2.AddValue("value2");
    return Results.Ok(service2.GetValues());
});

// Test endpoint for scoped services by concrete type
app.MapGet("/api/scoped/bytype", (ScopedService service) =>
{
    service.AddValue("value1");
    return Results.Ok(service.GetValues());
});

// Test endpoint for singleton services
app.MapGet("/api/singleton", (IServiceProvider sp) =>
{
    var service1 = sp.GetRequiredService<ISingletonService>();
    service1.AddValue("value1");
    var service2 = sp.GetRequiredService<ISingletonService>();
    service2.AddValue("value2");
    return Results.Ok(service2.GetValues());
});

// Test endpoint for transient services
app.MapGet("/api/transient", (IServiceProvider sp) =>
{
    var service1 = sp.GetRequiredService<ITransientService>();
    service1.AddValue("value1");
    var service2 = sp.GetRequiredService<ITransientService>();
    return Results.Ok(service2.GetValues());
});

// Test endpoint for multiple implementations
app.MapGet("/api/same", (IEnumerable<ISameService> services) =>
{
    return Results.Ok(services.Count());
});

// Test endpoint for keyed services - big cache
app.MapGet("/api/keyed/big", ([FromKeyedServices("big")] ICache cache) =>
{
    return Results.Ok(cache.Get("test"));
});

// Test endpoint for keyed services - small cache
app.MapGet("/api/keyed/small", ([FromKeyedServices("small")] ICache cache) =>
{
    return Results.Ok(cache.Get("test"));
});

// Test endpoint for all caches (including keyed)
app.MapGet("/api/keyed/all", (IEnumerable<ICache> caches) =>
{
    return Results.Ok(caches.Select(c => c.Get("test")));
});

app.Run();

// Make Program accessible for testing
namespace ServiceLocator.TestApi
{
    public partial class Program { }
}
