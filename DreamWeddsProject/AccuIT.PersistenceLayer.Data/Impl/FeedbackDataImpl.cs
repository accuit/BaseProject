using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samsung.SmartDost.PersistenceLayer.Repository.Contracts;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using System.IO;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using System.Web;
using Samsung.SmartDost.CommonLayer.Resources;
using System.Transactions;
using Samsung.SmartDost.CommonLayer.Aspects.Logging;

namespace Samsung.SmartDost.PersistenceLayer.Data.Impl
{
    public class FeedbackDataImpl : BaseDataImpl, IFeedbackRepository
    {
        SystemDataImpl systemImpl = new SystemDataImpl();
        public bool SubmitFeedbacks(List<SubmitFeedbacks> FeedBacks, long userID)
        {
            bool isSuccess = false;
            StringBuilder strexistingfeedbacks = new StringBuilder();
            StringBuilder strnewfeedbacks = new StringBuilder();
            //StringBuilder strnoSPOC = new StringBuilder();
            List<string> noSPOC = new List<string>();
            int countexistingfeedbacks = 0, countnewfeedbacks = 0;
            string newrowForNotificationMessage = "\r\n";
            string feedbacknumberidentifier = newrowForNotificationMessage + "<b>";
            string feedbacknumberidentifierEnd = "</b>";
            NotificationMessage MessageHeaderBody;
            List<NotificationMessage> notificationMessages; //SDCE-1082
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                     new TransactionOptions
                     {
                         IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                     }))
            {
                //Fetch list of messages to generate notification messages for SDCE-1082
                notificationMessages = SmartDostDbContext.NotificationMessages.Where
                                                        (x => x.NotificationType == (int)AspectEnums.NotificationType.FMS &&
                                                         x.IsDeleted == false &&
                                                         x.ForWebOrExe == (int)AspectEnums.InterfaceType.Web).ToList();

                #region finding existing feedbacks, their count and Feedback Numbers
                List<SubmitFeedbacks> existingfeedbacks = new List<SubmitFeedbacks>();

                foreach (var feedback in FeedBacks)
                {
                    var existingfeedback = SmartDostDbContext.FeedbackMasters.
                        Where(x => x.FeedbackCatID == feedback.FeedbackCatID &&
                                                        x.FeedbackTypeID == feedback.FeedbackTypeID &&
                                                        x.StoreID == feedback.storeID &&
                                                        x.CreatedBy == userID &&
                                                        x.IsDeleted == false &&
                                                        (x.CurrentStatusID != 4 &&
                                                        x.CurrentStatusID != 8 &&
                                                        x.CurrentStatusID != 9)).FirstOrDefault();

                    if (existingfeedback != null)
                    {
                        strexistingfeedbacks = strexistingfeedbacks.Append(feedbacknumberidentifier + existingfeedback.FeedbackNo + "," + existingfeedback.FeedbackID + feedbacknumberidentifierEnd);
                        existingfeedbacks.Add(feedback);
                        countexistingfeedbacks++;
                    }

                }
                #endregion
                #region remove existing feedbacks from List
                foreach (var item in existingfeedbacks)
                {
                    FeedBacks.Remove(item);
                }
                #endregion
                scope.Complete();
            }




            #region Create New Feedback
            if (FeedBacks.Count > 0)
            {
                var FeedBacksstoregrouped = FeedBacks.GroupBy(x => x.storeID).ToList();
                var resultuserMaster = SmartDostDbContext.UserMasters.FirstOrDefault(x => x.UserID == userID && x.IsDeleted == false);


                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }))
                {
                    foreach (var itemstore in FeedBacksstoregrouped)
                    {
                        var FeedBacksgrouped = itemstore.GroupBy(x => x.FeedbackTeamID).ToList();

                        var resultstoreMaster = SmartDostDbContext.StoreMasters.FirstOrDefault(x => x.StoreID == itemstore.Key && x.IsDeleted == false && x.IsActive == true);
                        DateTime date = DateTime.Now.Date;


                        foreach (var groupitem in FeedBacksgrouped)
                        {
                            FeedbackMaster objFeedback = null;
                            #region SPOC finding
                            var Spoc = GetSPOC(itemstore.Key, groupitem.Key).FirstOrDefault();
                            #endregion
                            //delete check is removed to prevent record save if master data deleted on console end but not on android end
                            var resultTeamMaster = SmartDostDbContext.TeamMasters.Where(x => x.TeamID == groupitem.Key).FirstOrDefault();

                            if (Spoc != null)
                            {

                                #region Generate Feedback Number
                                var feedbackCount = (from fm in SmartDostDbContext.FeedbackMasters
                                                     join team in SmartDostDbContext.FeedbackCategoryMasters
                                                     on fm.FeedbackCatID equals team.FeedbackCatID
                                                     where
                                                         fm.IsDeleted == false
                                                     &&
                                                         fm.CreatedBy == userID
                                                     &&
                                                         fm.StoreID == itemstore.Key
                                                     &&
                                                         fm.CreatedOn >= date
                                                     &&
                                                         team.TeamID == groupitem.Key
                                                     select new { fm.FeedbackNo }).Count() + 1;

                                StringBuilder feedbackNumber = new StringBuilder(resultuserMaster.EmplCode + "/" + resultstoreMaster.StoreCode + "/" + resultTeamMaster.Code + "/" + DateTime.Now.Date.ToString("ddMMyy"));
                                #endregion
                                int counter = 0;
                                foreach (var item in groupitem)
                                {
                                    counter++;

                                    #region TAT
                                    //delete check is removed to prevent record save if master data deleted on console end but not on android end
                                    var feedbackTypeMaster = SmartDostDbContext.FeedbackTypeMasters.Where(x => x.FeedbackCatID == item.FeedbackCatID && x.FeedbackTypeID == item.FeedbackTypeID).FirstOrDefault();
                                    #endregion
                                    string fileName = string.Empty;
                                    #region Store Image
                                    try
                                    {
                                        #region Store Image
                                        var image = Convert.FromBase64String(item.ImageBytes);
                                        Stream stream = new MemoryStream(image);
                                        string fileDirectory = GetUploadDirectory(AspectEnums.ImageFileTypes.FMS);
                                        if (Directory.Exists(fileDirectory))
                                        {
                                            FileStream fileData = null;
                                            string newFileName = groupitem.Key.ToString() + "_" + item.FeedbackCatID.ToString() + "_" + item.FeedbackTypeID.ToString() + userID.ToString() + itemstore.Key.ToString() + "_" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + counter.ToString() + ".jpeg";
                                            fileName = newFileName;
                                            newFileName = fileDirectory + @"\" + newFileName;
                                            using (fileData = new FileStream(newFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                                            {
                                                const int bufferLen = 4096;
                                                byte[] buffer = new byte[bufferLen];
                                                int count = 0;
                                                int totalBytes = 0;
                                                while ((count = stream.Read(buffer, 0, bufferLen)) > 0)
                                                {
                                                    totalBytes += count;
                                                    fileData.Write(buffer, 0, count);
                                                }
                                                //fileData.Close();
                                                stream.Close();
                                            }
                                        }
                                        #endregion
                                    }
                                    catch { }

                                    #endregion

                                    #region Prepare entity data for feedbackStatusLog
                                    List<FeedbackStatusLog> feedbackStatusLog = new List<FeedbackStatusLog>();
                                    feedbackStatusLog.Add(new FeedbackStatusLog()
                                    {
                                        Remarks = item.Remarks,
                                        StatusID = 1,
                                        PendingWith = Spoc.SpocID,
                                        IsDeleted = false,
                                        CreatedOn = DateTime.Now,
                                        CreatedBy = userID
                                    });
                                    #endregion

                                    #region Save data into Database
                                    objFeedback = new FeedbackMaster()
                                    {
                                        FeedbackNo = feedbackNumber + "/" + feedbackCount.ToString(),
                                        FeedbackCatID = item.FeedbackCatID,
                                        FeedbackTypeID = item.FeedbackTypeID,
                                        TAT = feedbackTypeMaster.TAT,
                                        IsDeleted = false,
                                        CurrentStatusID = 1,
                                        ImageURL = fileName == String.Empty ? null : fileName,
                                        PendingWith = Spoc.SpocID.Value,
                                        StoreID = itemstore.Key,
                                        CreatedOn = System.DateTime.Now,
                                        CreatedBy = userID,
                                        FeedbackStatusLogs = feedbackStatusLog,
                                        SpocID = Spoc.SpocID.Value
                                    };
                                    SmartDostDbContext.FeedbackMasters.Add(objFeedback);

                                    isSuccess = SmartDostDbContext.SaveChanges() > 1 ? true : false;
                                    strnewfeedbacks.Append(feedbacknumberidentifier + objFeedback.FeedbackNo + "," + objFeedback.FeedbackID + feedbacknumberidentifierEnd);

                                    /*added by vaishali for SDCE-1082*/
                                    MessageHeaderBody = notificationMessages.Where(x => x.NotificationSubType == (int)AspectEnums.FMSNotificationSubType.SPOC
                                                   && x.Attribute1 == (int)AspectEnums.FMSStatusIDs.PendingforETR).FirstOrDefault();

                                    systemImpl.QueueNotification(Spoc.SpocID.Value, String.Format(MessageHeaderBody.MessageHeader, resultuserMaster.Username) + String.Format(MessageHeaderBody.MessageBody, feedbacknumberidentifier + objFeedback.FeedbackNo + "," + objFeedback.FeedbackID + feedbacknumberidentifierEnd), AspectEnums.NotificationType.FMS);

                                    #endregion
                                    feedbackCount++; //increament the counter for feedbackNumber
                                    countnewfeedbacks++;
                                }
                            }
                            else
                            {
                                //If SPOC not found for store Geo
                                if (!noSPOC.Contains(resultTeamMaster.Name))
                                    noSPOC.Add(resultTeamMaster.Code);

                            }
                        }

                    }
                    scope.Complete();
                }
            }
            #endregion

            #region Queue Notification
            StringBuilder notificationMessage = new StringBuilder();
            MessageHeaderBody = notificationMessages.Where(x => x.NotificationSubType == (int)AspectEnums.FMSNotificationSubType.User
                                                    && x.Attribute1 == (int)AspectEnums.FMSStatusIDs.PendingforETR).FirstOrDefault();
            if (countnewfeedbacks > 0)
            {
                notificationMessage.Append(String.Format(MessageHeaderBody.MessageBody, newrowForNotificationMessage + strnewfeedbacks));
            }

            if (countexistingfeedbacks > 0)
            {
                notificationMessage.Append(countexistingfeedbacks + Messages.NotificationFMSfeedbackexists + newrowForNotificationMessage + strexistingfeedbacks);
            }

            if (!string.IsNullOrEmpty(notificationMessage.ToString()))
            {
                systemImpl.QueueNotification(userID, String.Format(MessageHeaderBody.MessageHeader, DateTime.Today.ToString("ddMMyy")) + notificationMessage.ToString(), AspectEnums.NotificationType.FMS);
            }

            if (noSPOC.Count > 0)
            {
                MessageHeaderBody = notificationMessages.Where(x => x.NotificationSubType == (int)AspectEnums.FMSNotificationSubType.User
                                        && x.Attribute3 == (int)AspectEnums.NotificationAttribute3.FMSSpocNotDefined).FirstOrDefault();

                systemImpl.QueueNotification(userID, String.Format(MessageHeaderBody.MessageHeader, DateTime.Today.ToString("ddMMyy")) + String.Format(MessageHeaderBody.MessageBody, String.Join("\r\n", noSPOC.Select(x => x))), AspectEnums.NotificationType.FMS);
            }

            #endregion

            return isSuccess;

        }

        public List<SPGetFeedbackSearch_Result> SearchFeedbacks(FeedbackSearch searchFeedBacks, int storeID, long userID, out bool HasMoreRows)
        {
            List<SPGetFeedbackSearch_Result> result = new List<SPGetFeedbackSearch_Result>();
            HasMoreRows = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }))
            {
                if (searchFeedBacks.Rowcounter < 1)
                {
                    searchFeedBacks.Rowcounter = (int)AspectEnums.RowCounter.FMSRowCounter;

                }

                //  result = SmartDostDbContext.SPGetFeedbackSearch(searchFeedBacks.FeedbackTeamIDs, searchFeedBacks.FeedbackCatIDs, searchFeedBacks.FeedbackTypeIDs,
                //searchFeedBacks.LastFeedbackID, searchFeedBacks.Rowcounter, searchFeedBacks.PendingWithType, userID, searchFeedBacks.StatusID, searchFeedBacks.DateFrom, searchFeedBacks.DateTo, null).ToList();


                result = SmartDostDbContext.SPGetFeedbackSearch(searchFeedBacks.FeedbackTeamIDs, searchFeedBacks.FeedbackCatIDs, searchFeedBacks.FeedbackTypeIDs,
                searchFeedBacks.LastFeedbackID, searchFeedBacks.Rowcounter + 1, searchFeedBacks.PendingWithType, userID, searchFeedBacks.StatusIDs, searchFeedBacks.DateFrom, searchFeedBacks.DateTo, null).ToList();

                HasMoreRows = result.Count > searchFeedBacks.Rowcounter ? true : false;
                result = result.Take(searchFeedBacks.Rowcounter).ToList();

                scope.Complete();
            }
            return result;
        }


        public List<SPGetSpoc_Result> GetSPOC(int storeID, int teamID)
        {
            return SmartDostDbContext.SPGetSpoc(storeID, teamID).ToList();

        }

        public bool UpdateFeedbacks(List<UpdateFeedback> feedbacks, long userID, int roleID)
        {
            bool IsSuccess = false;
            string feedbacknumberidentifier = "\r\n<b>";
            string feedbacknumberidentifierEnd = "</b>";
            List<NotificationMessage> notificationMessages;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                 new TransactionOptions
                 {
                     IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                 }))
            {
                DateTime Today = DateTime.Now;
                foreach (UpdateFeedback feedback in feedbacks)
                {
                    FeedbackMaster objFeedback = SmartDostDbContext.FeedbackMasters.Where(k => k.FeedbackID == feedback.FeedbackID && k.IsDeleted == false).FirstOrDefault();
                    if (objFeedback.CurrentStatusID != feedback.NewStatusID)
                    {
                        objFeedback.CurrentStatusID = feedback.NewStatusID;
                        objFeedback.PendingWith = feedback.UserIdPendingWith;
                        #region commented
                        //// TO Find SPOC of 
                        //long? UserID = objFeedback.CreatedBy;
                        //long? SpocID= (from k in SmartDostDbContext.FeedbackStatusLogs
                        //        where k.FeedbackID == feedback.FeedbackID && k.StatusID == 1 && k.IsDeleted == false
                        //        select k.PendingWith).FirstOrDefault();
                        #endregion
                        objFeedback.ModifiedOn = Today;
                        objFeedback.ModifiedBy = userID;
                        SmartDostDbContext.Entry<FeedbackMaster>(objFeedback).State = System.Data.EntityState.Modified;

                        FeedbackStatusLog objLog = new FeedbackStatusLog();
                        objLog.CreatedBy = userID;
                        objLog.CreatedOn = Today;
                        objLog.FeedbackID = feedback.FeedbackID;
                        objLog.StatusID = feedback.NewStatusID;
                        objLog.Remarks = feedback.Remarks;
                        objLog.PendingWith = feedback.UserIdPendingWith;
                        objLog.IsDeleted = false;
                        if (!string.IsNullOrWhiteSpace(feedback.ETRDate))
                        {
                            //string[] ar = feedback.ETRDate.Split('/');
                            //objLog.ResponseDate = new DateTime(Convert.ToInt32(ar[2]), Convert.ToInt32(ar[1]), Convert.ToInt32(ar[0]));
                            objLog.ResponseDate = Convert.ToDateTime(feedback.ETRDate);     // Current Format: dd/mmm/yyyy Changed as per ashish suggestion (24-Dec) Consistant format for search and Update
                        }
                        //   objLog.ResponseDate = new DateTime(Convert.ToInt32(feedback.ETRDate.Substring(0, 4)), Convert.ToInt32(feedback.ETRDate.Substring(4, 2)), Convert.ToInt32(feedback.ETRDate.Substring(6, 2)));
                        SmartDostDbContext.Entry<FeedbackStatusLog>(objLog).State = System.Data.EntityState.Added;
                        //TODO: Add Logic to send to Notification and Log table if required 

                        //Fetch list of messages to generate notification messages for SDCE-1082
                        notificationMessages = SmartDostDbContext.NotificationMessages.Where
                                                                (x => x.NotificationType == (int)AspectEnums.NotificationType.FMS &&
                                                                 x.IsDeleted == false &&
                                                                 x.ForWebOrExe == 1).ToList();


                        //Notify user for pending for closure
                        if (feedback.NewStatusID == (int)AspectEnums.FMSStatusIDs.PendingForClosure || feedback.NewStatusID == (int)AspectEnums.FMSStatusIDs.PendingForClosure2)
                        {
                            #region for SDCE-1082
                            var pendingFeedbacks = (from fm in SmartDostDbContext.FeedbackMasters
                                                    join um in SmartDostDbContext.UserMasters
                                                    on fm.SpocID equals um.UserID
                                                    select new { SpocName = um.Username, fm.FeedbackNo, fm.FeedbackID, fm.CurrentStatusID, fm.CreatedBy }).
                                                    Where(x =>
                                                            (x.FeedbackID == objFeedback.FeedbackID ||
                                                            (x.CreatedBy == objFeedback.CreatedBy &&
                                                            (x.CurrentStatusID == (int)AspectEnums.FMSStatusIDs.PendingForClosure || x.CurrentStatusID == (int)AspectEnums.FMSStatusIDs.PendingForClosure2)))).ToList();
                            ;

                            //Pick the feedbacks pending for closure 
                            StringBuilder feedbacksPendingForClosure = new StringBuilder();
                            foreach (var itempendingFeedbacks in pendingFeedbacks)
                            {
                                feedbacksPendingForClosure.Append(feedbacknumberidentifier + itempendingFeedbacks.FeedbackNo.ToString() + "," + itempendingFeedbacks.FeedbackID.ToString() + feedbacknumberidentifierEnd + " " + itempendingFeedbacks.SpocName);
                            }

                            var notificationMessage = notificationMessages.Where(x => x.NotificationSubType == (int)AspectEnums.FMSNotificationSubType.User &&
                                                                                x.Attribute1 == (int)AspectEnums.FMSStatusIDs.PendingForClosure).FirstOrDefault();
                            systemImpl.QueueNotification(objFeedback.CreatedBy, String.Format(notificationMessage.MessageHeader, pendingFeedbacks.Count) + String.Format(notificationMessage.MessageBody, feedbacksPendingForClosure), AspectEnums.NotificationType.FMS);

                            #endregion
                        }
                        #region commented by vaishali as per instructions in change of notification
                        //Notify Spoc for close status
                        //else if (feedback.NewStatusID == (int)AspectEnums.FMSStatusIDs.Closed || feedback.NewStatusID == (int)AspectEnums.FMSStatusIDs.ClosedwithAggree || feedback.NewStatusID == (int)AspectEnums.FMSStatusIDs.ClosedwithDisagree)
                        //{
                        //    systemImpl.QueueNotification(objFeedback.SpocID, Messages.NotificationFMSClosed + feedbacknumberidentifier + objFeedback.FeedbackNo.ToString() + "," + objFeedback.FeedbackID.ToString() + feedbacknumberidentifierEnd, AspectEnums.NotificationType.FMS);
                        //}
                        #endregion

                        //Notify Spoc for Re raised feedback
                        else if (feedback.NewStatusID == (int)AspectEnums.FMSStatusIDs.PendingforETR2)
                        {
                            var notificationMessage = notificationMessages.Where(x => x.NotificationSubType == (int)AspectEnums.FMSNotificationSubType.SPOC && x.Attribute1 == (int)AspectEnums.FMSStatusIDs.PendingforETR2).FirstOrDefault();
                            systemImpl.QueueNotification(objFeedback.SpocID,
                                                        String.Format(notificationMessage.MessageHeader, objFeedback.FeedbackNo.ToString()) +
                                                        String.Format(notificationMessage.MessageBody, feedbacknumberidentifier + objFeedback.FeedbackNo.ToString() + "," + objFeedback.FeedbackID.ToString() + feedbacknumberidentifierEnd),
                                                        AspectEnums.NotificationType.FMS);

                        }
                        SmartDostDbContext.SaveChanges();
                    }
                }
                scope.Complete();
                IsSuccess = true;
            }
            return IsSuccess;
        }


        public List<TeamMaster> GetTeamMaster(long userID, int roleID, int LastTeamID, int rowcounter, out bool HasMoreRows)
        {
            HasMoreRows = false;
            List<TeamMaster> result = new List<TeamMaster>();

            if (!SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.FMS)).FirstOrDefault() == true)
                return result;

            result = SmartDostDbContext.TeamMasters.Where(k => k.TeamID > LastTeamID && k.IsDeleted == false).Take(rowcounter).ToList();
            if (result.Count > 0)
            {
                int MaxID = SmartDostDbContext.TeamMasters.Where(k => k.IsDeleted == false).Max(k => k.TeamID);
                HasMoreRows = result.Max(k => k.TeamID) < MaxID;
            }
            return result;
        }


        public List<FeedbackCategoryMaster> GetFeedbackCategoryMaster(long userID, int roleID, int LastCategoryID, int rowcounter, out bool HasMoreRows)
        {
            HasMoreRows = false;

            List<FeedbackCategoryMaster> result = new List<FeedbackCategoryMaster>();
            if (!SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.FMS)).FirstOrDefault() == true)
                return result;
            if (rowcounter > 0)
                result = SmartDostDbContext.FeedbackCategoryMasters.Where(k => k.FeedbackCatID > LastCategoryID && k.IsDeleted == false).Take(rowcounter).ToList();
            else
                result = SmartDostDbContext.FeedbackCategoryMasters.Where(k => k.FeedbackCatID > LastCategoryID && k.IsDeleted == false).ToList();

            if (result.Count > 0)
            {
                int MaxID = SmartDostDbContext.FeedbackCategoryMasters.Where(k => k.IsDeleted == false).Max(k => k.FeedbackCatID);
                HasMoreRows = result.Max(k => k.FeedbackCatID) < MaxID;
            }
            return result;

        }
        public List<FeedbackTypeMaster> GetFeedbackTypeMaster(long userID, int roleID, int LastTypeID, int rowcounter, out bool HasMoreRows)
        {
            HasMoreRows = false;
            List<FeedbackTypeMaster> result = new List<FeedbackTypeMaster>();
            if (!SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.FMS)).FirstOrDefault() == true)
                return result;
            result = SmartDostDbContext.FeedbackTypeMasters.Where(k => k.FeedbackTypeID > LastTypeID && k.IsDeleted == false).Take(rowcounter).ToList();
            if (result.Count > 0)
            {
                int MaxID = SmartDostDbContext.FeedbackTypeMasters.Where(k => k.IsDeleted == false).Max(k => k.FeedbackTypeID);
                HasMoreRows = result.Max(k => k.FeedbackTypeID) < MaxID;
            }
            return result;

        }
        public List<CommonSetup> GetMasterFromCommonSetup(long userID, int roleID, string MainType) //FeedbackStatusMaster
        {
            List<CommonSetup> result = new List<CommonSetup>();
            if (MainType == "FeedbackStatusMaster" && !SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.FMS)).FirstOrDefault() == true)
                return result;

            result = (from k in SmartDostDbContext.CommonSetups
                      where k.MainType == MainType && k.IsDeleted == false
                      select k).ToList();

            return result;
            //return SmartDostDbContext.CommonSetups.Where(k => k.MainType == "FeedbackStatusMaster" && k.IsDeleted == false).Select(k=>k.DisplayText) .ToList();
        }


        /// <summary>
        /// Method to get file upload directory
        /// </summary>
        /// <param name="fileType">file type</param>
        /// <returns>returns file directory</returns>
        private string GetUploadDirectory(AspectEnums.ImageFileTypes fileType)
        {
            string fileDirectory = HttpContext.Current.Server.MapPath("Contents");
            switch (fileType)
            {
                case AspectEnums.ImageFileTypes.FMS:
                    fileDirectory = fileDirectory + @"\FMS";
                    break;
            }
            return fileDirectory;
        }

        public FeedbackMaster GetFeedbackDetails(int FeedbackID)
        {

            return SmartDostDbContext.FeedbackMasters.FirstOrDefault(k => k.FeedbackID == FeedbackID && k.IsDeleted == false);
        }




        #region for Feedback Category and Type Master by Navneet on  05 Dec 2014
        public IList<FeedbackCategoryMaster> GetFeedbackCategoryList(int TeamID)
        {
            try
            {
                return SmartDostDbContext.FeedbackCategoryMasters.Where(x => x.TeamID == TeamID && x.IsDeleted == false).ToList();
            }
            catch { throw; }
        }

        public IList<FeedbackTypeMaster> GetFeedbackTypeList(int FeedbackCatID)
        {
            try
            {
                return SmartDostDbContext.FeedbackTypeMasters.Where(x => x.FeedbackCatID == FeedbackCatID && x.IsDeleted == false).ToList();
            }
            catch { throw; }
        }


        public FeedbackTypeMaster GetFeedbackType(int FeedbackTypeID)
        {
            try
            {
                return SmartDostDbContext.FeedbackTypeMasters.FirstOrDefault(x => x.FeedbackTypeID == FeedbackTypeID && x.IsDeleted == false);
            }
            catch { throw; }
        }

        public IList<FeedbackCategoryMaster> GetFeedbackCategoryList()
        {
            try
            {
                return SmartDostDbContext.FeedbackCategoryMasters.Where(x => x.IsDeleted == false).ToList();
            }
            catch { throw; }
        }

        public IList<FeedbackTypeMaster> GetFeedbackTypeList()
        {
            try
            {
                return SmartDostDbContext.FeedbackTypeMasters.Where(x => x.IsDeleted == false).ToList();
            }
            catch { throw; }
        }

        /// <summary>   
        /// Function to Add the Category 
        /// </summary>
        /// <param name="SurveyQuestion">FeedbackCategoryMaster</param>
        /// <returns>0 or 1</returns>
        public bool IsSuccessfullInsert(FeedbackCategoryMaster response)
        {
            try
            {
                response.CreatedOn = DateTime.Now;
                response.ModifiedOn = null;
                response.ModifiedBy = null;
                SmartDostDbContext.Entry<FeedbackCategoryMaster>(response).State = System.Data.EntityState.Added;
                return SmartDostDbContext.SaveChanges() > 0 ? true : false;
            }
            catch { throw; }
        }

        /// <summary>   
        /// Function to Update the Category 
        /// </summary>
        /// <param name="SurveyQuestion">FeedbackCategoryMaster</param>
        /// <returns>0 or 1</returns>
        public bool IsSuccessfullUpdate(FeedbackCategoryMaster response)
        {
            bool isSuccess = false;
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                     new TransactionOptions
                     {
                         IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                     }))
                {
                    FeedbackCategoryMaster feedbackCategoryMaster = SmartDostDbContext.FeedbackCategoryMasters.FirstOrDefault(k => k.FeedbackCatID == response.FeedbackCatID);
                    if (feedbackCategoryMaster != null)
                    {
                        feedbackCategoryMaster.FeedbackCategoryName = response.FeedbackCategoryName;
                        feedbackCategoryMaster.ModifiedOn = DateTime.Now;
                        SmartDostDbContext.Entry<FeedbackCategoryMaster>(feedbackCategoryMaster).State = System.Data.EntityState.Modified;
                        isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;
                    }
                    scope.Complete();
                    return isSuccess;
                }

            }
            catch { throw; }
        }

        public bool IsSuccessfullInsert(FeedbackTypeMaster response)
        {
            try
            {
                response.CreatedOn = DateTime.Now;
                response.ModifiedOn = null;
                response.ModifiedBy = null;
                SmartDostDbContext.Entry<FeedbackTypeMaster>(response).State = System.Data.EntityState.Added;
                return SmartDostDbContext.SaveChanges() > 0 ? true : false;
            }
            catch { throw; }
        }


        /// <summary>   
        /// Function to Update the Category 
        /// </summary>
        /// <param name="SurveyQuestion">FeedbackTypeMaster</param>
        /// <returns>0 or 1</returns>
        public bool IsSuccessfullUpdate(FeedbackTypeMaster response)
        {
            bool isSuccess = false;
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                     new TransactionOptions
                     {
                         IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                     }))
                {
                    FeedbackTypeMaster feedbackTypeMaster = SmartDostDbContext.FeedbackTypeMasters.FirstOrDefault(k => k.FeedbackTypeID == response.FeedbackTypeID);
                    if (feedbackTypeMaster != null)
                    {
                        feedbackTypeMaster.FeedbackTypeName = response.FeedbackTypeName;
                        feedbackTypeMaster.TAT = response.TAT;
                        feedbackTypeMaster.SampleImageName = response.SampleImageName;
                        feedbackTypeMaster.ModifiedOn = DateTime.Now;
                        feedbackTypeMaster.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]);
                        SmartDostDbContext.Entry<FeedbackTypeMaster>(feedbackTypeMaster).State = System.Data.EntityState.Modified;
                        isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;
                    }
                    scope.Complete();
                }
                return isSuccess;
            }
            catch { throw; }
        }


        /// <summary>
        /// Method to delelte a Category on basis on Category Id
        /// </summary>
        /// <param name="CategorysID"></param>
        /// <returns></returns>
        public bool DeleteCategorys(List<int> categorys)
        {
            try
            {
                bool isSuccess = false;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                      new TransactionOptions
                      {
                          IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                      }))
                {
                    foreach (int id in categorys)
                    {

                        // Delete Current FeedbackCategory //
                        FeedbackCategoryMaster feedbackCategoryMaster = SmartDostDbContext.FeedbackCategoryMasters.FirstOrDefault(m => m.FeedbackCatID == id);
                        if (feedbackCategoryMaster != null)
                        {
                            feedbackCategoryMaster.ModifiedOn = System.DateTime.Now;
                            feedbackCategoryMaster.IsDeleted = true;
                            SmartDostDbContext.Entry<FeedbackCategoryMaster>(feedbackCategoryMaster).State = System.Data.EntityState.Modified;
                        }


                        // Delete Sub FeedbackType of Current FeedbackCategory //
                        IList<FeedbackTypeMaster> feedbackTypeMasterList = SmartDostDbContext.FeedbackTypeMasters.Where(m => m.FeedbackCatID == id).ToList();
                        for (int i = 0; i < feedbackTypeMasterList.Count; i++)
                        {
                            feedbackTypeMasterList.ElementAtOrDefault(i).ModifiedOn = System.DateTime.Now;
                            feedbackTypeMasterList.ElementAtOrDefault(i).IsDeleted = true;
                            SmartDostDbContext.Entry<FeedbackTypeMaster>(feedbackTypeMasterList.ElementAtOrDefault(i)).State = System.Data.EntityState.Modified;
                        }
                    }
                    SmartDostDbContext.SaveChanges();
                    scope.Complete();
                    isSuccess = true;
                }
                return isSuccess;
            }
            catch { throw; }
        }


        public bool DeleteTypes(List<int> types)
        {
            try
            {
                bool isSuccess = false;
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                      new TransactionOptions
                      {
                          IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                      }))
                {
                    foreach (int id in types)
                    {
                        FeedbackTypeMaster feedbackTypeMaster = SmartDostDbContext.FeedbackTypeMasters.FirstOrDefault(m => m.FeedbackTypeID == id);

                        if (feedbackTypeMaster != null)
                        {
                            feedbackTypeMaster.ModifiedOn = System.DateTime.Now;
                            feedbackTypeMaster.IsDeleted = true;
                            SmartDostDbContext.Entry<FeedbackTypeMaster>(feedbackTypeMaster).State = System.Data.EntityState.Modified;
                        }
                    }
                    SmartDostDbContext.SaveChanges();
                    scope.Complete();
                    isSuccess = true;
                }
                return isSuccess;
            }
            catch { throw; }
        }

        #endregion


        #region Get Notifications (New Logic)
        public List<NotificationServiceLog> GetNotifications(long userID, int roleID, byte NotificationType, long LastNotificationServiceID, int RowCounter)
        {
            List<NotificationServiceLog> Result = new List<NotificationServiceLog>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                       new TransactionOptions
                       {
                           IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                       }))
            {
                DateTime currentDateTime = DateTime.Now;

                if (RowCounter < 1)
                {
                    RowCounter = (int)AspectEnums.RowCounter.FMSRowCounter;
                }
                int notificationType = (int)NotificationType;   // This is parent Module id of KAS Notification

                if (LastNotificationServiceID == 0)
                {
                    Result = (from auth in SmartDostDbContext.KASModuleAuthorizations
                              join kasmodule in SmartDostDbContext.KASModules on auth.KASModuleID equals kasmodule.KASModuleID
                              join log in SmartDostDbContext.NotificationServiceLogs on kasmodule.NotificationType equals log.NotificationType
                              where
                                  auth.RoleID == roleID &&
                                  auth.IsActive == true &&
                                  auth.IsDeleted == false &&
                                  kasmodule.IsDeleted == false &&
                                  kasmodule.IsActive == true &&
                                  log.UserID == userID &&
                                  kasmodule.KASParentModuleID == notificationType &&
                                  log.NotificationDate <= currentDateTime &&
                                  log.NotificationServiceID > LastNotificationServiceID
                              orderby log.NotificationServiceID descending
                              select log).Take(RowCounter).ToList();
                }
                else
                {
                    Result = (from auth in SmartDostDbContext.KASModuleAuthorizations
                              join kasmodule in SmartDostDbContext.KASModules on auth.KASModuleID equals kasmodule.KASModuleID
                              join log in SmartDostDbContext.NotificationServiceLogs on kasmodule.NotificationType equals log.NotificationType
                              where
                                  auth.RoleID == roleID &&
                                  kasmodule.IsDeleted == false &&
                                  kasmodule.IsActive == true &&
                                  log.UserID == userID &&
                                  kasmodule.KASParentModuleID == notificationType &&
                                  log.NotificationDate <= currentDateTime &&
                                  log.NotificationServiceID < LastNotificationServiceID
                              orderby log.NotificationServiceID descending
                              select log).Take(RowCounter).ToList();
                }
                scope.Complete();
            }
            return Result;
        }
        #endregion

        #region GetNotifications (Old File)
        //        public List<NotificationServiceLog> GetNotifications(long userID, int roleID, byte NotificationType, long LastNotificationServiceID, int RowCounter)
        //        {


        //            List<NotificationServiceLog> Result = new List<NotificationServiceLog>();
        //            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //                       new TransactionOptions
        //                       {
        //                           IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //                       }))
        //            {
        //                DateTime currentDateTime = DateTime.Now;

        //                if (RowCounter < 1)
        //                {
        //                    RowCounter = (int)AspectEnums.RowCounter.FMSRowCounter;
        //                }
        //                int notificationType = (int)NotificationType;

        //                if (LastNotificationServiceID == 0)
        //                    Result = SmartDostDbContext.NotificationServiceLogs.Where
        //                                (k => k.UserID == userID &&
        //                                    k.NotificationServiceID > LastNotificationServiceID &&
        //                                    k.NotificationDate <= currentDateTime &&
        //                                    (
        //                                        (
        //                                            notificationType == (int)AspectEnums.NotificationType.CustomNotificationManagement &&
        //                                            (
        //                                                k.NotificationType == notificationType ||
        //                                                k.NotificationType == (int)AspectEnums.NotificationType.Beat ||
        //                                                k.NotificationType == (int)AspectEnums.NotificationType.Coverage ||
        //                                                k.NotificationType == (int)AspectEnums.NotificationType.EPOS
        //                                            )
        //                                        )
        //                                        || k.NotificationType == notificationType
        //                                #region PSI Notification inside KAS SDCE-2256
        //                                    /*
        //                        Created By  :   Dhiraj
        //                        Created On  :   04-Mar-2015
        //                     *  Reason      :   To show PSI notification inside KAS under head of SOP
        //                     */
        //                                        ||
        //                                        (
        //                                            notificationType == (int)AspectEnums.NotificationType.SOP &&
        //                                            (
        //                                                k.NotificationType == notificationType ||
        //                                            k.NotificationType == (int)AspectEnums.NotificationType.NewDealer
        //                                            )
        //                                        )
        //                                #endregion
        //)
        //                                    ).OrderByDescending(k => k.NotificationServiceID).Take(RowCounter).ToList();
        //                //).OrderByDescending(k => k.NotificationDate).Take(RowCounter).ToList();
        //                else
        //                    Result = SmartDostDbContext.NotificationServiceLogs.Where
        //                            (k => k.UserID == userID &&
        //                                k.NotificationServiceID < LastNotificationServiceID &&
        //                                k.NotificationDate <= currentDateTime &&
        //                                (
        //                                        (notificationType == (int)AspectEnums.NotificationType.CustomNotificationManagement &&
        //                                        (
        //                                            k.NotificationType == notificationType ||
        //                                            k.NotificationType == (int)AspectEnums.NotificationType.Beat ||
        //                                            k.NotificationType == (int)AspectEnums.NotificationType.Coverage ||
        //                                            k.NotificationType == (int)AspectEnums.NotificationType.EPOS
        //                                        )
        //                                        )
        //                                        || k.NotificationType == notificationType
        //                            #region PSI Notification inside KAS SDCE-2256
        //                                /*
        //                        Created By  :   Dhiraj
        //                        Created On  :   04-Mar-2015
        //                     *  Reason      :   To show PSI notification inside KAS under head of SOP
        //                     */
        //                                        ||
        //                                        (
        //                                            notificationType == (int)AspectEnums.NotificationType.SOP &&
        //                                            (
        //                                                k.NotificationType == notificationType ||
        //                                            k.NotificationType == (int)AspectEnums.NotificationType.NewDealer
        //                                            )
        //                                        )
        //                            #endregion
        //)
        //                                 ).OrderByDescending(k => k.NotificationServiceID).Take(RowCounter).ToList();
        //                scope.Complete();
        //            }
        //            return Result;
        //        }

        #endregion
        public bool UpdateNotificationStatus(long userID, int roleID, List<UpdateNotificationStatus> Notifications)
        {
            bool isSuccess = false;
            try
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                        new TransactionOptions
                        {
                            IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                        }))
                {
                    foreach (UpdateNotificationStatus obj in Notifications)
                    {

                        if (obj.NotificationServiceID == null || obj.NotificationServiceID == -1)//In case of general only
                        {
                            if (obj.UserID == null || obj.UserID == -1 || obj.UserID == 0)//In case user id is not avaiable with android && IMEI is provided
                            {
                                var employeeList = SmartDostDbContext.UserDevices.Where(x => x.IMEINumber == obj.IMEINumber && x.IsDeleted == false);
                                foreach (var item in employeeList)//In case same IMEI is registered with multiple user
                                {

                                    NotificationServiceLog ObjNotificaiton = SmartDostDbContext.NotificationServiceLogs.FirstOrDefault(k => k.NotificationID == obj.NotificationID && k.UserID == item.UserID);
                                    if (ObjNotificaiton != null)
                                    {
                                        ObjNotificaiton.ReadStatus = obj.ReadStatus;
                                        SmartDostDbContext.Entry<NotificationServiceLog>(ObjNotificaiton).State = System.Data.EntityState.Modified;
                                    }
                                }

                            }
                            else//In case user id is given by android
                            {
                                NotificationServiceLog ObjNotificaiton = SmartDostDbContext.NotificationServiceLogs.FirstOrDefault(k => k.NotificationID == obj.NotificationID && k.UserID == userID);
                                if (ObjNotificaiton != null)
                                {
                                    ObjNotificaiton.ReadStatus = obj.ReadStatus;
                                    SmartDostDbContext.Entry<NotificationServiceLog>(ObjNotificaiton).State = System.Data.EntityState.Modified;
                                }
                            }
                        }
                        else//In case of other than General Notification
                        {
                            NotificationServiceLog ObjNotificaiton = SmartDostDbContext.NotificationServiceLogs.FirstOrDefault(k => k.NotificationServiceID == obj.NotificationServiceID && k.UserID == userID);
                            if (ObjNotificaiton != null)
                            {
                                ObjNotificaiton.ReadStatus = obj.ReadStatus;
                                SmartDostDbContext.Entry<NotificationServiceLog>(ObjNotificaiton).State = System.Data.EntityState.Modified;
                            }
                        }

                    }
                    SmartDostDbContext.SaveChanges();

                    scope.Complete();
                    isSuccess = true;
                }
                return isSuccess;
            }
            catch
            {
                throw;
            }
        }

        #region NotificationTypeMaster New
        public List<NotificationTypeMaster> NotificationTypeMaster(long userID, int roleID)
        {
            /*
                 * TODO: 
                 * 1. Get All KAS Module (join Parent) Masters
                 * 2. Get All KAS Modules whose Authorization is given to selected Role
                 * 3. Get the NotificationsServiceLog Count joining with KAS Modules
                */
            DateTime CurrentDate = DateTime.Now;
            List<NotificationTypeMaster> result = new List<NotificationTypeMaster>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }))
            {
                var lstNotificationTypes = (from auth in SmartDostDbContext.KASModuleAuthorizations
                                            join child in SmartDostDbContext.KASModules on auth.KASModuleID equals child.KASModuleID
                                            join parent in SmartDostDbContext.KASParentModules on child.KASParentModuleID equals parent.KASParentModuleID
                                            join log in SmartDostDbContext.NotificationServiceLogs on child.NotificationType equals log.NotificationType
                                            where auth.RoleID == roleID && parent.IsDeleted == false && parent.IsActive == true && child.IsDeleted == false
                                                    && child.IsActive == true && log.UserID == userID && log.NotificationDate <= CurrentDate
                                            select new
                                            {
                                                parentName = parent.Name,
                                                parentID = parent.KASParentModuleID,
                                                NotificationType = child.NotificationType,
                                                NotificationServiceID = log.NotificationServiceID,
                                                ReadStatus = log.ReadStatus
                                            });

                var TotalCounts = (
                                        from t in lstNotificationTypes
                                        group t by new { t.parentName, t.parentID }
                                            into grp
                                            select new { grp.Key.parentName, grp.Key.parentID, TotalCount = grp.Count() }
                                    ).ToList();




                var UnReadCounts = (
                                        from t in lstNotificationTypes
                                        where t.ReadStatus == 0
                                        group t by new { t.parentName, t.parentID }
                                            into grp
                                            select new { grp.Key.parentName, grp.Key.parentID, UnReadCounts = grp.Count() }
                                    ).ToList();



                result = (from t in TotalCounts
                          join r in UnReadCounts on t.parentID equals r.parentID
                          into lj
                          from unread in lj.DefaultIfEmpty()
                          select new NotificationTypeMaster()
                          {
                              NotificationType = (byte)t.parentID,
                              NotificationTypeDescription = t.parentName,
                              TotalCount = t.TotalCount,
                              UnreadCount = unread == null ? 0 : unread.UnReadCounts
                          }).OrderBy(k => k.NotificationTypeDescription).ToList();
                scope.Complete();
            }
            return result;
        }
        #endregion

        #region NotificationTypeMaster Old
        //public List<NotificationTypeMaster> NotificationTypeMaster(long userID, int roleID)
        //{
        //    //return true;
        //    DateTime CurrentDate = DateTime.Now;
        //    List<NotificationTypeMaster> result = new List<NotificationTypeMaster>();
        //    IQueryable<NotificationServiceLog> notificationTypeMaster;

        //    List<KASModule> KasModules = SmartDostDbContext.KASModules.Where(k => !k.IsDeleted && k.IsActive).ToList();

        //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //             new TransactionOptions
        //             {
        //                 IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //             }))
        //    {
        //        var commonsetup = SmartDostDbContext.CommonSetups.Where(x => x.MainType == "notificationtypemaster" && x.SubType == "NotificationType" && x.IsDeleted == false).ToList();

        //        var notificationServiceLog = SmartDostDbContext.NotificationServiceLogs.Where(x => x.UserID == userID && x.NotificationDate <= CurrentDate);

        //        foreach (var itemcommonsetup in commonsetup)
        //        {
        //            #region for SDCE-1201 by vaishali on 07 Jan 2015
        //            if (itemcommonsetup.DisplayValue == (int)AspectEnums.NotificationType.CustomNotificationManagement)
        //            {
        //                notificationTypeMaster = notificationServiceLog.
        //                                         Where(x => x.NotificationType == itemcommonsetup.DisplayValue ||
        //                                             x.NotificationType == (int)AspectEnums.NotificationType.Beat ||
        //                                             x.NotificationType == (int)AspectEnums.NotificationType.Coverage ||
        //                                              x.NotificationType == (int)AspectEnums.NotificationType.EPOS
        //                                             );
        //            }
        //            #region PSI Notification inside KAS SDCE-2256
        //            /*
        //                Created By  :   Dhiraj
        //                Created On  :   04-Mar-2015
        //             *  Reason      :   To show PSI notification inside KAS under head of SOP
        //             */
        //            else if (itemcommonsetup.DisplayValue == (int)AspectEnums.NotificationType.SOP)
        //            {
        //                notificationTypeMaster = notificationServiceLog.
        //                                         Where(x => x.NotificationType == itemcommonsetup.DisplayValue ||
        //                                             x.NotificationType == (int)AspectEnums.NotificationType.NewDealer
        //                                             );
        //            }
        //            #endregion

        //            else
        //            {
        //                notificationTypeMaster = notificationServiceLog.
        //                                             Where(x => x.NotificationType == itemcommonsetup.DisplayValue);
        //            }
        //            #endregion

        //            result.Add(new NotificationTypeMaster()
        //            {
        //                NotificationType = itemcommonsetup.DisplayValue.Value,
        //                NotificationTypeDescription = itemcommonsetup.DisplayText,
        //                UnreadCount = notificationTypeMaster.Where(x => x.ReadStatus == 0).Count(),
        //                TotalCount = notificationTypeMaster.Count()

        //            });
        //        }
        //    }

        //    return result;
        //}
        #endregion

        public List<SPGetFeedbackCountSearch_Result> SearchFeedbackStatusCount(FeedbackCountSearch feedbackCountSearch, long? userID, int roleID)
        {
            return SmartDostDbContext.SPGetFeedbackCountSearch(feedbackCountSearch.FeedbackTeamIDs, feedbackCountSearch.FeedbackCatIDs, feedbackCountSearch.FeedbackTypeIDs, userID).ToList();
        }


        #region KAS Authorization
        /*
         * Created By: Amit Mishra
         * Date: 31 Mar 2015
         * JIRA ID: SDCE-2257
         */
        /// <summary>
        /// Function to get the KAS modules master data
        /// </summary>
        public IList<KASModule> GetKASModulesList()
        {
            List<KASModule> result = new List<KASModule>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                     new TransactionOptions
                     {
                         IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                     }))
            {
                result = SmartDostDbContext.KASModules.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                scope.Complete();

            }
            return result;
        }

        /// <summary>
        /// Get KAS Authorizations assigned to selected role
        /// </summary>
        /// <param name="RoleID">Particular Role ID for which data of KAS authorization needs to be fetched</param>
        /// <returns>List of KAS Authorization Data for a Role </returns>
        public List<KASModuleAuthorization> GetKASAuthorizationByRoleID(int RoleID)
        {
            List<KASModuleAuthorization> result = new List<KASModuleAuthorization>();

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                     new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }))
            {
                result = SmartDostDbContext.KASModuleAuthorizations.Where(x => x.RoleID == RoleID && x.IsDeleted == false && x.IsActive == true).ToList();
                scope.Complete();

            }
            return result;
        }

        /// <summary>
        /// Insert KAS Authorizations assigned to selected role
        /// </summary>
        /// <param name="kasModuleList">List of KAS Authorization Data for a Role</param>
        /// <returns>boolean value of if successfully executed</returns>
        public bool InsertKASAuthorization(List<KASModuleAuthorization> kasModuleList)
        {
            bool isSuccess = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }))
            {
                foreach (KASModuleAuthorization item in kasModuleList)
                {
                    if (item != null)
                    {
                        SmartDostDbContext.Entry<KASModuleAuthorization>(item).State = System.Data.EntityState.Added;
                    }
                }
                isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;
                scope.Complete();

            }
            return isSuccess;


        }

        /// <summary>
        /// Insert KAS Authorizations assigned to selected role
        /// </summary>
        /// <param name="kasModuleList">List of KAS Authorization Data for a Role</param>
        /// <returns>boolean value of if successfully executed</returns>
        public bool DeleteKASAuthorization(List<KASModuleAuthorization> kasModuleList)
        {
            bool isSuccess = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions
                   {
                       IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                   }))
            {
                foreach (KASModuleAuthorization item in kasModuleList)
                {
                    var kasExisting = SmartDostDbContext.KASModuleAuthorizations.Where(x => x.KASModuleAuthorizationID == item.KASModuleAuthorizationID && x.RoleID == item.RoleID).SingleOrDefault();
                    if (item != null)
                    {
                        SmartDostDbContext.KASModuleAuthorizations.Remove(kasExisting);
                    }
                }
                isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;
                scope.Complete();

            }
            return isSuccess;
        }
        #endregion

        #region MSS services for sync adaptor
        public IList<FeedbackTypeMaster> GetMSSTypeMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;

            IList<FeedbackTypeMaster> result = new List<FeedbackTypeMaster>();

            if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.FMS)).FirstOrDefault() == true)
            {
                if (LastUpdatedDate == null)
                {
                    result = SmartDostDbContext.FeedbackTypeMasters.Where(k =>!k.IsDeleted) 
                       .OrderBy(k => (k.ModifiedOn??k.CreatedOn))
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       

                }
                else
                {

                    result = SmartDostDbContext.FeedbackTypeMasters.Where(k => (
                            (LastUpdatedDate<(k.ModifiedOn ?? k.CreatedOn))
                            ||
                            (LastUpdatedDate.Value == (k.ModifiedOn ?? k.CreatedOn))
                            ))
                       .OrderBy(k => (k.ModifiedOn ?? k.CreatedOn))
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList();//+1 is used to know that "is data more than rowcount is available" or not            

                                              
                }

                HasMoreRows = result.Count > RowCount ? true : false;
                result = result.Take(RowCount).ToList();

                // Update last modified data among the data if available, else send the same modifieddate back  
                if (result.Count > 0)
                {
                    if (LastUpdatedDate == null && HasMoreRows == true)
                        MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
                    else if (LastUpdatedDate == null && HasMoreRows == false)
                        MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
                    else
                        MaxModifiedDate = result.Max(x =>  (x.ModifiedOn ?? x.CreatedOn));

                }

            }
            return result;
        }




        public IList<TeamMaster> GetMSSTeamMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;

            IList<TeamMaster> result = new List<TeamMaster>();

            
            if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.FMS)).FirstOrDefault() == true)
            {
                if (LastUpdatedDate == null)
                {
                    result = SmartDostDbContext.TeamMasters.Where(k => !k.IsDeleted)
                       .OrderBy(k => (k.ModifiedDate ?? k.CreatedDate))
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       

                }
                else
                {

                    result = SmartDostDbContext.TeamMasters.Where(k => (
                            (LastUpdatedDate < (k.ModifiedDate ?? k.CreatedDate))
                            ||
                            (LastUpdatedDate.Value == (k.ModifiedDate ?? k.CreatedDate))
                            ))
                       .OrderBy(k => (k.ModifiedDate ?? k.CreatedDate))
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList();//+1 is used to know that "is data more than rowcount is available" or not            


                }

                HasMoreRows = result.Count > RowCount ? true : false;
                result = result.Take(RowCount).ToList();

                // Update last modified data among the data if available, else send the same modifieddate back  
                if (result.Count > 0)
                {
                    if (LastUpdatedDate == null && HasMoreRows == true)
                        MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
                    else if (LastUpdatedDate == null && HasMoreRows == false)
                        MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
                    else
                        MaxModifiedDate = result.Max(k => (k.ModifiedDate ?? k.CreatedDate));

                }

            }
            return result;
        }


        public IList<FeedbackCategoryMaster> GetMSSCategoryMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;

            IList<FeedbackCategoryMaster> result = new List<FeedbackCategoryMaster>();


            if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.FMS)).FirstOrDefault() == true)
            {
                if (LastUpdatedDate == null)
                {
                    result = SmartDostDbContext.FeedbackCategoryMasters.Where(k => !k.IsDeleted)
                       .OrderBy(k => (k.ModifiedOn ?? k.CreatedOn))
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       

                }
                else
                {

                    result = SmartDostDbContext.FeedbackCategoryMasters.Where(k => (
                            (LastUpdatedDate < (k.ModifiedOn ?? k.CreatedOn))
                            ||
                            (LastUpdatedDate.Value == (k.ModifiedOn ?? k.CreatedOn))
                            ))
                       .OrderBy(k => k.ModifiedOn ?? k.CreatedOn)
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList();//+1 is used to know that "is data more than rowcount is available" or not            


                }

                HasMoreRows = result.Count > RowCount ? true : false;
                result = result.Take(RowCount).ToList();

                // Update last modified data among the data if available, else send the same modifieddate back  
                if (result.Count > 0)
                {
                    if (LastUpdatedDate == null && HasMoreRows == true)
                        MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
                    else if (LastUpdatedDate == null && HasMoreRows == false)
                        MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
                    else
                        MaxModifiedDate = result.Max(k => k.ModifiedOn ?? k.CreatedOn);

                }

            }
            return result;
        }



        public IList<CommonSetup> GetMSSStatusMaster(long userID, int roleID, string MainType,int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;

            IList<CommonSetup> result = new List<CommonSetup>();


            if (MainType == "FeedbackStatusMaster" && SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.FMS)).FirstOrDefault() == true)
            {
                if (LastUpdatedDate == null)
                {
                    result = SmartDostDbContext.CommonSetups.Where(k => !k.IsDeleted)
                       .OrderBy(k => (k.ModifiedDate ?? k.CreatedDate))
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       

                }
                else
                {

                    result = SmartDostDbContext.CommonSetups.Where(k => (
                            (LastUpdatedDate < (k.ModifiedDate ?? k.CreatedDate))
                            ||
                            (LastUpdatedDate.Value == (k.ModifiedDate ?? k.CreatedDate))
                            ))
                       .OrderBy(k => k.ModifiedDate ?? k.CreatedDate)
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList();//+1 is used to know that "is data more than rowcount is available" or not            


                }

                HasMoreRows = result.Count > RowCount ? true : false;
                result = result.Take(RowCount).ToList();

                // Update last modified data among the data if available, else send the same modifieddate back  
                if (result.Count > 0)
                {
                    if (LastUpdatedDate == null && HasMoreRows == true)
                        MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
                    else if (LastUpdatedDate == null && HasMoreRows == false)
                        MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
                    else
                        MaxModifiedDate = result.Max(k => k.ModifiedDate ?? k.CreatedDate);

                }

            }
            return result;
        }

        #endregion
    }
}
