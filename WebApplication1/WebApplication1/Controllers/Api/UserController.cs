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
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("api/User/GetAllNotifications/{id}")]
        public List<NotificationsClass> GetAllNotifications(int id)
        {
            var db = new DbUser();
            return db.GetAllNotifications(id);
        }
        [HttpGet]
        [Route("api/User/GetUser/{id}")]
        public UserClass GetUser(int id)
        {
            var db = new DbUser();
            return db.GetUser(id);
        }
        [HttpGet]
        [Route("api/User/GetName/{id}")]
        public String GetName(int id)
        {
            var db = new DbUser();
            return db.GetName(id);
        }
        [HttpGet]
        [Route("api/User/GetNoticiations/{id}")]
        public Boolean GetNoticiations(int id) // ???
        {
            var db = new DbUser();
            return db.GetNoticiations(id);
        }
        [HttpGet]
        [Route("api/User/GetPublic/{id}")]
        public Boolean GetPublic(int id)
        {
            var db = new DbUser();
            return db.GetPublic(id);
        }
        [HttpGet]
        [Route("api/User/GetArea/{id}")]
        public List<Double> GetArea(int id)
        {
            var db = new DbUser();
            return db.GetArea(id);
        }
        [HttpGet]
        [Route("api/User/GetRadius/{id}")]
        public int GetRadius(int id)
        {
            var db = new DbUser();
            return db.GetRadius(id);
        }
        [HttpPost]
        [Route("api/User/ChangeRadius/{radius, id}")]
        public Boolean ChangeRadius(int radius, int id)
        {
            var db = new DbUser();
            return db.ChangeRadius(radius, id);
        }
        [HttpPost]
        [Route("api/User/ChangeArea/{y, x, id}")]
        public Boolean ChangeArea(int y, int x, int id)
        {
            var db = new DbUser();
            return db.ChangeArea(y, x, id);
        }
        [HttpPost]
        [Route("api/User/ChangePublic/{ok, id}")]
        public Boolean ChangePublic(Boolean ok, int id)
        {
            var db = new DbUser();
            return db.ChangePublic(ok, id);
        }
        [HttpPost]
        [Route("api/User/ChangeNotifications/{ok, id}")]
        public Boolean ChangeNotifications(Boolean ok, int id)
        {
            var db = new DbUser();
            return db.ChangeNotifications(ok, id);
        }
        [HttpPost]
        [Route("api/User/RemoveGenre/{genreId, id}")]
        public Boolean RemoveGenre(int genreId, int id)
        {
            var db = new DbUser();
            return db.RemoveGenre(genreId, id);
        }
        [HttpPost]
        [Route("api/User/AddGenre/{genreId, id}")]
        public Boolean AddGenre(int genreId, int userId)
        {
            var db = new DbUser();
            return db.AddGenre(genreId, userId);
        }
        [HttpPost]
        [Route("api/User/ChangeName/{newName, id}")]
        public Boolean ChangeName(String newName, int id)
        {
            var db = new DbUser();
            return db.ChangeName(newName, id);
        }
    }
}
