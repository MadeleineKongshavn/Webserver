using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.SqlServer.Server;

namespace WebApplication1.Models
{
    public class DbBand
    {
        public Boolean AddBand(BandClass band)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    System.Net.WebRequest request = System.Net.WebRequest.Create("http://assets.rollingstone.com/assets/2015/article/mumford-sons-talk-going-electric-on-new-album-wilder-mind-20150302/187700/medium_rect/1425301454/720x405-Mumford-&-Sons-Press-Shot-2nd-March-(1).jpg");
                    System.Net.WebResponse response = request.GetResponse();
                    System.IO.Stream responseStream =  response.GetResponseStream();
                    var bitmap2 = new Bitmap(responseStream);

                    Band b = new Band();
                    b.Area = "32. street avenue";
                    b.BandName = "Mumford and sons";
                    b.Followers = 0;
                    b.Song = new byte[0];
                    b.SongName = " ";
                    b.Timestamp = DateTime.Now;
                    b.UrlFacebook = "http://semitone.azurewebsites.net/";
                    b.UrlRandom = "http://semitone.azurewebsites.net/";
                    b.UrlSoundCloud = "http://semitone.azurewebsites.net/";
                    b.Xcoordinates = 0.0;
                    b.Ycoordinates = 0.0;

                    MemoryStream ms = new MemoryStream();
                    bitmap2.Save(ms, ImageFormat.Bmp);
                    b.SmallBitmap = ms.ToArray();
                    b.Bitmap = ms.ToArray();
                    db.BandDb.Add(b);
                    db.SaveChanges();


                /*    Bitmap bmp;
                    using (var mse = new MemoryStream(b.Bitmap))
                    {
                        bmp = new Bitmap(mse);
                    }
                    if (bmp == null) return false;*/
                    return true;

                }
            }
            catch (Exception)
            {
                return false;
            }
            
        }
        public List<Band> FindAllBand() // find every band that exist 
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    List<Band> band = (from v in db.BandDb select v).ToList();
                    return band;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public String FindBandBasedOnQuery(String query) // finds bands based on the query parameter. 
        {
          try
          {
                using (var db = new ApplicationDbContext())
                {
                    query = query.Trim();
                    List<Band> allBands = (from b in db.BandDb where b.BandName.Contains(query) select b).ToList();


                    allBands.OrderBy(b => b.BandName).ToList();
                    List<BandClass> bands = new List<BandClass>();
                    foreach (var b in allBands)
                    {
                        BandClass newB = new BandClass();
                        newB.Xcoordinates = b.Xcoordinates;
                        newB.Ycoordinates = b.Ycoordinates;
                        newB.BandName = b.BandName;
                      //  newB.url = b.Url;
                        newB.BandId = b.BandId;    
                        bands.Add(newB);
                        
                    }
                    return "funket ikke";
                }

          }
            catch (Exception e)
            {
                return e.Message + " " + e.Source + " " + e.InnerException;
                // return new List<BandClass>();   
            }            
        }
        public List<BandClass> FindAllBandsToUser(int id) // Find all bands to a user 
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    List<BandFollowers> allBands = (from v in db.BandFollowersDb where v.UserId == id select v).ToList();
                    List<Band> bandsTouser = allBands.Select(b => (from v in db.BandDb where v.BandId == b.BandId select v).FirstOrDefault()).Where(bands => bands != null).ToList();

                    bandsTouser = bandsTouser.OrderBy(z => z.BandName).ToList();

                    var band = new List<BandClass>();
                    if (bandsTouser.Count == 0) return band;

                    foreach (Band b in bandsTouser) // Loop through List with foreach.
                    {
                        BandClass bandClass = new BandClass();
                        bandClass.BandName = b.BandName;
                        bandClass.BandId = b.BandId;
                        //bandClass.url = b.Url;
                        bandClass.Xcoordinates = b.Xcoordinates;
                        bandClass.Ycoordinates = b.Ycoordinates;

                        List<String> genreList = new List<string>();
                        List<BandGenre> list = b.BandGenre;
                        foreach (BandGenre genre in list)
                        {
                            genreList.Add((genre.Genre).GenreName);
                        }
                        bandClass.Genre = genreList.ToArray();
                        band.Add(bandClass);
                    }              
                    return band;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public Band FindBand(int id) // Finds a band
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    Band band = (from b in db.BandDb where b.BandId == id select b).FirstOrDefault();
                    return band;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public int Count(int id) // finds number of followers to this band
        {
            try
            {
                return (FindBand(id)).Followers;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}