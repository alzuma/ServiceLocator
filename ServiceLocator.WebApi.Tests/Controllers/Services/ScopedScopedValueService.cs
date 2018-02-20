using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using ServiceLocator.WebApi.Tests.Controllers.Services.interfaces;

namespace ServiceLocator.WebApi.Tests.Controllers.Services
{
    [Service(ServiceLifetime.Scoped)]
    internal class ScopedScopedValueService : IScopedValueService
    {
        private int _counter;

        public List<string> GetValues()
        {
            _counter++;

            var result = new List<string>();

            for (var i = 1; i <= _counter; i++)
            {
                result.Add($"value{i}");
            }

            return result;
        }
    }
}