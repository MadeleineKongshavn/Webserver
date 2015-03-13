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
        public int Xcoordinates { get; set; }
        public int Ycoordinates { get; set; }
        public String Bandname { get; set; }
        public String Date { get; set; }
        public String Time { get; set; }
        public List<FriendsClass> Friends { get; set; } 


        public int FriendsAttending { get; set; }
        public Boolean Attending { get; set; }
        public String url { get; set; }
        public int Followers { get; set; }
        public Boolean SeeAttends { get; set; }
        public int BandId { get; set; }
        public String LinkToBand { get; set; }
    }
}