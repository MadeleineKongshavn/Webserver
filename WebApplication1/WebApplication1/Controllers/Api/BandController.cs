using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
        public List<BandClass> FindAllBandsToUser(int id)
        {
            var db = new DbBand();
            return db.FindAllBandsToUser(id);
        }

    }
}
