using System.Web;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Configuration;
using AccuIT.CommonLayer.Aspects.Exceptions;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.CommonLayer.Aspects.Extensions;
using System.Linq;
using System;

namespace AccuIT.CommonLayer.Aspects.EmailService
{
    public class SendEmailService
    {

        XElement xElement = null;
        string SMTPServer = string.Empty;
        int SMTPPort = 0;
        public SendEmailService()
        {
            SMTPServer = ConfigurationManager.AppSettings["SMTPHost"];
            SMTPPort = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
        }

        public SendEmailService(string EmailXMLPath, string smtpServerName, int smtpServerPort)
        {
            try
            {
                xElement = XElement.Load(EmailXMLPath);
                SMTPServer = smtpServerName;
                SMTPPort = smtpServerPort;
            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }

        public void PrepareMergeField(EmailServiceDTO emailModel)
        {
            try
            {


                emailModel.Subject=emailModel.Subject.FindReplace("[COMPANY]", AppUtil.GetAppSettings(ConfigurationManager.AppSettings["CompanyName"]));
                emailModel.Body.FindReplace("[COMPANY]", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.CompanyName));
                emailModel.Body.FindReplace("[COMPANYLOGOURL]", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.CompanyLogoURL));
                emailModel.Body.FindReplace("[COMPANYWEBURL]", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.CompanyWebsite));
                emailModel.Body.FindReplace("[COMPANYEMAIL]", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.FromEmail));
                emailModel.Body.FindReplace("[QUERYDATE]", DateTime.Now.ToShortDateString());


            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        //public string FindReplace(string input, string replaceTag, string replaceVal)
        //{
        //    StringBuilder b = new StringBuilder(input);
        //    string FinalVal = String.Empty;
        //    FinalVal = b.Replace(replaceTag, replaceVal).ToString();


        //    return FinalVal;
        //}
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

        public int SendEmail(EmailServiceDTO emailmodel)
        {
            string fromPass, fromAddress, fromName = "";
            PrepareMergeField(emailmodel);
            MailMessage message = new MailMessage();
            SmtpClient smtpClient = new SmtpClient();
            bool isDebugMode = ConfigurationManager.AppSettings["IsDebugMode"] == "Y" ? true : false;
            try
            {
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["CCAddress"]))
                    emailmodel.CcEmail = ConfigurationManager.AppSettings["CCAddress"];

                if (isDebugMode)
                {

                    message.To.Add(ConfigurationManager.AppSettings["DbugToEmail"].ToString());
                    fromAddress = ConfigurationManager.AppSettings["DbugFromEmail"].ToString();
                    fromPass = ConfigurationManager.AppSettings["DbugFromPass"];
                    smtpClient.Host = ConfigurationManager.AppSettings["DbugSMTPHost"]; //"relay-hosting.secureserver.net";   //-- Donot change.
                    smtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["DbugSMTPPort"]); // 587; //--- Donot change    
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new System.Net.NetworkCredential(fromAddress, fromPass);
                    message.Subject = "[Debug Mode ON] - " + emailmodel.Subject;

                }
                else
                {
                    fromName = ConfigurationManager.AppSettings["FromName"].ToString();
                    fromAddress = ConfigurationManager.AppSettings["FromEmail"].ToString();
                    smtpClient.Host = ConfigurationManager.AppSettings["SMTPHost"];
                    message.To.Add(emailmodel.ToEmail);
                    message.Subject = emailmodel.Subject;
                }

                message.BodyEncoding = Encoding.UTF8;
                message.From = new System.Net.Mail.MailAddress(fromAddress, emailmodel.FromName);
                message.IsBodyHtml = true;
                message.Body = emailmodel.Body;
                smtpClient.Send(message);
                message.Dispose();
                return (int)AspectEnums.EmailStatus.Sent;
                //var isInsert =  DBEntities.EmailServices.Add(emailModel);
                //DBEntities.SaveChanges();

            }
            catch (Exception ex)
            {
                return (int)AspectEnums.EmailStatus.Failed;
            }
        }

    }
}