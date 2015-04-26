using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Net;
using WebApplication1.Models;
using WebApplication1.Models.Class;

namespace WebApplication1.Managers
{
    public class BandManager : BaseManager
    {

        private String PLACES_API_BASE = "https://maps.googleapis.com/maps/api/place";
        private String DETAILED_INFO = "/details/json?reference=";
        private String DETAILED_SETIING = "&sensor=true&key=";
        private String SERVER_API_KEY = "AIzaSyDMdRA7ma1FxaL82Ev3OU8kX2YXIw44ImA";

        //        /details/json?reference=CiQYAAAA0Q_JA...kT3ufVLDDvTQsOwZ_tc&sensor=true&key=AddYourOwnKeyHere

     
/*        public async Task<List<BandClass>> FindBandBasedOnQuery(String query, int uid)
        {
            var db = new DbBand();
            return await db.FindBandBasedOnQuery(query, uid);
        }*/
        public async Task<List<BandsImagesClass>> GetRandomBands(int userId)
        {
            var db = new DbBand();
            return await db.GetRandomBands(userId);
        }

        public async Task<bool> AddBand(String name, int userId)
        {
            var db = new DbBand();
            return await db.AddBand(name, userId);
        }
        
        public async Task<List<MemberClass>> GetAllAdminBands(int userId)
        {
            var db = new DbBand();
            return await db.GetAllAdminBands(userId);
        }
        
        public async Task<bool> UpdateMusicUrl(int bandId, String url)
        {
            var db = new DbBand();
            return await db.UpdateMusicUrl(bandId, url);
        }
        
        public async Task<bool> AddBandToUser(int userId, int bandId)
        {
            var db = new DbBand();
            var cacheKey = String.Format("Band_GetBandToUser_{0}", userId);
            RemoveCacheKeysByPrefix(cacheKey);
            return await db.AddBandToUser(userId, bandId);
        }
        
        public async Task<bool> AddBand(BandClass b, Byte[] pic)
        {
            var db = new DbBand();
            var imgUrl = await UploadImage(pic);
            b.BitmapUrl = imgUrl;
            b.SmallBitmapUrl = imgUrl;
            return await db.AddBand(b);
        }

        public async Task<String> Upload(Byte[] pic)
        {
            return await UploadImage(pic);
        }
        
        public async Task<bool> ChangeBand(BandClass b, Byte[] pic)
        {
            var db = new DbBand();
            var cacheKey = String.Format("Band_GetBandToUser_{0}", b.BandId);
            RemoveCacheKeysByPrefix(cacheKey);
            var imgUrl = await UploadImage(pic);
            b.BitmapUrl = imgUrl;
            b.SmallBitmapUrl = imgUrl;
            return await db.ChangeBand(b);
        }
      
        public async Task<BandClass> GetBandById(int bandId)
        {
            BandClass bandClass;
            var cacheKey = String.Format("Band_Get_{0}", bandId);
            if ((bandClass = (BandClass)Cache.Get(cacheKey)) != null)
                return bandClass;

            try
            {
                var db = new DbBand();
                bandClass = await db.GetBandById(bandId);
                if(bandClass != null)
                    Cache.Insert(cacheKey, bandClass, null, DateTime.Today.AddDays(1), Cache.NoSlidingExpiration);
            }
            catch (Exception)
            {
                return null;
            }
            return bandClass;
        }

        //fikse rekkefølge!
        public async Task<bool> updateBandName(String name, int bandId)
        {
            var db=new DbBand();
            return await db.UpdateBandName(bandId,name);

        }

        public async Task<bool> updateBandLinks(int bandid, string www, string fb, string soundcloud)
        {
            var db = new DbBand();
            return await db.UpdateBandLinks(bandid,www,fb,soundcloud);
        }

        public async Task<bool> updateBandLocation(int bandid,string area,long x,long y)
        {
            var db = new DbBand();
          //  long[] coordinates = GetCoordinates();
            return await db.UpdateBandLocation(bandid,area,x,y);
        }

        public void GetCoordinates(String placesRef){

            System.Net.HttpWebRequest webRequest = System.Net.WebRequest.Create(@"https://maps.googleapis.com/maps/api/place/details/json?reference=CoQBdAAAACIg0nIvOsdxqJKbL3HffQaFUUVLvCLXqVwLeyNVPtlJvsFR1DFbUCeh2N-gu7dLMW50vIGaIrH-mzk0rInbuV5Twy7lphbZKH1O-V5o1CEf3Kr7lxBBYK8tAiJMcdsf6CFZ7m8M0VSmSTayEviqqoysiVKLhXZ8dJ6Wcj9WWRO_EhA3ny5p9aIA1aAeCjMTil_oGhRDVTJJdS2kGniFpCeobF4PifX1mA&sensor=false&key=AIzaSyD3jfeMZK1SWfRFDgMfxn_zrGRSjE7S8Vg") as HttpWebRequest;

            //"CoQBdAAAACIg0nIvOsdxqJKbL3HffQaFUUVLvCLXqVwLeyNVPtlJvsFR1DFbUCeh2N-gu7dLMW50vIGaIrH-mzk0rInbuV5Twy7lphbZKH1O-V5o1CEf3Kr7lxBBYK8tAiJMcdsf6CFZ7m8M0VSmSTayEviqqoysiVKLhXZ8dJ6Wcj9WWRO_EhA3ny5p9aIA1aAeCjMTil_oGhRDVTJJdS2kGniFpCeobF4PifX1mA"
            ///details/json?reference=CiQYAAAA0Q_JA...kT3ufVLDDvTQsOwZ_tc&sensor=true&key=AddYourOwnKeyHere
            
            webRequest.Timeout = 20000;
            webRequest.Method = "GET";

            webRequest.BeginGetResponse(new AsyncCallback(RequestCompleted), webRequest);
            
        }

        private void RequestCompleted(IAsyncResult result)
        {
            var request = (HttpWebRequest)result.AsyncState;
            var response = (HttpWebResponse)request.EndGetResponse(result);
            using (var stream = response.GetResponseStream())
            {
                var r = new System.IO.StreamReader(stream);
                var resp = r.ReadToEnd();
                Console.Write(resp.ToString());
            }

        }


        public async Task<bool> UpdateBand(BandClass b)
        {
            var db = new DbBand();
            return await db.UpdateBand(b);
        }

        public async Task<List<BandClass>> FindAllBandsToUser(int userId)
        {
            List<BandClass> bandList;
            //Ingen grunn å cache noe som er så brukerspesefikt
            //cacher :D
            var cacheKey = String.Format("Band_GetBandToUser_{0}", userId);
            if ((bandList = (List<BandClass>)Cache.Get(cacheKey)) != null)
            return bandList;

            try
            {
                var db = new DbBand();
                bandList = await db.FindAllBandsToUser(userId);
                if (bandList != null)
                    Cache.Insert(cacheKey, bandList, null, DateTime.Today.AddDays(1), Cache.NoSlidingExpiration);

            }
            catch (Exception)
            {
                return null;
            }
            return bandList;
        }

    }
}
