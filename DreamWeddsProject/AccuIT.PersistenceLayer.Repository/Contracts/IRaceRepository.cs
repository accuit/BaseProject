using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Samsung.SmartDost.PersistenceLayer.Repository.Contracts
{
    public interface IRaceRepository
    {       

        /// <summary>
        /// Method to return Audit data
        /// </summary>
        /// <param name="AuditSearchDTO">auditSearchDTO</param>
        /// <returns>returns Audit Data</returns>

        List<SpAuditSummarySearch_Result> GetSearchAuditdata(AuditSearch auditSearch, long userID, long RoleID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="productAuditDTO"></param>
        /// <returns></returns>
        List<SpProductAuditSummary_Result> GetProductAuditdata(int auditID);
        


        /// <summary>
        /// Select list of modules in a survey Response
        /// </summary>
        /// <param name="surveyResponseID"></param>
        /// <returns></returns>
        //SpProductAuditSummary_Result GetSurveyModulesList(long surveyResponseID);
        List<SPGetAuditQuestionDetails_Result> GetSurveyModulesList(long surveyResponseID);

        /// <summary>
        /// Submit approve/reject response from QC Tool
        /// </summary>
        /// <param name="reviewerResponse"></param>
        /// <returns></returns>
        bool submitReviewerResponse(ReviewerResponse reviewerResponse, long userID, long RoleID);

    
        /// <summary>
        ///Get geo definitions 
        /// </summary>
        /// <returns></returns>
        List<GeoDefinition> GetGeoDefinitions();

        StoreGeoTag GetStoreDetails(long surveyResponseID);

        /// <summary>
        /// Return Audit log Details
        /// </summary>
        /// <param name="auditID"></param>
        /// <returns></returns>
        List<AuditLogDetails> getauditLogDetails(int auditID);

        /// <summary>
        /// Save APIKey and Token for QC user
        /// </summary>
        /// <param name="APIKey"></param>
        /// <param name="APIToken"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        bool generateAPIKeyToken(string APIKey, string APIToken,long userID);

    }
}
