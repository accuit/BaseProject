using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace AccuIT.BusinessLayer.Services.BO
{
    /// <summary>
    /// Business object to fetch user details only
    /// </summary>
    public class UserBO
    {
        /// <summary>
        /// Property to get/set User ID
        /// </summary>
        public long UserID
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get/set First Name
        /// </summary>
        public string FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get/set Last Name
        /// </summary>
        public string LastName
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get/set Middle Name
        /// </summary>
        public string MiddleName
        {
            get;
            set;
        }

    }


    public class UserMasterBO
    {
        public int UserID { get; set; }
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        public bool isDeleted { get; set; }
        //[Required]
        public string LoginName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public Nullable<decimal> Salary { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public string ImagePath { get; set; }
        public Nullable<int> AccountStatus { get; set; }
        public Nullable<bool> isActive { get; set; }
        public Nullable<int> SeniorEmpID { get; set; }

        public Nullable<bool> IsEmployee { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public int CreatedBy { get; set; }

        public int TemplateID { get; set; }

        //public virtual ICollection<DailyLoginHistoryBO> DailyLoginHistories { get; set; }
        public virtual ICollection<UserWeddingSubscriptionBO> UserWeddingSubscriptions { get; set; }
        public virtual ICollection<OrderMasterBO> OrderMasters { get; set; }
        public virtual ICollection<UserRoleBO> UserRoles { get; set; }
    }
}
