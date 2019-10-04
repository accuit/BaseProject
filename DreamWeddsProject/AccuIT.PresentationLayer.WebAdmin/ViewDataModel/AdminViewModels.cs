using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.BusinessLayer.Services.BO;
using System.Web.Mvc;

namespace AccuIT.PresentationLayer.WebAdmin.ViewDataModel
{
    public class AdminViewModels
    {
    }
    public class RoleModulesViewModel
    {

        public IEnumerable<SelectListItem> ListUnAssignedModules { get; set; }
        public IEnumerable<String> selectedUnassignedModules { get; set; }

        public IEnumerable<SelectListItem> ListAssignedModules { get; set; }
        public IEnumerable<String> selectedAssignedModules { get; set; }

        public int RoleID { get; set; }

        public IEnumerable<SelectListItem> ListRoles { get; set; }
        public IEnumerable<String> selectedRole { get; set; }

    }

    public class RolePermissionsViewModel
    {

        public IEnumerable<SelectListItem> ListRoles { get; set; }
        public IEnumerable<String> selectedRole { get; set; }

        public IEnumerable<SelectListItem> ListModules { get; set; }
        public bool isCheckedModules { get; set; }

        public List<PermissionBO> ListPermissions { get; set; }
        public bool checkedPermissions { get; set; }

        public int selectedRoleID { get; set; }

    }

    public class ModulesViewModel
    {
       public ModuleMasterBO moduleMasterBO { get; set; }
        public List<ModuleMasterBO> ListModuleMaster { get; set; }


        public IEnumerable<SelectListItem> ListParentModules { get; set; }
        public int selectedModuleID { get; set; }
    }
}