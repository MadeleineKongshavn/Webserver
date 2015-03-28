﻿using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using WebApplication1.Models;
using WebApplication1.Models.Class;

namespace WebApplication1.Managers
{
    public class UserManager : BaseManager
    {
        public async Task<UserClass> GetUserById(int userId)
        {
            UserClass objectDto;
            var cacheKey = String.Format("User_Get_{0}", userId);
            if ((objectDto = (UserClass)Cache.Get(cacheKey)) != null)//if object is in cache
                return objectDto;
            try
            {
                var db = new DbUser();
                objectDto = await db.GetUserById(userId);
                if (objectDto != null)
                    Cache.Insert(cacheKey, objectDto, null, DateTime.Today.AddDays(1), Cache.NoSlidingExpiration);
            }
            catch (Exception)
            {
                return null;
            }
            return objectDto;
        }

        public void UpdateUser(int userId)
        {
            //Husk å fjerne cache, det gjelder alt som oppdateres :)
            var cacheKey = String.Format("User_Get_{0}", userId);
            RemoveCacheKeysByPrefix(cacheKey);
        }

        public List<NotificationsClass> GetNotificationByUserId(int userId)
        {
            var db = new DbUser();
            return db.GetAllNotifications(userId);
        }

        public void ReadAllNotifications(int userId)
        {
            var db = new DbUser();
            db.UserHasSeenAllNotifications(userId);
        }
    }
}