using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebApplication1.Models.Class;
using WebApplication1.Models.Db;

namespace WebApplication1.Models
{
    public class DbUser
    {

        // legger til basisk funksjonene til en bruker
        public async Task<Boolean> AddUser(UserClass u, Byte[] pic) 
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var u1 = new User()
                    {
                        ProfileName = u.Name,
                        SeeNotifications = u.SeeNotifications,
                        Public = u.Public,
                        Radius = u.Radius,
                        Xcoordinates = u.Xcoordinates,
                        Ycoordinates = u.Ycoordinates,
                        Timestamp = DateTime.Now,
                        Url = "bilde url uer",
                    };
                    db.UserDb.Add(u1);
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        // endrer basisk funksjonene til en bruker
        public async Task<Boolean> ChangeUser(UserClass u1, Byte[] pic) 
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var u2 = (from u in db.UserDb
                              where u1.UserId == u.UserId
                              select u).FirstOrDefault();

                    // lagre nytt bilde her?

                    u2.Url = "ny url her";
                    u2.Public = u1.Public;
                    u2.SeeNotifications = u1.SeeNotifications;
                    u2.Radius = u1.Radius;
                    u2.ProfileName = u1.Name;
                    u2.Xcoordinates = u1.Xcoordinates;
                    u2.Ycoordinates = u1.Ycoordinates;
                    u2.Timestamp = DateTime.Now;
                    db.SaveChanges();
                    return true;
                }            
            }
            catch (Exception)
            {
                return false;
            }
        }
        public List<NotificationsClass> GetAllNotifications(int userId)
        {
            var newList = new List<NotificationsClass>();
            try
            {
                using (var db = new ApplicationDbContext())
                {

                    var not = (from n in db.NotificationsDb
                               where n.UserId == userId
                               select new NotificationsClass()
                               {
                                   FriendId = n.FriendRequestNotifications.UserId,
                                   Seen = n.Seen,
                                   Day = n.SendtTime.Day,
                                   Hour = n.SendtTime.Hour,
                                   Minute = n.SendtTime.Minute,
                                   Month = n.SendtTime.Month,
                                   Type = n.Type,
                                   Date = n.SendtTime,
                                   Url = n.FriendRequestNotifications.User.Url,
                                   Year = n.SendtTime.Year,
                                   Accepted = n.FriendRequestNotifications.Accepted,
                                   NotificationsId = n.NotificationsId,
                                   FriendName = n.FriendRequestNotifications.User.ProfileName,
                               }).OrderBy( d => d.Date).ToList();
                    newList = not;

                    return newList;
                }
            }
            catch (Exception)
            {
                return newList;
            }
        }



        public void UserHasSeenAllNotifications(int userId)
        {
            ThreadPool.QueueUserWorkItem(delegate//Kjør når du får tid :)
             {
                 using (var db = new ApplicationDbContext())
                 {
                     var res = (from n in db.NotificationsDb where userId == n.UserId select n);

                     foreach (var n in res)
                     {
                         n.Seen = true;
                     }
                     db.SaveChangesAsync();
                 }
             });
        }


        public async Task<UserClass> GetUserById(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var result = await (from u in db.UserDb
                                    where u.UserId == id
                                    select new UserClass()
                                    {//More info..
                                        Name = u.ProfileName
                                    }).FirstOrDefaultAsync();
                return result;
            }
        }

        private Bitmap ConvertByteToBitmap(Byte[] bytAarray)
        {
            try
            {
                Bitmap bmp;
                using (var mse = new MemoryStream(bytAarray))
                {
                    bmp = new Bitmap(mse);
                    return bmp;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

    }
}