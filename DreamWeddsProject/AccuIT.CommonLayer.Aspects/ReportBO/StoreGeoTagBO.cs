using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.ReportBO
{
    public class StoreGeoTagBO
    {
        
        public int StoreID { get; set; }
        public long UserID { get; set; }
        public System.DateTime GeoTagDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public string PictureFileName { get; set; }
        public Nullable<bool> UserOption { get; set; }        
    }
}
