﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApplication1.Managers;
using WebApplication1.Models.Class;
using WebApplication1.Models.Args;
namespace WebApplication1.Controllers.Api
{
    public class UserController : ApiController
    {
        [HttpGet]
        [Route("api/User/Get/{userid}")]
        public async Task<UserClass> GetUserById(int userid)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.GetUserById(userid);
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
        [HttpGet]
        [Route("api/User/GetAllGenres/{id}")]
        public async Task<List<GetGenreArgs>> GetAllGenres(int id)
        {
            return await ManagerFactory.GetUserManager().GetAllGenres(id);
        }
        [HttpGet]
        [Route("api/User/GetNotifications/{id}")]
        public async Task<List<NotificationsClass>> GetAllNotifications(int id)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                List<NotificationsClass> not = await mngr.GetNotificationByUserId(id);
                mngr.ReadAllNotifications(id);
                return not;
            }
        }
        [HttpPost]
        [Route("api/User/ChangePic/{userid}")]
        public async Task<bool> ChangePic(int userid)
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                HttpPostedFile filee = httpRequest.Files[0];
                Console.WriteLine(filee.ContentLength);
                using (var mngr = ManagerFactory.GetUserManager())
                {
                    var image = Image.FromStream(filee.InputStream);
                    return await mngr.ChangePic(userid, image);
                }
            }
            return false;
        }
        [HttpPost]
        [Route("api/User/ChangeRadius/{radius},{userId}")]
        public async Task<bool> ChangeRadius(int radius, int userId)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.ChangeRadius(radius, userId);
            }
        }
        [HttpPost]
        [Route("api/User/RegisterFacebook/{uid},{profilename},{path}")]
        public async Task<int> AddFaceUser(long uid, String profilename, String path)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.AddFaceUser(uid, profilename, path, 0.0, 0.0);
            }
        }
        [HttpPost]
        [Route("api/User/Login/{pass},{email}")]
        public async Task<int> NormalLogin(String pass, String email)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.NormalLogin(pass, email);
            }
        }
        [HttpPost]
        [Route("api/User/Register")]
        public async Task<int> NormalRegister([FromBody]RegisterUserArgs args)
        {
            var newUser = args;
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.NormalRegister(newUser.userName,newUser.email, newUser.password, 0, 0);
            }
        }
        [HttpPost]
        [Route("api/User/UpdateGenres")]
        public async Task<bool> UpdateGenres([FromBody] UpdateGenreArgs args)
        {
            var update = args;
            int userid = update.id;
            string[] genres = update.newGenres;

            using (var mng = ManagerFactory.GetUserManager())
            {
                return await mng.UpdateUserGenres(update.id, update.newGenres);
            }
        }

        [HttpPost]
        [Route("api/User/UpdateLocation")]
        public async Task<bool> UpdateLocation([FromBody]UpdateLocationArgs args)
        {
            var location = args;
            UserManager mng = ManagerFactory.GetUserManager();
            return await mng.updateUserLocation(location.id, location.area, location.placeId);
        }
    }
}
