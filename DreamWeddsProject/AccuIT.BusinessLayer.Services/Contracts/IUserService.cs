using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccuIT.BusinessLayer.Services.BO;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.CommonLayer.Aspects.ReportBO;

namespace AccuIT.BusinessLayer.Services.Contracts
{
    /// <summary>
    /// Interface to expose the User business definitions and methods
    /// </summary>
    public interface IUserService
    {

        /// <summary>
        /// Submit Login History
        /// </summary>
        /// <param name="dailyLoginHistory"></param>
        /// <returns></returns>
        DailyLoginHistoryBO GetActiveLogin(int userID, int LoginType);


        /// <summary>
        /// This method is used to validate user credentials.
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <returns>returns User Entity</returns>
        UserBO ValidateUser(string userName, string password);

        /// <summary>
        /// Displays the user profile.
        /// </summary>
        /// <param name="userId">The userId.</param>
        /// <returns></returns>
        UserProfileBO DisplayUserProfile(int userId);


        /// <summary>
        /// Method to login web user into the application
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <returns>returns login status</returns>
        int LoginWebUser(string userName, string password);

        /// <summary>
        /// Fetch list of Role Master
        /// </summary>
        /// <returns>List of RoleMaster</returns>
        IEnumerable<RoleMasterBO> GetRoleMaster();


        int SubmitNewEmployee(UserMasterBO model, string sessionID);

        UserProfileBO GetUserByLoginName(string userName);

        bool LogoutWebUser(int userID, string sessionID);



        ///// <summary>
        ///// Method to fetch all available user modules on the basis of user primary ID
        ///// </summary>
        ///// <param name="userID">user primary ID</param>
        ///// <returns>returns collection of user modules</returns>
        IList<UserModuleDTO> GetUserModules(int userID, int RoleID);

        ///// <summary>
        ///// Method to fetch incremental available user modules on the basis of user primary ID
        ///// </summary>
        ///// <param name="userID">user primary ID</param>
        ///// <returns>returns collection of user modules</returns>
         IList<UserModuleDTO> GetUserModules(int userID, int RoleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate, bool isForsurveyQuestions);


        /// <summary>
        /// Method to authenticate user login using IMEI number
        /// </summary>
        /// <param name="imei">mobile imei number</param>
        /// <param name="password">user's login password</param>
        /// <param name="geoTag">lattitude value</param>
        /// <param name="longitude">longitude value</param>
        /// <returns>returns status code</returns>
        ServiceResponseBO AuthenticateUser(string imei, string LoginName, string password, string lattitude, string longitude, string BrowserName, string ModelName, string IPaddress);

        ServiceResponseBO APIKEY(int userid);
        /// <summary>
        /// Method to authenticate user login using IMEI number
        /// </summary>
        /// <param name="imei">mobile imei number</param>
        /// <param name="password">user's login password</param>
        /// <param name="geoTag">lattitude value</param>
        /// <param name="longitude">longitude value</param>
        /// <returns>returns status code</returns>

        ForgotPasswordBO ForgetPassword(string imei, string employeeid, string newpassword);


        ///// <summary>
        ///// Get list of User Master 
        ///// </summary>        
        ///// <returns></returns>
        //IEnumerable<UserProfileBO> GetUsersMaster();

        /// <summary>
        /// Updates the user profile.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="mobile">The mobile.</param>
        /// <param name="address">The address.</param>
        /// <param name="emailId">The email identifier.</param>
        /// <returns></returns>
        bool UpdateUserProfile(UserProfileBO userProfile);

        ///// <summary>
        ///// Method to validate that user has marked an attendance for mentioned date or not
        ///// </summary>
        ///// <param name="userID">User Primary Key</param>
        ///// <param name="attendanceDate">attendance date</param>
        ///// <returns>returns boolean status</returns>
        //Tuple<bool, int> IsUserHasAttendance(long userID, DateTime attendanceDate);

        ///// <summary>
        ///// User to send the attendance info to the business layer
        ///// </summary>
        /////<param name="userAttendance">user attendance DTO</param>
        /////<param name="numberOfDays">number of days</param>
        ///// <returns>return 1 if new entry inserted in db, 2 if entry is updated in db</returns>
        //int MarkAttendance(UserAttendanceDTO userAttendance, int numberOfDays);

        ///// <summary>
        ///// User to send the training info in db
        ///// </summary>
        ///// <param name="userID">User Id</param>
        ///// <param name="trainingDetails">Represent Details of training</param>
        ///// <param name="trainingDate">represent date of training</param>      
        ///// <returns>return status</returns>
        //int OfficeTrainingDetails(long userID, string trainingDetails, DateTime trainingDate);

        ///// <summary>
        ///// User to send the geotag image to business layer
        ///// </summary>
        ///// <param name="employeeid">User Id</param>
        ///// <param name="longitude">Represent longitude of image</param>
        ///// <param name="latitude">represent latitude of image</param>      
        /////  /// <param name="image">represnt the byte array of image</param> 
        ///// <returns>return true if inserted entry in db else returns false</returns>
        //long SaveGeoTag(SurveyResponseDTO storeTag);

        /// <summary>
        /// Method to reset the login values after user gets logout
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>returns boolean status</returns>
        bool LogoutUser(int userID);

        ///// <summary>
        ///// Method to fetch user system settings value
        ///// </summary>
        ///// <param name="userID">user primary key value</param>
        ///// <returns>returns entity instance</returns>
        //UserSystemSettingDTO GetUserSystemSettings(long userID);




        //#region Added By Vinay Kanojia Dated: 19-Dec-2013

        //IList<UserRoleBO> GetAllUserRole();

        /// <summary>
        /// Method for send Password in case of Forget Password
        /// </summary>
        /// <param name="userName">Employee Code </param>
        /// <returns></returns>
        bool ForgetPasswordWeb(string userName);

        /// <summary>
        /// Method to return user for an employee code
        /// </summary>
        /// <param name="employeeCode">cemployee Code</param>
        /// <returns>returns user</returns>
        UserProfileBO GetUserByName(string loginName);

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
        //IList<UserBO> GetUsers(int companyID);

        ///// <summary>
        ///// Method to return device list for a company
        ///// </summary>
        ///// <param name="companyID">company primary ID</param>
        ///// <returns>returns user list</returns>
        //IList<UserDeviceBO> GetUserDevices(long userID);

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
        //UserProfileBO GetUserDetailsByIMEINumber(string imeiNumber);

        ///// <summary>
        ///// Method to fetch all available user modules on the basis of user primary ID
        ///// </summary>
        ///// <param name="userID">user primary ID</param>
        ///// <returns>returns collection of user modules</returns>
        IList<UserModuleDTO> GetUserWebModules(int empID);

        ///// <summary>
        ///// Method to update android registration id of user profile
        ///// </summary>
        ///// <param name="registrationId">registration id</param>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns boolean status</returns>
        //bool UpdateAndroidRegistrationId(string registrationId, long userID);

        ///// <summary>
        ///// Method to reset user's password
        ///// </summary>
        ///// <param name="employeeCode">employee code</param>
        ///// <returns>returns boolean status</returns>
        //bool ResetUserPassword(string employeeCode);

        ///// <summary>
        ///// Method to get senior employee name
        ///// </summary>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns employee name</returns>
        //string GetSeniorName(long userID);
        //#region Role Master


        //#endregion
        //#region Team Master
        ///// <summary>
        ///// Fetch list of Team Master
        ///// </summary>
        ///// <returns>List of RoleMaster</returns>
        //IEnumerable<TeamMasterBO> GetTeamMaster();
        //#endregion
        ///// <summary>
        ///// Method to get User Leave plan for a particular date range
        ///// </summary>
        ///// <param name="userID">User Id</param>
        ///// <param name="dtFrom">Date From</param>
        ///// <param name="dtTo">Date To</param>
        ///// <returns>List of UserLeavePlan data</returns>
        //List<UserLeavePlanBO> GetUserLeavePlans(long userID, DateTime dtFrom, DateTime dtTo);
        ///// <summary>

        ///// <param name="dtTo">Date To</param>
        ///// <returns>List of UserLeavePlan data</returns>
        //List<UserLeavePlanBO> GetUserLeavePlans(DateTime dtFrom, DateTime dtTo);
        //IList<GeoDefinitionBO> GetGeoDefinition(int geoId);
        //string GetGeoName(int geoId);
        //IList<GeoDefinitionBO> GetNextGeoDefinition(int currentGeoId, int nextGeoId);
        //List<GeoMasterBO> GetGeoMaster(int roleId);

        ///// <summary>
        ///// Terminate Old sessions for current user 
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        bool TerminateExistingSessions(int empID);

        /// <summary>
        /// Submit Login History
        /// </summary>
        /// <param name="dailyLoginHistory"></param>
        /// <returns></returns>
        bool SubmitDailyLoginHistory(DailyLoginHistoryBO dailyLoginHistory);


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
        CheckUserAuthenticationBO AuthenticateUserLogin(string imei, string LoginName, string password, string lattitude, string longitude, string BrowserName, string ModelName, string IPaddress, string APKVersion, byte? LoginType);
        #endregion

        #region Get User Details after authentication
        /// </summary>
        /// Select User details after authentication
        /// <param name="userID"></param>
        /// <param name="APKVersion"></param>
        /// <param name="showAnnouncment"></param>
        /// <param name="apiKey"></param>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        UserLoginDetailsBO GetLoginDetails(int userID, string APKVersion, bool? showAnnouncment, string apiKey, string apiToken);
        #endregion


        //#region Generate Service Tokens
        ///// <summary>
        ///// Generate Service Tokens
        ///// </summary>
        ///// <param name="userID"></param>
        //void GenerateServiceAccessToken(long userID);
        //#endregion
        //#region Beat Exception performance
        ///// <summary>
        ///// Get users on a particular geo
        ///// </summary>
        ///// <param name="geoId">Geo Master ID</param>
        ///// <param name="geoDefId">Geo Definition ID</param>
        ///// <param name="roleID">Role ID</param>
        ///// <returns></returns>
        //IList<UserProfileBO> GetBeatExceptionData(int geoId, int geoDefId, int roleID);
        //#endregion
    }
}
