using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.ReportBO
{
    /// <summary>
    /// Business object to fetch user Role authorization data
    /// Created By : Dhiraj
    /// Created On : 3-Dec-2013
    /// </summary>
    [Serializable]
    public class RoleMasterBO
    {

        public int RoleID { get; set; }
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
        public bool IsReportProfile{get;set;}
        public bool IsEffectiveProfile { get; set; }
        public bool IsGeoTagMandate { get; set; }
        public bool IsGeoPhotoMandate { get; set; }
        public bool IsStoreProfileVisible { get; set; }
        public bool IsOfflineAccess { get; set; }
        public bool ShowPerformanceTab { get; set; }
        public bool IsRaceProfile { get; set; }
        public bool IsAttendanceMandate { get; set; } // SDCE-4401
        public bool IsGeoFencingApplicable { get; set; } // SDCE-4452

    }
}
