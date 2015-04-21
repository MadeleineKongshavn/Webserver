using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Threading.Tasks;
using WebApplication1.Managers;
using WebApplication1.Models.Class;


namespace WebApplication1.Models
{
    public class DbBand
    {

       /* private String CompressExistingByteArrayBitmap(Byte[] bytAarray, int number)
        {
            try
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

                var myEncoderParameters = new EncoderParameters(1);
                var myEncoderParameter = new EncoderParameter(myEncoder, 0L);

                myEncoderParameters.Param[0] = myEncoderParameter;
                var mse = new MemoryStream(bytAarray);
                var bmp = new Bitmap(mse);

                bmp.Save(@"c:\Band " + number + ".jpg", jpgEncoder, myEncoderParameters);
            }
            catch (Exception)
            {
                return null;
            }
        }*/
        public async Task<List<BandsImagesClass>> GetRandomBands(int userId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    List<BandsImagesClass> ob = await (from b in db.BandDb
                        select new BandsImagesClass()
                        {
                            BandId = b.BandId,
                            Title = b.BandName,
                            SmallBitmapUrl = b.BitmapSmalUrl,

                        }).ToListAsync();
                    DbBand.Shuffle(ob);
                    var o = ob.Take(15);
                    return o.ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static void Shuffle<T>(IList<T> list)
        {
            int n = list.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public async Task<bool> AddBand(String name, int userId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    Band b = new Band()
                    {
                        Timestamp = DateTime.Now,
                        Xcoordinates = 0,
                        Ycoordinates = 0,
                        Followers = 0,
                        BandName = name,
                    };
                    db.BandDb.Add(b);
                    List<Member> members = new List<Member>();
                    Member mem = new Member()
                    {
                        BandId = b.BandId,
                        UserId = userId,
                    };
                    members.Add(mem);
                    b.Member = members;
                    db.SaveChanges();
                    return true;
                }


            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<List<MemberClass>> GetAllAdminBands(int userId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    List<MemberClass> members = new List<MemberClass>();
                    List<Member> m = await (from v in db.MemberDb where v.UserId == userId select v).ToListAsync();
                    foreach(Member b in m)
                    {
                        MemberClass mem = new MemberClass();
                        mem.BandName = b.Band.BandName;
                        mem.Id = b.BandId;
                        mem.Url = b.Band.BitmapUrl;
                        members.Add(mem);
                    }
                    return members;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<bool> UpdateMusicUrl(int bandId, String url)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var ob =  (from b in db.BandDb where b.BandId == bandId select b).FirstOrDefault();
                    ob.Songurl = url;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
        // legger et band på en bruker
        public async Task<bool> AddBandToUser(int userId, int bandId)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    db.BandFollowersDb.Add(new BandFollowers()
                    {
                        UserId = userId,
                        BandId = bandId,
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
        // legger til basisk funksjonene til et band
        public async Task<bool> AddBand(BandClass b)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    Band band = new Band()
                    {
                        UrlSoundCloud = b.UrlSoundCloud,
                        UrlFacebook = b.UrlFacebook,
                        Xcoordinates = b.Xcoordinates,
                        Ycoordinates = b.Ycoordinates,
                        Area = b.Area,
                        BandName = b.BandName,
                        Followers = 0,
                        BitmapSmalUrl = b.BitmapUrl,
                        BitmapUrl = b.BitmapUrl,
                        Songurl = "hvis sangen skal lastes opp istedenfor, url her",
                        SongName = b.SongName,
                        Song = null, //hvis sangen skal være en byte array
                        Timestamp = DateTime.Now,     
                        UrlRandom = b.UrlRandom,

                    };
                    db.BandDb.Add(band);
                    int bandId = band.BandId;

                    List<Member> listMembers = new List<Member>();
                    foreach (var id in b.Member)
                    {
                        listMembers.Add(new Member()
                        {
                            BandId = bandId,
                            UserId = id,
                        });
                    }
                    band.Member = listMembers;
                    db.SaveChanges();
                    return true;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }
        // endrer på basisk funskjonene til et band
        

        public async Task<bool> UpdateBandName(String name, int bandId)
        {
            try 
            {
                using (var db = new ApplicationDbContext())
                {
                    var band= (from getBand in db.BandDb
                              where getBand.BandId==bandId
                              select getBand).FirstOrDefault();

                    band.BandName = name;
                    db.SaveChanges();

                }
            }
            catch(Exception){
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateBand(BandClass b)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var band = (from tempBand in db.BandDb
                                where tempBand.BandId == b.BandId
                                select tempBand).FirstOrDefault();

                    if (b.BandName != null)
                        band.BandName = b.BandName;
                    db.SaveChanges();

                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        private Byte[] ConvertBitmapToByte(Bitmap bitmap)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Bmp);
                return ms.ToArray();
            }
            catch (Exception)
            {
                return null;
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

        public  async Task<String> CompressBitmap()
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var result = await (from v in db.BandDb select v).ToListAsync();
                    foreach (var b in result)
                    {

                        var request = WebRequest.Create(b.BitmapSmalUrl);

                        Image i;
                        using (var response = request.GetResponse())
                        using (var stream = response.GetResponseStream())
                        {

                            i = Bitmap.FromStream(stream);
                        }

                        ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                        System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

                        var myEncoderParameters = new EncoderParameters(1);
                        var myEncoderParameter = new EncoderParameter(myEncoder, 0L);

                        myEncoderParameters.Param[0] = myEncoderParameter;

                        Byte[] arr;
                        using (var ms = new MemoryStream())
                        {
                            i.Save(ms, jpgEncoder, myEncoderParameters);
                            arr = ms.ToArray();

                        }
                        using (var bmgr = ManagerFactory.GetBandManager())
                        {
                            var v = await bmgr.Upload(arr);
                            b.BitmapSmalUrl = v;
                            db.SaveChanges();
                        }                  
                    }
                    return "funket";

                }
            }
            catch (Exception e)
            {
                return e.Message + "";
            }
       

        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        public String FindBandBasedOnQuery(String query) // delvis under oppbygning
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    query = query.Trim();
                    List<Band> allBands = (from b in db.BandDb where b.BandName.Contains(query) select b).ToList();


                    allBands.OrderBy(b => b.BandName).ToList();
                    List<BandClass> bands = new List<BandClass>();
                    foreach (var b in allBands)
                    {
                        BandClass newB = new BandClass();
                        newB.Xcoordinates = b.Xcoordinates;
                        newB.Ycoordinates = b.Ycoordinates;
                        newB.BandName = b.BandName;
                        //  newB.url = b.Url;
                        newB.BandId = b.BandId;
                        bands.Add(newB);

                    }
                    return "funket ikke";
                }

            }
            catch (Exception e)
            {
                return e.Message + " " + e.Source + " " + e.InnerException;
                // return new List<BandClass>();   
            }
        }

        public async Task<List<BandClass>> FindAllBandsToUser(int id) // Find all bands to a user 
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {


                    var result = await (from v in db.BandFollowersDb
                                  where v.UserId == id
                                  //join c in db.BandDb on v.BandId equals c.BandId
                                  select new BandClass()
                                  {
                                      BandId = v.Band.BandId,
                                      BandName = v.Band.BandName,
                                      BandGenre = v.Band.BandGenre,
                                      SmallBitmapUrl = v.Band.BitmapSmalUrl,  
                                      Xcoordinates = v.Band.Xcoordinates,
                                      Ycoordinates = v.Band.Ycoordinates

                                  }).ToListAsync();      
                    return result;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets band by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>BandClass</returns>
        public async Task<BandClass> GetBandById(int id)
        {
            using (var db = new ApplicationDbContext())
            {
                var band = await (from b in db.BandDb
                                  where b.BandId == id
                                  select b).FirstOrDefaultAsync();

                return band.ConvertToBandClass();
            }
        }
    }
}