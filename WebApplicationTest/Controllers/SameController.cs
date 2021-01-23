using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebApplicationTest.Controllers.Services.interfaces;

namespace WebApplicationTest.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SameController: ControllerBase
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