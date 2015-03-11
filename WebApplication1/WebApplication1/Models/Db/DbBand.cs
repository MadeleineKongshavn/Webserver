using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class DbBand
    {
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
        public List<BandClass> FindAllBandsToUser(int id) // Find all bands to a user 
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    List<BandFollowers> allBands = (from v in db.BandFollowersDb where v.UserId == id select v).ToList();
                    List<Band> bandsTouser = allBands.Select(b => (from v in db.BandDb where v.BandId == b.BandId orderby v.BandName ascending select v).FirstOrDefault()).Where(bands => bands != null).ToList();

                    var band = new List<BandClass>();
                    if (bandsTouser.Count == 0) return band;



                    foreach (Band b in bandsTouser) // Loop through List with foreach.
                    {
                        BandClass bandClass = new BandClass();
                        bandClass.BandName = b.BandName;
                        bandClass.BandId = b.BandId;
                        bandClass.url = b.Url;
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