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
        public List<Band> FindAllBandsToUser(int id) // Find all bands to a user 
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    List<BandFollowers> allBands = (from v in db.BandFollowersDb where v.UserId == id select v).ToList();
                    List<Band> bandsTouser = allBands.Select(b => (from v in db.BandDb where v.BandId == b.BandId select v).FirstOrDefault()).Where(bands => bands != null).ToList();

                    if (bandsTouser.Count == 0) return null;
                    return bandsTouser;
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