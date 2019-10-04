using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.ReportBO
{
    public class EmployeeHierarchyBO
    {
        public string FirstName { get; set; }
        public Nullable<long> UserRoleID { get; set; }
        public Nullable<int> RoleID { get; set; }
        public Nullable<long> UserID { get; set; }
        public Nullable<long> ReportingUserID { get; set; }
        public int CompanyID { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public Nullable<int> TeamID { get; set; }
        public Nullable<int> ReportingTeamID { get; set; }
        public Nullable<int> ProfileLevel { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
        public Nullable<int> EmpLevel { get; set; }
        public string UserCode { get; set; }
        public string EmplCode { get; set; }
        public string FirstName1 { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string MySingleID { get; set; }
        public string Mobile_Calling { get; set; }
        public string Mobile_SD { get; set; }
        public string EmailID { get; set; }
        public bool IsSSOLogin { get; set; }
        public string UserCommunicationGroup { get; set; }
        public Nullable<int> ProductTypeID { get; set; }
        public Nullable<System.DateTime> EnrollmentDate { get; set; }
        public int AccountStatus { get; set; }
        public bool IsOfflineProfile { get; set; }
        public string ProfilePictureFileName { get; set; }
        public string Address { get; set; }
        public string Pincode { get; set; }
        public string AlternateEmailID { get; set; }
        public bool IsPinRegistered { get; set; }
        public string AndroidRegistrationId { get; set; }
        public Nullable<int> GeoID { get; set; }
        public string CoverageType { get; set; }
        public Nullable<int> TargetOutlets { get; set; }
        public string MarketOffDays { get; set; }
        public string DistyCode { get; set; }
        public string DistyName { get; set; }
    }
}
