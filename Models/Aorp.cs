using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace OnlineConverter.Models
{
    public class Aorp
    {
        public HttpPostedFileBase path { get; set; }

        public int codec { get; set; }

        public int bitrate { get; set; }

        public int ratio { get; set; }

        public HttpPostedFileBase pathImage { get; set; }
 
     
    }
}