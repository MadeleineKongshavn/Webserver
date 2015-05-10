using System;
using System.Drawing;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Net;
using WebApplication1.Models;
using WebApplication1.Models.Class;
using WebApplication1.Models.Args;


namespace WebApplication1.Managers
{
    public class BandManager : BaseManager
    {

        private String PLACES_API_QUERY = "https://maps.googleapis.com/maps/api/place/details/json?placeid=";
        private String SERVER_API_KEY = "&key=AIzaSyDMdRA7ma1FxaL82Ev3OU8kX2YXIw44ImA";
        

        public async Task<bool> ChangePic(int bid, Image image)
        {
            var cacheKey = String.Format("Band_Get_{0}", bid);
            RemoveCacheKeysByPrefix(cacheKey);
            var imgUrl = await UploadImage(image);
            var db = new DbBand();
            return await db.ChangePic(bid, imgUrl, imgUrl);
        }

        public async Task<List<BandClass>> FindBandBasedOnQuery(String query)
        {
            var db = new DbBand();
            return await db.FindBandBasedOnQuery(query);
        }
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
        
        public async Task<String> Upload(Byte[] pic)
        {
            return await UploadImage(pic);
        }
        
   /*     public async Task<bool> ChangeBand(BandClass b, Byte[] pic)
        {
            var db = new DbBand();
            var cacheKey = String.Format("Band_GetBandToUser_{0}", b.BandId);
            RemoveCacheKeysByPrefix(cacheKey);
            var imgUrl = await UploadImage(pic);
            b.BitmapUrl = imgUrl;
            b.SmallBitmapUrl = imgUrl;
            return await db.ChangeBand(b);
        }*/
      
        public async Task<BandClass> GetBandById(int bandId)
        {
            BandClass bandClass;
            var cacheKey = String.Format("Band_Get_{0}", bandId);
            if ((bandClass = (BandClass)Cache.Get(cacheKey)) != null) return bandClass;
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

        public async Task<bool> UpdateBandName(String name, int bandId)
        {
            var db=new DbBand();
            var cacheKey = String.Format("Band_Get_{0}", bandId);
            RemoveCacheKeysByPrefix(cacheKey);
            return await db.UpdateBandName(bandId,name);
        }

        public async Task<bool> updateBandLinks(int bandid, string www, string fb, string soundcloud)
        {
            var db = new DbBand();  
            bool ok= await db.UpdateBandLinks(bandid,www,fb,soundcloud);
            var cacheKey = String.Format("Band_Get_{0}", bandid);
            RemoveCacheKeysByPrefix(cacheKey);
            return ok;
        }

        public async Task<bool> updateBandLocation(int bandid, string area, string apiRef)
        {
            var db = new DbBand();
            double[] coordinates = GetCoordinates(apiRef);
            if (coordinates == null)
                return false;
            else
            return await db.UpdateBandLocation(bandid, area, coordinates[0], coordinates[1]);
        }

        private double[] GetCoordinates(String placesRef)
        {
            StringBuilder builder = new StringBuilder(PLACES_API_QUERY);
            builder.Append(placesRef);
            builder.Append(SERVER_API_KEY);
            String query = builder.ToString();
            Console.Write(query.ToString());
            System.Net.HttpWebRequest webRequest = System.Net.WebRequest.Create(query) as HttpWebRequest;
            webRequest.Timeout = 20000;
            webRequest.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
            return RequestCompleted(response);
        }

        private double[] RequestCompleted(HttpWebResponse res)
        {
            if (res.ContentLength == null)
                return null;

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

        public async Task<bool> updateBandGenres(int bandid, string[] newGenres)
        {
            var db = new DbBand();
            bool ok= await db.UpdateBandGenres(bandid, newGenres);
            var cacheKey = String.Format("Band_Get_{0}", bandid);
            RemoveCacheKeysByPrefix(cacheKey);

            return ok;
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

        public async Task<bool> AddToUserList(int userid,int bandid, bool ok)
        {
            return await new DbBand().AddToUserList(userid, bandid, ok);
        }

        public async Task<List<GetGenreArgs>> GetAllGenres(int userid)
        {
            List<GenreClass> allGenres = await new DbGenre().getGenres();
            List<GenreClass> bandGenres = await new DbBand().GetBandGenres(userid);
            List<GetGenreArgs> args = new List<GetGenreArgs>();

            if (bandGenres != null)
                foreach (GenreClass genre in bandGenres)
                {
                    GenreClass toList = allGenres.Where(x => x.GenreId == genre.GenreId).FirstOrDefault();
                    allGenres.Remove(toList);

                    args.Add(new GetGenreArgs()
                    {
                        genreid = toList.GenreId,
                        genreName = toList.GenreName,
                        isChosen = true
                    });


                }
            if (allGenres.Count() != 0)
                foreach (GenreClass genre in allGenres)
                {
                    args.Add(new GetGenreArgs()
                    {
                        genreid = genre.GenreId,
                        genreName = genre.GenreName,
                        isChosen = false
                    });

                }

            return args;
        }

        public async Task<bool> GetIfBandIsAdded(int userId, int bandId)
        {
            var db = new DbBand();
            return await db.GetIfBandIsAdded(userId, bandId);
        }
    }
}
