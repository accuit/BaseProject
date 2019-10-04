using AccuIT.PresentationLayer.WebAdmin.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopRegistrations;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.CommonLayer.AopContainer;
using System.Web.Optimization;

namespace AccuIT.PresentationLayer.WebAdmin
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        private IUserService userBusinessInstance;
        public IUserService UserBusinessInstance
        {
            get
            {
                if (userBusinessInstance == null)
                {
                    userBusinessInstance = AopEngine.Resolve<IUserService>(AspectEnums.AspectInstanceNames.UserManager, AspectEnums.ApplicationName.AccuIT);
                }
                return userBusinessInstance;
            }
        }
        protected void Application_Start()
        {
           // Database.SetInitializer<AccuIT.PresentationLayer.WebAdmin.CRMBizzEntities>(null);
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            //AuthConfig.RegisterOpenAuth();
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            UnityRegistration.InitializeAopContainer();
            string configFile = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.SchedulerConfigFile);
            log4net.Config.XmlConfigurator.Configure();
        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Set("Server", "AccuIT");
            Response.Headers.Remove("X-AspNet-Version"); //alternative to above solution
            Response.Headers.Remove("X-AspNetMvc-Version"); //alternative to above solution
            Response.Headers.Remove("X-Powered-By"); //alternative to above solution
        }

        protected void Session_Start(Object sender, EventArgs e)
        {
            Request.Cookies.Clear();
            SessionStateSection sessionState =
     (SessionStateSection)ConfigurationManager.GetSection("system.web/sessionState");
            string sidCookieName = sessionState.CookieName;

            if (Request.Cookies[sidCookieName] != null)
            {
                System.Web.HttpCookie sidCookie = Response.Cookies[sidCookieName];
                sidCookie.Value = Session.SessionID;
                sidCookie.HttpOnly = true;
                sidCookie.Secure = true;
                sidCookie.Path = "/";
            }
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        //[HandleError]
        protected void Application_Error(object sender, EventArgs e)
        {
            //Session.Abandon();
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
        protected void Session_End(Object sender, EventArgs e)
        {

            int loggenInUserID = Session[PageConstants.SESSION_USER_ID] != null ? Convert.ToInt32(Session[PageConstants.SESSION_USER_ID]) : 0;

            if (loggenInUserID > 0)
            {
                bool status = UserBusinessInstance.LogoutWebUser(loggenInUserID, Session.SessionID);
                Session.Abandon();
               // string loginURL = string.Format("~/Account/Login?ReturnUrl={0}", HttpUtility.UrlEncode(requestContext.Request.RawUrl));
            }
        }
    }
}