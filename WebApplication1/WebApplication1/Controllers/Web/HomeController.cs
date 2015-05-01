using System;
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
     /*
            int userid = 1;
            int bandid = 11;
            BandClass b;
            double x=0;
            double y=0;
            string bandname="jaffa cake revival";
            string refer = "CoQBdAAAACIg0nIvOsdxqJKbL3HffQaFUUVLvCLXqVwLeyNVPtlJvsFR1DFbUCeh2N-gu7dLMW50vIGaIrH-mzk0rInbuV5Twy7lphbZKH1O-V5o1CEf3Kr7lxBBYK8tAiJMcdsf6CFZ7m8M0VSmSTayEviqqoysiVKLhXZ8dJ6Wcj9WWRO_EhA3ny5p9aIA1aAeCjMTil_oGhRDVTJJdS2kGniFpCeobF4PifX1mA";
            char[] input = refer.ToCharArray();

            UserController uc=new UserController();*/
            DbBand bc = new DbBand();
            GenreController gc = new GenreController();


            Double v = bc.distance(59.941761, 10.760368, 59.941761, 10.757493);


            // var v = await new ConcertController().FindConcertBasedOnQuery("t");

           /// var i = await bc.GetBandsCoordinates(59.911449, 10.750401);
           /// 
           /// 
            /// 59,923302   10,752547           59,941761   10,760368

           List<BandsImagesClass> list = await bc.GetRandomBands(3);
           ViewBag.Message = "Here's hoping! " + list.Count + " " + ((int)v);

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

     
    }
}