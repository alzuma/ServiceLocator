using System.Collections.Generic;

namespace ServiceLocator.WebApi.Tests.Controllers.Services.interfaces
{
    public interface IScopedValueService
    {
        List<string> GetValues();
    }
}