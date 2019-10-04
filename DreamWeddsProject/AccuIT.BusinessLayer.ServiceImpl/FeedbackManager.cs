using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Samsung.SmartDost.BusinessLayer.Base;
using Samsung.SmartDost.BusinessLayer.Services.Contracts;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using Samsung.SmartDost.PersistenceLayer.Repository.Contracts;
using Samsung.SmartDost.BusinessLayer.Services.BO;

namespace Samsung.SmartDost.BusinessLayer.ServiceImpl
{
    public class FeedbackManager: FeedbackBaseService,IFeedbackService
    {

        /// <summary>
        /// Property to inject the store persistence layer object invocation
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.FEEDBACK_REPOSITORY)]
        public IFeedbackRepository FeedbackRepository { get; set; }

        #region for SDCE -1005 (FMS) by vaishali on 27 Nov 2014
        /// <summary>
        ///  This function returns mapping between compititor and compprofuctgroup
        /// </summary>
        /// <returns>returns list of mapped data</returns>
        public bool SubmitFeedbacks(List<SubmitFeedbacksDTO> FeedBacks, long userID)
        {
            List<SubmitFeedbacks> objFeedback = new List<SubmitFeedbacks>();
            ObjectMapper.Map(FeedBacks, objFeedback);
            return FeedbackRepository.SubmitFeedbacks(objFeedback, userID);
        }


        public List<FeedbackSearchResultDTO> SearchFeedbacks(FeedbackSearchDTO searchFeedBacks, int storeID, long userID, out bool HasMoreRows)
        {
            FeedbackSearch objFeedback = new FeedbackSearch();
            ObjectMapper.Map(searchFeedBacks, objFeedback);

            List<FeedbackSearchResultDTO> searchresult = new List<FeedbackSearchResultDTO>();
            ObjectMapper.Map(FeedbackRepository.SearchFeedbacks(objFeedback, storeID, userID, out HasMoreRows), searchresult);
            return searchresult;

        }

        public bool UpdateFeedbacks(List<UpdateFeedbackDTO> inputDTO, long userID, int roleID)
        {
            List<UpdateFeedback> objFeedback = new List<UpdateFeedback>();
            ObjectMapper.Map(inputDTO, objFeedback);
            return FeedbackRepository.UpdateFeedbacks(objFeedback, userID, roleID);
        }

        public FMSMastersDTO GetFMSMasters(long userID, int roleID, int LastTeamID, int LastCategoryID, int LastTypeID, int rowcounter)
        {
            FMSMastersDTO result = new FMSMastersDTO();
            List<TeamMasterDTO> lstTeam = null;
            List<FeedbackCategoryMasterDTO> lstCategory = null;
            List<FeedbackTypeMasterDTO> lstType = null;
            List<FeedbackStatusMasterDTO> lstStatus = null;

            bool TeamHasMoreRows = false;
            bool CatHasMoreRows = false;
            bool TypeHasMoreRows = false;

            lstTeam = (from t in FeedbackRepository.GetTeamMaster(userID, roleID,LastTeamID, rowcounter, out TeamHasMoreRows)
                       select new TeamMasterDTO { TeamId = t.TeamID, TeamName = t.Name }).ToList();


            lstCategory = (from c in FeedbackRepository.GetFeedbackCategoryMaster(userID, roleID, LastCategoryID, rowcounter, out CatHasMoreRows)
                           select new FeedbackCategoryMasterDTO()
                           {
                               FeedbackCatID = c.FeedbackCatID,
                               FeedbackCategoryName = c.FeedbackCategoryName,
                               TeamID = c.TeamID 
                           }
                                                                                ).ToList();
            lstType = (from t in FeedbackRepository.GetFeedbackTypeMaster(userID, roleID, LastTypeID, rowcounter, out TypeHasMoreRows)
                       select new FeedbackTypeMasterDTO()
                       {
                           FeedbackTypeID = t.FeedbackTypeID,
                           FeedbackCatID = t.FeedbackCatID ,
                           FeedbackTypeName = t.FeedbackTypeName
                           ,SampleImageName=t.SampleImageName
                       }).ToList();
            result.HasMoreRows = TeamHasMoreRows || CatHasMoreRows || TypeHasMoreRows;  // If any of these have more rows then call the service again
            if (LastTeamID <= 0 && LastTypeID <= 0 && LastCategoryID <= 0)
            {
                lstStatus = (from s in FeedbackRepository.GetMasterFromCommonSetup(userID, roleID, "FeedbackStatusMaster")
                             select new FeedbackStatusMasterDTO()
                             {
                                 FeedbackStatusID = Convert.ToInt32(s.DisplayValue ?? 0),
                                 FeedbackStatusName = s.DisplayText
                             }).ToList();
            }
            result.FeedbackCategories = lstCategory;
            result.FeedbackTypes = lstType;
            result.Teams = lstTeam;
            result.Status = lstStatus;


            return result;
        }

        public FeedbackDetailDTO GetFeedbackDetails(int FeedbackID)
        {
            FeedbackDetailDTO objResult = null;

            FeedbackMaster objFeedback = FeedbackRepository.GetFeedbackDetails(FeedbackID);
            
            DateTime FeedbackCloseDate;
            //=DateTime.Today;
            if (objFeedback != null)
            {
                if (objFeedback.CurrentStatusID == 4 && objFeedback.CurrentStatusID == 8 && objFeedback.CurrentStatusID == 9)
                    FeedbackCloseDate = objFeedback.ModifiedOn.Value.Date;
                else
                    FeedbackCloseDate = DateTime.Now;

                //double TimeTaken = (FeedbackCloseDate - objFeedback.CreatedOn.Date).TotalDays;
                double TimeTaken = (FeedbackCloseDate - objFeedback.CreatedOn).TotalHours;

                objResult = new FeedbackDetailDTO();
                objResult.FeedbackCatID = objFeedback.FeedbackCatID;
                objResult.FeedbackNumber = objFeedback.FeedbackNo;
                objResult.TeamID = objFeedback.FeedbackCategoryMaster.TeamID;
                objResult.FeedbackTypeID = objFeedback.FeedbackTypeID;
                objResult.TimeTaken = Convert.ToSingle(Math.Round(TimeTaken, 0));
                objResult.SpocID = objFeedback.SpocID;
                objResult.ImageURL = objFeedback.ImageURL;
                objResult.CurrentFeedbackStatusID = objFeedback.CurrentStatusID;
                //objFeedback.FeedbackStatusLogs.FirstOrDefault(k=>k.StatusID==1).PendingWith.Value;
                objResult.UserID = objFeedback.CreatedBy;
                var logs = from log in objFeedback.FeedbackStatusLogs
                           select new FeedbackDetailLogDTO()
                           {
                               CreatedBy = log.UserMaster.FirstName + " " + log.UserMaster.LastName,
                               CreatedOn = log.CreatedOn.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                               FeedbackStatusID = log.StatusID.Value,
                               PendingWith = log.UserMaster1.FirstName + " " + log.UserMaster1.LastName,
                               Remarks = log.Remarks,
                               ResponseDate = log.ResponseDate == null ? "" : log.ResponseDate.Value.ToString("dd/MM/yyyy HH:mm:ss"),
                           };
                objResult.FeedbackDetailLog = logs.ToList();
            }
            return objResult;
        }

        #endregion




        #region for Feedback Category and Type Master by Navneet on  05 Dec 2014
        public IList<FeedbackCategoryMasterBO> GetFeedbackCategoryMaster(int TeamID)
        {
            IList<FeedbackCategoryMasterBO> lstCategory = new List<FeedbackCategoryMasterBO>();
            ObjectMapper.Map(FeedbackRepository.GetFeedbackCategoryList(TeamID), lstCategory); 
            return lstCategory;
        }


        public IList<FeedbackTypeMasterBO> GetFeedbackTypeMaster(int FeedbackCatID)
        {
            IList<FeedbackTypeMasterBO> lstType = new List<FeedbackTypeMasterBO>();
            ObjectMapper.Map(FeedbackRepository.GetFeedbackTypeList(FeedbackCatID), lstType);
            return lstType;
        }

        public FeedbackTypeMasterBO GetFeedbackType(int FeedbackTypeID)
        {
            FeedbackTypeMasterBO ObjType = new FeedbackTypeMasterBO();
            ObjectMapper.Map(FeedbackRepository.GetFeedbackType(FeedbackTypeID), ObjType);
            return ObjType;
        }

        public IList<FeedbackCategoryMasterBO> GetFeedbackCategoryMaster()
        {
            IList<FeedbackCategoryMasterBO> lstCategory = new List<FeedbackCategoryMasterBO>();
            ObjectMapper.Map(FeedbackRepository.GetFeedbackCategoryList(), lstCategory);
            return lstCategory;
        }


        public IList<FeedbackTypeMasterBO> GetFeedbackTypeMaster()
        {
            IList<FeedbackTypeMasterBO> lstType = new List<FeedbackTypeMasterBO>();
            ObjectMapper.Map(FeedbackRepository.GetFeedbackTypeList(), lstType);
            return lstType;
        }

        public bool IsTypeInsert(FeedbackTypeMasterBO record)
        {
            bool isSuccess = false;
            FeedbackTypeMaster response = new FeedbackTypeMaster();
            ObjectMapper.Map(record, response);
            isSuccess = FeedbackRepository.IsSuccessfullInsert(response);
            return isSuccess;
        }


        public bool IsTypeUpdate(FeedbackTypeMasterBO record)
        {
            bool isSuccess = false;
            FeedbackTypeMaster response = new FeedbackTypeMaster();
            ObjectMapper.Map(record, response);
            isSuccess = FeedbackRepository.IsSuccessfullUpdate(response);
            return isSuccess;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public bool IsCategoryUpdate(FeedbackCategoryMasterBO record)
        {
            bool isSuccess = false;
            FeedbackCategoryMaster response = new FeedbackCategoryMaster();
            ObjectMapper.Map(record, response);
            isSuccess = FeedbackRepository.IsSuccessfullUpdate(response);
            return isSuccess;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public bool IsCategoryInsert(FeedbackCategoryMasterBO record)
        {
            bool isSuccess = false;
            FeedbackCategoryMaster response = new FeedbackCategoryMaster();
            ObjectMapper.Map(record, response);
            isSuccess = FeedbackRepository.IsSuccessfullInsert(response);
            return isSuccess;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        public bool DeleteCategory(List<int> categorys)
        {
            bool isDeleted = false;
            isDeleted = FeedbackRepository.DeleteCategorys(categorys);
            return isDeleted;
        }

        public bool DeleteType(List<int> types)
        {
            bool isDeleted = false;
            isDeleted = FeedbackRepository.DeleteTypes(types);
            return isDeleted;
        }

        #endregion

        public List<NotificationServiceLogDTO> GetNotifications(long userID, int roleID, byte NotificationType, long LastNotificationServiceID, int RowCounter)
        {
            List<NotificationServiceLogDTO> TargetNotifications = new List<NotificationServiceLogDTO>();

            List<NotificationServiceLog> SourceNotifications= FeedbackRepository.GetNotifications(userID, roleID, NotificationType, LastNotificationServiceID, RowCounter);

            foreach (NotificationServiceLog source in SourceNotifications)
            {
                NotificationServiceLogDTO Target = new NotificationServiceLogDTO();
                ObjectMapper.Map(source, Target);
                if (source.NotificationDate != null)
                    Target.NotificationDate = source.NotificationDate.Value.ToString("dd/MM/yyyy HH:mm:ss");
                Target.NotificationDate = Target.NotificationDate.Replace("\r\n", "<br>");
                TargetNotifications.Add(Target);
            }
            return TargetNotifications;
        }

        public bool UpdateNotificationStatus(long userID, int roleID, List<UpdateNotificationStatusDTO> Notifications)
        {
            bool Result=false;
            List<UpdateNotificationStatus> objInput= new List<UpdateNotificationStatus> ();

            ObjectMapper.Map(Notifications, objInput);

            FeedbackRepository.UpdateNotificationStatus(userID, roleID, objInput);
            return Result;

        }
        #region NotificationTypeMaster
        public List<NotificationTypeMasterDTO> NotificationTypeMaster(long userID, int roleID)
        {
            List<NotificationTypeMasterDTO> result = new List<NotificationTypeMasterDTO>();
            ObjectMapper.Map(FeedbackRepository.NotificationTypeMaster(userID, roleID),result);
            return result;

        }
        #endregion

        public List<SearchFeedbackStatusCountDTO> SearchFeedbackStatusCount(FeedbackCountSearchDTO feedbackCountSearch, long? userID, int roleID)
        {
            FeedbackCountSearch objFeedback = new FeedbackCountSearch();
            ObjectMapper.Map(feedbackCountSearch, objFeedback);

            List<SearchFeedbackStatusCountDTO> result = new List<SearchFeedbackStatusCountDTO>();
            ObjectMapper.Map(FeedbackRepository.SearchFeedbackStatusCount(objFeedback, userID, roleID), result);
            return result;
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
        public IList<KASModuleBO> GetKASModulesList()
        {

            IList<KASModuleBO> result = new List<KASModuleBO>();
            ObjectMapper.Map(FeedbackRepository.GetKASModulesList(), result);
            return result;
        }

        /// <summary>
        /// Get KAS Authorizations assigned to selected role
        /// </summary>
        /// <param name="RoleID">Particular Role ID for which data of KAS authorization needs to be fetched</param>
        /// <returns>List of KAS Authorization Data for a Role </returns>
        public List<KASModuleAuthorizationBO> GetKASAuthorizationByRoleID(int RoleID)
        {
            List<KASModuleAuthorizationBO> result = new List<KASModuleAuthorizationBO>();
            ObjectMapper.Map(FeedbackRepository.GetKASAuthorizationByRoleID(RoleID), result);
            return result.ToList();
            
        }

        /// <summary>
        /// Insert KAS Authorizations assigned to selected role
        /// </summary>
        /// <param name="kasModuleList">List of KAS Authorization Data for a Role</param>
        /// <returns>boolean value of if successfully executed</returns>
        public bool InsertKASAuthorization(List<KASModuleAuthorizationBO> kasModuleList)
        {
            bool isSuccess = false;
            if (kasModuleList != null && kasModuleList.Count > 0)
            {
                List<KASModuleAuthorization> kasAuthorizations = new List<KASModuleAuthorization>();
                ObjectMapper.Map(kasModuleList,kasAuthorizations);
                isSuccess = FeedbackRepository.InsertKASAuthorization(kasAuthorizations);
            }
            return isSuccess;
        }

        /// <summary>
        /// Insert KAS Authorizations assigned to selected role
        /// </summary>
        /// <param name="kasModuleList">List of KAS Authorization Data for a Role</param>
        /// <returns>boolean value of if successfully executed</returns>
        public bool DeleteKASAuthorization(List<KASModuleAuthorizationBO> kasModuleList)
        {
            bool isSuccess = false;
            if (kasModuleList != null && kasModuleList.Count > 0)
            {
                List<KASModuleAuthorization> kasAuthorizations = new List<KASModuleAuthorization>();
                ObjectMapper.Map(kasModuleList, kasAuthorizations);
                isSuccess = FeedbackRepository.DeleteKASAuthorization(kasAuthorizations);
            }
            return isSuccess;
        }

        #endregion

        #region MSS services for sync adaptor
        public List<FeedbackTypeMasterDTO> GetMSSTypeMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            List<FeedbackTypeMasterDTO> result = new List<FeedbackTypeMasterDTO>();
            ObjectMapper.Map(FeedbackRepository.GetMSSTypeMaster(userID, roleID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out MaxModifiedDate), result);
            return result;
        }


        public List<TeamMasterDTO> GetMSSTeamMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            List<TeamMasterDTO> result = new List<TeamMasterDTO>();            
                        
            result = (from t in FeedbackRepository.GetMSSTeamMaster(userID, roleID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out MaxModifiedDate)
                       select new TeamMasterDTO { TeamId = t.TeamID, TeamName = t.Name }).ToList();
            
            return result;
        }



        public List<FeedbackCategoryMasterDTO> GetMSSCategoryMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            List<FeedbackCategoryMasterDTO> result = new List<FeedbackCategoryMasterDTO>();            
            ObjectMapper.Map(FeedbackRepository.GetMSSCategoryMaster(userID, roleID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out MaxModifiedDate), result);
            return result;
        }


        public List<FeedbackStatusMasterDTO> GetMSSStatusMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            List<FeedbackStatusMasterDTO> result = new List<FeedbackStatusMasterDTO>();
            
            var MSSStatus = FeedbackRepository.GetMSSStatusMaster(userID, roleID,"FeedbackStatusMaster", RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out MaxModifiedDate);
            result = (from s in MSSStatus
                         select new FeedbackStatusMasterDTO()
                         {
                             FeedbackStatusID = Convert.ToInt32(s.DisplayValue ?? 0),
                             FeedbackStatusName = s.DisplayText
                         }).ToList();
            return result;
        }
        #endregion



    }
}
