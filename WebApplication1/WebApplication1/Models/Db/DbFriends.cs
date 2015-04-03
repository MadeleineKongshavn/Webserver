using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web;
using Antlr.Runtime;

namespace WebApplication1.Models.Db
{
    public class DbFriends
    {
        // legger til et nytt vennerequest userid er personen som får requestet 
        public async Task<Boolean> AddFriendRequest(int userId, int friendId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    db.NotificationsDb.Add(new Notifications()
                    {
                        SendtTime = DateTime.Now,
                        Seen = false,
                        UserId = friendId,
                        Type = 1,
                        FriendRequestNotifications = new FriendRequestNotifications()
                        {
                            Accepted = false,
                            UserId = userId,
                        }
                    });
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        // aksepterer eller sier nei til en venneforespørsel
        public async Task<Boolean> SetFriendAccept(int id, Boolean ok)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    Notifications n = (from x in db.NotificationsDb where x.NotificationsId == id select x).FirstOrDefault();
                    if (!ok)
                    {
                        db.FriendRequestNotificationsDb.Remove(n.FriendRequestNotifications);
                        db.NotificationsDb.Remove(n);
                        db.SaveChanges();
                        return true;
                    }
                    int userIde = n.FriendRequestNotifications.UserId;
                    int friendIde = n.UserId;


                    n.FriendRequestNotifications.Accepted = true;
                    n.SendtTime = DateTime.Now;

                    db.NotificationsDb.Add(new Notifications()
                    {
                        SendtTime = DateTime.Now,
                        Seen = false,
                        UserId = userIde,
                        Type = 1,
                        FriendRequestNotifications = new FriendRequestNotifications()
                        {
                            Accepted = true,
                            UserId = friendIde,
                        }
                    });
                    db.FriendsDb.Add(new Friends()
                    {
                        UserId = userIde,
                        Friend = friendIde,
                    });
                    db.FriendsDb.Add(new Friends()
                    {
                        UserId = friendIde,
                        Friend = userIde,
                    });
                    db.SaveChanges();
                    return true;
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
                                              url = fri.Url,
                                          }).ToListAsync();
                    return friendsclass;
                }

            }
            catch (Exception)
            {
                return null;
                
            }
        }
        // finds a friend based on a query
        public async Task<FriendsClass> FindFriend(String name)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    User found = (from f in db.UserDb where f.ProfileName == name select f).FirstOrDefault();
                    var friend = new FriendsClass();
                    friend.FriendsId = found.UserId;
                    friend.Friendsname = found.ProfileName;
                    return friend;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}