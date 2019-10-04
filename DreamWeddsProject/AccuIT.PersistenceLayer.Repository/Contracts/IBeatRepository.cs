using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;

namespace Samsung.SmartDost.PersistenceLayer.Repository.Contracts
{
    /// <summary>
    /// Interface to define Beat related methods
    /// </summary>
    public interface IBeatRepository
    {
        /// <summary>
        /// User to send the Beat Details info to the persistance layer
        /// </summary>
        ///<param name="userBeatCollection">Beat collection info of the user</param>
        /// <returns>return true if entry inserted else false</returns>
        int SaveUserBeat(List<CoveragePlan> userBeatCollection);

        /// <summary>
        ///  Method to Get the Beat Details info on the basis of status id
        /// </summary>
        /// <param name="statusID">represent 0 if rejected, 1 if approved</param>
        /// <returns>list of CoveragePlan records returns on the basis of status id </returns>
        IList<CoveragePlan> GetBeatInfoDetails(int statusID);

        /// <summary>
        /// Approves/Rejectes beat submitted by user.
        /// </summary>
        /// <param name="coverageCollection">The coverage collection.</param>
        /// <returns></returns>
        bool ApproveRejectBeat(Dictionary<int, int> coverageCollection);

        /// <summary>
        ///  Methos will delete the Beat which has statusid 2
        /// </summary>
        /// <param name="statusID">represent 0 if rejected, 1 if approved</param>
        /// <returns>Method will return a confirmation message if records deleted from the table</returns>
        string DeleteBeat(int statusID);

        /// <summary>
        /// Method to fetch pending coverage user list for a reporting user
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns coverage users</returns>
        IList<vwPendingCoverageUser> GetCoverageUsers(long userID);

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
        /// <param name="userID"></param>
        /// <returns></returns>
        UserBeatDetailsDTO GetApprovedUserBeatDetails(long userID);

        /// <summary>
        /// This function will insert usersytem setting
        /// </summary>
        /// <param name="userSystemSettings"></param>
        /// <returns></returns>
        bool InsertBeatException(List<UserSystemSetting> userSystemSettings, long currentUserId);
    }
}
