using Samsung.SmartDost.BusinessLayer.Base;
using Samsung.SmartDost.BusinessLayer.Services.BO;
using Samsung.SmartDost.BusinessLayer.Services.Contracts;
using Samsung.SmartDost.CommonLayer.AopContainer;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.CommonLayer.Aspects.ReportBO;
using Samsung.SmartDost.CommonLayer.Aspects.Security;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using Samsung.SmartDost.PersistenceLayer.Repository.Contracts;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Samsung.SmartDost.BusinessLayer.ServiceImpl
{
    /// <summary>
    /// Class to implement the store related business rules and execution
    /// </summary>
    public class StoreManager : StoreBaseService, IStoreService
    {
        #region Properties

        /// <summary>
        /// Property to inject the store persistence layer object invocation
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.STORE_REPOSITORY)]
        public IStoreRepository StoreRepository { get; set; }

        #endregion

        #region Public Methods

        ///// <summary>
        ///// Displays the store profile.
        ///// </summary>
        ///// <param name="storeID">The store identifier.</param>
        ///// <returns></returns>
        //public StoreBO DisplayStoreProfile(int storeID, long userID)
        //{
        //    StoreBO store = new StoreBO();
        //    ObjectMapper.Map(StoreRepository.DisplayStoreProfile(storeID, userID), store);
        //    if (!String.IsNullOrEmpty(store.PictureFileName))
        //    {
        //        store.PictureFileName = AppUtil.GetServerMobileImages(store.PictureFileName, AspectEnums.ImageFileTypes.Store);
        //    }
        //    if (store.GeoTagCount > 0)
        //    {
        //        string configGeoCount = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.GeoTagCountThreshold);
        //        if (!String.IsNullOrEmpty(configGeoCount) && store.GeoTagCount >= Convert.ToInt32(configGeoCount))
        //        {
        //            //store.IsGeoTagRequired = false;//Commented for enable geo tag required
        //        }
        //        store.IsGeoTagRequired = true;//Commented for enable geo tag required
        //    }
        //    store.MobileNo = EncryptionEngine.DecryptString(store.MobileNo);
        //    store.EmailID = EncryptionEngine.DecryptString(store.EmailID);
        //    return store;
        //}


        /// <summary>
        /// Displays the store profile.
        /// </summary>
        /// <param name="storeID">The store identifier.</param>
        /// <returns></returns>
        public List<StoreDTO> DisplayStoreProfile(int? storeID, long userID)
        {
     
            List<StoreDTO> userStores = new List<StoreDTO>();
            ObjectMapper.Map(StoreRepository.DisplayStoreProfile(storeID,userID), userStores);
            foreach (var item in userStores)
            {
                item.MobileNo = EncryptionEngine.DecryptString(item.MobileNo);
                item.EmailID = EncryptionEngine.DecryptString(item.EmailID);
                if (!String.IsNullOrEmpty(item.PictureFileName))
                {
                    item.PictureFileName = AppUtil.GetServerMobileImages(item.PictureFileName, AspectEnums.ImageFileTypes.Store);
                }
            }

            return userStores;

        }


        /// <summary>
        /// return Partner list Based on City an duser
        /// </summary>
        /// <param name="userid">User Id</param>   
        /// <returns>return list of partner  Based on  to userid and city</returns>
        public IList<PartnerListDTO> GetPartnerList(long userId)
        {
            IList<PartnerListDTO> partnetList = new List<PartnerListDTO>();
            ObjectMapper.Map(StoreRepository.GetpartnerList(userId), partnetList);
            return partnetList;
        }

        /// <summary>
        /// Method to return Dealer list Based on city
        /// </summary>
        /// <param name="userID">userID</param>
        public IList<DealerListDTO> DealersListBasedOnCity(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {

            IList<DealerListDTO> dealerList = new List<DealerListDTO>();
            ObjectMapper.Map(StoreRepository.DealersListBasedOnCity(userID, roleID,RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out MaxModifiedDate), dealerList);
            return dealerList;

           
            //SPGetAssignedDealers_Result

        }

        /// <summary>
        /// Updates the store profile.
        /// </summary>
        /// <param name="storeID">The store identifier.</param>
        /// <param name="contactPerson">The contact person.</param>
        /// <param name="mobileNo">The mobile no.</param>
        /// <param name="emailID">The email identifier.</param>
        /// <returns></returns>
        public StoreBO UpdateStoreProfile(int storeID, string contactPerson, string mobileNo, string emailID, string imageName,string storeAddress)
        {
            StoreBO store = new StoreBO();
            ObjectMapper.Map(StoreRepository.UpdateStoreProfile(storeID, contactPerson, mobileNo, emailID,imageName,storeAddress), store);
            return store;
        }

        /// <summary>
        /// return Partner Details Based on UserID and partnetID
        /// </summary>
        /// <param name="userid">User Id</param>   
        /// <returns>return Partner Details  Based on  to userID and partnerID</returns>
        public PartnerDetailsBO DisplayPartnerDetails(long userID, long PartnerID, string shipToCode)
        {
            PartnerDetailsBO partnerDetails = new PartnerDetailsBO();
            var partner = StoreRepository.DisplayPartnerDetails(userID, PartnerID, shipToCode);
            ObjectMapper.Map(partner, partnerDetails);
            partnerDetails.PartnerMobileNo = EncryptionEngine.DecryptString(partner.MobileNo);
            partnerDetails.PartnerEmailID = EncryptionEngine.DecryptString(partner.EmailID);
            partnerDetails.PartnerContactPerson = partner.ContactPerson;
            partnerDetails.AVMTDPurchase = partner.AVMTDPurchase;
            partnerDetails.AVMTDSale = partner.AVMTDSale;
            partnerDetails.HAMTDPurchase = partner.HAMTDPurchase;
            partnerDetails.HAMTDSale = partner.HAMTDSale;
            partnerDetails.ACMTDPurchase = partner.ACMTDPurchase;
            partnerDetails.ACMTDSale = partner.ACMTDSale;
            return partnerDetails;
        }

        /// <summary>
        /// return 1 in the case Updated and 0 notUpdated
        /// </summary>
        /// <param name="partnerID">partner id.</param>
        /// <param name="contactPerson">The contact person.</param>
        /// <param name="mobileNo">The mobile no.</param>
        /// <param name="emailID">The email identifier.</param>
        /// <returns></returns>
        public int UpdatePartner(long partnerID, string contactPerson, string mobileNo, string emailID, string shipToCode)
        {
            int status = StoreRepository.UpdatePartnerDetails(partnerID, contactPerson, mobileNo, emailID, shipToCode);
            return status;
        }

        /// <summary>
        /// Method to get store schemes on the basis of start date in ascending order
        /// </summary>
        /// <param name="storeID">store primary key</param>
        /// <returns>returns scheme entity collection</returns>
        public IList<SchemeDTO> GetStoreSchemes(int storeID)
        {
            List<SchemeDTO> schemes = new List<SchemeDTO>();
            ObjectMapper.Map(StoreRepository.GetStoreSchemes(storeID), schemes);
            return schemes;
        }

        ///// <summary>
        ///// Gets the stores for today beat.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <returns></returns>
        //public IList<UserStoresTodayBeatDTO> GetStoresForTodayBeat(long userID)
        //{
        //    IList<UserStoresTodayBeatDTO> userStores = new List<UserStoresTodayBeatDTO>();
        //    ObjectMapper.Map(StoreRepository.GetStoresForTodayBeat(userID), userStores);
        //    foreach (var item in userStores)
        //    {
        //        item.MobileNo = EncryptionEngine.DecryptString(item.MobileNo);
        //        item.EmailID = EncryptionEngine.DecryptString(item.EmailID);

        //        string configGeoCount = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.GeoTagCountThreshold);
        //        if (!String.IsNullOrEmpty(configGeoCount) && item.GeoTagCount >= Convert.ToInt32(configGeoCount))
        //        {
        //            //item.IsGeoTagRequired = false;
        //            item.IsGeoTagRequired = true;//Enable Geo Tag even threshold cross
        //        }
        //        //if (item.GeoTagCount > 2)
        //        //{
        //        //    item.IsGeoTagRequired = false;
        //        //}
        //        else
        //        {
        //            item.IsGeoTagRequired = true;
        //        }
        //        if (!String.IsNullOrEmpty(item.PictureFileName))
        //        {
        //            item.PictureFileName = AppUtil.GetServerMobileImages(item.PictureFileName, AspectEnums.ImageFileTypes.Store);
        //        }
        //        if (item.LastVisitDate.HasValue)
        //        {
        //            item.LastVisitedDate = item.LastVisitDate.Value.ToString("dd/MM/yyyy HH:mm:ss",
        //                    CultureInfo.InvariantCulture);
        //        }
        //    }
            
        //    return userStores;
        //}

        /// <summary>
        /// Gets the user stores.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public IList<UserStoresBO> GetUserStores(long userID)
        {
            IList<UserStoresBO> userStores = new List<UserStoresBO>();
            ObjectMapper.Map(StoreRepository.GetUserStores(userID), userStores);
            //bool isCurrinetMonthPlanWindow = AopEngine.Resolve<ISystemService>(AspectEnums.AspectInstanceNames.ServiceManager, AspectEnums.ApplicationName.Samsung).IsCoverageFirstWindow(userID).IsCurrentMonth;
            //foreach (var item in userStores)
            //{
            //    item.BeatSummary = StoreRepository.GetStoreBeatPlanSummary(item.StoreID, isCurrinetMonthPlanWindow);
            //}
            return userStores;
        }

        /// <summary>
        /// Method to get ProductDefinitions
        /// </summary>
        /// <returns>returns ProductDefinitions</returns>
        public IList<ProductDefinationsDTO> GetProductDefinitions(int companyID)
        {
            IList<ProductDefinationsDTO> productDefinations = new List<ProductDefinationsDTO>();
            ObjectMapper.Map(StoreRepository.GetProductDefinitions(companyID), productDefinations);
            return productDefinations;
        }

        /// <summary>
        /// Method to get Competitors based on CompanyID
        /// </summary>
        /// <param name="companyID"> company identifier.</param>
        /// <returns>returns CompetitorsList</returns>        
        public IList<CompetitorsDTO> GetCompetitors(int companyID, long? userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            IList<CompetitorsDTO> competitorsList = new List<CompetitorsDTO>();
            ObjectMapper.Map(StoreRepository.GetCompetitors(companyID, userID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out MaxModifiedDate), competitorsList);
            return competitorsList;
        }  

        /// <summary>
        /// Method to get selected outlet profile details
        /// </summary>
        /// <param name="userID">user primary ID</param>
        /// <param name="storeID">store ID</param>
        /// <returns>returns outlet entity instance</returns>
        public OutletProfileDTO GetOutletProfile(long userID, long storeID)
        {
            OutletProfileDTO olProfile = new OutletProfileDTO();
            ObjectMapper.Map(StoreRepository.GetOutletProfile(userID, storeID), olProfile);
            return olProfile;
        }

        /// <summary>
        /// Gets the dealers list.
        /// </summary>
        /// <returns></returns>
        public IList<StoreBO> GetDealersList()
        {
            IList<StoreBO> lstDealers = new List<StoreBO>();

            ObjectMapper.Map(StoreRepository.GetDealersList(), lstDealers);
            return lstDealers;
        }

        /// <summary>
        /// Gets the schemes by dealer identifier.
        /// </summary>
        /// <param name="dealerID">The dealer identifier.</param>
        /// <returns></returns>
        public List<SchemesBO> GetSchemesByDealerID(int dealerID)
        {
            List<SchemesBO> lstSchemes = new List<SchemesBO>();

            ObjectMapper.Map(StoreRepository.GetSchemesByDealerID(dealerID), lstSchemes);
            return lstSchemes;
        }

        /// <summary>
        /// Gets the scheme by scheme identifier.
        /// </summary>
        /// <param name="schemeID">The scheme identifier.</param>
        /// <returns></returns>
        public SchemesBO GetSchemeBySchemeID(int schemeID)
        {
            SchemesBO scheme = new SchemesBO();
            ObjectMapper.Map(StoreRepository.GetSchemeBySchemeID(schemeID), scheme);
            return scheme;
        }

        /// <summary>
        /// Inserts the scheme.
        /// </summary>
        /// <param name="scheme">The scheme.</param>
        /// <returns></returns>
        public bool InsertScheme(SchemesBO scheme)
        {
            bool isSuccess = false;
            Scheme dealerScheme = new Scheme();
            ObjectMapper.Map(scheme, dealerScheme);
            if (dealerScheme != null)
                isSuccess = StoreRepository.InsertScheme(dealerScheme);
            return isSuccess;
        }

        /// <summary>
        /// Updates the scheme.
        /// </summary>
        /// <param name="scheme">The scheme.</param>
        /// <returns></returns>
        public bool UpdateScheme(SchemesBO scheme)
        {
            bool isSuccess = false;
            Scheme dealerScheme = new Scheme();
            ObjectMapper.Map(scheme, dealerScheme);
            if (dealerScheme != null && dealerScheme.SchemeID > 0)
                isSuccess = StoreRepository.UpdateScheme(dealerScheme);
            return isSuccess;
        }

        /// <summary>
        /// Deletes the scheme.
        /// </summary>
        /// <param name="schemes">The schemes.</param>
        /// <returns></returns>
        public bool DeleteScheme(List<SchemesBO> schemes)
        {
            bool isSuccess = false;
            List<Scheme> dealerSchemes = new List<Scheme>();
            ObjectMapper.Map(schemes, dealerSchemes);
            if (dealerSchemes.Count > 0)
                isSuccess = StoreRepository.DeleteScheme(dealerSchemes);
            return isSuccess;
        }

        /// <summary>
        /// Method to identify that is geo tag require or not
        /// </summary>
        /// <param name="storeID">store ID</param>
        /// <returns>returns boolean status</returns>
        public bool IsGeoTagRequired(int storeID)
        {
            return StoreRepository.IsGeoTagRequired(storeID);
        }

        #endregion



        #region Planogram (Amit: 18 Sep 2014)

        //Added by Vaishali on 18 Sep 2014 to Submit Planogram Response from APK
        public bool SubmitPlanogramResponse(List<PlanogramResponseDTO> APKplanogramResponse, int companyID, long userID, long roleID)
        {
            List<PlanogramResponse> planogramResponse = new List<PlanogramResponse>();
            ObjectMapper.Map(APKplanogramResponse, planogramResponse);
            return StoreRepository.SubmitPlanogramResponse(planogramResponse, companyID, userID, roleID);

        }


        ///// <summary>
        /////  This function returns list of Planogram Products in the table
        ///// </summary>
        ///// <returns>returns list of Planogram Products</returns>
        //public List<PlanogramProductMasterDTO> GetPlanogramProductMasters(int companyID, long userID, long roleID, int PlanogramProductMasterID, int rowcounter,out int totalRow)
        //{
        //    List<PlanogramProductMasterDTO> result = new List<PlanogramProductMasterDTO>();
        //    ObjectMapper.Map(StoreRepository.GetPlanogramProductMasters(companyID, userID, roleID, PlanogramProductMasterID, rowcounter, out totalRow), result);
        //    return result;
        //}

        /// <summary>
        ///  This function returns list of Planogram Products in the table
        /// </summary>
        /// <returns>returns list of Planogram Products</returns>
        public List<PlanogramProductMasterDTO> GetPlanogramProductMasters(int companyID, long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            List<PlanogramProductMasterDTO> result = new List<PlanogramProductMasterDTO>();
            ObjectMapper.Map(StoreRepository.GetPlanogramProductMasters(companyID, userID, roleID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out  MaxModifiedDate), result);
            return result;
        }

        /// <summary>
        ///  This function returns list of Planogram Classes in the table
        /// </summary>
        /// <returns>returns list of Planogram Classes</returns>
        public List<PlanogramClassMasterDTO> GetPlanogramClassMasters(int companyID, long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            List<PlanogramClassMasterDTO> result = new List<PlanogramClassMasterDTO>();
            ObjectMapper.Map(StoreRepository.GetPlanogramClassMasters(companyID, userID, roleID,RowCount, StartRowIndex,  LastUpdatedDate, out HasMoreRows, out  MaxModifiedDate), result);
            return result;
        }

        #endregion

        #region CompetitiorProductGroupMapping
        ///// <summary>
        /////  This function returns mapping between compititor and compprofuctgroup
        ///// </summary>
        ///// <returns>returns list of mapped data</returns>
        //public List<CompetitorProductGroupMappingDTO> GetCompetitorProductGroupMapping(long userID)
        //{
        //    List<CompetitorProductGroupMappingDTO> result = new List<CompetitorProductGroupMappingDTO>();
        //    ObjectMapper.Map(StoreRepository.GetCompetitorProductGroupMapping(userID), result);
        //    return result;
        //}

        /// <summary>
        ///  This function returns mapping between compititor and compprofuctgroup
        /// </summary>
        /// <returns>returns list of mapped data</returns>
        public List<CompetitorProductGroupMappingDTO> GetCompetitorProductGroupMapping(int companyID, long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            List<CompetitorProductGroupMappingDTO> result = new List<CompetitorProductGroupMappingDTO>();
            ObjectMapper.Map(StoreRepository.GetCompetitorProductGroupMapping(companyID, userID, roleID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out  MaxModifiedDate), result);
            return result;
        }
        #endregion
        #region Submit StoreGeoTag through multipart
        /// <summary>
        /// Submit Store geo tag using multipart
        /// </summary>
        /// <param name="geoTagBO"></param>
        /// <returns></returns>
        public int SubmitStoreGeoTag(StoreGeoTagBO geoTagBO)
        {
            StoreGeoTag storeGeoTag = new StoreGeoTag();
            ObjectMapper.Map(geoTagBO, storeGeoTag);
            int status = StoreRepository.SubmitStoreGeoTag(storeGeoTag);
            return status;
        }
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
        public List<GetDistrictDealerCreationDTO> GetMDMDistrict(int userID)
        {
            List<GetDistrictDealerCreationDTO> result = new List<GetDistrictDealerCreationDTO>();
            ObjectMapper.Map(StoreRepository.GetMDMDistrict(userID), result);
            return result;
            //List<string> lstMDMDistrict = new List<string>();
            //lstMDMDistrict = StoreRepository.GetMDMDistrict(userID);
            //return lstMDMDistrict;

        }
        /// <summary>
        /// GetMDMCity
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        public List<GetCityDealerCreationDTO> GetMDMCity(string district)
        {
            List<GetCityDealerCreationDTO> lstMDMCity = new List<GetCityDealerCreationDTO>();
            //lstMDMCity = StoreRepository.GetMDMCity(district);
            ObjectMapper.Map(StoreRepository.GetMDMCity(district), lstMDMCity);
            return lstMDMCity;
        }
        /// <summary>
        /// GetMDMPinCode
        /// </summary>
        /// <param name="district"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        //public List<string> GetMDMPinCode(string district, string city)
        //{ // Need to Delete 
        //    List<string> lstMDMPinCode = new List<string>();
        //    lstMDMPinCode = StoreRepository.GetMDMPinCode(district, city);
        //    return lstMDMPinCode;
        //}
        /// <summary>
        /// GetMDMParentDealerCode
        /// </summary>
        /// <returns></returns>
        public List<string> GetMDMParentDealerCode()
        {
            List<string> lstMDMParentDealerCode = new List<string>();
            lstMDMParentDealerCode = StoreRepository.GetMDMParentDealerCode();
            return lstMDMParentDealerCode;
        }
        /// <summary>
        /// SubmitMDMDealerCreation
        /// </summary>
        /// <param name="dealerCreation"></param>
        /// <param name="emplCode"></param>
        /// <returns></returns>
        public int SubmitMDMDealerCreation(DealerCreationBO dealerCreation, string emplCode)
        {
            //bool IsSucess;
            int dealerCreationID = 0;
            //TODO Set All NR Via Backend Field<Begin>
            dealerCreation.DIVISIONCODE = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.DIVISIONCODE);
            dealerCreation.SOURCE = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.SOURCE);
            dealerCreation.CHANNEL = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.CHANNEL);
            dealerCreation.SUBCHANNELCODE = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.SUBCHANNELCODE);
            dealerCreation.APPROVALSTATUS = Convert.ToInt32(AppUtil.GetAppSettings(AspectEnums.ConfigKeys.APPROVALSTATUS));
            dealerCreation.PARTNERTYPECODE = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.PARTNERTYPECODE);
            //dealerCreation.GSBPHOTO = "gsb.jpeg";
            dealerCreation.CREATEDDATETIME = System.DateTime.Now;
            dealerCreation.CREATEDBY = emplCode;

            Random RandomPIN = new Random();
            var RandomPINResult = RandomPIN.Next(0, 99999).ToString();
            RandomPINResult = RandomPINResult.PadLeft(5, '0');
            string refNo = "CE" + "-" + RandomPINResult;
            dealerCreation.LEGACYREFERENCENO = refNo;
            //TODO Set All NR Via Backend Field<End>
            DealerCreation dealer = new DealerCreation();
            ObjectMapper.Map(dealerCreation, dealer);
            dealerCreationID = StoreRepository.SubmitMDMDealerCreation(dealer, emplCode);

            return dealerCreationID;
        }
        /// <summary>
        /// Get created dealer data
        /// </summary>
        /// <param name="dealerCreationID"></param>
        /// <returns></returns>
        public DealerCreationBO GetDealerCreationData(int dealerCreationID)
        {
            DealerCreationBO dealer = new DealerCreationBO();
            ObjectMapper.Map(StoreRepository.GetDealerCreationData(dealerCreationID), dealer);
            return dealer;
        }
        /// <summary>
        /// PhotoMDMDealerCreation
        /// </summary>
        /// <param name="dealerCreation"></param>
        /// <param name="dealerCreationID"></param>
        /// <returns></returns>
        public bool PhotoMDMDealerCreation(DealerCreationBO dealerCreation, int dealerCreationID)
        {
            bool IsSucess;
            DealerCreation dealer = new DealerCreation();
            ObjectMapper.Map(dealerCreation, dealer);
            IsSucess = StoreRepository.PhotoMDMDealerCreation(dealer, dealerCreationID);
            return IsSucess;

        }

       
        #endregion
    }
}
