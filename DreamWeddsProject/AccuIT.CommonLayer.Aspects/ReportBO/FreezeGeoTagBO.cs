using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.ReportBO
{    
        public class FreezeGeoTagBO
        {
            public string Zone { get; set; }
            public string Branch { get; set; }
            public string StoreCode { get; set; }
            public string StoreName { get; set; }
            public double Lattitude { get; set; }
            public double Longitude { get; set; }
            public int Occurance { get; set; }

        }
    
}
