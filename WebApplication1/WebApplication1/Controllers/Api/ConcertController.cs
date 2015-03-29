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
        [Route("api/Concert/AddConcert/{c},{pic}")]
        public async Task<Boolean> AddConcert(ConcertClass c, Byte[] pic)
        {
            using (var cMgr = ManagerFactory.GetConcertManager())
            {
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



    }
}
