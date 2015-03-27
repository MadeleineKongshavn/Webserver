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

        public async Task<List<ConcertClass>> FindAllConcertToUser(int id) // Find all concerts to a user 
        {
            using (var db = new ApplicationDbContext())
            {
                //var Concert = new List<ConcertClass>();
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


                    //Console.WriteLine(result);

                    //List<ConcertFollowers> alleConcertId = (from v in db.ConcertFollowersDb where v.UserId == id select v).ToList();
                    //List<Concert> myConcerts = alleConcertId.Select(concert => (from v in db.ConcertDb where v.ConcertId == concert.ConcertId select v).FirstOrDefault()).Where(c => c != null).ToList();
                    ////List<ConcertClass> allConcertClasses = (from )
                    //foreach (Concert c in myConcerts) // Loop through List with foreach.
                    //{
                    //    ConcertClass con = new ConcertClass();
                    //    con.ConcertId = c.ConcertId;
                    //    con.Title = c.Title;
                    //    //  con.url = c.Url;
                    //    con.Bandname = c.Band.BandName;
                    //    //con.Date = c.Date.ToLongDateString();

                    //    // con.Date = c.Date;

                    //    con.Attending = false; // må endres!!! 
                    //    con.FriendsAttending = 5; // må endres!!!                               
                    //    Concert.Add(con);
                    //}
                    return result;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        //public async Task<ConcertClass> GetConcertinfo(int id, int userId) //Get all information about one concert
        //{
        //    using (var db = new ApplicationDbContext())
        //    {
        //        try
        //        {
        //            var c = await GetConcert(id);
        //            ConcertClass con = new ConcertClass();
        //            con.ConcertId = c.ConcertId;
        //            con.Title = c.Title;
        //            con.Xcoordinates = c.Xcoordinates;
        //            con.Ycoordinates = c.Ycoordinates;
        //            // con.Bandname = c.Band.BandName;
        //            //con.Date = c.Date.ToLongDateString();
        //            //con.Time = c.Date.ToShortTimeString(); //WUUT?
        //            //  con.url = c.Url;

        //            con.Friends = FriendsGoingToConcert(userId, id);
        //            con.FriendsAttending = con.Friends.Count;
        //            // con.Attending = false; // må endres
        //            return con;
        //        }
        //        catch (Exception)
        //        {
        //            return null;
        //        }
        //    }
        //}

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

                    //Console.WriteLine(cf);
                    //List<Friends> friends = (from v in db.FriendsDb where v.UserId == userId select v).ToList();
                    //var f = new List<FriendsClass>();

                    //foreach (var friend in friends)
                    //{
                    //    ConcertFollowers c =
                    //        (from v in db.ConcertFollowersDb
                    //         where v.ConcertId == concertId && v.UserId == friend.UserId
                    //         select v).FirstOrDefault();
                    //    if (c != null)
                    //    {
                    //        FriendsClass fr = new FriendsClass();
                    //        fr.FriendsId = friend.Friend;
                    //        fr.Friendsname = (from x in db.UserDb where x.UserId == fr.FriendsId select x.ProfileName).FirstOrDefault();
                    //        f.Add(fr);
                    //    }
                    //}
                    //Console.WriteLine(f);
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