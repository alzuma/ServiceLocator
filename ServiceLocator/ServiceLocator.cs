using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ServiceLocator
{
    public static class ServiceLocator
    {
        public static IServiceCollection AddServiceLocator<T>(this IServiceCollection services)
        {
            // Phase 1: Non-keyed services using Scrutor (existing logic)
            var scanServices = new ServiceCollection();
            scanServices.Scan(scan =>
                scan.FromAssemblyOf<T>()
                    .AddClasses(classes => classes.WithAttribute<Service>(s => s.Lifetime == ServiceLifetime.Scoped && s.Key == null))
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime()
                    .AddClasses(classes => classes.WithAttribute<Service>(s => s.Lifetime == ServiceLifetime.Singleton && s.Key == null))
                    .AsSelfWithInterfaces()
                    .WithSingletonLifetime()
                    .AddClasses(classes => classes.WithAttribute<Service>(s => s.Lifetime == ServiceLifetime.Transient && s.Key == null))
                    .AsSelfWithInterfaces()
                    .WithTransientLifetime()
            );

            foreach (var service in scanServices)
            {
                services.Add(service);
            }

            // Phase 2: Keyed services using manual reflection
            var assembly = typeof(T).Assembly;
            var keyedServiceTypes = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract)
                .Select(type => new
                {
                    Type = type,
                    Attributes = type.GetCustomAttributes<Service>(false).ToArray()
                })
                .Where(x => x.Attributes.Length > 0)
                .SelectMany(x => x.Attributes.Select(attr => new { x.Type, Attribute = attr }))
                .Where(x => x.Attribute.Key != null);

            foreach (var serviceInfo in keyedServiceTypes)
            {
                var serviceType = serviceInfo.Type;
                var attribute = serviceInfo.Attribute;
                var interfaces = serviceType.GetInterfaces();

                // Register as self with key
                RegisterKeyedService(services, serviceType, serviceType, attribute.Key!, attribute.Lifetime);

                // Register as interfaces with key
                foreach (var interfaceType in interfaces)
                {
                    RegisterKeyedService(services, interfaceType, serviceType, attribute.Key!, attribute.Lifetime);
                    
                    // Also register as non-keyed for enumeration support
                    RegisterNonKeyedService(services, interfaceType, serviceType, attribute.Lifetime);
                }
            }

            return services;
        }

        private static void RegisterKeyedService(
            IServiceCollection services,
            Type serviceType,
            Type implementationType,
            object serviceKey,
            ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    services.AddKeyedScoped(serviceType, serviceKey, implementationType);
                    break;
                case ServiceLifetime.Singleton:
                    services.AddKeyedSingleton(serviceType, serviceKey, implementationType);
                    break;
                case ServiceLifetime.Transient:
                    services.AddKeyedTransient(serviceType, serviceKey, implementationType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, "Invalid service lifetime");
            }
        }

        private static void RegisterNonKeyedService(
            IServiceCollection services,
            Type serviceType,
            Type implementationType,
            ServiceLifetime lifetime)
        {
            var descriptor = new ServiceDescriptor(serviceType, implementationType, lifetime);
            services.TryAddEnumerable(descriptor);
        }
    }
}
