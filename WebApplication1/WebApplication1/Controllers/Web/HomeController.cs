﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApplication1.Controllers.Api;
using WebApplication1.Models;
using WebApplication1.Models.Class;
using WebApplication1.Models.Db;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> About()
        {


            
            int id=22;


      /*      Boolean ok = await new BandController().updateBandName("butterscotch topnotch", id);
            StringBuilder builder = new StringBuilder("BandNameUpdate: ");
            builder.Append(ok);
            builder.Append("\n");

            ok=await new BandController().updateBandLocation(id,"Setton",(long)44.8888,(long)77.918274);
            builder.Append("BandLocationUpdate: ");
            builder.Append(ok);
            builder.Append("\n");

            ok = await new BandController().updateBandLinks(id, "her er www", "da må jo dette være fb", "og soundcloud har det bra");
            builder.Append("BandLinksUpdate: ");
            builder.Append(ok);
            builder.Append("\n");*/

            String refer = "CoQBdAAAACIg0nIvOsdxqJKbL3HffQaFUUVLvCLXqVwLeyNVPtlJvsFR1DFbUCeh2N-gu7dLMW50vIGaIrH-mzk0rInbuV5Twy7lphbZKH1O-V5o1CEf3Kr7lxBBYK8tAiJMcdsf6CFZ7m8M0VSmSTayEviqqoysiVKLhXZ8dJ6Wcj9WWRO_EhA3ny5p9aIA1aAeCjMTil_oGhRDVTJJdS2kGniFpCeobF4PifX1mA";
            char[] input = refer.ToCharArray();
            new BandController().updateBandLocation(id,"HHuttiHeita",input);

            List<BandClass> l =  await new BandController().FindBandBasedOnQuery("h");
            ViewBag.Message = "Here's hoping! " + l.Count;// + m.Count;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}