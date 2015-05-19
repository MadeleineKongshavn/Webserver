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
        //OK
        [HttpGet]
        [Route("api/Band/GetRandom/{userId}")]
        public async Task<List<BandsImagesClass>> GetRandomBands(int userId)
        {
            using (var bmgr = ManagerFactory.GetBandManager())
            {
                return await bmgr.GetRandomBands(userId);
            }
        }


        //api/Band/FindBandBasedOnQuery/{query} || endret ||sjekk
        [HttpGet]
        [Route("api/Band/Query/{query}")]
        public async Task<List<BandClass>> FindBandBasedOnQuery(String query)
        {
            using (var bmgr = ManagerFactory.GetBandManager())
            {
                return await bmgr.FindBandBasedOnQuery(query);
            }
        }

        //api/Band/FindAllBandsToUser/{id} || endret || sjekk
        [HttpGet]
        [Route("api/Band/GetUsersList/{id}")]
        public async Task<List<BandClass>> FindAllBandsToUser(int id)
        {
            using (var bmgr = ManagerFactory.GetBandManager())
            {
                return await bmgr.FindAllBandsToUser(id);
            }
        }

        //api/Band/GetBandById/{id} || endret || sjekk
        [HttpGet]
        [Route("api/Band/Get/{id}")]
        public async Task<BandClass> GetBandById(int id)
        {
            using (var bmngr = ManagerFactory.GetBandManager())
            {
                return await bmngr.GetBandById(id);
            }
        }

        //api/Band/GetAllAdminBands/{userId} || endret || sjekk
        [HttpGet]
        [Route("api/Band/GetProfiles/{userId}")]
        public async Task<List<MemberClass>> GetAllAdminBands(int userId)
        {
            using (var bmngr = ManagerFactory.GetBandManager())
            {
                return await bmngr.GetAllAdminBands(userId);
            }
        }

        //OK
        [HttpGet]
        [Route("api/Band/GetAllGenres/{id}")]
        public async Task<List<GetGenreArgs>> GetAllGenres(int id)
        {
            return await ManagerFactory.GetBandManager().GetAllGenres(id);
        }

        //api/Band/GetIfBandIsAdded/{userId},{bandId} || endret || sjekk
        [HttpGet]
        [Route("api/Band/IsAdded/{userId},{bandId}")]
        public async Task<bool> GetIfBandIsAdded(int userId, int bandId)
        {
            return await ManagerFactory.GetBandManager().GetIfBandIsAdded(userId, bandId);
        }



        //api/Band/AddBand/{name},{userId} || endret ||sjekk
        [HttpPost]
        [Route("api/Band/Add/{name},{userId}")]
        public async Task<bool> AddBand(string name, int userId)
        {
            using (var bmngr = ManagerFactory.GetBandManager())
            {
                return await bmngr.AddBand(name, userId);
            }

        }

        //api/Band/updateBandName/{name},{bandId} ||endret ||sjekk
        [HttpPost]
        [Route("api/Band/UpdateName/{name},{bandId}")]
        public async Task<bool> UpdateBandName(String name, int bandId)
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
        [Route("api/Band/UpdateWwwLink/")]
        public async Task<bool> UpdateWwwLink([FromBody]WwwLinkArgs args)
        {
            var link = args;
            using (var mng = ManagerFactory.GetBandManager())
            {
                return await mng.updateBandLink(args.id, BandManager.TYPE_WWW, args.link);
            }
        }

        [HttpPost]
        [Route("api/Band/UpdateScLink/{bandid},{postfix}")]
        public async Task<bool> UpdateScLink(int bandid, string postfix)
        {
            using (var mng = ManagerFactory.GetBandManager())
            {
                return await mng.updateBandLink(bandid, BandManager.TYPE_SC, postfix);
            }
        }

        [HttpPost]
        [Route("api/Band/UpdateFbLink/{bandid},{postfix}")]
        public async Task<bool> UpdateFbLink(int bandid, string postfix)
        {
            using (var mng = ManagerFactory.GetBandManager())
            {
                return await mng.updateBandLink(bandid, BandManager.TYPE_FB, postfix);
            }
        }

        //OK
        [HttpPost]
        [Route("api/Band/ChangePic/{bandid}")]
        public async Task<bool> ChangePic(int bandid)
        {
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                HttpPostedFile filee = httpRequest.Files[0];
                Console.WriteLine(filee.ContentLength);
                using (var mngr = ManagerFactory.GetBandManager())
                {
                    var image = Image.FromStream(filee.InputStream);
                    return await mngr.ChangePic(bandid, image);
                }
            }
            return false;
        }

        //OK
        [HttpPost]
        [Route("api/Band/UpdateGenres")]
        public async Task<bool> UpdateGenres([FromBody] UpdateGenreArgs args)
        {

            var update = args;
            int bandid = update.id;
            string[] genres = update.newGenres;

            using (var mng = ManagerFactory.GetBandManager())
            {
                return await mng.updateBandGenres(bandid, genres);
            }
        }


        //OK
        [HttpPost]
        [Route("api/Band/AddToUserList/{userid},{bandid},{ok}")]
        public async Task<bool> AddToUserList(int userid, int bandid, bool ok)
        {
            using (var mng = ManagerFactory.GetBandManager())
            {
                return await mng.AddToUserList(userid, bandid, ok);
            }
        }

    }
}
