using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
     public class LMSLeaveTypeConfigurationBO
    {
        public int LMSLeaveTypeConfigurationID { get; set; }
        public int LMSLeaveTypeMasterID { get; set; }
        public int RoleID { get; set; }
        public int ConfigSetupID { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int ConfigValue { get; set; }
    }
}
