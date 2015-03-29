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
        public int Xcoordinates { get; set; }
        public int Ycoordinates { get; set; }
        public string Bandname { get; set; }//Flyttet inn i band class
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public List<FriendsClass> Friends { get; set; } 

        public BandClass BandClass { get; set; }

        public Band Band
        {
            set { BandClass = value.ConvertToBandClass(); }
        }

        public int FriendsAttending { get; set; }
        public Boolean Attending { get; set; }
        public string url { get; set; }
        public int Followers { get; set; }
        public Boolean SeeAttends { get; set; }
        public int BandId { get; set; }
        public string LinkToBand { get; set; }
        public String VenueName { get; set; }
        public String Area { get; set; }


    }
}