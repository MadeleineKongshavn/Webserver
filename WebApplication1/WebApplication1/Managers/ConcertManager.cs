using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using WebApplication1.Models;
using WebApplication1.Models.Class;

namespace WebApplication1.Managers
{
    public class ConcertManager : BaseManager
    {
        public async Task<ConcertClass> GetConcertById(int id)
        {
            ConcertClass objectDto;
            var cacheKey = String.Format("Concert_Get_{0}", id);
            if ((objectDto = (ConcertClass)Cache.Get(cacheKey)) != null)
                return objectDto;
            try
            {
                var db = new DbConcert();
                objectDto = await db.GetConcertById(id);
                if (objectDto != null)
                    Cache.Insert(cacheKey, objectDto, null, DateTime.Today.AddDays(1), Cache.NoSlidingExpiration);
            }
            catch (Exception)
            {
                return null;
            }
            return objectDto;

        }


        /// <summary>
        /// No cache
        /// </summary>
        /// <param name="id"></param>
        public async Task<List<UserClass>> GetFriendsGoingToConcertById(int concertId, int userId)
        {
            var db = new DbConcert();
            return await db.FriendsGoingToConcert(userId, concertId);
        }

        public async Task<List<ConcertClass>> GetAllConcertFromUser(int userId)
        {
            //var db = new DbConcert();
            //return db.FindAllConcertToUser(id);


            List<ConcertClass> objectDto;
            var cacheKey = String.Format("Concert_Get_{0}", userId);
            if ((objectDto = (List<ConcertClass>)Cache.Get(cacheKey)) != null)
                return objectDto;
            try
            {
                var db = new DbConcert();
                objectDto = await db.FindAllConcertToUser(userId);
                if (objectDto != null)
                    Cache.Insert(cacheKey, objectDto, null, DateTime.Today.AddDays(1), Cache.NoSlidingExpiration);
            }
            catch (Exception)
            {
                return null;
            }
            return objectDto;

        }
    }
}
