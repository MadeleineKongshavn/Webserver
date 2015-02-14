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
        public String[] Genre { get; set; }
        public String url { get; set; }
    }
}