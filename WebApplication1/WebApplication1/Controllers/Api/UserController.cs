﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication1.Managers;
using WebApplication1.Models;
using WebApplication1.Models.Class;

namespace WebApplication1.Controllers.Api
{
    public class UserController : ApiController
    {
        [HttpPost]
        [Route("api/User/AddFaceUser/{uid},{profilename},{xCord},{yCord}")]
        public async Task<Boolean> AddFaceUser(int uid, String profilename, long xCord, long yCord)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.AddFaceUser(uid, profilename, xCord, yCord);
            }  
        }
        [HttpPost]
        [Route("api/User/ChangeUser/{u},{pic}")]
        public async Task<Boolean> ChangeUser(UserClass u, Byte[] pic)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.ChangeUser(u, pic);
            }
        }
        [HttpPost]
        [Route("api/User/AddUser/{u},{pic}")]
        public async Task<Boolean> AddUser(UserClass u, Byte[] pic) // Ny metode
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.AddUser(u, pic);
            }
        }
        [HttpGet]
        [Route("api/User/GetAllNotifications/{id}")]
        public List<NotificationsClass> GetAllNotifications(int id)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                List<NotificationsClass> not =  mngr.GetNotificationByUserId(id);
                ReadNotifications(id);
                return not;
            }
        }
        [HttpGet]
        [Route("api/User/UserHasReadNotification/{userId}")]
        public void ReadNotifications(int userId)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                mngr.ReadAllNotifications(userId);
            }
        }
        [HttpGet]
        [Route("api/User/GetUserById/{id}")]
        public async Task<UserClass> GetUserById(int id)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.GetUserById(id);
            }
        }
        [HttpGet]
        [Route("api/User/CheckNewNotifications/{id}")]
        public async Task<Boolean> CheckNewNotifications(int id)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.CheckNewNotifications(id);
            }            
        }
    }
}
