using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Samsung.SmartDost.BusinessLayer.Base;
using Samsung.SmartDost.BusinessLayer.Services.Contracts;
using Samsung.SmartDost.CommonLayer.AopContainer;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.CommonLayer.Aspects.Logging;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using Samsung.SmartDost.PersistenceLayer.Repository.Contracts;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Samsung.SmartDost.PersistenceLayer.Data.Impl;
using Samsung.SmartDost.CommonLayer.EntityMapper;
using Samsung.SmartDost.BusinessLayer.Services.BO;
using Samsung.SmartDost.BusinessLayer.IC;
using System.Web;
using System.Web.Caching;
using System.Collections;

namespace Samsung.SmartDost.BusinessLayer.ServiceImpl
{
    /// <summary>
    /// Class to hold the instances and method for batch job
    /// </summary>
    public class BatchManager : ServiceBase, IBatchService
    {
        private MailingEngine mailEngine;

        public MailingEngine MailingEngine
        {
            get
            {
                if (mailEngine == null)
                {
                    mailEngine = new MailingEngine();
                }
                return mailEngine;
            }
        }

        public BatchManager()
        {
            InstantiateDatabaseAccess();
        }

        private ISystemRepository systemRepository;
        public ISystemRepository SystemRepository
        {
            get
            {
                if (systemRepository == null)
                {
                    systemRepository = new SystemDataImpl();
                }
                return systemRepository;
            }
        }

        private Microsoft.Practices.EnterpriseLibrary.Data.Database dataAccess;

        /// <summary>
        /// Property to get set database container instance of METLB
        /// </summary>
        public Microsoft.Practices.EnterpriseLibrary.Data.Database DataAccess
        {
            get
            {
                return dataAccess;
            }
            private set
            {
                dataAccess = value;
            }
        }

        public void SyncDataToDMS()
        {

        }

        /// <summary>
        /// Method to send email
        /// </summary>
        /// <param name="mail">mail details</param>
        /// <param name="setting">smtp server details</param>
        private void SendEmail(EmailServiceDTO mail, SMTPServer setting)
        {
            AspectEnums.EmailStatus mailStatus = AspectEnums.EmailStatus.None;
            SMTPServerDTO smtpDetail = new SMTPServerDTO();
            Mapper mapper = new Mapper();
            mapper.CreateMap<SMTPServer, SMTPServerDTO>();
            mapper.Map(setting, smtpDetail);
            string remarks = string.Empty;
            //call mathod to send email
            bool isSent = MailingEngine.SendEmail(mail, smtpDetail);
            if (isSent)
            {
                mailStatus = AspectEnums.EmailStatus.Delivered;
                remarks = "Success";
            }
            else
            {
                mailStatus = AspectEnums.EmailStatus.Failed;
                remarks = "Failure";
            }
            SystemRepository.UpdateEmailServiceStatus(mail.EmailServiceID, (int)mailStatus, remarks);
        }

        /// <summary>
        /// Method to insert email record into database
        /// </summary>
        /// <param name="email">email entity</param>
        /// <returns>returns boolean status</returns>
        public bool InsertEmailRecord(EmailServiceDTO email)
        {
            EmailService emailDetail = new EmailService();
            ObjectMapper.Map(email, emailDetail);
            return SystemRepository.InsertEmailRecord(emailDetail);
        }


        /// <summary>
        /// Batch method to send notification
        /// </summary>
        /// 

        //VC20140819        
        private void BatchNotifications()
        {
            try
            {
                DateTime CurrentDateTime = DateTime.Now;
                List<long> NotificationServiceID = new List<long>();//To update context after notification

                var Registration_IDsJSON = "";
                var groupedData = SystemRepository.GetNotificationService(CurrentDateTime).GroupBy(x => new { x.PushNotificationMessage, x.NotificationID }).ToList();
                // LogTraceEngine.WriteLogWithCategory("Notification service Called and count : " + groupedData.Count + "curent date time =" + CurrentDateTime.ToLongDateString(), AppVariables.AppLogTraceCategoryName.NotificationListener);
                AndroiddNotificationManager pushNotifier = new AndroiddNotificationManager();

                foreach (var notification in groupedData) //Iterate Grouped data on Notification ID
                {
                    NotificationServiceID.AddRange(notification.Select(x => x.NotificationServiceID));
                    Registration_IDsJSON = String.Join(",", notification.Select(x => "\"" + x.AndroidID + "\"").ToArray());
                    string response = pushNotifier.SendNotification(Registration_IDsJSON, notification.Key.PushNotificationMessage, 1); //Send for Notification
                    systemRepository.UpdateNotificationServiceResponse(NotificationServiceID, response, notification.Key.NotificationID.Value, CurrentDateTime); //Update Context 
                }
            }
            catch (Exception ex)
            {
                LogTraceEngine.WriteLogWithCategory("Batch Notification service Called and exception occured : Database string  : " + dataAccess.ConnectionString + " " + ex.Message, AppVariables.AppLogTraceCategoryName.NotificationListener);
            }
        }

        //PUSH Notification for Coverage
        private CoverageNotificationService GenerateCoverageNotificationResponse(bool isSuccess, bool isUpdate, long CoveragenotificationServiceID, string response)
        {
            CoverageNotificationService service = new CoverageNotificationService();
            if (service != null)
            {

                service.Remarks = response;
                service.ModifiedDate = System.DateTime.Now;
                service.CoverageNotificationServiceID = CoveragenotificationServiceID;
                if (isSuccess) // Notified successfully
                {
                    service.DeliveryStatus = 2;
                }
                else //Notification Failed
                {
                    service.DeliveryStatus = 1;
                }
            }
            return service;
        }
        public void CoverageNotifications()
        {
            try
            {
                //if (systemRepository == null)
                //{
                systemRepository = new SystemDataImpl();
                //}
                LogTraceEngine.WriteLogWithCategory("Coverage Notification service Called Step 1 ", AppVariables.AppLogTraceCategoryName.NotificationListener);
                if (systemRepository.CoverageNotificationTimeSetting() != null) //Get Time Slot settings
                {
                    LogTraceEngine.WriteLogWithCategory("Coverage Notification service Called Time Matched with TimeSettings ", AppVariables.AppLogTraceCategoryName.NotificationListener);
                    //If time slot gets in current time                                                
                    var notifications = SystemRepository.GetCoverageNotificationService(); //Execute Procedure to generate Notification Data    
                    AndroiddNotificationManager pushNotifier = new AndroiddNotificationManager();
                    List<CoverageNotificationService> CoverageNotification = new List<CoverageNotificationService>();

                    if (notifications != null && notifications.Count > 0)
                    {
                        LogTraceEngine.WriteLogWithCategory("Coverage Notification started pushing notification to users ", AppVariables.AppLogTraceCategoryName.NotificationListener);
                        foreach (var notification in notifications)
                        {
                            if (!String.IsNullOrEmpty(notification.AndroidID))
                            {
                                string response = pushNotifier.SendNotification("\"" + notification.AndroidID + "\"", "Coverage Notification~" + notification.PushNotificationMessage, 1); //Send for Notification
                                if (!String.IsNullOrEmpty(response) && response.Contains("id"))
                                {
                                    response = String.Format("Success Coverage Notification Status For UserID: {0} of Notification Date {1},{2}", notification.UserID, DateTime.Now.ToString(), response);
                                    CoverageNotification.Add(GenerateCoverageNotificationResponse(true, false, notification.CoverageNotificationServiceID, "Success: " + response));
                                }
                                else
                                {
                                    if (response != null)
                                    {
                                        response = String.Format("Failure Coverage Notification Status For UserID: {0} of Notification Date {1},{2}", notification.UserID, DateTime.Now.ToString(), response);
                                    }
                                    CoverageNotification.Add(GenerateCoverageNotificationResponse(true, false, notification.CoverageNotificationServiceID, "Success: " + response));
                                }
                            }
                        }
                        LogTraceEngine.WriteLogWithCategory("Coverage Notification End pushing notification to users ", AppVariables.AppLogTraceCategoryName.NotificationListener);


                        SystemRepository.UpdateCoverageNotificationServiceResponse(CoverageNotification);
                    }
                }

            }
            catch (Exception ex)
            {
                LogTraceEngine.WriteLogWithCategory("Coverage Notification service Called and exception occured : Database string  : " + dataAccess.ConnectionString + " " + ex.Message, AppVariables.AppLogTraceCategoryName.NotificationListener);
            }
        }
        //PUSH Notification for coverage
        //VC20140819

        public void SendBatchNotifications()
        {
            /*Code shifted to EXE
           if (AppUtil.GetAppSettings(AspectEnums.ConfigKeys.IsBatchNotificationOn) == "1")
           {
               try
               {
                   //Parallel.Invoke(
                   //     () =>
                   //     {
                   BatchNotifications();
                   //},
                   //() =>
                   //{
                   CoverageNotifications();
                   // }
                   //);                                                        
               }
               catch (Exception ex)
               {
                   LogTraceEngine.WriteLogWithCategory("Notification service Called and exception occured : Database string  : " + dataAccess.ConnectionString + " " + ex.Message, AppVariables.AppLogTraceCategoryName.NotificationListener);
               }
           }
             */
       }


       /// <summary>
       /// Method to get notifications
       /// </summary>
       /// <returns>returns notifications to user</returns>
       /// 
       //VC20140827
       //public IList<NotificationServiceBO> GetBatchNotifications()
       //{
       //    List<NotificationServiceBO> notifications = new List<NotificationServiceBO>();            
       //    List<object> paramObjects = new List<object>() { System.DateTime.Now.Date, System.DateTime.Today.AddDays(1).Date };
       //    using (IDataReader reader = DataAccess.ExecuteReader("SpGetBatchNotifications", paramObjects.ToArray()))
       //    {
       //        if (reader != null)
       //        {
       //            while (reader.Read())
       //            {
       //                NotificationServiceBO notification = new NotificationServiceBO()
       //                {
       //                    NotificationServiceID = Convert.ToInt64(reader[reader.GetOrdinal("NotificationServiceID")]),
       //                    NotificationID = Convert.ToInt64(GetReaderValue(reader, "NotificationID")),
       //                    AndroidID = Convert.ToString(reader[reader.GetOrdinal("AndroidID")]),
       //                    Frequency = Convert.ToInt32(reader[reader.GetOrdinal("Frequency")]),
       //                    Notification = Convert.ToString(GetReaderValue(reader, "PushNotificationMessage")),
       //                    NotificationDate = Convert.ToDateTime(reader[reader.GetOrdinal("NotificationDate")]),
       //                    EndDate = Convert.ToDateTime(reader[reader.GetOrdinal("EndDate")]),
       //                    UserID = Convert.ToInt64(reader[reader.GetOrdinal("UserID")]),
       //                };
       //                notifications.Add(notification);
       //            }
       //            reader.Close();
       //        }
       //    }

       //    return notifications;
       //}
       //VC20140827

       /// <summary>
       /// Method to fetch batch emails to send email
       /// </summary>
       /// <returns>returns batch emails</returns>
       public IList<EmailServiceDTO> GetBatchEmails()
       {
           List<EmailServiceDTO> emails = new List<EmailServiceDTO>();
           using (IDataReader reader = DataAccess.ExecuteReader("SpGetBatchEmail"))
           {
               if (reader != null)
               {
                   while (reader.Read())
                   {
                       EmailServiceDTO systemMail = new EmailServiceDTO()
                       {
                           EmailServiceID = Convert.ToInt64(reader[reader.GetOrdinal("EmailServiceID")]),
                           Remarks = Convert.ToString(GetReaderValue(reader, "Remarks")),
                           IsAttachment = Convert.ToBoolean(reader[reader.GetOrdinal("IsAttachment")]),
                           Body = Convert.ToString(GetReaderValue(reader, "Body")),
                           Subject = Convert.ToString(GetReaderValue(reader, "Subject")),
                           Status = Convert.ToInt32(reader[reader.GetOrdinal("Status")]),
                           IsHtml = Convert.ToBoolean(GetReaderValue(reader, "IsHtml")),
                           ToName = Convert.ToString(GetReaderValue(reader, "ToName")),
                           ToEmail = Convert.ToString(GetReaderValue(reader, "ToEmail")),
                           CcEmail = Convert.ToString(GetReaderValue(reader, "CcEmail")),
                           BccEmail = Convert.ToString(GetReaderValue(reader, "BccEmail")),
                           AttachmentFileName = Convert.ToString(GetReaderValue(reader, "AttachmentFileName")),

                       };
                       emails.Add(systemMail);
                   }
                   //reader.Close();
               }
           }

           return emails;
       }

       /// <summary>
       /// Method to send batch emails
       /// </summary>
       public void SendBatchEmails()
       {
           /*Code shifted to EXE
          bool isSchedulerOn = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.IsBatchSchedulerOn) == "1" ? true : false;
          if (isSchedulerOn)
          {
              
              IList<EmailServiceDTO> emails = GetBatchEmails();
              if (emails.Count > 0)
              {
                  try
                  {
                      SMTPServer smtpDetail = SystemRepository.GetEmailServerDetails();
                      if (smtpDetail != null)
                      {
                          foreach (EmailServiceDTO email in emails)
                          {
                              SendEmail(email, smtpDetail);
                          }
                      }
                      else
                      {
                          foreach (EmailServiceDTO email in emails)
                          {
                              SystemRepository.UpdateEmailServiceStatus(email.EmailServiceID, (int)AspectEnums.EmailStatus.Pending, "SMTP Details Not Found");
                          }
                      }
                  }
                  catch (Exception ex)
                  {
                      LogTraceEngine.WriteLog(ex.Message);
                      foreach (var item in emails)
                      {
                          SystemRepository.UpdateEmailServiceStatus(item.EmailServiceID, (int)AspectEnums.EmailStatus.Pending, "Error");
                      }

                  }
               
              }
          }
            */
        }

        /// <summary>
        /// Method to initialize enterprise library database container instance
        /// </summary>
        private void InstantiateDatabaseAccess()
        {
            if (dataAccess == null)
            {
                dataAccess = EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Data.Database>(AppUtil.GetAppSettings(AspectEnums.ConfigKeys.SQLDBConnectionName)) as SqlDatabase;
            }
            else
            {
                dataAccess = EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Data.Database>(AppUtil.GetAppSettings(AspectEnums.ConfigKeys.SQLDBConnectionName)) as SqlDatabase;
            }
        }

        /// <summary>
        /// Method to get Reader's nullable coilumn value which is compatible to db null type
        /// </summary>
        /// <param name="reader">data reader object</param>
        /// <param name="columnName">column name </param>
        /// <returns>returns value</returns>
        private object GetReaderValue(IDataReader reader, string columnName)
        {
            if (reader.IsDBNull(reader.GetOrdinal(columnName)))
            {
                return null;
            }
            else
            {
                return reader[reader.GetOrdinal(columnName)];
            }
        }

    }
}
