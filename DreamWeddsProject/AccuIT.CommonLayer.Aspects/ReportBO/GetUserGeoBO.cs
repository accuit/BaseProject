using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.ReportBO
{
   public class GetUserGeoBO
    {
        public string GEO { get; set; }
        public long UserID { get; set; }
        public int RoleID { get; set; }
        public int GeoID { get; set; }
        public Nullable<int> GeoLevelValue { get; set; }
        public string Branch { get; set; }
        public string Region { get; set; }
    }
}
