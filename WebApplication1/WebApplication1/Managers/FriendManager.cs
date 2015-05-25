using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Models.Db;
using System.Web.Caching;
namespace WebApplication1.Managers
{
    public class FriendManager : BaseManager
    {
        public async Task<bool> CancelFriendTask(int friendId, int uid)
        {
            var db = new DbFriends();
            return await db.CancelFriendTask(friendId, uid);
        }
        public async Task<Boolean> AddFriendRequest(int userId, int friendId)
        {
            var db = new DbFriends();
            return await db.AddFriendRequest(userId, friendId);
        }
        public async Task<Boolean> SetFriendAccept(int id, bool status)
        {
            var db = new DbFriends();
            return await db.SetFriendAccept(id, status);
        }
        public async Task<FriendsClass> FindFriend(String name, int uid)
        {
            var db = new DbFriends();
            return await db.FindFriend(name, uid);
        }
        public async Task<List<FriendsClass>> GetFriendsFromUserId(int userId)
        {
            var db = new DbFriends();
            return await db.GetFriendsByUserId(userId);
        }
    }
}
