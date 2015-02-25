using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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


    }
}