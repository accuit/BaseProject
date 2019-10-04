using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.CommonLayer.Aspects.Exceptions;
using Samsung.SmartDost.CommonLayer.Aspects.ReportBO;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using Samsung.SmartDost.CommonLayer.Resources;
using Samsung.SmartDost.PresentationLayer.ServiceImpl.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samsung.SmartDost.PresentationLayer.ServiceImpl
{
    public partial class SmartDost : BaseService
    {
        
        [UserSecureOperation]
        public JsonResponse<AuditSummarySearchDTO> GetSearchAuditdata(AuditSearchDTO auditSearchDTO, long userID, long RoleID)
        {

            JsonResponse<AuditSummarySearchDTO> response = new JsonResponse<AuditSummarySearchDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.Result = RaceBusinessInstance.GetSearchAuditdata(auditSearchDTO,userID, RoleID);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {                
                response.Message = ex.Message;
            }
            return response;

        }

        [UserSecureOperation]
        public JsonResponse<ProductAuditSummaryDTO> GetProductAuditdata(ProductAuditDTO productAuditDTO, long userID, long RoleID)
        {
            JsonResponse<ProductAuditSummaryDTO> response = new JsonResponse<ProductAuditSummaryDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.Result = RaceBusinessInstance.GetProductAuditdata(productAuditDTO);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {                
                response.Message = ex.Message;
            }
            return response;
        }
        
        /// <summary>
        /// Select list of modules in a survey Response
        /// </summary>
        /// <param name="surveyResponseID"></param>
        /// <returns></returns>
        /// 
        [UserSecureOperation]
        public JsonResponse<SurveyAuditDTO> GetSurveyModulesList(long surveyResponseID, long userID, long RoleID)
        {

            JsonResponse<SurveyAuditDTO> response = new JsonResponse<SurveyAuditDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.SingleResult = RaceBusinessInstance.GetSurveyModulesList(surveyResponseID);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {                
                response.Message = ex.Message;
            }
            return response;
        }

        [UserSecureOperation]
        public JsonResponse<bool> submitReviewerResponse(ReviewerResponseDTO reviewerResponse, long userID, long RoleID)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.SingleResult = RaceBusinessInstance.submitReviewerResponse(reviewerResponse,userID, RoleID);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;


            //ReviewerResponseDTO response = new ReviewerResponseDTO();
            //ObjectMapper.Map(reviewerResponse, response);
            //return ActivityRepository.submitReviewerResponse(response);
        }

   
        /// <summary>
        ///Get geo definitions 
        /// </summary>
        /// <returns></returns>
        /// 
        [UserSecureOperation]
        public JsonResponse<GeoDefinitionDTO> GetGeoDefinitions(long userID, long RoleID)
        {
            JsonResponse<GeoDefinitionDTO> response = new JsonResponse<GeoDefinitionDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.Result = RaceBusinessInstance.GetGeoDefinitions();
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;           

        }

        /// <summary>
        /// Method to login  user into the application
        /// </summary>
        /// <param name="userName">user name</param>
        /// <param name="password">password</param>
        /// <returns>returns login status</returns>
        ///         
        public JsonResponse<QCLoginResponseDTO> LoginWebUser(string userName, string password)
        {
            JsonResponse<QCLoginResponseDTO> response = new JsonResponse<QCLoginResponseDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.SingleResult = RaceBusinessInstance.LoginWebUser(userName, password);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;            
        }
        
        [UserSecureOperation]
        public JsonResponse<UserProfileDTO> DisplayRaceUserProfile(long userID)
        {            
            JsonResponse<UserProfileDTO> response = new JsonResponse<UserProfileDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
               {

                   UserProfileDTO objUserProfileDTO = new UserProfileDTO();

                   UserProfileBO objUserProfileBO = UserBusinessInstance.DisplayUserProfile(userID);
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

        [UserSecureOperation]
        public JsonResponse<AuditDetailsDTO> GetAuditDetails(long surveyResponseID, int AuditID, long userID, long RoleID)
        {

            JsonResponse<AuditDetailsDTO> response = new JsonResponse<AuditDetailsDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.SingleResult = RaceBusinessInstance.getAuditDetails(surveyResponseID, AuditID);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;             
        }
      
    }
}
