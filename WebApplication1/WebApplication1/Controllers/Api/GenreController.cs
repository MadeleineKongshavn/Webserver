
using System;
using System.Collections.Generic;
using System.Web.Http;
using WebApplication1.Models;

namespace WebApplication1.Controllers.Api
{
    public class GenreController : ApiController
    {
        [HttpGet]
        [Route("api/Genre/FindGenre/{name}")]
        public Genre FindGenre(String name)
        {
            var db = new DbGenre();
            return db.FindGenre(name);
        }
        [HttpPost]
        [Route("api/Genre/NewGenre/{name}")]
        public Boolean NewGenre(String name)
        {
            var db = new DbGenre();
            return db.NewGenre(name);
        }
        [HttpGet]
        [Route("api/Genre/FindGenre/{id}")]
        public Genre FindGenre(int id)
        {
            var db = new DbGenre();
            return db.FindGenre(id);
        }

        [HttpGet]
        [Route("api/Genre/AllGenres")]
        public List<Genre> AllGenres()
        {
            var db = new DbGenre();
            return db.AllGenres();
        }

        [HttpGet]
        [Route("api/Genre/Truee")]
        public Boolean Truee()
        {
            return true;
        }

     /*   // GET api/semitone
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/semitone/5 GET, POST
        public string Get(int id)
        {
            return "value";
        }
        // POST api/semitone
        public void Post([FromBody]string value)
        {
        }

        // PUT api/semitone/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/semitone/5
        public void Delete(int id)
        {
        }*/
    }
}
