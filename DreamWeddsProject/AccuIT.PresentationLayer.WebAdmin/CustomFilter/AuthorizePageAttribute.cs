using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;
using AccuIT.PresentationLayer.WebAdmin.Core;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.BusinessLayer.Services.BO;
using System.Web;
using AccuIT.PresentationLayer.WebAdmin.Models;

namespace AccuIT.PresentationLayer.WebAdmin.CustomFilter
{
    public class AuthorizePageAttribute : AuthorizeAttribute
    {
        #region Properties and Private Fields

        private ISystemService systemBusinessInstance;
        private IUserService empBusinessInstance;
        private ISecurityService securityBusinessInstance;
        private int empID;
        private int roleID;
        private UserProfileBO userProfile;

        public class UniqueEmployee
        {
            public int? EmpID;
        }



        public int EmpID
        {
            get { return (int)HttpContext.Current.Session[PageConstants.SESSION_USER_ID]; }
            set { empID = value; }
        }

        public UserProfileBO UserProfile
        {
            get { return (UserProfileBO)HttpContext.Current.Session[PageConstants.SESSION_PROFILE_KEY]; }
            set { userProfile = value; }
        }
        public int RoleID
        {
            get { return (int)HttpContext.Current.Session[PageConstants.SESSION_ROLE_ID]; }
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

        private readonly string _moduleCode;
        private readonly string _roleID;

        public AuthorizePageAttribute(int paramModule, int paramRole)
        {
            _moduleCode = paramModule.ToString();
            _roleID = paramRole.ToString();
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            int moduleCode = Convert.ToInt32(_moduleCode);
            int roleID = Convert.ToInt32(_roleID);
            /*Create permission string based on the requested controller 
              name and action name in the format 'controllername-action'*/
            string requiredPermission = String.Format("{0}_{1}",
                   filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
                   filterContext.ActionDescriptor.ActionName);



            /*Create an instance of our custom user authorisation object passing requesting 
              user's 'Windows Username' into constructor*/
            UserAccessRules requestingUser = new UserAccessRules(filterContext.RequestContext
                                                   .HttpContext.User.Identity.Name);

            if (HttpContext.Current.Session[PageConstants.SESSION_USER_ID] == null)
            {
                var context = filterContext.HttpContext;
                string redirectTo = "~/Account/Login";
                if (!string.IsNullOrEmpty(context.Request.RawUrl))
                {
                    redirectTo = string.Format("~/Account/Login?ReturnUrl={0}",
                        HttpUtility.UrlEncode(context.Request.RawUrl));
                }
                filterContext.Controller.ViewBag.ShowPopup = true;
                filterContext.Controller.ViewBag.IsSuccess = false;
                filterContext.Controller.ViewBag.Message = "There was no activity since last 30 minutes. Your session is expired.";
            }
            else if (requestingUser.HasPermission(moduleCode) == null & !requestingUser.IsAdmin)
            {

                /*The custom '401 Unauthorized' access error will be returned to the 
                browser in response to the initial request.*/
                filterContext.Result = new RedirectToRouteResult(
                                               new RouteValueDictionary { 
                                                { "action", "UnAuthorizedUser" }, 
                                                { "controller", "Account" } });
            }
        }
    }
}