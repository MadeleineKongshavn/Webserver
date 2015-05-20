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
        [Route("api/Friends/me")]
        public string test(){
            return "her";
            
        }

        //api/Friends/FriendsToUser/{id} ||endret || sjekk
        [HttpGet]
        [Route("api/Friends/GetFriends/{userid}")]
        public async Task<List<FriendsClass>> FriendsToUser(int userid)
        {
            using (var mngr = ManagerFactory.GetFriendManager())
            {
                return await mngr.GetFriendsFromUserId(userid);
            }
        }

        //api/Friends/FindFriend/{name},{uid} || endret || sjekk
        [HttpGet]
        [Route("api/Friends/Query/{name},{userid}")]
        public async Task<FriendsClass> FindFriend(String name, int uid)
        {
            using (var mngr = ManagerFactory.GetFriendManager())
            {
                return await mngr.FindFriend(name, uid);
            }
        }



        //api/Friends/AddFriendRequest/{userId},{friendId} || endret
        [HttpPost]
        [Route("api/Friends/SendRequest/{userId},{friendId}")]
        public async Task<Boolean> AddFriendRequest(int userId, int friendId)
        {
            using (var mngr = ManagerFactory.GetFriendManager())
            {
                return await mngr.AddFriendRequest(userId, friendId);
            }
        }

        //api/Friends/SetFriendAccept/{id},{ok} ||endret || sjekk
        [HttpPost]
        [Route("api/Friends/Respond/{requestId},{ok}")]
        public async Task<Boolean> SetFriendAccept(int id, Boolean ok)
        {
            using (var mngr = ManagerFactory.GetFriendManager())
            {
                return await mngr.SetFriendAccept(id, ok);
            }
        }

        //api/Friends/CancelFriendTask/{uid},{friendId} || endret || sjekk
        [HttpPost]
        [Route("api/Friends/Remove/{userid},{friendId}")]
        public async Task<bool> CancelFriendTask(int uid, int friendId)
        {
            using (var mngr = ManagerFactory.GetFriendManager())
            {
                return await mngr.CancelFriendTask(friendId, uid);
            }
        }


    }
}
