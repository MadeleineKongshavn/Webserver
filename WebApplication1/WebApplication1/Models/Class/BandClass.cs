using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class BandClass
    {
        public int BandId { get; set; }
        public Double Xcoordinates { get; set; }
        public Double Ycoordinates { get; set; }
        public String BandName { get; set; }
        public String About { get; set; }
        public int Followers { get; set; }
        public String Url { get; set; }
    }
}