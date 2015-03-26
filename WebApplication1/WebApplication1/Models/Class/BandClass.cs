using System;
using System.Collections.Generic;
using System.Drawing;
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
        public Bitmap Bitmap { get; set; }
        public Bitmap SmallBitmap { get; set; }
        public Byte[] Song { get; set; }
        public String SongName { get; set; }
        public String UrlFacebook { get; set; }
        public String UrlSoundCloud { get; set; }
        public String UrlRandom { get; set; }
        public String Area { get; set; }
    }
}