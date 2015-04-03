using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace WebApplication1.Models
{
    public class DbBand
    {
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
        public async Task<bool> AddBand(BandClass b, Byte[] pic)
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
                        BitmapSmalUrl = "bitmap small url her",
                        BitmapUrl = "bitmap big url her",
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
        public async Task<bool> ChangeBand(BandClass b, Byte[] pic)
        {
            try
            {
                using (var db = new ApplicationDbContext())
                {
                    var b1 = (from ban in db.BandDb
                              where b.BandId == ban.BandId
                              select ban).FirstOrDefault();

                    // lagre pic i bitmapurl og bitmapsmallurl

                    b1.UrlRandom = b.UrlRandom;
                    b1.UrlSoundCloud = b.UrlSoundCloud;
                    b1.UrlFacebook = b.UrlFacebook;
                    b1.Xcoordinates = b.Xcoordinates;
                    b1.Ycoordinates = b.Ycoordinates;
                    b1.Area = b.Area;
                    b1.BandName = b.BandName;
                    b1.BitmapSmalUrl = "bitmap small url her";
                    b1.BitmapUrl = "bitmap big url her";
                    b1.Songurl = "hvis sangen skal lastes opp istedenfor, url her";
                    b1.SongName = b.SongName;
                    b1.Song = null; //hvis sangen skal være en byte array
                    b1.Timestamp = DateTime.Now;

                    List<Member> listMembers = new List<Member>();
                    foreach (var id in b.Member)
                    {
                        listMembers.Add(new Member()
                        {
                            BandId = b.BandId,
                            UserId = id,
                        });
                    }
                    b1.Member = listMembers;
                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }  
            
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

        private Byte[] CompressBitmap(Bitmap bmp)
        {

            ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

            var myEncoderParameters = new EncoderParameters(1);
            var myEncoderParameter = new EncoderParameter(myEncoder, 20L);

            myEncoderParameters.Param[0] = myEncoderParameter;

            using (var ms = new MemoryStream())
            {
                bmp.Save(ms, jpgEncoder, myEncoderParameters);
                return ms.ToArray();
            }

        }

        private String CompressExistingByteArrayBitmap(Byte[] bytAarray)
        {
            try
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

                var myEncoderParameters = new EncoderParameters(1);
                var myEncoderParameter = new EncoderParameter(myEncoder, 20L);

                myEncoderParameters.Param[0] = myEncoderParameter;
                var mse = new MemoryStream(bytAarray);
                var bmp = new Bitmap(mse);

                bmp.Save(@"c:\filenamejpg", jpgEncoder, myEncoderParameters);
                return "url";
            }
            catch (Exception)
            {
                return null;
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
                                      Area = v.Band.Area,
                                      BandId = v.Band.BandId,
                                      BandName = v.Band.BandName,
                                      BandGenre = v.Band.BandGenre,
                                      BitmapUrl = v.Band.BitmapUrl,
                                      SmallBitmapUrl = v.Band.BitmapSmalUrl,
                                      
                                      //SmallBitmap = v.Band.SmallBitmap,//virker men bytt til url
                                      UrlFacebook = v.Band.UrlFacebook,
                                      UrlRandom = v.Band.UrlRandom,
                                      UrlSoundCloud = v.Band.UrlSoundCloud,
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