using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.CommonLayer.Aspects.Utilities;
using System;
using System.IO;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;

namespace AccuIT.CommonLayer.Aspects.Module
{
    /// <summary>
    /// File Processor class to provide requested file from file server
    /// </summary>
    public class SessionHandler : IHttpHandler, IReadOnlySessionState, IRequiresSessionState
    {


        /// <summary>
        /// Http Handler interface to process the http web context request
        /// </summary>
        /// <param name="context">http context</param>
        public void ProcessRequest(HttpContext context)
        {
            var cont = context.Session;            
            using (var reader = new StreamReader(context.Request.InputStream))
            {
                // This will equal to "charset = UTF-8 & param1 = val1 & param2 = val2 & param3 = val3 & param4 = val4"
                string values = reader.ReadToEnd();
                var @params = HttpUtility.ParseQueryString(values);                
                context.Session[SessionVariables.CurrentUserHierarchyForReport] = @params["currentUserHierarchy"];
                context.Session[SessionVariables.CurrentUserLevelForReport] = Convert.ToInt32(@params["currentUserLevel"]) ;
                context.Session[SessionVariables.CurrentUserBreadCumForReport] = @params["breadCum"];
                context.Session[SessionVariables.SelectedReportDateFrom] = @params["dateFrom"];
                context.Session[SessionVariables.SelectedReportDateTo] = @params["dateTo"];
                context.Session[SessionVariables.CurrentSelectedRoleForReport] = @params["currentSelectedRole"];

                
            }

        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
