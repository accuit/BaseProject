using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class UserStoresTodayBeatBO
    {
        public string StoreCode { get; set; }
        public string ShipToRegion { get; set; }
        public string ShipToBranch { get; set; }
        public string SoldToCode { get; set; }
        public string ShipToCode { get; set; }
        public string AccId { get; set; }
        public string StoreName { get; set; }
        public string AccountName { get; set; }
        public int StoreID { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public bool IsActive { get; set; }
        public long UserID { get; set; }
        public long UserRoleID { get; set; }
        public long StoreUserID { get; set; }
        public Nullable<System.DateTime> LastVisitDate { get; set; }
        public System.DateTime CoverageDate { get; set; }
        public long CoverageID { get; set; }
        public bool IsCoverage { get; set; }
    }
}
