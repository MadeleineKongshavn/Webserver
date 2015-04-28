﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WebApplication1.Models.Class;
using WebApplication1.Models.Db;

namespace WebApplication1.Models
{
    public class DbUser
    {
        public async Task<int> NormalRegister(String name, String email, String pass, double yCord, double xCord)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    byte[] password = Encoding.UTF8.GetBytes(pass);
                    byte[] salt = GetNewSalt();
                    User u = new User()
                    {
                        Timestamp = DateTime.Now,
                        Radius = 500,
                        Public = true,
                        SeeNotifications = true,
                        ProfileName = name,
                        Ycoordinates = yCord,
                        Xcoordinates = xCord,
                        Password = new Password()
                        {
                            Email = email,
                            PasswordSet = GenerateSaltedHash(password, salt),
                            Salt = salt,
                        }
                    };
                    db.UserDb.Add(u);
                    await db.SaveChangesAsync();
                    return u.UserId;
                }
            }
            catch (Exception)
            {
                return -1;
            }
            
        }
        public async Task<int> NormalLogin(String pass, String name)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var f = (from b in db.UserDb where b.Password.Email.Equals(name, StringComparison.InvariantCultureIgnoreCase) select b).FirstOrDefault();
                    Byte[] password = GenerateSaltedHash(Encoding.UTF8.GetBytes(pass), f.Password.Salt);
                    if (CompareByteArrays(f.Password.PasswordSet, password)) return f.UserId; else return -1;
                }

            }
            catch (Exception)
            {
                return -1;
            }
        }
        public byte[] GetNewSalt()
        {
            byte[] salt = new byte[32];
            System.Security.Cryptography.RNGCryptoServiceProvider.Create().GetBytes(salt);
            return salt;
        }
        static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
              new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }

        public static bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }
        public async Task<Boolean> CheckNewNotifications(int id)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var v =
                        (from n in db.NotificationsDb where n.UserId == id && n.Seen == false select n)
                            .FirstOrDefault();
                    if (v != null)
                        return true; else return false;
                }
            }
            catch (Exception e)
            {
                
                return false;
            }            
        }
        // legger til basisk funksjonene til en bruker
        public async Task<Boolean> AddUser(UserClass u) 
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
                        Url = u.Url,
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

        public async Task<Boolean> AddFaceUser(int uid, String profilename, long xCord, long yCord)
        {
            using (var db = new ApplicationDbContext())
            {

                var u1 = new User()
                {
                    ProfileName = profilename,
                    SeeNotifications = true,
                    Public = true,
                    Radius = 500,
                    Xcoordinates = xCord,
                    Ycoordinates = yCord,
                    Timestamp = DateTime.Now,
                };
                db.UserDb.Add(u1);
                u1.Api = new Api()
                {
                    ApiId = uid,
                };
                db.SaveChanges();
                return true;
            }
            
        }
        // endrer basisk funksjonene til en bruker
        public async Task<Boolean> ChangeUser(UserClass u1) 
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var u2 = (from u in db.UserDb
                              where u1.UserId == u.UserId
                              select u).FirstOrDefault();

                    // lagre nytt bilde her?

                    u2.Url = u1.Url;
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
            catch (Exception e)
            {
                
                return false;
            }
        }
        public List<NotificationsClass> GetAllNotifications(int userId)
        {
           // 1 friends, 2 inviter concert, 3 bekreftelse
           var newList = new List<NotificationsClass>();
            try
            {
                using (var db = new ApplicationDbContext())
                {

                    var not =  (from n in db.NotificationsDb
                               where n.UserId == userId select n).OrderByDescending(d => d.SendtTime).ToList();

                    List<NotificationsClass> list = new List<NotificationsClass>();

                    foreach (Notifications c in not)
                    {
                        NotificationsClass ny = new NotificationsClass();
                        ny.Seen = c.Seen;
                        ny.Day = c.SendtTime.Day;
                        ny.Hour = c.SendtTime.Hour;
                        ny.Minute = c.SendtTime.Minute;
                        ny.Month = c.SendtTime.Month;
                        int type = ny.Type = c.Type;
                        ny.Type = c.Type;
                        ny.Date = c.SendtTime;
                        ny.Year = c.SendtTime.Year;
                        ny.NotificationsId = c.NotificationsId;

                        if (type == 1) ny.FriendId = c.FriendRequestNotifications.UserId;
                        if (type == 2) ny.FriendId = c.InviteConcertNotifications.ConcertId;
                        if (type == 3) ny.FriendId = c.AcceptConcertInvitation.ConcertId;


                        if (type == 1) ny.Url = c.FriendRequestNotifications.User.Url;
                        if (type == 2) ny.Url = c.InviteConcertNotifications.Concert.BitmapSmalUrl;
                        if (type == 3) ny.Url = c.AcceptConcertInvitation.Concert.BitmapSmalUrl;

                        if (type == 1) ny.Accepted = c.FriendRequestNotifications.Accepted;
                        if (type == 2) ny.Accepted = c.InviteConcertNotifications.Accepted;

                        if (type == 1) ny.FriendName = c.FriendRequestNotifications.User.ProfileName;
                        if (type == 2) ny.FriendName = c.InviteConcertNotifications.User.ProfileName;
                        if (type == 3) ny.FriendName = c.AcceptConcertInvitation.User.ProfileName;

                        if (type == 2) ny.ConcertName = c.InviteConcertNotifications.Concert.Title;
                        if (type == 3) ny.ConcertName = c.AcceptConcertInvitation.Concert.Title;

                        list.Add(ny);
                    }
                    return list;
                }
            }
            catch (Exception e)
            {
                return new List<NotificationsClass>();
                //List<NotificationsClass>
            }
        }

        public static String Geturl(int Type, Notifications n)
        {
             if(Type==1) return n.FriendRequestNotifications.User.Url;  
             if (Type==2)  return  n.InviteConcertNotifications.Concert.BitmapSmalUrl;
             if(Type==3)  return  n.AcceptConcertInvitation.Concert.BitmapSmalUrl;
            return null;

        }

        public static Boolean GetAccepted(int type, Notifications n)
        {
            if(type==1) return n.FriendRequestNotifications.Accepted;
            if(type==2) return n.InviteConcertNotifications.Accepted;
            return false;

        }

        public static String getTitle(int type, Notifications n)
        {
            if (type == 1) return  n.FriendRequestNotifications.User.ProfileName;
            if (type == 2) return  n.InviteConcertNotifications.Concert.Title;
            if (type == 3) return  n.AcceptConcertInvitation.Concert.Title;
            return "Not specified";
        }
        public void UserHasSeenAllNotifications(int userId)
        {
           //ThreadPool.QueueUserWorkItem(delegate//Kjør når du får tid :) funker ikke?
           //  {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var res = (from n in db.NotificationsDb where userId == n.UserId select n);

                    foreach (var n in res)
                    {
                        n.Seen = true;
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                
            }

           // });
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

        public async Task<bool> UpdateUserGenres(int userid, String[] genres)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var oldGenres = (from ug in db.UserGenreDb
                                     where ug.UserId == userid
                                     select ug);
                    foreach (var obj in oldGenres)
                    {
                        db.UserGenreDb.Remove(obj);
                    }


                    var allGenres = (from g in db.GenreDb
                                     select g);

                    var newGenres = allGenres.Where(x => genres.Contains(x.GenreName));

                    foreach (Genre obj in newGenres)
                    {
                        UserGenre ug = new UserGenre { GenreId = obj.GenreId, UserId = userid };
                        db.UserGenreDb.Add(ug);
                    }

                    db.SaveChanges();
                }

                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }



    }
}




/*  select new NotificationsClass()
           {
               FriendId = n.FriendRequestNotifications.UserId,
               Seen = n.Seen,
               Day = n.SendtTime.Day,
               Hour = n.SendtTime.Hour,
               Minute = n.SendtTime.Minute,
               Month = n.SendtTime.Month,
               Type = n.Type,
               Date = n.SendtTime,
               Year = n.SendtTime.Year,
               NotificationsId = n.NotificationsId,
           }).OrderByDescending( d => d.Date).ToList();
foreach (var c in not)
{
    int type;

    if (type == 1) c.Url = n.FriendRequestNotifications.User.Url;
    if (type == 2) c.Url = n.InviteConcertNotifications.Concert.BitmapSmalUrl;
    if (type == 3) c.Url = n.AcceptConcertInvitation.Concert.BitmapSmalUrl;

    if (type == 1) c.Accepted = n.FriendRequestNotifications.Accepted;
    if (type == 2) c.Accepted = n.InviteConcertNotifications.Accepted;

    if (type == 1) c.FriendName = n.FriendRequestNotifications.User.ProfileName;
    if (type == 2) c.FriendName = n.InviteConcertNotifications.Concert.Title;
    if (type == 3) c.FriendName = n.AcceptConcertInvitation.Concert.Title;

}*/