using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Class
{
    public class UserClass
    {
        public int UserId { get; set; }
        public String Name { get; set; }
        public String Url { get; set; }
        public Boolean SeeNotifications { get; set; }
        public Boolean Public { get; set; }
        public int Radius { get; set; }
        public Double Xcoordinates { get; set; }
        public Double Ycoordinates { get; set; }
    }
}