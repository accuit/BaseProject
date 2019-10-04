using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.CommonLayer.Aspects.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using AccuIT.CommonLayer.Aspects.DTO;

namespace AccuIT.CommonLayer.Aspects.Utilities
{
    /// <summary>
    /// Class to send email and manage email related attributes
    /// </summary>
    public class MailingEngine
    {
        /// <summary>
        /// Enums to identify mail address type
        /// </summary>
        enum MailAddressType
        {
            ToAddress,
            CcAddress,
            BccAddress,
        }



        /// <summary>
        /// Method to formatmail addresses by using address type, mail message and address value
        /// </summary>
        /// <param name="emailAddresses">email addresses separated with ; in case of multiple address</param>
        /// <param name="mailName">to address names</param>
        /// <param name="mailMessage">mail message instance</param>
        /// <param name="addressType">mail address type</param>
        private void ManageMailMessageAddress(string emailAddresses, string mailName, MailMessage mailMessage, MailAddressType addressType)
        {
            if (mailMessage == null || String.IsNullOrEmpty(emailAddresses))
                return;
            if (mailName == null)
                mailName = string.Empty;
            string[] addressList = emailAddresses.Split(new char[] { ';', ',' });
            switch (addressType)
            {
                //if address type provided is to address
                case MailAddressType.ToAddress:
                    if (addressList.Length > 1)
                    {
                        addressList.ToList().ForEach(address =>
                        {
                            if (!String.IsNullOrEmpty(address))
                            {
                                mailMessage.To.Add(new MailAddress(address));
                            }
                        });

                    }
                    else
                    {
                        mailMessage.To.Add(new MailAddress(addressList[0], mailName));
                    }
                    break;
                //if address type provided is cc address
                case MailAddressType.CcAddress:
                    if (addressList.Length > 1)
                    {
                        addressList.ToList().ForEach(address =>
                        {
                            if (!String.IsNullOrEmpty(address))
                            {
                                mailMessage.CC.Add(new MailAddress(address));
                            }
                        });

                    }
                    else
                    {
                        mailMessage.CC.Add(new MailAddress(addressList[0], mailName));
                    }
                    break;
                //if address type provided is bcc address
                case MailAddressType.BccAddress:
                    if (addressList.Length > 1)
                    {
                        addressList.ToList().ForEach(address =>
                        {
                            if (!String.IsNullOrEmpty(address))
                            {
                                mailMessage.Bcc.Add(new MailAddress(address));
                            }
                        });

                    }
                    else
                    {
                        mailMessage.Bcc.Add(new MailAddress(addressList[0], mailName));
                    }
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mail">mail need to be sent</param>
        /// <param name="smtpSettings">smtp settings</param>
        /// <returns>TRUE if the email sent successfully, FALSE otherwise</returns>
        public bool SendEmail(EmailServiceDTO mail, SMTPServerDTO smtpSettings)
        {
            bool isSuccess = false;
            try
            {
                // setup email header
                MailMessage mailMessage = new MailMessage();
                // Set the message sender
                // sets the from address for this e-mail message. 
                mailMessage.From = new MailAddress(smtpSettings.FromEmail, smtpSettings.FromName);
                // Sets the address collection that contains the recipients of this e-mail message. 
                ManageMailMessageAddress(mail.ToEmail, mail.ToName, mailMessage, MailAddressType.ToAddress);
                ManageMailMessageAddress(mail.CcEmail, string.Empty, mailMessage, MailAddressType.CcAddress);
                ManageMailMessageAddress(mail.BccEmail, string.Empty, mailMessage, MailAddressType.BccAddress);
                // sets the message subject.
                mailMessage.Subject = mail.Subject;
                // sets the message body. 
                mailMessage.Body = mail.Body;
                // sets a value indicating whether the mail message body is in Html. 
                // if this is false then ContentType of the Body content is "text/plain". 
                mailMessage.IsBodyHtml = mail.IsHtml;
                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                // add all the file attachments if we have any
                if (mail.IsAttachment && !String.IsNullOrEmpty(mail.AttachmentFileName))
                {
                    string[] files = mail.AttachmentFileName.Split(';');
                    foreach (string attachment in files)
                    {
                        
                        string reportFileLocation =  AppDomain.CurrentDomain.BaseDirectory+"\\"+ AppUtil.GetAppSettings(AspectEnums.ConfigKeys.ReportFileFolder);
                        LogTraceEngine.WriteLogWithCategory(reportFileLocation, AppVariables.AppLogTraceCategoryName.EmailListener);
                        reportFileLocation = String.Format(@"{0}\{1}", reportFileLocation, attachment);
                        if (File.Exists(reportFileLocation))
                        {
                            mailMessage.Attachments.Add(new Attachment(reportFileLocation));
                        }
                    }
                }

                // SmtpClient Class Allows applications to send e-mail by using the Simple Mail Transfer Protocol (SMTP).
                SmtpClient smtpClient = new SmtpClient(smtpSettings.ServerName, Convert.ToInt32(smtpSettings.PortNumber));

                //Specifies how email messages are delivered. Here Email is sent through the network to an SMTP server.
                smtpClient.Credentials = new System.Net.NetworkCredential(smtpSettings.UserName, smtpSettings.Password);
                smtpClient.Port = Convert.ToInt32(smtpSettings.PortNumber);
                smtpClient.Host = smtpSettings.ServerName;
                smtpClient.EnableSsl = smtpSettings.IsSSL;

                //Let's send it
                smtpClient.Send(mailMessage);
                isSuccess = true;
                // Do cleanup
                mailMessage.Dispose();
                smtpClient = null;

            }
            catch (Exception ex)
            {
                string response = String.Format("Email Failure for {0}, {1}", mail.ToEmail, ex.Message);
                isSuccess = false;
                LogTraceEngine.WriteLogWithCategory(response, AppVariables.AppLogTraceCategoryName.EmailListener);
                if (ex.InnerException != null)
                    LogTraceEngine.WriteLogWithCategory(ex.InnerException.ToString(), AppVariables.AppLogTraceCategoryName.EmailListener);
            }

            return isSuccess;
        }


    }
}
