using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;

namespace Samsung.SmartDost.PersistenceLayer.Repository.Contracts
{
    /// <summary>
    /// Interface to define store related methods
    /// </summary>
    public interface IStoreRepository
    {
        ///// <summary>
        ///// Displays the store profile.
        ///// </summary>
        ///// <param name="storeID">The store identifier.</param>
        ///// <returns></returns>
        //StoreProfile DisplayStoreProfile(int storeID, long userID);

        /// <summary>
        /// Displays the store profile & Today Beat if store id is passed null
        /// </summary>
        /// <param name="storeID">The store identifier.</param>
        /// <returns></returns>
        /// 
        List<SPDisplayStoreProfile_Result> DisplayStoreProfile(int? storeID, long userID);

        /// <summary>
        /// Updates the store profile.
        /// </summary>
        /// <param name="storeID">The store identifier.</param>
        /// <param name="contactPerson">The contact person.</param>
        /// <param name="mobileNo">The mobile no.</param>
        /// <param name="emailID">The email identifier.</param>
        /// <returns></returns>
        StoreMaster UpdateStoreProfile(int storeID, string contactPerson, string mobileNo, string emailID, string imageName,string storeAddress);

        /// <summary>
        /// return Partner list Based on City an duser
        /// </summary>
        /// <param name="userid">User Id</param>   
        /// <returns>return list of partner  Based on  to userid and city</returns>
        IList<PartnerList> GetpartnerList(long userId);

        /// <summary>
        /// Method to return Dealer list Based on city
        /// </summary>
        /// <param name="userID">userID</param>
        IList<SPGetAssignedDealers_Result> DealersListBasedOnCity(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        /// <summary>
        /// return Partner Details Based on UserID and partnetID
        /// </summary>
        /// <param name="userid">User Id</param>   
        /// <returns>return Partner Details  Based on  to userID and partnerID</returns>
        StoreProfile DisplayPartnerDetails(long userID, long partnerID, string shipToCode);

        /// <summary>
        /// return 1 in the case Updated and 0 notUpdated
        /// </summary>
        /// <param name="partnerID">partner id.</param>
        /// <param name="contactPerson">The contact person.</param>
        /// <param name="mobileNo">The mobile no.</param>
        /// <param name="emailID">The email identifier.</param>
        /// <returns></returns>
        int UpdatePartnerDetails(long partnerID, string contactPerson, string mobileNo, string emailID, string shipToCode);

        /// <summary>
        /// Method to get store schemes on the basis of start date in ascending order
        /// </summary>
        /// <param name="storeID">store primary key</param>
        /// <returns>returns scheme entity collection</returns>
        IList<Scheme> GetStoreSchemes(int storeID);

        ///// <summary>
        ///// Gets the stores for today beat.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>

        ///// <returns></returns>
        //IList<StoreProfile> GetStoresForTodayBeat(long userID);

        IList<StoreMaster> GetUserStores(long userID);

        /// <summary>
        /// Method to get ProductDefinitions
        /// </summary>
        /// <returns>returns ProductDefinitions</returns>
        IList<ProductDefinition> GetProductDefinitions(int companyID);

        /// <summary>
        /// Method to get Competitors based on CompanyID
        /// </summary>
        /// <param name="companyID"> company identifier.</param>
        /// <returns>returns CompetitorsList</returns>
        IList<Competitor> GetCompetitors(int companyID, long? userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        /// <summary>
        /// Method to get selected outlet profile details
        /// </summary>
        /// <param name="userID">user primary ID</param>
        /// <param name="storeID">store ID</param>
        /// <returns>returns outlet entity instance</returns>
        OutletProfile GetOutletProfile(long userID, long storeID);

        /// <summary>
        /// Gets the dealers list.
        /// </summary>
        /// <returns></returns>
        IList<StoreMaster> GetDealersList();

        /// <summary>
        /// Gets the scheme by scheme identifier.
        /// </summary>
        /// <param name="schemeID">The scheme identifier.</param>
        /// <returns></returns>
        Scheme GetSchemeBySchemeID(int schemeID);

        /// <summary>
        /// Gets the schemes by dealer identifier.
        /// </summary>
        /// <param name="dealerID">The dealer identifier.</param>
        /// <returns></returns>
        List<Scheme> GetSchemesByDealerID(int dealerID);

        /// <summary>
        /// Inserts the scheme.
        /// </summary>
        /// <param name="scheme">The scheme.</param>
        /// <returns></returns>
        bool InsertScheme(Scheme scheme);

        /// <summary>
        /// Updates the scheme.
        /// </summary>
        /// <param name="scheme">The scheme.</param>
        /// <returns></returns>
        bool UpdateScheme(Scheme scheme);

        /// <summary>
        /// Deletes the scheme.
        /// </summary>
        /// <param name="schemes">The schemes.</param>
        /// <returns></returns>
        bool DeleteScheme(List<Scheme> schemes);

        /// <summary>
        /// Method to get store details
        /// </summary>
        /// <param name="storeID">store ID</param>
        /// <returns></returns>
        StoreMaster GetStoreDetails(int storeID);

        /// <summary>
        /// Method to get store beat plan summary
        /// </summary>
        /// <param name="storeID">store ID</param>
        /// <param name="isCurrentMonth">is current month</param>
        /// <returns>returns beat plan summary</returns>
        string GetStoreBeatPlanSummary(int storeID, bool isCurrentMonth);

        /// <summary>
        /// Method to identify that is geo tag require or not
        /// </summary>
        /// <param name="storeID">store ID</param>
        /// <returns>returns boolean status</returns>
        bool IsGeoTagRequired(int storeID);
        /// <summary>
        /// Method to get store parent details for provided user and store ids
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <param name="storeID">store ID</param>
        /// <returns>returns store parent mapping instance</returns>
        StoreParentMapping GetUserStoreParentDetails(long userID, int storeID);

        #region Planogram (Amit: 18 Sep 2014)
        ///// <summary>
        /////  This function returns list of Planogram Products in the table
        ///// </summary>
        ///// <returns>returns list of Planogram Products</returns>
        //List<PlanogramProductMaster> GetPlanogramProductMasters(int companyID, long userID, long roleID, int PlanogramProductMasterID, int rowcounter, out int totalRow);

        /// <summary>
        ///  This function returns list of Planogram Products in the table
        /// </summary>
        /// <returns>returns list of Planogram Products</returns>
        List<PlanogramProductMaster> GetPlanogramProductMasters(int companyID, long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);


        /// <summary>
        ///  This function returns list of Planogram Classes in the table
        /// </summary>
        /// <returns>returns list of Planogram Classes</returns>
        List<PlanogramClassMaster> GetPlanogramClassMasters(int companyID, long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        /// <summary>
        ///  Submit Planogram Response
        /// </summary>
        /// <returns>PlanResponseId</returns>
        bool SubmitPlanogramResponse(List<PlanogramResponse> planogramResponse, int companyID, long userID, long roleID);        
        #endregion

        #region GeoTagUnfreeze

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Stores"></param>
        /// <returns>list of stores not exists in StoreMaster</returns>        
        List<string> ValidateStores(List<string> Stores);
        #endregion


        #region CompetitorProductGroupMapping
        ///// <summary>
        /////  This function returns list of Planogram Classes in the table
        ///// </summary>
        ///// <returns>returns list of Planogram Classes</returns>
        //List<CompetitorProductGroupMapping> GetCompetitorProductGroupMapping(long userID);

        /// <summary>
        ///  This function returns list of Planogram Classes in the table
        /// </summary>
        /// <returns>returns list of Planogram Classes</returns>
        List<CompetitorProductGroupMapping> GetCompetitorProductGroupMapping(int companyID, long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);
        #endregion
        #region Submit StoreGeoTag through multipart
        /// <summary>
        /// Submit Store geo tag using multipart
        /// </summary>
        /// <param name="geoTagBO"></param>
        /// <returns></returns>
        int SubmitStoreGeoTag(StoreGeoTag storeGeoTag);
        #endregion

        /// <summary>
        /// Gets the dealer Creation.SDHHP-6114
        /// Added by Gourav Vishnoi on 31 July 2015
        /// </summary>
        #region Dealer Creation Form in SD and AX Service SDHHP-6114


        List<spGetDistrictDealerCreation_Result> GetMDMDistrict(int userID);
        /// <summary>
        /// GetMDMCity
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        List<spGetCityDealerCreation_Result> GetMDMCity(string district);
        /// <summary>
        /// GetMDMPinCode
        /// </summary>
        /// <param name="district"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        //List<string> GetMDMPinCode(string district, string city);
        /// <summary>
        /// GetMDMParentDealerCode
        /// </summary>
        /// <returns></returns>
        List<string> GetMDMParentDealerCode();
        /// <summary>
        /// SubmitMDMDealerCreation
        /// </summary>
        /// <param name="dealerCreation"></param>
        /// <param name="emplCode"></param>
        /// <returns></returns>
        int SubmitMDMDealerCreation(DealerCreation dealerCreation, string emplCode);
        /// <summary>
        /// PhotoMDMDealerCreation
        /// </summary>
        /// <param name="dealerCreation"></param>
        /// <param name="dealerCreationID"></param>
        /// <returns></returns>
        bool PhotoMDMDealerCreation(DealerCreation dealerCreation, int dealerCreationID);

        /// <summary>
        /// Get created dealer data
        /// </summary>
        /// <param name="dealerCreationID"></param>
        /// <returns></returns>
        DealerCreation GetDealerCreationData(int dealerCreationID);

        #endregion
    }
}
