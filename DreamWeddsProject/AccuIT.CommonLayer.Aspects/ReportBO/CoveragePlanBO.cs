using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.ReportBO
{
    public class CoveragePlanBO
    {
        public long CoverageID { get; set; }
        public int CompanyID { get; set; }
        public long UserID { get; set; }
        public int StoreID { get; set; }
        public System.DateTime CoverageDate { get; set; }
        public bool IsCoverage { get; set; }
        public int StatusID { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> StatusUpdateLastDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
    }
}
