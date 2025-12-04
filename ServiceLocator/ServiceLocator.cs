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
            var assembly = typeof(T).Assembly;
            var serviceTypes = assembly.GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract)
                .Select(type => new
                {
                    Type = type,
                    Attributes = type.GetCustomAttributes<Service>(false).ToArray()
                })
                .Where(x => x.Attributes.Length > 0)
                .SelectMany(x => x.Attributes.Select(attr => new { x.Type, Attribute = attr }));

            foreach (var serviceInfo in serviceTypes)
            {
                var serviceType = serviceInfo.Type;
                var attribute = serviceInfo.Attribute;

                if (attribute.Key == null)
                {
                    // Non-keyed service: register as self and all interfaces
                    RegisterNonKeyedServiceWithInterfaces(services, serviceType, attribute.Lifetime);
                }
                else
                {
                    // Keyed service: register with key + enumeration support
                    var interfaces = serviceType.GetInterfaces();

                    // Register as self with key
                    RegisterKeyedService(services, serviceType, serviceType, attribute.Key, attribute.Lifetime);

                    // Register as interfaces with key
                    foreach (var interfaceType in interfaces)
                    {
                        RegisterKeyedService(services, interfaceType, serviceType, attribute.Key, attribute.Lifetime);
                        
                        // Also register as non-keyed for enumeration support
                        RegisterNonKeyedService(services, interfaceType, serviceType, attribute.Lifetime);
                    }
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

        private static void RegisterNonKeyedServiceWithInterfaces(
            IServiceCollection services,
            Type serviceType,
            ServiceLifetime lifetime)
        {
            // Register as self (concrete type) - use TryAdd for first-wins behavior
            RegisterService(services, serviceType, serviceType, lifetime);
            
            // Register as all implemented interfaces - use TryAddEnumerable for multiple implementations
            foreach (var interfaceType in serviceType.GetInterfaces())
            {
                var descriptor = new ServiceDescriptor(interfaceType, serviceType, lifetime);
                services.TryAddEnumerable(descriptor);
            }
        }

        private static void RegisterService(
            IServiceCollection services,
            Type serviceType,
            Type implementationType,
            ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Scoped:
                    services.TryAddScoped(serviceType, implementationType);
                    break;
                case ServiceLifetime.Singleton:
                    services.TryAddSingleton(serviceType, implementationType);
                    break;
                case ServiceLifetime.Transient:
                    services.TryAddTransient(serviceType, implementationType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, "Invalid service lifetime");
            }
        }
    }
}
