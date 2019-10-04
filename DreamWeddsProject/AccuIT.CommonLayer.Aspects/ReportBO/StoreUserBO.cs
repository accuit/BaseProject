using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.ReportBO
{
    [Serializable]
    public class StoreUserBO
    {
        public long StoreUserID { get; set; }
        public int StoreID { get; set; }
        public long UserID { get; set; }
        public long UserRoleID { get; set; }
        public string StoreCode { get; set; }
        public string UserCode { get; set; }
        public Nullable<System.DateTime> LastVisitDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string VisitSummary { get; set; }
        public bool IsDeleted { get; set; }

       
    }
}
