using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Class
{
    public class ConcertClass
    {
        public int ConcertId { get; set; }
        public String Title { get; set; }
        public String Bandname { get; set; }
        public int FriendsAttending { get; set; }
        public Boolean Attending { get; set; }
        public String url { get; set; }
    }
}