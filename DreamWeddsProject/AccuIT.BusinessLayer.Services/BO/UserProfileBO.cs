using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccuIT.BusinessLayer.Services.BO
{
    /// <summary>
    /// Business object to fetch user profile details only
    /// </summary>
    [Serializable]
    public class UserProfileBO
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool isDeleted { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public string ImagePath { get; set; }
        public string UserCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public int AccountStatus { get; set; }
        public bool IsAdmin { get; set; }
        public int RoleID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public int userRoleID { get; set; }
       // public UserWeddingTemplateSubscriptionsBO userWeddingTemplSubscriptions { get; set; }
       // public List<UserWeddingSubscriptionBO> UserWeddingSubscriptions { get; set; }
       // public Nullable<System.DateTime> PurchaseDate { get; set; }
        public bool IsEmployee { get; set; }
        public int CompanyID { get; set; }
        public AddressMasterBO Address { get; set; }
    }

    public class UserWeddingTemplateSubscriptionsBO
    {
        public int UserWeddingSubscrptionID { get; set; }
        public int UserId { get; set; }
        public int InvoiceNo { get; set; }
        public int SubscriptionType { get; set; }
        public AccuIT.CommonLayer.Aspects.Utilities.AspectEnums.SubscriptionType SubscriptionTypeList { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string ReasonOfUpdate { get; set; }
        public int? SubscriptionStatus { get; set; }
        
        public List<TemplateMasterBO> Templates { get; set; }
    }



}
