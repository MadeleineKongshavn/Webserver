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
        public String area{set;get;}
        public String placeId{set;get;}
    }

    public class GetGenreArgs
    {
        public int genreid{set;get;}
        public bool isChosen { set; get; }
        public String genreName{set;get;}
    }
}