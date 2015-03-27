using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

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
    }
}
