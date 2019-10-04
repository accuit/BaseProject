using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.PersistenceLayer.Repository.Entities
{
    public class UserProfile
    {
        public int EmpID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool isDeleted { get; set; }
        public string UserID { get; set; }
        public string Password { get; set; }
        //        public Nullable<decimal> Salary { get; set; }
        public Nullable<System.DateTime> JoiningDate { get; set; }
        public string ImagePath { get; set; }
        public string EmpCode { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<int> AccountStatus { get; set; }
        public bool IsAdmin { get; set; }
        public int RoleID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public int empRoleID { get; set; }
        public List<UserWeddingTemplates> userWeddingTemplates { get; set; }
        public DateTime PurchaseDate { get; set; }
        public bool IsCustomer { get; set; }
    }

    public class UserWeddingTemplates
    {
        public int UserID { get; set; }
        public TemplateMaster userTemplate { get; set; }
        public Wedding userWedding { get; set; }
    }
}
