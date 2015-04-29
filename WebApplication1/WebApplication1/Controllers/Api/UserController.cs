using System;
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
        [HttpGet]
        [Route("api/User/ChangePic/{pic},{uid}")]
        public async Task<bool> ChangePic(int uid, byte[] pic)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.ChangePic(uid, pic);
            }
        }
        [HttpGet]
        [Route("api/User/ChangeRadius/{radius},{userId}")]
        public async Task<bool> ChangeRadius(int radius, int userId)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.ChangeRadius(radius, userId);
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
        [Route("api/User/CheckUserName/{name}")]
        public async Task<int> CheckUserName(String name)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.CheckUserName(name);
            }    
        }
        [HttpGet]
        [Route("api/User/CheckEmail/{email}")]
        public async Task<int> CheckEmail(String email)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.CheckEmail(email);
            }
        }
        [HttpGet]
        [Route("api/User/AddFaceUser/{uid},{profilename},{path}")]
        public async Task<int> AddFaceUser(long uid, String profilename, String path)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.AddFaceUser(uid, profilename, path, 0.0, 0.0);
            }
        }


        [HttpGet]
        [Route("api/User/NormalLogin/{pass},{email}")]
        public async Task<int> NormalLogin(String pass, String email)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.NormalLogin(pass, email);
            }            
        }

        [HttpGet]
        [Route("api/User/NormalRegister/{name},{email},{pass}")]
        public async Task<int> NormalRegister(String name, String email, String pass)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.NormalRegister(name, email, pass, 0, 0);
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
        [Route("api/User/CheckNewNotifications/{id}")]
        public async Task<Boolean> CheckNewNotifications(int id)
        {
            using (var mngr = ManagerFactory.GetUserManager())
            {
                return await mngr.CheckNewNotifications(id);
            }            
        }
        [HttpPost]
        [Route("api/User/UpdateUserGenres/{userid},{genres}")]
        public async Task<bool> UpdateUserGenres(int userid,string genres)
        {
            string[] gen = genres.Split(',');
            using (var mng = ManagerFactory.GetUserManager())
            {
                return await mng.UpdateUserGenres(userid, gen);
            }
        }

        [HttpPost]
        [Route("api/User/UpdateUserLocation/{userid},{area},{apiref}")]
        public async Task<bool> UpdateUserLocation(int userid, string area, string aipRef)
        {
            using (var mng = ManagerFactory.GetUserManager())
            {
                return await mng.updateUserLocation(userid, area, aipRef);
            }
        }

    }
}
