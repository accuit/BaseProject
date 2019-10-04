using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Samsung.SmartDost.BusinessLayer.Services.BO
{
    public class ParkingRoleMasterBO
    {
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public Nullable<int> TeamID { get; set; }
        public Nullable<int> GeoID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int ProfileLevel { get; set; }
        public bool IsAdmin { get; set; }
        public string CoverageType { get; set; }
        public Nullable<int> TargetOutlets { get; set; }
        public string MarketOffDays { get; set; }
        public bool IsReportProfile { get; set; }
        public bool IsEffectiveProfile { get; set; }
        public bool IsGeoTagMandate { get; set; }
        public bool IsGeoPhotoMandate { get; set; }
        public bool IsStoreProfileVisible { get; set; }
        public bool IsOfflineAccess { get; set; }
        public bool ShowPerformanceTab { get; set; }
        public bool IsRaceProfile { get; set; }
        public bool IsAttendanceMandate { get; set; }
        public bool IsGeoFencingApplicable { get; set; }
    }
}
