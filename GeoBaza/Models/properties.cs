using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GeoBaza.Models
{
    public class features
    {
        public string type { get; set; }
        public properties properties { get; set; }
        public geometry geometry { get; set; }
    }

    public class properties
    {
        public string fclass { get; set; }
        public string name { get; set; }
        public int gid { get; set; }
        public string address { get; set; }

    }

    public class geometry
    { public string type { get; set; }
      public decimal[] coordinates { get; set; }

    }

    public class crs
    {
        private name1 n = new name1();
        public string type { get{return "name";} }
        public name1 properties { get { return n; } }
    }

    public class name1
    {
        public string name { get { return "urn:ogc:def:crs:OGC:1.3:CRS84"; } }
    }
}