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
        [HttpPost]
        [Route("api/Friends/SetFriendRequestAccept/{id},{ok}")]
        public Boolean SetFriendRequestAccept(int id, Boolean ok)
        {
            var db = new DbFriends();
            return db.SetFriendRequestAccept(id, ok);
        }
        [HttpGet]
        [Route("api/Friends/FriendsToUser/{id}")]
        public List<FriendsClass> FriendsToUser(int id)
        {
            var db = new DbFriends();
            return db.FriendsToUser(id);
        }
        [HttpGet]
        [Route("api/Friends/FindFriend/{name}")]
        public FriendsClass FindFriend(String name)
        {
            var db = new DbFriends();
            return db.FindFriend(name);
        }
        [HttpPost]
        [Route("api/Friends/SendFriendRequest/{userId},{friendId}")]
        public Boolean SendFriendRequest(String userId, String friendId)
        {
            var db = new DbFriends();
            return db.SendFriendRequest(userId, friendId);
        }
    }
}
