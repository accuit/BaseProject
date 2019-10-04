using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationLayer.DreamWedds.Web.Controllers
{
    public class ServicesController : Controller
    {
       [Route("our-services")]
        public ActionResult Index()
        {
            return View();
        }
	}
}