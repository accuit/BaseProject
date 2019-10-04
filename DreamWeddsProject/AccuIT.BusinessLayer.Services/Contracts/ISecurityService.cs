using AccuIT.BusinessLayer.Services.BO;
using AccuIT.CommonLayer.Aspects.Utilities;
using System.Collections.Generic;

namespace AccuIT.BusinessLayer.Services.Contracts
{
    public interface ISecurityService
    {


        /// <summary>
        /// Authenticate OTP (One Time Password) entered by user
        /// </summary>
        /// <param name="userid">Userid</param>
        /// <param name="otp">One Time Password</param>
        /// <returns>reurns true if user have enterered latest OTP</returns>
        bool AuthenticateOTP(int userid, string otp, out string GuidString, out int MaxAttempts);

        /// <summary>
        /// Validate GUID in the link of forget password email
        /// </summary>
        /// <param name="GUID"> uniqe string </param>
        /// <returns>true if GUID in the URL is correct</returns>
        bool ValidateGUID(string GUID);

        // <summary>
        /// Change password of User
        /// </summary>
        /// <param name="GUID"> uniqe string </param>
        /// <param name="Password">password entered by user</param>
        /// <returns></returns>
        bool ChangePassword(string GUID, string Password);

         bool UpdatePassword(int UserID, string Password);

        /// <summary>
        /// save OTP (One Time Password) to database
        /// </summary>
        /// <param name="otp"> Object of OTP</param>
        /// <returns>returns true when data is saved</returns>
        bool SaveOTP(OTPBO otp);

        /// <summary>
        /// Method to fetch user authorization parameters for various modules in application
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns entity collection</returns>
        IList<SecurityAspectBO> GetUserAuthorization(int userID);
        //#region Forgot Password Functions
        /// <summary>
        /// Validate Employee if given Employee Code is correct or not
        /// </summary>
        /// <param name="EmplCode">Employee Code of User</param>
        /// <param name="Type">Validation type (Only Employee Code, Employee Code and Email etc)</param>
        /// <returns></returns>
        bool ValidateUser(int empID, AspectEnums.UserValidationType Type);

        ///// <summary>
        ///// Get UserID By Employee Code given in parameter
        ///// </summary>
        ///// <param name="EmplCode"></param>
        ///// <returns></returns>
        //long? GetUserIDByEmployeeCode(string EmplCode);

        ///// <summary>
        ///// Get Email Template based on ID
        ///// </summary>
        ///// <param name="TemplateTypeID">Template ID</param>
        ///// <returns>Obejct of EmailTemplate</returns>
        //EmailTemplateBO GetEmailTemplate(AspectEnums.EmailTemplateType TemplateTypeID);





        //#endregion
    }
}
