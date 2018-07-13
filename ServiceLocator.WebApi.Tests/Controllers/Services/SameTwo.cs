using Microsoft.Extensions.DependencyInjection;
using ServiceLocator.WebApi.Tests.Controllers.Services.interfaces;

namespace ServiceLocator.WebApi.Tests.Controllers.Services
{
    [Service(ServiceLifetime.Scoped)]
    public class SameTwo: ISame
    {
        
    }
}