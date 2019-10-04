using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Samsung.SmartDost.BusinessLayer.Services.BO;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.CommonLayer.Aspects.ReportBO;

namespace Samsung.SmartDost.BusinessLayer.Services.Contracts
{
    /// <summary>
    /// Interface to expose Store business definitions and methods
    /// </summary>
    public interface IStoreService
    { 
        /// <summary>
        /// Displays the store profile.
        /// </summary>
        /// <param name="storeID">The store identifier.</param>
        /// <returns></returns>
        //StoreBO DisplayStoreProfile(int storeID, long userID);
        List<StoreDTO> DisplayStoreProfile(int? storeID, long userID);

        /// <summary>
        /// Updates the store profile.
        /// </summary>
        /// <param name="storeID">The store identifier.</param>
        /// <param name="contactPerson">The contact person.</param>
        /// <param name="mobileNo">The mobile no.</param>
        /// <param name="emailID">The email identifier.</param>
        /// <returns></returns>
        StoreBO UpdateStoreProfile(int storeID, string contactPerson, string mobileNo, string emailID, string imageName,string storeAddress);

        /// <summary>
        /// return Partner list Based on City an duser
        /// </summary>
        /// <param name="userid">User Id</param>   
        /// <returns>return list of partner  Based on  to userid and city</returns>
        IList<PartnerListDTO> GetPartnerList(long userId);

        /// <summary>
        /// Method to return Dealer list Based on city
        /// </summary>
        /// <param name="userID">userID</param>
        IList<DealerListDTO> DealersListBasedOnCity(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        /// <summary>
        /// return Partner Details Based on UserID and partnetID
        /// </summary>
        /// <param name="userid">User Id</param>   
        /// <param name="PartnerID">partner ID</param>
        /// <param name="shipToCode">ship to code value</param>
        /// <returns>return Partner Details  Based on  to userID and partnerID</returns>
        PartnerDetailsBO DisplayPartnerDetails(long userID, long PartnerID, string shipToCode);

        /// <summary>
        /// return 1 in the case Updated and 0 notUpdated
        /// </summary>
        /// <param name="partnerID">partner id.</param>
        /// <param name="contactPerson">The contact person.</param>
        /// <param name="mobileNo">The mobile no.</param>
        /// <param name="emailID">The email identifier.</param>
        /// <returns></returns>
        int UpdatePartner(long partnerID, string contactPerson, string mobileNo, string emailID, string shipToCode);

        /// <summary>
        /// Method to get store schemes on the basis of start date in ascending order
        /// </summary>
        /// <param name="storeID">store primary key</param>
        /// <returns>returns scheme entity collection</returns>
        IList<SchemeDTO> GetStoreSchemes(int storeID);

        ///// <summary>
        ///// Gets the stores for today beat.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <returns></returns>
        //IList<UserStoresTodayBeatDTO> GetStoresForTodayBeat(long userID);

        IList<UserStoresBO> GetUserStores(long userID);

        /// <summary>
        /// Method to get ProductDefinitions
        /// </summary>
        /// <returns>returns ProductDefinitions</returns>
        IList<ProductDefinationsDTO> GetProductDefinitions(int companyID);

        /// <summary>
        /// Method to get Competitors based on CompanyID
        /// </summary>
        /// <param name="companyID"> company identifier.</param>
        /// <returns>returns CompetitorsList</returns>
        IList<CompetitorsDTO> GetCompetitors(int companyID, long? userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDateTime);

        /// <summary>
        /// Method to get selected outlet profile details
        /// </summary>
        /// <param name="userID">user primary ID</param>
        /// <param name="storeID">store ID</param>
        /// <returns>returns outlet entity instance</returns>
        OutletProfileDTO GetOutletProfile(long userID, long storeID);

        /// <summary>
        /// Gets the dealers list.
        /// </summary>
        /// <returns></returns>
        IList<StoreBO> GetDealersList();

        /// <summary>
        /// Gets the scheme by scheme identifier.
        /// </summary>
        /// <param name="schemeID">The scheme identifier.</param>
        /// <returns></returns>
        SchemesBO GetSchemeBySchemeID(int schemeID);

        /// <summary>
        /// Gets the schemes by dealer identifier.
        /// </summary>
        /// <param name="dealerID">The dealer identifier.</param>
        /// <returns></returns>
        List<SchemesBO> GetSchemesByDealerID(int dealerID);

        /// <summary>
        /// Inserts the scheme.
        /// </summary>
        /// <param name="scheme">The scheme.</param>
        /// <returns></returns>
        bool InsertScheme(SchemesBO scheme);

        /// <summary>
        /// Updates the scheme.
        /// </summary>
        /// <param name="scheme">The scheme.</param>
        /// <returns></returns>
        bool UpdateScheme(SchemesBO scheme);

        /// <summary>
        /// Deletes the scheme.
        /// </summary>
        /// <param name="schemes">The schemes.</param>
        /// <returns></returns>
        bool DeleteScheme(List<SchemesBO> schemes);

        /// <summary>
        /// Method to identify that is geo tag require or not
        /// </summary>
        /// <param name="storeID">store ID</param>
        /// <returns>returns boolean status</returns>
        bool IsGeoTagRequired(int storeID);


        #region Planogram (Amit: 18 Sep 2014)

        //Added by Vaishali on 18 Sep 2014 to Submit Planogram Response from APK
        /// <summary>
        /// Submit Planogram Response from APK
        /// </summary>
        /// <param name="PlanogramResponse"></param>
        /// <returns>success or Failure</returns>
        bool SubmitPlanogramResponse(List<PlanogramResponseDTO> PlanogramResponse, int companyID, long userID, long roleID);

        ///// <summary>
        /////  This function returns list of Planogram Products in the table
        ///// </summary>
        ///// <returns>returns list of Planogram Products</returns>
        //List<PlanogramProductMasterDTO> GetPlanogramProductMasters(int companyID, long userID, long roleID, int PlanogramProductMasterID, int rowcounter, out int totalRow);

        /// <summary>
        ///  This function returns list of Planogram Products in the table
        /// </summary>
        /// <returns>returns list of Planogram Products</returns>
        List<PlanogramProductMasterDTO> GetPlanogramProductMasters(int companyID, long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);
        

        /// <summary>
        ///  This function returns list of Planogram Classes in the table
        /// </summary>
        /// <returns>returns list of Planogram Classes</returns>
        List<PlanogramClassMasterDTO> GetPlanogramClassMasters(int companyID, long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        //, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate

        #endregion
       
        #region CompetitiorProductGroupMapping
        ///// <summary>
        /////  This function returns mapping between compititor and compprofuctgroup
        ///// </summary>
        ///// <returns>returns list of mapped data</returns>
        //List<CompetitorProductGroupMappingDTO> GetCompetitorProductGroupMapping( long userID);

        /// <summary>
        ///  This function returns mapping between compititor and compprofuctgroup
        /// </summary>
        /// <returns>returns list of mapped data</returns>
        List<CompetitorProductGroupMappingDTO> GetCompetitorProductGroupMapping(int companyID, long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        #endregion

        #region Submit StoreGeoTag through multipart
        /// <summary>
        /// Submit Store geo tag using multipart
        /// </summary>
        /// <param name="geoTagBO"></param>
        /// <returns></returns>
        int SubmitStoreGeoTag(StoreGeoTagBO geoTagBO);
        #endregion

        /// <summary>
        /// Gets the dealer Creation.SDHHP-6114
        /// Added by Gourav Vishnoi on 31 July 2015
        /// </summary>
        #region Dealer Creation Form in SD and AX Service SDHHP-6114
        /// <summary>
        /// GetMDMDistrict
        /// </summary>
        /// <returns></returns>
        List<GetDistrictDealerCreationDTO> GetMDMDistrict(int userID);
        /// <summary>
        /// GetMDMCity
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        List<GetCityDealerCreationDTO> GetMDMCity(string district);
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
        int SubmitMDMDealerCreation(DealerCreationBO dealerCreation, string emplCode);
        /// <summary>
        /// PhotoMDMDealerCreation
        /// </summary>
        /// <param name="dealerCreation"></param>
        /// <param name="dealerCreationID"></param>
        /// <returns></returns>
        bool PhotoMDMDealerCreation(DealerCreationBO dealerCreation, int dealerCreationID);
        /// <summary>
        /// GetDealerCreationData
        /// </summary>
        /// <param name="dealerCreationID"></param>
        /// <returns></returns>
        DealerCreationBO GetDealerCreationData(int dealerCreationID);


        #endregion

    }
}
