using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Samsung.SmartDost.PersistenceLayer.Repository.Contracts
{
    /// <summary>
    /// Interface to get survey questions
    /// </summary>
    public interface IActivityRepository
    {
        /// <summary>
        /// Method to save store survey response on the basis of coverage beat
        /// </summary>
        /// <param name="storeSurvey">store survey</param>
        /// <returns>returns status</returns>
        long SaveStoreSurveyResponse(SurveyResponse storeSurvey, bool? AuditRequired);

        /// <summary>
        /// Method to save user activities on the basis of store survey data
        /// </summary>
        /// <param name="activities">activities performed</param>
        /// <returns>returns status</returns>
        int SaveSurveyUserResponse(IList<SurveyUserResponse> activities,long userID, bool saveImage = true);

        /// <summary>
        /// Method to save user activities on the basis of general survey data
        /// </summary>
        /// <param name="activities">activities performed</param>
        /// <returns>returns status</returns>
        int SaveGeneralUserResponse(IList<GeneralUserResponse> activities, long userID, bool saveImage = true);

        /// <summary>
        /// Method to get survey questions on the basis of user profile selected
        /// </summary>
        /// <param name="userRoleID">user profile ID</param>
        /// <returns>returns questions list</returns>
        IList<vwSurveyQuestion> GetSurveyQuestions(long userRoleID, long userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        /// <summary>
        /// Method to get survey questions attributes
        /// </summary>
        /// <returns>returns questions attribute list</returns>
        IList<SurveyQuestionAttribute> GetSurveyQuestionAttributes();

        /// <summary>
        /// Method to submit competition booked in survey
        /// </summary>
        /// <param name="competitions">competition booked</param>
        /// <returns>returns boolean response</returns>
        long SubmitCompetitionBooked(IList<CompetitionSurvey> competitions);

        /// <summary>
        /// Method to submit collection survey 
        /// </summary>
        /// <param name="collection">collection survey</param>
        /// <returns>returns collection survey response</returns>
        long SubmitCollectionSurvey(IList<CollectionSurvey> collection);

        /// <summary>
        /// Method to submit order booking
        /// </summary>
        /// <param name="orders">order survey collection</param>
        /// <returns>returns response</returns>
        int SubmitOrderBooking(IList<OrderBookingSurvey> orders);


        /// <summary>
        /// Method to fetch competition group list from database
        /// </summary>
        /// <param name="companyID">company ID</param>
        /// <returns>returns product group list</returns>
        IList<CompProductGroup> GetCompetitionProductGroup(int companyID, long? userID);

        /// <summary>
        /// Method to fetch competition group list from database for APK
        /// </summary>
        /// <param name="companyID">company ID</param>
        /// <returns>returns product group list</returns>
        IList<CompProductGroup> GetCompetitionProductGroup(int companyID, long? userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        /// <summary>
        /// Method to submir partner meeting details
        /// </summary>
        /// <param name="partnerEntity">partner entity</param>
        /// <returns>returns boolean status</returns>
        bool SubmitPartnerMeeting(PartnerMeeting partnerEntity);

        /// <summary>
        /// Method to update order sync status
        /// </summary>
        /// <param name="orderID">order ID</param>
        /// <param name="syncStatus">sync status</param>
        /// <returns>returns boolean status</returns>
        bool UpdateOrderSyncStatus(long orderID, int syncStatus);

        /// <summary>
        /// Method to get partner meeting survey questions
        /// </summary>
        /// <param name="userRoleID">user role ID</param>
        /// <param name="userID"></param>
        /// <returns></returns>
        IList<vwSurveyQuestion> GetSurveyPartnerQuestions(long userRoleID, long userID);

        /// <summary>
        /// Method to get territory for selected user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        IList<SPGetMyTerritory_Result> GetMyTerritory(long userID);

        /// <summary>
        /// Get Rule Book Data (Cobined data from 3 tables ApproverTypeMaster, ActivityMaster, ActivityApporverMaster)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        RuleBook GetRuleBook(long userID);

        #region Import Coverage Export Data

        /// <summary>
        /// Inserts the entities new.
        /// </summary>
        /// <param name="dt">The dt.</param>
        void ImportCoverageExport(DataTable dt);


        #endregion


        #region SDCE-638 Added by Niranjan (Product Group Category In Question Master) 15-10-2014
        /// <summary>
        /// Function to bind the ProductGroupCategory base on Product Group
        /// </summary>
        /// <param name="productGroupID">productGroupID</param>
        /// <returns>Product Froup Category List </returns>
        IList<ProductGroupCategory> GetProductGroupCategoryList();
        #endregion

        bool SaveQuestionImages(long userID, int roleID, int storeID, string Image);
        //bool SaveQuestionImages(Stream Image);


        #region RACE
            List<RaceBrandMaster> GetRaceBrandMaster();
        
            List<RaceBrandCategoryMapping> GetRaceBrandCategoryMapping();
        
            List<RaceProductCategory> GetRaceProductCategory();
        
            List<RacePOSMMaster> GetRacePOSMMaster();

            List<RaceFixtureCategoryMaster> GetRaceFixtureMaster();
        
            //List<RaceProductMaster> GetRaceProductMaster(int LastProductID, int rowcounter, out bool HasMoreRows);

            List<RaceProductMaster> GetRaceProductMaster(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

            List<RacePOSMProductMapping> GetRacePOSMProductMappings();
            bool SubmitAuditResponse(long userID, int roleID, long SurveyResponseID, StockAudit auditResponse);
        #endregion

        #region 14 August
            IList<spGetParentDistributer_Result> GetParentDistributer(long UserID, int RoleID);
        #endregion


        #region RACE services for sync adaptor
        IList<RacePOSMMaster> GetRACEPOSMMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);


        IList<RaceFixtureCategoryMaster> GetRACEFixtureMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);


        IList<RacePOSMProductMapping> GetRACEPOSMProductMapping(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        
        #endregion

            #region RaceMaster
            /// <summary>
            /// Added by Prashant 18 Nov 2015
            /// </summary>   
            List<RaceBrandMaster> GetRaceBrandMaster(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

            List<RaceProductCategory> GetRaceProductCategory(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

            List<RaceBrandCategoryMapping> GetRaceBrandCategoryMapping(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

            #endregion
    }
}
