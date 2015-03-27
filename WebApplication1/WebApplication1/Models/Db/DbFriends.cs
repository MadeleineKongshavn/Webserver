﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Antlr.Runtime;

namespace WebApplication1.Models.Db
{
    public class DbFriends
    {
        public Boolean SetFriendRequestAccept(int id, Boolean ok)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    Notifications n = (from x in db.NotificationsDb where x.NotificationsId == id select x).FirstOrDefault();
                    if (ok)
                    {
                        n.FriendRequestNotifications.Accepted = true;
                        DateTime time = DateTime.Now;

                        var confirm = new Notifications();
                        confirm.SendtTime = DateTime.Now;
                        confirm.Seen = false;
                        confirm.Type = 1;
                        confirm.UserId = n.FriendRequestNotifications.UserId;
                        db.NotificationsDb.Add(confirm);
                        db.SaveChanges();

                        var f = new FriendRequestNotifications();
                        f.Accepted = true;
                        f.Answered = true;
                        f.UserId = n.UserId;
                        confirm.FriendRequestNotifications = f;
                        db.SaveChanges();

                        return AddFriend(n.UserId + "", n.FriendRequestNotifications.UserId + "");
                    }
                    else
                    {
                        db.FriendRequestNotificationsDb.Remove(n.FriendRequestNotifications);
                        db.NotificationsDb.Remove(n);
                        db.SaveChanges();
                    }
                    {
                        db.NotificationsDb.Remove(n);
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }


        public async Task<List<FriendsClass>> GetFriendsByUserId(int id)
        {
            var friendsclass = new List<FriendsClass>();

            try
            {
                using (var db = new ApplicationDbContext())
                {

                    friendsclass = await (from f in db.FriendsDb
                                          where f.UserId == id
                                           join fri in db.UserDb on f.Friend equals fri.UserId
                                          select new FriendsClass()
                                          {
                                              Friendsname = fri.ProfileName,
                                              FriendsId = f.Friend,
                                          }).ToListAsync();


                    //List<Friends> friends = (from f in db.FriendsDb where f.UserId == id select f).ToList();

                    //foreach (Friends friend in friends)
                    //{
                    //    FriendsClass f = new FriendsClass();
                    //    f.FriendsId = friend.Friend;
                    //    User user = (from u in db.UserDb where u.UserId == f.FriendsId select u).FirstOrDefault();
                    //    f.Friendsname = user.ProfileName;
                    //    //   f.url = user.Url;

                    //    friendsclass.Add(f);
                    //}
                    return friendsclass;
                }

            }
            catch (Exception e)
            {
                return null;
                
            }
        }
        public FriendsClass FindFriend(String name)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    User found = (from f in db.UserDb where f.ProfileName == name select f).FirstOrDefault();
                    var friend = new FriendsClass();
                    friend.FriendsId = found.UserId;
                    friend.Friendsname = found.ProfileName;
                  //  friend.url = found.Url;
                    return friend;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Boolean SendFriendRequest(String userId, String friendId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    return AddFriend(userId, friendId);
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Boolean AddFriend(String userId, String userId2)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {

                   Friends f1 = new Friends();
                   f1.UserId = Int32.Parse(userId);
                   f1.Friend = Int32.Parse(userId2);


                   Friends f2 = new Friends();
                   f2.UserId = Int32.Parse(userId2);
                   f2.Friend = Int32.Parse(userId);


                    f1.Blocked = false;
                    f2.Blocked = false;

                    DateTime  date = new DateTime(2009, 11, 11);
                    f2.FriendsSince = date;
                    f1.FriendsSince = date;


                   db.FriendsDb.Add(f1);
                   db.FriendsDb.Add(f2);
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