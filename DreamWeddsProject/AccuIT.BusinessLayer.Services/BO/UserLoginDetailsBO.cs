using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class UserLoginDetailsBO
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        //public bool IsGeoTagMandate { get; set; }
        //public bool IsGeoPhotoMandate { get; set; }
        //public bool IsStoreProfileVisible { get; set; }
        //public Nullable<bool> IsOfflineAccess { get; set; }
        //public Nullable<bool> ShowPerformanceTab { get; set; }
        //public Nullable<bool> IsRaceProfile { get; set; }
        //public bool ApkUpdated { get; set; }
        //public string ApkDownloadURL { get; set; }
        //public string DownloadList { get; set; }
        //public Nullable<bool> hasAttendance { get; set; }
        //public Nullable<byte> AttendanceType { get; set; }
        //public Nullable<bool> HasNewAnnouncment { get; set; }
        //public string MobileAnnoucment { get; set; }
        public int CompanyID { get; set; }
        public string UserCode { get; set; }
        public string LoginName { get; set; }
        public string FirstName { get; set; }
        //public Nullable<bool> IsRoamingProfile { get; set; }
        public Nullable<int> UserRoleID { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
       // public bool IsAttendanceMandate { get; set; } // SDCE-4401
       // public bool IsGeoFencingApplicable { get; set; }  // SDCE-4452

    }
}
