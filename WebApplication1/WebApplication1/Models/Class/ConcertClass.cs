using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Class
{
    public class ConcertClass
    {
        public ConcertClass(Concert concert)
        {
            ConcertId = concert.ConcertId;
        }

        public ConcertClass()
        {

        }
        public int ConcertId { get; set; }
        public string Title { get; set; }
        public double Xcoordinates { get; set; }
        public double Ycoordinates { get; set; }
        public string Bandname { get; set; }//Flyttet inn i band class
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public List<FriendsClass> Friends { get; set; } 

        public BandClass BandClass { get; set; }

        public Band Band
        {
            set { BandClass = value.ConvertToBandClass(); }
        }

        public Boolean Attending { get; set; }
        public String SmallBitmapUrl { get; set; }
        public String BitmapUrl { get; set; }
        public int Followers { get; set; }
        public int BandId { get; set; }
        public String VenueName { get; set; }
        public String Area { get; set; }


    }
}