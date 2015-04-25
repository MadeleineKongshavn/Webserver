using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
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


            /*BandClass b = new BandClass();
            b.BandId = 21;
            b.BandName = "Pony down";
            b.SongName = "Last Trick";
            b.Xcoordinates = 0;
            b.Ycoordinates = 0;


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            String band = serializer.Serialize(b);*/

            
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
            BandController bc = new BandController();
            long x = (long)66.33; long y = (long)22.04; int id = 21; string area = "Oslo";
           
            String www = "new www"; String fb = "new fb"; String soundcloud = "new soundcloud";
            Boolean ok = await bc.UpdateBandLinks(id,www,fb,soundcloud);


            ViewBag.Message = " Oppdatering av området: "+ok;// + m.Count;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}