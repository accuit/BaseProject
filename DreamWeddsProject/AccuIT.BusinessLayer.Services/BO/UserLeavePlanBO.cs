using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class UserLeavePlanBO
    {
        public long PlanLeaveID { get; set; }
        public System.DateTime LeaveDate { get; set; }
        public System.DateTime LeaveToDate { get; set; }
        public int LeaveTypeID { get; set; }
        public long UserID { get; set; }
        public string Remarks { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
