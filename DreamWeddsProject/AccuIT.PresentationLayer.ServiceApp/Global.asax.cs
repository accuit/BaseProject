using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopRegistrations;
using AccuIT.CommonLayer.Aspects.Utilities;
using Spring.Context.Support;
using System;
using System.Configuration;
using System.Web.Configuration;
using AccuIT.PresentationLayer.ServiceImpl.CORS;
using AccuIT.CommonLayer.AopContainer;
using System.Web;
using System.Linq;
using System.Web.Routing;
using System.ServiceModel.Activation;
using System.IO;

namespace AccuIT.PresentationLayer.ServiceApp
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            UnityRegistration.InitializeAopContainer();
            string configFile = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.SchedulerConfigFile);
            log4net.Config.XmlConfigurator.Configure();
            //XmlApplicationContext context = new XmlApplicationContext(Server.MapPath(configFile));
           // Stream stream = HttpContext.Current.Request.InputStream;
            // RouteTable.Routes.Add(new ServiceRoute("api", new CorsEnabledServiceHostFactory(), typeof(ValuesService)));
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
           
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            if (HttpContext.Current.Request.HttpMethod == "OPTIONS")
            {

                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Methods", "OPTIONS, GET, POST, PUT, DELETE");
                HttpContext.Current.Response.AddHeader("Content-Type", "application/json; charset=UTF-8");
                HttpContext.Current.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Authorization, No-Auth, APIKey, APIToken, userID");
                HttpContext.Current.Response.AddHeader("Access-Control-Max-Age", "1728000");
                HttpContext.Current.Response.End();
            }
        }

        //private bool IsAllowedDomain(String Domain)
        //{
        //    if (string.IsNullOrEmpty(Domain)) return false;
        //    string alloweddomains = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.CORSAllowedDomains).ToString(); // you can place comma separated domains here.
        //    foreach (string alloweddomain in alloweddomains.Split(',').ToList())
        //    {
        //        if (Domain.ToLower() == alloweddomain.ToLower())
        //            return true;
        //    }
        //    return false;
        //}

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
        protected void Application_PreSendRequestHeaders()
        {
            //Response.Headers.Remove("Server");
            Response.Headers.Set("Server", "DreamWedds");
            Response.Headers.Remove("X-AspNet-Version"); //alternative to above solution
            Response.Headers.Remove("X-AspNetMvc-Version"); //alternative to above solution
            Response.Headers.Remove("X-Powered-By"); //alternative to above solution
        }
    }
}