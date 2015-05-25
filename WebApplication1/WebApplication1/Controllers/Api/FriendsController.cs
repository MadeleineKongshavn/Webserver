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
        [HttpGet]
        [Route("api/Friends/GetList/{userid}")]
        public async Task<List<FriendsClass>> FriendsToUser(int userid)
        {
            using (var mngr = ManagerFactory.GetFriendManager())
            {
                return await mngr.GetFriendsFromUserId(userid);
            }
        }
        [HttpGet]
        [Route("api/Friends/Query/{name},{userid}")]
        public async Task<FriendsClass> FindFriend(String name, int userid)
        {
            using (var mngr = ManagerFactory.GetFriendManager())
            {
                return await mngr.FindFriend(name, userid);
            }
        }
        [HttpPost]
        [Route("api/Friends/SendRequest/{userId},{friendId}")]
        public async Task<Boolean> AddFriendRequest(int userId, int friendId)
        {
            using (var mngr = ManagerFactory.GetFriendManager())
            {
                return await mngr.AddFriendRequest(userId, friendId);
            }
        }
        [HttpPost]
        [Route("api/Friends/Respond/{requestId},{ok}")]
        public async Task<Boolean> SetFriendAccept(int requestId, Boolean ok)
        {
            using (var mngr = ManagerFactory.GetFriendManager())
            {
                return await mngr.SetFriendAccept(requestId, ok);
            }
        }
        [HttpPost]
        [Route("api/Friends/Remove/{uid},{friendId}")]
        public async Task<bool> Remove(int uid, int friendId)
        {
            using (var mngr = ManagerFactory.GetFriendManager())
            {
                return await mngr.CancelFriendTask(friendId, uid);
            }
        }
    }
}
