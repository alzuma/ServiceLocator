using System.Collections.Generic;

namespace ServiceLocator.WebApi.Tests.Controllers.Services.interfaces
{
    public interface ISingletonValueService
    {
        List<string> GetValues();
    }
}