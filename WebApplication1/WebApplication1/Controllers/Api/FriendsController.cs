using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using WebApplication1.Models.Db;

namespace WebApplication1.Controllers.Api
{
    public class FriendsController : ApiController
    {
        [HttpGet]
        [Route("api/Friends/FriendsToUser/{id}")]
        public List<FriendsClass> FriendsToUser(int id)
        {
            DbFriends db = new DbFriends();
            return db.FriendsToUser(id);
        }

        /*
        // GET api/friends
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/friends/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/friends
        public void Post([FromBody]string value)
        {
        }

        // PUT api/friends/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/friends/5
        public void Delete(int id)
        {
        }*/
    }
}
