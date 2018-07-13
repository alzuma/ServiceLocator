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
                    .AsImplementedInterfaces()
                    .WithScopedLifetime()
                    .AddClasses(classes => classes.WithAttribute<Service>(s => s.Lifetime == ServiceLifetime.Singleton))
                    .AsImplementedInterfaces()
                    .WithSingletonLifetime()
                    .AddClasses(classes => classes.WithAttribute<Service>(s => s.Lifetime == ServiceLifetime.Transient))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
            );

            services.Add(scanServices);
        }
    }
}
