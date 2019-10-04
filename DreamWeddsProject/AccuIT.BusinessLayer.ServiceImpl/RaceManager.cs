using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samsung.SmartDost.BusinessLayer.Base;
using Samsung.SmartDost.BusinessLayer.Services.Contracts;
using Samsung.SmartDost.PersistenceLayer.Repository.Contracts;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using Samsung.SmartDost.CommonLayer.Aspects.Security;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;

namespace Samsung.SmartDost.BusinessLayer.ServiceImpl
{
    public class RaceManager : RaceBaseService, IRaceService
    {
        /// <summary>
        /// Property to inject the store persistence layer object invocation
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.RACE_REPOSITORY)]
        public IRaceRepository RaceRepository { get; set; }

        /// <summary>
        /// Property to inject the user persistence layer object invocation
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.USER_REPOSITORY)]        
        public IUserRepository UserRepository { get; set; }



        public List<AuditSummarySearchDTO> GetSearchAuditdata(AuditSearchDTO auditSearchDTO, long userID, long RoleID)
        {
            List<AuditSummarySearchDTO> result = new List<AuditSummarySearchDTO>();
            AuditSearch request = new AuditSearch();
            ObjectMapper.Map(auditSearchDTO, request);

            ObjectMapper.Map(RaceRepository.GetSearchAuditdata(request,userID, RoleID), result);
            return result;
        }

        public List<ProductAuditSummaryDTO> GetProductAuditdata(ProductAuditDTO productAuditDTO)
        {
            List<ProductAuditSummaryDTO> result = new List<ProductAuditSummaryDTO>();           
            return result;
        }

        /// <summary>
        /// Audit response in detail
        /// </summary>
        /// <param name="surveyResponseID"></param>
        /// <param name="AuditID"></param>
        /// <returns></returns>
        public AuditDetailsDTO getAuditDetails(long surveyResponseID, int AuditID)
        {
            AuditDetailsDTO result = new AuditDetailsDTO();

            List<AuditQuestionDetailsDTO> auditQuestionDetails = new List<AuditQuestionDetailsDTO>();
            ObjectMapper.Map(RaceRepository.GetSurveyModulesList(surveyResponseID), auditQuestionDetails);
            result.auditDetails = auditQuestionDetails;

            List<ProductAuditSummaryDTO> productAuditSummary = new List<ProductAuditSummaryDTO>();
            ObjectMapper.Map(RaceRepository.GetProductAuditdata(AuditID), productAuditSummary);
            result.productAuditSummary = productAuditSummary;

            StoreGeoTagDTO geoTag = new StoreGeoTagDTO();
            ObjectMapper.Map(RaceRepository.GetStoreDetails(surveyResponseID), geoTag);
            result.tagDetails = geoTag;


            List<AuditLogDetailsDTO> auditlog = new List<AuditLogDetailsDTO>();
            ObjectMapper.Map(RaceRepository.getauditLogDetails(AuditID), auditlog);

            foreach (var item in auditlog)
            {
                var currentDate = DateTime.Today.AddDays(1); //being used to calculate days 
                int days = currentDate.Subtract(item.CreatedDate).Days;
                if (days > 0)
                {
                    item.Days = days.ToString() + " Day(s) ago";
                }
                else
                {
                    item.Days = " today";
                }
            }
            

            result.auditLogDetails = auditlog;
            return result;
        }

        /// <summary>
        /// Select list of modules in a survey Response
        /// </summary>
        /// <param name="surveyResponseID"></param>
        /// <returns></returns>
        public SurveyAuditDTO GetSurveyModulesList(long surveyResponseID)
        {
            SurveyAuditDTO result = new SurveyAuditDTO();
            
            ObjectMapper.Map(RaceRepository.GetSurveyModulesList(surveyResponseID), result);

            string fileDirectory = string.Empty;
            fileDirectory = AppUtil.GetUploadDirectory(AspectEnums.ImageFileTypes.Survey);


            foreach (var module in result.modules)
            {
                List<SurveyQuestionRepeatResponseDTO> surveyRepeatResponse = new List<SurveyQuestionRepeatResponseDTO>();
                foreach (var moduleresponse in module.surveyQuestionResponse)
                {
                    #region add singular question in list
                    surveyRepeatResponse.Add(new SurveyQuestionRepeatResponseDTO()
                    {
                        surveyQuestion = moduleresponse.surveyQuestion,
                        userResponse = moduleresponse.surveyUserResponse.UserResponse,
                        ModuleID = module.ModuleID,
                        ModuleCode = module.ModuleCode
                    });
                    #endregion

                    #region add repeat question in list
                    if (moduleresponse.surveyUserResponse.SurveyRepeatResponses.Count > 0)
                    {                        
                        foreach (var item in moduleresponse.surveyUserResponse.SurveyRepeatResponses)
                        {                           
                            surveyRepeatResponse.Add(new SurveyQuestionRepeatResponseDTO()
                            {
                                surveyQuestion = moduleresponse.surveyQuestion + " --> " + moduleresponse.RepeaterText + " " + item.SurveyQuestionRepeaterID.ToString(),
                                userResponse = item.UserResponse,
                                ModuleID = module.ModuleID,
                                imagePath = item.UserResponse.Contains(".jpeg") ?
                                    fileDirectory + @"\" + item.UserResponse
                                    : "",
                                ModuleCode = module.ModuleCode
                            });                            
                        }
                    }
                    #endregion
                    

                }
               module.surveyRepeatResponse = surveyRepeatResponse;
            }

            foreach (var item in result.auditLogDetails)
            {
                var currentDate = DateTime.Today.AddDays(1); //being used to calculate days 
                int days = currentDate.Subtract(item.CreatedDate).Days;
                if (days > 0)
                {
                    item.Days = days.ToString() + " Day(s) ago";
                }
                else
                {
                    item.Days = " today";
                }
            }
           
            return result;
        }

        public bool submitReviewerResponse(ReviewerResponseDTO reviewerResponse,long userID, long RoleID)
        {

            ReviewerResponse request = new ReviewerResponse();
            ObjectMapper.Map(reviewerResponse, request);
            return RaceRepository.submitReviewerResponse(request, userID, RoleID);
        }
      
        /// <summary>
        ///Get geo definitions 
        /// </summary>
        /// <returns></returns>
        public List<GeoDefinitionDTO> GetGeoDefinitions()
        {
            List<GeoDefinitionDTO> result = new List<GeoDefinitionDTO>();
            ObjectMapper.Map(RaceRepository.GetGeoDefinitions(), result);
            return result;

        }

        public QCLoginResponseDTO LoginWebUser(string userName, string password)
        {
            QCLoginResponseDTO result = new QCLoginResponseDTO();
            //generate apikey token
            var APIKey = AppUtil.GetUniqueKey();
            var APIToken = DateTime.Now.ToString().GetHashCode().ToString("x");        
    
            //authenticate user
            result.loginStatus = UserRepository.LoginWebUser(userName, EncryptionEngine.EncryptString(password));
            result.APIKey = APIKey;
            result.APIToken = APIToken;

            //save apikey and token in Database
            if(result.loginStatus>0)
                RaceRepository.generateAPIKeyToken(APIKey, APIToken, result.loginStatus);

            return result;                
        }
    }
}
