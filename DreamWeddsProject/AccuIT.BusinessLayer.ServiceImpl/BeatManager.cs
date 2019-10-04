using Samsung.SmartDost.BusinessLayer.Base;
using Samsung.SmartDost.BusinessLayer.Services.Contracts;
using Samsung.SmartDost.CommonLayer.AopContainer;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using Samsung.SmartDost.CommonLayer.Resources;
using Samsung.SmartDost.PersistenceLayer.Repository.Contracts;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samsung.SmartDost.BusinessLayer.ServiceImpl
{
    public class BeatManager : BeatBaseService, IBeatService
    {

        #region Properties

        /// <summary>
        /// Property to inject the store persistence layer object invocation
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.BEAT_REPOSITORY)]
        public IBeatRepository BeatRepository { get; set; }
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.USER_REPOSITORY)]
        public IUserRepository UserRepository { get; set; }
        private ISystemService systemBusinessInstance;
        /// <summary>
        /// Property to get instance of System manager business class
        /// </summary>
        public ISystemService SystemBusinessInstance
        {
            get
            {
                if (systemBusinessInstance == null)
                {
                    systemBusinessInstance = AopEngine.Resolve<ISystemService>(AspectEnums.AspectInstanceNames.ServiceManager, AspectEnums.ApplicationName.Samsung);
                }

                return systemBusinessInstance;
            }
        }

        #endregion

        /// <summary>
        /// Method used to send User's Beat Details to the persistant layer
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="userBeatCollection">Beat collection info</param>
        /// <returns>returns true if Beat is inserted and false if not inserted</returns>
        public int InsertUserBeatDetailsInfo(long userID, List<UserBeatDTO> userBeatCollection)
        {
            List<CoveragePlan> coveragePlan = new List<CoveragePlan>();
            ObjectMapper.Map(userBeatCollection, coveragePlan);
            int index = 0;
            foreach (var item in userBeatCollection)
            {
                coveragePlan[index].CoverageDate = Convert.ToDateTime(item.PlanDate);
                index++;
            }
            int status = BeatRepository.SaveUserBeat(coveragePlan);
            if (status == 1)
            {
                string employeeName = UserRepository.GetEmployeeName(userID);
                employeeName = String.IsNullOrEmpty(employeeName) ? "Employee" : employeeName;
                string message = String.Format(Messages.BeatCreationMessage,employeeName);
                long seniorID = UserRepository.GetSeniorID(userID);
                if (seniorID > 0)
                {
                    #region for SDCE -994 (Notification Queue) by vaishali on 06 Dec 2014
                    //SystemBusinessInstance.SendPushNotification(seniorID, message, string.Empty);                                        
                    SystemBusinessInstance.QueueNotification(seniorID, Messages.NotificationBeatHeader + message, AspectEnums.NotificationType.Beat);
                    #endregion
                }
            }
            return status;
        }

        /// <summary>
        /// Method will call the ApproveRejectBeat method form the persistant layer
        /// </summary>
        /// <param name="userIDCollection">Collection of Beats</param>
        /// <param name="statusID">Status of Beat</param>
        /// <returns></returns>
        public bool ApprovalOrRejectionBeatInfo(Dictionary<int, int> coverageCollection)
        {
            bool status = BeatRepository.ApproveRejectBeat(coverageCollection);
            return status;
        }

        /// <summary>
        ///  Methos to Get the Beat Details info on the basis of status id
        /// </summary>
        /// <param name="statusID">represent 0 if rejected, 1 if approved</param>
        /// <returns>returns list of UserBeatDTO on the basis of status id</returns>
        public IList<UserBeatDTO> GetBeatDetails(int statusID)
        {
            List<CoveragePlan> coveragePlan = new List<CoveragePlan>();
            List<UserBeatDTO> userBeatInfo = new List<UserBeatDTO>();
            coveragePlan = BeatRepository.GetBeatInfoDetails(statusID).ToList();
            ObjectMapper.Map(coveragePlan, userBeatInfo);
            return userBeatInfo;
        }

        /// <summary>
        ///  Methos will delete the Beat which has statusid 2
        /// </summary>
        /// <param name="statusID">represent 0 if rejected, 1 if approved</param>
        /// <returns>Beat will delete only if status id is 2.</returns>
        public string DeleteUsersBeat(int statusID)
        {
            return BeatRepository.DeleteBeat(statusID);
        }

        /// <summary>
        /// Method to fetch pending coverage user list for a reporting user
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns coverage users</returns>
        public IList<CoverageUserDTO> GetCoverageUsers(long userID)
        {
            List<CoverageUserDTO> users = new List<CoverageUserDTO>();
            ObjectMapper.Map(BeatRepository.GetCoverageUsers(userID), users);
            return users;
        }

        /// <summary>
        /// Method to update pending user's coverage
        /// </summary>
        /// <param name="userIDList">user ID list</param>
        /// <param name="status">status</param>
        /// <returns>returns boolean status</returns>
        public bool UpdatePendingCoverage(List<long> userIDList, int status)
        {
            bool response = BeatRepository.UpdatePendingCoverage(userIDList, status);
            if (response)
            {
                string message = string.Empty;
                foreach (long id in userIDList)
                {
                    string employeeName = UserRepository.GetEmployeeName(id);
                    switch (status)
                    {
                        case 1:
                            message = String.Format(Messages.BeatApprovalMessage, employeeName);
                            break;
                        case 2:
                            message = String.Format(Messages.BeatRejectionMessage, employeeName);
                            break;
                    }
                    #region for SDCE -994 (Notification Queue) by vaishali on 06 Dec 2014
                    //SystemBusinessInstance.SendPushNotification(id, message, string.Empty);
                    SystemBusinessInstance.QueueNotification(id, Messages.NotificationBeatHeader + message, AspectEnums.NotificationType.Beat);
                    #endregion
                }
            }
            return response;
        }

        /// <summary>
        /// Method to fetch user beat details
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns beat details</returns>
        public UserBeatDetailsDTO GetUserBeatDetails(long userID)
        {
            //UserBeatDetailsDTO beats = new UserBeatDetailsDTO();
            return BeatRepository.GetUserBeatDetails(userID);
            //return beats;
        }

        /// <summary>
        /// Method to fetch approved user beat details
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns beat details</returns>
        public UserBeatDetailsDTO GetApprovedUserBeatDetails(long userID)
        {
            return BeatRepository.GetApprovedUserBeatDetails(userID);
            
        }

        public bool InsertBeatException(List<UserSystemSettingDTO> userSystemSettings, long currentUserId)
        {
            List<UserSystemSetting> userSystemSettingsDB = new List<UserSystemSetting>();
            ObjectMapper.Map(userSystemSettings, userSystemSettingsDB);
            return BeatRepository.InsertBeatException(userSystemSettingsDB,currentUserId);
        }
    }
}
