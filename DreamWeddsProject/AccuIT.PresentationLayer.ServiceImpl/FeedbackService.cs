using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.CommonLayer.Aspects.Exceptions;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using Samsung.SmartDost.PresentationLayer.ServiceImpl.Security;

namespace Samsung.SmartDost.PresentationLayer.ServiceImpl
{
    public partial class SmartDost : BaseService
    {
        #region FMS
        /// <summary>
        /// List of Feedbacks for creation
        /// </summary>
        /// <param name="FeedBacks"></param>
        /// <returns>true or false</returns>
        [UserSecureOperation]
        public JsonResponse<bool> SubmitFeedbacks(List<SubmitFeedbacksDTO> FeedBacks, long userID)
        {

            JsonResponse<bool> response = new JsonResponse<bool>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.SingleResult = FeedbackBusinessInstance.SubmitFeedbacks(FeedBacks,  userID);
                    response.IsSuccess = true;

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }


        [UserSecureOperation]
        public JsonResponse<FeedbackSearchResultOutputDTO> SearchFeedbacks(FeedbackSearchDTO searchFeedBacks, int storeID, long userID)
        {

            JsonResponse<FeedbackSearchResultOutputDTO> response = new JsonResponse<FeedbackSearchResultOutputDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    bool HasMoreRows = false;
                    FeedbackSearchResultOutputDTO output = new FeedbackSearchResultOutputDTO();
                    output.FeedbacksearchList = FeedbackBusinessInstance.SearchFeedbacks(searchFeedBacks, storeID, userID, out HasMoreRows);

                    FeedbackCountSearchDTO inputStatuCount = new FeedbackCountSearchDTO()
                    {
                        FeedbackCatIDs = searchFeedBacks.FeedbackCatIDs,
                        FeedbackTeamIDs = searchFeedBacks.FeedbackTeamIDs,
                        FeedbackTypeIDs = searchFeedBacks.FeedbackTypeIDs
                    };
                    List <SearchFeedbackStatusCountDTO> FeedbackStatusCountList = FeedbackBusinessInstance.SearchFeedbackStatusCount(inputStatuCount, userID, 0);
                    StringBuilder data=new StringBuilder ();;
                   FeedbackStatusCountList.ForEach(k=>data.Append(k.FeedbackStatusName.Trim()+" ("+k.FeedbackCount+"),"));
                    //string ss= string.Join("," FeedbackStatusCountList.Select(k=>new { Status=k.FeedbackStatusName+" ("+k.FeedbackCount+")"}).ToArray() );
                   // output.SearchFeedbackStatusCountList = FeedbackBusinessInstance.SearchFeedbackStatusCount(inputStatuCount, userID, 0);
                    output.StatusCountStr = data.ToString().TrimEnd(',');
                    output.HasMoreRows = HasMoreRows;
                    response.SingleResult = output;

                    response.IsSuccess = true;

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        [UserSecureOperation]
        public JsonResponse<bool> UpdateFeedbacks(List<UpdateFeedbackDTO> Feedback, long userID, int roleID)
        {

            JsonResponse<bool> response = new JsonResponse<bool>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    FeedbackBusinessInstance.UpdateFeedbacks(Feedback, userID, roleID);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }



        [UserSecureOperation]
        public JsonResponse<FMSMastersDTO> GetFMSMasters(long userID, int roleID, int LastTeamID, int LastCategoryID, int LastTypeID, int rowcounter)
        {
            JsonResponse<FMSMastersDTO> response = new JsonResponse<FMSMastersDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.SingleResult = FeedbackBusinessInstance.GetFMSMasters(userID, roleID, LastTeamID, LastCategoryID, LastTypeID, rowcounter);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }


        [UserSecureOperation]
        public JsonResponse<FeedbackDetailDTO> GetFeedbackDetails(long userID, int roleID, int FeedbackID)
        {
            JsonResponse<FeedbackDetailDTO> response = new JsonResponse<FeedbackDetailDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.SingleResult = FeedbackBusinessInstance.GetFeedbackDetails(FeedbackID);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        #region Notifications Screen in APK (Added by Amit)
        [UserSecureOperation]
        public JsonResponse<NotificationServiceLogDTO> GetNotifications(long userID, int roleID, byte NotificationType, long LastNotificationServiceID, int RowCounter)
        {
            JsonResponse<NotificationServiceLogDTO> response = new JsonResponse<NotificationServiceLogDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    //List<NotificationServiceLogDTO> objResult = new List<NotificationServiceLogDTO>()
                    //{
                    //    new NotificationServiceLogDTO()
                    //    { 
                    //        NotificationDate="11/12/2014 14:50:00", NotificationServiceID=12, NotificationType=4, PushNotificationMessage="test subject~sample notification", ReadStatus=0
                    //        //dd/MM/yyyy HH:mm:ss
                    //    },
                    //    new NotificationServiceLogDTO()
                    //    { 
                    //        NotificationDate="09/12/2014 14:55:00", NotificationServiceID=13, NotificationType=3, PushNotificationMessage="test subject2~sample notification", ReadStatus=0
                    //    },
                    //     new NotificationServiceLogDTO()
                    //    { 
                    //        NotificationDate="10/12/2014 10:50:00", NotificationServiceID=14, NotificationType=1, PushNotificationMessage="test subject2~sample notification", ReadStatus=1
                    //    },
                    //     new NotificationServiceLogDTO()
                    //    { 
                    //        NotificationDate="08/12/2014 18:59:00", NotificationServiceID=19, NotificationType=2, PushNotificationMessage="test subject2~sample notification", ReadStatus=0
                    //    }
                    //};
                    response.Result = FeedbackBusinessInstance.GetNotifications(userID, roleID, NotificationType, LastNotificationServiceID, RowCounter);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        [UserSecureOperation]
        public JsonResponse<bool> UpdateNotificationStatus(long userID, int roleID, List<UpdateNotificationStatusDTO> Notifications)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.SingleResult = FeedbackBusinessInstance.UpdateNotificationStatus(userID, roleID, Notifications);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        #region NotificationTypeMaster
        [UserSecureOperation]
        public JsonResponse<NotificationTypeMasterDTO> NotificationTypeMaster(long userID, int roleID)
        {
            JsonResponse<NotificationTypeMasterDTO> response = new JsonResponse<NotificationTypeMasterDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.Result = FeedbackBusinessInstance.NotificationTypeMaster(userID, roleID);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        #region SearchFeedbackStatusCount
        [UserSecureOperation]
        public JsonResponse<SearchFeedbackStatusCountDTO> SearchFeedbackStatusCount(FeedbackCountSearchDTO feedbackCountSearch, long? userID, int roleID)
        {
            JsonResponse<SearchFeedbackStatusCountDTO> response = new JsonResponse<SearchFeedbackStatusCountDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.Result = FeedbackBusinessInstance.SearchFeedbackStatusCount(feedbackCountSearch, userID, roleID);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        #region MSS services for sync adaptor
      
        [UserSecureOperation]
        public JsonResponse<SyncFeedbackTypeMasterDTO> GetMSSTypeMaster(long userID, int roleID, int RowCount, int StartRowIndex, string LastUpdatedDate)
        {
            JsonResponse<SyncFeedbackTypeMasterDTO> response = new JsonResponse<SyncFeedbackTypeMasterDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    bool HasMoreRows = false;
                    DateTime? MaxModifiedDateTime;
                    DateTime? LastUpdatedDateTime = null;

                    if (LastUpdatedDate != null)
                    {
                        LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }

                    SyncFeedbackTypeMasterDTO output = new SyncFeedbackTypeMasterDTO();
                    //output.Result = SystemBusinessInstance.GetProductList(companyID, userID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
                    output.Result = FeedbackBusinessInstance.GetMSSTypeMaster(userID,roleID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
                    output.HasMoreRows = HasMoreRows;

                    #region convert date in dd/MM/yyyy format
                    if (MaxModifiedDateTime != null)
                        output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    else
                        output.MaxModifiedDate = null;

                    #endregion

                    response.SingleResult = output;
                    response.IsSuccess = true;


                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }


        [UserSecureOperation]
        public JsonResponse<SyncTeamMasterDTO> GetMSSTeamMaster(long userID, int roleID, int RowCount, int StartRowIndex, string LastUpdatedDate)
        {
            JsonResponse<SyncTeamMasterDTO> response = new JsonResponse<SyncTeamMasterDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    bool HasMoreRows = false;
                    DateTime? MaxModifiedDateTime=null;
                    DateTime? LastUpdatedDateTime = null;

                    if (LastUpdatedDate != null)
                    {
                        LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }

                    SyncTeamMasterDTO output = new SyncTeamMasterDTO();
                    output.Result = FeedbackBusinessInstance.GetMSSTeamMaster(userID, roleID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
                    output.HasMoreRows = HasMoreRows;

                    #region convert date in dd/MM/yyyy format
                    if (MaxModifiedDateTime != null)
                        output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    else
                        output.MaxModifiedDate = null;

                    #endregion

                    response.SingleResult = output;
                    response.IsSuccess = true;


                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }


        [UserSecureOperation]
        public JsonResponse<SyncFeedbackCategoryMasterDTO> GetMSSCategoryMaster(long userID, int roleID, int RowCount, int StartRowIndex, string LastUpdatedDate)
        {
            JsonResponse<SyncFeedbackCategoryMasterDTO> response = new JsonResponse<SyncFeedbackCategoryMasterDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    bool HasMoreRows = false;
                    DateTime? MaxModifiedDateTime = null;
                    DateTime? LastUpdatedDateTime = null;

                    if (LastUpdatedDate != null)
                    {
                        LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }

                    SyncFeedbackCategoryMasterDTO output = new SyncFeedbackCategoryMasterDTO();
                    output.Result = FeedbackBusinessInstance.GetMSSCategoryMaster(userID, roleID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
                    output.HasMoreRows = HasMoreRows;

                    #region convert date in dd/MM/yyyy format
                    if (MaxModifiedDateTime != null)
                        output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    else
                        output.MaxModifiedDate = null;

                    #endregion

                    response.SingleResult = output;
                    response.IsSuccess = true;


                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        [UserSecureOperation]
        public JsonResponse<SyncFeedbackStatusMasterDTO> GetMSSStatusMaster(long userID, int roleID, int RowCount, int StartRowIndex, string LastUpdatedDate)
        {
            JsonResponse<SyncFeedbackStatusMasterDTO> response = new JsonResponse<SyncFeedbackStatusMasterDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    bool HasMoreRows = false;
                    DateTime? MaxModifiedDateTime = null;
                    DateTime? LastUpdatedDateTime = null;

                    if (LastUpdatedDate != null)
                    {
                        LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }

                    SyncFeedbackStatusMasterDTO output = new SyncFeedbackStatusMasterDTO();
                    output.Result = FeedbackBusinessInstance.GetMSSStatusMaster(userID, roleID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
                    output.HasMoreRows = HasMoreRows;

                    #region convert date in dd/MM/yyyy format
                    if (MaxModifiedDateTime != null)
                        output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    else
                        output.MaxModifiedDate = null;

                    #endregion

                    response.SingleResult = output;
                    response.IsSuccess = true;


                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }



        #endregion
    }
}
