using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace Samsung.SmartDost.BusinessLayer.Services.BO
{
    public class ParkingUserMasterBO
    {
        public int CompanyID { get; set; }
        public string UserCode { get; set; }
        public string EmplCode { get; set; }
        public string FirstName { get; set; }
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
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsPinRegistered { get; set; }
        public bool IsDeleted { get; set; }
        public string DistyCode { get; set; }
        public string DistyName { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
