using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.ReportBO
{
    public class SurveyResponseBO
    {
        public long SurveyResponseID { get; set; }
        public long UserID { get; set; }
        public Nullable<int> GeoID { get; set; }
        public Nullable<int> StoreID { get; set; }
        public Nullable<int> ModuleID { get; set; }
        public Nullable<int> ModuleCode { get; set; }
        public string Comments { get; set; }
        public Nullable<long> CoverageID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string GeoTag { get; set; }
        public string PictureFileName { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public bool IsOffline { get; set; }
        public Nullable<System.DateTime> BeatPlanDate { get; set; }
        public Nullable<int> UserDeviceID { get; set; }
        public string ChannelType { get; set; }
    }
}
