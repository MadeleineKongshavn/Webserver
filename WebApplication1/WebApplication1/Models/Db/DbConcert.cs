using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using WebApplication1.Models.Class;
namespace WebApplication1.Models
{
    /**
     * x og y coordinater i databasen er int, må endre;
     */

    public class DbConcert
    {
        public async Task<List<ConcertClass>> FindConcertBasedOnQuery(String query)
        {
            using (var db = new ApplicationDbContext())
            {
                var con = await (from c in db.ConcertDb
                    where c.Title.Contains(query)
                    select new ConcertClass()
                    {
                        Bandname = c.Band.BandName,
                        ConcertId = c.ConcertId,
                        Title = c.Title,
                        SmallBitmapUrl = c.BitmapSmalUrl,
                    }).ToListAsync();
                return con;
            }
        }
        public async Task<int> AcceptConcertRequest(Boolean ok, int id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    Notifications n = (from x in db.NotificationsDb where x.NotificationsId == id select x).FirstOrDefault();

                    int Usrid = n.UserId;
                    int concrtId = n.InviteConcertNotifications.ConcertId;
                    int friendUsr = n.InviteConcertNotifications.UserId;
                    
                    if (!ok)
                    {
                        db.InviteConcertNotificationsDb.Remove(n.InviteConcertNotifications);
                        db.NotificationsDb.Remove(n);
                        db.SaveChanges();
                        return Usrid;
                    
                    }
                    // 1 friends, 2 inviter concert, 3 bekreftelse
                    n.InviteConcertNotifications.Accepted = true;
                    n.Seen = false;
                    n.SendtTime = DateTime.Now;

                    Notifications conf = new Notifications()
                    {
                        SendtTime = DateTime.Now,
                        Seen = false,
                        UserId = friendUsr,
                        Type = 3,
                    };
                    db.NotificationsDb.Add(conf);
                    conf.AcceptConcertInvitation = new AcceptConcertInvitation()
                    {
                        UserId = Usrid,
                        ConcertId = concrtId,
                    };

                    AddConcertToUser(Usrid, concrtId);

                    db.SaveChanges();
                    return Usrid;
                }
            }
            catch (Exception)
            {
                return -1;
            }            
        }
        public async Task<bool> AddConcertRequest(int fromUsr, int toUsr, int ConcertId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    Notifications n = new Notifications()
                    {
                        SendtTime = DateTime.Now,
                        Seen = false,
                        Type = 2,
                        UserId = toUsr,
                    };
                    db.NotificationsDb.Add(n);
                    n.InviteConcertNotifications = new InviteConcertNotifications()
                    {
                        UserId = fromUsr,
                        ConcertId = ConcertId,
                        Accepted = false,
                    };
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {    
                return false;
            }            
        }
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
        public async Task <Boolean> ConcertAlreadyAdded(int concertId, int UserId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var concert =
                        (from v in db.ConcertFollowersDb where v.ConcertId == concertId && v.UserId == UserId select v)
                            .FirstOrDefaultAsync();
                    if (concert == null) return false;
                    return true;

                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        // legger til en konsert, pic er bilde som skal inn
        public async Task<int> AddConcert(ConcertClass c)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var c1 = new Concert()
                    {
                        Title = c.Title,
                        Xcoordinates = c.Xcoordinates,
                        Ycoordinates = c.Ycoordinates,
                        BandId = c.BandId,
                        BitmapUrl = c.BitmapUrl,
                        BitmapSmalUrl = c.SmallBitmapUrl,
                        VenueName = c.VenueName,
                        Date = Convert.ToDateTime(c.Date),
                    };
                    db.ConcertDb.Add(c1);
                    await db.SaveChangesAsync();
                    return c.ConcertId;
                }
            }
            catch (Exception e)
            {
                return -1;
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
                    c1.Xcoordinates = c.Xcoordinates;
                    c1.Ycoordinates = c.Ycoordinates;
                    c1.BandId = c.BandId;
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
                                            SmallBitmapUrl = c.BitmapSmalUrl,
                                            ConcertId = c.ConcertId,
                                            Bandname = c.Band.BandName,
                                            Title = c.Title,
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
        public async Task<Boolean> GetAttendingConcerTask(int cid, int uid)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {

                    var v  = (from f in db.ConcertFollowersDb where f.UserId == uid && f.ConcertId == cid select f)
                                .FirstOrDefaultAsync();
                    if (v != null) return true;
                    else return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<Boolean> SetAttendingConcertTask(int cid, int uid, Boolean ok)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {

                    if (ok)
                    {
                        db.ConcertFollowersDb.Add(new ConcertFollowers()
                        {
                            ConcertId = cid,
                            UserId = uid,
                        });
                    }
                    else
                    {
                        ConcertFollowers c = await
                            (from f in db.ConcertFollowersDb where f.UserId == uid && f.ConcertId == cid select f)
                                .FirstOrDefaultAsync();
                        db.ConcertFollowersDb.Remove(c);
                    }
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateConcertLocation(int concertid, string area, double x, double y)
        {
            if (concertid == 0 || area == null)
                return false;
            try
            {
                using (var db = new ApplicationDbContext())
                {

                    var concert = (from c in db.ConcertDb
                                where c.ConcertId == concertid
                                select c).FirstOrDefault();
                   concert.Xcoordinates = x;
                   concert.Ycoordinates = y;
                    db.SaveChanges();
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}