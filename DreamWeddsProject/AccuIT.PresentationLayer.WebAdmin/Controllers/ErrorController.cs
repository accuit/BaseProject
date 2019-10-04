using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuIT.PresentationLayer.WebAdmin.Controllers
{
    public class ErrorController : Controller
    {

        public ActionResult NotFound()
        {
            Exception exception = Server.GetLastError();
            Response.Clear();

            HttpException httpException = exception as HttpException;

            if (httpException != null)
            {
                string action;

                switch (httpException.GetHttpCode())
                {
                    case 404:
                        // page not found
                        action = "Error";
                        break;
                    case 500:
                        // server error
                        action = "Error";
                        break;
                    default:
                        action = "Error";
                        break;
                }

                // clear error on server
                Server.ClearError();

                
            }
            return View();
        }

    }
}
