using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Threading.Tasks;
using WebApplication1.Models.Class;
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

        [HttpPost]
        [Route("api/Genre/GetGenreById/{id}")]
        public async Task<GenreClass> GetGenreById(int id)
        {
            GenreManager mng = new GenreManager();
            return await mng.GetGenreById(id);
        }

        [HttpPost]
        [Route("api/Genre/GetGenreByName/{name}")]
        public async Task<GenreClass> GetGenreByName(string name)
        {
            GenreManager mng = new GenreManager();
            return await mng.GetGenreByName(name);
        }

        [HttpPost]
        [Route("api/Genre/GetAllGenres")]
        public async Task<List<GenreClass>> GetAllGenres()
        {
            GenreManager mng = new GenreManager();
            return await mng.GetGenres();
        }


    }
}
