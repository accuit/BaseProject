using AccuIT.BusinessLayer.Base;
using AccuIT.BusinessLayer.Services.BO;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.CommonLayer.Aspects.ReportBO;
using AccuIT.CommonLayer.Aspects.Security;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.PersistenceLayer.Repository.Contracts;
using AccuIT.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using AccuIT.CommonLayer.EntityMapper;

namespace AccuIT.BusinessLayer.ServiceImpl
{
    /// <summary>
    /// Class to implement the user related business rules and execution
    /// </summary>
    public class UserManager : UserBaseService, IUserService
    {
        #region Properties

        /// <summary>
        /// Property to inject the user persistence layer object invocation
        /// </summary>
        [Unity.Dependency(ContainerDataLayerInstanceNames.EMP_REPOSITORY)]
        public IUserRepository UserRepository { get; set; }

        /// <summary>
        /// Property to inject the user persistence layer object invocation
        /// </summary>
        [Unity.Dependency(ContainerDataLayerInstanceNames.SYSTEM_REPOSITORY)]
        public ISystemRepository SystemRepository { get; set; }


        /// <summary>
        /// Property to inject the user persistence layer object invocation
        /// </summary>
        [Unity.Dependency(ContainerDataLayerInstanceNames.WEDDING_REPOSITORY)]
        public IWeddingRepository WeddingRepository { get; set; }

        #endregion


        /// <summary>
        /// Submit Login History
        /// </summary>
        /// <param name="dailyLoginHistory"></param>
        /// <returns></returns>
        public DailyLoginHistoryBO GetActiveLogin(int empID, int LoginType)
        {
            DailyLoginHistoryBO dailyLoginHistory = new DailyLoginHistoryBO();
            ObjectMapper.Map(UserRepository.GetActiveLogin(empID, LoginType), dailyLoginHistory);
            return dailyLoginHistory;
        }

        /// <summary>
        /// Method to login web user into the application
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <returns>returns login status</returns>
        public int LoginWebUser(string userName, string password)
        {
            userName = EncryptionEngine.EncryptString(userName);
            password = EncryptionEngine.EncryptString(password);
            return UserRepository.LoginWebUser(userName, password);
        }


        /// <summary>
        /// This method is used to validate user credentials.
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <returns>returns User Entity</returns>
        public UserBO ValidateUser(string userName, string password)
        {

            UserBO user = new UserBO();
            ObjectMapper.Map(UserRepository.ValidateUser(userName, password), user);
            return user;
        }


        /// <summary>
        /// Displays the user profile.
        /// </summary>
        /// <param name="userId">The userId.</param>
        /// <returns></returns>
        public UserProfileBO DisplayUserProfile(int userId)
        {

            UserProfileBO userProfile = new UserProfileBO();
            UserMaster user = UserRepository.DisplayUserProfile(userId);
            List<UserWeddingSubscriptionBO> userWeddingSubscriptions = new List<UserWeddingSubscriptionBO>();
            AddressMasterBO address = new AddressMasterBO();
            ObjectMapper.Map(SystemRepository.GetAddressDetailsByType((int)AspectEnums.AddressOwnerType.User, userId), address);

            ObjectMapper.Map(user, userProfile);
           // userProfile.UserWeddingSubscriptions = userWeddingSubscriptions;
            userProfile.Address = address;
            RoleMaster role = user.UserRoles.Where(k => !k.isDeleted).ToList().Count > 0 ? user.UserRoles.Where(k => !k.isDeleted).ToList()[0].RoleMaster : null;
            userProfile.RoleID = role != null ? role.RoleID : 0;
            userProfile.userRoleID = user.UserRoles.Where(k => !k.isDeleted).FirstOrDefault().UserRoleID;

            if (!String.IsNullOrEmpty(userProfile.ImagePath))
            {
                userProfile.ImagePath = userProfile.ImagePath; // AppUtil.GetServerMobileImages(userProfile.ImagePath, AspectEnums.ImageFileTypes.User);
            }
            else
            {
                userProfile.ImagePath = "~/content/images/users/avatar.png";
            }
            userProfile.IsAdmin = UserRepository.IsAdminRole(Convert.ToInt32(userProfile.RoleID));
            userProfile.Mobile = EncryptionEngine.DecryptString(userProfile.Mobile);
            userProfile.Phone = EncryptionEngine.DecryptString(userProfile.Phone);
            userProfile.Email = EncryptionEngine.DecryptString(userProfile.Email);
            userProfile.LoginName = EncryptionEngine.DecryptString(userProfile.LoginName);
            return userProfile;
        }

        /// <summary>
        /// Method to fetch all available user modules on the basis of user primary ID
        /// </summary>
        /// <param name="userID">user primary ID</param>
        /// <returns>returns collection of user modules</returns>
        public IList<UserModuleDTO> GetUserWebModules(int userID)
        {
            IList<UserModuleDTO> userModules = new List<UserModuleDTO>();
            ObjectMapper.Map(UserRepository.GetUserWebModules(userID), userModules);
            return userModules;
        }

        public int SubmitNewEmployee(UserMasterBO model, string sessionID)
        {
            UserMaster userMaster = new UserMaster();
            model.Mobile = EncryptionEngine.EncryptString(model.Mobile);
            model.Phone = EncryptionEngine.EncryptString(model.Phone);
            model.Email = EncryptionEngine.EncryptString(model.Email);
            model.LoginName = EncryptionEngine.EncryptString(model.LoginName);
            model.Password = EncryptionEngine.EncryptString(model.Password);
            ObjectMapper.Map(model, userMaster);
            return UserRepository.SubmitNewEmployee(userMaster, sessionID);

        }
        public UserProfileBO GetUserByLoginName(string userName)
        {
            UserProfileBO user = new UserProfileBO();
            ObjectMapper.Map(UserRepository.GetUserByLoginName(userName), user);
            return user;
        }


        /// <summary>
        /// Method to logout web user from the application
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public bool LogoutWebUser(int userID, string sessionID)
        {
            return UserRepository.LogoutWebUser(userID, sessionID);
        }


        /// <summary>
        /// Method to authenticate user login using IMEI number
        /// </summary>
        /// <param name="imei">mobile imei number</param>
        /// <param name="password">user's login password</param>
        /// <param name="geoTag">lattitude value</param>
        /// <param name="longitude">longitude value</param>
        /// <returns>returns status code</returns>
        public ServiceResponseBO AuthenticateUser(string imei, string LoginName, string password, string lattitude, string longitude, string BrowserName, string ModelName, string IPaddress)
        {
            string apiKey = string.Empty;
            string apiToken = string.Empty;
            Tuple<AspectEnums.UserLoginStatus, int, string, int, int, int> status = UserRepository.AuthenticateUser(imei, LoginName, password, lattitude, longitude, BrowserName, ModelName, IPaddress);
            if (status.Item1 == AspectEnums.UserLoginStatus.Active)
            {
                UserServiceAccess serviceAccess = UserRepository.GetUserServiceAccess(status.Item2);
                apiKey = serviceAccess.APIKey;
                apiToken = serviceAccess.APIToken;
            }
            return new ServiceResponseBO() { StatusCode = AppUtil.GetEnumStatusString(status.Item1), RoleID = status.Item6, UserID = status.Item2.ToString(), APIKey = apiKey, APIToken = apiToken, UserCode = status.Item3, CompanyID = status.Item4.ToString(), UserDeviceID = status.Item5 };
        }

        public ServiceResponseBO APIKEY(int userid)
        {
            string apiKey = string.Empty;
            string apiToken = string.Empty;

            UserServiceAccess serviceAccess = UserRepository.GetUserServiceAccess(userid);
            apiKey = serviceAccess.APIKey;
            apiToken = serviceAccess.APIToken;

            return new ServiceResponseBO() { APIKey = apiKey, APIToken = apiToken };
        }


        /// <summary>
        /// Method to reset user password in system
        /// </summary>
        /// <param name="imei"></param>
        /// <param name="loginName"></param>
        /// <param name="newpassword">new password</param>
        /// <returns>
        /// returns status code
        /// </returns>

        public ForgotPasswordBO ForgetPassword(string imei, string loginName, string newPassword)
        {
            
            AspectEnums.UserResetPasswordStatus status = UserRepository.ResetUserPassword(imei, loginName, newPassword);
            return new ForgotPasswordBO() { StatusCode = AppUtil.GetEnumStatusString(status) };
        }


        ///// <summary>
        ///// The product groups
        ///// </summary>
        //List<ProductGroupBO> productGroups = new List<ProductGroupBO>();

        ///// <summary>
        ///// Get list of User Master 
        ///// </summary>        
        ///// <returns></returns>
        //public IEnumerable<UserProfileBO> GetUsersMaster()
        //{
        //    List<UserProfileBO> GetUsersMaster = new List<UserProfileBO>();
        //    ObjectMapper.Map(UserRepository.GetUsersMaster(), GetUsersMaster);
        //    return GetUsersMaster;
        //}

        /// <summary>
        /// Updates the user profile.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="mobile">The mobile.</param>
        /// <param name="address">The address.</param>
        /// <param name="emailId">The email identifier.</param>
        /// <returns></returns>
        public bool UpdateUserProfile(UserProfileBO userProfile)
        {
            bool isSuccess = false;
            //UserProfileBO user = new UserProfileBO();
            UserMaster userMaster = new UserMaster();
            userProfile.Mobile = EncryptionEngine.EncryptString(userProfile.Mobile);
            userProfile.Phone = EncryptionEngine.EncryptString(userProfile.Phone);
            userProfile.Email = EncryptionEngine.EncryptString(userProfile.Email);
           // userProfile.LoginName = EncryptionEngine.EncryptString(userProfile.LoginName);
           // userProfile.Password = EncryptionEngine.EncryptString(userProfile.Password);
            ObjectMapper.Map(userProfile, userMaster);
            if (userMaster != null && userMaster.UserID > 0)
                isSuccess = UserRepository.UpdateUserProfile(userMaster);
            return isSuccess;
        }

        ///// <summary>
        ///// Method to validate that user has marked an attendance for mentioned date or not
        ///// </summary>
        ///// <param name="userID">User Primary Key</param>
        ///// <param name="attendanceDate">attendance date</param>
        ///// <returns>returns boolean status</returns>
        //public Tuple<bool, int> IsUserHasAttendance(long userID, DateTime attendanceDate)
        //{
        //    return UserRepository.IsUserHasAttendance(userID, attendanceDate);
        //}

        ///// <summary>
        ///// User to send the attendance info to the business layer
        ///// </summary>
        /////<param name="userAttendance">user attendance DTO</param>
        /////<param name="numberOfDays">number of days</param>
        ///// <returns>return 1 if new entry inserted in db, 2 if entry is updated in db</returns>
        //public int MarkAttendance(UserAttendanceDTO userAttendance, int numberOfDays)
        //{
        //    int status = 0;
        //    UserAttendance attendance = new UserAttendance();
        //    ObjectMapper.Map(userAttendance, attendance);
        //    attendance.AttendanceDate = System.DateTime.Now;
        //    if (attendance.IsAttendance)
        //        status = UserRepository.MarkAttendance(attendance);
        //    else
        //    {
        //        List<UserLeavePlan> plans = new List<UserLeavePlan>();
        //        UserLeavePlan leavePlan = new UserLeavePlan()
        //        {
        //            LeaveDate = System.DateTime.Today,
        //            LeaveToDate = System.DateTime.Today,
        //            LeaveTypeID = 0,
        //            CreatedDate = System.DateTime.Now,
        //            CreatedBy = userAttendance.UserID,
        //            Remarks = userAttendance.Remarks,
        //            UserID = userAttendance.UserID,
        //        };
        //        plans.Add(leavePlan);
        //        if (numberOfDays > 1)
        //        {
        //            for (int count = 1; count <= numberOfDays; count++)
        //            {
        //                UserLeavePlan furtherPlan = new UserLeavePlan()
        //                {
        //                    //LeaveDate = System.DateTime.Today.AddDays(count - 1),
        //                    //LeaveToDate = System.DateTime.Today.AddDays(count - 1),
        //                    LeaveDate = System.DateTime.Today.AddDays(count),
        //                    LeaveToDate = System.DateTime.Today.AddDays(count),
        //                    LeaveTypeID = 1,
        //                    CreatedDate = System.DateTime.Now,
        //                    CreatedBy = userAttendance.UserID,
        //                    Remarks = userAttendance.Remarks,
        //                    UserID = userAttendance.UserID,
        //                };
        //                plans.Add(furtherPlan);
        //            }
        //        }
        //        status = UserRepository.UpdateUserLeaves(plans) ? 1 : 3;
        //    }
        //    return status;

        //}

        ///// <summary>
        ///// User to send the training info in db
        ///// </summary>
        ///// <param name="employeeid">User Id</param>
        ///// <param name="trainingDetails">Represent Details of training</param>
        ///// <param name="trainingDate">represent date of training</param>      
        ///// <returns>return true if inserted entry in db else returns false</returns>
        //public int OfficeTrainingDetails(long userID, string trainingDetails, DateTime trainingDate)
        //{
        //    return UserRepository.AddTrainingDetails(userID, trainingDetails, trainingDate);
        //}

        ///// <summary>
        ///// This Method used to call the menthod of repository
        ///// </summary>
        ///// <param name="employeeid">User Id</param>
        ///// <param name="longitude">Represent longitude of image</param>
        ///// <param name="latitude">represent latitude of image</param>      
        /////  /// <param name="image">represnt the byte array of image</param> 
        ///// <returns>return true if inserted entry in db else returns false</returns>
        //public long SaveGeoTag(SurveyResponseDTO storeTag)
        //{
        //    SurveyResponse response = new SurveyResponse();
        //    ObjectMapper.Map(storeTag, response);
        //    return UserRepository.SaveGeoTag(response);
        //}


        /// <summary>
        /// Method to fetch all available user modules on the basis of user primary ID
        /// </summary>
        /// <param name="userID">user primary ID</param>
        /// <returns>returns collection of user modules</returns>
        public IList<UserModuleDTO> GetUserModules(int userID, int RoleID)
        {
            bool isExceptionAdded = false;
            IList<UserModuleDTO> userModules = new List<UserModuleDTO>();
            ObjectMapper.Map(UserRepository.GetUserModules(userID), userModules);

            return userModules.OrderBy(x => x.Sequence).ThenBy(x => x.Name).ToList();
        }

        /// <summary>
        /// Method to fetch all available user modules on the basis of user primary ID
        /// </summary>
        /// <param name="userID">user primary ID</param>
        /// <returns>returns collection of user modules</returns>
        public IList<UserModuleDTO> GetUserModules(int userID, int RoleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate, bool isForsurveyQuestions)
        {
            bool isExceptionAdded = false;
            //DateTime? MaxModifiedDateTime;
            // string strMaxModifiedDate;
            IList<UserModuleDTO> userModules = new List<UserModuleDTO>();
            ObjectMapper.Map(UserRepository.GetUserModules(userID, RoleID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out MaxModifiedDate, isForsurveyQuestions), userModules);
            return userModules.OrderBy(x => x.Sequence).ThenBy(x => x.Name).ToList();
        }



        /// <summary>
        /// Method to reset the login values after user gets logout
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>returns boolean status</returns>
        public bool LogoutUser(int userID)
        {
            return UserRepository.LogoutUser(userID);
        }

        ///// <summary>
        ///// Method to fetch user system settings value
        ///// </summary>
        ///// <param name="userID">user primary key value</param>
        ///// <returns>returns entity instance</returns>
        //public UserSystemSettingDTO GetUserSystemSettings(long userID)
        //{
        //    UserSystemSettingDTO userSettings = new UserSystemSettingDTO();
        //    ObjectMapper.Map(UserRepository.GetUserSystemSettings(userID), userSettings);
        //    return userSettings;
        //}

        //#region Added By Vinay Kanojia Dated: 19-Dec-2013

        //public IList<UserRoleBO> GetAllUserRole()
        //{
        //    IList<UserRoleBO> lstUserRole = new List<UserRoleBO>();
        //    ObjectMapper.Map(UserRepository.GetAllUserRole(), lstUserRole);
        //    return lstUserRole;
        //}

        /// <summary>
        /// Method for send Password in case of Forget Password
        /// </summary>
        /// <param name="userName">Employee Code </param>
        /// <returns></returns>
        public bool ForgetPasswordWeb(string userName)
        {
            return UserRepository.ForgetPasswordWeb(userName);
        }

        /// <summary>
        /// Method to return user for an employee code
        /// </summary>
        /// <param name="loginName">cemployee Code</param>
        /// <returns>returns user</returns>
        public UserProfileBO GetUserByName(string loginName)
        {
            UserProfileBO userDetail = new UserProfileBO();
            ObjectMapper.Map(UserRepository.GetUserByName(loginName), userDetail);
            userDetail.Email = EncryptionEngine.DecryptString(userDetail.Email);
            userDetail.Mobile = EncryptionEngine.DecryptString(userDetail.Mobile);
            return userDetail;
        }

        //public bool DeleteUserAttendence(long userID, DateTime selectedDate)
        //{
        //    return UserRepository.DeleteUserAttendence(userID, selectedDate);
        //}
        //#endregion



        ///// <summary>
        ///// Method to return user list for a company
        ///// </summary>
        ///// <param name="companyID">company primary ID</param>
        ///// <returns>returns user list</returns>
        //public IList<UserBO> GetUsers(int companyID)
        //{
        //    List<UserBO> users = new List<UserBO>();
        //    ObjectMapper.Map(UserRepository.GetUsers(companyID), users);
        //    return users;
        //}

        ///// <summary>
        ///// Method to Reset IsofflineStatus
        ///// </summary>
        ///// <param name="userID">userID</param>
        ///// <returns>returns user</returns>

        //public bool ManageUserProfile(long userID, bool currentStatus)
        //{
        //    return UserRepository.ManageUserProfile(userID, currentStatus);
        //}

        ///// <summary>
        ///// Method to return device list for a company
        ///// </summary>
        ///// <param name="companyID">company primary ID</param>
        ///// <returns>returns user list</returns>
        //public IList<UserDeviceBO> GetUserDevices(long userID)
        //{
        //    List<UserDeviceBO> devices = new List<UserDeviceBO>();
        //    ObjectMapper.Map(UserRepository.GetUserDevices(userID), devices);
        //    return devices;
        //}

        ///// <summary>
        ///// Method to save imei number for an user
        ///// </summary>
        ///// <param name="userID">user ID</param>
        ///// <param name="imeiNumber">imei number</param>
        ///// <returns>returns boolean status</returns>
        //public int SaveDeviceIMEI(long userID, string imeiNumber, long createdBy)
        //{
        //    return UserRepository.SaveDeviceIMEI(userID, imeiNumber, createdBy);
        //}

        ///// <summary>
        ///// Method to delete imei number from database
        ///// </summary>
        ///// <param name="imeiNumber">imei number</param>
        ///// <param name="modifiedBy">modified by</param>
        ///// <param name="userID">user id</param>
        ///// <returns>returns boolean status</returns>
        //public bool DeleteDeviceIMEI(string imeiNumber, long modifiedBy, long userID)
        //{
        //    return UserRepository.DeleteDeviceIMEI(imeiNumber, modifiedBy, userID);
        //}

        ///// <summary>
        ///// Method to check whether valid device user
        ///// </summary>
        ///// <param name="imeiNumber">device imei number</param>
        ///// <returns>returns boolean status</returns>
        //public bool IsValidIMEINumber(string imeiNumber)
        //{
        //    return UserRepository.IsValidIMEINumber(imeiNumber);
        //}

        ///// <summary>
        ///// Method to get user details on the basis of imei number
        ///// </summary>
        ///// <param name="imeiNumber">device imei number</param>
        ///// <returns>returns boolean status</returns>
        //public UserProfileBO GetUserDetailsByIMEINumber(string imeiNumber)
        //{
        //    UserProfileBO userProfile = new UserProfileBO();
        //    ObjectMapper.Map(UserRepository.GetUserDetailsByIMEINumber(imeiNumber), userProfile);
        //    return userProfile;
        //}

        ///// <summary>
        ///// Method to update android registration id of user profile
        ///// </summary>
        ///// <param name="registrationId">registration id</param>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns boolean status</returns>
        //public bool UpdateAndroidRegistrationId(string registrationId, long userID)
        //{
        //    return UserRepository.UpdateAndroidRegistrationId(registrationId, userID);
        //}

        /// <summary>
        /// Method to reset user's password
        /// </summary>
        /// <param name="employeeCode">employee code</param>
        /// <returns>returns boolean status</returns>
        public bool ResetUserPassword(string employeeCode)
        {
            bool isReset = false;
            UserMaster userDetails = UserRepository.GetUserByName(employeeCode);
            if (userDetails != null)
            {
                string password = DateTime.Now.ToString().GetHashCode().ToString("x");
                string encryptedPassword = EncryptionEngine.EncryptString(password);
                isReset = UserRepository.ResetWebUserPassword(userDetails.UserID, encryptedPassword);
                if (isReset && !String.IsNullOrEmpty(EncryptionEngine.DecryptString(userDetails.Email)))
                {
                    SendResetPasswordEmail(EncryptionEngine.DecryptString(userDetails.Email), String.Format("{0} {1}", userDetails.FirstName, userDetails.LastName).Trim(), password);
                    isReset = true;
                }
            }
            return isReset;
        }

        ///// <summary>
        ///// Method to find beat coverage days
        ///// </summary>
        ///// <returns>returns days list</returns>
        //private List<int> GetBeatCoverageDays(long RoleID)
        //{            
        //    List<int> days = new List<int>();
        //    //SystemSetting settings = SystemRepository.GetSystemSettings();
        //    BeatWindowSetting settings = SystemRepository.GetBeatWindowSettings((int)RoleID);
        //    if (settings != null)
        //    {
        //        if (!String.IsNullOrEmpty(settings.CoveragePlanFirstWindow))
        //        {
        //            string[] dayArray = settings.CoveragePlanFirstWindow.Trim().Split('-');
        //            if (dayArray.Length > 0)
        //            {
        //                int firstSlot = Convert.ToInt32(dayArray[0]);
        //                int secondSlot = Convert.ToInt32(dayArray[1]);

        //                for (int i = firstSlot; i <= secondSlot; i++)
        //                {
        //                    days.Add(i);
        //                }
        //                if (!String.IsNullOrEmpty(settings.CoveragePlanSecondWndow))
        //                {
        //                    dayArray = settings.CoveragePlanSecondWndow.Trim().Split('-');
        //                    firstSlot = Convert.ToInt32(dayArray[0]);
        //                    secondSlot = Convert.ToInt32(dayArray[1]);
        //                    for (int i = firstSlot; i <= secondSlot; i++)
        //                    {
        //                        days.Add(i);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return days;
        //}

        //private bool IsBeatCreationEnabled(int month, long userID, int year)
        //{
        //    return UserRepository.IsBeatCreationAllowed(month, userID, year);
        //}

        ///// <summary>
        ///// Method to check whether beat creation modules is enabled or not
        ///// </summary>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns boolean status</returns>
        //private bool IsBeatCreationEnabled(long userID)
        //{
        //    bool isAllowed = false;
        //    int coverageFirstWindowDay = 0;
        //    int coverageSecondWindowDay = 0;
        //    List<int> days = new List<int>();
        //    SystemSetting settings = SystemRepository.GetSystemSettings();
        //    if (settings != null)
        //    {
        //        if (!String.IsNullOrEmpty(settings.CoveragePlanFirstWindow))
        //        {
        //            string[] dayArray = settings.CoveragePlanFirstWindow.Trim().Split('-');
        //            if (dayArray.Length > 0)
        //            {
        //                int firstSlot = Convert.ToInt32(dayArray[0]);
        //                coverageFirstWindowDay = firstSlot;
        //                int secondSlot = Convert.ToInt32(dayArray[1]);

        //                for (int i = firstSlot; i <= secondSlot; i++)
        //                {
        //                    days.Add(i);
        //                }
        //                if (days.Contains(DateTime.Today.Day))
        //                {
        //                    isAllowed = IsBeatCreationEnabled(DateTime.Today.Month, userID, DateTime.Today.Year);
        //                    return isAllowed;
        //                }
        //                if (!String.IsNullOrEmpty(settings.CoveragePlanSecondWndow))
        //                {
        //                    dayArray = settings.CoveragePlanSecondWndow.Trim().Split('-');
        //                    firstSlot = Convert.ToInt32(dayArray[0]);
        //                    coverageSecondWindowDay = firstSlot;
        //                    secondSlot = Convert.ToInt32(dayArray[1]);
        //                    for (int i = firstSlot; i <= secondSlot; i++)
        //                    {
        //                        days.Add(i);
        //                    }
        //                    if (days.Contains(DateTime.Today.Day))
        //                    {
        //                        isAllowed = IsBeatCreationEnabled(DateTime.Today.AddMonths(1).Month, userID, DateTime.Today.Year);
        //                        return isAllowed;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    if (!isAllowed)
        //    {
        //        UserSystemSetting userSettings = UserRepository.GetUserSystemSettings(userID);
        //        if (userSettings != null)
        //        {
        //            List<int> exceptionDays = new List<int>();
        //            if (userSettings.IsCoverageException && userSettings.CoverageExceptionWindow.HasValue)
        //            {
        //                string[] dayArray = new string[] { userSettings.CoverageExceptionWindow.Value.Day.ToString() };
        //                if (dayArray.Length > 0)
        //                {
        //                    int firstSlot = Convert.ToInt32(dayArray[0]);
        //                    if (dayArray.Length > 1)
        //                    {
        //                        int secondSlot = Convert.ToInt32(dayArray[1]);

        //                        for (int i = firstSlot; i <= secondSlot; i++)
        //                        {
        //                            exceptionDays.Add(i);
        //                        }
        //                        if (secondSlot >= coverageSecondWindowDay)
        //                        {
        //                            isAllowed = IsBeatCreationEnabled(DateTime.Today.AddMonths(1).Month, userID, DateTime.Today.Year);
        //                            return isAllowed;
        //                        }
        //                        else
        //                        {
        //                            isAllowed = IsBeatCreationEnabled(DateTime.Today.Month, userID, DateTime.Today.Year);
        //                            return isAllowed;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        if (firstSlot >= coverageFirstWindowDay)
        //                        {
        //                            isAllowed = IsBeatCreationEnabled(DateTime.Today.Month, userID, DateTime.Today.Year);
        //                            return isAllowed;
        //                        }
        //                        else
        //                        {
        //                            isAllowed = IsBeatCreationEnabled(DateTime.Today.AddMonths(1).Month, userID, DateTime.Today.Year);
        //                            return isAllowed;
        //                        }

        //                    }
        //                }

        //            }
        //        }
        //    }
        //    return isAllowed;
        //}

        ///// <summary>
        ///// Method to check user exception beat settings
        ///// </summary>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns boolean status</returns>
        //private bool IsUserExceptionDateAdded(long userID)
        //{
        //    UserSystemSetting userSettings = UserRepository.GetUserSystemSettings(userID);
        //    if (userSettings != null)
        //    {
        //        List<int> days = new List<int>();
        //        if (userSettings.IsCoverageException && userSettings.CoverageExceptionWindow.HasValue)
        //        {
        //            string[] dayArray = new string[] { userSettings.CoverageExceptionWindow.Value.Day.ToString() };
        //            if (dayArray.Length > 0)
        //            {
        //                int firstSlot = Convert.ToInt32(dayArray[0]);
        //                if (dayArray.Length > 1)
        //                {
        //                    int secondSlot = Convert.ToInt32(dayArray[1]);

        //                    for (int i = firstSlot; i <= secondSlot; i++)
        //                    {
        //                        days.Add(i);
        //                    }
        //                }
        //                else
        //                {
        //                    days.Add(firstSlot);
        //                }
        //            }
        //            return days.Count > 0 && days.Contains(System.DateTime.Today.Day);
        //        }
        //    }
        //    return false;
        //}

        /// <summary>
        /// Method to send reset password email to user
        /// </summary>
        /// <param name="emailID">email ID</param>
        /// <param name="toName">to name</param>
        /// <param name="password">new password</param>
        private void SendResetPasswordEmail(string emailID, string toName, string password)
        {
            EmailServiceDTO emailService = new EmailServiceDTO();
            emailService.ToEmail = emailID;
            emailService.ToName = toName;
            emailService.Subject = "Your Password Details";
            emailService.FromEmail = emailID;
            emailService.Body = "Hi, <br/>Please check your Login Details<br/><br/>Your Username: " + toName + "<br/><br/>Your Password: " + password + "<br/><br/>";
            emailService.IsHtml = true;
            emailService.IsAttachment = false;
            emailService.AttachmentFileName = string.Empty;
            emailService.Priority = 1;
            emailService.Status = (int)AspectEnums.EmailStatus.Pending;
            //IBatchService batchInstance = AopEngine.Resolve<IBatchService>(AspectEnums.AspectInstanceNames.BatchManager, AspectEnums.ApplicationName.Samsung);
            //batchInstance.InsertEmailRecord(emailService);
        }

        //#endregion
        /// <summary>
        /// Fetch list of Role Master
        /// </summary>
        /// <returns>List of RoleMaster</returns>
        public IEnumerable<RoleMasterBO> GetRoleMaster()
        {
            List<RoleMasterBO> roles = new List<RoleMasterBO>();
            ObjectMapper.Map(UserRepository.GetRoleMaster(), roles);
            return roles;
        }
        ///// <summary>
        ///// Fetch list of Team Master
        ///// </summary>
        ///// <returns>List of RoleMaster</returns>
        //public IEnumerable<TeamMasterBO> GetTeamMaster()
        //{
        //    List<TeamMasterBO> teams = new List<TeamMasterBO>();
        //    ObjectMapper.Map(UserRepository.GetTeamMaster(), teams);
        //    return teams;
        //}

        ///// <summary>
        ///// Method to get senior employee name
        ///// </summary>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns employee name</returns>
        //public string GetSeniorName(long userID)
        //{
        //    return UserRepository.GetSeniorName(userID);
        //}
        ///// <summary>
        ///// Method to get User Leave plan for a particular date range
        ///// </summary>
        ///// <param name="userID">User Id</param>
        ///// <param name="dtFrom">Date From</param>
        ///// <param name="dtTo">Date To</param>
        ///// <returns>List of UserLeavePlan data</returns>
        //public List<UserLeavePlanBO> GetUserLeavePlans(long userID, DateTime dtFrom, DateTime dtTo)
        //{
        //    List<UserLeavePlanBO> leavePlans = new List<UserLeavePlanBO>();
        //    ObjectMapper.Map(UserRepository.GetUserLeavePlans(userID, dtFrom, dtTo), leavePlans);
        //    return leavePlans;
        //}
        ///// <summary>
        ///// Method to get  Leave plans for a particular date range
        ///// </summary>
        ///// <param name="userID">User Id</param>
        ///// <param name="dtFrom">Date From</param>
        ///// <param name="dtTo">Date To</param>
        ///// <returns>List of UserLeavePlan data</returns>
        //public List<UserLeavePlanBO> GetUserLeavePlans(DateTime dtFrom, DateTime dtTo)
        //{
        //    List<UserLeavePlanBO> leavePlans = new List<UserLeavePlanBO>();
        //    ObjectMapper.Map(UserRepository.GetUserLeavePlans(dtFrom, dtTo), leavePlans);
        //    return leavePlans;
        //}

        //public IList<GeoDefinitionBO> GetGeoDefinition(int geoId)
        //{
        //    List<GeoDefinitionBO> defs = new List<GeoDefinitionBO>();
        //    ObjectMapper.Map(UserRepository.GetGeoDefinition(geoId), defs);
        //    return defs;
        //}

        //public string GetGeoName(int geoId)
        //{
        //    return UserRepository.GetGeoName(geoId);
        //}

        //public IList<GeoDefinitionBO> GetNextGeoDefinition(int currentGeoId, int nextGeoId)
        //{
        //    List<GeoDefinitionBO> defs = new List<GeoDefinitionBO>();
        //    ObjectMapper.Map(UserRepository.GetNextGeoDefinition(currentGeoId, nextGeoId), defs);
        //    return defs;
        //}

        //public List<GeoMasterBO> GetGeoMaster(int roleId)
        //{
        //    List<GeoMasterBO> lst = new List<GeoMasterBO>();
        //    ObjectMapper.Map(UserRepository.GetGeoMaster(roleId), lst);
        //    return lst;
        //}

        ///// <summary>
        ///// Terminate Old sessions for current user 
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        public bool TerminateExistingSessions(int empID)
        {
            return UserRepository.TerminateExistingSessions(empID);
        }


        /// <summary>
        /// Submit Login History
        /// </summary>
        /// <param name="dailyLoginHistory"></param>
        /// <returns></returns>
        public bool SubmitDailyLoginHistory(DailyLoginHistoryBO dailyLoginHistory)
        {
            DailyLoginHistory LoginHistory = new DailyLoginHistory();
            ObjectMapper.Map(dailyLoginHistory, LoginHistory);
            return UserRepository.SubmitDailyLoginHistory(LoginHistory);
        }



        ///// <summary>
        ///// Download Master Data codes authorized for selected Role
        ///// </summary>
        ///// <param name="roleID"></param>
        ///// <returns></returns>
        //public List<string> GetDownloadDataMasterCodes(int roleID)
        //{
        //    List<string> result = new List<string>();
        //    result = UserRepository.GetDownloadDataMasterCodes(roleID);
        //    return result;
        //}

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
        public CheckUserAuthenticationBO AuthenticateUserLogin(string imei, string loginName, string password, string lattitude, string longitude, string BrowserName, string ModelName, string IPaddress, string APKVersion, byte? LoginType)
        {
            CheckUserAuthenticationBO result = new CheckUserAuthenticationBO();
            ObjectMapper.Map(UserRepository.AuthenticateUserLogin(imei, loginName, password, lattitude, longitude, BrowserName, ModelName, IPaddress, APKVersion, LoginType), result);
            return result;
        }
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
        public UserLoginDetailsBO GetLoginDetails(int userID, string APKVersion, bool? showAnnouncment, string apiKey, string apiToken)
        {
            UserLoginDetailsBO result = new UserLoginDetailsBO();
            ObjectMapper.Map(UserRepository.GetLoginDetails(userID, showAnnouncment, apiKey, apiToken, APKVersion), result);
            return result;
        }
        #endregion

        //#region Generate Service Tokens
        ///// <summary>
        ///// Generate Service Tokens
        ///// </summary>
        ///// <param name="userID"></param>
        //public void GenerateServiceAccessToken(long userID)
        //{
        //    UserRepository.GenerateServiceAccessToken(userID);
        //}
        //#endregion

        //#region Beat Exception performance
        ///// <summary>
        ///// Get users on a particular geo
        ///// </summary>
        ///// <param name="geoId">Geo Master ID</param>
        ///// <param name="geoDefId">Geo Definition ID</param>
        ///// <param name="roleID">Role ID</param>
        ///// <returns></returns>
        //public IList<UserProfileBO> GetBeatExceptionData(int geoId, int geoDefId, int roleID)
        //{
        //    List<UserProfileBO> users = new List<UserProfileBO>();
        //    ObjectMapper.Map(UserRepository.GetBeatExceptionData(geoId,geoDefId,roleID), users);
        //    return users;
        //}
        //#endregion
    }

}
