using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccuIT.BusinessLayer.Services.BO
{
    /// <summary>
    /// Business object to fetch user role details only
    /// </summary>
    public class UserRoleBO
    {
        public int UserRoleID { get; set; }
        public int RoleID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> isDeleted { get; set; }

        
    }
}
