using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using WebApplication1.Models.Class;

namespace WebApplication1.Models
{
    public class DbConcert
    {
        public async Task<Boolean> AddConcertToUser(int userId, int ConcertId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    db.ConcertFollowersDb.Add(new ConcertFollowers()
                    {
                        UserId = userId,
                        ConcertId = ConcertId,
                    });
                    db.SaveChanges();
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }
        // legger til en konsert, pic er bilde som skal inn
        public async Task<Boolean> AddConcert(ConcertClass c, Byte[] pic)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {

                    // er en SmallUrl og en Url så det mindre bildet som finnes i lister kan komprimeres mer.
                    // lagre pic i bitmapurl og bitmapsmallurl

                    var c1 = new Concert()
                    {
                        Title = c.Title,
                        Timestamp = DateTime.Now,
                        Xcoordinates = c.Xcoordinates,
                        Ycoordinates = c.Ycoordinates,
                        Area = c.Area,
                        Followers = 0,
                        SeeAttends = c.SeeAttends,
                        BandId = c.BandId,
                        LinkToBand = c.LinkToBand,
                        BitmapUrl = "stor url her :)",
                        BitmapSmalUrl = "small url her :)",
                        VenueName = c.VenueName,
                        Date = Convert.ToDateTime(c.Time),
                    };
                    db.ConcertDb.Add(c1);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        // endrer på en konsert med den gitte ideen inni ConcertClass
        public async Task<Boolean> ChangeConcert(ConcertClass c, Byte[] pic)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var c1 = (from con in db.ConcertDb
                              where con.ConcertId == c.ConcertId
                              select con).FirstOrDefault();

                    // lagre pic i bitmapurl og bitmapsmallurl

                    c1.Title = c.Title;
                    c1.Timestamp = DateTime.Now;
                    c1.Xcoordinates = c.Xcoordinates;
                    c1.Ycoordinates = c.Ycoordinates;
                    c1.Area = c.Area;
                    c1.SeeAttends = c.SeeAttends;
                    c1.BandId = c.BandId;
                    c1.LinkToBand = c.LinkToBand;
                    c1.BitmapUrl = "stor url her :)";
                    c1.BitmapSmalUrl = "small url her :)";
                    c1.VenueName = c.VenueName;
                    c1.Date = Convert.ToDateTime(c.Time);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }  
        }
        public async Task<List<ConcertClass>> FindAllConcertToUser(int id) // Find all concerts to a user 
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    //Mye bedre :)
                    var result = await (from v in db.ConcertFollowersDb
                                        where v.UserId == id
                                        join c in db.ConcertDb on v.ConcertId equals c.ConcertId
                                        select new ConcertClass()
                                        {
                                            ConcertId = c.ConcertId,
                                            Band = c.Band,
                                            Title = c.Title,
                                            Attending = true,
                                            Date = c.Date,
                                        }).ToListAsync();
                    return result;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public async Task<ConcertClass> GetConcertById(int id) // find one concert
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var concert = await (from v in db.ConcertDb
                                         where v.ConcertId == id
                                         select new ConcertClass()
                                         {
                                             ConcertId = v.ConcertId,
                                             Title = v.Title,
                                             Xcoordinates = v.Xcoordinates,
                                             Ycoordinates = v.Ycoordinates,
                                             Band = v.Band,
                                             Date = v.Date,
                                         }
                        ).FirstOrDefaultAsync();
                    return concert;

                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public async Task<List<UserClass>> FriendsGoingToConcert(int userId, int concertId) // Finds all your friends that are going to the same concert 
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {

                    //var f = (from v in db.FriendsDb
                    //             where v.UserId )
                    //var co = (from c in db.ConcertDb
                    //    where c.ConcertId == concertId
                    //    select c);

                    var cf = await (from fu in db.ConcertFollowersDb
                                    where fu.ConcertId == concertId && fu.User.Friends.Any(i =>  i.Friend == userId)
                                    //select fu 
                                    select new UserClass()
                                {
                                    Name = fu.User.ProfileName,
                                    Url = "?"
                                }
                            ).ToListAsync();
                    return cf;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}