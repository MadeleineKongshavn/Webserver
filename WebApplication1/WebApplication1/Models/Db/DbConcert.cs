using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.Ajax.Utilities;
using WebApplication1.Models;
using WebApplication1.Models.Class;
namespace WebApplication1.Models
{
  
    public class DbConcert
    {
        private static double GetRad(double x)
        {
            return x * Math.PI / 180;
        }
        public Double Distance(double lat1, double lon1, double lat2, double lon2)
        {
            int RADIUS_EARTH = 6371;

            double dLat = GetRad(lat2 - lat1);
            double dLong = GetRad(lon2 - lon1);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(GetRad(lat1)) *
                Math.Cos(GetRad(lat2)) * Math.Sin(dLong / 2) * Math.Sin(dLong / 2);


            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return ((RADIUS_EARTH * c) * 1000);
        }
        public async Task<List<BandsImagesClass>> GetRandomConcert(int userId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var user = (from u in db.UserDb where u.UserId == userId select u).FirstOrDefault();
                    Double lat = user.Xcoordinates;
                    Double lang = user.Ycoordinates;
                    int rad = user.Radius;

                    var val = await (from c in db.ConcertDb
                                     select new BandsImagesClass()
                                     {
                                         OpositeXCoordinates = lat,
                                         OpositeYCoordinates = lang,
                                         BandId = c.BandId,
                                         Title = c.Title,
                                         UnderTitle = c.Band.BandName,
                                         SmallBitmapUrl = c.BitmapSmalUrl,
                                         XCoordinates = c.Xcoordinates,
                                         YCoordinates = c.Ycoordinates,
                                     }).ToListAsync();
                    List<BandsImagesClass> images = new List<BandsImagesClass>();
                    foreach (var v in val)
                    {
                        Double vals = Distance(lat, lang, v.XCoordinates, v.YCoordinates);
                        if (rad >= ((int)vals)) images.Add(v);
                    }
                    IList<BandsImagesClass> t = await Shuffle<BandsImagesClass>(images);
                    return t.Take(15).ToList();
                }
            }
            catch (Exception e)
            {
                return null;
            }
            
        }
        public async Task<IList<T>> Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
            return list;
        }
        public async Task<bool> AddRemoveConcertToUser(int concertId, int userId, bool ok)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    if (ok)
                    {
                        db.ConcertFollowersDb.Add(new ConcertFollowers()
                        {
                            UserId = userId,
                            ConcertId = concertId,
                        });
                    }
                    else
                    {
                        var con =
                            await
                                (from u in db.ConcertFollowersDb
                                    where u.UserId == userId && u.ConcertId == concertId
                                    select u).FirstOrDefaultAsync();
                        db.ConcertFollowersDb.Remove(con);
                    }
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            
        }

        public async Task<ConcertInfoClass> NumberGoingToConcert(int concertId, int userId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var c = await (from friend in db.FriendsDb
                                   where friend.UserId == userId
                                   join con in db.ConcertFollowersDb on friend.Friend equals con.UserId select friend).ToListAsync();
                    ConcertInfoClass co =  new ConcertInfoClass()
                    {
                        ConcertId = concertId,
                        FriendsAttending = c.Count,
                        Added = await ConcertAlreadyAdded(concertId, userId),
                    };
                    return co;

                }
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<FriendsClass>> FindFriendsGoingToConcert(int concertId, int userId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var c = await (from friend in db.FriendsDb
                        where friend.UserId == userId
                        join con in db.ConcertFollowersDb on friend.Friend equals con.UserId
                        select new FriendsClass()
                        {
                            FriendsId = friend.Friend,
                            Friendsname =
                                (from d in db.UserDb where d.UserId == friend.Friend select d.ProfileName)
                                    .FirstOrDefault(),
                            url = (from d in db.UserDb where d.UserId == friend.Friend select d.Url).FirstOrDefault(),
                        }).ToListAsync();
                    return c;
                }
            }catch (Exception e)
            {
                return null;
            }
        }
        public async Task<List<ConcertClass>> FindConcertBasedOnQuery(String query)
        {
            try
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
            catch (Exception e)
            {
               ConcertClass c = new ConcertClass(){Title = e.Message + " " + e.ToString()};
                var v = new List<ConcertClass>();
                v.Add(c);
                return v;
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
                    n.SendtTime = DateTime.UtcNow.AddHours(2);

                    Notifications conf = new Notifications()
                    {
                        SendtTime =  DateTime.UtcNow.AddHours(2),
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

                    AddConcertToUser(Usrid, concrtId, true);

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
                        SendtTime = DateTime.UtcNow.AddHours(2),
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
        public async Task<bool> AddConcertToUser(int userId, int ConcertId, bool ok)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    if (ok)
                    {
                        db.ConcertFollowersDb.Add(new ConcertFollowers()
                        {
                            UserId = userId,
                            ConcertId = ConcertId,
                        });
                        db.SaveChanges();
                    }
                    else
                    {
                        var v =
                            await
                                (from o in db.ConcertFollowersDb
                                    where userId == o.UserId && ConcertId == o.ConcertId
                                    select o).FirstOrDefaultAsync();
                        db.ConcertFollowersDb.Remove(v);
                        db.SaveChanges();
                    }
                    return true;
                }

            }
            catch (Exception e)
            {
                return false;
                // return e.Message + " " + e.ToString() + " feil";
            }
        }
        public async Task <Boolean> ConcertAlreadyAdded(int concertId, int UserId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var concert = await (from v in db.ConcertFollowersDb where v.ConcertId == concertId && v.UserId == UserId select v).FirstOrDefaultAsync();
                    if (concert == null) return false; else  return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        // legger til en konsert, pic er bilde som skal inn
        public async Task<Boolean> AddConcert(ConcertClass c, Boolean getBitmapUrl)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var BandBitmapUrl = "";
                    var SmallBandBitmapUrl = "";
                    if(getBitmapUrl)
                    {
                        BandBitmapUrl = (from b in db.BandDb where b.BandId == c.BandId select b.BitmapUrl).FirstOrDefault();
                        SmallBandBitmapUrl = (from b in db.BandDb where b.BandId == c.BandId select b.BitmapSmalUrl).FirstOrDefault();
                    }
                    
                    var c1 = new Concert()
                    {
                        Title = c.Title,
                        Xcoordinates = c.Xcoordinates,
                        Ycoordinates = c.Ycoordinates,
                        BandId = c.BandId,
                        BitmapUrl = BandBitmapUrl,
                        BitmapSmalUrl = SmallBandBitmapUrl,
                        VenueName = c.VenueName,
                        Date = Convert.ToDateTime(c.Date),
                    };
                    db.ConcertDb.Add(c1);
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception e)
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
                                             VenueName = v.VenueName,
                                             ConcertId = v.ConcertId,
                                             Title = v.Title,
                                             Xcoordinates = v.Xcoordinates,
                                             Ycoordinates = v.Ycoordinates,
                                             Bandname = v.Band.BandName,
                                             Date = v.Date,
                                             BitmapUrl = v.BitmapUrl,
                                             SmallBitmapUrl = v.BitmapSmalUrl
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