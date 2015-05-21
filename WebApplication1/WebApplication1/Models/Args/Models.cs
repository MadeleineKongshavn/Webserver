using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace WebApplication1.Models.Args
{
    public class UpdateGenreArgs
    {
        public int id{set;get;}
        public string[] newGenres{set;get;}
    }
    public class UpdateLocationArgs
    {
        public int id {set;get;}
        public string area{set;get;}
        public string placeId{set;get;}
    }
    public class GetGenreArgs
    {
        public int genreid{set;get;}
        public bool isChosen { set; get; }
        public string genreName{set;get;}
    }
    public class QueryArgs
    {
        public string queryString { set; get; }
    }
    public class RegisterUserArgs{
        public string userName { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }
    public class WwwLinkArgs
    {
        public int id { get; set; }
        public string link{ get; set;}


        internal void setId(int id)
        {
            this.id = id;
        }
        internal void setLink(string link)
        {
            this.link = link;
        }
    }
}