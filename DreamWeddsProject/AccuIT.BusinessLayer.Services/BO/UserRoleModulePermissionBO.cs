using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    /// <summary>
    /// Business object to fetch user Role Module
    /// Created By : Dhiraj
    /// Created On : 3-Dec-2013
    /// </summary>

    public class UserRoleModulePermissionBO
    {
        public long UserRolePermissionID { get; set; }
        public Nullable<int> RoleModuleID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<int> ModuleID { get; set; }
        public int PermissionID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public string PermissionValue { get; set; }
    }
}
