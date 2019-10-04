using AccuIT.CommonLayer.Aspects.Security;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.PersistenceLayer.Repository.Contracts;
using AccuIT.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net.Mail;
using System.Transactions;
using System.Data.Objects;
using System.Web;
using System.Net.NetworkInformation;
using System.Net;


namespace AccuIT.PersistenceLayer.Data.Impl
{
    /// <summary>
    /// User Data Layer implementation
    /// </summary>
    public class EmpDataImpl : BaseDataImpl, IEmpRepository
    {

        /// <summary>
        /// Select active login for current user
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="LoginType"></param>
        /// <returns></returns>
        public DailyLoginHistory GetActiveLogin(int userid, int LoginType)
        {

            return AccuitAdminDbContext.DailyLoginHistories.Where(x => x.EmpID == userid && x.IsLogin == true && x.LoginType == LoginType).FirstOrDefault();
        }



        //        #region Public Methods

        /// <summary>
        /// This method is used to validate user credentials.
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <returns>returns User Entity</returns>
        public Emp ValidateUser(string userName, string password)
        {
            throw new NotImplementedException();
        }

        ///        <summary>
        ///Method to login web user into the application
        ///</summary>
        ///<param name="userName">user name</param>
        ///<param name="password">password</param>
        ///<returns>returns login status</returns>
        public int LoginWebUser(string userName, string password)
        {
            EmployeeMaster empDetail = default(EmployeeMaster);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
new TransactionOptions
{
    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
}))
            {
                empDetail = AccuitAdminDbContext.EmployeeMasters.FirstOrDefault(k => k.UserID == userName && !k.isDeleted);
                if (empDetail != null)
                {
                    LoginAttemptHistory history = AccuitAdminDbContext.LoginAttemptHistories.FirstOrDefault(k => k.EmpID == empDetail.EmpID);
                    if (empDetail.Password == password)
                    {
                        if (empDetail.AccountStatus == (int)AspectEnums.UserLoginStatus.Active)
                        {

                            UpdateUserLoginHistory(history, empDetail.EmpID, System.DateTime.Now, string.Empty, string.Empty, true);
                            if (empDetail.EmpRoles.Where(k => k.IsActive == true).ToList().Count > 0)
                            {

                                bool isAdmin = empDetail.EmpRoles.Where(k => k.IsActive == true).ToList()[0].RoleMaster.IsAdmin ? true : false;
                                if (!isAdmin)
                                {
                                    bool isWebUser = true;//GetUserWebModules(empDetail.EmpID).Count > 0 ? true : false;
                                    if (isWebUser)
                                    {
                                        scope.Complete();
                                        return empDetail.EmpID;
                                    }

                                    scope.Complete();
                                    return (int)AspectEnums.UserLoginAttemptStatus.InvalidWebUser;
                                }
                                else
                                {
                                    scope.Complete();
                                    return empDetail.EmpID;
                                }
                            }
                        }
                      
                    }
                    else
                    {
                        UpdateUserLoginHistory(history, empDetail.EmpID, System.DateTime.Now, string.Empty, string.Empty, false);
                        scope.Complete();
                        return (int)AspectEnums.UserLoginAttemptStatus.WrongPassword;
                    }
                }

                scope.Complete();
            }

            return (int)AspectEnums.UserLoginAttemptStatus.WrongUserId;

            //EmployeeMaster empDetail = default(EmployeeMaster);
            //empDetail = AccuitAdminDbContext.EmployeeMasters.FirstOrDefault(k => k.EmpCode == userName);
            //if (empDetail != null)
            //{
            //    LoginAttemptHistory history = AccuitAdminDbContext.LoginAttemptHistories.FirstOrDefault(k => k.EmpID == empDetail.EmpID);
            //    if (empDetail.Password == password)
            //    {
            //        if (empDetail.AccountStatus == (int)AspectEnums.UserLoginStatus.Active)
            //        {
            //            UpdateUserLoginHistory(history, empDetail.EmpID, System.DateTime.Now, string.Empty, string.Empty, true);
            //            return (int)AspectEnums.UserLoginAttemptStatus.Successful;
            //        }
                  
            //    }

            //}
            //return (int)AspectEnums.UserLoginAttemptStatus.Locked;
        }


        public int SubmitNewEmployee(EmployeeMaster model, string sessionID)
        {
            model.CreatedDate = DateTime.Now;
            AccuitAdminDbContext.EmployeeMasters.Add(model);
            return AccuitAdminDbContext.SaveChanges();

        }

        public int? GetEmpIDByUserName(string userName)
        {
            return AccuitAdminDbContext.EmployeeMasters.Where(x => x.UserID == userName).First().EmpID;
        }

        /// <summary>
        /// Method to authenticate user login using IMEI number
        /// </summary>
        /// <param name="imei">mobile imei number</param>
        /// <param name="password">user's login password</param>
        /// <param name="geoTag">geo tag value</param>
        /// <returns>returns status code</returns>
//        public Tuple<AspectEnums.UserLoginStatus, long, string, int, int, int> AuthenticateUser(string imei, string password, string lattitude, string longitude, string BrowserName, string ModelName, string IPaddress)
//        {

//            long userID = 0;
//            int companyId = 0;
//            string employeeId = string.Empty;
//            int userDeviceID = 0;
//            int roleID = 0;
//            AspectEnums.UserLoginStatus loginStatus = AspectEnums.UserLoginStatus.None;
//            bool isSuccess = false;
//            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
//new TransactionOptions
//{
//    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
//}))
//            {
//                //List<UserDevice> devices = AccuitAdminDbContext.UserDevices.Where(k => k.IMEINumber == imei && !k.IsDeleted).ToList();
//                //if (devices.Count < 2)
//                //{
//                //    UserDevice device = devices.FirstOrDefault(k => k.IMEINumber == imei && !k.IsDeleted);
//                //    if (device != null)
//                //    {
//                        EmployeeMaster userDetail = AccuitAdminDbContext.EmployeeMasters.FirstOrDefault(k => k.UserID == device.UserID && !k.isDeleted);
//                        LoginAttemptHistory history = AccuitAdminDbContext.LoginAttemptHistories.FirstOrDefault(k => k.UserID == device.UserID);
//                        if (userDetail != null)
//                        {
//                            loginStatus = AppUtil.NumToEnum<AspectEnums.UserLoginStatus>(userDetail.AccountStatus);
//                            if (loginStatus == AspectEnums.UserLoginStatus.Active)
//                            {
//                                SystemSetting settings = new SystemDataImpl().GetCompanySystemSettings(userDetail.CompanyID);
//                                int expiringDays = settings != null ? settings.IdleSystemDay : 3;
//                                if (userDetail.Password == password)
//                                {
//                                    if (history != null && history.LastLoginDate != null)
//                                    {
//                                        if (GetLoginDayDifference(history.LastLoginDate.Value) > expiringDays)
//                                            loginStatus = AspectEnums.UserLoginStatus.DaysExpire;

//                                    }
//                                    isSuccess = loginStatus == AspectEnums.UserLoginStatus.Active ? true : false;
//                                    if (isSuccess)
//                                    {
//                                        userID = userDetail.UserID;
//                                        employeeId = userDetail.EmplCode;
//                                        companyId = userDetail.CompanyID;
//                                        GenerateServiceAccessToken(userDetail.UserID);
//                                        userDeviceID = device.UserDeviceID;
//                                        roleID = userDetail.UserRoles.Where(k => !k.IsDeleted).ToList().Count > 0 ? userDetail.UserRoles.Where(k => !k.IsDeleted).ToList()[0].RoleID : 0;
//                                    }

//                                }
//                                else
//                                {
//                                    loginStatus = AspectEnums.UserLoginStatus.WrongPassword;
//                                }
//                                UpdateUserLoginHistory(history, device.UserID, history != null ? history.LastLoginDate : new Nullable<DateTime>(), lattitude, longitude, isSuccess);

//                            }
//                        }
//                        else
//                        {
//                            loginStatus = AspectEnums.UserLoginStatus.InActive;
//                        }
//                    }
//                //    else
//                //    {
//                //        loginStatus = AspectEnums.UserLoginStatus.WrongIMEI;
//                //    }
//                //}
//                //else
//                //{
//                //    loginStatus = AspectEnums.UserLoginStatus.MultipleIMEI;
//                //}
//                scope.Complete();
//            }
//            return new Tuple<AspectEnums.UserLoginStatus, long, string, int, int, int>(loginStatus, userID, employeeId, companyId, userDeviceID, roleID);
//        }

        /// <summary>
        /// Displays the user profile.
        /// </summary>
        /// <param name="userId">The userId.</param>
        /// <returns></returns>
        public EmployeeMaster DisplayEmpProfile(int userId)
        {

            EmployeeMaster userDetails = new EmployeeMaster();

            if (userId > 0)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
new TransactionOptions
{
    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
}))
                {
                    userDetails = AccuitAdminDbContext.EmployeeMasters.FirstOrDefault(x => x.EmpID == userId);
                    scope.Complete();
                }
            }
            return userDetails;
        }

        /// <summary>
        /// Submit Login History
        /// </summary>
        /// <param name="dailyLoginHistory"></param>
        /// <returns></returns>
        public bool SubmitDailyLoginHistory(DailyLoginHistory dailyLoginHistory)
        {
            AccuitAdminDbContext.DailyLoginHistories.Add(dailyLoginHistory);
            return AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        }

        /// <summary>
        /// Fetch list of Role Master
        /// </summary>
        /// <returns>List of RoleMaster</returns>
        public IEnumerable<RoleMaster> GetRoleMaster()
        {
            return AccuitAdminDbContext.RoleMasters;
        }

        /// <summary>
        /// Update user login history on the basis of user login status and history
        /// </summary>
        /// <param name="history">login history instance</param>
        /// <param name="userID">user ID</param>
        /// <param name="lastLoginDate">last login date</param>
        /// <param name="lattitude">lattitude value</param>
        /// <param name="longitude"> longitude value</param>
        /// <param name="isLogin">is successfull login</param>
        private void UpdateUserLoginHistory(LoginAttemptHistory history, int userID, DateTime? lastLoginDate, string lattitude, string longitude, bool isLogin)
        {
            if (history == null)
                history = new LoginAttemptHistory();
            history.EmpID = userID;
            history.Lattitude = lattitude;
            history.Longitude = longitude;
            history.LoginDate = System.DateTime.Now;
            history.LastLoginDate = isLogin ? System.DateTime.Now : history.LastLoginDate;
            history.FailedAttempt = isLogin ? 0 : history.FailedAttempt + 1;
            if (history.LoginAttemptID == 0)
                AccuitAdminDbContext.LoginAttemptHistories.Add(history);
            else
                AccuitAdminDbContext.Entry<LoginAttemptHistory>(history).State = System.Data.EntityState.Modified;
            AccuitAdminDbContext.SaveChanges();
            if (history.FailedAttempt >= 3)
            {
                EmployeeMaster userDetail = AccuitAdminDbContext.EmployeeMasters.FirstOrDefault(k => k.EmpID == userID);
                if (userDetail != null)
                {
                    userDetail.AccountStatus = (int)AspectEnums.UserLoginStatus.Locked;
                    userDetail.ModifiedDate = System.DateTime.Now;
                    AccuitAdminDbContext.Entry<EmployeeMaster>(userDetail).State = System.Data.EntityState.Modified;
                    AccuitAdminDbContext.SaveChanges();
                }
            }
        }


        public bool LogoutWebUser(int userID, string sessionID)
        {
            bool isSuccess = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }))
            {
                DailyLoginHistory dailyLoginHistory = AccuitAdminDbContext.DailyLoginHistories.FirstOrDefault(k => k.EmpID == userID && k.SessionID == sessionID && k.IsLogin == true);
                if (dailyLoginHistory != null)
                {
                    dailyLoginHistory.IsLogin = false;
                    dailyLoginHistory.LogOutTime = System.DateTime.Now;
                    AccuitAdminDbContext.Entry<DailyLoginHistory>(dailyLoginHistory).State = System.Data.EntityState.Modified;
                    isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
                }
                scope.Complete();
            }
            return isSuccess;
        }

                /// <summary>
        /// increamental download Method to fetch all available user modules on the basis of user primary ID
        /// </summary>
        /// <param name="empID">user primary ID</param>
        /// <returns>returns collection of user modules</returns>
        //public IList<vwGetUserRoleModules> GetUserModules(int empID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate, bool isForsurveyQuestions = false)
        //{
        //    IList<vwGetUserRoleModules> modules = new List<vwGetUserRoleModules>();

        //   // IList<SPGetRoleModules_Result> modulessp = new List<SPGetRoleModules_Result>();

        //    HasMoreRows = false;
        //    MaxModifiedDate = LastUpdatedDate;
        //    DateTime CurrentDateTime = System.DateTime.Now;

        //    if (isForsurveyQuestions)
        //    {
        //        if (LastUpdatedDate == null)
        //        {
        //            modules = AccuitAdminDbContext.vwGetUserRoleModules.Where(k => k.EmpID == empID && k.IsMobile && !k.IsDeleted.Value).ToList();


        //        }
        //        else
        //        {
        //            //modules = SmartDostDbContext.SPGetUserModules(userID, null, true).ToList().Select(x => new vwGetUserRoleModules() { Icon =x.Icon});
        //            modules = AccuitAdminDbContext.vwGetUserRoleModules.Where(k => k.EmpID == empID && k.IsMobile).ToList();

        //        }

        //    }
        //    else
        //    {
        //        if (AccuitAdminDbContext.SPIsDownloadAuthorized(empID, Convert.ToInt32(AspectEnums.DownloadService.Modules)).FirstOrDefault() == true)
        //        {
        //            if (LastUpdatedDate == null)
        //            {
        //                modules = AccuitAdminDbContext.vwGetUserRoleModules.Where(k => k.EmpID == empID && k.IsMobile && !k.IsDeleted) /*delete flag is there for APK user TBD*/
        //                   .OrderBy(k => k.MaxModifiedDate)
        //                   .Skip(StartRowIndex)
        //                   .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not   
        //                // SmartDostDbContext.SPGetRoleModules(roleID, false, true);

        //            }
        //            else
        //            {
        //                modules = AccuitAdminDbContext.vwGetUserRoleModules.Where(k => k.UserID == empID && k.IsMobile  /*delete flag is there for APK user TBD*/
        //                &&
        //                (
        //                (k.MaxModifiedDate > LastUpdatedDate)
        //                ||
        //                (k.MaxModifiedDate == LastUpdatedDate.Value)
        //                )
        //                )
        //                .OrderBy(k => k.MaxModifiedDate)
        //                .Skip(StartRowIndex)
        //                .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                   
        //            }

        //            HasMoreRows = modules.Count > RowCount ? true : false;
        //            modules = modules.Take(RowCount).OrderBy(k => k.Sequence).ThenBy(k => k.Name).ToList();

        //            // Update last modified data among the data if available, else send the same modifieddate back  
        //            if (modules.Count > 0)
        //            {
        //                if (LastUpdatedDate == null && HasMoreRows == true)
        //                    MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
        //                else if (LastUpdatedDate == null && HasMoreRows == false)
        //                    MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
        //                else
        //                    MaxModifiedDate = modules.Max(x => x.MaxModifiedDate);

        //            }

        //        }
        //    }
        //    return modules;
        //}



        //        /// <summary>
        //        /// Get list of User Master 
        //        /// </summary>
        //        /// <param name="userId">The userId.</param>
        //        /// <returns></returns>
        //        public IEnumerable<UserMaster> GetUsersMaster()
        //        {
        //            return AccuitAdminDbContext.UserMasters.Where(k => k.AccountStatus == 1 && k.IsDeleted == false);

        //        }

        //        /// <summary>
        //        /// Updates the user profile.
        //        /// </summary>
        //        /// <param name="userDetail"></param>
        //        /// <returns></returns>
        //        public bool UpdateUserProfile(UserMaster userDetail)
        //        {
        //            bool isSuccess = false;
        //            if (userDetail != null && userDetail.UserID > 0)
        //            {
        //                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //new TransactionOptions
        //{
        //    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //}))
        //                {
        //                    UserMaster user = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == userDetail.UserID);
        //                    if (user != null)
        //                    {
        //                        user.FirstName = userDetail.FirstName;
        //                        user.LastName = userDetail.LastName;
        //                        user.Mobile_Calling = EncryptionEngine.EncryptString(userDetail.Mobile_Calling);
        //                        user.Mobile_SD = EncryptionEngine.EncryptString(userDetail.Mobile_SD);
        //                        user.EmailID = EncryptionEngine.EncryptString(userDetail.EmailID);
        //                        user.AlternateEmailID = EncryptionEngine.EncryptString(userDetail.AlternateEmailID);
        //                        user.Address = userDetail.Address;
        //                        user.Pincode = userDetail.Pincode;
        //                        user.ProfilePictureFileName = !string.IsNullOrEmpty(userDetail.ProfilePictureFileName) ? userDetail.ProfilePictureFileName : user.ProfilePictureFileName;
        //                        user.ModifiedDate = System.DateTime.Now;
        //                        AccuitAdminDbContext.Entry<UserMaster>(user).State = System.Data.EntityState.Modified;
        //                        isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //                    }
        //                    scope.Complete();
        //                }

        //            }
        //            return isSuccess;
        //        }

        //        /// <summary>
        //        /// Method to get user service access entity
        //        /// </summary>
        //        /// <param name="userID">user ID</param>
        //        /// <returns>returns entity instance</returns>
        //        public UserServiceAccess GetUserServiceAccess(long userID)
        //        {
        //            return AccuitAdminDbContext.UserServiceAccesses.FirstOrDefault(k => k.UserID == userID);
        //        }

        //        /// <summary>
        //        /// Method to reset user password in system
        //        /// </summary>
        //        /// <param name="userid">userid</param>
        //        /// <param name="newpassword">new password</param>
        //        /// <returns>returns status</returns>
        //        public AspectEnums.UserResetPasswordStatus ResetUserPassword(string imei, string employeeid, string newpassword)
        //        {
        //            AspectEnums.UserResetPasswordStatus resetstatus = AspectEnums.UserResetPasswordStatus.None;
        //            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //new TransactionOptions
        //{
        //    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //}))
        //            {
        //                UserDevice device = AccuitAdminDbContext.UserDevices.FirstOrDefault(k => k.IMEINumber == imei && !k.IsDeleted);
        //                if (device != null)
        //                {
        //                    UserMaster userDetail = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.EmplCode == employeeid && k.UserID == device.UserID);
        //                    if (userDetail != null)
        //                    {
        //                        UpdateUserPassword(userDetail, employeeid, newpassword);
        //                        LoginAttemptHistory loginHistory = AccuitAdminDbContext.LoginAttemptHistories.FirstOrDefault(k => k.UserID == userDetail.UserID);
        //                        if (loginHistory != null)
        //                        {
        //                            loginHistory.FailedAttempt = 0;
        //                            loginHistory.LastLoginDate = System.DateTime.Now;
        //                            AccuitAdminDbContext.SaveChanges();
        //                        }
        //                        resetstatus = AspectEnums.UserResetPasswordStatus.Reset;


        //                    }
        //                    else
        //                    {
        //                        resetstatus = AspectEnums.UserResetPasswordStatus.WrongEmployeeCode;
        //                    }
        //                }
        //                else
        //                {
        //                    resetstatus = AspectEnums.UserResetPasswordStatus.WrongIMEI;
        //                }
        //                scope.Complete();
        //            }

        //            return resetstatus;
        //        }

        //        /// <summary>
        //        /// Method to validate that user has marked an attendance for mentioned date or not
        //        /// </summary>
        //        /// <param name="userID">User Primary Key</param>
        //        /// <param name="attendanceDate">attendance date</param>
        //        /// <returns>returns boolean status</returns>
        //        public Tuple<bool, int> IsUserHasAttendance(long userID, DateTime attendanceDate)
        //        {
        //            int status = 0;
        //            DateTime date = attendanceDate.Date;

        //            bool isAttendance = AccuitAdminDbContext.UserAttendances.FirstOrDefault(k => k.UserID == userID && EntityFunctions.TruncateTime(k.AttendanceDate) == date && !k.IsDeleted) != null;
        //            if (isAttendance)
        //                status = 1;
        //            if (!isAttendance)
        //            {
        //                isAttendance = AccuitAdminDbContext.UserLeavePlans.FirstOrDefault(k => k.UserID == userID && k.LeaveDate == date && !k.IsDeleted) != null;
        //                if (isAttendance)
        //                    status = 2;
        //            }
        //            return new Tuple<bool, int>(isAttendance, status);
        //        }

        //        /// <summary>
        //        /// Method Used to Add and Update the entry of the user attendance in the DB
        //        /// </summary>
        //        /// <param name="userAttendance">UserAttendance will hold the value inserted by the user at the time of entry</param>
        //        /// <returns></returns>
        //        public int MarkAttendance(UserAttendance userAttendance)
        //        {
        //            // UserAttendance userAttendance = new UserAttendance();
        //            int attendenceStatus = 0;
        //            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        // new TransactionOptions
        // {
        //     IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        // }))
        //            {
        //                UserAttendance attendance = AccuitAdminDbContext.UserAttendances.FirstOrDefault(k => k.UserID == userAttendance.UserID && EntityFunctions.TruncateTime(k.AttendanceDate) == userAttendance.AttendanceDate.Date && k.IsDeleted == false);
        //                if (attendance != null)
        //                {
        //                    attendenceStatus = 2;
        //                }
        //                else
        //                {
        //                    AccuitAdminDbContext.UserAttendances.Add(userAttendance);
        //                    attendenceStatus = AccuitAdminDbContext.SaveChanges();
        //                }
        //                var leaves = AccuitAdminDbContext.UserLeavePlans.Where(k => EntityFunctions.TruncateTime(k.LeaveDate) == userAttendance.AttendanceDate.Date && k.UserID == userAttendance.UserID).ToList();
        //                if (leaves != null)
        //                {
        //                    foreach (var item in leaves)
        //                    {
        //                        AccuitAdminDbContext.UserLeavePlans.Remove(item);
        //                    }
        //                    AccuitAdminDbContext.SaveChanges();
        //                }
        //                scope.Complete();
        //            }
        //            return attendenceStatus;
        //        }

        //        /// <summary>
        //        /// Method Used to Add training details in db
        //        /// </summary>
        //        /// <param name="userid">userid</param>
        //        /// /// <param name="trainingDetails">represent the training detail info</param>
        //        /// /// <param name="trainingDate">represent the training date</param>
        //        /// <returns></returns>
        //        public int AddTrainingDetails(long userID, string trainingDetails, DateTime trainingDate)
        //        {
        //            int status = 0;
        //            var training = AccuitAdminDbContext.OfficeTrainings.FirstOrDefault(k => k.UserID == userID && k.TrainingDate == trainingDate);
        //            if (training == null)
        //            {
        //                AccuitAdminDbContext.OfficeTrainings.Add(new OfficeTraining()
        //                {
        //                    UserID = userID,
        //                    TrainingDetails = trainingDetails,
        //                    TrainingDate = trainingDate,
        //                    CreatedDate = DateTime.Now,
        //                    CreatedBy = userID,
        //                    IsDeleted = false
        //                });

        //                AccuitAdminDbContext.SaveChanges();
        //                status = 1;
        //            }
        //            else
        //            {
        //                status = 2;
        //            }

        //            return status;
        //        }

        //        /// <summary>
        //        /// User to send the geotag image to db
        //        /// </summary>
        //        /// <param name="employeeid">User Id</param>
        //        /// <param name="longitude">Represent longitude of image</param>
        //        /// <param name="latitude">represent latitude of image</param>      
        //        ///  /// <param name="image">represnt the byte array of image</param> 
        //        /// <returns>return true if inserted entry in db else returns false</returns>
        //        public long SaveGeoTag(SurveyResponse storeTag)
        //        {
        //            long response = 0;
        //            SurveyResponse geoTag = null;
        //            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //new TransactionOptions
        //{
        //    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //}))
        //            {
        //                if (AccuitAdminDbContext.SurveyResponses.FirstOrDefault(k => k.CoverageID == storeTag.CoverageID.Value) == null)
        //                {
        //                    geoTag = new SurveyResponse()
        //                    {
        //                        UserID = storeTag.UserID,
        //                        Longitude = storeTag.Longitude,
        //                        Lattitude = storeTag.Lattitude,
        //                        PictureFileName = storeTag.PictureFileName,
        //                        CreatedDate = DateTime.Now.Date,
        //                        BeatPlanDate = storeTag.BeatPlanDate,
        //                        Comments = storeTag.Comments,
        //                        CoverageID = storeTag.CoverageID,
        //                    };
        //                    AccuitAdminDbContext.SurveyResponses.Add(geoTag);
        //                    AccuitAdminDbContext.SaveChanges();
        //                    response = geoTag.SurveyResponseID;
        //                }
        //                else
        //                {
        //                    response = -1;
        //                }
        //                scope.Complete();
        //            }
        //            return response;
        //        }



        //        /// <summary>
        //        /// Method to fetch all available user modules on the basis of user primary ID
        //        /// </summary>
        //        /// <param name="userID">user primary ID</param>
        //        /// <returns>returns collection of user modules</returns>
        //        public IList<vwUserRoleModule> GetUserModules(long userID)
        //        {

        //            IList<vwUserRoleModule> modules = new List<vwUserRoleModule>();
        //            // Amit: survey question based on user profile (24 Sep 2014)
        //            if (AccuitAdminDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.Modules)).FirstOrDefault() == true)
        //            {
        //                var data = AccuitAdminDbContext.vwGetUserRoleModuless.Where(k => k.UserID == userID && k.IsMobile && !k.IsDeleted.Value).OrderBy(k => k.Sequence)
        //                    .GroupBy(k => new { k.ModuleCode, k.ModuleID, k.Name, k.IsMandatory, k.IsMobile, k.ParentModuleID, k.Sequence, k.Icon, k.IsQuestionModule, k.IsStoreWise,k.ModuleType });
        //                var records = data.Select(k => new
        //                {
        //                    ModuleID = k.Key.ModuleID,
        //                    ModuleCode = k.Key.ModuleCode,
        //                    ParentModuleID = k.Key.ParentModuleID,
        //                    Name = k.Key.Name,
        //                    IsMandatory = k.Key.IsMandatory,
        //                    Sequence = k.Key.Sequence,
        //                    icon = k.Key.Icon,
        //                    IsQuestionModule = k.Key.IsQuestionModule,
        //                    IsStoreWise = k.Key.IsStoreWise,
        //                    ModuleType = k.Key.ModuleType

        //                }).ToList();
        //                foreach (var item in records)
        //                {
        //                    vwUserRoleModule module = new vwUserRoleModule()
        //                    {
        //                        ModuleID = item.ModuleID,
        //                        ModuleCode = item.ModuleCode,
        //                        ParentModuleID = item.ParentModuleID,
        //                        Name = item.Name,
        //                        IsMandatory = item.IsMandatory,
        //                        Sequence = item.Sequence,
        //                        Icon = item.icon,
        //                        IsQuestionModule = item.IsQuestionModule,
        //                        IsStoreWise = item.IsStoreWise,
        //                        ModuleType=item.ModuleType


        //                    };
        //                    modules.Add(module);
        //                }

        //                #region commented by vaishali removing logic to calculate parents of modules
        //                //List<int?> moduleIDs = (from k in modules
        //                //                        where k.ParentModuleID != null
        //                //                        select k.ParentModuleID).Distinct().ToList();
        //                //List<int> parentModules = (from k in modules
        //                //                           where !moduleIDs.Contains(k.ParentModuleID)
        //                //                           select k.ModuleID).Distinct().ToList();
        //                //foreach (int id in moduleIDs)
        //                //{
        //                //    var module = SmartDostDbContext.Modules.FirstOrDefault(k => k.ModuleID == id && k.IsMobile && !k.IsDeleted);
        //                //    if (module != null)
        //                //    {
        //                //        if (modules.FirstOrDefault(k => k.ModuleID == module.ModuleID) == null)
        //                //        {
        //                //            modules.Add(new vwUserRoleModule()
        //                //            {
        //                //                UserID = userID,
        //                //                ModuleID = module.ModuleID,
        //                //                ParentModuleID = module.ParentModuleID,
        //                //                Name = module.Name,
        //                //                Sequence = module.Sequence,
        //                //                ModuleCode = module.ModuleCode,
        //                //                Icon = module.Icon,
        //                //                IsQuestionModule = module.IsQuestionModule,
        //                //                IsStoreWise = module.IsStoreWise,
        //                //                ModuleType=module.ModuleType
        //                //            });
        //                //        }
        //                //    }
        //                //}
        //                #endregion
        //                modules = modules.OrderBy(k => k.Sequence).ThenBy(k => k.Name).ToList();
        //            }
        //            return modules;
        //        }

        //        /// <summary>
        //        /// Method to reset the login values after user gets logout
        //        /// </summary>
        //        /// <param name="userID">User ID</param>
        //        /// <returns>returns boolean status</returns>
        //        public bool LogoutUser(long userID)
        //        {
        //            bool isSuccess = false;
        //            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //new TransactionOptions
        //{
        //    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //}))
        //            {
        //                UserServiceAccess serviceDetail = AccuitAdminDbContext.UserServiceAccesses.FirstOrDefault(k => k.UserID == userID);
        //                if (serviceDetail != null)
        //                {
        //                    serviceDetail.APIKey = string.Empty;
        //                    serviceDetail.APIToken = string.Empty;
        //                    serviceDetail.ModifiedDate = System.DateTime.Now;
        //                    serviceDetail.ModifiedBy = userID;
        //                    AccuitAdminDbContext.Entry<UserServiceAccess>(serviceDetail).State = System.Data.EntityState.Modified;
        //                    isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //                }
        //                scope.Complete();
        //            }
        //            return isSuccess;
        //        }

        //        /// <summary>
        //        /// Method to update user leave plans
        //        /// </summary>
        //        /// <param name="leavePlan">user leave plans</param>
        //        /// <returns>returns boolean status</returns>
        //        public bool UpdateUserLeaves(IList<UserLeavePlan> leavePlan)
        //        {

        //            bool isSaved = false;
        //            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //new TransactionOptions
        //{
        //    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //}))
        //            {
        //                foreach (UserLeavePlan plan in leavePlan)
        //                {
        //                    var userLeave = AccuitAdminDbContext.UserLeavePlans.FirstOrDefault(k => k.UserID == plan.UserID && k.LeaveDate == plan.LeaveDate);
        //                    if (userLeave == null)
        //                    {
        //                        AccuitAdminDbContext.UserLeavePlans.Add(plan);
        //                    }
        //                }
        //                isSaved = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //                scope.Complete();
        //            }
        //            return isSaved;
        //        }



        //        /// <summary>
        //        /// Method to logout web user from the application
        //        /// </summary>
        //        /// <param name="userID">The user identifier.</param>
        //        /// <returns></returns>


        //        #region Added By Vinay Kanojia Dated: 19-Dec-2013

        //        public IList<UserRole> GetAllUserRole()
        //        {
        //            return AccuitAdminDbContext.UserRoles.ToList();
        //        }
        //        /// <summary>
        //        /// 
        //        /// </summary>
        //        /// <param name="userName"></param>
        //        /// <returns></returns>
        //        public bool ForgetPasswordWeb(string userName)
        //        {
        //            UserMaster userDetail = default(UserMaster);
        //            userDetail = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.EmplCode == userName);
        //            if (userDetail != null)
        //            {
        //                if (SendPasswordWeb(userDetail))
        //                {
        //                    return true;
        //                }
        //            }
        //            return false;
        //        }

        //        #region Not In Use
        //        /// <summary>
        //        /// MEthod to send Email for Forget Password
        //        /// </summary>
        //        /// <param name="userDetail"></param>
        //        /// <returns></returns>
        //        private bool SendPasswordWeb(UserMaster userDetail)
        //        {
        //            try
        //            {
        //                using (System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage())
        //                {
        //                    var test = AppUtil.GetAppSettings("smtpHost");
        //                    // Sender e-mail address.
        //                    msg.From = new MailAddress(AppUtil.GetAppSettings(AspectEnums.SmtpCredential.fromEmail.ToString()));
        //                    // Recipient e-mail address.
        //                    msg.To.Add(userDetail.EmailID);
        //                    msg.Subject = "Your Password Details";
        //                    msg.Body = "Hi, <br/>Please check your Login Details<br/><br/>Your Username: " + userDetail.FirstName + "<br/><br/>Your Password: " + EncryptionEngine.DecryptString(userDetail.Password) + "<br/><br/>";
        //                    msg.IsBodyHtml = true;
        //                    // your remote SMTP server IP.
        //                    SmtpClient smtp = new SmtpClient();
        //                    smtp.Host = AppUtil.GetAppSettings(AspectEnums.SmtpCredential.smtpHost.ToString());
        //                    smtp.Port = Convert.ToInt32(AppUtil.GetAppSettings(AspectEnums.SmtpCredential.smtpPort.ToString()));
        //                    smtp.Credentials = new System.Net.NetworkCredential(AppUtil.GetAppSettings(AspectEnums.SmtpCredential.smtpUserName.ToString()), AppUtil.GetAppSettings(AspectEnums.SmtpCredential.smtpPassword.ToString()));
        //                    smtp.EnableSsl = false;
        //                    smtp.Send(msg);
        //                    return true;
        //                }
        //            }
        //            catch (Exception)
        //            {
        //                return false;
        //            }
        //        }
        //        #endregion

        //        /// <summary>
        //        /// Method to return user for an employee code
        //        /// </summary>
        //        /// <param name="employeeCode">cemployee Code</param>
        //        /// <returns>returns user</returns>
        //        public UserMaster GetUserByName(string employeeCode)
        //        {
        //            return AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.EmplCode == employeeCode && !k.IsDeleted);
        //        }

        //        #endregion

        //        /// <summary>
        //        /// Method to return user list for a company
        //        /// </summary>
        //        /// <param name="companyID">company primary ID</param>
        //        /// <returns>returns user list</returns>
        //        public IList<UserMaster> GetUsers(int companyID)
        //        {
        //            return AccuitAdminDbContext.UserMasters.Where(k => k.CompanyID == companyID).ToList();
        //        }

        //        /// <summary>
        //        /// Method to return device list for a company
        //        /// </summary>
        //        /// <param name="companyID">company primary ID</param>
        //        /// <returns>returns user list</returns>
        //        public IList<UserDevice> GetUserDevices(long userID)
        //        {
        //            return AccuitAdminDbContext.UserDevices.Where(k => k.UserID == userID && k.IsDeleted == false).ToList();
        //        }

        //        /// <summary>
        //        /// Method to save imei number for an user
        //        /// </summary>
        //        /// <param name="userID">user ID</param>
        //        /// <param name="imeiNumber">imei number</param>
        //        /// <returns>returns boolean status</returns>
        //        public int SaveDeviceIMEI(long userID, string imeiNumber, long createdBy)
        //        {
        //            if (AccuitAdminDbContext.UserDevices.FirstOrDefault(k => k.IMEINumber == imeiNumber && !k.IsDeleted) == null)
        //            {
        //                UserDevice device = new UserDevice()
        //                {
        //                    UserID = userID,
        //                    IMEINumber = imeiNumber,
        //                    IsActive = true,
        //                    CreatedBy = createdBy,
        //                    CreatedDate = System.DateTime.Now,
        //                    IsDeleted = false,
        //                };
        //                AccuitAdminDbContext.UserDevices.Add(device);
        //                return AccuitAdminDbContext.SaveChanges();
        //            }

        //            return -1;
        //        }

        //        /// <summary>
        //        /// Method to delete imei number from database
        //        /// </summary>
        //        /// <param name="imeiNumber">imei number</param>
        //        /// <param name="modifiedBy">modified by</param>
        //        /// <param name="userID">user id</param>
        //        /// <returns>returns boolean status</returns>
        //        public bool DeleteDeviceIMEI(string imeiNumber, long modifiedBy, long userID)
        //        {
        //            var device = AccuitAdminDbContext.UserDevices.FirstOrDefault(k => k.IMEINumber == imeiNumber && !k.IsDeleted && k.UserID == userID);
        //            if (device != null)
        //            {
        //                device.ModifiedBy = modifiedBy;
        //                device.ModifiedDate = System.DateTime.Now;
        //                device.IsDeleted = true;
        //                AccuitAdminDbContext.Entry<UserDevice>(device).State = System.Data.EntityState.Modified;
        //                return AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //            }

        //            return false;
        //        }

        //        /// <summary>
        //        /// Method to fetch all available user modules on the basis of user primary ID
        //        /// </summary>
        //        /// <param name="userID">user primary ID</param>
        //        /// <returns>returns collection of user modules</returns>
        //        public IList<vwUserRoleModule> GetUserWebModules(long userID)
        //        {
        //            IList<vwUserRoleModule> modules = new List<vwUserRoleModule>();
        //            var data = AccuitAdminDbContext.vwGetUserRoleModules.Where(k => k.UserID == userID && !k.IsMobile && !k.IsDeleted).OrderBy(k => k.Sequence)
        //                .GroupBy(k => new { k.ModuleCode, k.ModuleID, k.Name, k.IsMandatory, k.IsMobile, k.ParentModuleID, k.PageURL, k.ModuleType, Icon = k.Icon });
        //            var records = data.Select(k => new
        //            {
        //                ModuleID = k.Key.ModuleID,
        //                ModuleCode = k.Key.ModuleCode,
        //                ParentModuleID = k.Key.ParentModuleID,
        //                Name = k.Key.Name,
        //                IsMandatory = k.Key.IsMandatory,
        //                PageURL = k.Key.PageURL,
        //                ModuleType = k.Key.ModuleType,
        //                Icon = k.Key.Icon
        //            }).ToList();
        //            foreach (var item in records)
        //            {
        //                vwUserRoleModule module = new vwUserRoleModule()
        //                {
        //                    ModuleID = item.ModuleID,
        //                    ModuleCode = item.ModuleCode,
        //                    ParentModuleID = item.ParentModuleID,
        //                    Name = item.Name,
        //                    IsMandatory = item.IsMandatory,
        //                    PageURL = item.PageURL,
        //                    ModuleType = item.ModuleType,
        //                    Icon = item.Icon
        //                };
        //                modules.Add(module);
        //            }
        //            return modules;
        //        }

        //        /// <summary>
        //        /// Method to check whether valid device user
        //        /// </summary>
        //        /// <param name="imeiNumber">device imei number</param>
        //        /// <returns>returns boolean status</returns>
        //        public bool IsValidIMEINumber(string imeiNumber)
        //        {
        //            UserDevice userDeviceDetails = AccuitAdminDbContext.UserDevices.FirstOrDefault(k => k.IMEINumber == imeiNumber && !k.IsDeleted && k.IsActive);
        //            return userDeviceDetails != null;
        //        }

        //        /// <summary>
        //        /// Method to get user details on the basis of imei number
        //        /// </summary>
        //        /// <param name="imeiNumber">device imei number</param>
        //        /// <returns>returns boolean status</returns>
        //        public UserMaster GetUserDetailsByIMEINumber(string imeiNumber)
        //        {
        //            UserDevice userDeviceDetails = AccuitAdminDbContext.UserDevices.FirstOrDefault(k => k.IMEINumber == imeiNumber && !k.IsDeleted && k.IsActive);
        //            if (userDeviceDetails != null)
        //            {
        //                UserMaster userDetails = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == userDeviceDetails.UserID);
        //                return userDetails;
        //            }
        //            return null;
        //        }

        //        /// <summary>
        //        /// Method to get user system settings
        //        /// </summary>
        //        /// <param name="userID">user ID</param>
        //        /// <returns>returns user system settings</returns>
        //        public UserSystemSetting GetUserSystemSettings(long userID)
        //        {
        //            return AccuitAdminDbContext.UserSystemSettings.OrderByDescending(k => k.CoverageExceptionWindow).FirstOrDefault(k => k.UserID == userID && !k.IsDeleted);
        //        }

        //        /// <summary>
        //        /// Method to update android registration id of user profile
        //        /// </summary>
        //        /// <param name="registrationId">registration id</param>
        //        /// <param name="userID">user ID</param>
        //        /// <returns>returns boolean status</returns>
        //        public bool UpdateAndroidRegistrationId(string registrationId, long userID)
        //        {
        //            bool isSuccess = false;
        //            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //new TransactionOptions
        //{
        //    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //}))
        //            {

        //                UserMaster userDetail = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == userID);
        //                if (userDetail != null)
        //                {
        //                    userDetail.AndroidRegistrationId = registrationId;
        //                    userDetail.ModifiedDate = System.DateTime.Now;
        //                    userDetail.ModifiedBy = userID.ToString();
        //                    AccuitAdminDbContext.Entry<UserMaster>(userDetail).State = System.Data.EntityState.Modified;

        //                    isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //                }
        //                scope.Complete();
        //            }
        //            return isSuccess;
        //        }

        //        /// <summary>
        //        /// Method to identify assigned role is admin or not
        //        /// </summary>
        //        /// <param name="roleID">role ID</param>
        //        /// <returns>returns boolean status</returns>
        //        public bool IsAdminRole(int roleID)
        //        {
        //            RoleMaster roleDetail = AccuitAdminDbContext.RoleMasters.FirstOrDefault(k => k.RoleID == roleID);
        //            if (roleDetail != null)
        //                return roleDetail.IsAdmin;
        //            return false;
        //        }

        //        /// <summary>
        //        /// Method to update user's password in web
        //        /// </summary>
        //        /// <param name="userID">user ID</param>
        //        /// <param name="newPassword">new password</param>
        //        /// <returns>returns boolean response</returns>
        //        public bool ResetWebUserPassword(long userID, string newPassword)
        //        {
        //            bool isReset = false;
        //            //using (TransactionScope scope = new TransactionScope())
        //            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //new TransactionOptions
        //{
        //    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //}))

        //            //  using (var t = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions(){ IsolationLevel= IsolationLevel.ReadUncommitted}))
        //            {
        //                UserMaster user = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == userID && !k.IsDeleted);
        //                if (user != null)
        //                {
        //                    user.Password = newPassword;
        //                    user.ModifiedDate = System.DateTime.Now;
        //                    user.ModifiedBy = userID.ToString();
        //                    user.AccountStatus = (int)AspectEnums.UserLoginStatus.Active;
        //                    AccuitAdminDbContext.Entry<UserMaster>(user).State = System.Data.EntityState.Modified;
        //                    AccuitAdminDbContext.SaveChanges();
        //                    LoginAttemptHistory history = AccuitAdminDbContext.LoginAttemptHistories.FirstOrDefault(k => k.UserID == userID);
        //                    if (history != null)
        //                    {
        //                        history.LoginDate = System.DateTime.Now;
        //                        history.FailedAttempt = 0;
        //                        AccuitAdminDbContext.Entry<LoginAttemptHistory>(history).State = System.Data.EntityState.Modified;
        //                        AccuitAdminDbContext.SaveChanges();
        //                    }
        //                    scope.Complete();
        //                    isReset = true;
        //                }
        //            }
        //            return isReset;
        //        }
        
        //        /// <summary>
        //        /// Fetch list of Team Master
        //        /// </summary>
        //        /// <returns>List of RoleMaster</returns>
        //        public IEnumerable<TeamMaster> GetTeamMaster()
        //        {
        //            return AccuitAdminDbContext.TeamMasters.Where(x => x.IsDeleted == false);
        //        }

        //        /// <summary>
        //        /// Method to Reset IsofflineStatus
        //        /// </summary>
        //        /// <param name="userID">userID</param>
        //        /// <returns>returns user</returns>

        //        public bool ManageUserProfile(long userID, bool currentStatus)
        //        {
        //            bool isResetoffline = true;
        //            var profile = AccuitAdminDbContext.UserMasters.Where(k => k.UserID == userID && k.AccountStatus == 1 && k.IsDeleted == false).FirstOrDefault();
        //            if (profile != null)
        //            {
        //                profile.IsOfflineProfile = currentStatus;
        //                ////foreach (var item in profile)
        //                ////{
        //                //    if (item.IsOfflineProfile)
        //                //        item.IsOfflineProfile = false;
        //                //    else
        //                //        item.IsOfflineProfile = true;
        //                AccuitAdminDbContext.Entry<UserMaster>(profile).State = System.Data.EntityState.Modified;
        //                ////}
        //            }

        //            return AccuitAdminDbContext.SaveChanges() > 0 ? (isResetoffline) : false;


        //        }

        //        public bool DeleteUserAttendence(long userID, DateTime selectedDate)
        //        {
        //            bool IsLeaveDeleted = false;
        //            bool IsAttendenceDeleted = false;
        //            //DateTime currentDate = System.DateTime.Today;
        //            var userAtt = AccuitAdminDbContext.UserAttendances.Where(s => s.UserID == userID && EntityFunctions.TruncateTime(s.AttendanceDate) == selectedDate).ToList();
        //            if (userAtt.Count > 0)
        //            {
        //                foreach (var u in userAtt)
        //                {
        //                    AccuitAdminDbContext.UserAttendances.Remove(u);
        //                }
        //                IsAttendenceDeleted = true;
        //            }
        //            var userLeave = AccuitAdminDbContext.UserLeavePlans.Where(s => s.UserID == userID && EntityFunctions.TruncateTime(s.LeaveDate) == selectedDate).ToList();
        //            if (userLeave.Count > 0)
        //            {
        //                foreach (var u in userLeave)
        //                {
        //                    AccuitAdminDbContext.UserLeavePlans.Remove(u);
        //                }
        //                IsLeaveDeleted = true;
        //            }
        //            return AccuitAdminDbContext.SaveChanges() > 0 ? (IsLeaveDeleted || IsAttendenceDeleted) : false;
        //        }
        //        #endregion

        //        /// <summary>
        //        /// Method to get employee code of user
        //        /// </summary>
        //        /// <param name="userID">user ID</param>
        //        /// <returns>returns employee code</returns>
        //        public string GetEmployeeCode(long userID)
        //        {
        //            var userDetails = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == userID);
        //            return userDetails != null ? userDetails.EmplCode : string.Empty;
        //        }

        //        /// <summary>
        //        /// Method to check beat creation allowed or not
        //        /// </summary>
        //        /// <param name="month">month</param>
        //        /// <param name="userID">user ID</param>
        //        /// <returns>returns boolean status</returns>
        //        public bool IsBeatCreationAllowed(int month, long userID, int year)
        //        {
        //            return AccuitAdminDbContext.CoveragePlans.Count(k => k.StatusID == 1 && k.CoverageDate.Month == month && k.UserID == userID && k.CoverageDate.Year == year) == 0;
        //        }

        //        /// <summary>
        //        /// Method to get senior employee name
        //        /// </summary>
        //        /// <param name="userID">user ID</param>
        //        /// <returns>returns employee name</returns>
        //        public string GetSeniorName(long userID)
        //        {
        //            var profile = AccuitAdminDbContext.UserRoles.FirstOrDefault(k => k.UserID == userID && !k.IsDeleted);
        //            if (profile != null)
        //            {
        //                return (profile.UserMaster1 != null) ? String.Format("{0} {1}", profile.UserMaster1.FirstName, profile.UserMaster1.LastName) : string.Empty;
        //            }
        //            return string.Empty;
        //        }

        //        /// <summary>
        //        /// Method to get employee name
        //        /// </summary>
        //        /// <param name="userID">user ID</param>
        //        /// <returns>returns employee name</returns>
        //        public string GetEmployeeName(long userID)
        //        {
        //            var profile = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == userID && !k.IsDeleted);
        //            if (profile != null)
        //                return String.Format("{0} {1}", profile.FirstName, profile.LastName);
        //            return string.Empty;
        //        }

        //        /// <summary>
        //        /// Method to get senior employee ID of provided ID
        //        /// </summary>
        //        /// <param name="userID">userID</param>
        //        /// <returns>returns senior ID</returns>
        //        public long GetSeniorID(long userID)
        //        {
        //            //Changed by Dhiraj SDCE-2460
        //            var profile = (from senior in AccuitAdminDbContext.EmployeeSeniors
        //                           where senior.UserID == userID && senior.TeamID == senior.ReportingTeamID
        //                           orderby senior.ProfileLevel descending
        //                           select senior).FirstOrDefault();

        //            if (profile != null)
        //            {
        //                return profile.ReportingUserID.Value;
        //            }
        //            else
        //            {
        //                var profileOld = AccuitAdminDbContext.UserRoles.FirstOrDefault(k => k.UserID == userID && !k.IsDeleted);
        //                if (profileOld != null)
        //                    return profileOld.UserMaster1 != null ? profileOld.UserMaster1.UserID : 0;
        //            }
        //            return 0;
        //        }
        //        /// <summary>
        //        /// Method to get User Leave plan for a particular date range
        //        /// </summary>
        //        /// <param name="userID">User Id</param>
        //        /// <param name="dtFrom">Date From</param>
        //        /// <param name="dtTo">Date To</param>
        //        /// <returns>List of UserLeavePlan data</returns>
        //        public List<UserLeavePlan> GetUserLeavePlans(long userID, DateTime dtFrom, DateTime dtTo)
        //        {
        //            /*
        //                * AND (
        //                ('start_date' BETWEEN STARTDATE AND ENDDATE) -- caters for inner and end date outer
        //                OR
        //                ('end_date' BETWEEN STARTDATE AND ENDDATE) -- caters for inner and start date outer
        //                OR
        //                (STARTDATE BETWEEN 'start_date' AND 'end_date') -- only one needed for outer range where dates are inside.
        //                ) 
        //             */
        //            return AccuitAdminDbContext.UserLeavePlans.Where(x =>
        //                (x.UserID == userID && !x.IsDeleted)
        //               &&
        //               (
        //                    (
        //                        EntityFunctions.TruncateTime(dtFrom) >= EntityFunctions.TruncateTime(x.LeaveDate) &&
        //                        EntityFunctions.TruncateTime(dtFrom) <= EntityFunctions.TruncateTime(x.LeaveToDate)
        //                    )
        //                    ||
        //                    (
        //                        EntityFunctions.TruncateTime(dtTo) >= EntityFunctions.TruncateTime(x.LeaveDate) &&
        //                        EntityFunctions.TruncateTime(dtTo) <= EntityFunctions.TruncateTime(x.LeaveToDate)
        //                    )
        //                    ||

        //                    (
        //                        EntityFunctions.TruncateTime(x.LeaveDate) >= EntityFunctions.TruncateTime(dtFrom) &&
        //                        EntityFunctions.TruncateTime(x.LeaveDate) <= EntityFunctions.TruncateTime(dtTo)
        //                    )

        //            )
        //        ).ToList();
        //        }
        //        /// <summary>
        //        /// Method to get User Leave plan for a particular date range
        //        /// </summary>
        //        /// <param name="userID">User Id</param>
        //        /// <param name="dtFrom">Date From</param>
        //        /// <param name="dtTo">Date To</param>
        //        /// <returns>List of UserLeavePlan data</returns>
        //        public List<UserLeavePlan> GetUserLeavePlans(DateTime dtFrom, DateTime dtTo)
        //        {
        //            /*
        //                * AND (
        //                ('start_date' BETWEEN STARTDATE AND ENDDATE) -- caters for inner and end date outer
        //                OR
        //                ('end_date' BETWEEN STARTDATE AND ENDDATE) -- caters for inner and start date outer
        //                OR
        //                (STARTDATE BETWEEN 'start_date' AND 'end_date') -- only one needed for outer range where dates are inside.
        //                ) 
        //             */
        //            return AccuitAdminDbContext.UserLeavePlans.Where(x =>
        //                (!x.IsDeleted)
        //               &&
        //               (
        //                    (
        //                        EntityFunctions.TruncateTime(dtFrom) >= EntityFunctions.TruncateTime(x.LeaveDate) &&
        //                        EntityFunctions.TruncateTime(dtFrom) <= EntityFunctions.TruncateTime(x.LeaveToDate)
        //                    )
        //                    ||
        //                    (
        //                        EntityFunctions.TruncateTime(dtTo) >= EntityFunctions.TruncateTime(x.LeaveDate) &&
        //                        EntityFunctions.TruncateTime(dtTo) <= EntityFunctions.TruncateTime(x.LeaveToDate)
        //                    )
        //                    ||

        //                    (
        //                        EntityFunctions.TruncateTime(x.LeaveDate) >= EntityFunctions.TruncateTime(dtFrom) &&
        //                        EntityFunctions.TruncateTime(x.LeaveDate) <= EntityFunctions.TruncateTime(dtTo)
        //                    )

        //            )
        //        ).ToList();
        //        }
        //        #region Private Methods

        //        /// <summary>
        //        /// Method to generate API access tokens for user
        //        /// </summary>
        //        /// <param name="userID">user ID</param>
        //        public void GenerateServiceAccessToken(long userID)
        //        {
        //            UserServiceAccess serviceData = AccuitAdminDbContext.UserServiceAccesses.FirstOrDefault(k => k.UserID == userID);
        //            if (serviceData == null)
        //            {
        //                serviceData = new UserServiceAccess();
        //                serviceData.CreatedDate = System.DateTime.Now;
        //                serviceData.CreatedBy = userID;
        //                serviceData.UserID = userID;
        //            }
        //            else
        //            {
        //                serviceData.ModifiedDate = System.DateTime.Now;
        //                serviceData.ModifiedBy = userID;
        //            }
        //            serviceData.IsActive = true;
        //            serviceData.APIKey = AppUtil.GetUniqueKey();
        //            serviceData.APIToken = DateTime.Now.ToString().GetHashCode().ToString("x");
        //            if (serviceData.ServiceAccessID == 0)
        //            {
        //                AccuitAdminDbContext.UserServiceAccesses.Add(serviceData);
        //            }
        //            else
        //            {
        //                AccuitAdminDbContext.Entry<UserServiceAccess>(serviceData).State = System.Data.EntityState.Modified;
        //            }
        //            AccuitAdminDbContext.SaveChanges();

        //        }

        //        /// <summary>
        //        /// Method to get login days difference from last login date of user
        //        /// </summary>
        //        /// <param name="lastLoginDate">last login date</param>
        //        /// <returns>returns numbers of days difference</returns>
        //        private double GetLoginDayDifference(DateTime lastLoginDate)
        //        {
        //            DateTime currentDate = System.DateTime.Now;
        //            if (currentDate.Month == lastLoginDate.Month)
        //                return currentDate.Day - lastLoginDate.Day;
        //            else
        //            {
        //                return (currentDate - lastLoginDate).TotalDays;
        //            }
        //        }



        //        /// <summary>
        //        /// Method to update user password on the basis of user employeeid
        //        /// </summary>
        //        /// <param name="usermaster">usermaster table instance</param>
        //        /// <param name="employeeid">employee ID</param>
        //        /// <param name="newpassword">New Password</param>
        //        private void UpdateUserPassword(UserMaster usermaster, string employeeid, string newpassword)
        //        {
        //            if (usermaster == null)
        //                usermaster = new UserMaster();
        //            usermaster.Password = newpassword;
        //            usermaster.AccountStatus = 1;
        //            usermaster.IsPinRegistered = true;
        //            usermaster.ModifiedDate = System.DateTime.Now;
        //            AccuitAdminDbContext.SaveChanges();
        //        }

        //        #endregion
        //        public IList<GeoDefinition> GetGeoDefinition(int geoId)
        //        {
        //            List<GeoDefinition> lstGeoDefinition = new List<GeoDefinition>();
        //            var lstGeoDef = AccuitAdminDbContext.GeoDefinitions.Where(x => x.GeoID == geoId && x.IsDeleted==false).ToList();//Deleted check added after MDM by Dhiraj on 18-Aug-2015 SDCE-3790
        //            if (lstGeoDef.Count > 0)
        //            {
        //                lstGeoDefinition.AddRange(lstGeoDef);
        //            }
        //            return lstGeoDefinition;
        //        }


        //        public string GetGeoName(int geoId)
        //        {
        //            var geoName = AccuitAdminDbContext.GeoMasters.Where(x => x.GeoID == geoId).First().Name;
        //            return geoName;
        //        }
        //        public IList<GeoDefinition> GetNextGeoDefinition(int currentGeoDefId, int nextGeoId)
        //        {
        //            List<GeoDefinition> lstGeoDefinition = new List<GeoDefinition>();
        //            var geoID = AccuitAdminDbContext.GeoMasters.FirstOrDefault(x => x.Level == nextGeoId);
        //            var lstGeoDef = AccuitAdminDbContext.GeoDefinitions.Where(x => x.GeoID == geoID.GeoID && x.ParentGeoDefinitionId == currentGeoDefId && x.IsDeleted == false).ToList();//Deleted check added after MDM by Dhiraj on 18-Aug-2015 SDCE-3790
        //            if (lstGeoDef.Count > 0)
        //            {
        //                lstGeoDefinition.AddRange(lstGeoDef);
        //            }
        //            return lstGeoDefinition;
        //        }

        //        public List<GeoMaster> GetGeoMaster(int roleId)
        //        {
        //            RoleMaster usrRole = new RoleMaster();
        //            usrRole = AccuitAdminDbContext.RoleMasters.Where(x => x.RoleID == roleId).FirstOrDefault();
        //            var geoId = usrRole.GeoID.Value;
        //            List<GeoMaster> lstGeoMaster = new List<GeoMaster>();
        //            var geoLevel = AccuitAdminDbContext.GeoMasters.Where(x => x.GeoID == geoId).First().Level.Value;
        //            for (int i = -1; i <= geoLevel; i++)
        //            {
        //                var geoid = AccuitAdminDbContext.GeoMasters.Where(x => x.Level == i).FirstOrDefault();
        //                lstGeoMaster.Add(geoid);
        //            }
        //            return lstGeoMaster;
        //        }




        //        /// <summary>
        //        /// Terminate Old sessions for current user 
        //        /// </summary>
        //        /// <param name="userID"></param>
        //        /// <returns></returns>
        //        public bool TerminateExistingSessions(long userID)
        //        {
        //            bool isSuccess = false;
        //            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //                    new TransactionOptions
        //                    {
        //                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //                    }))
        //            {
        //                //DailyLoginHistory dailyLoginHistory = SmartDostDbContext.DailyLoginHistories.FirstOrDefault(k => k.UserID == userID && k.IsLogin == true);
        //                //if (dailyLoginHistory != null)
        //                //{
        //                //    dailyLoginHistory.IsLogin = false;
        //                //    SmartDostDbContext.Entry<DailyLoginHistory>(dailyLoginHistory).State = System.Data.EntityState.Modified;
        //                //    isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;
        //                //}
        //                var dailyLoginHistories = AccuitAdminDbContext.DailyLoginHistories.Where(k => k.UserID == userID && k.IsLogin == true);
        //                foreach (var item in dailyLoginHistories)
        //                {
        //                    item.IsLogin = false;
        //                    AccuitAdminDbContext.Entry<DailyLoginHistory>(item).State = System.Data.EntityState.Modified;
        //                }
        //                isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //                scope.Complete();
        //            }
        //            return isSuccess;
        //        }


        


        //        /// <summary>
        //        /// Download Master Data codes authorized for selected Role
        //        /// </summary>
        //        /// <param name="roleID"></param>
        //        /// <returns></returns>
        //        public List<string> GetDownloadDataMasterCodes(int roleID)
        //        {
        //            List<string> result = new List<string>();
        //            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //                   new TransactionOptions
        //                   {
        //                       IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //                   }))
        //            {
        //                var authorizedCodes = from d in AccuitAdminDbContext.DownloadDataMasters
        //                                      join auth in AccuitAdminDbContext.DownloadMasterAuthorizations
        //                                      on d.DownloadDataMasterID equals auth.DownloadDataMasterID
        //                                      where auth.RoleID == roleID
        //                                           && d.IsActive && !d.IsDeleted
        //                                           && auth.IsActive && !auth.IsDeleted
        //                                      select d.datacode;
        //                result = authorizedCodes.Distinct().ToList();

        //                scope.Complete();
        //            }
        //            return result;
        //        }


        //        #region Authenticate User Login
        //        /// <summary>
        //        /// Authenticate login crednetials for APK
        //        /// </summary>
        //        /// <param name="imei"></param>
        //        /// <param name="password"></param>
        //        /// <param name="lattitude"></param>
        //        /// <param name="longitude"></param>
        //        /// <param name="BrowserName"></param>
        //        /// <param name="ModelName"></param>
        //        /// <param name="IPaddress"></param>
        //        /// <param name="APKVersion"></param>
        //        /// <param name="LoginType"></param>
        //        /// <returns></returns>
        //        public SPCheckAuthentication_Result AuthenticateUserLogin(string imei, string password, string lattitude, string longitude, string BrowserName, string ModelName, string IPaddress, string APKVersion, byte? LoginType)
        //        {
        //            return AccuitAdminDbContext.SPCheckAuthentication(imei, password, lattitude, longitude, IPaddress, BrowserName, ModelName, APKVersion, LoginType).SingleOrDefault();
        //        }
        //        #endregion

        //        #region Get User Details after authentication
        //        /// <summary>
        //        /// Select User details after authentication
        //        /// </summary>
        //        /// <param name="userID"></param>
        //        /// <param name="APKVersion"></param>
        //        /// <param name="showAnnouncment"></param>
        //        /// <param name="APIKey"></param>
        //        /// <param name="APIToken"></param>
        //        /// <returns></returns>
        //        public SPGetLoginDetails_Result GetLoginDetails
        //            (
        //            long userID,
        //            string APKVersion,
        //            bool? showAnnouncment,
        //            string APIKey,
        //            string APIToken)
        //        {
        //            return AccuitAdminDbContext.SPGetLoginDetails(userID, APKVersion, showAnnouncment, APIKey, APIToken).FirstOrDefault();
        //        }
        //        #endregion
        //        #region Beat Exception performance
        //        /// <summary>
        //        /// Get users on a particular geo
        //        /// </summary>
        //        /// <param name="geoId">Geo Master ID</param>
        //        /// <param name="geoDefId">Geo Definition ID</param>
        //        /// <param name="roleID">Role ID</param>
        //        /// <returns></returns>
        //        public IList<UserMaster> GetBeatExceptionData(int geoId, int geoDefId, int roleID)
        //        {
        //            var currentGeoID = AccuitAdminDbContext.GeoMasters.FirstOrDefault(x => x.Level == geoId);
        //            var users = (from user in AccuitAdminDbContext.UserMasters
        //                        join userrole in AccuitAdminDbContext.UserRoles on user.UserID equals userrole.UserID
        //                         where userrole.IsDeleted == false && userrole.GeoID == currentGeoID.GeoID && userrole.GeoLevelValue == geoDefId && userrole.RoleID == roleID
        //                        && user.IsDeleted==false
        //                        select user).Distinct();
        //            return users.ToList();
        //        }
        //        #endregion

    }
}
