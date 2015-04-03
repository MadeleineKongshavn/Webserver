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
        public int[] Member { get; set; }
        public int BandId { get; set; }
        public Double Xcoordinates { get; set; }
        public Double Ycoordinates { get; set; }
        public String BandName { get; set; }
        public string[] Genre { get; set; }

        public List<BandGenre> BandGenre
        {
            set
            {
                List<string> temp = new List<string>();
                foreach (var v in value)
                {
                    temp.Add(v.Genre.GenreName);
                }
                
                Genre = temp.ToArray();

            }
        }
        public String SmallBitmapUrl { get; set; }
        public String BitmapUrl { get; set; }

        public Byte[] Song { get; set; }
        public String SongName { get; set; }
        public String UrlFacebook { get; set; }
        public String UrlSoundCloud { get; set; }
        public String UrlRandom { get; set; }
        public String Area { get; set; }


        public BandClass(int bandId, double xcoordinates, double ycoordinates, string bandName, string songName, string urlFacebook, string urlSoundCloud, string urlRandom, string area)
        {
            BandId = bandId;
            Xcoordinates = xcoordinates;
            Ycoordinates = ycoordinates;
            BandName = bandName;
            SongName = songName;
            UrlFacebook = urlFacebook;
            UrlSoundCloud = urlSoundCloud;
            UrlRandom = urlRandom;
            Area = area;
        }

        public BandClass(Band b)
        {
            BandId = b.BandId;
            Xcoordinates = b.Xcoordinates;
            Ycoordinates = b.Ycoordinates;
            BandName = b.BandName;
            //Genre = b.BandGenre,??
            //SmallBitmap = b.SmallBitmap,
            //Song = b.Song,
            UrlFacebook = b.UrlFacebook;
            UrlRandom = b.UrlRandom;
            UrlSoundCloud = b.UrlRandom;
        }

        public BandClass()
        {
        }
    }

    public static class BandConvert
    {
        public static BandClass ConvertToBandClass(this Band b)
        {
            BandClass band = new BandClass()
            {
                BandId = b.BandId,
                Xcoordinates = b.Xcoordinates,
                Ycoordinates = b.Ycoordinates,
                BandName = b.BandName,
                //Genre = b.BandGenre,?
                //SmallBitmap = b.SmallBitmap,
                //Song = b.Song,
                UrlFacebook = b.UrlFacebook,
                UrlRandom = b.UrlRandom,
                UrlSoundCloud = b.UrlRandom,
                Area = b.Area
            };
            return band;
        }
    }
}