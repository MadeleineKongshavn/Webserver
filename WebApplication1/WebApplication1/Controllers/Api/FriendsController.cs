using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication1.Managers;
using WebApplication1.Models;
using WebApplication1.Models.Db;

namespace WebApplication1.Controllers.Api
{
    public class FriendsController : ApiController
    {
        [HttpPost]
        [Route("api/Friends/SetFriendRequestAccept/{id},{ok}")]
        public bool SetFriendRequestAccept(int id, Boolean ok)
        {
            using (var mngr = ManagerFactory.GetFriendManager())
            {
                return mngr.SetFriendRequestAccept(id,ok);
            }
        }
        [HttpGet]
        [Route("api/Friends/FriendsToUser/{id}")]
        public async Task<List<FriendsClass>> FriendsToUser(int id)
        {
            using (var mngr = ManagerFactory.GetFriendManager())
            {
                return await mngr.GetFriendsFromUserId(id);
            }
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
        public bool SendFriendRequest(String userId, String friendId)
        {
            var db = new DbFriends();
            return db.SendFriendRequest(userId, friendId);
        }
    }
}
