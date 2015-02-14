using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Class
{
    public class ConcertClass
    {
        public int ConcertId { get; set; }
        public String Title { get; set; }
        public String Time { get; set; }
        public String Date { get; set; }
        public double Xcoordinates { get; set; }
        public double Ycoordinates { get; set; }
        public int Followers { get; set; }
        public bool SeeAttends { get; set; }
        public String Url { get; set; }
        public int BandId { get; set; }
        public String LinkToBand { get; set; }
    }
}