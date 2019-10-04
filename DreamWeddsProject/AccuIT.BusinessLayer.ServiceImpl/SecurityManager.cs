using AccuIT.BusinessLayer.Base;
using AccuIT.BusinessLayer.Services.BO;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.PersistenceLayer.Repository.Contracts;
using AccuIT.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.ServiceImpl
{
    /// <summary>
    /// Business class to define and implement security rules and validation in application
    /// </summary>
    public class SecurityManager : ServiceBase, ISecurityService
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.SECURITY_REPOSITORY)]
        public ISecurityRepository SecurityRepository { get; set; }

        #endregion

        /// <summary>
        /// Validate GUID in the link of forget password email
        /// </summary>
        /// <param name="GUID"> uniqe string </param>
        /// <returns>true if GUID in the URL is correct</returns>
        public bool ValidateGUID(string GUID)
        {
            return SecurityRepository.ValidateGUID(GUID);
        }

        // <summary>
        /// Change password of User
        /// </summary>
        /// <param name="GUID"> uniqe string </param>
        /// <param name="Password">password entered by user</param>
        /// <returns></returns>
        public bool ChangePassword(string GUID, string Password)
        {
            return SecurityRepository.ChangePassword(GUID, Password);
        }

        public bool UpdatePassword(int UserID, string Password)
        {
            return SecurityRepository.UpdatePassword(UserID, Password);
        }


        /// <summary>
        /// Authenticate OTP (One Time Password) entered by user
        /// </summary>
        /// <param name="empID">Userid</param>
        /// <param name="otp">One Time Password</param>
        /// <returns>reurns true if user have enterered latest OTP</returns>
        public bool AuthenticateOTP(int empID, string otp, out string GuidString, out int MaxAttempts)
        {
            return SecurityRepository.AuthenticateOTP(empID, otp, out GuidString, out MaxAttempts);
        }

        /// <summary>
        /// save OTP (One Time Password) to database
        /// </summary>
        /// <param name="otp"> Object of OTP</param>
        /// <returns>returns true when data is saved</returns>
        public bool SaveOTP(OTPBO otp)
        {
            OTPMaster otpmaster = new OTPMaster();
            ObjectMapper.Map(otp, otpmaster);
            return SecurityRepository.SaveOTP(otpmaster);
        }

        /// <summary>
        /// Method to fetch user authorization parameters for various modules in application
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns entity collection</returns>
        public IList<SecurityAspectBO> GetUserAuthorization(int userID)
        {
            IList<SecurityAspectBO> permissions = new List<SecurityAspectBO>();
            ObjectMapper.Map(SecurityRepository.GetUserAuthorization(userID), permissions);
            return permissions;
        }

        //#region Forgot Password Functions
        /// <summary>
        /// Validate Employee if given Employee Code is correct or not
        /// </summary>
        /// <param name="EmplCode">Employee Code of User</param>
        /// <param name="Type">Validation type (Only Employee Code, Employee Code and Email etc)</param>
        /// <returns></returns>
        public bool ValidateUser(int empID, AspectEnums.UserValidationType Type)
        {
            return SecurityRepository.ValidateUser(empID, Type);
        }

        ///// <summary>
        ///// Get UserID By Employee Code given in parameter
        ///// </summary>
        ///// <param name="EmplCode"></param>
        ///// <returns></returns>
        //public long? GetUserIDByEmployeeCode(string EmplCode)
        //{
        //    return SecurityRepository.GetUserIDByEmployeeCode(EmplCode);
        //}

        ///// <summary>
        ///// Get Email Template based on ID
        ///// </summary>
        ///// <param name="TemplateTypeID">Template ID</param>
        ///// <returns>Obejct of EmailTemplate</returns>
        //public EmailTemplateBO GetEmailTemplate(AspectEnums.EmailTemplateType TemplateTypeID)
        //{
        //    EmailTemplateBO emailTemplate = new EmailTemplateBO();
        //    ObjectMapper.Map(SecurityRepository.GetEmailTemplate(TemplateTypeID), emailTemplate);
        //    return emailTemplate;
        //}





  
        //#endregion
    }
}
