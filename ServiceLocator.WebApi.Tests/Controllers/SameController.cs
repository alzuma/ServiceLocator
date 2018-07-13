using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ServiceLocator.WebApi.Tests.Controllers.Services.interfaces;

namespace ServiceLocator.WebApi.Tests.Controllers
{
    [Route("api/[controller]")]
    public class SameController: Controller
    {
        private readonly List<ISame> _sames;

        public SameController(IEnumerable<ISame> sames)
        {
            _sames = sames.ToList();
        }

        [HttpGet]
        public int Get()
        {
            return _sames.Count;
        }
    }
}