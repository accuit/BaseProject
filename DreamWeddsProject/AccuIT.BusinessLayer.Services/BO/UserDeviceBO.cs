using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    /// <summary>
    /// BO class for user devices
    /// </summary>
    public class UserDeviceBO
    {
        public int UserDeviceID { get; set; }
        public long UserID { get; set; }
        public string IMEINumber { get; set; }
        public string SenderKey { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
