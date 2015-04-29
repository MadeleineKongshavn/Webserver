using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using WebApplication1.Models;
using WebApplication1.Models.Class;

namespace WebApplication1.Managers
{
    public class UserManager : BaseManager
    {

        private String PLACES_API_QUERY = "https://maps.googleapis.com/maps/api/place/details/json?reference=";
        private String DETAILED_SETIING = "&sensor=true&key=";
        private String SERVER_API_KEY = "AIzaSyDMdRA7ma1FxaL82Ev3OU8kX2YXIw44ImA";



        public async Task<int> CheckEmail(String email)
        {
            var db = new DbUser();
            return await db.CheckEmail(email);

        }
        public async Task<int> CheckUserName(String name)
        {
            var db = new DbUser();
            return await db.CheckUserName(name);
        }

        public async Task<int> NormalRegister(String name, String email, String pass, double yCord, double xCord)
        {
            var db = new DbUser();
            return await db.NormalRegister(name, email, pass, yCord, xCord);
        }
        public async Task<int> NormalLogin(String pass, String email)
        {
            var db = new DbUser();
            return await db.NormalLogin(pass, email);
        }
        public async Task<int> AddFaceUser(long uid, String profilename,String path, double xCord, double yCord)
        {
            var db = new DbUser();
            return await db.AddFaceUser(uid, profilename, path, xCord, yCord);
        }
        public async Task<Boolean> CheckNewNotifications(int id)
        {
            var db = new DbUser();
            return await db.CheckNewNotifications(id);
        }
        public async Task<Boolean> AddUser(UserClass u, Byte[] pic) 
        {
            var db = new DbUser();
            var imgUrl = await UploadImage(pic);
            u.Url = imgUrl;
            return await db.AddUser(u);
        }

        public async Task<Boolean> ChangeUser(UserClass u, Byte[] pic)
        {
            UpdateUser(u.UserId);
            var db = new DbUser();
            var imgUrl = await UploadImage(pic);
            u.Url = imgUrl;
            return await db.ChangeUser(u);
        }

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

        public async Task<bool> UpdateUserGenres(int userid,string[] genres)
        {
            var db = new DbUser();
            return await db.UpdateUserGenres(userid, genres);
        }

        public async Task<bool> updateUserLocation(int userid, string area, string apiRef)
        {
            var db = new DbUser();
            double[] coordinates = GetCoordinates(apiRef);
            return await db.UpdateUserLocation(userid, area, coordinates[0], coordinates[1]);
        }

        private double[] GetCoordinates(String placesRef)
        {
            StringBuilder query = new StringBuilder(PLACES_API_QUERY);
            query.Append(placesRef);
            query.Append(DETAILED_SETIING);
            query.Append(SERVER_API_KEY);
            Console.Write(query.ToString());
            System.Net.HttpWebRequest webRequest = System.Net.WebRequest.Create(query.ToString()) as HttpWebRequest;
            webRequest.Timeout = 20000;
            webRequest.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            return RequestCompleted(response);
        }

        private double[] RequestCompleted(HttpWebResponse res)
        {
            double[] coord = new double[2];
            var response = (HttpWebResponse)res;

            using (var stream = response.GetResponseStream())
            {
                var r = new System.IO.StreamReader(stream);
                var resp = r.ReadToEnd();
                JObject jsonResp = JObject.Parse(resp.ToString());
                JValue lng = (JValue)jsonResp["result"]["geometry"]["location"]["lng"];
                double longitude = (double)lng.Value;
                coord[0] = longitude;

                JValue lat = (JValue)jsonResp["result"]["geometry"]["location"]["lat"];
                double latidtude = (double)lat.Value;
                coord[1] = latidtude;

                Console.Write(resp.ToString());
            }

            return coord;
        }

    }
}
