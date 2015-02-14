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

        public Bitmap GetConcertImage(String url)
        {
            System.Net.WebRequest request =System.Net.WebRequest.Create(url);
            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream responseStream =response.GetResponseStream();
            return new Bitmap(responseStream);
        }

        public List<User> FriendsGoingToConcert(int userId) // Finds all your friends that are going to the same concert 
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    List<Friends> friends = (from v in db.FriendsDb where v.UserId == userId select v).ToList();
                    List<User> userList = friends.Select(f => (from v in db.UserDb where v.UserId == f.Friend select v).FirstOrDefault()).ToList();
                    return userList;
                }
                catch (Exception)
                {
                    return null;
                }
            }            
        }
        public int FriendsGoingToConcertNumber(int userId)// Finds all your friends that are going to the same concert number
        {
            List<User> users = FriendsGoingToConcert(userId);
            if (users == null) return 0; else return users.Count;
        }
        // they post on facebook and it automatical updates the app
    }
}