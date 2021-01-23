using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using WebApplicationTest.Controllers.Services.interfaces;

namespace WebApplicationTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        public ValuesController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            var scopedValueService = _serviceProvider.GetRequiredService<IScopedValueService>();
            scopedValueService.GetValues();
            var scopedValueService2 = _serviceProvider.GetRequiredService<IScopedValueService>();
            return scopedValueService2.GetValues();
        }

        [HttpGet("singleton")]
        public IEnumerable<string> GetSingleton()
        {
            var singletonValueService = _serviceProvider.GetRequiredService<ISingletonValueService>();
            singletonValueService.GetValues();
            var singletonValueService2 = _serviceProvider.GetRequiredService<ISingletonValueService>();
            return singletonValueService2.GetValues();
        }

        [HttpGet("transient")]
        public IEnumerable<string> GetTransient()
        {
            var transientValueService = _serviceProvider.GetRequiredService<ITransientValueService>();
            transientValueService.GetValues();
            var transientValueService2 = _serviceProvider.GetRequiredService<ITransientValueService>();
            return transientValueService2.GetValues();
        }
    }
}
