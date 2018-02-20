using System;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceLocator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class Service : Attribute
    {
        public ServiceLifetime Lifetime;

        public Service(ServiceLifetime lifetime)
        {
            Lifetime = lifetime;
        }
    }
}