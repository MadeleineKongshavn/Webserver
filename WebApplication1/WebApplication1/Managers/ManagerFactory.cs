using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Security;

namespace WebApplication1.Managers
{
    public class ManagerFactory : BaseManager
    {
        public static BandManager GetBandManager()
        {
            return new BandManager();
        }
        public static ConcertManager GetConcertManager()
        {
            return new ConcertManager();
        }
        public static UserManager GetUserManager()
        {
            return new UserManager();
        }
        public static GenreManager GetGenreManager()
        {
            return new GenreManager();
        }
        public static FriendManager GetFriendManager()
        {
            return new FriendManager();
        }
    }
}
