using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using WebApplication1.Models.Class;

namespace WebApplication1.Controllers.Api
{
    public class ConcertController : ApiController
    {
        [HttpGet]
        [Route("api/Concert/FindAllConcert/")]
        public List<Concert> FindAllConcert()
        {
            var db = new DbConcert();
            return db.FindAllConcert();
        }
        [HttpGet]
        [Route("api/Concert/FindAllConcertToUser/{id}")]
        public List<ConcertClass> FindAllConcertToUser(int id)
        {
            var db = new DbConcert();
            return db.FindAllConcertToUser(id);
        }
        [HttpGet]
        [Route("api/Concert/FriendsGoingToConcert/{id}")]
        public List<User> FriendsGoingToConcert(int id)
        {
            var db = new DbConcert();
            return db.FriendsGoingToConcert(id);
            
        }
        [HttpGet]
        [Route("api/Concert/FriendsGoingToConcertNumber/{id}")]
        public int FriendsGoingToConcertNumber(int id)
        {
            var db = new DbConcert();
            return db.FriendsGoingToConcertNumber(id);
            
        }
    }
}
