﻿using System;
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
        public bool SetFriendRequestAccept(int id, bool status)
        {
            var db = new DbFriends();
            return db.SetFriendRequestAccept(id, status);
        }

        public void FindFriend()
        {
            
        }

        public void SendFriendRequest()
        {
            
        }

        public async Task<List<FriendsClass>> GetFriendsFromUserId(int userId)
        {
            List<FriendsClass> objectDto;
            var cacheKey = String.Format("FriendsFromUser_Get_{0}", userId);
            //if ((objectDto = (List<FriendsClass>)Cache.Get(cacheKey)) != null)//if object is in cache
            //    return objectDto;
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