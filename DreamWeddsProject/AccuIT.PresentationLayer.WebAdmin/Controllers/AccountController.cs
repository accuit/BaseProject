using AccuIT.CommonLayer.Aspects.Exceptions;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.BusinessLayer.Services.BO;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.CommonLayer.Aspects.Extensions;
using AccuIT.PresentationLayer.WebAdmin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AccuIT.PresentationLayer.WebAdmin.Models;
using System.Web.UI.WebControls;
using System.Web.Security;
using AccuIT.CommonLayer.Aspects.Logging;
using AccuIT.CommonLayer.Aspects.EmailService;
using AccuIT.CommonLayer.Resources;
using AccuIT.CommonLayer.Aspects.Security;
using System.Configuration;
using System.Data.Entity.Validation;
using PresentationLayer.DreamWedds.WebAdmin.CustomFilter;
using PresentationLayer.DreamWedds.Web.Models;

namespace AccuIT.PresentationLayer.WebAdmin.Controllers
{
    //[RequireHttps]
    public class AccountController : Controller
    {
        #region Private Method initialization

        int loginResponse = 0;

        private IUserService userBusinessInstance;
        private ISecurityService securityBusinessInstance;
        private IEmailService emailBusinessInstance;
        private IWeddingService weddingBusinessInstance;
        public IUserService UserBusinessInstance
        {
            get
            {
                if (userBusinessInstance == null)
                {
                    userBusinessInstance = AopEngine.Resolve<IUserService>(AspectEnums.AspectInstanceNames.UserManager, AspectEnums.ApplicationName.AccuIT);
                }
                return userBusinessInstance;
            }
        }

        public ISecurityService SecurityBusinessInstance
        {
            get
            {
                if (securityBusinessInstance == null)
                {
                    securityBusinessInstance = AopEngine.Resolve<ISecurityService>(AspectEnums.AspectInstanceNames.SecurityManager, AspectEnums.ApplicationName.AccuIT);
                }
                return securityBusinessInstance;
            }
        }
        public IEmailService EmailBusinessInstance
        {
            get
            {
                if (emailBusinessInstance == null)
                {
                    emailBusinessInstance = AopEngine.Resolve<IEmailService>(AspectEnums.AspectInstanceNames.EmailManager, AspectEnums.ApplicationName.AccuIT);
                }
                return emailBusinessInstance;
            }
        }

        public IWeddingService WeddingBusinessInstance
        {
            get
            {
                if (weddingBusinessInstance == null)
                {
                    weddingBusinessInstance = AopEngine.Resolve<IWeddingService>(AspectEnums.AspectInstanceNames.WeddingManager, AspectEnums.ApplicationName.AccuIT);
                }
                return weddingBusinessInstance;
            }
        }

        #endregion
        UserProfileBO USERPROFILE = new UserProfileBO();
        List<UserWeddingSubscriptionBO> USERWEDDINGPROFILE = new List<UserWeddingSubscriptionBO>();

        bool isDebugMode = ConfigurationManager.AppSettings["IsDebugMode"] == "Y" ? true : false;

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            string loginuserType = Request.QueryString["loginUserType"];
            ViewBag.hdLoginUserType = loginuserType;
            LoginViewModel model = new LoginViewModel();
            ViewBag.ReturnUrl = returnUrl;

            if (Request.Cookies["usercode"] != null && Request.Cookies["userpassword"] != null)
            {
                model.UserName = Request.Cookies["usercode"].Value.ToString().Trim();
                //model.UserName.ToString().Attributes.Add("value", Request.Cookies["userpassword"].Value.ToString().Trim());
                model.RememberMe = true;

            }
            else
            {
                model.RememberMe = false;
            }
            ActivityLog.SetLog("LogIn Page Opened Successfully : ", LogLoc.DEBUG);
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    if (!String.IsNullOrEmpty(model.UserName.Trim()) && !String.IsNullOrEmpty(model.Password.Trim()))
                    {
                        ActivityLog.SetLog("Initializing LogIn Page for user : " + model.UserName, LogLoc.INFO);


                        loginResponse = UserBusinessInstance.LoginWebUser(model.UserName.Trim(), model.Password.Trim());
                        ActivityLog.SetLog("Login Response >> " + loginResponse.ToString() + " <<", LogLoc.DEBUG);
                        if (loginResponse > 0)
                        {
                            List<string> ErrorMessage = new List<string>();

                            #region Show popup if sessionID not matching with existing SessionID
                            HttpContext.Session[PageConstants.SESSION_USER_ID] = loginResponse;

                            var dailyLoginHistory = UserBusinessInstance.GetActiveLogin(loginResponse, (int)AspectEnums.AnnouncementDevice.Console);

                            if (dailyLoginHistory.SessionID != null)
                            {
                                if (HttpContext.Session.SessionID != dailyLoginHistory.SessionID)
                                {
                                    ActivityLog.SetLog("Multiple session found user : " + model.UserName, LogLoc.DEBUG);
                                    userBusinessInstance.TerminateExistingSessions((int)HttpContext.Session[PageConstants.SESSION_USER_ID]);
                                    loginResponse = (int)AspectEnums.LoginAccessType.Terminate;
                                    ViewBag.Message = String.Format(Messages.MultipleSessionMessage, dailyLoginHistory.IpAddress, dailyLoginHistory.BrowserName);
                                    ViewBag.IsSuccess = false;
                                    ViewBag.ShowPopup = true;
                                    return;
                                }
                                else
                                {
                                    ActivityLog.SetLog("Login Success! Going to call WelcomeUserAccuITAdmin: ", LogLoc.DEBUG);
                                    WelcomeUserAccuITAdmin(loginResponse); //Permit user to access application
                                    return;
                                }
                            }
                            else
                            {
                                ActivityLog.SetLog("No existing sessionID found. Creating new session: ", LogLoc.DEBUG);
                                DailyLoginHistoryBO LoginHistory = new DailyLoginHistoryBO()
                                {
                                    UserID = loginResponse,
                                    LoginTime = System.DateTime.Now,
                                    SessionID = Session.SessionID,
                                    IpAddress = Request.ServerVariables["REMOTE_ADDR"],
                                    IsLogin = true,
                                    BrowserName = Request.Browser.Browser.ToString(),
                                    LoginType = (int)AspectEnums.AnnouncementDevice.Console,
                                };
                                UserBusinessInstance.SubmitDailyLoginHistory(LoginHistory);
                                WelcomeUserAccuITAdmin(loginResponse);
                                return;
                            }

                            #endregion

                        }

                    }
                }, AspectEnums.ExceptionPolicyName.AssistingAdministrators.ToString());
            }
            catch (Exception ex)
            {
                ActivityLog.SetLog("Exception : Message| " + ex.Message, LogLoc.ERROR);
                ViewBag.IsSuccess = false;
                ViewBag.ShowPopup = true;
                ViewBag.Message = "Connection to server failed. Please try again.";
                return View();
            }

            if (loginResponse == (int)AspectEnums.LoginAccessType.Terminate)
            {
                ViewBag.Message = Messages.TerminateSessionConfirmation;
            }
            else if (loginResponse == (int)AspectEnums.UserLoginAttemptStatus.WrongPassword)
            {
                ViewBag.Message = Messages.LoginWrongPassword + " : Incorrect Password!";
            }
            else if (loginResponse == (int)AspectEnums.UserLoginAttemptStatus.WrongUserId)
            {
                ViewBag.Message = Messages.LoginWrongUserId + " : Incorrect UserName!";
            }
            else if (loginResponse == (int)AspectEnums.UserLoginAttemptStatus.InActive)
            {
                ViewBag.Message = Messages.LoginInActive;
            }
            else if (loginResponse == (int)AspectEnums.UserLoginAttemptStatus.Locked)
            {
                ViewBag.Message = Messages.LoginLocked;
            }
            else if (loginResponse == (int)AspectEnums.UserLoginAttemptStatus.InvalidWebUser)
            {
                ViewBag.Message = "You are not authorized to login, please contact your administrator";
                Response.Redirect("~/Account/UnAuthorizedUser", true);
                return View("UnAuthorizedUser", "Account");
            }
            else if (loginResponse > (int)AspectEnums.UserLoginAttemptStatus.Successful)
            {
                ViewBag.ShowPopup = false;
                if (USERPROFILE.IsAdmin)
                    return RedirectToAction("Index", "Admin");
                return RedirectToAction("Index", "Home");
            }

            if (loginResponse < 0)
            {
                ViewBag.IsSuccess = false; ViewBag.ShowPopup = true;
            }

            return View();
        }

        private void WelcomeUserAccuITAdmin(int userID)
        {
            USERPROFILE = UserBusinessInstance.DisplayUserProfile(userID);
            USERWEDDINGPROFILE = WeddingBusinessInstance.GetUserWeddingSubscriptions(userID);
            CreateFreshSession();

            int roleID = (int)USERPROFILE.RoleID;

            HttpContext.Session[PageConstants.SESSION_USER_ID] = userID;
            HttpContext.Session[PageConstants.SESSION_PROFILE_KEY] = USERPROFILE;
            HttpContext.Session[PageConstants.SESSION_WEDDING_PROFILE] = USERWEDDINGPROFILE;
            HttpContext.Session[PageConstants.SESSION_ROLE_ID] = roleID;
            HttpContext.Session[PageConstants.SESSION_ADMIN] = USERPROFILE.IsAdmin ? "1" : "0";
            var myWeddings = WeddingBusinessInstance.GetUserWeddingDetail(userID);
            SetUserModules(userID);
            ActivityLog.SetLog("Welcome to Accuit| Sessions created.", LogLoc.INFO);
        }
        /// <summary>
        /// In order to create fresh session and send auth cookie to broswer
        /// </summary>
        private void CreateFreshSession()
        {
            Session.Clear();

            // createa a new GUID and save into the session
            string guid = Guid.NewGuid().ToString();
            HttpContext.Session[SessionVariables.AuthToken] = guid;
            // now create a new cookie with this guid value
            HttpContext.Response.Cookies.Add(new HttpCookie(CookieVariables.AuthToken, guid));
        }

        private void SetUserModules(int userID)
        {
            IList<UserModuleDTO> modules = UserBusinessInstance.GetUserWebModules(userID);
            Session[PageConstants.SESSION_MODULES] = modules;
            IList<SecurityAspectBO> permissions = SecurityBusinessInstance.GetUserAuthorization(userID);
            Session[PageConstants.SESSION_PERMISSIONS] = permissions;

        }

        public ActionResult LogOut()
        {
            bool status = false;
            try
            {
                AccuIT.CommonLayer.Aspects.Exceptions.ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    int loggenInUserID = HttpContext.Session[PageConstants.SESSION_USER_ID] != null ? Convert.ToInt32(HttpContext.Session[PageConstants.SESSION_USER_ID]) : 0;

                    if (loggenInUserID > 0)
                    {
                        status = UserBusinessInstance.LogoutWebUser(loggenInUserID, Session.SessionID);
                        Session.Abandon();

                        #region Clear All Cookies
                        HttpCookie aCookie;
                        string cookieName;
                        int limit = HttpContext.Request.Cookies.Count;
                        for (int i = 0; i < limit; i++)
                        {
                            cookieName = HttpContext.Request.Cookies[i].Name;
                            aCookie = new HttpCookie(cookieName);
                            aCookie.Expires = DateTime.Now.AddDays(-1);
                            Response.Cookies.Add(aCookie);
                        }
                        #endregion

                    }
                }, AspectEnums.ExceptionPolicyName.AssistingAdministrators.ToString());
            }
            catch
            {
            }
            if (status)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                return View();
            }

        }


        [AllowAnonymous]
        public ActionResult Register(string identifier)
        {
            UserMasterBO user = new UserMasterBO();
            identifier = identifier.Replace(' ', '+'); // Decoding URL into actual encrypted string.
            try
            {

                if (identifier != null)
                {
                    ViewBag.IsRegistered = true;

                    string decrypt = EncryptionEngine.Decrypt(identifier);
                    user.UserID = Convert.ToInt32(decrypt.Split(',')[0]);
                    user.FirstName = decrypt.Split(',')[1].ToString();
                    user.LastName = decrypt.Split(',')[2].ToString();
                    user.LoginName = decrypt.Split(',')[3].ToString();
                    ViewBag.TemplateName = decrypt.Split(',')[4].ToString();
                    var status = UserBusinessInstance.GetUserByLoginName(user.LoginName).AccountStatus;
                    if (status == (int)AspectEnums.UserAccountStatus.Pending)
                    {
                        return View(user);
                    }
                    else
                    {
                        ViewBag.Message = "User already exist. Please login in with your email address.";
                        ViewBag.IsSuccess = false;
                        ViewBag.ShowPopup = true;
                        return View();
                    }

                }
                else
                {
                    ViewBag.Message = "Invalid registration url. Contact administrator.";
                    ViewBag.IsSuccess = false;
                    ViewBag.ShowPopup = true;
                    return View();
                }

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Something went wrong. Contact administrator.";
                ViewBag.IsSuccess = false;
                ViewBag.ShowPopup = true;
                return View();
            }

        }

        [HttpPost]
        public ActionResult Register(UserMasterBO model, string sessionID)
        {

            List<string> ErrorMessage = new List<string>();
            string newPassword = model.Password;
            var userinfo = new UserProfileBO();

            if (model.Password != model.ConfirmPassword)
            {
                ViewBag.Message = "New Password & Confirm Password did not match. Try again.";
                ViewBag.IsSuccess = false;
                return View(model);
            }

            newPassword.IsComplexPassword(ref ErrorMessage);
            if (ErrorMessage.Count > 0)
            {
                ViewBag.Message = ErrorMessage.Select(k => k).Aggregate((a, b) => a + "\n" + b);
                ViewBag.IsSuccess = false;
                return View(model);
            }
            try
            {

                userinfo = UserBusinessInstance.GetUserByLoginName(model.LoginName);
                bool IfUserExists = userinfo.UserID > 0 ? true : false;
                #region Old registration code
                //else
                //{
                //   
                //    if (!IfUserExists)
                //    {

                //        model.CreatedBy = 0;
                //        model.JoiningDate = DateTime.Now;
                //        model.Email = model.LoginName;
                //        model.isDeleted = false;
                //        model.isActive = true;
                //        model.IsEmployee = false;
                //        model.AccountStatus = (int)AspectEnums.UserLoginStatus.Active;
                //        sessionID = HttpContext.Session.SessionID.ToString();
                //        int registerEmp = UserBusinessInstance.SubmitNewEmployee(model, sessionID);

                //        ViewBag.IsSuccess = true;
                //        ViewBag.ShowPopup = true;
                //        ViewBag.Message = "Congratulations for being a part of Dream Wedds family.";

                //        return View();
                //    }
                #endregion
                if (IfUserExists && userinfo.AccountStatus == (int)AspectEnums.UserAccountStatus.Pending)
                {
                    userinfo.AccountStatus = (int)AspectEnums.UserLoginStatus.Active;
                    sessionID = HttpContext.Session.SessionID.ToString();
                    userinfo.Password = model.Password;
                    bool isUpdated = UserBusinessInstance.UpdateUserProfile(userinfo);
                    if (isUpdated)
                    {

                        ViewBag.IsSuccess = true;
                        ViewBag.ShowPopup = true;
                        ViewBag.Message = "You have created your password. Login now.";
                        return View("Login");
                    }
                    else
                    {
                        ViewBag.IsSuccess = true;
                        ViewBag.ShowPopup = true;
                        ViewBag.Message = "Something went wrong. Try again later.";
                    }
                    return View(model);
                }
                else
                {
                    ViewBag.Message = "User with this email address already exists. Please with your email address.";
                    ViewBag.IsSuccess = false;
                    return View(model);
                }

            }
            catch (DbEntityValidationException ex)
            {
                ViewBag.IsSuccess = false;
                var newException = new FormattedDbEntityValidationException(ex);
                ViewBag.Message = "Error: " + ex;
            }
            catch (Exception e)
            {
                ViewBag.IsSuccess = false;
                ViewBag.Message = "Error: " + e;
            }
            return View(model);
        }


        [AllowAnonymous]
        [HttpGet]
        public ActionResult ForgetPassword()
        {

            return View();
        }

        [HttpPost]
        public ActionResult ForgetPassword(LoginViewModel model)
        {
            ExceptionEngine.ProcessAction(() =>
            {
                string loginName = model.UserName.Trim();
                int? UserId =  userBusinessInstance.GetUserByLoginName(loginName).UserID;

                if (!ValidateUser(UserId))
                {
                    ViewBag.ShowPopup = true;
                    ViewBag.Message = "User does not exist. Please write correct email/userId.";
                    return;
                }

                if (SendOTPAndEmail(UserId.Value))
                {
                    ViewBag.ShowPopup = true;
                    ViewBag.Message = "Password reset link has been sent to your email.";
                }


            }, AspectEnums.ExceptionPolicyName.AssistingAdministrators.ToString());
            return View();
        }

        public ActionResult UnAuthorizedUser()
        {

            return View();
        }


        #region  Validate Employee
        private bool ValidateUser(int? EmpId)
        {
            if (EmpId == null || EmpId == 0)
            {
                ViewBag.Message = "Invalid or incomplete data, Please contact administrator";
                return false;

            }
            if (!SecurityBusinessInstance.ValidateUser(EmpId.Value, AspectEnums.UserValidationType.EmplCode_Email))
            {
                ViewBag.Message = "Invalid or incomplete data, Please contact administrator";
                return false;

            }
            if (!SecurityBusinessInstance.ValidateUser(EmpId.Value, AspectEnums.UserValidationType.ForgotPasswordAttempts))
            {
                ViewBag.Message = "You have exceeded maximum number of password reset attempts, please try again tomorrow";
                return false;
            }
            if (!SecurityBusinessInstance.ValidateUser(EmpId.Value, AspectEnums.UserValidationType.LastAttemptDuration))
            {
                ViewBag.Message = "you have already attempt to change password, please use the same email to reset password";
                return false;
            }
            return true;


        }
        #endregion

        private bool SendOTPAndEmail(int UserId)
        {
            bool IsSuccess = false;
            #region Prepare OTP Data

            string UniqueString = AppUtil.GetUniqueGuidString();
            string OTPString = AppUtil.GetUniqueRandomNumber(100000, 999999); // Generate a Six Digit OTP
            OTPBO objOTP = new OTPBO() { GUID = UniqueString, OTP = OTPString, CreatedDate = DateTime.Now, UserID = UserId, Attempts = 0 };

            #endregion
            try
            {

                if (SecurityBusinessInstance.SaveOTP(objOTP))
                {
                    #region Send Email Servie and OTP
                    //string hostName = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.HostName);
                    string resetUrl = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.ForgotPasswordURL);
                    string PasswordResetURL = resetUrl +  UniqueString;
                    //string PasswordResetURL = Request.Url.AbsoluteUri.Split('/')[0] + Request.Url.AbsoluteUri.Split('/')[1]  + resetUrl + "?id=" + UniqueString;
                    EmailNotificationService eNotification = new EmailNotificationService();
                    var userProfile = UserBusinessInstance.DisplayUserProfile(UserId); // empBusinessInstance.DisplayEmpProfile(EmpId);
                    TemplateMasterBO objEmailTemplate = EmailBusinessInstance.GetEmailTemplate((int)AspectEnums.EmailTemplateCode.ResetPassword);
                    List<TemplateMergeFieldBO> mergeFields = EmailBusinessInstance.GetEmailMergeFields(objEmailTemplate.TemplateID);
                    foreach (var field in mergeFields)
                    {
                        if (field.SRC_FIELD == "{{PASSWORDRESETURL}}")
                            objEmailTemplate.TemplateContent = eNotification.FindReplace(objEmailTemplate.TemplateContent, "{{PASSWORDRESETURL}}", PasswordResetURL);

                        else if (field.SRC_FIELD == "{{TONAME}}")
                            objEmailTemplate.TemplateContent = eNotification.FindReplace(objEmailTemplate.TemplateContent, field.SRC_FIELD, userProfile.FirstName + " " + userProfile.LastName);
                    }
                    objEmailTemplate.TemplateContent = eNotification.FindReplace(objEmailTemplate.TemplateContent, "{{COMPANY}}", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.CompanyName));


                    EmailServiceDTO emailService = new EmailServiceDTO();
                    emailService.Priority = 1;
                    emailService.CreatedBy = userProfile.UserID;
                    emailService.IsHtml = true;
                    emailService.ToName = userProfile.FirstName + " " + userProfile.LastName;
                    emailService.Body = objEmailTemplate.TemplateContent;
                    emailService.Status = (int)AspectEnums.EmailStatus.Pending;
                    emailService.ToEmail = userProfile.Email;
                    emailService.FromName = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.FromName);
                    emailService.FromEmail = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.FromEmail);
                    emailService.Subject = eNotification.FindReplace(objEmailTemplate.TemplateSubject, "{{COMPANY}}", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.CompanyName));
                    emailService.IsAttachment = false;
                    emailService.TemplateID = objEmailTemplate.TemplateID;
                    emailBusinessInstance.InsertEmailRecord(emailService);

                    eNotification.SendEmailNotification(emailService, objEmailTemplate);
                    IsSuccess = true;

                    #endregion
                }
            }
            catch (Exception ex)
            {
                IsSuccess = false;
            }


            return IsSuccess;
        }



        [HttpGet]
        public ActionResult ChangePassword()
        {
            string uniqueid = Request.QueryString["id"];
            if (SecurityBusinessInstance.ValidateGUID(uniqueid))
            {
                ViewBag.hdnUniqueID = uniqueid;
                ViewBag.ShowForm = true;
            }
            else
            {
                ViewBag.ShowPopup = true;
                ViewBag.Message = "Invalid or expired link , Please try again later.";

            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(ManageUserViewModel model, FormCollection collection)
        {
            ExceptionEngine.ProcessAction(() =>
            {

                //lblError.Text = "";
                string uniqueid = collection["hdnUniqueID"].ToString();

                String NewPassword = model.NewPassword;

                List<string> ErrorMessage = new List<string>();

                if (model.NewPassword != model.ConfirmPassword)
                {
                    ViewBag.Message = "New Password & Retype Password did not match";
                    ViewBag.ShowForm = true;
                    return;
                }

                NewPassword.IsComplexPassword(ref ErrorMessage);

                if (ErrorMessage.Count > 0)
                {
                    ViewBag.Message = ErrorMessage.Select(k => k).Aggregate((a, b) => a + "\n" + b);
                    ViewBag.ShowForm = true;
                    return;
                }
                else if (SecurityBusinessInstance.ChangePassword(uniqueid, NewPassword))
                {
                    ViewBag.ShowPopUp = true;
                    ViewBag.Message = "Password Changed Successfully";
                    return;
                }
                else
                {
                    ViewBag.ShowPopup = true;
                    ViewBag.Message = "You are not authorized to change password.";
                    return;
                }

            }, AspectEnums.ExceptionPolicyName.AssistingAdministrators.ToString());

            return View();

        }


    }
}
