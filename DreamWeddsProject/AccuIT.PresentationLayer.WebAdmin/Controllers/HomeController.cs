using AccuIT.BusinessLayer.Services.BO;
using AccuIT.CommonLayer.Aspects.Exceptions;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.PresentationLayer.WebAdmin.Core;
using AccuIT.PresentationLayer.WebAdmin.CustomFilter;
using AccuIT.PresentationLayer.WebAdmin.ViewDataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccuIT.PresentationLayer.WebAdmin.Controllers
{

   // [AuthorizePage((int)AspectEnums.WebModules.CustomerDashBoard, (int)AspectEnums.RoleType.Customer)]
  //  [SessionTimeOut]
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            UserProfileBO profile = new UserProfileBO();
            //UserWeddingSubscriptionBO weddingProfile = new UserWeddingSubscriptionBO();
            WeddingViewModel WVM = new WeddingViewModel();
           var  weddingProfile = Session[PageConstants.SESSION_WEDDING_PROFILE] as List<UserWeddingSubscriptionBO>;
             WVM.listTemplates = weddingProfile.Select(x=>x.TemplateMaster).ToList();
            WVM.userSubscriptions = weddingProfile;
            foreach (var temp in WVM.listTemplates)
            {
                temp.Weddings = WeddingBusinessInstance.GetUserWeddingDetail(UserID).Where(x => x.TemplateID == temp.TemplateID && x.IsDeleted == false).ToList();
            }
            if (Session[PageConstants.SESSION_PROFILE_KEY] == null)
            {
                WVM.userProfile = EmpBusinessInstance.DisplayUserProfile(UserID);

            }
            else
            {
                WVM.userProfile = Session[PageConstants.SESSION_PROFILE_KEY] as UserProfileBO;
            }

            return View(WVM);
        }

        [HttpPost]
        public ActionResult Index(WeddingViewModel model)
        {
            bool isSuccess = false;
            ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    isSuccess = EmpBusinessInstance.UpdateUserProfile(model.userProfile);
                    //Update User session so that values can be updated.
                    Session[PageConstants.SESSION_PROFILE_KEY] = EmpBusinessInstance.DisplayUserProfile(model.userProfile.UserID);

                    if (isSuccess)
                    {

                        ViewBag.Message = "User successfully updated.";
                    }
                    else
                    {
                        ViewBag.ErrMessage = "OOPS something went wrong. Please try again later!";
                    }
                    ViewBag.isSuccess = isSuccess;
                    ViewBag.ShowPopup = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            return View(model);
        }

        public ActionResult Upload()
        {
            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[fileName];
                    fName = file.FileName;
                    if (file != null && file.ContentLength > 0)
                    {
                        var path = Path.Combine(Server.MapPath("~/Content/Images/Users"));
                        string pathString = System.IO.Path.Combine(path.ToString());
                        var fileName1 = UserID;// Path.GetFileName(file.FileName);
                        bool isExists = System.IO.Directory.Exists(pathString);
                        if (!isExists) System.IO.Directory.CreateDirectory(pathString);
                        var uploadpath = string.Format("{0}\\{1}", pathString, file.FileName);
                        file.SaveAs(uploadpath);
                    }
                }
            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }
            if (isSavedSuccessfully)
            {
                return Json(new
                {
                    Message = fName
                });
            }
            else
            {
                return Json(new
                {
                    Message = "Error in saving file"
                });
            }
        }
    }
}
