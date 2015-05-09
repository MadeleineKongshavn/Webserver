using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebApplication1.Managers;
using WebApplication1.Models;
using WebApplication1.Models.Args;
using WebApplication1.Models.Class;



namespace WebApplication1.Controllers.Api
{

    public class BandController : ApiController
    {
        [HttpGet]
        [Route("api/Band/GetRandomBands/{userId}")]
        public async Task<List<BandsImagesClass>> GetRandomBands(int userId)
        {
            using (var bmgr = ManagerFactory.GetBandManager())
            {
                return await bmgr.GetRandomBands(userId);                
            }
        }
       
    
        [HttpPost]
        [Route("api/Band/AddBandToUser/{userId},{bandId}")]
        public async Task<bool> AddBandToUser(int userId, int bandId)
        {
            using (var bmgr = ManagerFactory.GetBandManager())
            {
                return await bmgr.AddBandToUser(userId, bandId);
            }
        }
        
  
       
 /*       [HttpPost]
        [Route("api/Band/ChangeBand/{b},{pic}")]
        public async Task<bool> ChangeBand(BandClass b, Byte[] pic)
        {
            using (var bmgr = ManagerFactory.GetBandManager())
            {
                return await bmgr.ChangeBand(b, pic);
            }
        } */
  
        [HttpGet]
        [Route("api/Band/FindBandBasedOnQuery/{query}")]
        public async Task<List<BandClass>> FindBandBasedOnQuery(String query)
        {
            using (var bmgr = ManagerFactory.GetBandManager())
            {
                return await bmgr.FindBandBasedOnQuery(query);
            }
        }
        [HttpGet]
        [Route("api/Band/FindBasedOnQuery/")]
        public async Task<List<BandClass>> FindBandBasedOnQuery([FromBody]QueryArgs query)
        {
            var q = query;
            using (var bmgr = ManagerFactory.GetBandManager())
            {
                return await bmgr.FindBandBasedOnQuery(query.queryString);
            }
        }





        [HttpGet]
        [Route("api/Band/FindAllBandsToUser/{id}")]
        public async Task<List<BandClass>> FindAllBandsToUser(int id)
        {
            using (var bmgr = ManagerFactory.GetBandManager())
            {
                return  await bmgr.FindAllBandsToUser(id);
            }
        }
       
        [HttpGet]
        [Route("api/Band/GetBandById/{id}")]
        public async Task<BandClass> GetBandById(int id)
        {
            using (var bmngr = ManagerFactory.GetBandManager())
            {
                return await bmngr.GetBandById(id);
            }
        }
       
        [HttpGet]
        [Route("api/Band/GetAllAdminBands/{userId}")]
        public async Task<List<MemberClass>> GetAllAdminBands(int userId)
        {
            using (var bmngr = ManagerFactory.GetBandManager())
            {
                return await bmngr.GetAllAdminBands(userId);
            }
        }
        
        [HttpPost]
        [Route("api/Band/UpdateMusicUrl/{bandId},{url}")]
        public async Task<bool> UpdateMusicUrl(int bandId, String url)
        {
            using (var bmngr = ManagerFactory.GetBandManager())
            {
                return await bmngr.UpdateMusicUrl(bandId, url);              
            }
        }
        
        [HttpPost]
        [Route("api/Band/TestObject/{b}")]
        public async Task<bool> TestObject(BandClass b)
        {
            return true;
        }
        
        [HttpPost]
        [Route("api/Band/AddBand/{name},{userId}")]
        public async Task<bool> AddBand(string name, int userId)
        {
            using (var bmngr = ManagerFactory.GetBandManager())
            {
                return await bmngr.AddBand(name, userId);
            }
        
        }
        
        [HttpPost]
        [Route("api/Band/updateBandName/{name},{bandId}")]
        public async Task<bool> updateBandName(String name, int bandId)
        {
            if (bandId == null || name == null)
                return false;

            using (var mng = ManagerFactory.GetBandManager())
            {
                return await mng.UpdateBandName(name, bandId);
            }
        }

       
        [HttpPost]
        [Route("api/Band/UpdateLocation")]
        public async Task<bool> UpdateLocation([FromBody]UpdateLocationArgs args)
        {
            var location = args;
            int bandid = location.id;
            String area = location.area;
            String placeId = location.placeId;
                BandManager mng = ManagerFactory.GetBandManager();
                return await mng.updateBandLocation(bandid, area, placeId);
            
        }



        [HttpPost]
        [Route("api/Band/updateBandLinks/{bandid},{www},{fb},{soundcloud}")]
        public async Task<bool> updateBandLinks(int bandid, string www,string fb, string soundcloud)
        {
            using (var mng = ManagerFactory.GetBandManager())
            {
                return await mng.updateBandLinks(bandid, www, fb, soundcloud);
            }
        }
        [HttpPost]
        [Route("api/Band/UpdateBandImage/{bandid}")]
        public async Task<bool> ChangePic(int userid)
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                HttpPostedFile filee = httpRequest.Files[0];
                Console.WriteLine(filee.ContentLength);
                using (var mngr = ManagerFactory.GetBandManager())
                {
                    var image = Image.FromStream(filee.InputStream);
                    return await mngr.ChangePic(userid, image);
                }
            }
            return false;
        }

        [HttpPost]
        [Route("api/Band/UpdateGenres")]
        public async Task<bool> UpdateGenres([FromBody] UpdateGenreArgs args)
        {

            var update = args;
            int bandid = update.id;
            string[] genres = update.newGenres;   
       
           using(var mng = ManagerFactory.GetBandManager())
           {
               return await mng.updateBandGenres(bandid, genres);
           }
        }

        [HttpPost]
        [Route("api/Band/AddToUserList/{userid},{bandid},{ok}")]
        public async Task<bool> AddToUserList(int userid, int bandid, bool ok)
        {
            using (var mng = ManagerFactory.GetBandManager())
            {
                return await mng.AddToUserList(userid, bandid, ok);
            }
        }


        [HttpPost]
        [Route("api/Band/PostTestObject/")]
        public BandClass PostTestObject([FromBody]BandClass obj)
        {
            var b = obj;
            return new BandClass() { BandName = b.BandName };

        }

        [HttpGet]
        [Route("api/Band/GetAllGenres/{id}")]
        public async Task<List<GetGenreArgs>> GetAllGenres(int id)
        {
            return await ManagerFactory.GetBandManager().GetAllGenres(id);
        }

  

    }
}
