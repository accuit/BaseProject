using PresentationLayer.DreamWedds.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using AccuIT.CommonLayer.Aspects.Logging;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.BusinessLayer.Services.BO;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.BusinessLayer.Services.Contracts;

namespace PresentationLayer.DreamWedds.Web.Controllers
{

    public class HomeController : Controller
    {

        private ISystemService systemBusinessInstance;

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

        DreamWeddsData D = new DreamWeddsData();
        DreamweddsModel M = new DreamweddsModel();
        [OutputCache(Duration = 9999 * 999, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        public ActionResult Index()
        {

            if (DreamWeddsData.DreamWeddsWeb == null)
            {
                M.templateMasters = D.GetWebsiteData().templateMasters;
            }
            else
            {
                M.templateMasters = DreamWeddsData.DreamWeddsWeb.templateMasters;
            }

            return View(M);
        }

        //[OutputCache(Duration = 9999 * 999, VaryByParam = "none", Location = OutputCacheLocation.Client, NoStore = true)]
        [Route("know-about-dreamWedds")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        //[Route("contact-dreamWedds-team")]
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        //[Route("contact-dreamWedds-team")]
        [HttpPost]
        public ActionResult Contact(EmailServiceDTO email)
        {
            try
            {


                int templateCode = (int)AspectEnums.EmailTemplateCode.ContactUsReply; // DreamWedds Contact Us Reply
                TemplateMasterBO template = new TemplateMasterBO();
                template = SystemBusinessInstance.GetTemplateData(0, templateCode);

                email.TemplateID = template.TemplateID;
                email.Body = template.TemplateContent;
                email.CreatedDate = DateTime.Now;
                email.IsHtml = true;
                email.Priority = 2;
                email.IsAttachment = false;
                try
                {

                    EmailNotificationService eNotification = new EmailNotificationService();
                    eNotification.SendEmailNotification(email, template);
                    ViewBag.IsSuccess = true;
                    return View();
                }
                catch (DbEntityValidationException ex)
                {
                    ViewBag.IsFail = true;
                    var newException = new FormattedDbEntityValidationException(ex);
                }
                catch (Exception ex)
                {
                    ViewBag.IsFail = true;

                }
                return View(email);
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }
        }

        [Route("frequently-asked-questions")]
        public ActionResult FAQ()
        {
            try
            {
                
                if (DreamWeddsData.DreamWeddsWeb.faqs == null)
                {
                    M.faqs = D.GetWebsiteData().faqs;
                }
                else
                {
                    M.faqs = DreamWeddsData.DreamWeddsWeb.faqs;
                }
            }
            catch
            {
                return RedirectToAction("Index", "Error");
            }

            return View(M);
        }
    }
}