﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Caching;
using WebApplication1.Models;
using WebApplication1.Models.Class;

namespace WebApplication1.Managers
{
    public class BandManager : BaseManager
    {
/*        public async Task<List<BandClass>> FindBandBasedOnQuery(String query, int uid)
        {
            var db = new DbBand();
            return await db.FindBandBasedOnQuery(query, uid);
        }*/
        public async Task<List<BandsImagesClass>> GetRandomBands(int userId)
        {
            var db = new DbBand();
            return await db.GetRandomBands(userId);
        }

        public async Task<bool> AddBand(String name, int userId)
        {
            var db = new DbBand();
            return await db.AddBand(name, userId);
        }
        
        public async Task<List<MemberClass>> GetAllAdminBands(int userId)
        {
            var db = new DbBand();
            return await db.GetAllAdminBands(userId);
        }
        
        public async Task<bool> UpdateMusicUrl(int bandId, String url)
        {
            var db = new DbBand();
            return await db.UpdateMusicUrl(bandId, url);
        }
        
        public async Task<bool> AddBandToUser(int userId, int bandId)
        {
            var db = new DbBand();
            var cacheKey = String.Format("Band_GetBandToUser_{0}", userId);
            RemoveCacheKeysByPrefix(cacheKey);
            return await db.AddBandToUser(userId, bandId);
        }
        
        public async Task<bool> AddBand(BandClass b, Byte[] pic)
        {
            var db = new DbBand();
            var imgUrl = await UploadImage(pic);
            b.BitmapUrl = imgUrl;
            b.SmallBitmapUrl = imgUrl;
            return await db.AddBand(b);
        }

        public async Task<String> Upload(Byte[] pic)
        {
            return await UploadImage(pic);
        }
        
        public async Task<bool> ChangeBand(BandClass b, Byte[] pic)
        {
            var db = new DbBand();
            var cacheKey = String.Format("Band_GetBandToUser_{0}", b.BandId);
            RemoveCacheKeysByPrefix(cacheKey);
            var imgUrl = await UploadImage(pic);
            b.BitmapUrl = imgUrl;
            b.SmallBitmapUrl = imgUrl;
            return await db.ChangeBand(b);
        }
      
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

        public async Task<bool> updateBandName(String name, int bandId)
        {
            var db=new DbBand();
            return await db.UpdateBandName(name,bandId);

        }

        public async Task<bool> UpdateBandLocation(int bandid, long x, long y, string area)
        {
            var db = new DbBand();
            return await db.UpdateBandLocation(bandid,x,y,area);

        }

        public async Task<bool> UpdateBandLinks(int bandid, string www, string fb, string soundcloud)
        {
            var db = new DbBand();
            return await db.UpdateBandLinks(bandid, www, fb, soundcloud);
        }




        public async Task<bool> UpdateBand(BandClass b)
        {
            var db = new DbBand();
            return await db.UpdateBand(b);
        }

        public async Task<List<BandClass>> FindAllBandsToUser(int userId)
        {
            List<BandClass> bandList;
            //Ingen grunn å cache noe som er så brukerspesefikt
            //cacher :D
            var cacheKey = String.Format("Band_GetBandToUser_{0}", userId);
            if ((bandList = (List<BandClass>)Cache.Get(cacheKey)) != null)
            return bandList;

            try
            {
                var db = new DbBand();
                bandList = await db.FindAllBandsToUser(userId);
                if (bandList != null)
                    Cache.Insert(cacheKey, bandList, null, DateTime.Today.AddDays(1), Cache.NoSlidingExpiration);

            }
            catch (Exception)
            {
                return null;
            }
            return bandList;
        }

    }
}
