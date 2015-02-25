using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models.Class;

namespace WebApplication1.Models
{
    public class DbUser
    {

        public String GetName(int id)
        {
            User user = FindUser(id);
            return user.ProfileName;
        }

        public Boolean GetNoticiations(int id) // ???
        {
            User user = FindUser(id);
            return user.Notifications;
        }

        public Boolean GetPublic(int id)
        {
            User user = FindUser(id);
            return user.Public;
        }

        public List<Double> GetArea(int id)
        {
            User user = FindUser(id);
            var area = new List<Double> {user.Xcoordinates, user.Ycoordinates};
            return area;
        }

        public int GetRadius(int id)
        {
            User user = FindUser(id);
            return user.Radius;
        }

        public Boolean ChangeName(String newName, int id) // Change your public name 
        {

            using (var db = new ApplicationDbContext())
            {
                try
                {
                    User user = FindUser(id);
                    user.ProfileName = newName;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        public Boolean AddGenre(int genreId, int userId) // adds a new genre to a person
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    User user = FindUser(userId);
                    Genre genre = (new DbGenre()).FindGenre(genreId);
                    if (user == null || genre == null) return false;

                    UserGenre userGenre = new UserGenre();
                    userGenre.UserId = userId;
                    userGenre.GenreId = genreId;

                    db.UserGenreDb.Add(userGenre);
                    db.SaveChanges();
                    return true;

                }
                catch (Exception)
                {
                    
                    return false;
                }
            }            
        }
        public Boolean RemoveGenre(int genreId, int id) // Removes a genre from a person
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    UserGenre deleteGenre = (from b in db.UserGenreDb where b.GenreId == genreId && b.UserId == id select b).FirstOrDefault();
                    db.UserGenreDb.Remove(deleteGenre);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }          
        }

        public UserClass GetUser(int id)
        {
            try
            {
                User u = FindUser(id);
                UserClass user = new UserClass();
                user.Name = u.ProfileName;
                user.Url = u.Url;
                return user;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public User FindUser(int id) // Finds a user
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    User user = (from b in db.UserDb where b.UserId == id select b).FirstOrDefault();
                    return user;
                }
                catch (Exception)
                {
                    return null;
                }

            }
        }
        public Boolean ChangeNotifications(Boolean ok, int id) // Change the notifications 
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    User user = FindUser(id);
                    user.Notifications = ok;
                    db.SaveChanges();
                    return true;

                }
                catch (Exception)
                {
                    return false;

                }
            }
        }
        public Boolean ChangePublic(Boolean ok, int id) // Change if you're the profile is public or not
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    User user = FindUser(id);
                    user.Public = ok;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    
                    return false;
                }
            }
        }
        public Boolean ChangeArea(int y, int x, int id) // Change the area you're in 
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    User user = FindUser(id);
                    user.Xcoordinates = x;
                    user.Ycoordinates = y;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }

            }    
        }
        public Boolean ChangeRadius(int radius, int id) // change the radius in km
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    User user = FindUser(id);
                    user.Radius = radius;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception)
                {              
                    return false;
                }
            }            
        }
    }
}