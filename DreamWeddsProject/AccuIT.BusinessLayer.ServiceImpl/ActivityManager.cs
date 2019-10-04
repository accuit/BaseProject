using Samsung.SmartDost.BusinessLayer.Base;
using Samsung.SmartDost.BusinessLayer.IC.Contracts;
using Samsung.SmartDost.BusinessLayer.IC.Entities;
using Samsung.SmartDost.BusinessLayer.Services.BO;
using Samsung.SmartDost.BusinessLayer.Services.Contracts;
using Samsung.SmartDost.CommonLayer.AopContainer;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using Samsung.SmartDost.PersistenceLayer.Repository.Contracts;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Transactions;

namespace Samsung.SmartDost.BusinessLayer.ServiceImpl
{
    /// <summary>
    /// 
    /// </summary>
    public class ActivityManager : ServiceBase, IActivityService
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.ACTIVITY_REPOSITORY)]
        public IActivityRepository ActivityRepository { get; set; }

       // [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.STORE_REPOSITORY)]
       // public IStoreRepository StoreRepository { get; set; }

        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.SYSTEM_REPOSITORY)]
        public ISystemRepository SystemRepository { get; set; }

        /// <summary>
        /// Property to inject the user persistence layer object invocation
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.USER_REPOSITORY)]
        public IUserRepository UserRepository { get; set; }
        #endregion

        /// <summary>
        /// Method to save store survey response on the basis of coverage beat
        /// </summary>
        /// <param name="storeSurvey">store survey</param>
        /// <returns>returns status</returns>
        public long SaveStoreSurveyResponse(SurveyResponseDTO storeSurvey)
        {
            SurveyResponse survey = new SurveyResponse();
            ObjectMapper.Map(storeSurvey, survey);
            return ActivityRepository.SaveStoreSurveyResponse(survey, storeSurvey.RaceProfile);
        }

        /// <summary>
        /// Method to save user activities on the basis of store survey data
        /// </summary>
        /// <param name="activities">activities performed</param>
        /// <returns>returns status</returns>
        public int SaveSurveyUserResponse(IList<SurveyUserResponseDTO> activities,long userID, bool saveImage = true)
        {
            int result = 0;
            List<SurveyUserResponse> storeSurveys = new List<SurveyUserResponse>();
            IList<SurveyUserResponseDTO> storeActivies = activities.Where(k => k.SurveyResponseID != 0).ToList();
            if (storeActivies.Count > 0)
            {
                ObjectMapper.Map(storeActivies, storeSurveys);
                result += ActivityRepository.SaveSurveyUserResponse(storeSurveys,userID, saveImage);
            }
            
            List<GeneralUserResponse> generalSurveys = new List<GeneralUserResponse>();
            IList<SurveyUserResponseDTO> generalActivies = activities.Where(k => k.SurveyResponseID == 0).ToList();
                        
            if (generalActivies.Count > 0)
            {
                ObjectMapper.Map(generalActivies, generalSurveys);                
                result += ActivityRepository.SaveGeneralUserResponse(generalSurveys, userID, saveImage);
            }
            return result;

        }

     
        /// <summary>
        /// Method to get partner meeting survey questions
        /// </summary>
        /// <param name="userRoleID">user role ID</param>
        /// <param name="userID">user ID</param>
        /// <returns>returns questions</returns>
        public IList<SurveyModuleDTO> GetSurveyPartnerQuestions(long userRoleID, long userID)
        {
            IList<SurveyQuestionDTO> questions = new List<SurveyQuestionDTO>();
            ObjectMapper.Map(ActivityRepository.GetSurveyPartnerQuestions(userRoleID,userID), questions);
            IList<SurveyQuestionAttributeDTO> attributes = GetSurveyQuestionAttributes();
            IList<UserModuleDTO> modules = AopEngine.Resolve<IUserService>(AspectEnums.AspectInstanceNames.UserManager, AspectEnums.ApplicationName.Samsung).GetUserModules(userID, userRoleID);
            int partnerModuleID = 0;
            var partnerModule = modules.FirstOrDefault(k => k.ModuleCode == (int)AspectEnums.AppModules.PartnerMeeting);
            if (partnerModule != null)
                partnerModuleID = partnerModule.ModuleID;
            List<SurveyModuleDTO> surveyModules = modules.Where(k => k.ModuleID == partnerModuleID).OrderBy(h => h.Sequence).Select(k => new SurveyModuleDTO()
            {
                ModuleCode = k.ModuleCode.HasValue ? k.ModuleCode.Value : 0,
                ModuleID = k.ModuleID,
                Name = k.Name
            }).ToList();
            surveyModules.ForEach(k =>
            {
                k.Questions = questions.Where(question => question.ModuleID == k.ModuleID).OrderBy(j => j.Sequence).ToList();
                if (k.Questions != null)
                {
                    k.Questions.ForEach(j =>
                    {
                        j.Options = attributes.Where(h => h.SurveyQuestionID == j.SurveyQuestionID).OrderBy(t => t.Sequence).ToList();
                    });
                }
            });
            return surveyModules;
            
        }

        /// <summary>
        /// Method to get survey questions on the basis of user profile selected
        /// </summary>
        /// <param name="userRoleID">user profile ID</param>
        /// <param name="userID">user primary ID</param>
        /// <returns>returns questions list</returns>
        public IList<SurveyModuleDTO> GetSurveyQuestions(long userRoleID, long userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {

            IList<SurveyQuestionDTO> questions = new List<SurveyQuestionDTO>();
            ObjectMapper.Map(ActivityRepository.GetSurveyQuestions(userRoleID,userID,RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out MaxModifiedDate), questions);
            
            IList<SurveyQuestionAttributeDTO> attributes = GetSurveyQuestionAttributes();


            bool HasMoreRowsModules = false;
            DateTime? MaxModifiedDateModules = null;
            IList<UserModuleDTO> modules = AopEngine.Resolve<IUserService>(AspectEnums.AspectInstanceNames.UserManager, AspectEnums.ApplicationName.Samsung)
                .GetUserModules(userID, userRoleID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRowsModules, out MaxModifiedDateModules, true).ToList();//using this method to pick all methods deleted or not deleted
           
            #region select only those modules whose questions are changed
           
            List<SurveyModuleDTO> surveyModules = (from m in modules
                                                   join q in questions
                                                   on m.ModuleCode equals q.ModuleCode
            select m).OrderBy(h => h.Sequence)
                .Select(k => new SurveyModuleDTO()
            {
                ModuleCode = k.ModuleCode.HasValue ? k.ModuleCode.Value : 0,
                ModuleID = k.ModuleID,
                Name = k.Name
            }).Distinct().OrderBy(k => k.Sequence).ToList();
            #endregion

                                  //                     channelTypeDisplay = (from CTM in SmartDostDbContext.ChannelTypeTeamMappings
                                  //join CTD in SmartDostDbContext.ChannelTypeDisplays on CTM.ChannelType equals CTD.ChannelType into ej
                                  //from CTD in ej.DefaultIfEmpty()
                                  //where CTM.IsDeleted == false
                                  //select new { CType = CTM.ChannelType, IsDisplay = CTD == null ? false : CTD.IsDisplayCounterShare, IsPlanogram = CTD == null ? false : CTD.IsPlanogram, ChannelTypeDisplayID = CTD == null ? 0 : CTD.ChannelTypeDisplayID }
                                  //).AsEnumerable().Select(d => new ChannelTypeDisplay() { ChannelType = d.CType, IsPlanogram = d.IsPlanogram, IsDisplayCounterShare = d.IsDisplay, ChannelTypeDisplayID = d.ChannelTypeDisplayID }).Distinct().ToList();



            //List<SurveyModuleDTO> surveyModules = modules.OrderBy(h => h.Sequence)
            //    .Select(k => new SurveyModuleDTO()
            //{
            //    ModuleCode = k.ModuleCode.HasValue ? k.ModuleCode.Value : 0,
            //    ModuleID = k.ModuleID,
            //    Name = k.Name
            //}).OrderBy(k => k.Sequence).ToList();

            surveyModules.ForEach(k =>
            {
                k.Questions = questions.Where(question => question.ModuleID == k.ModuleID).OrderBy(j => j.Sequence).ToList();
                if (k.Questions != null)
                {
                    k.Questions.ForEach(j =>
                    {
                        j.Options = attributes.Where(h => h.SurveyQuestionID == j.SurveyQuestionID).OrderBy(t => t.Sequence).ToList();

                        if (j.IsDeleted == true)
                        {
                            foreach (var item in j.Options)
                            {
                                item.IsDeleted = true;
                            }
                        }
                    });
                }
            });
        

            return surveyModules;
        }

        /// <summary>
        /// Method to get survey questions attributes
        /// </summary>
        /// <returns>returns questions attribute list</returns>
        public IList<SurveyQuestionAttributeDTO> GetSurveyQuestionAttributes()
        {
            IList<SurveyQuestionAttributeDTO> attributes = new List<SurveyQuestionAttributeDTO>();
            ObjectMapper.Map(ActivityRepository.GetSurveyQuestionAttributes(), attributes);
            return attributes;
        }

        /// <summary>
        /// Method to submit competition booked in survey
        /// </summary>
        /// <param name="competitions">competition booked</param>
        /// <returns>returns boolean response</returns>
        public long SubmitCompetitionBooked(IList<CompetitionSurveyDTO> competitions)
        {
            long response = 0;
            if (competitions != null && competitions.Count > 0)
            {
                List<CompetitionSurvey> surveys = new List<CompetitionSurvey>();
                ObjectMapper.Map(competitions, surveys);
                response = ActivityRepository.SubmitCompetitionBooked(surveys);
            }
            return response;
        }

        /// <summary>
        /// Method to submit collection survey 
        /// </summary>
        /// <param name="collection">collection survey</param>
        /// <returns>returns collection survey response</returns>
        public long SubmitCollectionSurvey(IList<CollectionSurveyDTO> collection)
        {
            List<CollectionSurvey> survey = new List<CollectionSurvey>();
            ObjectMapper.Map(collection, survey);
            int index = 0;
            foreach (var item in collection)
            {
                survey[index].TransactionDate = Convert.ToDateTime(item.PaymentDate);
                index++;
            }
            return ActivityRepository.SubmitCollectionSurvey(survey);
        }

        /// <summary>
        /// Method to submit order booking
        /// </summary>
        /// <param name="orders">order survey collection</param>
        /// <returns>returns response</returns>
        public int SubmitOrderBooking(IList<OrderBookingSurveyDTO> orders)
        {
            int status = 0;
            List<OrderBookingSurvey> bookings = new List<OrderBookingSurvey>();
            ObjectMapper.Map(orders, bookings);
            if (bookings.Count > 0)
            {
                status = ActivityRepository.SubmitOrderBooking(bookings);
                #region Commented By Dhiraj on 17-Dec-2014 for the purpose of adding replication of stock esclation
                //string salesManCode = UserRepository.GetEmployeeCode(orders[0].UserID);
                //UpdateSamsungDMSOrders(bookings, salesManCode);
                #endregion
            }
            return status;
        }

        /// <summary>
        /// Method to update samsung orders in DMS
        /// </summary>
        /// <param name="orders">order collection</param>
        /// <param name="salesmanCode">salesman code</param>
        private void UpdateSamsungDMSOrders(IList<OrderBookingSurvey> orders, string salesmanCode)
        {
            IOrderBooking orderInstance = AopEngine.Resolve<IOrderBooking>("Samsung_OrderManager");

            foreach (var item in orders)
            {
                //using (TransactionScope scope = new TransactionScope())
                //{
                   var storeDetail = "";// StoreRepository.GetStoreDetails(item.StoreID);
                   // var storeParentDetails = StoreRepository.GetUserStoreParentDetails(item.UserID, item.StoreID);
                    var productDetails = SystemRepository.GetProductDetails(item.ProductID.Value);
                    if (storeDetail != null && productDetails != null)
                    {
                        OrderBooking order = new OrderBooking()
                        {
                            OrderDate = item.CreatedDate,
                            OrderQty = item.Quantity,
                            OrderKeyNo = item.OrderNo,
                            //RTRCode = storeDetail.StoreCode,
                           // DistyCode = storeParentDetails != null? storeParentDetails.ShipToCode:string.Empty,
                            ProductCode = productDetails.SKUCode,
                            OrderID = item.OrderBookingID,
                            SRPCode = salesmanCode,
                            CreatedDate = System.DateTime.Now,
                            ModifiedDate = System.DateTime.Now,
                            CreatedBy = item.UserID.ToString(),
                            ModifiedBy = item.UserID.ToString(),
                            RouteCode = item.CoverageID.HasValue ? item.CoverageID.Value.ToString() : string.Empty,
                            DownloadFlag = "N",
                        };
                        bool isSuccess = orderInstance.SaveOrderInDMS(order);
                        ActivityRepository.UpdateOrderSyncStatus(order.OrderID, isSuccess ? 3 : 2);
                    }

                //}
            }
        }

        /// <summary>
        /// Method to fetch competition group list from database
        /// </summary>
        /// <param name="companyID">company ID</param>
        /// <returns>returns product group list</returns>
        public IList<CompProductGroupDTO> GetCompetitionProductGroup(int companyID, long? userID)
        {
            List<CompProductGroupDTO> productGroups = new List<CompProductGroupDTO>();
            ObjectMapper.Map(ActivityRepository.GetCompetitionProductGroup(companyID, userID),productGroups);
            return productGroups;
        }

        /// <summary>
        /// Method to fetch competition group list from database
        /// </summary>
        /// <param name="companyID">company ID</param>
        /// <returns>returns product group list</returns>
        public IList<CompProductGroupDTO> GetCompetitionProductGroup(int companyID, long? userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            List<CompProductGroupDTO> productGroups = new List<CompProductGroupDTO>();
            ObjectMapper.Map(ActivityRepository.GetCompetitionProductGroup(companyID,userID,RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out MaxModifiedDate), productGroups);
            return productGroups;
        }

        /// <summary>
        /// Method to submir partner meeting details
        /// </summary>
        /// <param name="partnerEntity">partner entity</param>
        /// <returns>returns boolean status</returns>
        public bool SubmitPartnerMeeting(PartnerMeetingDTO partnerEntity)
        {
            PartnerMeeting partner = new PartnerMeeting();
            ObjectMapper.Map(partnerEntity, partner);
            return ActivityRepository.SubmitPartnerMeeting(partner);
        }


        /// <summary>
        /// Method to get territory for selected user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IList<MyTerritoryDTO> GetMyTerritory(long UserID)
        {
            IList<MyTerritoryDTO> MyTerritories = new List<MyTerritoryDTO>();
            ObjectMapper.Map(ActivityRepository.GetMyTerritory(UserID), MyTerritories);
            return MyTerritories;
        }

        /// <summary>
        /// Get Rule Book Data (Cobined data from 3 tables ApproverTypeMaster, ActivityMaster, ActivityApporverMaster)
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public RuleBookDTO GetRuleBook(long userID)
        {
            RuleBookDTO objRuleBookDTO = new RuleBookDTO();

            ObjectMapper.Map(ActivityRepository.GetRuleBook(userID), objRuleBookDTO);

            return objRuleBookDTO;

        }

        #region Import Coverage Export Data
     
        /// <summary>
        /// Inserts the entities new.
        /// </summary>
        /// <param name="dt">The dt.</param>
        public void ImportCoverageExport(DataTable dt)
        {

            ActivityRepository.ImportCoverageExport(dt);
        }

        #endregion


        #region SDCE-638 Added by Niranjan (Product Group Category In Question Master) 16-10-2014
        /// <summary>
        ///Function to bind the ProductGroupCategory base on Product Group
        /// </summary>
        /// <param name="ProductGroupID">ProductGroupID</param>
        /// <returns>returns product group list</returns>
        public IList<ProductGroupCategoryBO> GetProductGroupCategoryList()
        {
            List<ProductGroupCategoryBO> productGroups = new List<ProductGroupCategoryBO>();
            ObjectMapper.Map(ActivityRepository.GetProductGroupCategoryList(), productGroups);
            return productGroups;
        }

#endregion

        public bool SaveQuestionImages(long userID, int roleID, int storeID, string Image)
        //public bool SaveQuestionImages(Stream Image)
        {            
            return ActivityRepository.SaveQuestionImages(userID, roleID, storeID, Image);
            //return ActivityRepository.SaveQuestionImages(Image);
        }

        #region RACE
        public RACEMastersDTO GetRaceMasters(long userID, int roleID)
        {
            RACEMastersDTO result = new RACEMastersDTO();


            if (SystemRepository.IsDownloadAuthorized(userID, AspectEnums.DownloadService.RACE) == true)
            {
                List<RaceBrandMasterDTO> lstBrands = new List<RaceBrandMasterDTO>(); ;
                List<RaceProductCategoryDTO> lstProductCategory = new List<RaceProductCategoryDTO>();
                List<RacePOSMMasterDTO> lstPOSM = new List<RacePOSMMasterDTO>();
                List<RaceFixtureMasterDTO> lstFixtures = new List<RaceFixtureMasterDTO>();
                List<RaceBrandCategoryMappingDTO> lstBrandCategoryMapping = new List<RaceBrandCategoryMappingDTO>();
                List<RacePOSMProductMappingDTO> lstPOSMProductMapping = new List<RacePOSMProductMappingDTO>();


                ObjectMapper.Map(ActivityRepository.GetRaceBrandMaster(), lstBrands);
                ObjectMapper.Map(ActivityRepository.GetRaceProductCategory(), lstProductCategory);
                ObjectMapper.Map(ActivityRepository.GetRacePOSMMaster(), lstPOSM);
                ObjectMapper.Map(ActivityRepository.GetRaceFixtureMaster(),lstFixtures);
                ObjectMapper.Map(ActivityRepository.GetRaceBrandCategoryMapping(), lstBrandCategoryMapping);
                ObjectMapper.Map(ActivityRepository.GetRacePOSMProductMappings(), lstPOSMProductMapping);               

                result.BrandCategoryMapping = lstBrandCategoryMapping;
                result.BrandMaster = lstBrands;
                result.FixtureMaster = lstFixtures;
                result.POSMMaster = lstPOSM;
                result.ProductCategory = lstProductCategory;
                result.POSMProductMapping = lstPOSMProductMapping;

            }
            return result;
        }

        //public RaceProductMasterOutputDTO GetRaceProductMasters(long userID, int roleID, int LastProductID, int rowcounter)
        //{
        //    RaceProductMasterOutputDTO result = new RaceProductMasterOutputDTO();
            
        //    if (SystemRepository.IsDownloadAuthorized(userID, AspectEnums.DownloadService.RACE) == true)
        //    {
        //        List<RaceProductMasterDTO> products = new List<RaceProductMasterDTO>();

        //        if (rowcounter < 1)
        //            rowcounter = (int)AspectEnums.RowCounter.RACERowCounter;

        //        bool HasMoreRows = false;
        //        ObjectMapper.Map(ActivityRepository.GetRaceProductMaster(LastProductID, rowcounter, out HasMoreRows).ToList(), products);

        //        result.Products = products;
        //        result.HasMoreRows = HasMoreRows;
        //    }
        //    return result;
        //}


        public List<RaceProductMasterDTO> GetRaceProductMasters(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            List<RaceProductMasterDTO> result = new List<RaceProductMasterDTO>();
            ObjectMapper.Map(ActivityRepository.GetRaceProductMaster(userID, roleID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out  MaxModifiedDate), result);
            return result;
        }

        public bool SubmitAuditResponse(long userID, int roleID, long SurveyResponseID,StockAuditDTO auditResponse)
        {
            
            StockAudit audit=new StockAudit();
            ObjectMapper.Map(auditResponse, audit);

            return ActivityRepository.SubmitAuditResponse(userID, roleID,SurveyResponseID, audit);
            
        }
       
        #endregion

        #region 14 August
        /// <summary>
        /// GetParentDistributer
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public IList<ParentDistributerDTO> GetParentDistributer(long UserID, int RoleID)
        {
            // IList
            IList<ParentDistributerDTO> srdLoc = new List<ParentDistributerDTO>();
            //OfficeLocationDTO> srdLoc = new 
            ObjectMapper.Map(ActivityRepository.GetParentDistributer(UserID, RoleID), srdLoc);        
            return srdLoc;
        }
        #endregion 

        #region RaceMaster
        /// <summary>
        /// Added by Prashant 18 Nov 2015
        /// </summary>      

        public List<RaceBrandMasterDTO> GetRaceBrandMaster(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            List<RaceBrandMasterDTO> result = new List<RaceBrandMasterDTO>();
            ObjectMapper.Map(ActivityRepository.GetRaceBrandMaster(userID, roleID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out  MaxModifiedDate), result);
            return result;
        }

        public List<RaceProductCategoryDTO> GetRaceProductCategory(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            List<RaceProductCategoryDTO> result = new List<RaceProductCategoryDTO>();
            ObjectMapper.Map(ActivityRepository.GetRaceProductCategory(userID, roleID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out  MaxModifiedDate), result);
            return result;
        }

        public List<RaceBrandCategoryMappingDTO> GetRaceBrandCategoryMapping(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            List<RaceBrandCategoryMappingDTO> result = new List<RaceBrandCategoryMappingDTO>();
            ObjectMapper.Map(ActivityRepository.GetRaceBrandCategoryMapping(userID, roleID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out  MaxModifiedDate), result);
            return result;
        }
        #endregion

        #region RACE services for sync adaptor
        public List<RacePOSMMasterDTO> GetRACEPOSMMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            List<RacePOSMMasterDTO> result = new List<RacePOSMMasterDTO>();
            ObjectMapper.Map(ActivityRepository.GetRACEPOSMMaster(userID, roleID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out MaxModifiedDate), result);
            return result;
        }


        public List<RaceFixtureMasterDTO> GetRACEFixtureMaster(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            List<RaceFixtureMasterDTO> result = new List<RaceFixtureMasterDTO>();
            ObjectMapper.Map(ActivityRepository.GetRACEFixtureMaster(userID, roleID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out MaxModifiedDate), result);
            return result;
        }


        public List<RacePOSMProductMappingDTO> GetRACEPOSMProductMapping(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            List<RacePOSMProductMappingDTO> result = new List<RacePOSMProductMappingDTO>();
            ObjectMapper.Map(ActivityRepository.GetRACEPOSMProductMapping(userID, roleID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out MaxModifiedDate), result);
            return result;
        }
        #endregion
    }
}
