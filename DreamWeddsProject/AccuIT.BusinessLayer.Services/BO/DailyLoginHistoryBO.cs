using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class DailyLoginHistoryBO
    {
        public long DailyLoginID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public string SessionID { get; set; }
        public string IpAddress { get; set; }
        public string BrowserName { get; set; }
        public Nullable<int> LoginType { get; set; }
        public Nullable<System.DateTime> LogOutTime { get; set; }
        public Nullable<System.DateTime> LoginTime { get; set; }
        public bool IsLogin { get; set; }

        public virtual UserMasterBO EmployeeMasterBO { get; set; }
    }
}
