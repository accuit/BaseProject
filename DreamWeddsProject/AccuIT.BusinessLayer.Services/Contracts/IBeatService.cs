using Samsung.SmartDost.BusinessLayer.Services.BO;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using System.Collections.Generic;

namespace Samsung.SmartDost.BusinessLayer.Services.Contracts
{
    public interface IBeatService
    {
        /// <summary>
        /// Method used to send User's Beat Details to the persistant layer
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="userBeatCollection">Beat collection info</param>
        /// <returns>returns true if Beat is inserted and false if not inserted</returns>
        int InsertUserBeatDetailsInfo(long userID, List<UserBeatDTO> userBeatCollection);

        /// <summary>
        /// Method to Approve / Reject the Beat info
        /// </summary>
        ///<param name="userIdCollection">collection of userid </param>
        ///<param name="statusID">status of Beat either approved or rejected</param>
        /// <returns>return true if entry inserted else false</returns>    
        bool ApprovalOrRejectionBeatInfo(Dictionary<int, int> coverageCollection);

        /// <summary>
        ///  Method to Get the Beat Details info on the basis of status id
        /// </summary>
        /// <param name="statusID">represent 0 if rejected, 1 if approved</param>
        /// <returns></returns>
        IList<UserBeatDTO> GetBeatDetails(int statusID);

        /// <summary>
        ///  Method will delete the Beat records which has statusid 2
        /// </summary>
        /// <param name="statusID">represent 0 if rejected, 1 if approved</param>
        /// <returns>Beat will delete only if status id is 2.</returns>
        string DeleteUsersBeat(int statusID);

        /// <summary>
        /// Method to fetch pending coverage user list for a reporting user
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns coverage users</returns>
        IList<CoverageUserDTO> GetCoverageUsers(long userID);

        /// <summary>
        /// Method to update pending user's coverage
        /// </summary>
        /// <param name="userIDList">user ID list</param>
        /// <param name="status">status</param>
        /// <returns>returns boolean status</returns>
        bool UpdatePendingCoverage(List<long> userIDList, int status);

        /// <summary>
        /// Method to fetch user beat details
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns beat details</returns>
        UserBeatDetailsDTO GetUserBeatDetails(long userID);

        /// <summary>
        /// Method to fetch approved user beat details
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns beat details</returns>
        UserBeatDetailsDTO GetApprovedUserBeatDetails(long userID);

        bool InsertBeatException(List<UserSystemSettingDTO> userSystemSettings, long currentUserId);
    }
}
