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
        [Route("api/Band/FindAllBand/")]
        public List<Band> FindAllBand()
        {
            var db = new DbBand();
            return db.FindAllBand();
        }
        [HttpGet]
        [Route("api/Band/FindAllBandsToUser/{id}")]
        public List<BandClass> FindAllBandsToUser(int id)
        {
            var db = new DbBand();
            return db.FindAllBandsToUser(id);
        }
        [HttpGet]
        [Route("api/Band/FindBand/{id}")]
        public Band FindBand(String id)
        {
            var db = new DbBand();
            return db.FindBand((int.Parse(id)));
        }
        [HttpGet]
        [Route("api/Band/Count/{id}")]
        public int Count(int id)
        {
            var db = new DbBand();
            return db.Count(id);
        }
    }
}
