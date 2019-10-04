using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AccuIT.BusinessLayer.Services.BO
{
    /// <summary>
    /// Business object to fetch user Role Module
    /// Created By : Dhiraj
    /// Created On : 3-Dec-2013
    /// </summary>

    public class RoleMasterBO
    {
        public int RoleID { get; set; }

        [Required]
        [Display(Name = "Role Name")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Role Code")]
        public string Code { get; set; }
        public Nullable<int> Type { get; set; }

        [Required]
        public int Status { get; set; }
        public bool IsAdmin { get; set; }
        public Nullable<bool> IsDeleted { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual UserMasterBO employeeMasterBO { get; set; }
        public virtual ICollection<UserRoleBO> empRolesBO { get; set; }
        public virtual ICollection<RoleModuleBO> RoleModulesBO { get; set; }

        public List<RoleMasterBO> lstRoleMaster { get; set; }

    }

    public class RoleManagerBO
    {
        public RoleMasterBO roleMasterBO { get; set; }

        public List<RoleMasterBO> lstRoleMaster { get; set; }

    }


    public  class RoleModuleBO
    {


        public int RoleModuleID { get; set; }
        public int RoleID { get; set; }
        public int ModuleID { get; set; }
        public string ModuleName { get; set; }
        public int Sequence { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }

        public List<String> selectedModules { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

    }
}
