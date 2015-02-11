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
        public BandClass FindBand(String id)
        {
            var db = new DbBand();
            Band b = db.FindBand((int.Parse(id)));
            BandClass c = new BandClass();
            c.BandName = b.BandName;
            c.About = b.About;
            c.BandId = b.BandId;
            c.Followers = b.Followers;
            c.Url = b.Url;
            c.Xcoordinates = b.Xcoordinates;
            c.Ycoordinates = b.Ycoordinates;
            return c;
        }
        [HttpGet]
        [Route("api/Band/Count/{id}")]
        public int Count(int id)
        {
            var db = new DbBand();
            return db.Count(id);
        }
        [HttpGet]
        [Route("api/Band/GetFirstBand/")]
        public BandClass GetFirstBand()
        {
            var db = new DbBand();
            Band b = db.FindBand(1);
            BandClass c = new BandClass();
            c.BandName = b.BandName;
            c.About = b.About;
            c.BandId = b.BandId;
            c.Followers = b.Followers;
            c.Url = b.Url;
            c.Xcoordinates = b.Xcoordinates;
            c.Ycoordinates = b.Ycoordinates;
            return c;
        }


   /*     // GET api/band
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/band/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/band
        public void Post([FromBody]string value)
        {
        }

        // PUT api/band/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/band/5
        public void Delete(int id)
        {
        }*/
    }
}
