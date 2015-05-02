using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;

namespace WebApplication1.Managers
{
    public class BaseManager : IDisposable
    {
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        
        protected Cache Cache
        {
            get
            {
                return (HttpContext.Current == null)
                    ? HttpRuntime.Cache
                    : HttpContext.Current.Cache;
            }
        }

        protected void RemoveCacheKeysByPrefix(string prefix)
        {
            var ide = Cache.GetEnumerator();
            while (ide.MoveNext())
            {
                //Debug.WriteLine(ide.Key.ToString());
                if (ide.Key.ToString().StartsWith(prefix))
                {
                    Cache.Remove(ide.Key.ToString());
                }
            }
        }

        public async Task<string> UploadImage(Image newImage)
        {

            var stream = new MemoryStream();
            newImage.Save(stream, ImageFormat.Jpeg);
            stream.Seek(0, SeekOrigin.Begin);

            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["BlobStorage"].ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("gfx");
            var blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
            blob.Properties.ContentType = "image/jpeg";
            //await blob.UploadFromByteArrayAsync(arBytes,0,arBytes.Length);
            await blob.UploadFromStreamAsync(stream);
            return blob.StorageUri.PrimaryUri.AbsoluteUri;
        }   
        public async Task<string> UploadImage(byte[] arBytes)
        {
            var storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings["BlobStorage"].ConnectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("gfx");
            var blob = container.GetBlockBlobReference(Guid.NewGuid().ToString());
            blob.Properties.ContentType = "image/jpeg";
            await blob.UploadFromByteArrayAsync(arBytes, 0, arBytes.Length);
            //await blob.UploadFromStreamAsync(stream);
            return blob.StorageUri.PrimaryUri.AbsoluteUri;
        }
    }
}
