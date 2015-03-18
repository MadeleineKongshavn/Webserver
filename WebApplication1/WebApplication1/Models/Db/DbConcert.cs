using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using WebApplication1.Models.Class;

namespace WebApplication1.Models
{
    public class DbConcert
    {   
        public List<Concert> FindAllConcert() // find every concert that exist 
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    List<Concert> concert = (from v in db.ConcertDb select v).ToList();
                    return concert;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public List<ConcertClass> FindAllConcertToUser(int id) // Find all concerts to a user 
        {
            using (var db = new ApplicationDbContext())
            {
                var Concert = new List<ConcertClass>();
                try
                {
                    List<ConcertFollowers> alleConcertId = (from v in db.ConcertFollowersDb where v.UserId == id select v).ToList();
                    List<Concert> myConcerts = alleConcertId.Select(concert => (from v in db.ConcertDb where v.ConcertId == concert.ConcertId select v).FirstOrDefault()).Where(c => c != null).ToList();
                    
                    foreach (Concert c in myConcerts) // Loop through List with foreach.
                    {
                        ConcertClass con = new ConcertClass();
                        con.ConcertId = c.ConcertId;
                        con.Title = c.Title;
                        con.url = c.Url;
                        con.Bandname = c.Band.BandName;
                        con.Date = c.Date.ToLongDateString();

                       // con.Date = c.Date;

                        con.Attending = false; // må endres!!! 
                        con.FriendsAttending = 5; // må endres!!!                               
                        Concert.Add(con);
                    }
                    return Concert;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public ConcertClass GetConcertinfo(int id, int userId) //Get all information about one concert
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    Concert c = FindConcert(id);
                    ConcertClass con = new ConcertClass();
                    con.ConcertId = c.ConcertId;
                    con.Title = c.Title;
                    con.Xcoordinates = c.Xcoordinates; 
                    con.Ycoordinates = c.Ycoordinates; 
                 // con.Bandname = c.Band.BandName;
                    con.Date = c.Date.ToLongDateString();
                    con.Time = c.Date.ToShortTimeString();
                    con.url = c.Url;

                    con.Friends = FriendsGoingToConcert(userId, id);
                    con.FriendsAttending = con.Friends.Count;
                 // con.Attending = false; // må endres
                    return con;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public Concert FindConcert(int id) // find one concert
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    Concert concert = (from v in db.ConcertDb where v.ConcertId == id select v).FirstOrDefault();
                    return concert;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public Bitmap GetConcertImage(String url)
        {
            System.Net.WebRequest request =System.Net.WebRequest.Create(url);
            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream responseStream =response.GetResponseStream();
            return new Bitmap(responseStream);
        }

        public List<FriendsClass> FriendsGoingToConcert(int userId, int concertId) // Finds all your friends that are going to the same concert 
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    List<Friends> friends = (from v in db.FriendsDb where v.UserId == userId select v).ToList();
                    var f = new List<FriendsClass>();

                    foreach (var friend in friends)
                    {
                        ConcertFollowers c =
                            (from v in db.ConcertFollowersDb
                                where v.ConcertId == concertId && v.UserId == friend.UserId
                                select v).FirstOrDefault();
                        if (c != null)
                        {
                            FriendsClass fr = new FriendsClass();
                            fr.FriendsId = friend.Friend;
                            fr.Friendsname = (from x in db.UserDb where x.UserId == fr.FriendsId select x.ProfileName).FirstOrDefault();
                            f.Add(fr);
                        }
                    }                   
                    return f;
                }
                catch (Exception)
                {
                    return new List<FriendsClass>();
                }
            }            
        }
    }
}