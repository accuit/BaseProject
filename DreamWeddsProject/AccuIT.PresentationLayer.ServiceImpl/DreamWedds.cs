using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using AccuIT.BusinessLayer.Services.BO;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.CommonLayer.Aspects.Exceptions;
using AccuIT.CommonLayer.Aspects.Security;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.CommonLayer.Aspects.Utilities.HttpMultipartParser;
using AccuIT.CommonLayer.Resources;
using AccuIT.PresentationLayer.ServiceImpl.Security;
using AccuIT.CommonLayer.Aspects.ReportBO;
using AccuIT.CommonLayer.Aspects.Extensions;
using AccuIT.CommonLayer.Log;
using System.IO;
using System.Web;
using System.Web.Http;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace AccuIT.PresentationLayer.ServiceImpl
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SmartDost" in both code and config file together.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    [ExceptionShielding("ServiceExceptionPolicy")]
    public partial class DreamWedds : BaseService, IDreamWedds
    {

        public void GetOptions()
        {
            throw new NotImplementedException();
        }

        public string GetData(string value)
        {
            throw new NotImplementedException();
        }

        #region Login Authentication and Security Services
        string Domain = "{D}"; //AppUtil.GetAppSettings(AspectEnums.ConfigKeys.USERFOLDER);
        private string GenerateLoginErrorMessage(byte LoginStatus)
        {
            string message = string.Empty;

            switch (LoginStatus)
            {
                case 1:
                    message = Messages.LoginSuccess;
                    break;
                case 2:
                    message = Messages.Inactive;
                    break;
                case 3:
                    message = Messages.Lock;
                    break;
                case 4:
                    message = Messages.DaysExpire;
                    break;
                case 5:
                    message = Messages.WrongPassword;
                    break;
                case 6:
                    message = Messages.Wrongimi;
                    break;
                case 7:
                    message = Messages.MultipleIMEI;
                    break;
            }
            return message;
        }

        public JsonResponse<ServiceOutputDTO> CommonLoginService(string username, string password)
        {
            JsonResponse<ServiceOutputDTO> response = new JsonResponse<ServiceOutputDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    ServiceInputDTO SIDTO = new ServiceInputDTO();
                    SIDTO.username = username;
                    SIDTO.password = password;

                    ServiceOutputDTO outputDTO = new ServiceOutputDTO();
                    response.SingleResult = outputDTO;
                    //bool isSucssfull = true;

                    #region Check user Authentication
                    var authOutput = UserBusinessInstance.AuthenticateUserLogin(
                        SIDTO.imei,
                                                        EncryptionEngine.EncryptString(SIDTO.username),
                                                        EncryptionEngine.EncryptString(SIDTO.password),
                                                      Convert.ToString(SIDTO.lattitude),
                                                      Convert.ToString(SIDTO.longitude),
                                                      Convert.ToString(SIDTO.BrowserName),
                                                      Convert.ToString(SIDTO.ModelName),
                                                      Convert.ToString(SIDTO.IPAddress),
                                                      Convert.ToString(SIDTO.apkVersion),
                                                      (byte)AspectEnums.AnnouncementDevice.Apk);
                    #endregion

                    response.Message = GenerateLoginErrorMessage(authOutput.AuthStatus.Value);
                    response.StatusCode = authOutput.AuthStatus.Value == 1 ? "001" : "";
                    response.SingleResult.StatusCode = response.StatusCode;
                    response.SingleResult.Message = response.Message;
                    if (authOutput.userID == 0)
                    {
                        response.IsSuccess = false;
                        //response.Message = GenerateLoginErrorMessage(authOutput.AuthStatus.Value);
                    }
                    else
                    {
                        ////generate service token 

                        var APIKey = AppUtil.GetUniqueKey();
                        var APIToken = DateTime.Now.ToString().GetHashCode().ToString("x");

                        var result = UserBusinessInstance.GetLoginDetails(authOutput.userID, Convert.ToString(SIDTO.apkVersion), SIDTO.RequireAnnouncement, APIKey, APIToken);
                        response.SingleResult.UserID = result.UserID;
                        response.SingleResult.RoleID = result.RoleID;
                        response.SingleResult.CompanyID = result.CompanyID;
                        response.SingleResult.UserCode = result.UserCode;
                        response.SingleResult.LoginName = EncryptionEngine.DecryptString(result.LoginName);
                        response.SingleResult.FirstName = result.FirstName;
                        response.SingleResult.UserRoleID = result.UserRoleID.Value;
                        response.SingleResult.LastName = result.LastName;
                        response.SingleResult.Mobile = EncryptionEngine.DecryptString(result.Mobile);
                        response.SingleResult.APIKey = APIKey;
                        response.SingleResult.APIToken = APIToken;
                        response.IsSuccess = true;


                        /*//user is logging first time so make an entry in DailyLoginHistory
                        DailyLoginHistoryBO LoginHistory = new DailyLoginHistoryBO()
                        {
                            UserID = Convert.ToInt32(authOutput.SingleResult.UserID),
                            LoginTime = System.DateTime.Now,
                            Longitude = Convert.ToString(SIDTO.longitude),
                            Lattitude = Convert.ToString(SIDTO.lattitude),
                            IpAddress = Convert.ToString(SIDTO.IPAddress),
                            IsLogin = true,
                            BrowserName = Convert.ToString(SIDTO.BrowserName),
                            APKVersion = Convert.ToString(SIDTO.apkVersion),
                            ApkDeviceName = Convert.ToString(SIDTO.ModelName),
                            LoginType = (int)AspectEnums.AnnouncementDevice.Apk,
                        };
                        UserBusinessInstance.SubmitDailyLoginHistory(LoginHistory);

                        response.FailedValidations = authOutput.FailedValidations;
                        response.StatusCode = authOutput.StatusCode;
                        response.SingleResult.APIKey = authOutput.SingleResult.APIKey;
                        response.SingleResult.APIToken = authOutput.SingleResult.APIToken;
                        response.SingleResult.CompanyID = authOutput.SingleResult.CompanyID;
                        response.SingleResult.EmplCode = authOutput.SingleResult.EmplCode;
                        response.SingleResult.Message = authOutput.SingleResult.Message;
                        response.SingleResult.RoleID = authOutput.SingleResult.RoleID;
                        response.SingleResult.StatusCode = authOutput.SingleResult.StatusCode;
                        response.SingleResult.UserDeviceID = authOutput.SingleResult.UserDeviceID;
                        response.SingleResult.UserID = authOutput.SingleResult.UserID;

                        //Added by Amit (21-Aug-2013 to get ISGeoTag and IsGeoPhoto
                        RoleMasterBO objRoleMaster = UserBusinessInstance.GetRoleMaster().FirstOrDefault(k => k.RoleID == authOutput.SingleResult.RoleID);
                        response.SingleResult.IsGeoTagMandate = objRoleMaster.IsGeoTagMandate;
                        response.SingleResult.IsGeoPhotoMandate = objRoleMaster.IsGeoPhotoMandate;

                        var isapk = SystemBusinessInstance.IsApkVersionUpdated(SIDTO.apkVersion);
                        response.SingleResult.APKURL = isapk.APKURL;
                        response.SingleResult.IsUpdated = isapk.IsUpdated;
                        var result = SystemBusinessInstance.GetAnnouncement(DateTime.Now.Date).FirstOrDefault();
                        
                        */
                    }

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;

        }
        /// <summary>
        /// Method to authenticate user in system
        /// </summary>
        /// <param name="imei">mobile imei number</param>
        /// <param name="password">password</param>
        /// <param name="geoTag">lattitude value</param>
        /// <param name="longitude">longitude value</param>
        /// <returns>returns status</returns>
        public JsonResponse<ServiceResponseDTO> AuthenticateUser(string imei, string LoginName, string password, string lattitude, string longitude, string BrowserName, string ModelName, string IPaddress)
        {
            JsonResponse<ServiceResponseDTO> response = new JsonResponse<ServiceResponseDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    password = EncryptionEngine.EncryptString(password);
                    LoginName = EncryptionEngine.EncryptString(LoginName);
                    ServiceResponseDTO serviceStatus = new ServiceResponseDTO();
                    ServiceResponseBO status = UserBusinessInstance.AuthenticateUser(imei, LoginName, password, lattitude, longitude, BrowserName, ModelName, IPaddress);
                    EntityMapper.Map(status, serviceStatus);
                    response.IsSuccess = false;
                    switch (serviceStatus.StatusCode)
                    {
                        case "001":
                            response.IsSuccess = true;
                            response.Message = Messages.LoginSuccess;
                            break;
                        case "002":
                            //  response.IsSuccess = true;
                            response.Message = Messages.Inactive;
                            break;
                        case "003":
                            // response.IsSuccess = true;
                            response.Message = Messages.Lock;
                            break;
                        case "004":
                            //  response.IsSuccess = true;
                            response.Message = Messages.DaysExpire;
                            break;
                        case "005":
                            // response.IsSuccess = true;
                            response.Message = Messages.WrongPassword;
                            break;
                        case "006":
                            //  response.IsSuccess = true;
                            response.Message = Messages.Wrongimi;
                            break;
                        case "007":
                            // response.IsSuccess = true;
                            response.Message = Messages.MultipleIMEI;
                            break;
                    }
                    response.SingleResult = serviceStatus;

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Method to reset user password in system
        /// </summary>
        /// <param name="userid">userid</param>
        /// <param name="newpassword">new password</param>
        /// <returns>returns status</returns>
        public JsonResponse<AuthUserDTO> AuthUser(string loginName)
        {
            JsonResponse<AuthUserDTO> response = new JsonResponse<AuthUserDTO>();
            try
            {
                string apiKey = string.Empty;
                string apiToken = string.Empty;
                UserProfileBO profile = UserBusinessInstance.GetUserByLoginName(loginName);
                AuthUserDTO serviceResponse = new AuthUserDTO();
                if (profile.UserID == 0)
                {
                    response.IsSuccess = false;
                    response.Message = "Incorrect User Login Name (UserName)";
                }
                else
                {
                    ServiceResponseBO srbo = UserBusinessInstance.APIKEY(profile.UserID);
                    serviceResponse.UserID = Convert.ToString(profile.UserID);
                    serviceResponse.APIKey = srbo.APIKey;
                    serviceResponse.APIToken = srbo.APIToken;
                    serviceResponse.CompanyID = Convert.ToString(profile.CompanyID);
                    response.IsSuccess = true;
                    response.Message = "Successful";
                    response.SingleResult = serviceResponse;
                }


            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Displays the user profile.
        /// </summary>
        /// <param name="userID">The userId.</param>
        /// <returns></returns>
        //[UserSecureOperation]
        public JsonResponse<UserProfileDTO> DisplayUserProfile(string userID)
        {
            JsonResponse<UserProfileDTO> response = new JsonResponse<UserProfileDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    UserProfileDTO objUserProfileDTO = new UserProfileDTO();

                    UserProfileBO objUserProfileBO = UserBusinessInstance.DisplayUserProfile(Convert.ToInt32(userID));
                    EntityMapper.Map(objUserProfileBO, objUserProfileDTO);
                    if (objUserProfileDTO.UserID > 0)
                    {
                        response.IsSuccess = true;
                        response.SingleResult = objUserProfileDTO;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = Messages.InvalidUserID;
                    }
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        public JsonResponse<ForgotPasswordDTO> ForgetPassword(string imei, string loginName, string newpassword)
        {
            JsonResponse<ForgotPasswordDTO> response = new JsonResponse<ForgotPasswordDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    List<string> ErrorMessage = new List<string>();

                    ForgotPasswordDTO forgetpasswordStatus = new ForgotPasswordDTO();
                    if (newpassword.IsComplexPassword(ref ErrorMessage))
                    {
                        newpassword = EncryptionEngine.EncryptString(newpassword);
                        loginName = EncryptionEngine.EncryptString(loginName);
                        ForgotPasswordBO status = UserBusinessInstance.ForgetPassword(imei, loginName, newpassword);
                        EntityMapper.Map(status, forgetpasswordStatus);
                        if (forgetpasswordStatus.StatusCode == "001")
                        {
                            response.Message = Messages.AccountReset;
                            response.IsSuccess = true;
                        }
                        else if (forgetpasswordStatus.StatusCode == "002")
                        {

                            response.Message = Messages.EmpCodeNotValid;
                        }
                        else if (forgetpasswordStatus.StatusCode == "003")
                        {
                            response.Message = Messages.ImeiNotValid;
                        }
                    }
                    else
                    {
                        response.Message = Messages.PasswordRule;
                        response.IsSuccess = true;
                    }
                    response.SingleResult = forgetpasswordStatus;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        [UserSecureOperation]
        public JsonResponse<bool> UpdatePassword(int UserID, string Password)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    List<string> ErrorMessage = new List<string>();

                    if (Password.IsComplexPassword(ref ErrorMessage))
                    {
                        bool status = SecurityBusinessInstance.UpdatePassword(UserID, Password);
                        if (!status)
                        {
                            response.Message = ErrorMessage[0];
                            response.IsSuccess = false;
                        }
                        else
                        {
                            response.Message = Messages.PasswordUpdated;
                            response.IsSuccess = true;
                            response.SingleResult = status;
                        }
                    }
                    else
                    {
                        response.Message = ErrorMessage[0];
                        response.IsSuccess = false;
                    }
                    response.StatusCode = "202";
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.StatusCode = "404";
                response.Message = ex.Message;
            }
            return response;
        }

        [UserSecureOperation]
        public JsonResponse<bool> UpdateUserProfile(UserProfileDTO user)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    UserProfileBO BO = new UserProfileBO();
                    EntityMapper.Map(user, BO);
                    bool isSuccess = UserBusinessInstance.UpdateUserProfile(BO);
                    if (!isSuccess)
                    {
                        response.Message = String.Format(Messages.UpdateFailed, BO.FirstName);
                        response.IsSuccess = false;
                    }
                    else
                    {
                        response.Message = Messages.ProfileUpdated;
                        response.IsSuccess = true;
                        response.SingleResult = isSuccess;
                    }
                    response.StatusCode = "202";
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.StatusCode = "404";
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Method to reset the login values after user gets logout
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>returns boolean status</returns>
        [UserSecureOperation]
        public JsonResponse<bool> LogoutUser(int userID)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            try
            {
                response.IsSuccess = response.SingleResult = UserBusinessInstance.LogoutUser(userID);
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        #endregion

        [UserSecureOperation]
        public JsonResponse<List<UserWeddingSubscriptionDTO>> DisplayUserDashboard(string userID)
        {
            ActivityLog.SetLog("DisplayUserDashboard initiated.", LogLoc.INFO);
            JsonResponse<List<UserWeddingSubscriptionDTO>> response = new JsonResponse<List<UserWeddingSubscriptionDTO>>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    List<UserWeddingSubscriptionDTO> userDashborad = new List<UserWeddingSubscriptionDTO>();
                    EntityMapper.Map(WeddingBusinessInstance.GetUserWeddingSubscriptions(Convert.ToInt32(userID)), userDashborad);

                    response.SingleResult = userDashborad;
                    response.StatusCode = "200";
                    response.Message = "Success";
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        //[UserSecureOperation]
        public List<CommonSetupDTO> GetWeddingCommonFields(string ParentId)
        {
            //ParentId = "2";
            List<CommonSetupDTO> output = new List<CommonSetupDTO>();
            ExceptionEngine.ProcessAction(() =>
            {
                if ((!string.IsNullOrEmpty(ParentId)) && ParentId != "null")
                {
                    output = SystemBusinessInstance.GetCommonSetup(0, null, ParentId);
                }

            }, AspectEnums.ExceptionPolicyName.AssistingAdministrators.ToString());

            return output;
        }

        [UserSecureOperation]
        public JsonResponse<WeddingDTO> GetWeddingDetailByID(string weddingID)
        {
            ActivityLog.SetLog("GetWeddingDetailByID initiated.", LogLoc.INFO);

            JsonResponse<WeddingDTO> response = new JsonResponse<WeddingDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    WeddingDTO weddingDTO = new WeddingDTO();
                    EntityMapper.Map(WeddingBusinessInstance.GetWeddingDetailByID(Convert.ToInt32(weddingID)), weddingDTO);
                    if (weddingDTO.IsActive)
                    {
                        response.SingleResult = weddingDTO;
                        response.Message = "Your wedding details are successfully fetched.";
                        response.IsSuccess = true;
                    }
                    else
                    {
                        response.SingleResult = new WeddingDTO();
                        response.IsSuccess = false;
                        response.Message = String.Format(Messages.WeddingExpired, weddingDTO.SubscriptionEndDate);
                    }

                    ActivityLog.SetLog(response.Message, LogLoc.INFO);

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                ActivityLog.SetLog(String.Format(Messages.Exception, ex.Message, ex.InnerException, ex.StackTrace), LogLoc.INFO);
                response.Message = ex.Message;
                response.IsSuccess = false;
            }

            return response;
        }

        [UserSecureOperation]
        public JsonResponse<WeddingEventDTO> GetWeddingEventByID(string eventID)
        {
            ActivityLog.SetLog("GetWeddingEventByID initiated.", LogLoc.INFO);

            JsonResponse<WeddingEventDTO> response = new JsonResponse<WeddingEventDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    WeddingEventDTO eventDTO = new WeddingEventDTO();
                    EntityMapper.Map(WeddingBusinessInstance.GetEventDetailsByID(Convert.ToInt32(eventID)), eventDTO);
                    response.SingleResult = eventDTO;
                    response.Message = "Event details are successfully fetched.";
                    response.IsSuccess = true;
                    ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = "404";
                response.Message = ex.Message;
            }

            return response;
        }

        [UserSecureOperation]
        public JsonResponse<BrideAndMaidDTO> GetBrideMaidByID(string brideID)
        {
            ActivityLog.SetLog("GetWeddingBrideMaids initiated.", LogLoc.INFO);

            JsonResponse<BrideAndMaidDTO> response = new JsonResponse<BrideAndMaidDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    BrideAndMaidDTO bridemaidDTO = new BrideAndMaidDTO();
                    EntityMapper.Map(WeddingBusinessInstance.GetBrideDetailsByID(Convert.ToInt32(brideID)), bridemaidDTO);
                    response.SingleResult = bridemaidDTO;
                    response.Message = "BrideMaid details are successfully fetched.";
                    response.IsSuccess = true;
                    ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                response.StatusCode = "404";
            }

            return response;
        }

        [UserSecureOperation]
        public JsonResponse<GroomAndMenDTO> GetGroomMenByID(string groomID)
        {
            ActivityLog.SetLog("GetWeddingBrideMaids initiated.", LogLoc.INFO);

            JsonResponse<GroomAndMenDTO> response = new JsonResponse<GroomAndMenDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    GroomAndMenDTO groomMenDTO = new GroomAndMenDTO();
                    EntityMapper.Map(WeddingBusinessInstance.GetGroomDetailsByID(Convert.ToInt32(groomID)), groomMenDTO);

                    response.SingleResult = groomMenDTO;
                    response.Message = "Groom and Men details are successfully fetched.";
                    response.IsSuccess = true;
                    ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                response.StatusCode = "404";
            }

            return response;
        }

        [UserSecureOperation]
        public JsonResponse<TimeLineDTO> GetTimeLineByID(string timelineID)
        {
            ActivityLog.SetLog("GetTimeLineByID initiated. TimelineID: " + timelineID, LogLoc.INFO);

            JsonResponse<TimeLineDTO> response = new JsonResponse<TimeLineDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    TimeLineDTO timeDTO = new TimeLineDTO();
                    EntityMapper.Map(WeddingBusinessInstance.GetTimeLineDetailsByID(Convert.ToInt32(timelineID)), timeDTO);
                    response.SingleResult = timeDTO;
                    response.Message = "TimeLine details are successfully fetched.";
                    response.IsSuccess = true;
                    ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        [UserSecureOperation]
        public JsonResponse<List<BrideAndMaidDTO>> GetWeddingBrideMaids(string weddingID)
        {
            ActivityLog.SetLog("GetWeddingBrideMaids initiated.", LogLoc.INFO);

            JsonResponse<List<BrideAndMaidDTO>> response = new JsonResponse<List<BrideAndMaidDTO>>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    List<BrideAndMaidDTO> bridemaidDTO = new List<BrideAndMaidDTO>();
                    EntityMapper.Map(WeddingBusinessInstance.GetWeddingBrideMaids(Convert.ToInt32(weddingID)), bridemaidDTO);

                    response.SingleResult = bridemaidDTO;
                    response.Message = "Your wedding details are successfully fetched.";
                    response.IsSuccess = true;
                    ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        [UserSecureOperation]
        public JsonResponse<List<GroomAndMenDTO>> GetWeddingGroomMen(string weddingID)
        {
            ActivityLog.SetLog("GetWeddingGroomMen initiated.", LogLoc.INFO);

            JsonResponse<List<GroomAndMenDTO>> response = new JsonResponse<List<GroomAndMenDTO>>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    List<GroomAndMenDTO> groommen = new List<GroomAndMenDTO>();
                    EntityMapper.Map(WeddingBusinessInstance.GetWeddingGroomMen(Convert.ToInt32(weddingID)), groommen);

                    response.SingleResult = groommen;
                    response.Message = "Your wedding details are successfully fetched.";
                    response.IsSuccess = true;
                    ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        [UserSecureOperation]
        public JsonResponse<List<WeddingEventDTO>> GetWeddingEvents(string weddingID)
        {
            ActivityLog.SetLog("GetWeddingGroomMen initiated.", LogLoc.INFO);

            JsonResponse<List<WeddingEventDTO>> response = new JsonResponse<List<WeddingEventDTO>>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    List<WeddingEventDTO> events = new List<WeddingEventDTO>();
                    EntityMapper.Map(WeddingBusinessInstance.GetWeddingEvents(Convert.ToInt32(weddingID)), events);

                    response.SingleResult = events;
                    response.Message = "Your wedding events are successfully fetched.";
                    response.IsSuccess = true;
                    ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                response.StatusCode = "404";
            }

            return response;
        }

        [UserSecureOperation]
        public JsonResponse<List<TimeLineDTO>> GetWeddingTimeLines(string weddingID)
        {
            ActivityLog.SetLog("GetWeddingTimeLines initiated.", LogLoc.INFO);

            JsonResponse<List<TimeLineDTO>> response = new JsonResponse<List<TimeLineDTO>>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    List<TimeLineDTO> timelines = new List<TimeLineDTO>();
                    EntityMapper.Map(WeddingBusinessInstance.GetWeddingTimeLines(Convert.ToInt32(weddingID)), timelines);

                    response.SingleResult = timelines;
                    response.Message = "Your wedding timelines are successfully fetched.";
                    response.IsSuccess = true;
                    ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = "404";
                response.Message = ex.Message;
            }

            return response;
        }

        [UserSecureOperation]
        public JsonResponse<List<WeddingGalleryDTO>> GetWeddingGallery(string weddingID)
        {
            ActivityLog.SetLog("GetWeddingTimeLines initiated.", LogLoc.INFO);

            JsonResponse<List<WeddingGalleryDTO>> response = new JsonResponse<List<WeddingGalleryDTO>>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    List<WeddingGalleryDTO> gallery = new List<WeddingGalleryDTO>();
                    EntityMapper.Map(WeddingBusinessInstance.GetWeddingGallery(Convert.ToInt32(weddingID)), gallery);

                    response.SingleResult = gallery;
                    response.Message = "Your wedding timelines are successfully fetched.";
                    response.IsSuccess = true;
                    ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.IsSuccess = false;
                response.StatusCode = "404";
            }

            return response;
        }


        #region Wedding Submit Methods implementation...


        [UserSecureOperation]
        public JsonResponse<string> UploadImages()
        {
            JsonResponse<string> response = new JsonResponse<string>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    var request = HttpContext.Current.Request;
                    var uploadfile = request.Files["Image"];


                    var bride = JsonConvert.DeserializeObject<BrideAndMaidDTO>(request.Params.Get("Bride"));
                    var userid = JsonConvert.DeserializeObject<BrideAndMaidDTO>(request.Params.Get("UserID"));

                    string type = uploadfile.FileName.Split('_')[2];
                    int typeID = Convert.ToInt32(uploadfile.FileName.Split('_')[3]);

                    string fileExtn = Path.GetExtension(uploadfile.FileName);

                    string root = AppUtil.GetUploadDirectory(AspectEnums.ImageFileTypes.User) + "U" + userid;
                    string fileDirectory = root + "/W" + bride.WeddingID + "/" + type + "/";
                    if (!Directory.Exists(fileDirectory))
                        Directory.CreateDirectory(fileDirectory);


                    string newFileName = typeID + "_" + DateTime.Now.ToShortDateString().Replace(" ", "").Replace(":", "").Replace("/", "") + fileExtn;
                    string path = fileDirectory + @"\" + newFileName;
                    uploadfile.SaveAs(path);
                    response.IsSuccess = true;
                    response.SingleResult = newFileName;


                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        [UserSecureOperation]
        public JsonResponse<string> UploadImage(Stream image)
        {
            JsonResponse<string> response = new JsonResponse<string>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    string fileDirectory = AppUtil.GetUploadDirectory(AspectEnums.ImageFileTypes.User);
                    string newFileName = AppUtil.GetUniqueKey().ToUpper() + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + ".jpeg";
                    string uploadedFileName = fileDirectory + @"\" + newFileName;
                    if (Directory.Exists(fileDirectory))
                    {
                        //FileStream fileData = null;
                        //StreamReader bodyReader = new StreamReader(image);
                        //string bodyString = bodyReader.ReadToEnd();
                        //int length = bodyString.Length;
                        string FilePath = Path.Combine(fileDirectory, newFileName);
                        int length = 0;
                        using (FileStream writer = new FileStream(FilePath, FileMode.Create))
                        {
                            int readCount;
                            var buffer = new byte[8192];
                            while ((readCount = image.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                writer.Write(buffer, 0, readCount);
                                length += readCount;
                            }
                        }


                        //using (fileData = new FileStream(uploadedFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                        //{
                        //    const int bufferLen = 900096;
                        //    byte[] buffer = new byte[bufferLen];
                        //    int count = 0;
                        //    int totalBytes = 0;
                        //    while ((count = image.Read(buffer, 0, bufferLen)) > 0)
                        //    {
                        //        totalBytes += count;
                        //        fileData.Write(buffer, 0, count);
                        //    }
                        //    image.Close();
                        //}
                        response.IsSuccess = true;
                        response.SingleResult = newFileName;
                    }


                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        [UserSecureOperation]
        public JsonResponse<int> SubmitUserWedding()
        {
            ActivityLog.SetLog("SubmitUserWedding initiated.", LogLoc.INFO);

            JsonResponse<int> response = new JsonResponse<int>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    #region PARSING AND CONVERTING FORM DATA...
                    var request = HttpContext.Current.Request;
                    var file = request.Files["Image"];
                    WeddingDTO wedding = JsonConvert.DeserializeObject<WeddingDTO>(request.Params.Get("Wedding"));
                    int UserID = JsonConvert.DeserializeObject<int>(request.Params.Get("UserID"));


                    string path = "U" + UserID + "/W{0}" + "/W";
                    string name = wedding.WeddingID > 0 ? wedding.WeddingID + ".jpg" : "{0}.jpg";
                    wedding.BackgroundImage = Domain + path + name;

                    #endregion

                    #region SAVE AND UPDATE WEDDING ENTITES....
                    WeddingBO weddingBO = new WeddingBO();
                    wedding.WeddingStyle = "Anonymous";
                    EntityMapper.Map(wedding, weddingBO);


                    ActivityLog.SetLog("EntityMapper.Map(WeddingDTO, WeddingBO); ", LogLoc.INFO);
                    response.SingleResult = WeddingBusinessInstance.SubmitUserWeddingDetail(UserID, weddingBO);
                    path = "U" + UserID + "/W" + response.SingleResult + "/W";
                    if (response.SingleResult > 0)
                    {
                        try
                        {
                            StoreImage(file, path, name, response.SingleResult);
                        }
                        catch (Exception e)
                        {
                            response.IsSuccess = false;
                            ActivityLog.SetLog("Image uploading failed! Message: " + e.Message + " Inner Ex: " + e.InnerException, LogLoc.INFO);
                        }

                        response.IsSuccess = true;
                        ActivityLog.SetLog("Wedding data submitted successfully.", LogLoc.INFO);
                    }
                    else
                    {
                        response.IsSuccess = false;
                        ActivityLog.SetLog("Wedding Data failed to submit.", LogLoc.INFO);
                    }
                    #endregion

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.StatusCode = "404";
                ActivityLog.SetLog("Error Message: " + ex.Message + " Inner Ex: " + ex.InnerException, LogLoc.ERROR);
            }
            ActivityLog.SetLog("SubmitUserWedding finished.", LogLoc.INFO);
            return response;
        }

        [UserSecureOperation]
        public JsonResponse<int> SubmitWeddingEvent()
        {
            ActivityLog.SetLog("SubmitWeddingEvent initiated.", LogLoc.INFO);

            JsonResponse<int> response = new JsonResponse<int>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    #region PARSING AND CONVERTING FORM DATA...
                    var request = HttpContext.Current.Request;
                    var eventfile = request.Files["EventImage"];
                    var venuefile = request.Files["VenueImage"];
                    EventAndVenueDTO data = JsonConvert.DeserializeObject<EventAndVenueDTO>(request.Params.Get("Event"));
                    int UserID = JsonConvert.DeserializeObject<int>(request.Params.Get("UserID"));



                    // MAPPING OF DATA TO EVENT, VENUE AND ADDRESS



                    if (data.IsDeleted && data.WeddingEventID > 0)
                    {
                        ActivityLog.SetLog("Going to delete wedding Event ID:  " + data.WeddingEventID, LogLoc.INFO);

                        response.IsSuccess = WeddingBusinessInstance.DeleteEventDetailsByID(data.WeddingEventID, UserID);
                        if (response.IsSuccess)
                            response.Message = Messages.WeddingEventDeleted;
                        else
                            response.Message = Messages.DeleteFailed;
                    }
                    else
                    {
                        ActivityLog.SetLog("Going to update/ insert new wedding Event:  " + data.Title + "of wedding ID " + data.WeddingID, LogLoc.INFO);
                        WeddingEventBO weddEventBO = new WeddingEventBO();
                        EntityMapper.Map(data, weddEventBO);

                        string path = "U" + UserID + "/W" + weddEventBO.WeddingID + "/Events/E";
                        string name = weddEventBO.WeddingEventID > 0 ? weddEventBO.WeddingEventID + ".jpg" : "{0}.jpg";
                        weddEventBO.ImageUrl = Domain + path + name;


                        #endregion
                        int eventID = WeddingBusinessInstance.SubmitWeddingEvent(UserID, weddEventBO);
                        response.SingleResult = eventID;

                        if (eventID > 0)
                        {
                            StoreImage(eventfile, path, name, eventID);

                            VenueBO venueBO = new VenueBO();
                            EntityMapper.Map(data, venueBO);
                            venueBO.IsDeleted = data.IsDeleted;
                            venueBO.WeddingEventID = eventID;
                            path = path + eventID + "/V";
                            name = venueBO.VenueID > 0 ? venueBO.VenueID + ".jpg" : "{0}.jpg";
                            venueBO.VenueImageUrl = Domain + path + name;

                            int venueID = WeddingBusinessInstance.SubmitVenue(UserID, venueBO);
                            if (venueID > 0)
                            {
                                StoreImage(venuefile, path, name, venueID);

                                AddressMasterBO addressBO = new AddressMasterBO();
                                EntityMapper.Map(data, addressBO);
                                addressBO.AddressOwnerType = (int)AspectEnums.AddressOwnerType.Venue;
                                addressBO.AddressType = (int)AspectEnums.AddressType.PartyHall;
                                addressBO.AddressOwnerTypePKID = venueID;
                                int AddressID = WeddingBusinessInstance.SubmitAddress(UserID, addressBO);

                                if (AddressID > 0)
                                {
                                    response.IsSuccess = true;
                                    response.Message = "Event successfully submitted.";
                                    ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                                }
                            }
                        }
                    }

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                ActivityLog.SetLog("Error Message: " + ex.Message + " Inner Ex: " + ex.InnerException, LogLoc.ERROR);
            }

            return response;
        }
        [UserSecureOperation]
        public JsonResponse<int> SubmitBrideMaids()
        {
            ActivityLog.SetLog("SubmitBrideMaids initiated.", LogLoc.INFO);
            JsonResponse<int> response = new JsonResponse<int>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    #region PARSING AND CONVERTING FORM DATA...
                    var request = HttpContext.Current.Request;
                    var file = request.Files["Image"];
                    BrideAndMaidDTO bride = JsonConvert.DeserializeObject<BrideAndMaidDTO>(request.Params.Get("Bride"));
                    int UserID = JsonConvert.DeserializeObject<int>(request.Params.Get("UserID"));
                    string path = "U" + UserID + "/W" + bride.WeddingID + "/Bride/B";
                    string name = bride.BrideAndMaidID > 0 ? bride.BrideAndMaidID + ".jpg" : "{0}.jpg";
                    bride.Imageurl = Domain + path + name;

                    #endregion

                    #region SAVE AND UPDATE BRIDE ENTITES....
                    BrideAndMaidBO brideBO = new BrideAndMaidBO();
                    EntityMapper.Map(bride, brideBO);
                    ActivityLog.SetLog("EntityMapper.Map(orderDTO, orderBO); ", LogLoc.INFO);
                    response.SingleResult = WeddingBusinessInstance.SubmitBrideMaids(UserID, brideBO);
                    if (response.SingleResult > 0)
                    {
                        StoreImage(file, path, name, response.SingleResult);

                        response.IsSuccess = true;
                        ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                    }
                    else
                    {
                        response.IsSuccess = false;
                        ActivityLog.SetLog("Bride Data failed to submit.", LogLoc.INFO);
                    }
                    #endregion

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                ActivityLog.SetLog("Error Message: " + ex.Message + " Inner Ex: " + ex.InnerException, LogLoc.ERROR);
            }

            return response;
        }
        [UserSecureOperation]
        public JsonResponse<int> SubmitGroomMen()
        {
            ActivityLog.SetLog("SubmitBrideMaids initiated.", LogLoc.INFO);

            JsonResponse<int> response = new JsonResponse<int>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    #region PARSING AND CONVERTING FORM DATA...
                    var request = HttpContext.Current.Request;
                    var file = request.Files["Image"];
                    GroomAndMenDTO groom = JsonConvert.DeserializeObject<GroomAndMenDTO>(request.Params.Get("Groom"));
                    int UserID = JsonConvert.DeserializeObject<int>(request.Params.Get("UserID"));
                    string name = groom.GroomAndMenID > 0 ? groom.GroomAndMenID + ".jpg" : "{0}.jpg";
                    string path = "U" + UserID + "/W" + groom.WeddingID + "/Groom/G";
                    groom.Imageurl = Domain + path + name;
                    #endregion

                    #region SAVE AND UPDATE BRIDE ENTITES....
                    GroomAndManBO groomBO = new GroomAndManBO();
                    EntityMapper.Map(groom, groomBO);
                    response.SingleResult = WeddingBusinessInstance.SubmitGroomMen(UserID, groomBO);
                    if (response.SingleResult > 0)
                    {
                        try
                        {
                            StoreImage(file, path, name, response.SingleResult);
                        }
                        catch (Exception e)
                        {
                            response.IsSuccess = false;
                            ActivityLog.SetLog("Image uploading failed! Message: " + e.Message + " Inner Ex: " + e.InnerException, LogLoc.INFO);
                        }

                        response.IsSuccess = true;
                        ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                    }
                    else
                    {
                        response.IsSuccess = false;
                        ActivityLog.SetLog("Groom Data failed to submit.", LogLoc.INFO);
                    }
                    #endregion

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                ActivityLog.SetLog("Error Message: " + ex.Message + " Inner Ex: " + ex.InnerException, LogLoc.ERROR);
            }

            return response;
        }

        [UserSecureOperation]
        public JsonResponse<int> SubmitTimeLine()
        {
            ActivityLog.SetLog("SubmitTimeLine initiated.", LogLoc.INFO);

            JsonResponse<int> response = new JsonResponse<int>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    #region PARSING AND CONVERTING FORM DATA...
                    var request = HttpContext.Current.Request;
                    var file = request.Files["Image"];
                    TimeLineDTO timeline = JsonConvert.DeserializeObject<TimeLineDTO>(request.Params.Get("TimeLine"));
                    int UserID = JsonConvert.DeserializeObject<int>(request.Params.Get("UserID"));

                    string path = "U" + UserID + "/W" + timeline.WeddingID + "/TimeLines/T";
                    string name = timeline.TimeLineID > 0 ? timeline.TimeLineID + ".jpg" : "{0}.jpg";
                    timeline.ImageUrl = Domain + path + name;

                    #endregion

                    #region SAVE AND UPDATE TIMLINE ENTITES....
                    TimeLineBO timeLineBO = new TimeLineBO();
                    EntityMapper.Map(timeline, timeLineBO);

                    int TimeLineID = WeddingBusinessInstance.SubmitTimeLine(UserID, timeLineBO);
                    response.SingleResult = TimeLineID;
                    if (TimeLineID > 0)
                    {
                        StoreImage(file, path, name, TimeLineID);
                        response.IsSuccess = true;
                        ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                    }
                    else
                    {
                        response.IsSuccess = false;
                        ActivityLog.SetLog("TimeLine Data failed to submit.", LogLoc.INFO);
                    }
                    #endregion

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                ActivityLog.SetLog("Error Message: " + ex.Message + " Inner Ex: " + ex.InnerException, LogLoc.ERROR);
            }

            return response;
        }


        private void StoreImage(HttpPostedFile file, string path, string name, int ID)
        {
            name = name.Replace("{0}", ID.ToString());// Path.GetExtension(file.FileName); Path.GetExtension(file.FileName);
            string fileDirectory = AppUtil.GetUploadDirectory(AspectEnums.ImageFileTypes.User) + path;
            if (!Directory.Exists(fileDirectory))
                Directory.CreateDirectory(fileDirectory);
            try
            {
                if (file != null)
                    file.SaveAs(fileDirectory + name);
            }
            catch (Exception e)
            {
                ActivityLog.SetLog("Image uploading failed! Message: " + e.Message + " Inner Ex: " + e.InnerException, LogLoc.INFO);
            }
        }
        public class FilesRequest
        {
            public ICollection<Stream> MyStreams { get; set; }
        }

        [UserSecureOperation]
        public JsonResponse<int> UploadGallery()
        {
            var response = new JsonResponse<int>();
            try
            {
                var context = HttpContext.Current.Request;
                if (context.Files.Count > 0)
                {
                    Stream images = HttpContext.Current.Request.InputStream;
                    var parser = new MultipartFormDataParser(images);


                    var UserID = Convert.ToInt32(context.Params["UserID"]);
                    int WeddingID = Convert.ToInt32(context.Params["WeddingID"]);

                    int counter = 0;
                    string path = "U" + UserID + "/W" + WeddingID + "/Gallery/";
                    string fileDirectory = AppUtil.GetUploadDirectory(AspectEnums.ImageFileTypes.User) + path;
                    foreach (var file in parser.Files)
                    {

                        string filename = "{0}.jpg"; //AppUtil.GetUniqueKey().ToUpper() + counter.ToString() + ".jpg";
                        if (!Directory.Exists(fileDirectory))
                            Directory.CreateDirectory(fileDirectory);

                        #region Step 1: Save DB Entity Image

                        WeddingGalleryBO BO = new WeddingGalleryBO
                        {
                            ImageName = filename,
                            ImageUrl = Domain + path + filename,
                            WeddingID = WeddingID,
                            DateTaken = DateTime.Now
                        };


                        int imageID = WeddingBusinessInstance.SubmitGallery(UserID, BO);
                        #endregion

                        FileStream fileData = null;
                        string FullFilePath = fileDirectory + @"\" + imageID + ".jpg";

                        #region Step 1: Store Image
                        //fileData = new FileStream(file.Data);
                        using (fileData = new FileStream(FullFilePath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            file.Data.CopyTo(fileData);
                            file.Data.Close();
                        }
                        #endregion




                        response.SingleResult = counter;


                    }
                    response.IsSuccess = true;
                    response.Message = counter + " images uploaded successfully";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        [UserSecureOperation]
        public JsonResponse<int> SubmitWeddingGallery(int userId, List<WeddingGalleryDTO> galleryDTO)
        {
            ActivityLog.SetLog("SubmitWeddingGallery initiated.", LogLoc.INFO);

            JsonResponse<int> response = new JsonResponse<int>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    List<WeddingGalleryBO> lstGalleryBO = new List<WeddingGalleryBO>();
                    EntityMapper.Map(galleryDTO, lstGalleryBO);
                    ActivityLog.SetLog("EntityMapper.Map(orderDTO, orderBO); ", LogLoc.INFO);
                    foreach (var gallery in lstGalleryBO)
                    {
                        response.SingleResult = WeddingBusinessInstance.SubmitGallery(userId, gallery);
                        if (response.SingleResult == 0)
                            break;
                        response.IsSuccess = true;
                    }

                    if (response.SingleResult == 0)
                    {
                        response.Message = "Something Went Wrong. Try again later.";
                        response.IsSuccess = false;
                    }


                    ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }


        #endregion

        [UserSecureOperation]
        public JsonResponse<string> SendWelcomeEmail(EmailServiceDTO model)
        {
            JsonResponse<string> response = new JsonResponse<string>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    EmailServiceDTO emailService = new EmailServiceDTO();

                    var template = SystemBusinessInstance.GetTemplateData(model.TemplateID, null);
                    model.Body = template.TemplateContent;
                    response.SingleResult = model.Body;

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        [UserSecureOperation]
        public JsonResponse<List<OrderMasterDTO>> GetUserOrders(int UserID)
        {
            ActivityLog.SetLog("GetWeddingDetailByID initiated.", LogLoc.INFO);

            JsonResponse<List<OrderMasterDTO>> response = new JsonResponse<List<OrderMasterDTO>>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    List<OrderMasterDTO> orders = new List<OrderMasterDTO>();
                    EntityMapper.Map(WeddingBusinessInstance.GetUserOrders(UserID), orders);

                    if (orders.Count < 0)
                    {
                        response.Message = "Your wedding subscription is expired.";
                    }
                    response.SingleResult = orders;
                    response.IsSuccess = true;
                    ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        [UserSecureOperation]
        public JsonResponse<int> SubmitUserOrder(OrderMasterDTO orderDTO)
        {
            ActivityLog.SetLog("SubmitUserOrder initiated.", LogLoc.INFO);

            JsonResponse<int> response = new JsonResponse<int>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    OrderMasterBO orderBO = new OrderMasterBO();
                    EntityMapper.Map(orderDTO, orderBO);
                    ActivityLog.SetLog("EntityMapper.Map(orderDTO, orderBO); ", LogLoc.INFO);
                    response.SingleResult = WeddingBusinessInstance.SubmitUserOrder(orderBO);

                    if (response.SingleResult == 0)
                    {
                        response.Message = "Something Went Wrong. Try again later.";
                    }

                    response.IsSuccess = true;
                    ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        [UserSecureOperation]
        public JsonResponse<OrderMasterDTO> GetOrderByID(int OrderID)
        {
            ActivityLog.SetLog("GetOrder DetailByID initiated.", LogLoc.INFO);

            JsonResponse<OrderMasterDTO> response = new JsonResponse<OrderMasterDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    OrderMasterDTO orders = new OrderMasterDTO();
                    EntityMapper.Map(WeddingBusinessInstance.GetOrderByID(OrderID), orders);

                    response.SingleResult = orders;
                    response.IsSuccess = true;
                    ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        [UserSecureOperation]
        public JsonResponse<int> SubmitUserWedding(int UserID, WeddingDTO model)
        {
            ActivityLog.SetLog("SubmitWedding initiated.", LogLoc.INFO);

            JsonResponse<int> response = new JsonResponse<int>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    WeddingBO wedding = new WeddingBO();
                    EntityMapper.Map(model, wedding);

                    int weddingID = WeddingBusinessInstance.SubmitUserWeddingDetail(UserID, wedding);
                    response.IsSuccess = true;
                    response.SingleResult = weddingID;

                    if (weddingID == 0)
                        response.Message = "Congratulations! Your wedding has been created successfully.";
                    else
                        response.Message = "Your wedding has been updated successfully.";

                    ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }



        public JsonResponse<string> GetTemplateContent(int TemplateID)
        {
            ActivityLog.SetLog("GetTemplateContent initiated for template ID : " + TemplateID, LogLoc.INFO);

            JsonResponse<string> response = new JsonResponse<string>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    string content = SystemBusinessInstance.GetTemplateData(TemplateID, 0).TemplateContent;
                    response.IsSuccess = true;
                    response.SingleResult = content;
                    response.Message = "Success";
                    ActivityLog.SetLog("Response is Success.", LogLoc.INFO);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        public JsonResponse<string> GetTemplateContent(string TemplateID)
        {
            throw new NotImplementedException();
        }
    }
}
