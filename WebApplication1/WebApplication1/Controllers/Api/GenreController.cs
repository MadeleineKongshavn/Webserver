using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading.Tasks;
using WebApplication1.Models.Class;
using WebApplication1.Managers;
using WebApplication1.Models.Args;
namespace WebApplication1.Controllers.Api
{
    public class GenreController : ApiController
    {
        [HttpGet]
        [Route("api/Genre/GetAll")]
        public async Task<List<GenreClass>> GetAllGenres()
        {
            GenreManager mng = new GenreManager();
            return await mng.GetGenres();
        }
    }    
}
