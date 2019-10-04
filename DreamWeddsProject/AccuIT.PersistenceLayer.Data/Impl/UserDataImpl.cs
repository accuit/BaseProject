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
using AccuIT.CommonLayer.Aspects.Logging;
using System.Reflection;


namespace AccuIT.PersistenceLayer.Data.Impl
{
    /// <summary>
    /// User Data Layer implementation
    /// </summary>
    public class UserDataImpl : BaseDataImpl, IUserRepository
    {

        /// <summary>
        /// Select active login for current user
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="LoginType"></param>
        /// <returns></returns>
        public DailyLoginHistory GetActiveLogin(int userid, int LoginType)
        {
            try
            {
                return AccuitAdminDbContext.DailyLoginHistories.Where(x => x.UserID == userid && x.IsLogin == true && x.LoginType == LoginType).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //        #region Public Methods

        /// <summary>
        /// This method is used to validate user credentials.
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <returns>returns User Entity</returns>
        public User ValidateUser(string userName, string password)
        {
            throw new NotImplementedException();
        }

        ///<summary>
        ///Method to login web user into the application
        ///</summary>
        ///<param name="userName">user name</param>
        ///<param name="password">password</param>
        ///<returns>returns login status</returns>
        public int LoginWebUser(string userName, string password)
        {
            UserMaster empDetail = default(UserMaster);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
new TransactionOptions
{
    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
}))
            {
                try
                {
                    empDetail = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.LoginName == userName && !k.isDeleted);


                    if (empDetail != null)
                    {
                        LoginAttemptHistory history = AccuitAdminDbContext.LoginAttemptHistories.FirstOrDefault(k => k.UserID == empDetail.UserID);
                        if (empDetail.Password == password)
                        {
                            if (empDetail.AccountStatus == (int)AspectEnums.UserLoginStatus.Active)
                            {

                                UpdateUserLoginHistory(history, empDetail.UserID, System.DateTime.Now, string.Empty, string.Empty, true);
                                if (empDetail.UserRoles.Where(k => k.IsActive == true).ToList().Count > 0)
                                {

                                    bool isAdmin = empDetail.UserRoles.Where(k => k.IsActive == true).ToList()[0].RoleMaster.IsAdmin ? true : false;
                                    if (!isAdmin)
                                    {
                                        bool isWebUser = GetUserWebModules(empDetail.UserID).Count > 0 ? true : false;
                                        if (isWebUser)
                                        {
                                            scope.Complete();
                                            return empDetail.UserID;
                                        }

                                        scope.Complete();
                                        return (int)AspectEnums.UserLoginAttemptStatus.InvalidWebUser;
                                    }
                                    else
                                    {
                                        scope.Complete();
                                        return empDetail.UserID;
                                    }
                                }
                            }
                            else if (empDetail.AccountStatus == (int)AspectEnums.UserLoginStatus.InActive)
                            {
                                scope.Complete();
                                return (int)AspectEnums.UserLoginAttemptStatus.InActive;
                            }
                            else if (empDetail.AccountStatus == (int)AspectEnums.UserLoginStatus.Locked)
                            {
                                scope.Complete();
                                return (int)AspectEnums.UserLoginAttemptStatus.Locked;
                            }
                            else
                            {
                                scope.Complete();
                                return (int)AspectEnums.UserLoginAttemptStatus.InActive;
                            }

                        }
                        else
                        {
                            UpdateUserLoginHistory(history, empDetail.UserID, System.DateTime.Now, string.Empty, string.Empty, false);
                            scope.Complete();
                            return (int)AspectEnums.UserLoginAttemptStatus.WrongPassword;
                        }
                    }

                    scope.Complete();
                }

                catch (Exception ex)
                {

                    ActivityLog.SetLog("Exception IN <" + this.GetType().Name + "> IN <<" + MethodBase.GetCurrentMethod().Name + ">>: Message| " + ex.Message, LogLoc.ERROR);
                    throw ex;
                }
            }
            return (int)AspectEnums.UserLoginAttemptStatus.WrongUserId;
        }

        public int SubmitNewEmployee(UserMaster model, string sessionID)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
            }))
            {
                model.CreatedDate = DateTime.Now;
                model.Email = model.LoginName;
                AccuitAdminDbContext.UserMasters.Add(model);
                AccuitAdminDbContext.SaveChanges();

                if (model.IsEmployee == false && model.UserID > 0)
                {
                    AccuitAdminDbContext.UserRoles.Add(new UserRole
                    {
                        UserID = model.UserID,
                        isDeleted = false,
                        RoleID = (int)AspectEnums.RoleType.Customer,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedBy = model.UserID
                    });
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
                else
                {
                    AccuitAdminDbContext.UserRoles.Add(new UserRole
                    {
                        UserID = model.UserID,
                        isDeleted = false,
                        RoleID = (int)AspectEnums.RoleType.Employee,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        CreatedBy = model.UserID
                    });
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }

            }
            return model.UserID;
        }

        public UserMaster GetUserByLoginName(string loginName)
        {
            try
            {
                loginName = EncryptionEngine.EncryptString(loginName);
                var user = AccuitAdminDbContext.UserMasters.Where(x => x.Email == loginName || x.LoginName == loginName).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                ActivityLog.SetLog("Exception IN <" + this.GetType().Name + "> IN <<" + MethodBase.GetCurrentMethod().Name + ">>: Message| " + ex.Message, LogLoc.ERROR);
                throw ex;
            }
        }





        /// <summary>
        /// Method to authenticate user login using IMEI number
        /// </summary>
        /// <param name="imei">mobile imei number</param>
        /// <param name="password">user's login password</param>
        /// <param name="geoTag">geo tag value</param>
        /// <returns>returns status code</returns>
        public Tuple<AspectEnums.UserLoginStatus, int, string, int, int, int> AuthenticateUser(string imei, string LoginName, string password, string lattitude, string longitude, string BrowserName, string ModelName, string IPaddress)
        {

            int userID = 0;
            int companyId = 0;
            string employeeId = string.Empty;
            int userDeviceID = 0;
            int roleID = 0;
            AspectEnums.UserLoginStatus loginStatus = AspectEnums.UserLoginStatus.None;
            bool isSuccess = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
new TransactionOptions
{
    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
}))
            {
                //UserDevice device = devices.FirstOrDefault(k => k.IMEINumber == imei && !k.IsDeleted);
                UserMaster userDetail = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.LoginName == LoginName);
                if (userDetail != null)
                {                   
                    LoginAttemptHistory history = AccuitAdminDbContext.LoginAttemptHistories.FirstOrDefault(k => k.UserID == userDetail.UserID);
                    if (!userDetail.isDeleted)
                    {
                        loginStatus = AppUtil.NumToEnum<AspectEnums.UserLoginStatus>(Convert.ToInt32(userDetail.AccountStatus));
                        if (loginStatus == AspectEnums.UserLoginStatus.Active)
                        {
                            SystemSetting settings = new SystemDataImpl().GetCompanySystemSettings(userDetail.CompanyId);
                            int expiringDays = settings != null ? settings.IdleSystemDay : 5;
                            if (userDetail.Password == password)
                            {
                                if (history != null && history.LastLoginDate != null)
                                {
                                    if (GetLoginDayDifference(history.LastLoginDate.Value) > expiringDays)
                                        loginStatus = AspectEnums.UserLoginStatus.DaysExpire;

                                }
                                isSuccess = loginStatus == AspectEnums.UserLoginStatus.Active ? true : false;
                                if (isSuccess)
                                {
                                    userID = userDetail.UserID;
                                    employeeId = userDetail.EmpCode;
                                    companyId = userDetail.CompanyId;
                                    GenerateServiceAccessToken(userDetail.UserID);
                                   // userDeviceID = device.UserDeviceID;
                                    roleID = userDetail.UserRoles.Where(k => !k.isDeleted).ToList().Count > 0 ? userDetail.UserRoles.Where(k => !k.isDeleted).ToList()[0].RoleID : 0;
                                }
                            }
                            else
                            {
                                loginStatus = AspectEnums.UserLoginStatus.WrongPassword;
                            }
                            UpdateUserLoginHistory(history, Convert.ToInt32(userDetail.UserID), history != null ? history.LastLoginDate : new Nullable<DateTime>(), lattitude, longitude, isSuccess);

                        }
                    }
                    else
                    {
                        loginStatus = AspectEnums.UserLoginStatus.InActive;
                    }
                }
                else
                {
                    loginStatus = AspectEnums.UserLoginStatus.WrongUserName;
                }

                scope.Complete();
            }
            return new Tuple<AspectEnums.UserLoginStatus, int, string, int, int, int>(loginStatus, userID, employeeId, companyId, userDeviceID, roleID);
        }

        /// <summary>
        /// Displays the user profile.
        /// </summary>
        /// <param name="userId">The userId.</param>
        /// <returns></returns>
        public UserMaster DisplayUserProfile(int userId)
        {

            UserMaster userDetail = new UserMaster();
            try
            {
                if (userId > 0)
                {
                    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }))
                    {
                        AccuitAdminDbContext.Configuration.LazyLoadingEnabled = false;
                        userDetail = AccuitAdminDbContext.UserMasters
                            .Include("UserRoles")
                            .FirstOrDefault(x => x.UserID == userId);
                        scope.Complete();
                    }
                }
                return userDetail;
            }
            catch (Exception ex)
            {
                ActivityLog.SetLog("Exception IN <" + this.GetType().Name + "> IN <<" + MethodBase.GetCurrentMethod().Name + ">>: Message| " + ex.Message, LogLoc.ERROR);
                throw ex;
            }
        }



        /// <summary>
        /// Submit Login History
        /// </summary>
        /// <param name="dailyLoginHistory"></param>
        /// <returns></returns>
        public bool SubmitDailyLoginHistory(DailyLoginHistory dailyLoginHistory)
        {
            try
            {
                AccuitAdminDbContext.DailyLoginHistories.Add(dailyLoginHistory);
                return AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
            }
            catch (Exception ex)
            {
                ActivityLog.SetLog("Exception IN <" + this.GetType().Name + "> IN <<" + MethodBase.GetCurrentMethod().Name + ">>: Message| " + ex.Message, LogLoc.ERROR);
                throw ex;
            }

        }

        /// <summary>
        /// Fetch list of Role Master
        /// </summary>
        /// <returns>List of RoleMaster</returns>
        public IEnumerable<RoleMaster> GetRoleMaster()
        {
            try
            {
                return AccuitAdminDbContext.RoleMasters;
            }
            catch (Exception ex)
            {
                ActivityLog.SetLog("Exception IN <" + this.GetType().Name + "> IN <<" + MethodBase.GetCurrentMethod().Name + ">>: Message| " + ex.Message, LogLoc.ERROR);
                throw ex;
            }

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
            history.UserID = userID;
            history.Lattitude = lattitude;
            history.Longitude = longitude;
            history.LoginDate = System.DateTime.Now;
            history.LastLoginDate = isLogin ? System.DateTime.Now : history.LastLoginDate;
            history.FailedAttempt = isLogin ? 0 : history.FailedAttempt + 1;
            if (history.LoginAttemptID == 0)
                AccuitAdminDbContext.LoginAttemptHistories.Add(history);
            else
                AccuitAdminDbContext.Entry<LoginAttemptHistory>(history).State = System.Data.Entity.EntityState.Modified;
            AccuitAdminDbContext.SaveChanges();
            if (history.FailedAttempt >= 3)
            {
                UserMaster userDetail = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == userID);
                if (userDetail != null)
                {
                    userDetail.AccountStatus = (int)AspectEnums.UserLoginStatus.Locked;
                    userDetail.ModifiedDate = System.DateTime.Now;
                    AccuitAdminDbContext.Entry<UserMaster>(userDetail).State = System.Data.Entity.EntityState.Modified;
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
                try
                {
                    DailyLoginHistory dailyLoginHistory = AccuitAdminDbContext.DailyLoginHistories.FirstOrDefault(k => k.UserID == userID && k.SessionID == sessionID && k.IsLogin == true);
                    if (dailyLoginHistory != null)
                    {
                        dailyLoginHistory.IsLogin = false;
                        dailyLoginHistory.LogOutTime = System.DateTime.Now;
                        AccuitAdminDbContext.Entry<DailyLoginHistory>(dailyLoginHistory).State = System.Data.Entity.EntityState.Modified;
                        isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
                    }
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    ActivityLog.SetLog("Exception IN <" + this.GetType().Name + "> IN <<" + MethodBase.GetCurrentMethod().Name + ">>: Message| " + ex.Message, LogLoc.ERROR);
                    throw ex;
                }
            }
            return isSuccess;
        }

        //        /// <summary>
        //        /// Terminate Old sessions for current user 
        //        /// </summary>
        //        /// <param name="userID"></param>
        //        /// <returns></returns>
        public bool TerminateExistingSessions(int UserID)
        {
            bool isSuccess = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }))
            {
                var dailyLoginHistories = AccuitAdminDbContext.DailyLoginHistories.Where(k => k.UserID == UserID && k.IsLogin == true);
                foreach (var item in dailyLoginHistories)
                {
                    item.IsLogin = false;
                    AccuitAdminDbContext.Entry<DailyLoginHistory>(item).State = System.Data.Entity.EntityState.Modified;
                }
                isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
                scope.Complete();
            }
            return isSuccess;
        }

        /// <summary>
        /// increamental download Method to fetch all available user modules on the basis of user primary ID
        /// </summary>
        /// <param name="UserID">user primary ID</param>
        /// <returns>returns collection of user modules</returns>
        public IList<vwGetUserRoleModule> GetUserModules(int UserID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate, bool isForsurveyQuestions = false)
        {
            IList<vwGetUserRoleModule> modules = new List<vwGetUserRoleModule>();

            // IList<SPGetRoleModules_Result> modulessp = new List<SPGetRoleModules_Result>();

            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;

            if (LastUpdatedDate == null)
            {
                modules = AccuitAdminDbContext.vwGetUserRoleModules.Where(k => k.UserID == UserID && !k.IsDeleted).ToList();


            }
            else
            {
                //modules = SmartDostDbContext.SPGetUserModules(userID, null, true).ToList().Select(x => new vwGetUserRoleModules() { Icon =x.Icon});
                modules = AccuitAdminDbContext.vwGetUserRoleModules.Where(k => k.UserID == UserID && !k.IsDeleted).ToList();

            }

            return modules;
        }

        /// <summary>
        /// Method to fetch all available user modules on the basis of user primary ID
        /// </summary>
        /// <param name="userID">user primary ID</param>
        /// <returns>returns collection of user modules</returns>
        public IList<vwGetUserRoleModule> GetUserModules(int userID)
        {

            IList<vwGetUserRoleModule> modules = new List<vwGetUserRoleModule>();
            // Amit: survey question based on user profile (24 Sep 2014)

            var data = AccuitAdminDbContext.vwGetUserRoleModules.Where(k => k.UserID == userID && k.IsMobile && !k.IsDeleted).OrderBy(k => k.Sequence)
                .GroupBy(k => new { k.ModuleCode, k.ModuleID, k.Name, k.IsMandatory, k.IsMobile, k.ParentModuleID, k.Sequence, k.Icon, k.ModuleType });
            var records = data.Select(k => new
            {
                ModuleID = k.Key.ModuleID,
                ModuleCode = k.Key.ModuleCode,
                ParentModuleID = k.Key.ParentModuleID,
                Name = k.Key.Name,
                IsMandatory = k.Key.IsMandatory,
                Sequence = k.Key.Sequence,
                icon = k.Key.Icon,

                ModuleType = k.Key.ModuleType

            }).ToList();
            foreach (var item in records)
            {
                vwGetUserRoleModule module = new vwGetUserRoleModule()
                {
                    ModuleID = item.ModuleID,
                    ModuleCode = item.ModuleCode,
                    ParentModuleID = item.ParentModuleID,
                    Name = item.Name,
                    IsMandatory = item.IsMandatory,
                    Sequence = item.Sequence,
                    Icon = item.icon,
                    ModuleType = item.ModuleType
                };
                modules.Add(module);
            }


            modules = modules.OrderBy(k => k.Sequence).ThenBy(k => k.Name).ToList();

            return modules;
        }



        //        /// <summary>
        //        /// Get list of User Master 
        //        /// </summary>
        //        /// <param name="userId">The userId.</param>
        //        /// <returns></returns>
        //        public IEnumerable<UserMaster> GetUsersMaster()
        //        {
        //            return AccuitAdminDbContext.UserMasters.Where(k => k.AccountStatus == 1 && k.IsDeleted == false);

        //        }

        /// <summary>
        /// Updates the user profile.
        /// </summary>
        /// <param name="userDetail"></param>
        /// <returns></returns>
        public bool UpdateUserProfile(UserMaster userDetail)
        {
            bool isSuccess = false;
            if (userDetail != null && userDetail.UserID > 0)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions
                {
                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                }))
                {
                    UserMaster user = AccuitAdminDbContext.UserMasters.Where(k => k.UserID == userDetail.UserID && k.isActive == true && k.isDeleted == false).FirstOrDefault();
                    // List<AddressMaster> address = AccuitAdminDbContext.AddressMasters.Where(a => a.AddressOwnerType == (int)AspectEnums.AddressOwnerType.Employee && a.AddressOwnerTypePKID == userDetail.UserID).ToList();
                    if (userDetail != null)
                    {
                        user.FirstName = userDetail.FirstName;
                        user.LastName = userDetail.LastName;
                        user.Mobile = userDetail.Mobile;
                        user.Phone = userDetail.Phone;
                        user.Email = userDetail.Email;
                        user.ImagePath = !string.IsNullOrEmpty(userDetail.ImagePath) ? userDetail.ImagePath : user.ImagePath;
                        user.ModifiedDate = System.DateTime.Now;
                        user.ModifiedBy = userDetail.UserID;
                        //user.Password = userDetail.Password;
                        //user.AccountStatus = userDetail.AccountStatus;
                        AccuitAdminDbContext.Entry<UserMaster>(user).State = System.Data.Entity.EntityState.Modified;
                        isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
                    }
                    scope.Complete();
                }

            }
            return isSuccess;
        }

        /// <summary>
        /// Method to get user service access entity
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns entity instance</returns>
        public UserServiceAccess GetUserServiceAccess(int userID)
        {
            return AccuitAdminDbContext.UserServiceAccesses.FirstOrDefault(k => k.UserID == userID);
        }

        /// <summary>
        /// Method to reset user password in system
        /// </summary>
        /// <param name="userid">userid</param>
        /// <param name="newpassword">new password</param>
        /// <returns>returns status</returns>
        public AspectEnums.UserResetPasswordStatus ResetUserPassword(string imei, string loginName, string newpassword)
        {
            AspectEnums.UserResetPasswordStatus resetstatus = AspectEnums.UserResetPasswordStatus.None;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
new TransactionOptions
{
    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
}))
            {
                
                    UserMaster userDetail = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.LoginName == loginName && !k.isDeleted);
                    if (userDetail != null)
                    {
                        UpdateUserPassword(userDetail, loginName, newpassword);
                        LoginAttemptHistory loginHistory = AccuitAdminDbContext.LoginAttemptHistories.FirstOrDefault(k => k.UserID == userDetail.UserID);
                        if (loginHistory != null)
                        {
                            loginHistory.FailedAttempt = 0;
                            loginHistory.LastLoginDate = System.DateTime.Now;
                            AccuitAdminDbContext.SaveChanges();
                        }
                        resetstatus = AspectEnums.UserResetPasswordStatus.Reset;


                    }
                    else
                    {
                        resetstatus = AspectEnums.UserResetPasswordStatus.WrongLoginName;
                    }
                
                scope.Complete();
            }

            return resetstatus;
        }

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




        /// <summary>
        /// Method to reset the login values after user gets logout
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>returns boolean status</returns>
        public bool LogoutUser(int userID)
        {
            bool isSuccess = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
new TransactionOptions
{
    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
}))
            {
                UserServiceAccess serviceDetail = AccuitAdminDbContext.UserServiceAccesses.FirstOrDefault(k => k.UserID == userID);
                if (serviceDetail != null)
                {
                    serviceDetail.APIKey = string.Empty;
                    serviceDetail.APIToken = string.Empty;
                    serviceDetail.ModifiedDate = System.DateTime.Now;
                    serviceDetail.ModifiedBy = userID;
                    AccuitAdminDbContext.Entry<UserServiceAccess>(serviceDetail).State = System.Data.Entity.EntityState.Modified;
                    isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
                }
                scope.Complete();
            }
            return isSuccess;
        }

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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ForgetPasswordWeb(string userName)
        {
            UserMaster userDetail = default(UserMaster);
            userDetail = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.LoginName == userName);
            if (userDetail != null)
            {
                if (SendPasswordWeb(userDetail))
                {
                    return true;
                }
            }
            return false;
        }

        #region Not In Use
        /// <summary>
        /// MEthod to send Email for Forget Password
        /// </summary>
        /// <param name="userDetail"></param>
        /// <returns></returns>
        private bool SendPasswordWeb(UserMaster userDetail)
        {
            try
            {
                using (System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage())
                {
                    var test = AppUtil.GetAppSettings("smtpHost");
                    // Sender e-mail address.
                    msg.From = new MailAddress(AppUtil.GetAppSettings(AspectEnums.SmtpCredential.fromEmail.ToString()));
                    // Recipient e-mail address.
                    msg.To.Add(userDetail.Email);
                    msg.Subject = "Your Password Details";
                    msg.Body = "Hi, <br/>Please check your Login Details<br/><br/>Your Username: " + userDetail.FirstName + "<br/><br/>Your Password: " + EncryptionEngine.DecryptString(userDetail.Password) + "<br/><br/>";
                    msg.IsBodyHtml = true;
                    // your remote SMTP server IP.
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = AppUtil.GetAppSettings(AspectEnums.SmtpCredential.smtpHost.ToString());
                    smtp.Port = Convert.ToInt32(AppUtil.GetAppSettings(AspectEnums.SmtpCredential.smtpPort.ToString()));
                    smtp.Credentials = new System.Net.NetworkCredential(AppUtil.GetAppSettings(AspectEnums.SmtpCredential.smtpUserName.ToString()), AppUtil.GetAppSettings(AspectEnums.SmtpCredential.smtpPassword.ToString()));
                    smtp.EnableSsl = false;
                    smtp.Send(msg);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// Method to return user for an employee code
        /// </summary>
        /// <param name="employeeCode">cemployee Code</param>
        /// <returns>returns user</returns>
        public UserMaster GetUserByName(string loginName)
        {
            return AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.LoginName == loginName && !k.isDeleted);
        }

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
        //                AccuitAdminDbContext.Entry<UserDevice>(device).State = System.Data.Entity.EntityState.Modified;
        //                return AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //            }

        //            return false;
        //        }

        /// <summary>
        /// Method to fetch all available user modules on the basis of user primary ID
        /// </summary>
        /// <param name="userID">user primary ID</param>
        /// <returns>returns collection of user modules</returns>
        public IList<vwGetUserRoleModule> GetUserWebModules(int userID)
        {
            IList<vwGetUserRoleModule> modules = new List<vwGetUserRoleModule>();
            try
            {

                var data = AccuitAdminDbContext.vwGetUserRoleModules.Where(k => k.UserID == userID && !k.IsDeleted).OrderBy(k => k.Sequence)
                    .GroupBy(k => new { k.ModuleCode, k.ModuleID, k.Name, k.IsMandatory, k.ParentModuleID, k.PageURL, k.ModuleType, Icon = k.Icon });
                var records = data.Select(k => new
                {
                    ModuleID = k.Key.ModuleID,
                    ModuleCode = k.Key.ModuleCode,
                    ParentModuleID = k.Key.ParentModuleID,
                    Name = k.Key.Name,
                    IsMandatory = k.Key.IsMandatory,
                    PageURL = k.Key.PageURL,
                    ModuleType = k.Key.ModuleType,
                    Icon = k.Key.Icon
                }).ToList();
                foreach (var item in records)
                {
                    vwGetUserRoleModule module = new vwGetUserRoleModule()
                    {
                        ModuleID = item.ModuleID,
                        ModuleCode = item.ModuleCode,
                        ParentModuleID = item.ParentModuleID,
                        Name = item.Name,
                        IsMandatory = item.IsMandatory,
                        PageURL = item.PageURL,
                        ModuleType = item.ModuleType,
                        Icon = item.Icon
                    };
                    modules.Add(module);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return modules;
        }

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
        //                    AccuitAdminDbContext.Entry<UserMaster>(userDetail).State = System.Data.Entity.EntityState.Modified;

        //                    isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //                }
        //                scope.Complete();
        //            }
        //            return isSuccess;
        //        }

        /// <summary>
        /// Method to identify assigned role is admin or not
        /// </summary>
        /// <param name="roleID">role ID</param>
        /// <returns>returns boolean status</returns>
        public bool IsAdminRole(int roleID)
        {
            RoleMaster roleDetail = AccuitAdminDbContext.RoleMasters.FirstOrDefault(k => k.RoleID == roleID);
            if (roleDetail != null)
                return roleDetail.IsAdmin;
            return false;
        }

        /// <summary>
        /// Method to update user's password in web
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <param name="newPassword">new password</param>
        /// <returns>returns boolean response</returns>
        public bool ResetWebUserPassword(int userID, string newPassword)
        {
            bool isReset = false;
            //using (TransactionScope scope = new TransactionScope())
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
new TransactionOptions
{
    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
}))

            //  using (var t = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions(){ IsolationLevel= IsolationLevel.ReadUncommitted}))
            {
                UserMaster user = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == userID && !k.isDeleted);
                if (user != null)
                {
                    user.Password = newPassword;
                    user.ModifiedDate = System.DateTime.Now;
                    user.ModifiedBy = userID;
                    user.AccountStatus = (int)AspectEnums.UserLoginStatus.Active;
                    AccuitAdminDbContext.Entry<UserMaster>(user).State = System.Data.Entity.EntityState.Modified;
                    AccuitAdminDbContext.SaveChanges();
                    LoginAttemptHistory history = AccuitAdminDbContext.LoginAttemptHistories.FirstOrDefault(k => k.UserID == userID);
                    if (history != null)
                    {
                        history.LoginDate = System.DateTime.Now;
                        history.FailedAttempt = 0;
                        AccuitAdminDbContext.Entry<LoginAttemptHistory>(history).State = System.Data.Entity.EntityState.Modified;
                        AccuitAdminDbContext.SaveChanges();
                    }
                    scope.Complete();
                    isReset = true;
                }
            }
            return isReset;
        }

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
        //                AccuitAdminDbContext.Entry<UserMaster>(profile).State = System.Data.Entity.EntityState.Modified;
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

        /// <summary>
        /// Method to generate API access tokens for user
        /// </summary>
        /// <param name="userID">user ID</param>
        public void GenerateServiceAccessToken(int userID)
        {
            UserServiceAccess serviceData = AccuitAdminDbContext.UserServiceAccesses.FirstOrDefault(k => k.UserID == userID);
            if (serviceData == null)
            {
                serviceData = new UserServiceAccess();
                serviceData.CreatedDate = System.DateTime.Now;
                serviceData.CreatedBy = userID;
                serviceData.UserID = userID;
            }
            else
            {
                serviceData.ModifiedDate = System.DateTime.Now;
                serviceData.ModifiedBy = userID;
            }
            serviceData.IsActive = true;
            serviceData.APIKey = AppUtil.GetUniqueKey();
            serviceData.APIToken = DateTime.Now.ToString().GetHashCode().ToString("x");
            if (serviceData.ServiceAccessID == 0)
            {
                AccuitAdminDbContext.UserServiceAccesses.Add(serviceData);
            }
            else
            {
                AccuitAdminDbContext.Entry<UserServiceAccess>(serviceData).State = System.Data.Entity.EntityState.Modified;
            }
            AccuitAdminDbContext.SaveChanges();

        }

        /// <summary>
        /// Method to get login days difference from last login date of user
        /// </summary>
        /// <param name="lastLoginDate">last login date</param>
        /// <returns>returns numbers of days difference</returns>
        private double GetLoginDayDifference(DateTime lastLoginDate)
        {
            DateTime currentDate = System.DateTime.Now;
            if (currentDate.Month == lastLoginDate.Month)
                return currentDate.Day - lastLoginDate.Day;
            else
            {
                return (currentDate - lastLoginDate).TotalDays;
            }
        }



        /// <summary>
        /// Method to update user password on the basis of user employeeid
        /// </summary>
        /// <param name="usermaster">usermaster table instance</param>
        /// <param name="loginName">employee ID</param>
        /// <param name="newpassword">New Password</param>
        private void UpdateUserPassword(UserMaster usermaster, string loginName, string newpassword)
        {
            if (usermaster == null)
                usermaster = new UserMaster();
            usermaster.Password = newpassword;
            usermaster.AccountStatus = 1;
            // usermaster.IsPinRegistered = true;
            usermaster.ModifiedDate = System.DateTime.Now;
            AccuitAdminDbContext.Entry<UserMaster>(usermaster).State = System.Data.Entity.EntityState.Modified;
            AccuitAdminDbContext.SaveChanges();
        }

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
        public SPCheckAuthentication_Result AuthenticateUserLogin(string imei, string LoginName, string password, string lattitude, string longitude, string BrowserName, string ModelName, string IPaddress, string APKVersion, byte? LoginType)
        {
            return AccuitAdminDbContext.SPCheckAuthentication(LoginName, password, imei, lattitude, longitude, IPaddress, BrowserName, ModelName, APKVersion, LoginType).SingleOrDefault();
        }
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
        public SPGetLoginDetails_Result GetLoginDetails
            (
            int userID,
            bool? showAnnouncment,
            string APIKey,
            string APIToken,
             string APKVersion)
        {
            return AccuitAdminDbContext.SPGetLoginDetails(Convert.ToInt32(userID), showAnnouncment, APIKey, APIToken, APKVersion).FirstOrDefault();
        }
        #endregion
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
