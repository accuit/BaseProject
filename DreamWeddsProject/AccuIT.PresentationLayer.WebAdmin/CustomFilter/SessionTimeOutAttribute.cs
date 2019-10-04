using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using AccuIT.PresentationLayer.WebAdmin.Core;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.BusinessLayer.Services.BO;

namespace AccuIT.PresentationLayer.WebAdmin.CustomFilter
{

    [AttributeUsage(AttributeTargets.Class |
    AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class SessionTimeOutAttribute : ActionFilterAttribute
    {
        private ISystemService systemBusinessInstance;
        private IUserService empBusinessInstance;
        private ISecurityService securityBusinessInstance;
        private int empID;
        private int companyID;
        private int roleID;
        private UserProfileBO userProfile;
        private int weddingID;

        public class UniqueEmployee
        {
            public int? EmpID;
        }

        #region Properties

        public int EmpID
        {
            get { return (int)HttpContext.Current.Session[PageConstants.SESSION_USER_ID]; }
            set { empID = value; }
        }
        public int CompanyID
        {
            get { return (int)HttpContext.Current.Session[PageConstants.SESSION_COMPANY_ID]; }
            set { companyID = value; }
        }
        public UserProfileBO EmpProfile
        {
            get { return userProfile; }
            set { userProfile = value; }
        }
        public int RoleID
        {
            get { return roleID; }
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


        public string LastMinute
        {
            get { return " 11:00 PM"; }

        }

        #endregion


        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpContext requestContext = HttpContext.Current;

            bool IsValidUserSession = false;
            if (requestContext.Session[SessionVariables.AuthToken] != null && requestContext.Request.Cookies[CookieVariables.AuthToken] != null)
            {
                if (requestContext.Session[SessionVariables.AuthToken].ToString().Equals(requestContext.Request.Cookies[CookieVariables.AuthToken].Value))
                {
                    if (!(requestContext.Session[PageConstants.SESSION_USER_ID] == null || requestContext.Session[PageConstants.SESSION_ADMIN] == null))
                    {
                        #region  Validation for multiple session
                        var dailyLoginHistory = EmpBusinessInstance.GetActiveLogin((int)requestContext.Session[PageConstants.SESSION_USER_ID], (int)AspectEnums.AnnouncementDevice.Console);
                        if (dailyLoginHistory.SessionID == requestContext.Session.SessionID || requestContext.Session[PageConstants.SESSION_ADMIN] != null)
                        {
                            IsValidUserSession = true;
                            EmpProfile = (UserProfileBO)requestContext.Session[PageConstants.SESSION_PROFILE_KEY];
                            EmpID = (int)requestContext.Session[PageConstants.SESSION_USER_ID];
                            RoleID = (int)EmpProfile.RoleID;
                            SetSessionData(EmpID, RoleID);
                        }
                        #endregion
                    }
                }
            }
            if (!IsValidUserSession)
            {
                string loginURL = "~/Account/Login";
                //string loginURL = string.Format(AppUtil.GetAppSettings(AspectEnums.ConfigKeys.LoginURL), AppUtil.GetAppSettings(AspectEnums.ConfigKeys.HostName));
                loginURL = string.Format("~/Account/Login?ReturnUrl={0}", HttpUtility.UrlEncode(requestContext.Request.RawUrl));
                filterContext.Result = new RedirectResult(loginURL);
                return;
            }
            base.OnActionExecuting(filterContext);
        }

        private void SetSessionData(int empID, int roleID)
        {

            if (HttpContext.Current.Session[SessionVariables.EmployeeListUnderCurrentUser] == null)
            {
                List<int> modulesToBeChecked = new List<int>();
                DateTime DateFrom = DateTime.Now;
                DateTime DateTo = DateTime.Now;

                var roleMaster = EmpBusinessInstance.GetRoleMaster();
                HttpContext.Current.Session[SessionVariables.RoleMasters] = roleMaster.ToList();
            }

            //Get UserModuleList
            var userModules = EmpBusinessInstance.GetUserWebModules(empID);

        }
    }
}