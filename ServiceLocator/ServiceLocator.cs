using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ServiceLocator
{
    public static class ServiceLocator
    {
        public static void AddServiceLocator<T>(this IServiceCollection services)
        {
            var scanServices = new ServiceCollection();

            scanServices.Scan(scan =>
                scan.FromAssemblyOf<T>()
                    .AddClasses(classes => classes.WithAttribute<Service>(s => s.Lifetime == ServiceLifetime.Scoped))
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime()
                    .AddClasses(classes => classes.WithAttribute<Service>(s => s.Lifetime == ServiceLifetime.Singleton))
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime()
                    .AddClasses(classes => classes.WithAttribute<Service>(s => s.Lifetime == ServiceLifetime.Transient))
                    .AsSelfWithInterfaces()
                    .WithTransientLifetime()
            );

            services.Add(scanServices);
        }
    }
}
