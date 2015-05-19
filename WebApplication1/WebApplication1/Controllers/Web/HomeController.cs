using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebApplication1.Controllers.Api;
using WebApplication1.Managers;
using WebApplication1.Models;
using WebApplication1.Models.Args;
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
            WwwLinkArgs args = new WwwLinkArgs();
            args.setId(8);
            args.setLink("http://semitone.azurewebsites.net/");


            var v = await new BandController().FindBandBasedOnQuery("b"); 

            ViewBag.Message = "Here's hoping!\nuSC: " + v.First().Member ;

            return View();
        }     
    }
}