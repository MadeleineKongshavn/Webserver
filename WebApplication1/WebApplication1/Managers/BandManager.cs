using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using WebApplication1.Models;

namespace WebApplication1.Managers
{
    public class BandManager : BaseManager
    {

        public async Task<BandClass> GetBandById(int bandId)
        {
            BandClass bandClass;
            var cacheKey = String.Format("Band_Get_{0}", bandId);
            if ((bandClass = (BandClass)Cache.Get(cacheKey)) != null)
                return bandClass;

            try
            {
                var db = new DbBand();
                bandClass = await db.GetBandById(bandId);
                if(bandClass != null)
                    Cache.Insert(cacheKey, bandClass, null, DateTime.Today.AddDays(1), Cache.NoSlidingExpiration);
            }
            catch (Exception)
            {
                return null;
            }
            return bandClass;
        }

        //public async bool Create()
        //{
            
        //}


        public async Task<List<BandClass>> FindAllBandsToUser(int userId)
        {
            List<BandClass> bandList;
            //Ingen grunn å cache noe som er så brukerspesefikt
            //var cacheKey = String.Format("Band_GetBandToUser_{0}", userId);
            //if ((bandList = (List<BandClass>)Cache.Get(cacheKey)) != null)
            //    return bandList;

            try
            {
                var db = new DbBand();
                bandList = await db.FindAllBandsToUser(userId);
                //if (bandList != null)
                //    Cache.Insert(cacheKey, bandList, null, DateTime.Today.AddDays(1), Cache.NoSlidingExpiration);

            }
            catch (Exception)
            {
                return null;
            }
            return bandList;
        }

    }
}
