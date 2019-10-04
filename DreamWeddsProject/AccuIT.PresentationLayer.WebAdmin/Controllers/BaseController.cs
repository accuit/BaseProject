using AccuIT.BusinessLayer.Services.BO;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.CommonLayer.Aspects.ReportBO;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.PresentationLayer.WebAdmin.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace AccuIT.PresentationLayer.WebAdmin.Controllers
{
    public class BaseController : Controller
    {
        private ISystemService systemBusinessInstance;
        private IUserService empBusinessInstance;
        private ISecurityService securityBusinessInstance;
        private IWeddingService weddingBusinessInstance;
        private IEmailService emailBusinessInstance;
        private int userID;
        private int companyID;
        private int roleID;
        private UserProfileBO userProfile;

        public class UniqueEmployee
        {
            public int? UserID;
        }

        #region Properties

        public int UserID
        {
            get { return (int)Session[PageConstants.SESSION_USER_ID]; }
            set { userID = value; }
        }
        public int CompanyID
        {
            get { return (int)Session[PageConstants.SESSION_COMPANY_ID]; }
            set { companyID = value; }
        }
        public UserProfileBO UserProfile
        {
            get { return (UserProfileBO)Session[PageConstants.SESSION_PROFILE_KEY]; }
            set { userProfile = value; }
        }
        public int USERRoleID
        {
            get { return (int)Session[PageConstants.SESSION_ROLE_ID]; }
            set { roleID = value; }
        }



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
        public IWeddingService WeddingBusinessInstance
        {
            get
            {
                if (weddingBusinessInstance == null)
                {
                    weddingBusinessInstance = AopEngine.Resolve<IWeddingService>(AspectEnums.AspectInstanceNames.WeddingManager, AspectEnums.ApplicationName.AccuIT);
                }
                return weddingBusinessInstance;
            }
        }


        public string LastMinute
        {
            get { return " 11:00 PM"; }

        }

        #endregion

        //protected virtual ActionResult Index()
        //{
        //    return base.RedirectToAction("Index", "Home");
        //}
        //protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        //{
        //    bool IsValidUserSession = false;
        //    if (requestContext.HttpContext.Session[SessionVariables.AuthToken] != null && requestContext.HttpContext.Request.Cookies[CookieVariables.AuthToken] != null)
        //    {
        //        if (requestContext.HttpContext.Session[SessionVariables.AuthToken].ToString().Equals(requestContext.HttpContext.Request.Cookies[CookieVariables.AuthToken].Value))
        //        {
        //            if (!(requestContext.HttpContext.Session[PageConstants.SESSION_EMP_ID] == null || requestContext.HttpContext.Session[PageConstants.SESSION_ADMIN] == null))
        //            {
        //                #region  Validation for multiple session
        //                var dailyLoginHistory = EmpBusinessInstance.GetActiveLogin((int)requestContext.HttpContext.Session[PageConstants.SESSION_EMP_ID], (int)AspectEnums.AnnouncementDevice.Console);
        //                if (dailyLoginHistory.SessionID == requestContext.HttpContext.Session.SessionID )
        //                {
        //                    IsValidUserSession = true;
        //                    EmpProfile = (EmpProfileBO)requestContext.HttpContext.Session[PageConstants.SESSION_PROFILE_KEY];
        //                    EmpID = (int)requestContext.HttpContext.Session[PageConstants.SESSION_EMP_ID];
        //                    RoleID = (int)EmpProfile.RoleID;
        //                    //SetSessionData(EmpID, RoleID);

        //                }
        //                #endregion
        //            }
        //        }
        //    }
        //    if (!IsValidUserSession)
        //    {
        //        //string loginURL = string.Format(AppUtil.GetAppSettings(AspectEnums.ConfigKeys.LoginURL), AppUtil.GetAppSettings(AspectEnums.ConfigKeys.HostName));
        //        RedirectTo(2);
        //    }

        //    base.Initialize(requestContext);
        //    //this.HttpContext.Session.Keys = Session.SessionID;
        //    Response.Cache.SetNoStore();
        //    Response.Cache.AppendCacheExtension("no-cache");
        //    Response.Expires = 0;
        //}

        public ActionResult RedirectTo(int type)
        {
            if (type == 1)
            {
                return RedirectToAction("Login", "Account");
            }
            else if (type == 2)
            {
                return RedirectToAction("UnAuthorizedUser", "Account");
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }

        private void SetSessionData(int empID, int roleID)
        {

            //if (HttpContext.Session[SessionVariables.EmployeeListUnderCurrentUser] == null)
            //{
            //IList<UserModuleDTO> modules = HttpContext.Session[PageConstants.SESSION_MODULES] as List<UserModuleDTO>;
            // List<int> assingnedAuthorizedModule = modules.Select(x => x.ModuleCode.Value).Distinct().ToList();
            // List<int> modulesToBeChecked = new List<int>();
            DateTime DateFrom = DateTime.Now;
            DateTime DateTo = DateTime.Now;

            var employeeList = new List<EmployeeHierarchyBO>();

            //employeeList = null;// ReportBusinessInstance.GetEmployeesHierachyUnderUser(userID);
            //HttpContext.Session[SessionVariables.EmployeeListUnderCurrentUser] = employeeList;
            //var uniqueEmployeeList = employeeList.Select(x => new UniqueEmployee { UserID = x.UserID.Value }).Distinct().ToList();

            GetRoleMaster();

            // }
        }



        private void GetRoleMaster()
        {
            var roleMaster = EmpBusinessInstance.GetRoleMaster();
            HttpContext.Session[SessionVariables.RoleMasters] = roleMaster.ToList();
        }


        private void RemoveAllCache()
        {
            // Code disables caching by browser.
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
            Response.Cache.SetNoStore();
        }

        protected void AuthorizePage(AspectEnums.WebModules module)
        {
            bool isValid = false;

            IList<UserModuleDTO> modules = Session[PageConstants.SESSION_MODULES] as List<UserModuleDTO>;
            if (modules.Count > 0)
            {
                isValid = modules.Count(k => k.ModuleCode == (int)module) > 0 ? true : false;
                if (isValid)
                {
                    isValid = IsPermission(1, module);
                }
            }
            //}
            if (!isValid)
            {
                Response.Redirect("../Account/UnAuthorizedUser.aspx", true);
            }
        }

        protected bool IsPermission(int permissionID, AspectEnums.WebModules module)
        {
            bool isPermission = false;
            IList<SecurityAspectBO> permissions = Session[PageConstants.SESSION_PERMISSIONS] as List<SecurityAspectBO>;
            if (permissions != null && permissions.Count > 0)
            {
                var perm = permissions.FirstOrDefault(k => k.ModuleCode == (int)module && k.PermissionID == (int)AspectEnums.RolePermissionEnums.View);
                if (perm != null)
                {
                    isPermission = true;
                }
            }
            return isPermission;
        }


        public string GetCurrentPageModuleName(AspectEnums.WebModules module)
        {
            string CurrentModuleName = string.Empty;
            IList<UserModuleDTO> moduleList = new List<UserModuleDTO>();
            moduleList = (IList<UserModuleDTO>)Session[PageConstants.SESSION_MODULES];
            UserModuleDTO moduleDTO = moduleList.FirstOrDefault(m => m.ModuleCode == (int)module);
            if (moduleDTO != null)
                CurrentModuleName = moduleDTO.Name;
            return CurrentModuleName;
        }


    }
}
