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
        [Route("api/Friends/AddFriendRequest/{userId},{friendId}")]
        public async Task<Boolean> AddFriendRequest(int userId, int friendId)
        {
            using (var mngr = ManagerFactory.GetFriendManager())
            {
                return await mngr.AddFriendRequest(userId, friendId);
            }
        }
        [HttpPost]
        [Route("api/Friends/SetFriendAccept/{id},{ok}")]
        public async Task<Boolean> SetFriendAccept(int id, Boolean ok)
        {
            using (var mngr = ManagerFactory.GetFriendManager())
            {
                return await mngr.SetFriendAccept(id, ok);
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
        public async Task<FriendsClass> FindFriend(String name, int uid)
        {
            using (var mngr = ManagerFactory.GetFriendManager())
            {
                return await mngr.FindFriend(name, uid);
            }
        }

    }
}
