using AccuIT.BusinessLayer.Services.BO;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.CommonLayer.Aspects.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using AccuIT.CommonLayer.Aspects.Logging;
using AccuIT.CommonLayer.Aspects.DTO;


namespace AccuIT.PresentationLayer.WebAdmin.Models
{
    public class DropDown
    {
        List<ModuleMasterBO> allModulesBO = new List<ModuleMasterBO>();
        List<ModuleMasterBO> AssignedModules = new List<ModuleMasterBO>();


        #region Private Methods of BusinessInstance Initialization

        private ISystemService systemBusinessInstance;
        private IUserService empBusinessInstance;
        private ISecurityService securityBusinessInstance;
        public ISystemService SystemBusinessInstance
        {
            get
            {
                if (systemBusinessInstance == null)
                {
                    systemBusinessInstance = AopEngine.Resolve<ISystemService>(AspectEnums.AspectInstanceNames.ServiceManager, AspectEnums.ApplicationName.AccuIT);
                }
                return systemBusinessInstance;
            }
        }
        public ISecurityService SecurityBusinessInstance
        {
            get
            {
                if (securityBusinessInstance == null)
                {
                    securityBusinessInstance = AopEngine.Resolve<ISecurityService>(AspectEnums.AspectInstanceNames.SecurityManager, AspectEnums.ApplicationName.AccuIT);
                }
                return securityBusinessInstance;
            }
        }
        public IUserService EmpBusinessInstance
        {
            get
            {
                if (empBusinessInstance == null)
                {
                    empBusinessInstance = AopEngine.Resolve<IUserService>(AspectEnums.AspectInstanceNames.UserManager, AspectEnums.ApplicationName.AccuIT);
                }
                return empBusinessInstance;
            }
        }

        #endregion

        #region Private Methods for Getting List from Database

        private List<RoleModuleBO> GetRoleModules(int RoleID)
        {
            return SystemBusinessInstance.GetRoleModulesByRoleID(RoleID, null);
        }
        private List<RoleMasterBO> GetRoleMaster()
        {
            return SystemBusinessInstance.GetRoleMasters();
        }
        private List<ModuleMasterBO> FillModules()
        {
            return SystemBusinessInstance.GetModuleList().ToList(); ;

        }

        public List<ModuleMasterBO> GetAssignedRoleModules(int roleID)
        {
            allModulesBO = FillModules();
            AssignedModules = (from roleModule in GetRoleModules(roleID).Where(x => x.IsDeleted == false)
                               join module in allModulesBO on roleModule.ModuleID equals module.ModuleID
                               select module).ToList();
            return AssignedModules;
        }

        #endregion

        public List<SelectListItem> GetCSVMastersList()
        {
            List<SelectListItem> master = new List<SelectListItem>();
            master.Add(new SelectListItem() { Text = "--Select--", Value = "-1" });
            master = GetCSVEnumsList<AspectEnums.enumExcelType>().ToList();

           ActivityLog.SetLog("GetCSVMastersList completed", LogLoc.DEBUG);

            return master;
        }
        public static IEnumerable<SelectListItem> GetCSVEnumsList<T>()
        {
            return (Enum.GetValues(typeof(T)).Cast<T>().Select(
                enu => new SelectListItem() { Text = enu.ToString(), Value = Convert.ToInt32(enu).ToString() })).ToList();
        }

        public List<SelectListItem> GetBrideGroomRelations()
        {
            var Relations = SystemBusinessInstance.GetCommonSetup(0, "Relation", Convert.ToString((int)AspectEnums.CommonTableMainType.Wedding)).ToList();
            List<SelectListItem> relationsListItem = new List<SelectListItem>();
            relationsListItem.Add(new SelectListItem() { Text = "--Select--", Value = "-1" });
            foreach (CommonSetupDTO relation in Relations)
            {
                relationsListItem.Add(new SelectListItem() { Text = relation.DisplayText, Value = relation.DisplayValue.ToString() });
            }
            
            return relationsListItem;
        }
     


        public IEnumerable<SelectListItem> GetAllRolesList()
        {
            var listRoles = SystemBusinessInstance.GetRoleMasters();
            List<SelectListItem> rolesListItem = new List<SelectListItem>();
            rolesListItem.Add(new SelectListItem() { Text = "--Select--", Value = "" });
            foreach (RoleMasterBO roles in listRoles)
            {
                rolesListItem.Add(new SelectListItem() { Text = roles.Name, Value = roles.RoleID.ToString() });
            }

            return rolesListItem;
        }

        public IEnumerable<SelectListItem> GetAssignedModulesList(int roleID)
        {

            List<SelectListItem> assignedSelectListItem = new List<SelectListItem>();
            AssignedModules = GetAssignedRoleModules(roleID);

            foreach (ModuleMasterBO modules in AssignedModules)
            {
                SelectListItem selectListItem = new SelectListItem()
                {
                    Text = modules.Name,
                    Value = modules.ModuleID.ToString(),
                    Selected = modules.IsSelected
                };
                assignedSelectListItem.Add(selectListItem);
            }

            return assignedSelectListItem;
        }

        public IEnumerable<SelectListItem> GetUnAssignedModulesList(int roleID)
        {
            List<ModuleMasterBO> ListModules = new List<ModuleMasterBO>();
            allModulesBO = FillModules();
            AssignedModules = GetAssignedRoleModules(roleID);
            var UnAssignedModules = allModulesBO.Except(AssignedModules);

            List<SelectListItem> unAssignedSelectListItem = new List<SelectListItem>();
            foreach (ModuleMasterBO modules in UnAssignedModules)
            {
                unAssignedSelectListItem.Add(new SelectListItem()
                {
                    Text = modules.Name,
                    Value = modules.ModuleID.ToString(),
                    Selected = modules.IsSelected
                });
            }

            return unAssignedSelectListItem;
        }

        public IEnumerable<SelectListItem> GetParentModuleNamesList(List<ModuleMasterBO> listModules)
        {
            List<SelectListItem> parentModuleListItem = new List<SelectListItem>();

            var listparents = listModules.Where(x => x.ParentModuleID == null);

            foreach (var item in listparents)
            {
                parentModuleListItem.Add(new SelectListItem() { Text = item.Name, Value = item.ModuleID.ToString() });
            }

            return parentModuleListItem;
        }

        public string GetModuleMasterNames(List<ModuleMasterBO> listModules)
        {
            string ParentName = "";
            foreach (var item in listModules)
            {
                if (item.ParentModuleID != null)
                {

                    var parentList = listModules.Where(k => k.ModuleID == item.ParentModuleID).FirstOrDefault();
                    if (parentList != null)
                        ParentName = parentList.Name;
                }
                else
                {
                    ParentName = "No Parent";
                }
            }
            return ParentName;
        }


    }



}