using AccuIT.BusinessLayer.Services.BO;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.CommonLayer.Aspects.Security;
using AccuIT.CommonLayer.Aspects.Utilities;
using PresentationLayer.DreamWedds.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;


namespace PresentationLayer.DreamWedds.Web.Controllers
{

    public class TemplateController : Controller
    {

        #region Private Method initialization


        private IUserService userBusinessInstance;
        private ISystemService systemBusinessInstance;
        private IWeddingService weddingBusinessInstance;
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
        public IWeddingService WeddingBusinessInstance
        {
            get
            {
                if (weddingBusinessInstance == null)
                {
                    weddingBusinessInstance = AopEngine.Resolve<IWeddingService>(AspectEnums.AspectInstanceNames.WeddingManager, AspectEnums.ApplicationName.AccuIT);
                }
                return weddingBusinessInstance;
            }
        }

        #endregion

        //DreamWeddsDBEntities DBEntities = new DreamWeddsDBEntities();
        DreamWeddsData D = new DreamWeddsData();
        DreamweddsModel M = new DreamweddsModel();

        [OutputCache(Duration = 9999 * 999, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        [Route("View-All-Wedding-Themes")]
        public ActionResult Index()
        {
            try
            {
                if (DreamWeddsData.DreamWeddsWeb == null)
                {
                    M.templateMasters = D.GetWebsiteData().templateMasters;
                }
                else
                {
                    M.templateMasters = DreamWeddsData.DreamWeddsWeb.templateMasters.Where(x => x.TemplateType == (int)AspectEnums.TemplateType.Wedding).ToList();
                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }
            return View(M);
        }

        //[Route("template/Specific-type-Templates")]
        //public ActionResult Index(int templateTypeID)
        //{
        //    var weddingTemplates = GetTemplates().Where(x => x.TemplateType == templateTypeID);
        //    return View(weddingTemplates);
        //}

        [Route("{template}/{templateID:int}")]
        public ActionResult Details(string templateName, int templateId = 5)
        {
            try
            {

                if (DreamWeddsData.DreamWeddsWeb == null)
                {
                    M.templateMasters = D.GetWebsiteData().templateMasters;
                }
                else
                {
                    M.templateMasters = DreamWeddsData.DreamWeddsWeb.templateMasters;
                }
                M.templateMaster = M.templateMasters.Where(x => x.TemplateID == templateId).FirstOrDefault();

                if (M.templateMaster.templateImages == null || M.templateMaster.templateImages.Count == 0)
                {
                    M.templateMaster.templateImages = new List<TemplateImageBO>();
                    //Fetch images from the Screenshot folder AND  iNSERT INTO DB
                    ViewBag.ShareImage = "~/Templates/Wedding/" + M.templateMaster.TemplateFolderPath + "/images/ScreenShots/1.jpg";
                    string screenshotsPath = "~/Templates/Wedding/" + M.templateMaster.TemplateFolderPath + "/images/ScreenShots";
                    M.templateMaster.templateImages = GetTemplateScreenShots(screenshotsPath, M.templateMaster);

                }
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }

            return View(M);
        }

        private List<TemplateImageBO> GetTemplateScreenShots(string path, TemplateMasterBO model)
        {

            List<TemplateImageBO> imagesList = new List<TemplateImageBO>();
            try
            {
                DirectoryInfo directory = new DirectoryInfo(Server.MapPath(path));
                var files = directory.GetFiles().ToList();

                if (files.Count > 0)
                {
                    foreach (var item in files.Where(x => x.Name != "Thumbs.db"))
                    {
                        TemplateImageBO image = new TemplateImageBO();

                        image.TemplateID = model.TemplateID;
                        image.ImageName = item.Name;
                        image.ImageUrl = path.Replace("~", "..") + "/" + item.Name;
                        image.IsBannerImage = false;
                        image.ImageTagLine = model.TemplateName + "Wedding Theme ScreenShot - " + item.Name;
                        image.ImageFolderPath = path;
                        image.ImageTitle = "ScreenShot - " + item.Name;
                        image.CreatedDate = item.CreationTime;
                        image.CreatedBy = 1;
                        imagesList.Add(image);
                    }

                    imagesList = SystemBusinessInstance.SubmitTemplateImages(imagesList);
                }
                else
                {
                    TemplateImageBO image = new TemplateImageBO();
                    image.ImageName = "Screenshot is Not Available";
                }
            }
            catch (Exception ex)
            {

            }

            return imagesList;


        }


        public ActionResult Purchase(int templateid, int version)
        {
            UserPurchaseViewModel model = new UserPurchaseViewModel();
            model.user = new UserMasterBO();
            model.order = new OrderMasterBO();
            try
            {

                model.subscriptions = new UserWeddingSubscriptionBO();
                bool istrial = false;
                if (version == (int)AspectEnums.SubscriptionType.Trial)
                {
                    istrial = true;
                }

                if (DreamWeddsData.DreamWeddsWeb == null)
                    model.template = SystemBusinessInstance.GetTemplateData(templateid, null);
                else
                    model.template = DreamWeddsData.DreamWeddsWeb.templateMasters.Where(x => x.TemplateID == templateid).FirstOrDefault();

                if (istrial)
                {
                    ViewBag.IsTrial = true;
                    model.subscriptions.SubscriptionTypeList = AspectEnums.SubscriptionType.Trial;
                    model.order.OrderStatus = AspectEnums.OrderStatus.Submitted.ToString();
                    model.order.PaymentMode = (int)AspectEnums.PaymentMode.FREE;
                    model.order.PaymentTerms = "Trial";
                    model.subscriptions.SubscriptionType = (int)AspectEnums.SubscriptionType.Trial;
                    model.subscriptions.SubscriptionStatus = (int)AspectEnums.SubscriptionStatus.Active;
                    model.subscriptions.TemplateID = templateid;
                }
                else
                {
                    ViewBag.IsTrial = false;

                    // model.subscriptions.SubscriptionTypeList = AspectEnums.SubscriptionType;
                }

                ViewBag.ShowMessage = false;
            }
            catch (Exception e)
            {
                return RedirectToAction("Index", "Error");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Purchase(UserPurchaseViewModel model)
        {
            try
            {
                ViewBag.ShowMessage = true;
                ViewBag.IsTrial = false;


                if (model.subscriptions.SubscriptionType == (int)AspectEnums.SubscriptionType.Trial)
                {
                    ViewBag.IsTrial = true;
                    model.template.IsTrial = true;
                }

                #region Create NEW USER - SUBMIT USERMASTER

                bool isUserExist = UserBusinessInstance.GetUserByLoginName(model.user.Email).UserID > 0 ? true : false;

                if (isUserExist)
                {
                    ViewBag.Message = "This email address already exist.";
                    ViewBag.IsSuccess = false;
                    return View(model);
                }

                model.user.CreatedBy = 1;
                model.user.CreatedDate = DateTime.Now;
                model.user.AccountStatus = (int)AspectEnums.UserAccountStatus.Pending;
                model.user.isActive = true;
                model.user.isDeleted = false;
                model.user.IsEmployee = false;
                model.user.LoginName = model.user.Email;
                model.user.Password = "Dreamwedds@17";
                string sessionID = HttpContext.Session.SessionID.ToString();
                int newUserID = UserBusinessInstance.SubmitNewEmployee(model.user, sessionID);
                #endregion

                #region CREATE NEW ORDER - SUBMIT ORDERMASTER
                model.order.UserID = newUserID;
                decimal cost = 0;
                int Discount = Convert.ToInt32(ConfigurationManager.AppSettings["Discount"]);
                if (model.subscriptions.SubscriptionType == (int)AspectEnums.SubscriptionType.Trial)
                {
                    cost = 0;
                    model.subscriptions.EndDate = DateTime.Now.AddDays(10);
                }
                if (model.subscriptions.SubscriptionTypeList == AspectEnums.SubscriptionType.Annual)
                {
                    cost = Convert.ToDecimal(model.template.COST);
                    model.subscriptions.EndDate = DateTime.Now.AddMonths(12);
                }
                else if (model.subscriptions.SubscriptionTypeList == AspectEnums.SubscriptionType.HalfYearly)
                {
                    cost = Convert.ToDecimal(model.template.COST * .60);
                    model.subscriptions.EndDate = DateTime.Now.AddMonths(06);
                }
                else if (model.subscriptions.SubscriptionTypeList == AspectEnums.SubscriptionType.Quarterly)
                {
                    cost = Convert.ToDecimal(model.template.COST * 0.30);
                    model.subscriptions.EndDate = DateTime.Now.AddMonths(3);
                }

                model.order.Discount = Discount;
                model.order.Amount = cost - (cost * (Discount / 100));
                model.template.COST = Convert.ToInt32(model.order.Amount);
                int OrderID = SystemBusinessInstance.SubmitNewOrder(model.order);
                #endregion

                #region CREATE NEW SUBSCRIPTION - SUBMIT USERWEDDINGSUBSCRIPTION
                model.subscriptions.UserId = newUserID;
                model.subscriptions.InvoiceNo = OrderID;


                int SubscriptionID = SystemBusinessInstance.SubmitUserSubscription(model.subscriptions);
                #endregion

                if (newUserID > 1)
                {
                    EmailServiceDTO email = new EmailServiceDTO();
                    TemplateMasterBO emailTemplate = new TemplateMasterBO();
                    int emailTemplateCode = (int)AspectEnums.EmailTemplateCode.WelcomeEmail;

                    if (DreamWeddsData.DreamWeddsWeb == null)
                        emailTemplate = SystemBusinessInstance.GetTemplateData(0, emailTemplateCode);
                    else
                        emailTemplate = DreamWeddsData.DreamWeddsWeb.templateMasters.Where(x => x.TemplateCode == emailTemplateCode).FirstOrDefault();

                    model.template.UrlIdentifier = EncryptionEngine.Encrypt(newUserID.ToString() + "," + model.user.FirstName + "," + model.user.LastName + "," + model.user.LoginName + "," + model.template.TemplateName);
                    //string encodedValue = HttpUtility.UrlEncode(model.template.UrlIdentifier);
                    string decrypt = EncryptionEngine.Decrypt(model.template.UrlIdentifier);
                    email.ToName = model.user.FirstName + " " + model.user.LastName;
                    email.Subject = emailTemplate.TemplateSubject;
                    email.ToEmail = model.user.Email;
                    email.Status = (int)AspectEnums.EmailStatus.Pending;
                    email.Message = emailTemplate.TemplateName;
                    email.Phone = model.user.Phone;
                    email.Mobile = model.user.Mobile;
                    email.IsCustomerCopy = false;
                    email.TemplateID = emailTemplate.TemplateID;
                    email.Body = emailTemplate.TemplateContent;
                    email.CreatedDate = DateTime.Now;
                    email.CreatedBy = newUserID;
                    email.IsHtml = true;
                    email.Priority = 2;
                    email.IsAttachment = false;
                    email.Body = PrepareEmailContent(email, emailTemplate);
                    EmailNotificationService eNotification = new EmailNotificationService();
                    eNotification.SendEmailNotification(email, model.template);
                    ViewBag.IsSuccess = true;

                }
            }
            catch (DbEntityValidationException ex)
            {
                ViewBag.IsSuccess = false;
                var newException = new FormattedDbEntityValidationException(ex);
                ViewBag.Message = "Error: " + ex;
            }
            catch (Exception e)
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "Error: " + e;
            }
            return View(model);
        }

        private string PrepareEmailContent(EmailServiceDTO model, TemplateMasterBO Template)
        {
            bool isDebugMode = ConfigurationManager.AppSettings["IsDebugMode"] == "Y" ? true : false;
            var MergeFields = SystemBusinessInstance.GetTemplateMergeFields(Template.TemplateID);
            string emailContent = model.Body;
            string path = ConfigurationManager.AppSettings["WeddingTemplatePath"].ToString();
            string welcomeRegisterUrl = string.Empty;
            if (isDebugMode)
                welcomeRegisterUrl = ConfigurationManager.AppSettings["DebugWelcomeLoginURL"].ToString();
            else
                welcomeRegisterUrl = ConfigurationManager.AppSettings["WelcomeLoginURL"].ToString();
            foreach (var field in MergeFields)
            {

                if (field.SRC_FIELD == "{{IDENTIFIER}}")
                    emailContent = FindReplace(emailContent, "{{IDENTIFIER}}", welcomeRegisterUrl + Template.UrlIdentifier);

                else if (field.SRC_FIELD == "{{TONAME}}")
                    emailContent = FindReplace(emailContent, field.SRC_FIELD, model.ToName);

                else if (field.SRC_FIELD == "{{PURCHASE_DATE}}")
                    emailContent = FindReplace(emailContent, field.SRC_FIELD, DateTime.Now.ToShortDateString());

                else if (field.SRC_FIELD == "{{TEMPLATENAME}}")
                    emailContent = FindReplace(emailContent, field.SRC_FIELD, Template.TemplateName);

                else if (field.SRC_FIELD == "{{TEMPLATEPREVIEWIMAGE}}")
                    emailContent = FindReplace(emailContent, field.SRC_FIELD, path + Template.TemplateName + "/images/ScreenShots/1.png");

                else if (field.SRC_FIELD == "{{DEMO_URL}}")
                    emailContent = FindReplace(emailContent, field.SRC_FIELD, path + Template.TemplateName + "/index.html");

                else if (field.SRC_FIELD == "{{PRICE}}")
                {
                    if (Template.IsTrial)
                        emailContent = FindReplace(emailContent, field.SRC_FIELD, "TRIAL");
                    else
                        emailContent = FindReplace(emailContent, field.SRC_FIELD, "INR " + Template.COST.ToString());
                }

                else if (field.SRC_FIELD == "{{ABOUT_TEMPLATE}}")
                    emailContent = FindReplace(emailContent, field.SRC_FIELD, Template.AboutTemplate);

            }
            model.Body = emailContent;
            return emailContent;
        }

        public string FindReplace(string input, string replaceTag, string replaceVal)
        {
            StringBuilder b = new StringBuilder(input);
            string FinalVal = String.Empty;
            FinalVal = b.Replace(replaceTag, replaceVal).ToString();
            return FinalVal;
        }
    }
}