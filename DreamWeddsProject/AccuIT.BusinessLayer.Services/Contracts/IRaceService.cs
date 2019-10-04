using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samsung.SmartDost.BusinessLayer.Services.Contracts
{
    public interface IRaceService
    {
     
        /// <summary>
        /// Method to Get Audit Data
        /// </summary>
        /// <param name="AuditSearchDTO">auditSearchDTO</param>
        /// <returns>returns Audit data</returns>
        List<AuditSummarySearchDTO> GetSearchAuditdata(AuditSearchDTO auditSearchDTO, long userID, long RoleID);

        /// <summary>
        /// Select list of modules in a survey Response
        /// </summary>
        /// <param name="surveyResponseID"></param>
        /// <returns></returns>
        SurveyAuditDTO GetSurveyModulesList(long surveyResponseID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productAuditDTO"></param>
        /// <returns></returns>

        List<ProductAuditSummaryDTO> GetProductAuditdata(ProductAuditDTO productAuditDTO);


        bool submitReviewerResponse(ReviewerResponseDTO reviewerResponse, long userID, long RoleID);

        /// <summary>
        ///Get geo definitions 
        /// </summary>
        /// <returns></returns>
        List<GeoDefinitionDTO> GetGeoDefinitions();

        /// <summary>
        /// Audit response in detail
        /// </summary>
        /// <param name="surveyResponseID"></param>
        /// <param name="AuditID"></param>
        /// <returns></returns>
        AuditDetailsDTO getAuditDetails(long surveyResponseID, int AuditID);

        /// <summary>
        /// authenticate user by credentials
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        QCLoginResponseDTO LoginWebUser(string userName, string password);
    }
}
