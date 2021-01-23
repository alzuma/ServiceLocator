using Microsoft.Extensions.DependencyInjection;
using ServiceLocator;
using WebApplicationTest.Controllers.Services.interfaces;

namespace WebApplicationTest.Controllers.Services
{
    [Service(ServiceLifetime.Scoped)]
    public class SameTwo: ISame
    {
        
    }
}