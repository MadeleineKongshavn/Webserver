using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
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
                                   Seen = n.Seen,
                                   Day = n.SendtTime.Day,
                                   Hour = n.SendtTime.Hour,
                                   Minute = n.SendtTime.Minute,
                                   Month = n.SendtTime.Month,
                                   Type = n.Type,
                                   Year = n.SendtTime.Year,
                                   Accepted = n.FriendRequestNotifications.Accepted,
                                   Answered = n.FriendRequestNotifications.Answered,
                                   FriendId = n.FriendRequestNotifications.UserId,
                                   FriendName = n.FriendRequestNotifications.User.ProfileName,
                               }).ToList();
                    newList = not;

                    //SEND HELLER MED ALT, OG LA APPEN VIKSE DET :)

                    //List<Notifications> noti = (from n in db.NotificationsDb where userId == n.UserId select n).OrderBy(b => b.SendtTime).ToList();
                    //foreach (Notifications n in noti)
                    //{
                    //    var newN = new NotificationsClass();
                    //    newN.Seen = n.Seen;
                    //    newN.Day = n.SendtTime.Day;
                    //    newN.Hour = n.SendtTime.Hour;
                    //    newN.Minute = n.SendtTime.Minute;
                    //    newN.Month = n.SendtTime.Month;
                    //    newN.Type = n.Type;
                    //    newN.Year = n.SendtTime.Year;

                    //    n.Seen = true;

                    //    switch (n.Type) // 1 = request, 2=concert invitasjon, 3 = band ny konsert 
                    //    {
                    //        case 1:
                    //            var friend = n.FriendRequestNotifications;
                    //            newN.Accepted = friend.Accepted;
                    //            newN.Answered = friend.Answered;
                    //            newN.FriendId = friend.UserId;
                    //            newN.FriendName = friend.User.ProfileName;
                    //            newN.ByteArray = friend.User.Bitmap;
                    //            break;
                    //        default: break;
                    //    }
                    //    db.SaveChanges();
                    //    newList.Add(newN);
                    //}



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