using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PresentationLayer.DreamWedds.Web.Controllers
{
    public class WeddingController : Controller
    {

       // WeddingWebService.DreamWeddsClient wedding = new WeddingWebService.DreamWeddsClient();
        //[Route("{wedding}/{id}/{title}")]
        //public ActionResult Index(int Id, string title = "title")
        //{

        //   // WeddingWebService.WeddingDTOResponse weddingDTO = new WeddingWebService.WeddingDTOResponse();
        //    weddingDTO = wedding.GetWeddingDetailByID(Convert.ToInt32(Id));
        //    if(weddingDTO.IsSuccess)
        //    {
        //        //return File("~/Templates/Wedding/" + weddingDTO.SingleResult.TemplatePreviewUrl + "/index.html", "text/html"); 
        //        return Redirect("http://dreamwedds.com/templates/wedding/" + weddingDTO.SingleResult.TemplatePreviewUrl + "/index.html");
        //    }
        //    else
        //    {
        //       return RedirectToAction("NotFound", "Error");
        //    }
            
        //}

        //public JsonResult GetWeddingData(int Id)
        //{
        //    WeddingWebService.WeddingDTOResponse weddingDTO = new WeddingWebService.WeddingDTOResponse();
        //    weddingDTO = wedding.GetWeddingDetailByID(Convert.ToInt32(Id));
        //    if (weddingDTO.IsSuccess)
        //    {
        //        return Json(weddingDTO.SingleResult, JsonRequestBehavior.AllowGet); 
        //    }
        //    return Json(weddingDTO.SingleResult, JsonRequestBehavior.AllowGet); 
        //}
    }
}