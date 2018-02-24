# ServiceLocator

Fill service provider with services marked by attributes

## Installation

```
Install-Package ServiceLocator
```

## Usage

#### Add Service locator

<T> where T is a Assembly to scan

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // ****
    services.AddServiceLocator<Program>();
    // ****
}
```

Decorate class with attributes, so that locator can find them and register them in service provider.

1. [Service(ServiceLifetime.Scoped)]
2. [Service(ServiceLifetime.Singleton)]
3. [Service(ServiceLifetime.Transient)]

**Singleton** means only a single instance will ever be created. That instance is shared between all components that require it. The same instance is thus used always.

**Scoped means** an instance is created once per scope. A scope is created on every request to the application, thus any components registered as Scoped will be created once per request.

**Transient** components are created every time they are requested and are never shared.

```csharp
[Service(ServiceLifetime.Scoped)]
internal class ScopedScopedValueService : IScopedValueService
{
}
```
