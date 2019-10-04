using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccuIT.PersistenceLayer.Repository.Entities;
using AccuIT.CommonLayer.Aspects.Utilities;

namespace AccuIT.PersistenceLayer.Repository.Contracts
{
    /// <summary>
    /// Interface to define user related method
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// This method is used to validate user credentials.
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <returns>returns User Entity</returns>
        User ValidateUser(string userName, string password);

        /// <summary>
        /// Displays the user profile.
        /// </summary>
        /// <param name="userId">The userId.</param>
        /// <returns></returns>
        UserMaster DisplayUserProfile(int userID);

        /// <summary>
        /// Submit Login History
        /// </summary>
        /// <param name="dailyLoginHistory"></param>
        /// <returns></returns>
        DailyLoginHistory GetActiveLogin(int userID, int LoginType);

        /// <summary>
        /// Method to login web user into the application
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <returns>returns login status</returns>
        int LoginWebUser(string userName, string password);

        /// <summary>
        /// Submit Login History
        /// </summary>
        /// <param name="dailyLoginHistory"></param>
        /// <returns></returns>
        bool SubmitDailyLoginHistory(DailyLoginHistory dailyLoginHistory);

        /// <summary>
        /// Fetch list of Role Master
        /// </summary>
        /// <returns>List of RoleMaster</returns>
        IEnumerable<RoleMaster> GetRoleMaster();

        /// <summary>
        /// Method to logout web user from the application
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        bool LogoutWebUser(int userID, string sessionID);

        int SubmitNewEmployee(UserMaster model, string sessionID);

        UserMaster GetUserByLoginName(string userName);

        /// <summary>
        /// Method to authenticate user login using IMEI number
        /// </summary>
        /// <param name="imei">mobile imei number</param>
        /// <param name="password">user's login password</param>
        /// <param name="geoTag">lattitude value</param>
        /// <param name="longitude">longitude value</param>
        /// <returns>returns status code</returns>
        Tuple<AspectEnums.UserLoginStatus, int, string, int, int, int> AuthenticateUser(string imei, string LoginName, string password, string lattitude, string longitude, string BrowserName, string ModelName, string IPaddress);

        /// <summary>
        /// Method to get user service access entity
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns entity instance</returns>
        UserServiceAccess GetUserServiceAccess(int userID);

        /// <summary>
        /// Method to reset user password in system
        /// </summary>
        /// <param name="userid">userid</param>
        /// <param name="newpassword">new password</param>
        /// <returns>returns status</returns>
        AspectEnums.UserResetPasswordStatus ResetUserPassword(string imei, string employeeid, string newpassword);


        ///// <summary>
        ///// Get list of User Master 
        ///// </summary>        
        ///// <returns></returns>
        //IEnumerable<UserMaster> GetUsersMaster();

        /// <summary>
        /// Updates the user profile.
        /// </summary>
        /// <param name="userDetail">The user detail.</param>
        /// <returns></returns>
        bool UpdateUserProfile(UserMaster userDetail);

        ///// <summary>
        ///// Method to validate that user has marked an attendance for mentioned date or not
        ///// </summary>
        ///// <param name="userID">User Primary Key</param>
        ///// <param name="attendanceDate">attendance date</param>
        ///// <returns>returns boolean status</returns>
        //Tuple<bool, int> IsUserHasAttendance(long userID, DateTime attendanceDate);

        ///// <summary>
        ///// Will update the user attendance in db
        ///// </summary>
        ///// <param name="UserAttendance">Attendance info</param>

        ///// <returns>returns status</returns>
        //int MarkAttendance(UserAttendance userAttendance);

        ///// <summary>
        ///// User to send the training info in db
        ///// </summary>
        ///// <param name="userID">User Id</param>
        ///// <param name="trainingDetails">Represent Details of training</param>
        ///// <param name="trainingDate">represent date of training</param>      
        ///// <returns>return status</returns>      
        //int AddTrainingDetails(long userID, string trainingDetails, DateTime trainingDate);

        ///// <summary>
        ///// User to send the geotag image to db
        ///// </summary>
        ///// <param name="userID">User Id</param>
        ///// <param name="longitude">Represent longitude of image</param>
        ///// <param name="latitude">represent latitude of image</param>      
        /////  /// <param name="image">represnt the byte array of image</param> 
        ///// <returns>return true if inserted entry in db else returns false</returns>
        //long SaveGeoTag(SurveyResponse storeTag);

        /// <summary>
        /// Method to fetch all available user modules on the basis of user primary ID
        /// </summary>
        /// <param name="userID">user primary ID</param>
        /// <returns>returns collection of user modules</returns>
        IList<vwGetUserRoleModule> GetUserModules(int empID);


        /// <summary>
        /// Method to fetch all available user modules on the basis of user primary ID
        /// </summary>
        /// <param name="userID">user primary ID</param>
        /// <returns>returns collection of user modules</returns>
        IList<vwGetUserRoleModule> GetUserModules(int empID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate, bool isForsurveyQuestions);


        /// <summary>
        /// Method to reset the login values after user gets logout
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>returns boolean status</returns>
        bool LogoutUser(int userID);

        ///// <summary>
        ///// Method to update user leave plans
        ///// </summary>
        ///// <param name="leavePlan">user leave plans</param>
        ///// <returns>returns boolean status</returns>
        //bool UpdateUserLeaves(IList<UserLeavePlan> leavePlan);

        ///// <summary>
        ///// Method to fetch user system settings value
        ///// </summary>
        ///// <param name="userID">user primary key value</param>
        ///// <returns>returns entity instance</returns>
        //UserSystemSetting GetUserSystemSettings(long userID);

        //#region Added By Vinay Kanojia Dated: 19-Dec-2013
        //IList<UserRole> GetAllUserRole();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool ForgetPasswordWeb(string userName);

        /// <summary>
        /// Method to return user for an employee code
        /// </summary>
        /// <param name="employeeCode">cemployee Code</param>
        /// <returns>returns user</returns>
        UserMaster GetUserByName(string loginName);

        //bool DeleteUserAttendence(long userID, DateTime selectedDate);
        //#endregion

        ///// <summary>
        ///// Method to Reset IsofflineStatus
        ///// </summary>
        ///// <param name="userID">userID</param>
        ///// <returns>returns user</returns>

        //bool ManageUserProfile(long userID, bool currentStatus);


        ///// <summary>
        ///// Method to return user list for a company
        ///// </summary>
        ///// <param name="companyID">company primary ID</param>
        ///// <returns>returns user list</returns>
        //IList<UserMaster> GetUsers(int companyID);

        ///// <summary>
        ///// Method to return device list for a company
        ///// </summary>
        ///// <param name="companyID">company primary ID</param>
        ///// <returns>returns user list</returns>
        //IList<UserDevice> GetUserDevices(long userID);

        ///// <summary>
        ///// Method to save imei number for an user
        ///// </summary>
        ///// <param name="userID">user ID</param>
        ///// <param name="imeiNumber">imei number</param>
        ///// <returns>returns boolean status</returns>
        //int SaveDeviceIMEI(long userID, string imeiNumber, long createdBy);

        ///// <summary>
        ///// Method to delete imei number from database
        ///// </summary>
        ///// <param name="imeiNumber">imei number</param>
        ///// <param name="modifiedBy">modified by</param>
        ///// <param name="userID">user id</param>
        ///// <returns>returns boolean status</returns>
        //bool DeleteDeviceIMEI(string imeiNumber, long modifiedBy, long userID);

        ///// <summary>
        ///// Method to fetch all available user modules on the basis of user primary ID
        ///// </summary>
        ///// <param name="userID">user primary ID</param>
        ///// <returns>returns collection of user modules</returns>
        IList<vwGetUserRoleModule> GetUserWebModules(int userID);

        ///// <summary>
        ///// Method to check whether valid device user
        ///// </summary>
        ///// <param name="imeiNumber">device imei number</param>
        ///// <returns>returns boolean status</returns>
        //bool IsValidIMEINumber(string imeiNumber);

        ///// <summary>
        ///// Method to get user details on the basis of imei number
        ///// </summary>
        ///// <param name="imeiNumber">device imei number</param>
        ///// <returns>returns boolean status</returns>
        //UserMaster GetUserDetailsByIMEINumber(string imeiNumber);

        ///// <summary>
        ///// Method to update android registration id of user profile
        ///// </summary>
        ///// <param name="registrationId">registration id</param>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns boolean status</returns>
        //bool UpdateAndroidRegistrationId(string registrationId, long userID);

        /// <summary>
        /// Method to identify assigned role is admin or not
        /// </summary>
        /// <param name="roleID">role ID</param>
        /// <returns>returns boolean status</returns>
        bool IsAdminRole(int roleID);

        /// <summary>
        /// Method to update user's password in web
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <param name="newPassword">new password</param>
        /// <returns>returns boolean response</returns>
        bool ResetWebUserPassword(int userID, string newPassword);

        ///// <summary>
        ///// Fetch list of Team Master
        ///// </summary>
        ///// <returns>List of RoleMaster</returns>
        //IEnumerable<TeamMaster> GetTeamMaster();

        ///// <summary>
        ///// Method to get employee code of user
        ///// </summary>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns employee code</returns>
        //string GetEmployeeCode(long userID);

        ///// <summary>
        ///// Method to check beat creation allowed or not
        ///// </summary>
        ///// <param name="month">month</param>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns boolean status</returns>
        //bool IsBeatCreationAllowed(int month, long userID, int year);

        ///// <summary>
        ///// Method to get senior employee name
        ///// </summary>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns employee name</returns>
        //string GetSeniorName(long userID);

        ///// <summary>
        ///// Method to get employee name
        ///// </summary>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns employee name</returns>
        //string GetEmployeeName(long userID);

        ///// <summary>
        ///// Method to get senior employee ID of provided ID
        ///// </summary>
        ///// <param name="userID">userID</param>
        ///// <returns>returns senior ID</returns>
        //long GetSeniorID(long userID);
        ///// <summary>

        ///// <param name="dtTo">Date To</param>
        ///// <returns>List of UserLeavePlan data</returns>
        //List<UserLeavePlan> GetUserLeavePlans(long userID, DateTime dtFrom, DateTime dtTo);
        ///// <summary>
        ///// Method to get  Leave plans for a particular date range
        ///// </summary>
        ///// <param name="userID">User Id</param>
        ///// <param name="dtFrom">Date From</param>
        ///// <param name="dtTo">Date To</param>
        ///// <returns>List of UserLeavePlan data</returns>
        //List<UserLeavePlan> GetUserLeavePlans(DateTime dtFrom, DateTime dtTo);
        //IList<GeoDefinition> GetGeoDefinition(int geoId);
        //string GetGeoName(int geoId);
        //IList<GeoDefinition> GetNextGeoDefinition(int currentGeoId, int nextGeoId);
        //List<GeoMaster> GetGeoMaster(int roleId);

        ///// <summary>
        ///// Terminate Old sessions for current user 
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        bool TerminateExistingSessions(int empID);




        ///// <summary>
        ///// Download Master Data codes authorized for selected Role
        ///// </summary>
        ///// <param name="roleID"></param>
        ///// <returns></returns>
        //List<string> GetDownloadDataMasterCodes(int roleID);

        #region Authenticate User Login
        /// <summary>
        /// Authenticate login crednetials for APK
        /// </summary>
        /// <param name="imei"></param>
        /// <param name="password"></param>
        /// <param name="lattitude"></param>
        /// <param name="longitude"></param>
        /// <param name="BrowserName"></param>
        /// <param name="ModelName"></param>
        /// <param name="IPaddress"></param>
        /// <param name="APKVersion"></param>
        /// <param name="LoginType"></param>
        /// <returns></returns>
        SPCheckAuthentication_Result AuthenticateUserLogin(string imei, string loginName, string password, string lattitude, string longitude, string BrowserName, string ModelName, string IPaddress, string APKVersion, byte? LoginType);
        #endregion

        #region Get User Details after authentication
        /// <summary>
        /// Select User details after authentication
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="APKVersion"></param>
        /// <param name="showAnnouncment"></param>
        /// <param name="APIKey"></param>
        /// <param name="APIToken"></param>
        /// <returns></returns>
        SPGetLoginDetails_Result GetLoginDetails
            (
            int userID,
            bool? showAnnouncment,
            string APIKey,
            string APIToken,
            string APKVersion);
        #endregion

        #region Generate Service Tokens
        /// <summary>
        /// Generate Service Tokens
        /// </summary>
        /// <param name="userID"></param>
        void GenerateServiceAccessToken(int userID);
        #endregion

        //#region Beat Exception performance
        ///// <summary>
        ///// Get users on a particular geo
        ///// </summary>
        ///// <param name="geoId">Geo Master ID</param>
        ///// <param name="geoDefId">Geo Definition ID</param>
        ///// <param name="roleID">Role ID</param>
        ///// <returns></returns>
        //IList<UserMaster> GetBeatExceptionData(int geoId, int geoDefId, int roleID);
        //#endregion

    }
}
