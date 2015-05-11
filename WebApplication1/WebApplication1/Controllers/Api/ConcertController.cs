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
    public class ConcertArgsClass
    {
        public String title,venue,locRef; 
        public int bandId, year, month, day,hour, min;
        public Boolean getBitmapUrl;
    }

    public class ConcertController : ApiController
    {
        [HttpGet]
        [Route("api/Concert/GetRandomConcert/{userId}")]
        public async Task<List<BandsImagesClass>> GetRandomConcert(int userId)
        {
            using (var bmgr = ManagerFactory.GetConcertManager())
            {
                return await bmgr.GetRandomConcert(userId);
            }
        }
        [HttpGet]
        [Route("api/Concert/AddRemoveConcertToUser/{concertId},{userId},{ok}")]
        public async Task<bool> AddRemoveConcertToUser(int concertId, int userId, bool ok)
        {
            using (var bmgr = ManagerFactory.GetConcertManager())
            {
                return await bmgr.AddRemoveConcertToUser(concertId, userId, ok);
            } 
        }

        [HttpGet]
        [Route("api/Concert/NumberGoingToConcert/{concertId},{userId}")]
        public async Task<ConcertInfoClass> NumberGoingToConcert(int concertId, int userId)
        {
            using (var bmgr = ManagerFactory.GetConcertManager())
            {
                return await bmgr.NumberGoingToConcert(concertId, userId);
            }         
        }
        [HttpGet]
        [Route("api/Concert/FindFriendsGoingToConcert/{concertId},{userId}")]
        public async Task<List<FriendsClass>> FindFriendsGoingToConcert(int concertId, int userId)
        {
            using (var bmgr = ManagerFactory.GetConcertManager())
            {
                return await bmgr.FindFriendsGoingToConcert(concertId, userId);
            }
        }
        [HttpGet]
        [Route("api/Concert/FindConcertBasedOnQuery/{query}")]
        public async Task<List<ConcertClass>> FindConcertBasedOnQuery(String query)
        {
            using (var bmgr = ManagerFactory.GetConcertManager())
            {
                return await bmgr.FindConcertBasedOnQuery(query);
            }
        }
        [HttpGet]
        [Route("api/Concert/GetAttendingConcertTask/{cid},{uid}")]
        public async Task<Boolean> GetAttendingConcertTask(int cid, int uid)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.GetAttendingConcerTask(cid, uid);
            }
        }
        [HttpGet]
        [Route("api/Concert/SetAttendingConcertTask/{cid},{uid},{ok}")]
        public async Task<Boolean> SetAttendingConcertTask(int cid, int uid, Boolean ok)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.SetAttendingConcertTask(cid, uid, ok);
            }     
        }
        [HttpPost]
        [Route("api/Concert/AcceptConcertRequest/{id},{ok}")]
        public async Task<bool> AcceptConcertRequest(int id, Boolean ok)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.AcceptConcertRequest(ok, id);
            }
        }
        [HttpPost]
        [Route("api/Concert/AddConcertRequest/{fromUsr},{toUsr},{ConcertId}")]
        public async Task<bool> AddConcertRequest(int fromUsr, int toUsr, int ConcertId)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.AddConcertRequest(fromUsr, toUsr, ConcertId);
            }
        }
        [HttpPost]
        [Route("api/Concert/AddConcertToUser/{userId},{concertId},{ok}")]
        public async Task<Boolean> AddConcertToUser(int userId, int concertId, bool ok)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.AddConcertToUser(userId, concertId, ok);
            }  
        }
        [HttpPost]
        [Route("api/Concert/ChangeConcert/{c},{pic}")]
        public async Task<Boolean> ChangeConcert(ConcertClass c, Byte[] pic)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.ChangeConcert(c, pic);
            }
        }

        [HttpPost]
        [Route("api/Concert/AddConcert/")]
        public async Task<Boolean> AddConcert(ConcertArgsClass args)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                var a = args;
                ConcertClass c = new ConcertClass();
                c.Title = a.title;
                c.BandId = a.bandId;
                c.Date = new DateTime(a.year, a.month, a.day, a.hour, a.min, 0);
                c.VenueName = a.venue;
                return await cMgr.AddConcert(c, a.locRef, a.getBitmapUrl);
            }
        }


        [HttpGet]
        [Route("api/Concert/FindAllConcertToUser/{id}")]
        public async Task<List<ConcertClass>> FindAllConcertToUser(int id)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.GetAllConcertFromUser(id);
            }
        }
        [Route("api/Concert/GetConcertinfo/{id}")]
        public async Task<ConcertClass> GetConcertInfo(int id)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.GetConcertById(id);
            }
        }

        [Route("api/Concert/GetConcertInfoWithFriends/{id}/{userid}")]
        public async Task<List<UserClass>> GetConcertInfoWithFriends(int id,int userid)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.GetFriendsGoingToConcertById(id,userid);
            }
        }

       /* [Route("api/Concert/UpdateConcertLocation/{concertid},{venue},{ref}")]
        public async Task<bool> UpdateConcertLocation(int concertidea,string venue,string refr)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.updateConcertLocation(concertidea, venue, refr);
            }
        }*/


    }
}
