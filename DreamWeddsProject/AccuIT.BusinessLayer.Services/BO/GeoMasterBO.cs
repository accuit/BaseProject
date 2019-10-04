using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class GeoMasterBO
    {
        public int GeoID { get; set; }
        public int CompanyID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<int> Level { get; set; }
        public Nullable<int> ParentGeoID { get; set; }
    }
}
