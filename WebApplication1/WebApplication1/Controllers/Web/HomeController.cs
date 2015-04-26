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
          /*  UserController u = new UserController();
            UserClass us = new UserClass()
            {
                UserId = 1,
                Name = "nelly lindhaugensds",
                SeeNotifications = true,
                Public = true,
                Radius = 500,
                Xcoordinates = 34.00,
                Ycoordinates = 23.00,
                Url = "url",
                      
            };
            u.ChangeUser(us); */
            //new FriendsController().SetFriendAccept(2, true);


          /*  ConcertClass c = new ConcertClass();
            c.ConcertId = 1;
            c.Time = "09.11.2014 09:00";
            c.Title = "Purple band tonight brave";
            c.LinkToBand = "link to band";
            c.SeeAttends = true;
            c.VenueName = "rockefeller";
            c.Xcoordinates = 0;
            c.Ycoordinates = 0;
            c.Area = "area her";
            c.BandId = 3;


            new ConcertController().AddConcertToUser(1, 1);*/


        /*    BandClass b = new BandClass();
            b.BandId = 21;
            b.BandName = "Pony down";
            b.SongName = "Last Trick";
            b.Xcoordinates = 0;
            b.Ycoordinates = 0;


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            String band = serializer.Serialize(b);*/



        //    var k = await new DbFriends().CancelFriendTask(1,2);

            
           // List <MemberClass> m = await new BandController().GetAllAdminBands(2);
            //new ConcertController().AcceptConcertRequest(true, 28);

            
            

          //  new ConcertController().AddConcertRequest(2, 1, 1);
            //new BandController().AddBandToUser(1, 16);
           // var b = await new BandController().FindAllBandsToUser(1);


          //  var ba = await new DbBand().CompressBitmap();


            //List<BandsImagesClass> v = await new BandController().GetRandomBands(1);
            //BandsImagesClass c = v.FirstOrDefault();

           //Boolean ok =  await new ConcertController(). GetAttendingConcerTask(2, 1);
        //   Boolean ok = await new ConcertController().SetAttendingConcertTask(2, 1, false);
            
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

            new BandController().updateBandLocation(id, "area", refer);


            ViewBag.Message = "Here's hoping!";// + m.Count;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}