﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Class
{
    public class NotificationsClass
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
        public Boolean Seen { get; set; }
        public int Type { get; set; }

        // hvis det er en venneforespørsel
        public Boolean Accepted { get; set; }
        public Boolean Answered { get; set; }
        public int FriendId { get; set; }
        public String FriendName { get; set; }
        public String Url { get; set; }
        public Byte[] ByteArray { get; set; }


    }
}