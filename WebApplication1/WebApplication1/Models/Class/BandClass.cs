using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using WebGrease;

namespace WebApplication1.Models
{
    public class BandClass
    {
        public int BandId { get; set; }
        public Double Xcoordinates { get; set; }
        public Double Ycoordinates { get; set; }
        public String BandName { get; set; }
        public string[] Genre { get; set; }
        public String Genres { get; set; }

        public String Member { get; set; }
        public String SmallBitmapUrl { get; set; }
        public String BitmapUrl { get; set; }
        public String SongName { get; set; }
        public String UrlFacebook { get; set; }
        public String UrlSoundCloud { get; set; }
        public String UrlRandom { get; set; }
        public String Area { get; set; }
        public BandClass()
        {
        }
    }
}