using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;

namespace Samsung.SmartDost.PersistenceLayer.Repository.Contracts
{
    public interface IFeedbackRepository
    {
        #region for SDCE -1005 (FMS) by vaishali on 27 Nov 2014
        bool SubmitFeedbacks(List<SubmitFeedbacks> FeedBacks,  long userID);
        List<SPGetFeedbackSearch_Result> SearchFeedbacks(FeedbackSearch searchFeedBacks, int storeID, long userID, out bool HasMoreRows);
        bool UpdateFeedbacks(List<UpdateFeedback> feedback, long userID, int roleID);


        List<TeamMaster> GetTeamMaster(long userID, int roleID, int LastTeamID, int rowcounter, out bool HasMoreRows);

        List<FeedbackCategoryMaster> GetFeedbackCategoryMaster(long userID, int roleID, int LastCategoryID, int rowcounter, out bool HasMoreRows);

        List<FeedbackTypeMaster> GetFeedbackTypeMaster(long userID, int roleID, int LastTypeID, int rowcounter, out bool HasMoreRows);

        List<CommonSetup> GetMasterFromCommonSetup(long userID, int roleID, string MainType);

        FeedbackMaster GetFeedbackDetails(int FeedbackID);
        #region for Feedback Category and Type Master by Navneet on  05 Dec 2014
        IList<FeedbackCategoryMaster> GetFeedbackCategoryList(int TeamID);
        IList<FeedbackTypeMaster> GetFeedbackTypeList(int FeedbackCatID);
        FeedbackTypeMaster GetFeedbackType(int FeedbackTypeID);
        IList<FeedbackCategoryMaster> GetFeedbackCategoryList();
        IList<FeedbackTypeMaster> GetFeedbackTypeList();
        bool IsSuccessfullInsert(FeedbackCategoryMaster response);
        bool IsSuccessfullUpdate(FeedbackCategoryMaster response);
        bool IsSuccessfullInsert(FeedbackTypeMaster response);
        bool IsSuccessfullUpdate(FeedbackTypeMaster response);
        bool DeleteCategorys(List<int> categorys);
        bool DeleteTypes(List<int> types);

        List<NotificationServiceLog> GetNotifications(long userID, int roleID, byte NotificationType, long LastNotificationServiceID, int RowCounter);
        bool UpdateNotificationStatus(long userID, int roleID, List<UpdateNotificationStatus> Notifications);

        #endregion
        #endregion

        #region NotificationTypeMaster
        List<NotificationTypeMaster> NotificationTypeMaster(long userID, int roleID);        
        #endregion

        List<SPGetFeedbackCountSearch_Result> SearchFeedbackStatusCount(FeedbackCountSearch feedbackCountSearch, long? userID, int roleID);
       
        #region KAS Authorization
        /*
         * Created By: Amit Mishra
         * Date: 31 Mar 2015
         * JIRA ID: SDCE-2257
         */
        /// <summary>
        /// Function to get the KAS modules master data
        /// </summary>
        IList<KASModule> GetKASModulesList();

        /// <summary>
        /// Get KAS Authorizations assigned to selected role
        /// </summary>
        /// <param name="RoleID">Particular Role ID for which data of KAS authorization needs to be fetched</param>
        /// <returns>List of KAS Authorization Data for a Role </returns>
        List<KASModuleAuthorization> GetKASAuthorizationByRoleID(int RoleID);

        /// <summary>
        /// Insert KAS Authorizations assigned to selected role
        /// </summary>
        /// <param name="kasModuleList">List of KAS Authorization Data for a Role</param>
        /// <returns>boolean value of if successfully executed</returns>
        bool InsertKASAuthorization(List<KASModuleAuthorization> kasModuleList);

        /// <summary>
        /// Insert KAS Authorizations assigned to selected role
        /// </summary>
        /// <param name="kasModuleList">List of KAS Authorization Data for a Role</param>
        /// <returns>boolean value of if successfully executed</returns>
        bool DeleteKASAuthorization(List<KASModuleAuthorization> kasModuleList);
        #endregion

        #region MSS services for sync adaptor
        IList<FeedbackTypeMaster> GetMSSTypeMaster(long userID,int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);


        IList<TeamMaster> GetMSSTeamMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);


        IList<FeedbackCategoryMaster> GetMSSCategoryMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);


        IList<CommonSetup> GetMSSStatusMaster(long userID, int roleID, string MainType, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);
        #endregion
    }
}
