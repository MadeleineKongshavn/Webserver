using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Microsoft.SqlServer.Server;

namespace WebApplication1.Models
{
    public class DbBand
    {

        private Byte[] ConvertBitmapToByte(Bitmap bitmap)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                bitmap.Save(ms, ImageFormat.Bmp);
                return ms.ToArray();
            }
            catch (Exception e)
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

        private Byte[] CompressExistingByteArrayBitmap(Byte[] bytAarray)
        {
            try
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;

                var myEncoderParameters = new EncoderParameters(1);
                var myEncoderParameter = new EncoderParameter(myEncoder, 20L);

                myEncoderParameters.Param[0] = myEncoderParameter;

                Bitmap bmp;
                Bitmap newBit;
                using (var mse = new MemoryStream(bytAarray))
                {
                    bmp = new Bitmap(mse);
                    using (var ms = new MemoryStream())
                    {
                        bmp.Save(ms, jpgEncoder, myEncoderParameters);
                        return ms.ToArray();
                    }
                }
            }
            catch (Exception e)
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

        public List<BandClass> FindAllBandsToUser(int id) // Find all bands to a user 
        {
            using (var db = new ApplicationDbContext())
            {
                try
                {
                    List<BandFollowers> allBands = (from v in db.BandFollowersDb where v.UserId == id select v).ToList();
                    List<Band> bandsTouser =
                        allBands.Select(b => (from v in db.BandDb where v.BandId == b.BandId select v).FirstOrDefault())
                            .Where(bands => bands != null)
                            .ToList();

                    bandsTouser = bandsTouser.OrderBy(z => z.BandName).ToList();

                    var band = new List<BandClass>();
                    if (bandsTouser.Count == 0) return band;

                    foreach (Band b in bandsTouser) // Loop through List with foreach.
                    {
                        BandClass bandClass = new BandClass();
                        bandClass.BandName = b.BandName;
                        bandClass.BandId = b.BandId;
                        //bandClass.url = b.Url;
                        bandClass.Xcoordinates = b.Xcoordinates;
                        bandClass.Ycoordinates = b.Ycoordinates;

                        List<String> genreList = new List<string>();
                        List<BandGenre> list = b.BandGenre;
                        foreach (BandGenre genre in list)
                        {
                            genreList.Add((genre.Genre).GenreName);
                        }
                        bandClass.Genre = genreList.ToArray();
                        band.Add(bandClass);
                    }
                    return band;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}