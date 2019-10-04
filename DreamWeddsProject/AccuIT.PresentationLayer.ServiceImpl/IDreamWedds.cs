using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using AccuIT.BusinessLayer.Services.BO;
using AccuIT.CommonLayer.Aspects.DTO;
using System;
using AccuIT.PresentationLayer.ServiceImpl.CORS;

namespace AccuIT.PresentationLayer.ServiceImpl
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ISmartDost" in both code and config file together.
    [ServiceContract]
    public interface IDreamWedds
    {
        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "*")]
        void GetOptions();

        [WebInvoke(Method = "POST",
                    ResponseFormat = WebMessageFormat.Json,
                    BodyStyle = WebMessageBodyStyle.WrappedRequest,
                    RequestFormat = WebMessageFormat.Json
                   )]
        [OperationContract]
        string GetData(string value);


        #region User Profile Service Methods
        /// <summary>
        /// Method is created by Shalu Chaudhary:20-May-2014
        /// </summary>
        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/CommonLoginService")]
        JsonResponse<ServiceOutputDTO> CommonLoginService(string username, string password);


        #region Hide SDCE-3759

        /// <summary>
        /// Method to authenticate user in system
        /// </summary>
        /// <param name="imei">mobile imei number</param>
        /// <param name="password">password</param>
        /// <param name="geoTag">lattitude value</param>
        /// <param name="longitude">longitude value</param>
        /// <returns>returns status</returns>

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/AuthenticateUser")]
        JsonResponse<ServiceResponseDTO> AuthenticateUser(string imei, string LoginName, string password, string lattitude, string longitude, string BrowserName, string ModelName, string IPaddress);
        #endregion

        #region Hide SDCE-3759

        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/AuthUser")]
        //JsonResponse<AuthUserDTO> AuthUser(string imei);
        #endregion
        /// <summary>
        /// Method to reset user password in system
        /// </summary>
        /// <param name="userid">userid</param>
        /// <param name="newpassword">new password</param>
        /// <returns>returns status</returns>

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/ForgetPassword")]
        JsonResponse<ForgotPasswordDTO> ForgetPassword(string imei, string loginName, string newPassword);


        [OperationContract]
        [WebInvoke(Method = "POST",
          BodyStyle = WebMessageBodyStyle.WrappedRequest,
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "/UpdatePassword")]
        JsonResponse<bool> UpdatePassword(int UserID, string Password);


        [OperationContract]
        [WebInvoke(Method = "POST",
          BodyStyle = WebMessageBodyStyle.WrappedRequest,
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "/UpdateUserProfile")]
        JsonResponse<bool> UpdateUserProfile(UserProfileDTO userProfile);

        /// <summary>
        /// Displays the user profile.
        /// </summary>
        /// <param name="userId">The userId.</param>
        /// <returns></returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/DisplayUserProfile/")]
        JsonResponse<UserProfileDTO> DisplayUserProfile(string userID);


        /// <summary>
        /// Method to reset the login values after user gets logout
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>returns boolean status</returns>
        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/LogoutUser")]
        JsonResponse<bool> LogoutUser(int userID);

        #endregion

        #region All Wedding APIs

        #region Get APIs of Wedding Data
        [OperationContract]
        [WebInvoke(Method = "GET",
           BodyStyle = WebMessageBodyStyle.WrappedRequest,
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/GetWeddingBrideMaids/{weddingID}")]
        JsonResponse<List<BrideAndMaidDTO>> GetWeddingBrideMaids(string weddingID);

        [OperationContract]
        [WebInvoke(Method = "GET",
         BodyStyle = WebMessageBodyStyle.WrappedRequest,
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "/GetWeddingGroomMen/{weddingID}")]
        JsonResponse<List<GroomAndMenDTO>> GetWeddingGroomMen(string weddingID);

        [OperationContract]
        [WebInvoke(Method = "GET",
         BodyStyle = WebMessageBodyStyle.WrappedRequest,
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "/GetWeddingEvents/{weddingID}")]
        JsonResponse<List<WeddingEventDTO>> GetWeddingEvents(string weddingID);

        [OperationContract]
        [WebInvoke(Method = "GET",
         BodyStyle = WebMessageBodyStyle.WrappedRequest,
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "/GetWeddingTimeLines/{weddingID}")]
        JsonResponse<List<TimeLineDTO>> GetWeddingTimeLines(string weddingID);



        [OperationContract]
        [WebInvoke(Method = "GET",
         BodyStyle = WebMessageBodyStyle.WrappedRequest,
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "/GetWeddingGallery/{weddingID}")]
        JsonResponse<List<WeddingGalleryDTO>> GetWeddingGallery(string weddingID);
        





        [OperationContract]
        [WebInvoke(Method = "GET",
           BodyStyle = WebMessageBodyStyle.WrappedRequest,
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/GetWeddingEventByID/{eventID}")]
        JsonResponse<WeddingEventDTO> GetWeddingEventByID(string eventID);

        [OperationContract]
        [WebInvoke(Method = "GET",
           BodyStyle = WebMessageBodyStyle.WrappedRequest,
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/GetBrideMaidByID/{brideID}")]
        JsonResponse<BrideAndMaidDTO> GetBrideMaidByID(string brideID);

        [OperationContract]
        [WebInvoke(Method = "GET",
         BodyStyle = WebMessageBodyStyle.WrappedRequest,
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "/GetGroomMenByID/{groomID}")]
        JsonResponse<GroomAndMenDTO> GetGroomMenByID(string groomID);


        [OperationContract]
        [WebInvoke(Method = "GET",
         BodyStyle = WebMessageBodyStyle.WrappedRequest,
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "/GetTimeLineByID/{timelineID}")]
        JsonResponse<TimeLineDTO> GetTimeLineByID(string timelineID);

        [OperationContract]
        [WebInvoke(Method = "GET",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/GetWeddingDetailByID/{weddingID}")]
        JsonResponse<WeddingDTO> GetWeddingDetailByID(string weddingID);

        #endregion

        #region Post Wedding Data For Angular APP

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/SubmitUserWedding/")]
        JsonResponse<int> SubmitUserWedding();

        [OperationContract]
        [WebInvoke(Method = "POST",
             BodyStyle = WebMessageBodyStyle.WrappedRequest,
             RequestFormat = WebMessageFormat.Json,
             ResponseFormat = WebMessageFormat.Json,
             UriTemplate = "/submitWeddingEvent/")]
        JsonResponse<int> SubmitWeddingEvent();

        [OperationContract]
        [WebInvoke(Method = "POST",
             BodyStyle = WebMessageBodyStyle.WrappedRequest,
             RequestFormat = WebMessageFormat.Json,
             ResponseFormat = WebMessageFormat.Json,
             UriTemplate = "/SubmitBrideMaids/")]
        JsonResponse<int> SubmitBrideMaids();


        [OperationContract]
        [WebInvoke(Method = "POST",
             BodyStyle = WebMessageBodyStyle.WrappedRequest,
             RequestFormat = WebMessageFormat.Json,
             ResponseFormat = WebMessageFormat.Json,
             UriTemplate = "/SubmitGroomMen/")]
        JsonResponse<int> SubmitGroomMen();

        [OperationContract]
        [WebInvoke(Method = "POST",
             BodyStyle = WebMessageBodyStyle.WrappedRequest,
             RequestFormat = WebMessageFormat.Json,
             ResponseFormat = WebMessageFormat.Json,
             UriTemplate = "/SubmitTimeLine/")]
        JsonResponse<int> SubmitTimeLine();


        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.Bare,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/UploadImage")]
        JsonResponse<string> UploadImage(Stream image);

        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/UploadImages")]
        JsonResponse<string> UploadImages();

        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/UploadGallery")]
        JsonResponse<int> UploadGallery();

        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/SubmitWeddingGallery")]
        JsonResponse<int> SubmitWeddingGallery(int userId, List<WeddingGalleryDTO> galleryDTO);

        #endregion
        #endregion

        [OperationContract]
        [WebInvoke(Method = "POST",
            BodyStyle = WebMessageBodyStyle.WrappedRequest,
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            UriTemplate = "/SendWelcomeEmail")]
        JsonResponse<string> SendWelcomeEmail(EmailServiceDTO model);

        [OperationContract]
        [WebInvoke(Method = "POST",
          BodyStyle = WebMessageBodyStyle.WrappedRequest,
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json,
          UriTemplate = "/GetUserOrders")]
        JsonResponse<List<OrderMasterDTO>> GetUserOrders(int UserID);

        [OperationContract]
        [WebInvoke(Method = "POST",
           BodyStyle = WebMessageBodyStyle.WrappedRequest,
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json,
           UriTemplate = "/GetOrderByID")]
        JsonResponse<OrderMasterDTO> GetOrderByID(int OrderID);

        [OperationContract]
        [WebInvoke(Method = "POST",
        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        RequestFormat = WebMessageFormat.Json,
        ResponseFormat = WebMessageFormat.Json,
        UriTemplate = "/SubmitUserOrder")]
        JsonResponse<int> SubmitUserOrder(OrderMasterDTO order);


        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //   BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //   RequestFormat = WebMessageFormat.Json,
        //   ResponseFormat = WebMessageFormat.Json,
        //   UriTemplate = "/SubmitUserWedding")]
        //JsonResponse<int> SubmitUserWedding(int UserID, WeddingDTO wedding);

        [OperationContract]
        [WebInvoke(Method = "POST",
                 BodyStyle = WebMessageBodyStyle.WrappedRequest,
                 RequestFormat = WebMessageFormat.Json,
                 ResponseFormat = WebMessageFormat.Json,
                 UriTemplate = "/DisplayUserDashboard")]
        JsonResponse<List<UserWeddingSubscriptionDTO>> DisplayUserDashboard(string userID);


        #region LIVE WEDDING DATA ........................

        [OperationContract]
        [WebInvoke(Method = "POST",
         BodyStyle = WebMessageBodyStyle.WrappedRequest,
         RequestFormat = WebMessageFormat.Json,
         ResponseFormat = WebMessageFormat.Json,
         UriTemplate = "/GetTemplateContent")]
        JsonResponse<string> GetTemplateContent(string TemplateID);




        #endregion



    }
}
