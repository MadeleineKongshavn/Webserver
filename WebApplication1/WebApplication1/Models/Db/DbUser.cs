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
        public async Task<bool> ChangeRadius(int radius, int userId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var v = await (from u in db.UserDb where u.UserId == userId select u).FirstOrDefaultAsync();
                    v.Radius = radius;
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> ChangePic(int uid, String img)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var v = await (from u in db.UserDb where u.UserId == uid select u).FirstOrDefaultAsync();
                    v.Url = img;
                    await db.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<int> NormalRegister(String name, String email, String pass, double yCord, double xCord)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                      var user = await (from v in db.UserDb
                        where v.ProfileName == name || v.Password.Email == email
                        select v).FirstOrDefaultAsync();

                    if (user != null && user.ProfileName.Equals(name)) return -3;
                    if (user != null && user.Password.Email.Equals(email)) return -2;

                    byte[] password = Encoding.UTF8.GetBytes(pass);
                    byte[] salt = GetNewSalt();
                    User u = new User()
                    {
                        Radius = 100000,
                        ProfileName = name,
                        Ycoordinates = 59.911032,
                        Xcoordinates = 10.752408,
                        Area = "Oslo Sentralstasjon, Oslo, Norway",
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
        public async Task<Boolean> AddUser(UserClass u) 
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var u1 = new User()
                    {
                        ProfileName = u.Name,
                        Radius = u.Radius,
                       Xcoordinates = u.Xcoordinates,
                       Ycoordinates = u.Ycoordinates,
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
        public async Task<int> AddFaceUser(long uid, String profilename, String path, double xCord, double yCord)
        {
            using (var db = new ApplicationDbContext())
            {

                var d = await (from v in db.UserDb where v.Api.ApiId == uid select v).FirstOrDefaultAsync();
                if (d != null) return d.UserId;   

                var u1 = new User()
                {
                    ProfileName = profilename,
                    Radius = 500,
                    Xcoordinates = 0,
                    Ycoordinates = 0,
                    Url = "https://graph.facebook.com/" + path + "/picture",
                };
                db.UserDb.Add(u1);
                u1.Api = new Api()
                {
                    ApiId = uid,
                };
                await db.SaveChangesAsync();
                return u1.UserId;
            }
        }
        public async Task<Boolean> ChangeUser(UserClass u1) 
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var u2 = (from u in db.UserDb
                              where u1.UserId == u.UserId
                              select u).FirstOrDefault();
                    u2.Url = u1.Url;
                    u2.Radius = u1.Radius;
                    u2.ProfileName = u1.Name;
                    u2.Xcoordinates = 0;
                    u2.Ycoordinates = 0;
                    db.SaveChanges();
                    return true;
                }            
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public async Task <List<NotificationsClass>> GetAllNotifications(int userId)
        {
           // 1 friends, 2 inviter concert, 3 bekreftelse
           var newList = new List<NotificationsClass>();
            try
            {
                using (var db = new ApplicationDbContext())
                {

                    var not = await (from n in db.NotificationsDb
                               where n.UserId == userId select n).OrderByDescending(d => d.SendtTime).ToListAsync();

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
                        if (type == 2) ny.Url = c.InviteConcertNotifications.User.Url;
                        if (type == 3) ny.Url = c.AcceptConcertInvitation.User.Url;

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
            }
        }
        public static String Geturl(int Type, Notifications n)
        {
             if(Type==1) return n.FriendRequestNotifications.User.Url;  
             if (Type==2)  return  n.InviteConcertNotifications.Concert.BitmapSmalUrl;
             if(Type==3)  return  n.AcceptConcertInvitation.Concert.BitmapSmalUrl;
            return null;
        }
        public void UserHasSeenAllNotifications(int userId)
        {
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
        }
        public async Task<UserClass> GetUserById(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var result = await (from u in db.UserDb
                                    where u.UserId == id
                                    select new UserClass()
                                    {
                                        UserId = u.UserId,
                                        Ycoordinates = u.Ycoordinates,
                                        Xcoordinates = u.Xcoordinates,
                                        Area = u.Area,
                                        Url = u.Url,
                                        Radius = u.Radius,
                                        Name = u.ProfileName
                                    }).FirstOrDefaultAsync();
                return result;
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
                    var allGenres = (from g in db.GenreDb select g);
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
        public async Task<List<GenreClass>> GetUserGenres(int userid)
        {
            List<GenreClass> userGenres = null;
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    userGenres = (from ug in db.UserGenreDb
                                  where ug.UserId == userid
                                  select new GenreClass
                                  {
                                      GenreId=ug.GenreId
                                  }).ToList();
                }
                return userGenres;
            }
            catch (Exception)
            {

                return null;
            }
        }
        public async Task<bool> UpdateUserLocation(int userid, string area, double x, double y)
        {
            if (userid == null || area == null)
                return false;
            try
            {
                using (var db = new ApplicationDbContext())
                {

                    var user = (from u in db.UserDb
                                where u.UserId == userid
                                select u).FirstOrDefault();
                   user.Xcoordinates = x;
                   user.Ycoordinates = y;
                   user.Area = area;
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
