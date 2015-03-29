using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebApplication1.Models;
using WebApplication1.Models.Class;
using WebApplication1.Models.Db;
using System.Web.Caching;

namespace WebApplication1.Managers
{
    public class FriendManager : BaseManager
    {
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

        public async Task<FriendsClass> FindFriend(String name)
        {
            var db = new DbFriends();
            return await db.FindFriend(name);
        }
        public void UpdateFriendList(int userId)
        {
            var cacheKey = String.Format("FriendsFromUser_Get_{0}", userId);
            RemoveCacheKeysByPrefix(cacheKey);
        }

        public async Task<List<FriendsClass>> GetFriendsFromUserId(int userId)
        {
            List<FriendsClass> objectDto;
            var cacheKey = String.Format("FriendsFromUser_Get_{0}", userId);
        //    if ((objectDto = (List<FriendsClass>)Cache.Get(cacheKey)) != null)//if object is in cache
        //        return objectDto;
            try
            {
                var db = new DbFriends();
                objectDto = await db.GetFriendsByUserId(userId);
                if (objectDto != null)
                    Cache.Insert(cacheKey, objectDto, null, DateTime.Today.AddDays(1), Cache.NoSlidingExpiration);
            }
            catch (Exception)
            {
                return null;
            }
            return objectDto;
        }
    }
}
