using AccuIT.BusinessLayer.Services.BO;
using AccuIT.CommonLayer.Aspects.Exceptions;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.PresentationLayer.WebAdmin.CustomFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccuIT.PresentationLayer.WebAdmin.ViewDataModel;
using Newtonsoft.Json;
using System.IO;
using AccuIT.CommonLayer.Aspects.Utilities.HttpMultipartParser;
using AccuIT.PresentationLayer.WebAdmin.Core;
using System.Web.Helpers;
using AccuIT.PresentationLayer.WebAdmin.Models;
using System.Text.RegularExpressions;


namespace AccuIT.PresentationLayer.WebAdmin.Controllers
{
    [AuthorizePage((int)AspectEnums.WebModules.Wedding, (int)AspectEnums.RoleType.Customer)]
    [SessionTimeOut]
    public class WeddingController : BaseController
    {
        static int WEDDINGID = 0;
        static int TEMPLATEID = 0;

        [HttpGet]
       // [Route("{wedding}/{Id:int}")]
        public ActionResult Index( int? Id)
        {
            WeddingViewModel WVM = new WeddingViewModel();
            //WeddingBO wedding = new WeddingBO();
            //UserWeddingTemplateSubscriptionsBO weddingProfile = HttpContext.Session[PageConstants.SESSION_WEDDING_PROFILE] as UserWeddingTemplateSubscriptionsBO;
            //WVM.listTemplates = weddingProfile.userTemplates.ToList();
            if (Id != null)
                WVM.WeddingBO = WeddingBusinessInstance.GetWeddingDetailByID(Convert.ToInt32(Id));
            ViewBag.WeddingForm = true;

            return View(WVM);
        }

        public WeddingViewModel InitializeWeddingForm(WeddingViewModel WVM)
        {
            WVM.WeddingBO = new WeddingBO();
            WVM.BrideAndMaidsBO = new BrideAndMaidBO();
            WVM.GroomAndMenBO = new GroomAndManBO();
            WVM.TimeLineBO = new TimeLineBO();
            WVM.WeddingEventsBO = new WeddingEventBO();

            if (WEDDINGID > 0)
            {
                HttpContext.Session[PageConstants.SESSION_WEDDING_ID] = WEDDINGID;
                WVM.WeddingBO = WeddingBusinessInstance.GetWeddingDetailByID(WEDDINGID);
                if (WVM.WeddingBO.BrideAndMaids != null && WVM.WeddingBO.BrideAndMaids.Count > 0)
                    WVM.BrideAndMaidsBO = WVM.WeddingBO.BrideAndMaids.Where(x => x.IsBride).FirstOrDefault();
                else
                {
                    WVM.BrideAndMaidsBO = new BrideAndMaidBO();
                    WVM.BrideAndMaidsBO.IsBride = true;
                }
                if (WVM.WeddingBO.GroomAndMen != null && WVM.WeddingBO.GroomAndMen.Count > 0)
                    WVM.GroomAndMenBO = WVM.WeddingBO.GroomAndMen.Where(x => x.IsGroom).FirstOrDefault();
                else
                {
                    WVM.GroomAndMenBO = new GroomAndManBO();
                    WVM.GroomAndMenBO.IsGroom = true;
                }
                WVM.TimeLineBO = new TimeLineBO();
            }

            return WVM;
        }

        [HttpGet]
        public ActionResult Create(int? tID, int? weddingID)
        {

            TEMPLATEID = (int)tID == 0 ? TEMPLATEID : (int)tID;
            WEDDINGID = weddingID == null ? WEDDINGID : (int)weddingID;
            WeddingViewModel WVM = new WeddingViewModel();

            UserWeddingTemplateSubscriptionsBO weddingProfile = HttpContext.Session[PageConstants.SESSION_WEDDING_PROFILE] as UserWeddingTemplateSubscriptionsBO;
            if (weddingID > 0)
            {
                InitializeWeddingForm(WVM);
            }
            else
            {
                WVM.WeddingBO = new WeddingBO();
                WVM.BrideAndMaidsBO = new BrideAndMaidBO();
                WVM.GroomAndMenBO = new GroomAndManBO();
                WVM.TimeLineBO = new TimeLineBO();
                WVM.WeddingEventsBO = new WeddingEventBO();
            }
            DropDown DD = new DropDown();
            ViewBag.RelationsList = DD.GetBrideGroomRelations();
            WVM.listTemplates = weddingProfile.Templates.ToList();
            WVM.userTemplate = WVM.listTemplates.Where(x => x.TemplateID == TEMPLATEID).FirstOrDefault();
            WVM.WeddingBO.TemplateID = TEMPLATEID;

            WVM.WeddingBO.TemplateImageUrl = WVM.userTemplate.ThumbnailImageUrl;
            WVM.WeddingBO.TemplatePreviewUrl = WVM.userTemplate.TemplatePreviewUrl;
            return View(WVM);
        }

        [HttpPost]
        public ActionResult Create(WeddingViewModel WVM)
        {
            WEDDINGID = WeddingBusinessInstance.SubmitUserWeddingDetail(UserID, WVM.WeddingBO);
            if (WEDDINGID > 0)
            {
                ViewBag.IsSuccess = true;
                ViewBag.Message = "Congratulations! Your wedding basic details has been created.";
            }
            else
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "OOPs something went wrong. Try again!";
            }
            ViewBag.ShowPopUp = true;
            WVM = InitializeWeddingForm(WVM);
            return View(WVM);
        }

        [HttpPost]
        public PartialViewResult _WizardWedding(WeddingViewModel wedding)
        {
            WeddingViewModel WVM = new WeddingViewModel();
            WVM.WeddingBO = wedding.WeddingBO;
            WVM.WeddingBO.WeddingID = WeddingBusinessInstance.SubmitUserWeddingDetail(UserID, wedding.WeddingBO);
            WEDDINGID = WVM.WeddingBO.WeddingID;
            // Get wedding BrideMaids
            WVM.WeddingBO.BrideAndMaids = WeddingBusinessInstance.GetWeddingBrideMaids(wedding.WeddingBO.WeddingID);
            WVM.BrideAndMaidsBO = WVM.WeddingBO.BrideAndMaids.Where(x => x.IsBride).FirstOrDefault();
            if (WVM.BrideAndMaidsBO == null)
            {
                WVM.BrideAndMaidsBO = new BrideAndMaidBO();
                WVM.BrideAndMaidsBO.WeddingID = WEDDINGID;
            }
            if (WEDDINGID > 0)
            {
                ViewBag.BrideMaidForm = true;
                DropDown DD = new DropDown();
                ViewBag.RelationsList = DD.GetBrideGroomRelations();
                return PartialView("_WizardWedding", WVM);
            }
            else
            {
                return PartialView("_WizardWedding", WVM);
            }

        }


        [HttpPost]
        public ActionResult _WizardBrideMaids(WeddingViewModel WVM)
        {
            //WeddingViewModel WVM = new WeddingViewModel();
            WVM.BrideAndMaidsBO.CreatedBy = UserID;
            WVM.BrideAndMaidsBO.BrideAndMaidID = WeddingBusinessInstance.SubmitBrideMaids(WEDDINGID, WVM.BrideAndMaidsBO);
            // Get wedding BrideMaids
            WVM.GroomAndMenBO = WeddingBusinessInstance.GetWeddingDetailByID(WEDDINGID).GroomAndMen.Where(x => x.IsGroom).FirstOrDefault();
            WVM.BrideAndMaidsBO = WVM.WeddingBO.BrideAndMaids.Where(x => x.IsBride).FirstOrDefault();
            if (WVM.GroomAndMenBO == null)
            {
                WVM.GroomAndMenBO.WeddingID = WEDDINGID;
                WVM.GroomAndMenBO = new GroomAndManBO();

            }
            if (WVM.GroomAndMenBO.GroomAndMenID > 0)
            {
                ViewBag.GroomMenForm = true;
                DropDown DD = new DropDown();
                ViewBag.RelationsList = DD.GetBrideGroomRelations();
                return View("Index", WVM);
            }
            else
            {
                return PartialView("_WizardBrideMaids", WVM);
            }

        }


        [HttpGet]
        public JsonResult GetWeddingEventByID(int? Id)
        {
            WeddingViewModel WVM = new WeddingViewModel();
            try
            {
                WVM.WeddingEventsBO = WeddingBusinessInstance.GetWeddingDetailByID(WEDDINGID).WeddingEvents.Where(x => x.WeddingEventID == Id).FirstOrDefault();

                WeddingEventBO myEvent = new WeddingEventBO();
                myEvent = WVM.WeddingEventsBO;
                myEvent.strEventDate = WVM.WeddingEventsBO.EventDate.ToShortDateString();
                myEvent.strStartTime = WVM.WeddingEventsBO.StartTime.ToShortTimeString();
                myEvent.strEndTime = WVM.WeddingEventsBO.EndTime.ToShortTimeString();
                ///return PartialView("_SubmitEvent", WVM);
                return Json(myEvent, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            // return View();
        }

        [Route("{wedding}/submitevent/{Id:int}")]
        public ActionResult SubmitEvent(int? Id)
        {
            WeddingViewModel WVM = new WeddingViewModel();
            UserWeddingTemplateSubscriptionsBO weddingProfile = new UserWeddingTemplateSubscriptionsBO();
            WVM.listTemplates = weddingProfile.Templates.ToList();
            WVM.userTemplate = WVM.listTemplates.Where(x => x.TemplateID == TEMPLATEID).FirstOrDefault();

            try
            {
                WVM.WeddingBO = WeddingBusinessInstance.GetWeddingDetailByID(WEDDINGID);
                if (WVM.WeddingBO.WeddingEvents.Count > 0 && Id > 0)
                {
                    WVM.WeddingEventsBO = WVM.WeddingBO.WeddingEvents.Where(x => x.WeddingEventID == Id).FirstOrDefault();
                    WeddingEventBO myEvent = new WeddingEventBO();
                    WVM.WeddingEventsBO.Venue = new VenueBO();
                    WVM.WeddingEventsBO.Venue.WeddingEventID = WVM.WeddingEventsBO.WeddingEventID;
                    myEvent = WVM.WeddingEventsBO;

                    // myEvent.strStartTime = WVM.WeddingEventsBO.StartTime.ToShortTimeString();
                    // myEvent.strEndTime = WVM.WeddingEventsBO.EndTime.ToShortTimeString();
                    if (myEvent.Venues.Count() > 0)
                    {
                        WVM.WeddingEventsBO.Venue = myEvent.Venues.FirstOrDefault();
                        WVM.WeddingEventsBO.Venue.VenueAddress = SystemBusinessInstance.GetAddressDetails(0, myEvent.Venue.VenueID, (int)AspectEnums.AddressOwnerType.Venue);
                    }
                    return View(WVM);
                }
                else
                {
                    WVM.WeddingEventsBO = new WeddingEventBO();
                    WVM.WeddingEventsBO.Venue = new VenueBO();
                    WVM.WeddingEventsBO.Venue.VenueAddress = new AddressMasterBO();
                    return View(WVM);
                }

            }
            catch (Exception ex)
            {
                throw ex;

            }
            // return View();
        }

        [HttpPost]
        public ActionResult SubmitEvent(WeddingViewModel model, FormCollection collection)
        {
            DateTime eventdate = model.WeddingEventsBO.EventDate;
            //model.WeddingEventsBO.EventDate = Convert.ToDateTime(model.WeddingEventsBO.strEventDate);
            model.WeddingEventsBO.WeddingID = WEDDINGID;
            model.WeddingEventsBO.StartTime = Convert.ToDateTime(eventdate.ToShortDateString() + " " + model.WeddingEventsBO.strStartTime);
            model.WeddingEventsBO.EndTime = Convert.ToDateTime(eventdate.ToShortDateString() + " " + model.WeddingEventsBO.strEndTime);
            model.WeddingEventsBO.WeddingEventID = WeddingBusinessInstance.SubmitWeddingEvent(UserID, model.WeddingEventsBO);
            if (model.WeddingEventsBO.WeddingEventID > 0)
            {
                ViewBag.IsSuccess = true;
                ViewBag.Message = "Your event has been created.";
            }
            else
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "OOPs something went wrong. Try again!";
            }
            return View(model);
            //return RedirectToAction("SubmitEvent", new { Id = model.WeddingEventsBO.WeddingEventID });
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult UploadImageFile(string imgtype, int myId, string sequence, int? eventID, string imageof)
        {

            string fileName = string.Empty;
            string imageurl = "";
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                var pic = System.Web.HttpContext.Current.Request.Files["MyImages"];
                if (pic.ContentLength > 0)
                {

                    //if (myId == 0)
                    //    fileName = imageof + "_" + DateTime.Now.ToString("ddMMyyyHmss") + "_.jpg".ToLower();
                    //else
                    fileName = imageof + "_" + myId + "_" + pic.FileName.Split('.')[0] + ".jpg".ToLower();
                    // var _ext =  Path.GetExtension(pic.FileName);
                    if (imgtype == "User")
                        imageurl = "~/UserImages/User-" + myId;
                    if (imgtype == "Background")
                        imageurl = "~/WeddingImages/Wedding-" + WEDDINGID;
                    if (imgtype == "Event")
                        imageurl = "~/WeddingImages/Wedding-" + WEDDINGID + "/Event-" + myId;
                    else if (imgtype == "Venue")
                        imageurl = "~/WeddingImages/Wedding-" + WEDDINGID + "/Event-" + eventID + "/Venue-" + myId;
                    else if (imgtype == "Bride")
                        imageurl = "~/WeddingImages/Wedding-" + WEDDINGID + "/Bride-Maids";
                    else if (imgtype == "Groom")
                        imageurl = "~/WeddingImages/Wedding-" + WEDDINGID + "/Groom-Men";
                    else if (imgtype == "TimeLine")
                        imageurl = "~/WeddingImages/Wedding-" + WEDDINGID + "/TimeLine";
                    else if (imgtype == "Gallery")
                        imageurl = "~/WeddingImages/Wedding-" + WEDDINGID + "/Gallery";

                    var strpath = Path.Combine(Server.MapPath(imageurl));
                    string pathString = Path.Combine(strpath.ToString());
                    bool isExists = System.IO.Directory.Exists(pathString);
                    if (!isExists)
                        System.IO.Directory.CreateDirectory(pathString);

                    var _imgPath = pathString + "/" + fileName;
                    ViewBag.Msg = _imgPath;
                    var path = _imgPath;

                    // Saving Image in Original Mode
                    pic.SaveAs(path);

                    // resizing image ---------

                    //MemoryStream ms = new MemoryStream();
                    //WebImage img = new WebImage(_imgPath);

                    //if (img.Width > 800)
                    //    img.Resize(800, 800);
                    //img.Save(_imgPath);

                    // end resize -----------

                }
            }
            string ImgSourceUrl = imageurl.Replace("~", "") + "/" + fileName;
            return Json(ImgSourceUrl, JsonRequestBehavior.AllowGet);
        }


        public ActionResult BrideMaids(int weddingID)
        {
            BrideAndMaidBO bridemaid = new BrideAndMaidBO();
            bridemaid.WeddingID = weddingID;
            return View(bridemaid);
        }

        [HttpGet]
        public JsonResult GetBrideDetailsByID(int Id)
        {
            WeddingViewModel WVM = new WeddingViewModel();
            try
            {
                DropDown DD = new DropDown();
                ViewBag.RelationsList = DD.GetBrideGroomRelations();
                WVM.BrideAndMaidsBO = WeddingBusinessInstance.GetBrideDetailsByID(Id);
                return Json(WVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            // return View();
        }

        [HttpGet]
        public JsonResult GetGroomDetailsByID(int Id)
        {
            WeddingViewModel WVM = new WeddingViewModel();
            try
            {
                DropDown DD = new DropDown();
                ViewBag.RelationsList = DD.GetBrideGroomRelations();
                WVM.GroomAndMenBO = WeddingBusinessInstance.GetGroomDetailsByID(Id);
                return Json(WVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;

            }
            // return View();
        }

        [HttpPost]
        public ActionResult SubmitBrideMaids(WeddingViewModel model)
        {
            DropDown DD = new DropDown();
            ViewBag.RelationsList = DD.GetBrideGroomRelations();

            if (model.BrideAndMaidsBO.BrideAndMaidID == 0)
            {
                model.BrideAndMaidsBO.IsActive = true;
                model.BrideAndMaidsBO.IsDeleted = false;
                model.BrideAndMaidsBO.CreatedBy = UserID;
                model.BrideAndMaidsBO.CreatedDate = DateTime.Now;
            }

            model.BrideAndMaidsBO.WeddingID = WEDDINGID;
            model.BrideAndMaidsBO.BrideAndMaidID = WeddingBusinessInstance.SubmitBrideMaids(model.BrideAndMaidsBO.WeddingID, model.BrideAndMaidsBO);

            if (model.BrideAndMaidsBO.BrideAndMaidID > 0)
            {
                //RenameNewImageFile(model.BrideAndMaidsBO.Imageurl, model.BrideAndMaidsBO.BrideAndMaidID);
                ViewBag.IsSuccess = true;
                ViewBag.ShowPopup = true;
                ViewBag.Message = "Bride Maid's detail submitted successfully.";
            }
            else
            {
                ViewBag.ShowPopup = true;
                ViewBag.IsSuccess = false;
                ViewBag.Message = "OOPs something went wrong. Try again!";
            }
            ViewBag.ShowPopUp = true;
            return RedirectToAction("Create", "Wedding", new { tid = TEMPLATEID, weddingID = WEDDINGID });

        }

        public void RenameNewImageFile(string oldImageUrl, int PK_ID)
        {
            string oldfileName = oldImageUrl.Split('_')[1];
            string newImageUrl = oldImageUrl.Replace(oldfileName, PK_ID.ToString().Replace("_.", "."));
            System.IO.File.Move(oldImageUrl, newImageUrl);
        }

        public ActionResult GroomsMen(int weddingID)
        {
            GroomAndManBO groomMen = new GroomAndManBO();
            groomMen.WeddingID = weddingID;
            return View(groomMen);
        }

        [HttpPost]
        public ActionResult SubmitGroomsMen(WeddingViewModel model, FormCollection collection)
        {
            model.GroomAndMenBO.WeddingID = WEDDINGID;
            if (model.GroomAndMenBO.GroomAndMenID == 0)
            {
                model.GroomAndMenBO.IsActive = true;
                model.GroomAndMenBO.IsDeleted = false;
                model.GroomAndMenBO.CreatedBy = UserID;
                model.GroomAndMenBO.CreatedDate = DateTime.Now;
            }

            model.GroomAndMenBO.GroomAndMenID = WeddingBusinessInstance.SubmitGroomMen(model.GroomAndMenBO.WeddingID, model.GroomAndMenBO);
            if (model.GroomAndMenBO.GroomAndMenID > 0)
            {
                ViewBag.IsSuccess = true;
                ViewBag.ShowPopup = true;
                ViewBag.Message = "Groom's Man submitted successfully.";
            }
            else
            {
                ViewBag.IsSuccess = false;
                ViewBag.ShowPopup = true;
                ViewBag.Message = "OOPs something went wrong. Try again!";
            }
            DropDown DD = new DropDown();
            ViewBag.RelationsList = DD.GetBrideGroomRelations();
            ViewBag.ShowPopUp = true;
            return RedirectToAction("Create", "Wedding", new { tid = TEMPLATEID, weddingID = WEDDINGID });

        }


        [HttpGet]
        public JsonResult GetTimeLineDetailsByID(int Id)
        {
            WeddingViewModel WVM = new WeddingViewModel();
            try
            {
                WVM.TimeLineBO = WeddingBusinessInstance.GetTimeLineDetailsByID(Id);
                return Json(WVM, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw ex;

            }
        }

        [HttpPost]
        public ActionResult SubmitTimeLine(WeddingViewModel model)
        {
            model.TimeLineBO.WeddingID = WEDDINGID;
            model.TimeLineBO.TimeLineID = WeddingBusinessInstance.SubmitTimeLine(WEDDINGID, model.TimeLineBO);

            if (model.TimeLineBO.TimeLineID > 0)
            {
                ViewBag.IsSuccess = true;
                ViewBag.Message = "TimeLine submitted successfully.";
            }
            else
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "OOPs something went wrong. Try again!";
            }
            ViewBag.ShowPopUp = true;
            //return Json(model, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Create", "Wedding", new { tid = TEMPLATEID, weddingID = WEDDINGID });
            //return PartialView("_SubmitTimeLine", model);
        }
    }
}
