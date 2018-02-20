using System.Collections.Generic;

namespace ServiceLocator.WebApi.Tests.Controllers.Services.interfaces
{
    public interface ITransientValueService
    {
        List<string> GetValues();
    }
}