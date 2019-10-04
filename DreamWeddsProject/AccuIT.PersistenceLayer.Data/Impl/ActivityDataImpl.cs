using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using Samsung.SmartDost.PersistenceLayer.Repository.Contracts;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Web;
using System.Transactions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Samsung.SmartDost.CommonLayer.Aspects.Logging;

namespace Samsung.SmartDost.PersistenceLayer.Data.Impl
{
    /// <summary>
    /// Class to manage the activities and surveys
    /// </summary>
    public class ActivityDataImpl : BaseDataImpl, IActivityRepository
    {
        /// <summary>
        /// Method to save store survey response on the basis of coverage beat
        /// </summary>
        /// <param name="storeSurvey">store survey</param>
        /// <returns>returns status</returns>
        public long SaveStoreSurveyResponse(SurveyResponse storeSurvey, bool? AuditRequired)
        {
            long status = 0;
            string fileName = string.Empty;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
    new TransactionOptions
    {
        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
    }))
            {
                DateTime currentDate = System.DateTime.Today;
                DateTime currentDateTime = System.DateTime.Now;
                SurveyResponse survey = null;
                if (storeSurvey.CoverageID.HasValue && storeSurvey.CoverageID.Value > 0)
                {
                    long coverageID = storeSurvey.CoverageID.Value;
                    survey = SmartDostDbContext.SurveyResponses.FirstOrDefault(k => k.CoverageID == coverageID && k.BeatPlanDate == currentDate);
                }
                else
                {
                    survey = SmartDostDbContext.SurveyResponses.FirstOrDefault(k => k.StoreID == storeSurvey.StoreID && k.UserID == storeSurvey.UserID && k.BeatPlanDate == currentDate);
                }
                if (survey == null)
                {
                    if (AuditRequired == true)
                    {
                        storeSurvey.AuditSummaries.Add(new AuditSummary { CreatedDate = currentDateTime, AuditNumber = "DemoAuditNumber", CreatedBy = storeSurvey.UserID });
                    }
                    storeSurvey.BeatPlanDate = currentDate;
                    storeSurvey.CreatedDate = currentDateTime;
                    SmartDostDbContext.SurveyResponses.Add(storeSurvey);
                    SmartDostDbContext.SaveChanges();
                    status = storeSurvey.SurveyResponseID;
                }
                else
                {

                    //survey.PictureFileName = storeSurvey.PictureFileName;
                    //survey.ModifiedDate = System.DateTime.Now;
                    //survey.Comments = storeSurvey.Comments;
                    //SmartDostDbContext.Entry<SurveyResponse>(survey).State = System.Data.EntityState.Modified;
                    //SmartDostDbContext.SaveChanges();
                    status = survey.SurveyResponseID;

                }
                UpdateStoreVisit(storeSurvey.UserID, storeSurvey.StoreID.Value);



                //if (storeSurvey.StoreID > 0)
                //{
                /*Geo tag is moved to another method named SubmitStoreGeoTag by Dhiraj on 20-May-2015
                if (!String.IsNullOrEmpty(storeSurvey.Lattitude) && !String.IsNullOrEmpty(storeSurvey.Longitude))
                {
                    StoreGeoTag geoTag = new StoreGeoTag()
                    {
                        StoreID = storeSurvey.StoreID.Value,
                        GeoTagDate = System.DateTime.Now,
                        UserID = storeSurvey.UserID,
                        CreatedDate = System.DateTime.Now,
                        Lattitude = storeSurvey.Lattitude,
                        Longitude = storeSurvey.Longitude,
                        PictureFileName = storeSurvey.PictureFileName,
                        CreatedBy = storeSurvey.UserID,
                        ISEligibleForFreeze = true,//Added by Dhiraj to add true everytime
                        UserOption = UserOption,
                    };
                    SmartDostDbContext.StoreGeoTags.Add(geoTag);
                    SmartDostDbContext.SaveChanges();

                    //if (UserOption != null) : Changed By Dhiraj 
                    if (UserOption == null)
                        SmartDostDbContext.SPFreezeGeoTag(storeSurvey.StoreID.Value);

                }
                /* Commented by Vaishali on 30 sep 2014 
                Tuple<string, string, bool> geoResponse = DoGeoTagSampling(storeSurvey.StoreID.Value);
                if (geoResponse.Item3)
                {
                    StoreMaster store = SmartDostDbContext.StoreMasters.FirstOrDefault(k => k.StoreID == storeSurvey.StoreID);
                    if (store != null)
                    {
                        store.Lattitude = geoResponse.Item1;
                        store.Longitude = geoResponse.Item2;
                        store.GeoTagCount = 3;
                        store.LastGeoTagDate = System.DateTime.Now;
                        store.GeoTagFileName = storeSurvey.PictureFileName;
                        store.ModifiedDate = System.DateTime.Now;
                        store.ModifiedBy = Convert.ToString(storeSurvey.UserID);
                        SmartDostDbContext.Entry<StoreMaster>(store).State = System.Data.EntityState.Modified;
                        SmartDostDbContext.SaveChanges();
                    }
                }
                 * */
                // }
                scope.Complete();
            }

            return status;
        }

        /// <summary>
        /// Method to save user activities on the basis of store survey data
        /// </summary>
        /// <param name="activities">activities performed</param>
        /// <returns>returns status</returns>
        public int SaveSurveyUserResponse(IList<SurveyUserResponse> activities, long userID, bool saveImage = true)
        {
            DateTime currentDateTime = DateTime.Now;
            int status = 0;
            int i = 1;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
    new TransactionOptions
    {
        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
    }))
            {
                foreach (SurveyUserResponse survey in activities)
                {
                    #region TemporararyImage Saving code
                    ////TemporararyImage Saving code
                    //if (saveImage)
                    //{
                    //    SurveyQuestion question = SmartDostDbContext.SurveyQuestions.FirstOrDefault(k => k.SurveyQuestionID == survey.SurveyQuestionID);
                    //    if (question != null)
                    //    {
                    //        if (question.QuestionTypeID == (int)AspectEnums.QuestionTypes.PictureBox)
                    //        {
                    //            try
                    //            {
                    //                var image = Convert.FromBase64String(survey.UserResponse);
                    //                Stream stream = new MemoryStream(image);
                    //                string previousUploadedFile = GetEntityImageName(survey.SurveyResponseID, survey.SurveyQuestionID, AspectEnums.ImageFileTypes.Survey);
                    //                string fileName = string.Empty;
                    //                string fileDirectory = GetUploadDirectory(AspectEnums.ImageFileTypes.Survey);
                    //                if (Directory.Exists(fileDirectory))
                    //                {
                    //                    if (!String.IsNullOrEmpty(previousUploadedFile))
                    //                    {
                    //                        fileName = fileDirectory + @"\" + previousUploadedFile;
                    //                        if (File.Exists(fileName))
                    //                        {
                    //                            File.Delete(fileName);
                    //                        }
                    //                    }
                    //                    FileStream fileData = null;
                    //                    string newFileName = survey.SurveyResponseID.ToString() + "_" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + "_" + i + ".jpeg";
                    //                    fileName = newFileName;
                    //                    newFileName = fileDirectory + @"\" + newFileName;
                    //                    using (fileData = new FileStream(newFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                    //                    {
                    //                        const int bufferLen = 4096;
                    //                        byte[] buffer = new byte[bufferLen];
                    //                        int count = 0;
                    //                        int totalBytes = 0;
                    //                        while ((count = stream.Read(buffer, 0, bufferLen)) > 0)
                    //                        {
                    //                            totalBytes += count;
                    //                            fileData.Write(buffer, 0, count);
                    //                        }
                    //                        //fileData.Close();
                    //                        stream.Close();
                    //                    }

                    //                    survey.UserResponse = fileName;
                    //                    i++;
                    //                }
                    //            }
                    //            catch (Exception ex)
                    //            {

                    //                LogTraceEngine.WriteLogWithCategory("Error while saving image for QuestionID = " + survey.SurveyQuestionID + "; SurveyResponseID =" + survey.SurveyResponseID + "; Error Message=" + ex.Message, AppVariables.AppLogTraceCategoryName.General);
                    //            }
                    //        }
                    //    }

                    //}
                    #endregion
                    SurveyUserResponse userResponse = SmartDostDbContext.SurveyUserResponses.FirstOrDefault(k => k.SurveyResponseID == survey.SurveyResponseID && k.SurveyQuestionID == survey.SurveyQuestionID);
                    if (userResponse == null)
                    {
                        survey.CreatedDate = currentDateTime;
                        foreach (var item in survey.SurveyRepeatResponses)
                        {
                            item.CreatedDate = currentDateTime;
                            item.CreatedBy = userID;
                        }
                        SmartDostDbContext.Entry<SurveyUserResponse>(survey).State = System.Data.EntityState.Added;

                    }
                    else
                    {
                        userResponse.ModifiedDate = currentDateTime;
                        userResponse.UserResponse = survey.UserResponse;
                        var surveyUserResponseID = userResponse.SurveyUserResponseID;
                        foreach (var item in userResponse.SurveyRepeatResponses.Where(x => x.IsDeleted == false))
                        {
                            //   userResponse.SurveyRepeatResponses.Remove(item);
                            item.SurveyUserResponseID = surveyUserResponseID;
                            item.ModifiedDate = currentDateTime;
                            item.ModifiedBy = userID;
                            item.IsDeleted = true;
                            //SmartDostDbContext.SurveyRepeatResponses.Remove(item);

                        }
                        SmartDostDbContext.Entry<SurveyUserResponse>(userResponse).State = System.Data.EntityState.Modified;
                        //SmartDostDbContext.SaveChanges();
                        //SmartDostDbContext.SaveChanges();
                        foreach (var item in survey.SurveyRepeatResponses)
                        {
                            item.SurveyUserResponseID = surveyUserResponseID;
                            item.CreatedDate = currentDateTime;
                            item.CreatedBy = userID;
                            SmartDostDbContext.SurveyRepeatResponses.Add(item);
                        }
                        // userResponse.SurveyRepeatResponses = survey.SurveyRepeatResponses;
                        //SmartDostDbContext.Entry<SurveyRepeatResponse>(userResponse).State = System.Data.EntityState.Added;

                    }
                    #region Image saving previous logic is commented
                    /*Code commented as images saving is moved to new service
                    SurveyQuestion question = SmartDostDbContext.SurveyQuestions.FirstOrDefault(k => k.SurveyQuestionID == survey.SurveyQuestionID);
                    if (question != null)
                    {
                        if (question.QuestionTypeID == (int)AspectEnums.QuestionTypes.Checkbox)
                        {
                            if (survey.UserResponse != null)
                            {
                                survey.UserResponse = survey.UserResponse.Replace('@', ',');//To replace @ with , for case of checkboxes
                            }

                        }
                       
                        //if (question.QuestionTypeID == (int)AspectEnums.QuestionTypes.PictureBox)
                        //{
                           
                            try
                            {

                                #region Store Image
                                var image = Convert.FromBase64String(survey.UserResponse);
                                Stream stream = new MemoryStream(image);
                                //Stream stream = img;
                                //AspectEnums.ImageFileTypes fileType = AppUtil.NumToEnum<AspectEnums.ImageFileTypes>(2);
                                string previousUploadedFile = GetEntityImageName(survey.SurveyResponseID, survey.SurveyQuestionID, AspectEnums.ImageFileTypes.Survey);
                                string fileName = string.Empty;
                                string fileDirectory = GetUploadDirectory(AspectEnums.ImageFileTypes.Survey);
                                if (Directory.Exists(fileDirectory))
                                {
                                    if (!String.IsNullOrEmpty(previousUploadedFile))
                                    {
                                        fileName = fileDirectory + @"\" + previousUploadedFile;
                                        if (File.Exists(fileName))
                                        {
                                            File.Delete(fileName);
                                        }
                                    }
                                    FileStream fileData = null;
                                    string newFileName = survey.SurveyResponseID.ToString() + "_" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + "_" + i + ".jpeg";
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
                                        fileData.Close();
                                        stream.Close();
                                    }
                                    //UpdateEntityImageName(survey.SurveyUserResponseID.ToString(), newFileName, AspectEnums.ImageFileTypes.Survey);
                                    survey.UserResponse = fileName;
                                    i++;
                                }
                                #endregion
                            }
                            catch { }
                             */
                    //  }
                    //if (question.QuestionTypeID != (int)AspectEnums.QuestionTypes.Checkbox)
                    //{
                    //else
                    //{


                    //else
                    //{
                    //    SurveyUserResponse userResponse = null;
                    //    if (!string.IsNullOrEmpty(survey.UserResponse))
                    //    {
                    //        string responses = survey.UserResponse.Replace("@", ",");
                    //        SurveyUserResponse response = new SurveyUserResponse()
                    //               {
                    //                   UserResponse = responses,
                    //                   SurveyQuestionID = survey.SurveyQuestionID,
                    //                   SurveyResponseID = survey.SurveyResponseID,
                    //                   SurveyTypeID = survey.SurveyTypeID,
                    //               };
                    //        //if (responses.Length > 0)
                    //        //{
                    //        //    foreach (string answer in responses)
                    //        //    {
                    //        //        SurveyUserResponse response = new SurveyUserResponse()
                    //        //        {
                    //        //            UserResponse = answer,
                    //        //            SurveyQuestionID = survey.SurveyQuestionID,
                    //        //            SurveyResponseID = survey.SurveyResponseID,
                    //        //            SurveyTypeID = survey.SurveyTypeID,
                    //        //        };
                    //        userResponse = SmartDostDbContext.SurveyUserResponses.FirstOrDefault(k => k.SurveyResponseID == survey.SurveyResponseID && k.SurveyQuestionID == survey.SurveyQuestionID);
                    //        if (userResponse == null)
                    //        {
                    //            response.CreatedDate = System.DateTime.Now;
                    //            SmartDostDbContext.SurveyUserResponses.Add(response);

                    //        }
                    //        else
                    //        {
                    //            userResponse.ModifiedDate = System.DateTime.Now;
                    //            userResponse.UserResponse = survey.UserResponse;
                    //            SmartDostDbContext.Entry<SurveyUserResponse>(userResponse).State = System.Data.EntityState.Modified;

                    //        }
                    //        //    }
                    //        //}
                    //    }
                    //    else
                    //    {
                    //        userResponse = SmartDostDbContext.SurveyUserResponses.FirstOrDefault(k => k.SurveyResponseID == survey.SurveyResponseID && k.SurveyQuestionID == survey.SurveyQuestionID && k.UserResponse == survey.UserResponse);
                    //        if (userResponse == null)
                    //        {
                    //            survey.CreatedDate = System.DateTime.Now;
                    //            SmartDostDbContext.SurveyUserResponses.Add(survey);

                    //        }
                    //        else
                    //        {
                    //            userResponse.ModifiedDate = System.DateTime.Now;
                    //            userResponse.UserResponse = survey.UserResponse;
                    //            SmartDostDbContext.Entry<SurveyUserResponse>(userResponse).State = System.Data.EntityState.Modified;

                    //        }
                    //    }


                    //}
                    //}
                    #endregion
                }
                status = SmartDostDbContext.SaveChanges();
                scope.Complete();
            }
            return status;
        }


        /// <summary>
        /// Method to save user activities on the basis of general survey data
        /// </summary>
        /// <param name="activities">activities performed</param>
        /// <returns>returns status</returns>
        public int SaveGeneralUserResponse(IList<GeneralUserResponse> activities, long userID, bool saveImage = true)
        {

            int status = 0;
            int i = 1;
            var currentDate = DateTime.Now.Date;
            var currentDateTime = DateTime.Now;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
    new TransactionOptions
    {
        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
    }))
            {
                foreach (GeneralUserResponse survey in activities)
                {
                    survey.SurveyDate = currentDate;

                    #region General Repeat Response By Vaishali on 25-May-2015

                    survey.CreatedDate = currentDateTime;
                    survey.SurveyDate = currentDate;
                    survey.UserID = userID;
                    survey.CreatedBy = (int)userID;

                    foreach (var item in survey.SurveyRepeatResponses)
                    {
                        item.CreatedDate = currentDateTime;
                        item.CreatedBy = userID;
                    }

                    SmartDostDbContext.GeneralUserResponses.Add(survey);

                    #endregion


                }
                status = SmartDostDbContext.SaveChanges();
                scope.Complete();
            }
            return status;
        }


        /// <summary>
        /// Method to get survey questions on the basis of user profile selected
        /// Change:- Amit - New Parameter userID added to get survey question based on user profile
        /// </summary>
        /// <param name="userRoleID">user profile ID</param>
        /// <returns>returns questions list</returns>
        public IList<vwSurveyQuestion> GetSurveyQuestions(long userRoleID, long userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;

            IList<vwSurveyQuestion> result = new List<vwSurveyQuestion>();
            if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.Question)).FirstOrDefault() == true)
            {
                UserRole userRole = SmartDostDbContext.UserRoles.FirstOrDefault(k => k.UserRoleID == userRoleID);
                if (userRole != null)
                {

                   // result = SmartDostDbContext.vwSurveyQuestions.Where(k => k.RoleID == userRole.RoleID && k.IsActive == true && k.UserID == userRole.UserID).OrderBy(k => k.Sequence).ThenBy(k => k.Question).ToList();


                    if (LastUpdatedDate == null)
                    {

                        result = SmartDostDbContext.vwSurveyQuestions.Where(k => k.UserID == userID && !k.IsDeleted)
                           .OrderBy(k => k.MaxModifiedDate)
                           .Skip(StartRowIndex)
                           .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       

                    }
                    else
                    {
                        result = SmartDostDbContext.vwSurveyQuestions.Where(k => k.UserID == userID 
                        &&
                        (
                        (k.MaxModifiedDate > LastUpdatedDate)
                        ||
                        (k.MaxModifiedDate == LastUpdatedDate.Value)
                        )
                        )
                        .OrderBy(k => k.MaxModifiedDate)
                        .Skip(StartRowIndex)
                        .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                   
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
                            MaxModifiedDate = result.Max(x => x.MaxModifiedDate);
                    }

                }
            }
            return result;
        }
        /// <summary>
        /// Method to get survey questions on the basis of user profile selected
        /// </summary>
        /// <param name="userRoleID">user profile ID</param>
        /// <returns>returns questions list</returns>
        public string GetEntityImageName(string entityID, AspectEnums.ImageFileTypes imageType)
        {
            string fileName = string.Empty;
            switch (imageType)
            {
                case AspectEnums.ImageFileTypes.Store:
                    int storeID = Convert.ToInt32(entityID);
                    var store = SmartDostDbContext.StoreMasters.FirstOrDefault(k => k.StoreID == storeID);
                    if (store != null)
                        fileName = store.PictureFileName;
                    break;
                case AspectEnums.ImageFileTypes.User:
                    long userID = Convert.ToInt64(entityID);
                    var user = SmartDostDbContext.UserMasters.FirstOrDefault(k => k.UserID == userID);
                    if (user != null)
                        fileName = user.ProfilePictureFileName;
                    break;
                case AspectEnums.ImageFileTypes.Survey:
                    long surveyID = Convert.ToInt64(entityID);
                    var survey = SmartDostDbContext.SurveyUserResponses.FirstOrDefault(k => k.SurveyUserResponseID == surveyID);
                    if (survey != null)
                        fileName = survey.UserResponse;
                    break;
                case AspectEnums.ImageFileTypes.Question:
                    long questionID = Convert.ToInt64(entityID);
                    var question = SmartDostDbContext.SurveyUserResponses.FirstOrDefault(k => k.SurveyQuestionID == questionID);
                    if (question != null)
                        fileName = question.UserResponse;
                    break;
                case AspectEnums.ImageFileTypes.General:
                    long surveyID1 = Convert.ToInt64(entityID);
                    var survey1 = SmartDostDbContext.GeneralUserResponses.FirstOrDefault(k => k.GeneralUserResponseID == surveyID1);
                    if (survey1 != null)
                        fileName = survey1.UserResponse;
                    break;
            }
            return fileName;
        }
        /// <summary>
        /// Method to get survey questions on the basis of user profile selected
        /// </summary>
        /// <param name="userRoleID">user profile ID</param>
        /// <returns>returns questions list</returns>
        public string GetEntityImageName(long entityID, int questionID, AspectEnums.ImageFileTypes imageType)
        {
            string fileName = string.Empty;
            switch (imageType)
            {
                case AspectEnums.ImageFileTypes.Store:
                    int storeID = Convert.ToInt32(entityID);
                    var store = SmartDostDbContext.StoreMasters.FirstOrDefault(k => k.StoreID == storeID);
                    if (store != null)
                        fileName = store.PictureFileName;
                    break;
                case AspectEnums.ImageFileTypes.User:
                    long userID = Convert.ToInt64(entityID);
                    var user = SmartDostDbContext.UserMasters.FirstOrDefault(k => k.UserID == userID);
                    if (user != null)
                        fileName = user.ProfilePictureFileName;
                    break;
                case AspectEnums.ImageFileTypes.Survey:
                    long surveyResponseID = entityID;
                    var survey = SmartDostDbContext.SurveyUserResponses.FirstOrDefault(k => k.SurveyResponseID == surveyResponseID && k.SurveyQuestionID == questionID);
                    if (survey != null)
                        fileName = survey.UserResponse;
                    break;
                case AspectEnums.ImageFileTypes.Question:

                    var question = SmartDostDbContext.SurveyUserResponses.FirstOrDefault(k => k.SurveyQuestionID == questionID && k.SurveyResponseID == questionID);
                    if (question != null)
                        fileName = question.UserResponse;
                    break;
                case AspectEnums.ImageFileTypes.General:
                    long userIDCurrent = entityID;

                    DateTime currentDate = DateTime.Now.Date;
                    var oldResponse = SmartDostDbContext.GeneralUserResponses.FirstOrDefault(k => k.UserID == userIDCurrent && k.SurveyDate == currentDate && k.SurveyQuestionID == questionID);
                    if (oldResponse != null)
                        fileName = oldResponse.UserResponse;
                    break;
            }
            return fileName;
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
                case AspectEnums.ImageFileTypes.Survey:
                    fileDirectory = fileDirectory + @"\SDImages";
                    break;
                case AspectEnums.ImageFileTypes.User:
                    fileDirectory = fileDirectory + @"\UserFiles";
                    break;
                case AspectEnums.ImageFileTypes.Store:
                    fileDirectory = fileDirectory + @"\StoreFiles";
                    break;
                case AspectEnums.ImageFileTypes.Question:
                    fileDirectory = fileDirectory + @"\QuestionFiles";
                    break;
                case AspectEnums.ImageFileTypes.General:
                    fileDirectory = fileDirectory + @"\GeneralFiles";
                    break;
            }
            return fileDirectory;
        }
        /// <summary>
        /// Method to get survey questions attributes
        /// </summary>
        /// <returns>returns questions attribute list</returns>
        /// 
        public IList<SurveyQuestionAttribute> GetSurveyQuestionAttributes()
        {
            return SmartDostDbContext.SurveyQuestionAttributes.Where(k => k.IsActive).OrderBy(k => k.Sequence).ThenBy(k => k.OptionValue).ToList();
        }

        /// <summary>
        /// Method to submit competition booked in survey
        /// </summary>
        /// <param name="competitions">competition booked</param>
        /// <returns>returns boolean response</returns>
        public long SubmitCompetitionBooked(IList<CompetitionSurvey> competitions)
        {
            long response = 0;
            DateTime CurrentTime = DateTime.Now;
            DateTime CurrentDate = DateTime.Now;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
    new TransactionOptions
    {
        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
    }))
            {
                string surveyResponseIDS = string.Empty;
                string userid = string.Empty;
                foreach (var item in competitions)
                {
                    CompetitionSurvey comp = SmartDostDbContext.CompetitionSurveys.FirstOrDefault(k => k.SurveyResponseID == item.SurveyResponseID && k.CompetitionType == item.CompetitionType && k.CompetitorID == item.CompetitorID && k.ProductGroupID == item.ProductGroupID && k.SurveyQuestionID == item.SurveyQuestionID);
                    if (comp == null)
                    {
                        surveyResponseIDS += item.SurveyResponseID + ",";
                        userid = item.UserID.ToString();
                        //item.CreatedDate = System.DateTime.Now;
                        item.CreatedDate = CurrentTime;
                        //item.SurveyDate = System.DateTime.Today;
                        item.SurveyDate = CurrentDate;
                        SmartDostDbContext.CompetitionSurveys.Add(item);
                    }
                    else
                    {
                        comp.ModifiedDate = CurrentTime;
                        comp.UserResponse = item.UserResponse;
                        SmartDostDbContext.Entry<CompetitionSurvey>(comp).State = System.Data.EntityState.Modified;
                    }
                }
                try
                {
                    SmartDostDbContext.SaveChanges();
                }
                catch (Exception ex)
                {
                    LogTraceEngine.WriteLogWithCategory("Error in competition with these survey responseids=" + surveyResponseIDS + " userid=" + userid, AppVariables.AppLogTraceCategoryName.DiskFiles);
                    LogTraceEngine.WriteTrace("Error in competition with these survey responseids=" + surveyResponseIDS + " userid=" + userid);

                    throw ex;
                }

                scope.Complete();
                response = 1;
            }


            return response;
        }

        /// <summary>
        /// Method to submit collection survey 
        /// </summary>
        /// <param name="collection">collection survey</param>
        /// <returns>returns collection survey response</returns>
        public long SubmitCollectionSurvey(IList<CollectionSurvey> collection)
        {
            int status = 0;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                       new TransactionOptions
                       {
                           IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                       }))
            {
                foreach (var item in collection)
                {
                    CollectionSurvey survey = SmartDostDbContext.CollectionSurveys.FirstOrDefault(k => k.SurveyResponseID == item.SurveyResponseID && k.PaymentModeID == item.PaymentModeID);
                    if (survey == null)
                    {
                        item.CreatedDate = System.DateTime.Now;
                        SmartDostDbContext.CollectionSurveys.Add(item);
                    }
                    else
                    {
                        survey.ModifiedDate = System.DateTime.Now;
                        survey.PaymentModeID = item.PaymentModeID;
                        survey.Amount = item.Amount;
                        survey.Comments = item.Comments;
                        survey.TransactionDate = item.TransactionDate;
                        survey.TransactionID = item.TransactionID;
                        SmartDostDbContext.Entry<CollectionSurvey>(survey).State = System.Data.EntityState.Modified;
                    }
                }
                SmartDostDbContext.SaveChanges();
                status = 1;
                scope.Complete();
            }
            return status;

        }

        /// <summary>
        /// Method to submit order booking
        /// </summary>
        /// <param name="orders">order survey collection</param>
        /// <returns>returns response</returns>
        public int SubmitOrderBooking(IList<OrderBookingSurvey> orders)
        {
            int status = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                        new TransactionOptions
                        {
                            IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                        }))
            {
                int index = 0;
                string orderNumber = string.Empty;
                foreach (var item in orders)
                {
                    //Jira ID : SDCE-2937 Changes done by Dhiraj
                    //OrderBookingSurvey orderBooking = SmartDostDbContext.OrderBookingSurveys.FirstOrDefault(k => k.SurveyResponseID == item.SurveyResponseID && k.ProductID == item.ProductID && k.OrderBookingType == item.OrderBookingType);
                    //if (orderBooking == null)
                    //{
                    if (String.IsNullOrEmpty(orderNumber))
                        orderNumber = GetOrderNumber(item.StoreID, item.UserID, item.ProductID.Value, index, item.OrderBookingType);
                    item.SyncStatus = 0;
                    item.CreatedDate = System.DateTime.Now;
                    item.CreatedBy = item.UserID;
                    item.OrderNo = orderNumber;
                    SmartDostDbContext.OrderBookingSurveys.Add(item);
                    //}
                    //else
                    //{
                    //    orderBooking.SyncStatus = 0;
                    //    orderBooking.ModifiedDate = System.DateTime.Now;
                    //    orderBooking.ModifiedBy = item.UserID;
                    //    orderBooking.Quantity = item.Quantity;
                    //    SmartDostDbContext.Entry<OrderBookingSurvey>(orderBooking).State = System.Data.EntityState.Modified;
                    //    item.OrderBookingID = orderBooking.OrderBookingID;
                    //    item.CreatedDate = orderBooking.CreatedDate;
                    //    item.OrderNo = orderBooking.OrderNo;
                    //}
                    index++;
                }
                SmartDostDbContext.SaveChanges();
                status = 1;
                scope.Complete();
            }
            return status;
        }

        /// <summary>
        /// Method to fetch competition group list from database
        /// Change:- Amit - New Parameter userID added to get survey question based on user profile (24 Sep 2014)
        /// </summary>
        /// <param name="companyID">company ID</param>
        /// <returns>returns product group list</returns>
        public IList<CompProductGroup> GetCompetitionProductGroup(int companyID, long? userID)
        {
         
            IList<CompProductGroup> result = new List<CompProductGroup>();
            if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.CompitionProductGroup)).FirstOrDefault() == true)
            {
                result = SmartDostDbContext.CompProductGroups.Where(k => k.CompanyID == companyID && k.IsActive && !k.IsDeleted).ToList();

            }
            return result;
        }

        /// <summary>
        /// Method to fetch competition group list from database for APK
        /// Change:- Amit - New Parameter userID added to get survey question based on user profile (24 Sep 2014)
        /// </summary>
        /// <param name="companyID">company ID</param>
        /// <returns>returns product group list</returns>
        public IList<CompProductGroup> GetCompetitionProductGroup(int companyID, long? userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;


            IList<CompProductGroup> result = new List<CompProductGroup>();
            if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.CompitionProductGroup)).FirstOrDefault() == true)
            {
                //result = SmartDostDbContext.CompProductGroups.Where(k => k.CompanyID == companyID && k.IsActive && !k.IsDeleted).ToList();

                if (LastUpdatedDate == null)
                {
                    result = SmartDostDbContext.CompProductGroups.Where(k => k.CompanyID == companyID && k.IsActive && !k.IsDeleted).ToList()
                       .OrderBy(k => k.CreatedDate)
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       
                }
                else
                {
                 
                    result = SmartDostDbContext.CompProductGroups.Where(k => k.CompanyID == companyID).ToList()
                    .Where(k =>
                        (((k.Modifieddate ??  k.CreatedDate) > LastUpdatedDate)
                        ||
                        ((k.Modifieddate ?? k.CreatedDate) == LastUpdatedDate.Value)
                        ))
                    .OrderBy(k => (k.Modifieddate ?? k.CreatedDate))
                    .Skip(StartRowIndex)
                    .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                   
                }

                HasMoreRows = result.Count > RowCount ? true : false;
                result = result.Take(RowCount).ToList();

                // Update last modified data among the data if available, else send the same modifieddate back  
                if (result.Count > 0)
                {
                    //MaxModifiedDate = dealer.Max(x => x.MaxModifiedDate);

                    if (LastUpdatedDate == null && HasMoreRows == true)
                        MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
                    //else if (LastUpdatedDate == null && HasMoreRows == false)
                    else if (HasMoreRows == false)
                        MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
                    else
                        MaxModifiedDate = result.Max(k => (k.Modifieddate ?? k.CreatedDate));
                }

            }
            return result;
        }

        /// <summary>
        /// Method to update order sync status
        /// </summary>
        /// <param name="orderID">order ID</param>
        /// <param name="syncStatus">sync status</param>
        /// <returns>returns boolean status</returns>
        public bool UpdateOrderSyncStatus(long orderID, int syncStatus)
        {
            OrderBookingSurvey order = SmartDostDbContext.OrderBookingSurveys.FirstOrDefault(k => k.OrderBookingID == orderID);
            if (order != null)
            {
                order.SyncStatus = syncStatus;
                order.ModifiedDate = System.DateTime.Now;
                SmartDostDbContext.Entry<OrderBookingSurvey>(order).State = System.Data.EntityState.Modified;
                return SmartDostDbContext.SaveChanges() > 0 ? true : false;
            }
            return false;
        }

        /// <summary>
        /// Method to submir partner meeting details
        /// </summary>
        /// <param name="partnerEntity">partner entity</param>
        /// <returns>returns boolean status</returns>
        public bool SubmitPartnerMeeting(PartnerMeeting partnerEntity)
        {
            DateTime currentDate = System.DateTime.Today;
            PartnerMeeting partner = SmartDostDbContext.PartnerMeetings.FirstOrDefault(k => k.MeetingDate == currentDate && k.ShipToCode == partnerEntity.ShipToCode);
            if (partner == null)
            {
                partnerEntity.CreatedDate = System.DateTime.Now;
                partnerEntity.CreatedBy = partnerEntity.UserID;
                partnerEntity.MeetingDate = currentDate;
                partnerEntity.ShipToCode = "";
                SmartDostDbContext.PartnerMeetings.Add(partnerEntity);
            }
            else
            {
                partner.ModifiedBy = partnerEntity.UserID;
                partner.ModifiedDate = System.DateTime.Now;
                partner.Remarks = partnerEntity.Remarks;
                SmartDostDbContext.Entry<PartnerMeeting>(partner).State = System.Data.EntityState.Modified;
            }
            return SmartDostDbContext.SaveChanges() > 0 ? true : false;

        }

        /// <summary>
        /// Method to get order number
        /// </summary>
        /// <param name="storeID">store ID</param>
        /// <param name="userID">user ID</param>
        /// <param name="productID">product ID</param>
        /// <param name="orderIndex">order index</param>
        /// <returns>returns order number</returns>
        private string GetOrderNumber(int storeID, long userID, int productID, int orderIndex, short orderBookingType)
        {
            StoreMaster store = SmartDostDbContext.StoreMasters.FirstOrDefault(k => k.StoreID == storeID);
            if (store != null)
            {
                string shipToCode = string.Empty;
                UserMaster userDetails = SmartDostDbContext.UserMasters.FirstOrDefault(k => k.UserID == userID);
                if (userDetails != null && !string.IsNullOrEmpty(userDetails.DistyCode))
                {
                    StoreParentMapping parentDetails = SmartDostDbContext.StoreParentMappings.FirstOrDefault(k => k.StoreID == storeID && k.ParentCode == userDetails.DistyCode && (k.IsDeleted == null || k.IsDeleted == false));
                    if (parentDetails != null)
                    {
                        shipToCode = parentDetails.ShipToCode;
                    }
                }
                int userOrderCount = SmartDostDbContext.OrderBookingSurveys.Count(k => k.UserID == userID && k.ProductID == productID && k.StoreID == storeID && k.OrderBookingType == orderBookingType);
                userOrderCount = userOrderCount + 1;
                return String.Format("{0}_{1}_{2}_{3}_{4}", shipToCode, store.StoreCode, System.DateTime.Now.Day, System.DateTime.Now.Month, userOrderCount);
            }
            return string.Empty;
        }

        /// <summary>
        /// Method to update user last visit in store
        /// </summary>
        /// <param name="userID">user id</param>
        /// <param name="storeID">store id</param>
        /// <returns>returns boolean status</returns>
        private bool UpdateStoreVisit(long userID, int storeID)
        {
            StoreUser storeUserDetail = SmartDostDbContext.StoreUsers.FirstOrDefault(k => k.StoreID == storeID && k.UserID == userID && !k.IsDeleted);
            if (storeUserDetail != null)
            {
                storeUserDetail.LastVisitDate = System.DateTime.Now;
                SmartDostDbContext.Entry<StoreUser>(storeUserDetail).State = System.Data.EntityState.Modified;
                return SmartDostDbContext.SaveChanges() > 0 ? true : false;
            }
            return false;
        }

        /// <summary>
        /// Method to perform geo tag sampling 
        /// </summary>
        /// <param name="storeID">store id for which sampling needs to do</param>
        /// <returns>returns probale co-ordinates if geo tag done</returns>
        /* Commented by Vaishali on 30 sep 2014 
        private Tuple<string, string, bool> DoGeoTagSampling(int storeID)
        {
            bool isGeoTagged = false;
            List<StoreGeoTag> geoTags = SmartDostDbContext.StoreGeoTags.Where(k => k.StoreID == storeID && !String.IsNullOrEmpty(k.Lattitude) && !String.IsNullOrEmpty(k.Longitude)).ToList();
            string lattitude = string.Empty;
            string longitude = string.Empty;
            if (geoTags.Count > 2)
            {
                double distance1 = AppUtil.GetDistanceTo(Convert.ToDouble(geoTags[0].Lattitude), Convert.ToDouble(geoTags[0].Longitude), Convert.ToDouble(geoTags[1].Lattitude), Convert.ToDouble(geoTags[1].Longitude));
                double distance2 = AppUtil.GetDistanceTo(Convert.ToDouble(geoTags[0].Lattitude), Convert.ToDouble(geoTags[0].Longitude), Convert.ToDouble(geoTags[2].Lattitude), Convert.ToDouble(geoTags[2].Longitude));
                double distance3 = AppUtil.GetDistanceTo(Convert.ToDouble(geoTags[2].Lattitude), Convert.ToDouble(geoTags[2].Longitude), Convert.ToDouble(geoTags[1].Lattitude), Convert.ToDouble(geoTags[1].Longitude));
                if (distance1 - distance2 < 100 && distance2 - distance1 < 100)
                {
                    lattitude = geoTags[0].Lattitude;
                    longitude = geoTags[0].Lattitude;
                    isGeoTagged = true;
                }
                else if (distance1 - distance3 < 100 && distance3 - distance1 < 100)
                {
                    lattitude = geoTags[2].Lattitude;
                    longitude = geoTags[2].Lattitude;
                    isGeoTagged = true;
                }
                if (distance2 - distance3 < 100 && distance3 - distance2 < 100)
                {
                    lattitude = geoTags[1].Lattitude;
                    longitude = geoTags[1].Lattitude;
                    isGeoTagged = true;
                }
            }
            return new Tuple<string, string, bool>(lattitude, longitude, isGeoTagged);
        }
         */

        /// <summary>
        /// Method to get partner meeting survey questions
        /// </summary>
        /// <param name="userRoleID">user role ID</param>
        /// <param name="userID">user ID</param>
        /// <returns>returns questions</returns>
        public IList<vwSurveyQuestion> GetSurveyPartnerQuestions(long userRoleID, long userID)
        {
            IList<vwSurveyQuestion> result = new List<vwSurveyQuestion>();
            result = SmartDostDbContext.vwSurveyQuestions.Where(k => k.RoleID == userRoleID && k.IsActive == true && k.UserID == userID && k.ModuleCode == (int)AspectEnums.AppModules.PartnerMeeting).OrderBy(k => k.Sequence).ToList();
            return result;
        }

        /// <summary>
        /// Method to get territory for selected user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IList<SPGetMyTerritory_Result> GetMyTerritory(long userID)
        {
            IList<SPGetMyTerritory_Result> result = new List<SPGetMyTerritory_Result>();
            result = SmartDostDbContext.SPGetMyTerritory(userID, null).ToList();
            return result;
        }

        /// <summary>
        /// Get Rule Book Data (Cobined data from 3 tables ApproverTypeMaster, ActivityMaster, ActivityApporverMaster)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public RuleBook GetRuleBook(long userID)
        {
            RuleBook ruleBookObj = new RuleBook();
            try
            {

                ruleBookObj.ApproverTypes = SmartDostDbContext.ApproverTypeMasters.Where(x => x.IsDeleted == false).ToList();
                ruleBookObj.ActivityMasters = SmartDostDbContext.ActivityMasters.ToList();
                ruleBookObj.ActivityApproverMasters = SmartDostDbContext.ActivityApproverMasters.ToList().Where(x => x.ApproverValue != null).ToList();

            }
            catch { throw; }
            return ruleBookObj;

        }

        #region Import Coverage Export Data

        /// <summary>
        /// Inserts the entities new.
        /// </summary>
        /// <param name="dt">The dt.</param>
        public void ImportCoverageExport(DataTable dt)
        {
            // // Copy the DataTable to SQL Server using SqlBulkCopy
            //// Microsoft.Practices.EnterpriseLibrary.Data.Database DataAccess = EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Data.Database>("SmartDostLog_DB_Connection") as SqlDatabase;
            // using (SqlConnection dbConnection = new SqlConnection(DataAccess.ConnectionString))
            // {
            //     dbConnection.Open();
            //     using (SqlBulkCopy s = new SqlBulkCopy(dbConnection))
            //     {
            //         s.DestinationTableName = "CoverageExport";

            //         foreach (var column in dt.Columns)
            //             s.ColumnMappings.Add(column.ToString(), column.ToString());

            //         s.WriteToServer(dt);
            //         dbConnection.Close();
            //     }
            // }

        }

        #endregion

        #region SDCE-638 Added by Niranjan (Product Group Category In Question Master) 15-10-2014
        /// <summary>
        /// Method to bind Product Group Category  
        /// Product Group Category In Question Master
        /// </summary>
        /// <param name="companyID">company ID</param>
        /// <returns>returns product group list</returns>
        public IList<ProductGroupCategory> GetProductGroupCategoryList()
        {
            IList<ProductGroupCategory> result = new List<ProductGroupCategory>();
            result = SmartDostDbContext.ProductGroupCategories.Where(s => s.IsDeleted == false).ToList();
            return result;
        }
        #endregion

        public bool SaveQuestionImages(long userID, int roleID, int storeID, string Image)
        //public bool SaveQuestionImages(Stream stream)
        {
            //using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
            //    new TransactionOptions
            //    {
            //        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
            //    }))
            //{

            #region Store Image
            var image = Convert.FromBase64String(Image);
            string fileName = string.Empty;
            Stream stream = new MemoryStream(image);
            string fileDirectory = GetUploadDirectory(AspectEnums.ImageFileTypes.FMS);
            if (Directory.Exists(fileDirectory))
            {
                FileStream fileData = null;
                string newFileName = DateTime.Today.Date.ToString() + "_" + DateTime.Now.Ticks + "_" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + ".jpeg";
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
                    // fileData.Close();
                    stream.Close();
                }
                //}
            #endregion

                //#region Store Image
                //Stream stream = new MemoryStream(Image);
                //string fileName = string.Empty;
                //string fileDirectory = GetUploadDirectory(AspectEnums.ImageFileTypes.FMS);
                //if (Directory.Exists(fileDirectory))
                //{
                //    FileStream fileData = null;
                //    string newFileName = DateTime.Today.Date.ToString() + "_" + DateTime.Now.Ticks + "_" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + ".jpeg";
                //    fileName = newFileName;
                //    newFileName = fileDirectory + @"\" + newFileName;
                //    using (fileData = new FileStream(newFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                //    {
                //        const int bufferLen = 4096;
                //        byte[] buffer = new byte[bufferLen];
                //        int count = 0;
                //        int totalBytes = 0;
                //        while ((count = stream.Read(buffer, 0, bufferLen)) > 0)
                //        {
                //            totalBytes += count;
                //            fileData.Write(buffer, 0, count);
                //        }
                //        fileData.Close();
                //        stream.Close();
                //    }
                //}
                //#endregion
            }
            return true;
        }


        #region RACE
        public List<RaceBrandMaster> GetRaceBrandMaster()
        {
            return SmartDostDbContext.RaceBrandMasters.Where(k => k.IsDeleted == false).ToList();
        }


        public List<RaceBrandCategoryMapping> GetRaceBrandCategoryMapping()
        {

            return SmartDostDbContext.RaceBrandCategoryMappings.Where(k => k.IsDeleted == false).ToList();
        }

        public List<RaceProductCategory> GetRaceProductCategory()
        {
            return SmartDostDbContext.RaceProductCategories.Where(k => k.IsDeleted == false).ToList();
        }

        public List<RacePOSMMaster> GetRacePOSMMaster()
        {
            return SmartDostDbContext.RacePOSMMasters.ToList();
        }


        public List<RaceFixtureCategoryMaster> GetRaceFixtureMaster()
        {
            List<RaceFixtureMaster> fixture = new List<RaceFixtureMaster>();
            List<RaceFixtureCategoryMaster> result = new List<RaceFixtureCategoryMaster>();
            fixture = SmartDostDbContext.RaceFixtureMasters.Where(x => x.IsDeleted == false).ToList();

            foreach (var item in fixture)
            {
                #region Append Categories seperated by comma
                List<RaceProductCategory> category = new List<RaceProductCategory>();
                foreach (var subitem in item.RaceFixtureCategoryMappings)
                {
                    category.Add(SmartDostDbContext.RaceProductCategories.Where(x => x.CompProductGroupID == subitem.CompProductGroupID).FirstOrDefault());
                }
                #endregion


                result.Add(new RaceFixtureCategoryMaster()
                {
                    Category = item.Category,
                    FixtureID = item.FixtureID,
                    IsColumnAvailable = item.IsColumnAvailable,
                    IsCompetitorAvailable = item.IsCompetitorAvailable,
                    IsRowAvailable = item.IsRowAvailable,
                    SubCategory = item.SubCategory,
                    CategoryGroups = category.Count > 0 ? String.Join(",", category.Select(x => x.ProductGroupCode).ToArray()) : ""
                });
            }
            return result;
        }

        public List<RacePOSMProductMapping> GetRacePOSMProductMappings()
        {

            return SmartDostDbContext.RacePOSMProductMappings.Where(k => k.IsDeleted == false).ToList();
        }

        //public List<RaceProductMaster> GetRaceProductMaster(int LastProductID, int rowcounter, out bool HasMoreRows)
        //{

        //    HasMoreRows = false;
        //    List<RaceProductMaster> result = new List<RaceProductMaster>();

        //    result = SmartDostDbContext.RaceProductMasters.Where(k => k.ProductID > LastProductID && k.IsDeleted == false).Take(rowcounter).ToList();

        //    if (result.Count > 0)
        //    {
        //        int MaxID = SmartDostDbContext.RaceProductMasters.Where(k => k.IsDeleted == false).Max(k => k.ProductID);
        //        HasMoreRows = result.Max(k => k.ProductID) < MaxID;
        //    }
        //    return result;
        //}

        public List<RaceProductMaster> GetRaceProductMaster(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;

            //DateTime dt = System.DateTime.Now;
            //var test = SmartDostDbContext.RaceProductMasters.Where(x => x.IsDeleted == false &&
            //dt == (x.ModifiedDate == null ? x.CreatedDate : x.ModifiedDate)).ToList();

            List<RaceProductMaster> result = new List<RaceProductMaster>();
           

                if (LastUpdatedDate == null)
                {
                    result = SmartDostDbContext.RaceProductMasters.Where(k => k.IsDeleted == false).ToList().ToList()
                       .OrderBy(k => k.ModifiedDate ?? k.CreatedDate)
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       
                }
                else
                {
                    result = SmartDostDbContext.RaceProductMasters
                   .Where(k =>
                       ((LastUpdatedDate == (k.ModifiedDate == null ? k.CreatedDate : k.ModifiedDate))
                       ||
                       (LastUpdatedDate < (k.ModifiedDate == null ? k.CreatedDate : k.ModifiedDate))
                       ))
                   .OrderBy(k => k.ModifiedDate ?? k.CreatedDate)
                   .Skip(StartRowIndex)
                   .Take(RowCount + 1).ToList();                                     
                }

                HasMoreRows = result.Count > RowCount ? true : false;
                result = result.Take(RowCount).ToList();

                // Update last modified data among the data if available, else send the same modifieddate back  
                if (result.Count > 0)
                {
                    //MaxModifiedDate = dealer.Max(x => x.MaxModifiedDate);

                    if (LastUpdatedDate == null && HasMoreRows == true)
                        MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
                    //else if (LastUpdatedDate == null && HasMoreRows == false)
                    else if (HasMoreRows == false)
                        MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
                    else
                        MaxModifiedDate = result.Max(k => k.ModifiedDate ?? k.CreatedDate);
                }           
            return result;           
        }

        public bool SubmitAuditResponse(long userID, int roleID, long SurveyResponseID, StockAudit auditResponse)
        {
            bool IsSuccess = false;
            DateTime CurrentDate = System.DateTime.Now;
            int auditID = SmartDostDbContext.AuditSummaries.Where(x => x.SurveyResponseID == SurveyResponseID).FirstOrDefault().AuditID;

            //var auditID = SmartDostDbContext.AuditSummaries.Where(x => x.SurveyResponseID == SurveyResponseID).FirstOrDefault().AuditID; 

            if (auditID > 0)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                       new TransactionOptions
                       {
                           IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                       }))
                {
                    #region update delete flag as true for existing products

                    var result = SmartDostDbContext.StockAuditResponses.Where(x => x.AuditID == auditID).ToList();

                    foreach (var item in result)
                    {
                        item.IsDeleted = true;
                        foreach (var subitem in item.StockAuditPOSMResponses)
                        {
                            subitem.IsDeleted = true;
                        }
                        SmartDostDbContext.Entry<StockAuditResponse>(item).State = System.Data.EntityState.Modified;
                    }
                    #endregion

                    #region save audit response
                    foreach (var item in auditResponse.StockAuditSummary)
                    {

                        foreach (var subitem in item.StockAuditPOSMResponse)
                        {
                            subitem.CreatedBy = userID;
                            subitem.CreatedDate = CurrentDate;
                            subitem.ProductID = item.ProductID;

                        }
                        StockAuditResponse newStockAuditResponse = new StockAuditResponse()
                            {
                                AuditID = auditID,
                                BrandID = item.BrandID,
                                CreatedBy = userID,
                                CreatedDate = CurrentDate,
                                FixtureID = item.FixtureID,
                                IsDeleted = false,
                                PriceTag = item.PriceTag,
                                ProductID = item.ProductID,
                                RowNumber = item.RowNumber,
                                SwitchedOn = item.SwitchedOn,
                                Topper = item.Topper,
                                WallNumber = item.WallNumber,
                                StockAuditPOSMResponses = item.StockAuditPOSMResponse
                            };

                        SmartDostDbContext.StockAuditResponses.Add(newStockAuditResponse);

                    }
                    #endregion

                    IsSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;
                    scope.Complete();
                }
            }
            return IsSuccess;
        }

        #endregion

        #region 14th August Manoranjan
        /// <summary>
        /// GetParentDistributer
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public IList<spGetParentDistributer_Result> GetParentDistributer(long UserID, int RoleID)
        {
            //var Result = SmartDostDbContext.RoleMasters.FirstOrDefault(a => a.RoleID == @RoleID);
            //int? team = 0;
            //if (Result != null)
            //{
            //    team = Result.TeamID ?? 0;
            //}
            List<spGetParentDistributer_Result> lst = new List<spGetParentDistributer_Result>();

            var sublst = SmartDostDbContext.spGetParentDistributer(Convert.ToInt32(UserID)).ToList();

            //foreach (var item in sublst)
            //{
            //    lst.Add(new spGetSrdDetails_Result { EmplCode = item.EmplCode , Username= item.Username  });
            //}

            return sublst;
        }
        #endregion


        #region RACE services for Sync Adaptor
        public IList<RacePOSMMaster> GetRACEPOSMMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;

            IList<RacePOSMMaster> result = new List<RacePOSMMaster>();

                        
            if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.RACE)).FirstOrDefault() == true)
            {
                if (LastUpdatedDate == null)
                {
                    result = SmartDostDbContext.RacePOSMMasters.Where(k => !k.IsDeleted)
                       .OrderBy(k => (k.CreateDate))
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       

                }
                else
                {

                    result = SmartDostDbContext.RacePOSMMasters.Where(k => (
                            (LastUpdatedDate < (k.CreateDate))
                            ||
                            (LastUpdatedDate.Value == (k.CreateDate))
                            ))
                       .OrderBy(k => (k.CreateDate))
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
                        MaxModifiedDate = result.Max(k => (k.CreateDate));

                }

            }
            
            return result;
        }


        public IList<RaceFixtureCategoryMaster> GetRACEFixtureMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;

            //IList<RaceFixtureMaster> result = new List<RaceFixtureMaster>();


            List<RaceFixtureMaster> fixture = new List<RaceFixtureMaster>();
            List<RaceFixtureCategoryMaster> result = new List<RaceFixtureCategoryMaster>();
           // fixture = SmartDostDbContext.RaceFixtureMasters.Where(x => x.IsDeleted == false).ToList();
            
            if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.RACE)).FirstOrDefault() == true)
            {
                if (LastUpdatedDate == null)
                {
                    fixture = SmartDostDbContext.RaceFixtureMasters.Where(k => !k.IsDeleted)
                       .OrderBy(k => (k.CreatedDate))
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       

                }
                else
                {

                    fixture = SmartDostDbContext.RaceFixtureMasters.Where(k => (
                            (LastUpdatedDate < (k.CreatedDate))
                            ||
                            (LastUpdatedDate.Value == (k.CreatedDate))
                            ))
                       .OrderBy(k => (k.CreatedDate))
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
                        MaxModifiedDate = result.Max(k => (k.CreatedDate));

                }

            }

            foreach (var item in fixture)
            {
                #region Append Categories seperated by comma
                List<RaceProductCategory> category = new List<RaceProductCategory>();
                foreach (var subitem in item.RaceFixtureCategoryMappings)
                {
                    category.Add(SmartDostDbContext.RaceProductCategories.Where(x => x.CompProductGroupID == subitem.CompProductGroupID).FirstOrDefault());
                }
                #endregion


                result.Add(new RaceFixtureCategoryMaster()
                {
                    Category = item.Category,
                    FixtureID = item.FixtureID,
                    IsColumnAvailable = item.IsColumnAvailable,
                    IsCompetitorAvailable = item.IsCompetitorAvailable,
                    IsRowAvailable = item.IsRowAvailable,
                    SubCategory = item.SubCategory,
                    CategoryGroups = category.Count > 0 ? String.Join(",", category.Select(x => x.ProductGroupCode).ToArray()) : ""
                });
            }


            return result;
        }



        public IList<RacePOSMProductMapping> GetRACEPOSMProductMapping(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;

            IList<RacePOSMProductMapping> result = new List<RacePOSMProductMapping>();


         


            if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.RACE)).FirstOrDefault() == true)
            {
                if (LastUpdatedDate == null)
                {
                    result = SmartDostDbContext.RacePOSMProductMappings.Where(k => !k.IsDeleted)
                       .OrderBy(k => (k.ModifiedDate ?? k.CreatedDate))
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       

                }
                else
                {

                    result = SmartDostDbContext.RacePOSMProductMappings.Where(k => (
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


        #endregion

        #region RaceMaster
        /// <summary>
        /// Added by Prashant 18 Nov 2015
        /// </summary>   
       public List<RaceBrandMaster> GetRaceBrandMaster(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;

            //DateTime dt = System.DateTime.Now;
            //var test = SmartDostDbContext.RaceProductMasters.Where(x => x.IsDeleted == false &&
            //dt == (x.ModifiedDate == null ? x.CreatedDate : x.ModifiedDate)).ToList();

            List<RaceBrandMaster> result = new List<RaceBrandMaster>();
           

                if (LastUpdatedDate == null)
                {
                    result = SmartDostDbContext.RaceBrandMasters.Where(k => k.IsDeleted == false).ToList().ToList()
                       .OrderBy(k => k.CreatedDate)
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       
                }
                else
                {
                    result = SmartDostDbContext.RaceBrandMasters.ToList()
                   .Where(k =>
                       ((LastUpdatedDate == (k.ModifiedDate == null ? k.CreatedDate : k.ModifiedDate))
                       ||
                       (LastUpdatedDate < (k.ModifiedDate == null ? k.CreatedDate : k.ModifiedDate))
                       ))
                   .OrderBy(k => k.ModifiedDate ?? k.CreatedDate)
                   .Skip(StartRowIndex)
                   .Take(RowCount + 1).ToList();                                  
                }

                HasMoreRows = result.Count > RowCount ? true : false;
                result = result.Take(RowCount).ToList();

                // Update last modified data among the data if available, else send the same modifieddate back  
                if (result.Count > 0)
                {
                    //MaxModifiedDate = dealer.Max(x => x.MaxModifiedDate);

                    if (LastUpdatedDate == null && HasMoreRows == true)
                        MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
                    //else if (LastUpdatedDate == null && HasMoreRows == false)
                    else if (HasMoreRows == false)
                        MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
                    else
                        MaxModifiedDate = result.Max(k => k.ModifiedDate ?? k.CreatedDate);
                }           
            return result;           
        }

       public List<RaceProductCategory> GetRaceProductCategory(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
       {
           HasMoreRows = false;
           MaxModifiedDate = LastUpdatedDate;
           DateTime CurrentDateTime = System.DateTime.Now;
                     
           List<RaceProductCategory> result = new List<RaceProductCategory>();
           if (LastUpdatedDate == null)
           {
               result = SmartDostDbContext.RaceProductCategories.Where(k => k.IsDeleted == false).ToList().ToList()
                  .OrderBy(k => k.ModifiedDate ?? k.CreatedDate)
                  .Skip(StartRowIndex)
                  .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       
           }
           else
           {
               result = SmartDostDbContext.RaceProductCategories.ToList()
               .Where(k =>
                       ((LastUpdatedDate == (k.ModifiedDate == null ? k.CreatedDate : k.ModifiedDate))
                       ||
                       (LastUpdatedDate < (k.ModifiedDate == null ? k.CreatedDate : k.ModifiedDate))
                       ))
                   .OrderBy(k => k.ModifiedDate ?? k.CreatedDate)
                   .Skip(StartRowIndex)
                   .Take(RowCount + 1).ToList(); 
           }

           HasMoreRows = result.Count > RowCount ? true : false;
           result = result.Take(RowCount).ToList();

           // Update last modified data among the data if available, else send the same modifieddate back  
           if (result.Count > 0)
           {
               //MaxModifiedDate = dealer.Max(x => x.MaxModifiedDate);

               if (LastUpdatedDate == null && HasMoreRows == true)
                   MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
               //else if (LastUpdatedDate == null && HasMoreRows == false)
               else if (HasMoreRows == false)
                   MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
               else
                   MaxModifiedDate = result.Max(k => k.ModifiedDate ?? k.CreatedDate);
           }
           return result;
       }

       public List<RaceBrandCategoryMapping> GetRaceBrandCategoryMapping(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
       {
           HasMoreRows = false;
           MaxModifiedDate = LastUpdatedDate;
           DateTime CurrentDateTime = System.DateTime.Now;
           List<RaceBrandCategoryMapping> result = new List<RaceBrandCategoryMapping>();

           if (LastUpdatedDate == null)
           {
               result = SmartDostDbContext.RaceBrandCategoryMappings.Where(k => k.IsDeleted == false).ToList().ToList()
                  .OrderBy(k => k.ModifiedDate ?? k.CreatedDate)
                  .Skip(StartRowIndex)
                  .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       
           }
           else
           {
               result = SmartDostDbContext.RaceBrandCategoryMappings.ToList()
               .Where(k =>
                       ((LastUpdatedDate == (k.ModifiedDate == null ? k.CreatedDate : k.ModifiedDate))
                       ||
                       (LastUpdatedDate < (k.ModifiedDate == null ? k.CreatedDate : k.ModifiedDate))
                       ))
                   .OrderBy(k => k.ModifiedDate ?? k.CreatedDate)
                   .Skip(StartRowIndex)
                   .Take(RowCount + 1).ToList(); 
           }

           HasMoreRows = result.Count > RowCount ? true : false;
           result = result.Take(RowCount).ToList();

           // Update last modified data among the data if available, else send the same modifieddate back  
           if (result.Count > 0)
           {
               //MaxModifiedDate = dealer.Max(x => x.MaxModifiedDate);

               if (LastUpdatedDate == null && HasMoreRows == true)
                   MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
               //else if (LastUpdatedDate == null && HasMoreRows == false)
               else if (HasMoreRows == false)
                   MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
               else
                   MaxModifiedDate = result.Max(k => k.ModifiedDate ?? k.CreatedDate);
           }
           return result;
       }
        #endregion
    }
}

