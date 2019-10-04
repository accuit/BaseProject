using AccuIT.CommonLayer.Aspects.Security;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.PersistenceLayer.Repository.Contracts;
using AccuIT.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuIT.PersistenceLayer.Data.Impl
{
    /// <summary>
    /// Implementation class for system/user authorization and security in application
    /// </summary>
    public class SecurityDataImpl : BaseDataImpl, ISecurityRepository
    {


        /// <summary>
        /// Authenticate OTP (One Time Password) entered by user
        /// </summary>
        /// <param name="userid">Userid</param>
        /// <param name="otp">One Time Password</param>
        /// <returns>reurns true if user have enterered latest OTP</returns>
        public bool AuthenticateOTP(int userid, string otp, out string GuidString, out int MaxAttUserts)
        {
            OTPMaster ObjOTP = AccuitAdminDbContext.OTPMasters.OrderByDescending(k => k.CreatedDate).FirstOrDefault(k => k.UserID == userid);

            GuidString = "";
            MaxAttUserts = 0;
            if (ObjOTP != null)
            {
                MaxAttUserts = ObjOTP.Attempts.Value;
                if (ObjOTP.OTP == otp)
                {
                    GuidString = ObjOTP.GUID;
                    return true;
                }
                else
                {
                    ObjOTP.Attempts = ++MaxAttUserts;
                    AccuitAdminDbContext.Entry<OTPMaster>(ObjOTP).State = System.Data.EntityState.Modified;
                    AccuitAdminDbContext.SaveChanges(); // TBD
                    return false;
                }
            }
            else
            {
                return true;
            }

        }

        /// <summary>
        /// save OTP (One Time Password) to database
        /// </summary>
        /// <param name="otp"> Object of OTP</param>
        /// <returns>returns true when data is saved</returns>
        public bool SaveOTP(OTPMaster otp)
        {
            bool IsSuccess = false;
            // In case from Generating OTP from Automatic redirect to Change Password because of not complex password multiple OTPs can be generated
            // Use this validation to restrict user to generate multiple OTPs
            if (ValidateUser(otp.UserID, AspectEnums.UserValidationType.LastAttemptDuration))
            {
                AccuitAdminDbContext.Entry<OTPMaster>(otp).State = System.Data.EntityState.Added;
                IsSuccess = AccuitAdminDbContext.SaveChanges() > 0;
            }
            return IsSuccess;

        }


        /// <summary>
        /// Method to fetch user authorization parameters for various modules in application
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns entity collection</returns>
        public IList<SecurityAspect> GetUserAuthorization(int userID)
        {
            //ActivityLog.SetLog("Initializing LogIn Page : ", LogLoc.DEBUG);

            var result = (from urm in AccuitAdminDbContext.UserRoleModulePermissions
                          join rm in AccuitAdminDbContext.RoleModules on urm.RoleModuleID equals rm.RoleModuleID
                          join m in AccuitAdminDbContext.ModuleMasters on rm.ModuleID equals m.ModuleID
                          join rl in AccuitAdminDbContext.RoleMasters on rm.RoleID equals rl.RoleID
                          join ur in AccuitAdminDbContext.UserRoles on rl.RoleID equals ur.RoleID
                          join um in AccuitAdminDbContext.UserMasters on ur.UserID equals um.UserID
                          where um.UserID == userID && !um.isDeleted && !rl.IsDeleted //&& urm.PermissionID == 1
                          && ur.IsActive && !ur.isDeleted
                          orderby urm.UserRolePermissionID
                          select new SecurityAspect()
                          {
                              ModuleID = m.ModuleID,
                              PermissionID = urm.PermissionID,
                              PermissionValue = urm.PermissionValue,
                              RoleID = ur.RoleID,
                              UserID = um.UserID,
                              UserRolePermissionID = urm.UserRolePermissionID,
                              ModuleCode = m.ModuleCode.HasValue ? m.ModuleCode.Value : 0,
                          });

            return result.ToList();

        }


        #region Forgot Password Functions
        /// <summary>
        /// Validate User if given User Code is correct or not
        /// </summary>
        /// <param name="UserlCode">User Code of User</param>
        /// <param name="Type">Validation type (Only User Code, User Code and Email etc)</param>
        /// <returns></returns>
        public bool ValidateUser(int UserID, AspectEnums.UserValidationType Type)
        {
            bool IsValid = false;
            UserMaster User = null;
            //if (Type == AspectEnums.UserValidationType.UserlCode)
            //{
            //    User = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == UserlCode && !k.IsDeleted);
            //}
            if (Type == AspectEnums.UserValidationType.EmplCode_Email)
            {
                User = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == UserID && !k.isDeleted && !string.IsNullOrEmpty(k.Email));
            }
            if (Type == AspectEnums.UserValidationType.ForgotPasswordAttempts)
            {

                DateTime Today = DateTime.Today;
                DateTime Tomorrow = DateTime.Today.AddDays(1);
                UserMaster User1 = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == UserID && !k.isDeleted);
                //// Check Max AttUserts
                if (User1 != null)
                {
                    int TodaysAttUserts = AccuitAdminDbContext.OTPMasters.Where(k => k.UserID == UserID && k.CreatedDate >= Today && k.CreatedDate < Tomorrow).Count();
                    int PasswordAttUserts = Convert.ToInt32(AppUtil.GetAppSettings(AspectEnums.ConfigKeys.FotgotPasswordAttempts));
                    IsValid = TodaysAttUserts < PasswordAttUserts;
                }

            }
            if (Type == AspectEnums.UserValidationType.LastAttemptDuration)
            {
                DateTime Now = DateTime.Now;
                UserMaster user1 = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == UserID && !k.isDeleted);
                //// Check Last AttUsert
                if (user1 != null)
                {
                    string LastAttUsertDuration = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.LastAttemptDuration);
                    string[] TimeArr = LastAttUsertDuration.Split(':');

                    DateTime LastAttUsertStart = Now.Subtract(new TimeSpan(Int32.Parse(TimeArr[0]), Int32.Parse(TimeArr[1]), Int32.Parse(TimeArr[2])));

                    IsValid = AccuitAdminDbContext.OTPMasters.Where(k => k.UserID == UserID && k.CreatedDate >= LastAttUsertStart && k.CreatedDate < Now).Count() <= 0;

                }

            }

            if (User != null)
                IsValid = true;

            return IsValid;
        }

        /// <summary>
        /// Get Email TUserlate based on ID
        /// </summary>
        /// <param name="TUserlateTypeID">TUserlate ID</param>
        /// <returns>Obejct of EmailTUserlate</returns>
        //public TUserlateMaster GetEmailTUserlate(AspectEnums.TUserlateType TUserlateTypeID)
        //{
        //    return AccuitAdminDbContext.TUserlateMasters.FirstOrDefault(k => k.TUserlateID == (int)TUserlateTypeID && k.TUserlateStatus == 1);
        //}





        //    return false;
        //}
        ///// <summary>
        ///// Get UserID By User Code given in parameter
        ///// </summary>
        ///// <param name="UserlCode"></param>
        ///// <returns></returns>
        //public long? GetUserIDByUserCode(string UserlCode)
        //{
        //    UserMaster user = null;
        //    user = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserlCode == UserlCode && !k.IsDeleted);
        //    if (user != null)
        //        return user.UserID;
        //    else
        //        return null;
        //}

        /// <summary>
        /// Validate GUID in the link of forget password email
        /// </summary>
        /// <param name="GUID"> uniqe string </param>
        /// <returns>true if GUID in the URL is correct</returns>
        public bool ValidateGUID(string GUID)
        {
            int OTPExirationHrs = Convert.ToInt32(AppUtil.GetAppSettings(AspectEnums.ConfigKeys.OTPExirationHrs));

            DateTime StartTime = DateTime.Now.Subtract(new TimeSpan(OTPExirationHrs, 0, 0));
            DateTime EndTime = DateTime.Now.AddMinutes(30);
            OTPMaster objOTP = null;
            if (GUID != null)
            {
                objOTP = AccuitAdminDbContext.OTPMasters.FirstOrDefault(k => k.CreatedDate >= StartTime && k.CreatedDate <= EndTime && k.GUID == GUID);
            }
            if (objOTP != null)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Change password of User
        /// </summary>
        /// <param name="GUID"> uniqe string </param>
        /// <param name="Password">password entered by user</param>
        /// <returns></returns>
        public bool ChangePassword(string GUID, string Password)
        {
            int OTPExirationHrs = Convert.ToInt32(AppUtil.GetAppSettings(AspectEnums.ConfigKeys.OTPExirationHrs));
            DateTime StartTime = DateTime.Now.Subtract(new TimeSpan(OTPExirationHrs, 0, 0));
            DateTime EndTime = DateTime.Now;
            OTPMaster objOTP = AccuitAdminDbContext.OTPMasters.FirstOrDefault(k => k.CreatedDate >= StartTime && k.CreatedDate <= EndTime && k.GUID == GUID);
            if (objOTP != null)
            {
                UserMaster user = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == objOTP.UserID && !k.isDeleted);
                user.Password = EncryptionEngine.EncryptString(Password);
                user.AccountStatus = (int)AspectEnums.UserLoginStatus.Active;
                user.ModifiedDate = DateTime.Now;
                user.ModifiedBy = objOTP.UserID;
                AccuitAdminDbContext.Entry<UserMaster>(user).State = System.Data.EntityState.Modified;
                //Delete all previous OTPs
                foreach (var o in AccuitAdminDbContext.OTPMasters.Where(k => k.UserID == user.UserID))
                    AccuitAdminDbContext.OTPMasters.Remove(o);
                return AccuitAdminDbContext.SaveChanges() > 0;
            }
            else
                return false;
        }

        public bool UpdatePassword(int UserID, string Password)
        {
            UserMaster user = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == UserID && !k.isDeleted);
            user.Password = EncryptionEngine.EncryptString(Password);
            user.AccountStatus = (int)AspectEnums.UserLoginStatus.Active;
            user.ModifiedDate = DateTime.Now;
            user.ModifiedBy = UserID;
            AccuitAdminDbContext.Entry<UserMaster>(user).State = System.Data.EntityState.Modified;
            //Delete all previous OTPs
            return AccuitAdminDbContext.SaveChanges() > 0;
        }


        #endregion
    }
}