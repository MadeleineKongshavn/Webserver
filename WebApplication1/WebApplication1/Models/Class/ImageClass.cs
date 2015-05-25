using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace WebApplication1.Models.Class
{
    public class ImageClass
    {
        public int BandId { get; set; }
        public String UnderTitle { get; set; }
        public String Title { get; set; }
        public String SmallBitmapUrl { get; set; }
        public Double YCoordinates { get; set; }
        public Double XCoordinates { get; set; }
        public Double OpositeXCoordinates { get; set; }
        public Double OpositeYCoordinates { get; set; }

    }
}