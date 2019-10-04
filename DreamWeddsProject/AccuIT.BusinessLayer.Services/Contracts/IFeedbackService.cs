using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.BusinessLayer.Services.BO;


namespace Samsung.SmartDost.BusinessLayer.Services.Contracts
{
    public interface IFeedbackService
    {
        /// <summary>
        /// List of Feedbacks for creation
        /// </summary>
        /// <param name="FeedBacks"></param>
        /// <returns>true or false</returns>
        #region FMS
        bool SubmitFeedbacks(List<SubmitFeedbacksDTO> FeedBacks,  long userID);
        List<FeedbackSearchResultDTO> SearchFeedbacks(FeedbackSearchDTO searchFeedBacks, int storeID, long userID, out bool totalRow);
        bool UpdateFeedbacks(List<UpdateFeedbackDTO> Feedback, long userID, int roleID);
        FMSMastersDTO GetFMSMasters(long userID, int roleID, int LastTeamID, int LastCategoryID, int LastTypeID, int rowcounter);
        FeedbackDetailDTO GetFeedbackDetails(int FeedbackID);
        List<NotificationServiceLogDTO> GetNotifications(long userID, int roleID, byte NotificationType, long LastNotificationServiceID, int RowCounter);
        

        bool UpdateNotificationStatus(long userID, int roleID, List<UpdateNotificationStatusDTO> Notifications);
        #endregion FMS
        #region for Feedback Category and Type Master by Navneet on  05 Dec 2014

        /// <summary>
        /// Gets the Feedback Category Master list.
        /// </summary>
        /// <param></param>
        /// <returns></returns>
        IList<FeedbackCategoryMasterBO> GetFeedbackCategoryMaster(int TeamID);
        IList<FeedbackCategoryMasterBO> GetFeedbackCategoryMaster();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        bool IsCategoryInsert(FeedbackCategoryMasterBO record);

        bool IsCategoryUpdate(FeedbackCategoryMasterBO record);

        bool DeleteCategory(List<int> categorys);


        /// <summary>
        /// Gets the Feedback Type Master list.
        /// </summary>
        /// <param></param>
        /// <returns></returns>  
        IList<FeedbackTypeMasterBO> GetFeedbackTypeMaster(int FeedbackCatID);
        IList<FeedbackTypeMasterBO> GetFeedbackTypeMaster();
        /// <summary>
        /// Insert Type Master list.
        /// </summary>
        /// <param>"record"</param>
        /// <returns>1 or 0</returns>        /// 
        bool IsTypeInsert(FeedbackTypeMasterBO record);
        bool IsTypeUpdate(FeedbackTypeMasterBO record);
        bool DeleteType(List<int> types);
        FeedbackTypeMasterBO GetFeedbackType(int FeedbackTypeID);
        #endregion

        #region NotificationTypeMaster
        List<NotificationTypeMasterDTO> NotificationTypeMaster(long userID, int roleID);
        #endregion

        List<SearchFeedbackStatusCountDTO> SearchFeedbackStatusCount(FeedbackCountSearchDTO feedbackCountSearch, long? userID, int roleID);

        #region KAS Authorization
        /*
         * Created By: Amit Mishra
         * Date: 31 Mar 2015
         * JIRA ID: SDCE-2257
         */
        /// <summary>
        /// Function to get the KAS modules master data
        /// </summary>
        IList<KASModuleBO> GetKASModulesList();

        /// <summary>
        /// Get KAS Authorizations assigned to selected role
        /// </summary>
        /// <param name="RoleID">Particular Role ID for which data of KAS authorization needs to be fetched</param>
        /// <returns>List of KAS Authorization Data for a Role </returns>
        List<KASModuleAuthorizationBO> GetKASAuthorizationByRoleID(int RoleID);

        /// <summary>
        /// Insert KAS Authorizations assigned to selected role
        /// </summary>
        /// <param name="kasModuleList">List of KAS Authorization Data for a Role</param>
        /// <returns>boolean value of if successfully executed</returns>
        bool InsertKASAuthorization(List<KASModuleAuthorizationBO> kasModuleList);

        /// <summary>
        /// Insert KAS Authorizations assigned to selected role
        /// </summary>
        /// <param name="kasModuleList">List of KAS Authorization Data for a Role</param>
        /// <returns>boolean value of if successfully executed</returns>
        bool DeleteKASAuthorization(List<KASModuleAuthorizationBO> kasModuleList);

        #endregion

        #region MSS services for sync adaptor
        List<FeedbackTypeMasterDTO> GetMSSTypeMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        List<TeamMasterDTO> GetMSSTeamMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        List<FeedbackCategoryMasterDTO> GetMSSCategoryMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        List<FeedbackStatusMasterDTO> GetMSSStatusMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);
        #endregion
    }
}
