using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Managers;

namespace WebApplication1.Controllers.Api
{
    public class GenreController : ApiController
    {
        [HttpPost]
        [Route ("api/Genre/AddGenre/{name}")]
        public async Task<Boolean> AddGenre(string name)
        {
            GenreManager mng = new GenreManager();
            return await mng.AddGenre(name);
        }

    }
}
