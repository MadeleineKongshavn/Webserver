using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using WebApplication1.Models;
using WebApplication1.Models.Class;

namespace WebApplication1.Managers
{
    public class ConcertManager : BaseManager
    {
        private String PLACES_API_QUERY = "https://maps.googleapis.com/maps/api/place/details/json?reference=";
        private String DETAILED_SETIING = "&sensor=true&key=";
        private String SERVER_API_KEY = "AIzaSyDMdRA7ma1FxaL82Ev3OU8kX2YXIw44ImA";

        public async Task<List<ConcertClass>> FindConcertBasedOnQuery(String query)
        {
            var db = new DbConcert();
            return await db.FindConcertBasedOnQuery(query);
        }
        public async Task<Boolean> GetAttendingConcerTask(int cid, int uid)
        {
            var db = new DbConcert();
            return await db.GetAttendingConcerTask(cid, uid);
        }

        public async Task<Boolean> SetAttendingConcertTask(int cid, int uid, Boolean ok)
        {
            var db = new DbConcert();
            var cacheKey = String.Format("Concert_User_Get_{0}", cid);
            RemoveCacheKeysByPrefix(cacheKey); 
            return await db.SetAttendingConcertTask(cid, uid, ok);
        }

        public async Task<bool> AcceptConcertRequest(Boolean ok, int id)
        {
            var db = new DbConcert();
            int k = await db.AcceptConcertRequest(ok, id);
            if (k == -1) return false;

            if (ok)
            {
                var cacheKey = String.Format("Concert_User_Get_{0}", k);
                RemoveCacheKeysByPrefix(cacheKey);
                return true;
            }
            return true;
        }

        public async Task<bool> AddConcertRequest(int fromUsr, int toUsr, int ConcertId)
        {
            var db = new DbConcert();
            return await db.AddConcertRequest(fromUsr, toUsr, ConcertId);
        }
        public async Task<Boolean> AddConcertToUser(int userId, int concertId)
        {
            var db = new DbConcert();
            var cacheKey = String.Format("Concert_User_Get_{0}", userId);
            RemoveCacheKeysByPrefix(cacheKey); 
            return await db.AddConcertToUser(userId, concertId);
        }
        public async Task<Boolean> ChangeConcert(ConcertClass c, Byte[] pic)
        {
            var db = new DbConcert();
            var cacheKey = String.Format("Concert_Get_{0}", c.ConcertId);
            RemoveCacheKeysByPrefix(cacheKey);
            return await db.ChangeConcert(c, pic);
        }
        public async Task<Boolean> AddConcert(ConcertClass c, Byte[] pic, String locRef)
        {
            var db = new DbConcert();
            var imgUrl = await UploadImage(pic);
            if (pic != null && pic.Length != 0)
            {
                c.BitmapUrl = imgUrl;
                c.SmallBitmapUrl = imgUrl;
            }
           
            double[] coor = updateConcertLocation(c.ConcertId, c.VenueName, locRef);
            c.Xcoordinates = coor[0];
            c.Ycoordinates = coor[1];

            return await db.AddConcert(c);
        }
        public async Task<ConcertClass> GetConcertById(int id)
        {
            ConcertClass objectDto;
            var cacheKey = String.Format("Concert_Get_{0}", id);
            if ((objectDto = (ConcertClass)Cache.Get(cacheKey)) != null)
                return objectDto;
            try
            {
                var db = new DbConcert();
                objectDto = await db.GetConcertById(id);
                if (objectDto != null)
                    Cache.Insert(cacheKey, objectDto, null, DateTime.Today.AddDays(1), Cache.NoSlidingExpiration);
            }
            catch (Exception)
            {
                return null;
            }
            return objectDto;

        }


        /// <summary>
        /// No cache
        /// </summary>
        /// <param name="id"></param>
        public async Task<List<UserClass>> GetFriendsGoingToConcertById(int concertId, int userId)
        {
            var db = new DbConcert();
            return await db.FriendsGoingToConcert(userId, concertId);
        }

        public async Task<List<ConcertClass>> GetAllConcertFromUser(int userId)
        {
            //var db = new DbConcert();
            //return db.FindAllConcertToUser(id);


            List<ConcertClass> objectDto;
            var cacheKey = String.Format("Concert_User_Get_{0}", userId);
            if ((objectDto = (List<ConcertClass>)Cache.Get(cacheKey)) != null)
                return objectDto;
            try
            {
                var db = new DbConcert();
                objectDto = await db.FindAllConcertToUser(userId);
                if (objectDto != null)
                    Cache.Insert(cacheKey, objectDto, null, DateTime.Today.AddDays(1), Cache.NoSlidingExpiration);
            }
            catch (Exception)
            {
                return null;
            }
            return objectDto;

        }

        public double[] updateConcertLocation(int concertid, string area, string apiRef)
        {
            var db = new DbConcert();
            double[] coordinates = GetCoordinates(apiRef);
            return coordinates; //await db.UpdateConcertLocation(concertid, area, coordinates[0], coordinates[1]);
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
