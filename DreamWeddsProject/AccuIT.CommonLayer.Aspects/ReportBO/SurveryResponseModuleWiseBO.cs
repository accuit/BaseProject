using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.ReportBO
{
    public class SurveryResponseModuleWiseBO
    {
        public long SurveyUserResponseID { get; set; }
        public long SurveyResponseID { get; set; }
        public int SurveyQuestionID { get; set; }
        public string Question { get; set; }
        public string UserResponse { get; set; }
        public int SurveyTypeID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public int ModuleID { get; set; }
        public Nullable<int> ModuleCode { get; set; }
        public string Name { get; set; }
        public long UserID { get; set; }
        public Nullable<int> StoreID { get; set; }
        public int RoleId { get; set; }
        public string StoreName { get; set; }
        public string RoleName { get; set; }
        public string StoreCode { get; set; }
        public string ShipToRegion { get; set; }
        public string State { get; set; }
        public string ShipToBranch { get; set; }
        public string City { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public string ShipToName { get; set; }
        public string UserCode { get; set; }
        public string EmplCode { get; set; }
        public string FullName { get; set; }
        public string Username { get; set; }
        public int AccountStatus { get; set; }
        public string Mobile_Calling { get; set; }
        public string EmailID { get; set; }
        public string ProfilePictureFileName { get; set; }
    }
}
