using AccuIT.BusinessLayer.Services.BO;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.CommonLayer.Log;
using PresentationLayer.DreamWedds.WebAdmin.CustomFilter;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace PresentationLayer.DreamWedds.Web.Models
{
    public class EmailNotificationService
    {
        // DreamWeddsDBEntities DBEntities = new DreamWeddsDBEntities();
        XElement xElement = null;
        string SMTPServer = string.Empty;
        int SMTPPort = 0;
        bool isDebugMode = ConfigurationManager.AppSettings["IsDebugMode"] == "Y" ? true : false;
        #region Private Method initialization


        private IUserService userBusinessInstance;
        private ISystemService systemBusinessInstance;
        private IWeddingService weddingBusinessInstance;
        private IEmailService emailBusinessInstance;
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
        public IEmailService EmailBusinessInstance
        {
            get
            {
                if (emailBusinessInstance == null)
                {
                    emailBusinessInstance = AopEngine.Resolve<IEmailService>(AspectEnums.AspectInstanceNames.EmailManager, AspectEnums.ApplicationName.AccuIT);
                }
                return emailBusinessInstance;
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
        public EmailNotificationService()
        {
            SMTPServer = ConfigurationManager.AppSettings["SMTPHost"];
            SMTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
        }

        public EmailNotificationService(string EmailXMLPath, string smtpServerName, int smtpServerPort)
        {
            try
            {
                xElement = XElement.Load(EmailXMLPath);
                SMTPServer = smtpServerName;
                SMTPPort = smtpServerPort;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SendEmailNotification(EmailServiceDTO emailModel, TemplateMasterBO emailTemplate)
        {
            string CCAddress = string.Empty;
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CCAddress"]))
                CCAddress = ConfigurationManager.AppSettings["CCAddress"];
            SendEmail(emailModel, CCAddress);
        }

        private string PrepareEmailContent(EmailServiceDTO model, TemplateMasterBO Template)
        {
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
        private static string GetHtmlTextByName(string templateName, string filePath, string elementVal)
        {
            var xmlDoc = XElement.Load(filePath);

            XElement templateSelected = (from xTmlt in xmlDoc.DescendantsAndSelf("EmailTemplates").Descendants("EmailTemplate")
                                         where xTmlt.Element("Name").Value == templateName
                                         select xTmlt).FirstOrDefault();
            // string htmlContent = templateSelected.Element("Content").Value;
            string htmlContent = templateSelected.Element(elementVal).Value;
            return htmlContent;
        }
        public void SendEmail(EmailServiceDTO emailModel, string ccAddress)
        {
            string fromPass = "";

            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();

                if (isDebugMode)
                {
                    emailModel.Subject = "[DEBUG MODE] Email To : " + emailModel.FromName + ". | Subject: " + emailModel.Subject;
                    emailModel.ToEmail = ConfigurationManager.AppSettings["DbugToEmail"].ToString();
                    emailModel.FromEmail = ConfigurationManager.AppSettings["DbugFromEmail"].ToString();
                    emailModel.FromName = ConfigurationManager.AppSettings["DebugFromName"].ToString();
                    fromPass = ConfigurationManager.AppSettings["DbugFromPass"];
                    smtpClient.Host = ConfigurationManager.AppSettings["DbugSMTPHost"]; //"relay-hosting.secureserver.net";   //-- Donot change.
                    smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["DbugSMTPPort"]); // 587; //--- Donot change               
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new System.Net.NetworkCredential(emailModel.FromEmail, fromPass);
                    message.To.Add(emailModel.ToEmail);
                }
                else
                {
                    if (ccAddress != "")
                        message.CC.Add(ccAddress);
                    emailModel.FromEmail = ConfigurationManager.AppSettings["FromEmail"].ToString();
                    emailModel.FromName = ConfigurationManager.AppSettings["FromName"].ToString();
                    smtpClient.Host = ConfigurationManager.AppSettings["SMTPHost"];
                    message.To.Add(emailModel.FromEmail);
                }

                message.BodyEncoding = Encoding.UTF8;
                message.From = new System.Net.Mail.MailAddress(emailModel.FromEmail, "The DreamWedds");
                message.IsBodyHtml = true;
                message.Body = emailModel.Body;
                message.Subject = emailModel.Subject;

                bool isInsert = EmailBusinessInstance.InsertEmailRecord(emailModel);// EmailBu.EmailServices.Add(emailModel);

                if (isInsert)
                    smtpClient.Send(message);

                ActivityLog.SetLog("Email sent to " + emailModel.FromEmail, LogLoc.DEBUG);
                message.Dispose();

            }
            catch (DbEntityValidationException ex)
            {
                var newException = new FormattedDbEntityValidationException(ex);
                ActivityLog.SetLog("Email failed.", LogLoc.DEBUG);
                throw newException;
            }
            catch (Exception ex)
            {
                ActivityLog.SetLog("Email failed.", LogLoc.DEBUG);
                throw ex;
            }
        }

    }
}