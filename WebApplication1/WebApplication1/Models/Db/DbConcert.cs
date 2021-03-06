﻿using System;
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
        public async Task<List<ImageClass>> GetRandomConcert(int userId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var user = (from u in db.UserDb where u.UserId == userId select u).FirstOrDefault();
                    Double lat = user.Xcoordinates;
                    Double lang = user.Ycoordinates;
                    int rad = user.Radius;
                    var BandGenre = db.BandGenreDb;
                    var UserGenre = from usr in db.UserGenreDb
                                    where usr.UserId == userId
                                    select usr;
                    var Concerts = db.ConcertDb;
                    var prefUser = (from e in BandGenre
                                 join d in UserGenre on e.GenreId equals d.GenreId
                                 join b in Concerts on e.BandId equals b.BandId
                                 select b);

                    var concertlist = prefUser.Distinct();
                    if (concertlist.Count() == 0)
                        concertlist = Concerts;

                    var val = await (from c in concertlist
                                     select new ImageClass()
                                     {
                                         OpositeXCoordinates = lat,
                                         OpositeYCoordinates = lang,
                                         BandId = c.ConcertId,
                                         Title = c.Title,
                                         UnderTitle = c.Band.BandName,
                                         SmallBitmapUrl = c.Band.BitmapSmalUrl,
                                         XCoordinates = c.Xcoordinates,
                                         YCoordinates = c.Ycoordinates,
                                     }).ToListAsync();
                    List<ImageClass> images = new List<ImageClass>();
                    foreach (var v in val)
                    {
                        Double vals = Distance(lat, lang, v.XCoordinates, v.YCoordinates);
                        if (rad >= ((int)vals)) images.Add(v);
                    }
                    IList<ImageClass> t = await Shuffle<ImageClass>(images);
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
                                   join con in db.ConcertFollowersDb on friend.Friend equals con.UserId select con).ToListAsync();
                    int n = 0;
                    foreach (var fe in c)
                    {
                        if (fe.ConcertId == concertId) n++;
                    } 
                    ConcertInfoClass co =  new ConcertInfoClass()
                    {
                        ConcertId = concertId,
                        FriendsAttending = n,
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

        public async Task<List<ConcertClass>> FindConcertWithName(String query)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var con = await (from c in db.ConcertDb
                        where c.Title.Contains(query)
                        select new ConcertClass()
                        {
                            Date = c.Date,
                            Bandname = c.Band.BandName,
                            ConcertId = c.ConcertId,
                            Title = c.Title,
                            SmallBitmapUrl = c.Band.BitmapSmalUrl,
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
        public async Task<List<ConcertClass>> FindConcertWithDate(String query,int userid)
        {
            List<ConcertClass> concerts = new List<ConcertClass>();
            List<ConcertClass> inArea = new List<ConcertClass>();
            User usr=null;
            
            string[] date = query.Split('.');
            if (date.Length == 1)
            {
                if (date[0].Equals(""))
                    return concerts;
                if (date.Length == 1)
                    concerts = await FindDay(date[0]);
            }
            else if (date.Length == 2)
                concerts = await FindDayMonth(date);
            else if (date.Length == 3)
                concerts = await FindDayMonthYear(date);

            if (userid > 0)
            {
                usr = getUserLocation(userid);

                if (usr != null)
                {
                    foreach (var concert in concerts)
                    {
                        Double vals = Distance(usr.Xcoordinates, usr.Ycoordinates, concert.Xcoordinates, concert.Ycoordinates);
                        if (usr.Radius >= ((int)vals)) inArea.Add(concert);
                    }

                    if (inArea.Count > 0)
                    {
                        if (inArea.Count > 5)
                            concerts = sortOnUserGenre(usr.UserId, inArea);
                        else
                            concerts = inArea;
                    }
                }
            }
            return concerts;       
        }
        private List<ConcertClass> sortOnUserGenre(int userid, List<ConcertClass> concertlist){

            List<ConcertClass> withGenre = new List<ConcertClass>();
            using (var db = new ApplicationDbContext())
            {
                var BandGenre = db.BandGenreDb;
                var UserGenre = from usr in db.UserGenreDb
                                where usr.UserId == userid
                                select usr;
                var Concert = db.ConcertDb;
                var prefUser = (from band in BandGenre
                                join user in UserGenre on band.GenreId equals user.GenreId
                                join concert in Concert on band.BandId equals concert.BandId
                                select concert.ConcertId);
                int[] ids = prefUser.ToArray();
                withGenre = concertlist.Where(x => ids.Contains(x.ConcertId)).ToList();
            }
            if (withGenre.Count > 0) return withGenre; else return concertlist;
        }
        private User getUserLocation(int userid)
        {
            User usr = null;
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var v =(from u in db.UserDb where u.UserId == userid select u).FirstOrDefault();
                    usr = v;
                }  
            }
            catch (Exception ex)
            {
                usr = null;
            }
            return usr;
        }
        private async Task<List<ConcertClass>> FindDay(string day)
        {
            List<ConcertClass> concerts = new List<ConcertClass>();

            int dayInt;
            bool isDate = int.TryParse(day, out dayInt) && dayInt>0 && dayInt<32;
            if (!isDate)
                return concerts;

            DateTime date = new DateTime(2200, 12, dayInt);

            try
            {
                using (var db=new ApplicationDbContext()){

                  concerts= await
                      (from c in db.ConcertDb
                           where c.Date.Day==date.Day
                           select new ConcertClass()
                        {
                            Date = c.Date,
                            Bandname = c.Band.BandName,
                            ConcertId = c.ConcertId,
                            Title = c.Title,
                            SmallBitmapUrl = c.Band.BitmapSmalUrl,
                            Xcoordinates=c.Xcoordinates,
                            Ycoordinates=c.Ycoordinates
                        }).ToListAsync(); 
                }
            }catch(Exception){
            }
            return concerts;
        }
        private async Task<List<ConcertClass>> FindDayMonth(string[] date)
        {
            List<ConcertClass> concerts = new List<ConcertClass>();

            int dayInt, monthInt;
            bool isDay = int.TryParse(date[0], out dayInt) && dayInt > 0 && dayInt < 32;
            bool isMonth = int.TryParse(date[1], out monthInt) && monthInt>0 && monthInt<13;
            if (isMonth && isDay && monthInt == 2)
                if (dayInt > 28)isDay = false;
            if (!isDay || !isMonth) return concerts;
            DateTime dateTime = new DateTime(2015, monthInt, dayInt);
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    concerts = await
                        (from c in db.ConcertDb
                         where c.Date.Day == dateTime.Day && c.Date.Month==dateTime.Month
                         select new ConcertClass()
                         {
                             Date = c.Date,
                             Bandname = c.Band.BandName,
                             ConcertId = c.ConcertId,
                             Title = c.Title,
                             SmallBitmapUrl = c.Band.BitmapSmalUrl,
                         }).ToListAsync();
                }
            }
            catch (Exception)
            {
            }
            return concerts;
        }
        private async Task<List<ConcertClass>> FindDayMonthYear(string[] date)
        {
            List<ConcertClass> concerts = new List<ConcertClass>();
            String year = date[2];
            if (year.Length == 2) year = "20" + year;
            int dayInt, monthInt,yearInt;
            bool isDay = int.TryParse(date[0], out dayInt) && dayInt>0 && dayInt<32; 
            bool isMonth = int.TryParse(date[1], out monthInt) && monthInt > 0 && monthInt < 13;
            if (isMonth && isDay && monthInt == 2)
                if (dayInt > 28) isDay = false;
            bool isYear = int.TryParse(year, out yearInt);
            if (!isDay || !isMonth || !isYear) return concerts;
            DateTime dateTime = new DateTime(yearInt, monthInt, dayInt);

            try
            {
                using (var db = new ApplicationDbContext())
                {

                    concerts = await
                        (from c in db.ConcertDb
                         where c.Date.Day == dateTime.Day && c.Date.Month==dateTime.Month && c.Date.Year==dateTime.Year
                         select new ConcertClass()
                         {
                             Date = c.Date,
                             Bandname = c.Band.BandName,
                             ConcertId = c.ConcertId,
                             Title = c.Title,
                             SmallBitmapUrl = c.Band.BitmapSmalUrl,
                         }).ToListAsync();
                }
            }
            catch (Exception)
            {
            }
            return concerts;
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
            }
        }
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
        public async Task<List<ConcertClass>> FindAllConcertToUser(int id) 
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    var result = await (from v in db.ConcertFollowersDb
                                        where v.UserId == id
                                        join c in db.ConcertDb on v.ConcertId equals c.ConcertId
                                        select new ConcertClass()
                                        {
                                            SmallBitmapUrl = c.Band.BitmapSmalUrl,
                                            ConcertId = c.ConcertId,
                                            Bandname = c.Band.BandName,
                                            Title = c.Title,
                                            Date = c.Date,
                                        }).OrderByDescending(u => u.Date).ToListAsync();
                    return result;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public async Task<ConcertClass> GetConcertById(int id) 
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
                                             BitmapUrl = v.Band.BitmapUrl,
                                             SmallBitmapUrl = v.Band.BitmapSmalUrl
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
        public async Task<Boolean> ConcertAlreadyAdded(int concertId, int UserId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var concert = await (from v in db.ConcertFollowersDb where v.ConcertId == concertId && v.UserId == UserId select v).FirstOrDefaultAsync();
                    if (concert == null) return false; else return true;
                }
            }
            catch (Exception e)
            {
                return false;
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

    }
}