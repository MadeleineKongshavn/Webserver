using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Controllers.Api;
using WebApplication1.Models;
using WebApplication1.Models.Class;

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
            BandController b = new BandController();

            b.updateBandName("newName",20);
            
            //ViewBag.Message = " your count name"  + "er informasjonen";// + m.Count;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}