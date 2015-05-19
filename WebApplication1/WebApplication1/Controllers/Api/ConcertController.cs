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
        
        //GET

        //OK || endret || sjekk
        [HttpGet]
        [Route("api/Concert/GetRandom/{userId}")]
        public async Task<List<BandsImagesClass>> GetRandomConcert(int userId)
        {
            using (var bmgr = ManagerFactory.GetConcertManager())
            {
                return await bmgr.GetRandomConcert(userId);
            }
        }

        //api/Concert/GetConcertinfo/{id} || endret 

        [Route("api/Concert/Get/{id}")]
        public async Task<ConcertClass> GetConcertInfo(int id)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.GetConcertById(id);
            }
        }

        //api/Concert/FindConcertBasedOnQuery/{query} ||endret
        [HttpGet]
        [Route("api/Concert/Query/{query}")]
        public async Task<List<ConcertClass>> FindConcertBasedOnQuery(String query)
        {
            using (var bmgr = ManagerFactory.GetConcertManager())
            {
                return await bmgr.FindConcertBasedOnQuery(query);
            }
        }

        //api/Concert/FindAllConcertToUser/{id} || endret
        [HttpGet]
        [Route("api/Concert/GetUsersList/{id}")]
        public async Task<List<ConcertClass>> FindAllConcertToUser(int id)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.GetAllConcertFromUser(id);
            }
        }

        //api/Concert/NumberGoingToConcert/{concertId},{userId} ||endret
        [HttpGet]
        [Route("api/Concert/GetFriendsAttendanceNumber/{concertId},{userId}")]
        public async Task<ConcertInfoClass> NumberGoingToConcert(int concertId, int userId)
        {
            using (var bmgr = ManagerFactory.GetConcertManager())
            {
                return await bmgr.NumberGoingToConcert(concertId, userId);
            }
        }

        //api/Concert/FindFriendsGoingToConcert/{concertId},{userId} || endret
        [HttpGet]
        [Route("api/Concert/GetAttendingFriends/{concertId},{userId}")]
        public async Task<List<FriendsClass>> FindFriendsGoingToConcert(int concertId, int userId)
        {
            using (var bmgr = ManagerFactory.GetConcertManager())
            {
                return await bmgr.FindFriendsGoingToConcert(concertId, userId);
            }
        }





        //POST

        
        //api/Concert/AddConcertToUser/{userId},{concertId},{ok} || endret || sjekk
        [HttpPost]
        [Route("api/Concert/AddToUserList/{userId},{concertId},{ok}")]
        public async Task<Boolean> AddConcertToUser(int userId, int concertId, bool ok)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.AddConcertToUser(userId, concertId, ok);
            }
        }
        
        //api/Concert/AcceptConcertRequest/{id},{ok} || endret || Sjekk
        [HttpPost]
        [Route("api/Concert/AcceptInvitation/{id},{ok}")]
        public async Task<bool> AcceptConcertInvitation(int id, Boolean ok)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.AcceptConcertRequest(ok, id);
            }
        }

        //api/Concert/AddConcertRequest/{fromUsr},{toUsr},{ConcertId} || endret || sjekk
        [HttpPost]
        [Route("api/Concert/SendInvitation/{from},{to},{concertid}")]
        public async Task<bool> AddConcertRequest(int from, int to, int concertid)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.AddConcertRequest(from, to, concertid);
            }
        }

        //api/Concert/AddConcert/ || endret || sjekk
        [HttpPost]
        [Route("api/Concert/Add/")]
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


        //----------------------------------------Metoder ikke funnet i appen--------------------

        //api/Concert/GetAttendingConcertTask/{cid},{uid}
        [HttpGet]
        [Route("api/Concert/GetAttending/{cid},{uid}")]
        public async Task<Boolean> GetAttendingConcertTask(int cid, int uid)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.GetAttendingConcerTask(cid, uid);
            }
        }


        //api/Concert/SetAttendingConcertTask/{cid},{uid},{ok}
        [HttpGet]
        [Route("api/Concert/SetAttending/{cid},{uid},{ok}")]
        public async Task<Boolean> SetAttendingConcertTask(int cid, int uid, Boolean ok)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.SetAttendingConcertTask(cid, uid, ok);
            }
        }

        //AddRemoveConcertToUser/{concertId},{userId},{ok} !!POST
        [HttpPost]
        [Route("api/Concert/AddRemoveConcertToUser/{concertId},{userId},{ok}")]
        public async Task<bool> AddRemoveConcertToUser(int concertId, int userId, bool ok)
        {
            using (var bmgr = ManagerFactory.GetConcertManager())
            {
                return await bmgr.AddRemoveConcertToUser(concertId, userId, ok);
            }
        }


        //api/Concert/GetConcertInfoWithFriends/{id}/{userid}

        [Route("api/Concert/GetFriendsAttending/{id}/{userid}")]
        public async Task<List<UserClass>> GetConcertInfoWithFriends(int id, int userid)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.GetFriendsGoingToConcertById(id, userid);
            }
        }


        //"api/Concert/ChangeConcert/{c},{pic}" || endret
        [HttpPost]
        [Route("api/Concert/UpdateConcert/{c},{pic}")]
        public async Task<Boolean> ChangeConcert(ConcertClass c, Byte[] pic)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.ChangeConcert(c, pic);
            }
        }


    }
}
