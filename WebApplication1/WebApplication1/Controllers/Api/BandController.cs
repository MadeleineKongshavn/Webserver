using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebApplication1.Managers;
using WebApplication1.Models;

namespace WebApplication1.Controllers.Api
{
    public class BandController : ApiController
    {
 
        [HttpGet]
        [Route("api/Band/FindBandBasedOnQuery/{query}")]
        public List<BandClass> FindBandBasedOnQuery(String query)
        {
            var db = new DbBand();
            return null;
            // return db.FindBandBasedOnQuery(query);
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

    }
}
