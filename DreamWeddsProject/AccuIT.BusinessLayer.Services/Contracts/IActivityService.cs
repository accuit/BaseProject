using Samsung.SmartDost.BusinessLayer.Services.BO;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Samsung.SmartDost.BusinessLayer.Services.Contracts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IActivityService
    {
        ///// <summary>
        ///// Method to save store survey response on the basis of coverage beat
        ///// </summary>
        ///// <param name="storeSurvey">store survey</param>
        ///// <returns>returns status</returns>
        //long SaveStoreSurveyResponse(SurveyResponseDTO storeSurvey);

        ///// <summary> 
        ///// Method to save user activities on the basis of store survey data
        ///// </summary>
        ///// <param name="activities">activities performed</param>
        ///// <param name="userID">user primary ID</param>
        ///// <returns>returns status</returns>
        //int SaveSurveyUserResponse(IList<SurveyUserResponseDTO> activities, long userID, bool saveImage = true);
        
        
        /////// <summary>
        /////// Method to save user activities on the basis of general survey data
        /////// </summary>
        /////// <param name="activities">activities performed</param>
        /////// <returns>returns status</returns>
        ////int SaveGeneralUserResponse(IList<GeneralUserResponseDTO> activities);

        ///// <summary>
        ///// Method to get survey questions on the basis of user profile selected
        ///// </summary>
        ///// <param name="userRoleID">user profile ID</param>
        ///// <param name="userID">user primary ID</param>
        ///// <returns>returns questions list</returns>
        //IList<SurveyModuleDTO> GetSurveyQuestions(long userRoleID, long userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        ///// <summary>
        ///// Method to get survey questions attributes
        ///// </summary>
        ///// <returns>returns questions attribute list</returns>
        //IList<SurveyQuestionAttributeDTO> GetSurveyQuestionAttributes();

        ///// <summary>
        ///// Method to submit competition booked in survey
        ///// </summary>
        ///// <param name="competitions">competition booked</param>
        ///// <returns>returns boolean response</returns>
        //long SubmitCompetitionBooked(IList<CompetitionSurveyDTO> competitions);

        ///// <summary>
        ///// Method to submit collection survey 
        ///// </summary>
        ///// <param name="collection">collection survey</param>
        ///// <returns>returns collection survey response</returns>
        //long SubmitCollectionSurvey(IList<CollectionSurveyDTO> collection);

        ///// <summary>
        ///// Method to submit order booking
        ///// </summary>
        ///// <param name="orders">order survey collection</param>
        ///// <returns>returns response</returns>
        //int SubmitOrderBooking(IList<OrderBookingSurveyDTO> orders);

        ///// <summary>
        ///// Method to fetch competition group list from database
        ///// </summary>
        ///// <param name="companyID">company ID</param>
        ///// <returns>returns product group list</returns>
        //IList<CompProductGroupDTO> GetCompetitionProductGroup(int companyID, long? userID);

        ///// <summary>
        ///// Method to fetch competition group list from database for APK
        ///// </summary>
        ///// <param name="companyID">company ID</param>
        ///// <returns>returns product group list</returns>
        //IList<CompProductGroupDTO> GetCompetitionProductGroup(int companyID, long? userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        ///// <summary>
        ///// Method to submir partner meeting details
        ///// </summary>
        ///// <param name="partnerEntity">partner entity</param>
        ///// <returns>returns boolean status</returns>
        //bool SubmitPartnerMeeting(PartnerMeetingDTO partnerEntity);

        ///// <summary>
        ///// Method to get partner meeting survey questions
        ///// </summary>
        ///// <param name="userRoleID">user role ID</param>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns questions</returns>
        //IList<SurveyModuleDTO> GetSurveyPartnerQuestions(long userRoleID, long userID);


        ///// <summary>
        ///// Method to get territory for selected user
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //IList<MyTerritoryDTO> GetMyTerritory(long userID);


        ///// <summary>
        ///// Get Rule Book Data (Cobined data from 3 tables ApproverTypeMaster, ActivityMaster, ActivityApporverMaster)
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <returns></returns>
        //RuleBookDTO GetRuleBook(long userID);

        //#region Import Coverage Export Data
       
        // /// <summary>
        // /// Inserts the entities new.
        // /// </summary>
        // /// <param name="dt">The dt.</param>
        // void ImportCoverageExport(DataTable dt);

        //#endregion

        // #region SDCE-638 Added by Niranjan (Product Group Category In Question Master) 16-10-2014
        // /// <summary>
        // /// Function to bind the ProductGroupCategory base on Product Group
        // /// </summary>
        // /// <param name="productGroupID">productGroupID</param>
        // /// <returns>Product Froup Category List </returns>
        // IList<ProductGroupCategoryBO> GetProductGroupCategoryList();
        // #endregion

        // bool SaveQuestionImages(long userID, int roleID, int storeID, string Image);
        // //bool SaveQuestionImages(Stream Image);


        //#region RACE
        //RACEMastersDTO GetRaceMasters(long userID, int roleID);

        ////RaceProductMasterOutputDTO GetRaceProductMasters(long userID, int roleID, int LastProductID, int rowcounter);

        //List<RaceProductMasterDTO> GetRaceProductMasters(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        //bool SubmitAuditResponse(long userID, int roleID, long SurveyResponseID, StockAuditDTO auditResponse);
        //#endregion

        //#region 14th August
        //IList<ParentDistributerDTO> GetParentDistributer(long UserID, int RoleID);
        //#endregion

        //#region RACE services for SyncAdaptor
        //List<RacePOSMMasterDTO> GetRACEPOSMMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        //List<RaceFixtureMasterDTO> GetRACEFixtureMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        //List<RacePOSMProductMappingDTO> GetRACEPOSMProductMapping(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);
        //#endregion

        //#region RaceMaster
        ///// <summary>
        ///// Added by Prashant 18 Nov 2015
        ///// </summary>      

        //List<RaceBrandMasterDTO> GetRaceBrandMaster(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        //List<RaceProductCategoryDTO> GetRaceProductCategory(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        //List<RaceBrandCategoryMappingDTO> GetRaceBrandCategoryMapping(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);


        //#endregion
    }
}
