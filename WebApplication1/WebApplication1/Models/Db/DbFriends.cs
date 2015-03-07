using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Antlr.Runtime;

namespace WebApplication1.Models.Db
{
    public class DbFriends
    {
        public List<FriendsClass> FriendsToUser(int id)
        {
            List<FriendsClass> friendsclass = new List<FriendsClass>();

            try
            {
                using (var db = new ApplicationDbContext())
                {
                    List<Friends> friends = (from f in db.FriendsDb where f.UserId == id select f).ToList();

                    foreach (Friends friend in friends)
                    {
                        FriendsClass f = new FriendsClass();
                        f.FriendsId = friend.Friend;
                        User user = (from u in db.UserDb where u.UserId == f.FriendsId select u).FirstOrDefault();
                        f.Friendsname = user.ProfileName;
                        f.url = user.Url;

                        friendsclass.Add(f);
                    }
                    return friendsclass;


                }

            }
            catch (Exception e)
            {
                return friendsclass;
                
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
                    friend.url = found.Url;
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