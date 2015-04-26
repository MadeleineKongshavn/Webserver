using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebApplication1.Managers;
using WebApplication1.Models;
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
        
        [HttpPost]
        [Route("api/Band/AddBand/{b},{pic}")]
        public async Task<bool> AddBand(BandClass b, Byte[] pic)
        {
            using (var bmgr = ManagerFactory.GetBandManager())
            {
                return await bmgr.AddBand(b, pic);
            }
        }
       
        [HttpPost]
        [Route("api/Band/ChangeBand/{b},{pic}")]
        public async Task<bool> ChangeBand(BandClass b, Byte[] pic)
        {
            using (var bmgr = ManagerFactory.GetBandManager())
            {
                return await bmgr.ChangeBand(b, pic);
            }
        } 
  
        /*      [HttpGet]
        [Route("api/Band/FindBandBasedOnQuery/{query},{uid}")]
        public async Task<List<BandClass>> FindBandBasedOnQuery(String query, int uid)
        {
            using (var bmgr = ManagerFactory.GetBandManager())
            {
                return await bmgr.FindBandBasedOnQuery(query, uid);
            }
        }*/

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
        public async Task<bool> AddBand(String name, int userId)
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
                return await mng.updateBandName(name, bandId);
            }
        }

        [HttpPost]
        [Route("api/Band/updateBandLocation/{bandid},{area},{x},{y}")]
        public async Task<bool> updateBandLocation(int bandid,string area,long x, long y)
        {
            if (bandid == null || area == null)
                return false;

            using (var mng = ManagerFactory.GetBandManager())
            {
                long xCoo = (long)x;
                long yCoo = (long)y;

                return await mng.updateBandLocation(bandid, area, x, y);
            }
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
        [Route("api/Band/UpdateBand/{jsonBand}")]
        public async Task<bool> UpdateBand(String jsonBand)
        {
            BandManager mng=ManagerFactory.GetBandManager();
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            BandClass b = (BandClass)serializer.Deserialize<BandClass>(jsonBand);
            return await mng.UpdateBand(b);
        } 

    }
}
