using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
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

                    List<Notifications> noti = (from n in db.NotificationsDb where userId == n.UserId select n).OrderBy(b => b.SendtTime).ToList();
                    foreach (Notifications n in noti)
                    {
                        var newN = new NotificationsClass();
                        newN.Seen = n.Seen;
                        newN.Day = n.SendtTime.Day;
                        newN.Hour = n.SendtTime.Hour;
                        newN.Minute = n.SendtTime.Minute;
                        newN.Month = n.SendtTime.Month;
                        newN.Type = n.Type;
                        newN.Year = n.SendtTime.Year;

                        n.Seen = true;
                
                        switch (n.Type) // 1 = request, 2=concert invitasjon, 3 = band ny konsert 
                        {
                            case 1:
                                var friend = n.FriendRequestNotifications;
                                newN.Accepted = friend.Accepted;
                                newN.Answered = friend.Answered;
                                newN.FriendId = friend.UserId;
                                newN.FriendName = friend.User.ProfileName;
                                newN.ByteArray = friend.User.Bitmap;
                                break;
                            default: break;
                        }
                        db.SaveChanges();
                        newList.Add(newN);
                    }
                    return newList;
                }
            }
            catch (Exception)
            {
                return newList;
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