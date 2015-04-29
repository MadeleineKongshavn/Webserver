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
    public class ConcertController : ApiController
    {
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
        [Route("api/Concert/AddConcertToUser/{userId},{concertId}")]
        public async Task<Boolean> AddConcertToUser(int userId, int concertId)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.AddConcertToUser(userId, concertId);
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
        [Route("api/Concert/AddConcert/{title},{bandId},{year},{month},{day},{hour},{min},{pic}")]
        public async Task<bool> AddConcert(String title,int bandId, int year, int month, int day, int hour, int min,  Byte[] pic)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                ConcertClass c = new ConcertClass();
                c.Title = title;
                c.BandId = bandId;
                c.Date = new DateTime(year, month, day, hour, min, 0);
                return await cMgr.AddConcert(c, pic);
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

        [Route("api/Concert/UpdateConcertLocation/{concertid},{venue},{ref}")]
        public async Task<bool> UpdateConcertLocation(int concertidea,string venue,string refr)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
                return await cMgr.updateConcertLocation(concertidea, venue, refr);
            }
        }


    }
}
