
using AccuIT.PersistenceLayer.Repository.Entities;
using System.Collections.Generic;
using AccuIT.CommonLayer.Aspects.Utilities;
using System;
using System.Data;
using AccuIT.BusinessLayer.Services.BO;

namespace AccuIT.PersistenceLayer.Repository.Contracts
{
    /// <summary>
    /// Interface to define methods for Application system services
    /// </summary>
    public interface ISystemRepository
    {

        List<int> UploadMasterDataParking2Main(DataTable dtDemoValidation, int enumMaster);

        AddressMaster GetAddressDetails(int addressID, int? ID, int? Type);
        AddressMaster GetAddressDetailsByType(int typeID, int typePkID);
        TemplateMaster GetTemplateData(int templateID, int? code);
        List<TemplateMergeField> GetTemplateMergeFields(int templateID);

        List<TemplateMaster> GetAllTemplates( int? type);

        TemplateMasterBO GetBasicTemplateInfo(int ID);

        int SubmitNewOrder(OrderMaster Order);
        int UpdateOrder(OrderMaster Order);

        int SubmitUserSubscription(UserWeddingSubscription Subscription);
        int UpdateUserSubscription(UserWeddingSubscription Subscription);

        List<TemplateImage> SubmitTemplateImages(List<TemplateImage> Images);
        /// <summary>
        /// Method to validate WCF service user access 
        /// </summary>
        /// <param name="apiKey">provided API Key value</param>
        /// <param name="apiToken">provided API Token value</param>
        /// <returns>returns boolean status</returns>
        bool IsValidServiceUser(string apiKey, string apiToken, string userID);

        /// <summary>
        /// Method to get company system setting values
        /// </summary>
        /// <param name="companyID">company ID</param>
        /// <returns>returns entity instance</returns>
        SystemSetting GetCompanySystemSettings(int companyID);

        ///// <summary>
        ///// Method to log error details onto server database
        ///// </summary>
        ///// <param name="errorLog">error log DTO instance</param>
        ///// <returns>returns boolean response</returns>
        //bool WriteLog(ErrorLog errorLog);

        //#region Admin Module

        ///// <summary>
        ///// Insert/Update System Settings
        ///// </summary>
        ///// <param name="systemSetting"></param>
        ///// <returns></returns>
        //bool InsertUpdateSystemSetting(SystemSetting systemSetting, int? RoleID);

        ///// <summary>
        ///// Gets the system settings.
        ///// </summary>
        ///// <returns></returns>
        //SystemSetting GetSystemSettings();

        //#region Methods Added for Role Master :Dhiraj 3-Dec-2013

        /// <summary>
        /// Get All role data
        /// </summary>
        /// <returns></returns>
        List<RoleMaster> GetRoleMasters();

        ///// <summary>
        ///// Get All role based on user Company
        ///// </summary>
        ///// <param name="companyID"></param>
        ///// <returns></returns>
        //List<RoleMaster> GetRoleMasters(int companyID);

        ///// <summary>
        ///// Get All Role Name with team and Profile Level
        ///// </summary>
        ///// <param name="companyID"></param>
        ///// <param name="profileLevel"></param>
        ///// <returns></returns>
        //List<spGetRoleMaster_Result> GetRoleMasters(int companyID, int? profileLevel);

        ///// <summary>
        ///// Get Approval Path Master for roleId and expenseType
        ///// </summary>
        ///// <param name="roleId"></param>
        ///// <param name="expenseType"></param>
        ///// <returns></returns>
        //List<ApproverPathMaster> GetApprovalParthMaster(int roleId, int expenseType);
        ///// <summary>
        ///// Get role data for roleid
        ///// </summary>
        ///// <returns></returns>
        //List<RoleMaster> GetRoleMastersByRoleId(Int32 RoleId);
        /// <summary>
        /// Get All rolemoudle data for particular roleID
        /// </summary>
        /// <param name="RoleID">Particular Role ID for which data of role module needs to be fetched</param>
        /// <returns>List of RoleModule</returns>
        List<RoleModule> GetRoleModulesByRoleID(int RoleID, int? ModuleID);
        /// <summary>
        /// Gets the module name by module identifier.
        /// </summary>
        /// <param name="moduleID">The module identifier.</param>
        /// <returns></returns>
        string GetModuleNameByModuleID(int moduleID);
        /// <summary>
        /// Get All Permissions
        /// </summary>        
        /// <returns>List of Valid permisions</returns>
        List<Permission> GetPermissions();
        /// <summary>
        /// Get All RoleModulePermissions
        /// </summary>        
        /// <returns>List of Valid permisions</returns>
        List<UserRoleModulePermission> GetUserRoleModulePermisions();
        /// <summary>
        /// Insert update RoleModulePermission of user
        /// </summary>        
        /// <returns>Success or Failure</returns>
        bool InsertUpdateUserRoleModulePermision(UserRoleModulePermission userRoleModulePermission);
        /// <summary>
        /// Insert Role Module data
        /// </summary>        
        /// <returns>Success or Failure</returns>
        bool InsertRoleModule(RoleModule roleModule);
        /// <summary>
        /// Delete Role Module data
        /// </summary>        
        /// <returns>Success or Failure</returns>
        bool DeleteRoleModule(RoleModule roleModule);

        //#endregion

        /// <summary>
        /// Function to get the Modules
        /// </summary>
        IList<ModuleMaster> GetModulesList(bool? isMobile = null);

        ///// <summary>
        ///// Gets the apk module list.
        ///// </summary>
        ///// <param name="isMobile">if set to <c>true</c> [is mobile].</param>
        ///// <returns></returns>
        //IList<Module> GetAPKModuleList(bool isMobile);

        ///// <summary>
        ///// Function to bind the Questions base on Module
        ///// </summary>
        ///// <param name="moduleID">ModuleID</param>
        ///// <returns>Question List</returns>
        //IList<SurveyQuestion> GetQuestionList(int moduleID);

        ///// <summary>
        ///// Function to find the Questions base on search string data for particular module code
        ///// </summary>
        ///// <param name="moduleID">ModuleID</param>
        ///// <returns>Question List</returns>
        //IList<SurveyQuestion> GetQuestionList(int moduleCode, string strSearchData);

        ///// <summary>
        ///// Function to bind the Questions base on Module
        ///// </summary>
        ///// <param name="moduleID">ModuleID</param>
        ///// <returns>Answer List</returns>
        //IList<SurveyQuestionAttribute> GetAnswerList(int questionID);

        ///// <summary>
        ///// Function to bind the Questions base on Module
        ///// </summary>
        ///// <param name="moduleID">ModuleID</param>
        ///// <returns>Answer List</returns>
        //SurveyQuestionAttribute GetAnswer(int SurveyOptionID);




        ///// <summary>
        ///// Function to bind the QuestionsType
        ///// </summary>     
        ///// <returns>Question Type List</returns>
        //IList<QuestionType> QuestionTypeList(bool isForRepeaterType);
        //#region Added By Vinay Kanojia: Dated 17-Dec-2013

        ///// <summary>
        ///// Function to bind the Competitor List
        ///// </summary>     
        ///// <returns>Competitor List</returns>
        //IList<Competitor> GetCompetitorList(int productTypeID);

        ///// <summary>
        ///// Function to Get the Competitor Details base on CompetitorID.
        ///// </summary>
        ///// <param name="competitorID"></param>
        ///// <returns>Competitor Class object</returns>
        //Competitor CompetitorDetails(int competitorID);

        ///// <summary>
        ///// Funtion to Insert Competitor
        ///// </summary>
        ///// <param name="response"></param>
        ///// <returns></returns>
        //bool IsSuccessfullInsert(Competitor response);

        ///// <summary>
        ///// Funtion to Delete Competitor
        ///// </summary>
        ///// <param name="competitorID"></param>
        ///// <returns></returns>
        //bool ISDeleteCompetitor(int competitorID);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IList<ModuleMaster> GetModuleList(bool? isMobile = null);
        /// <summary>
        /// find all module for mobile for modume name
        /// </summary>
        /// <param name="isMobile"></param>
        /// <param name="strModuleName"></param>
        /// <returns></returns>
        IList<ModuleMaster> GetModuleList(bool? isMobile, string strModuleName);
        //IList<FeedbackCategoryMaster> GetFeedbackCategoryList();
        //IList<Module> GetParentModule(int ModuleID);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        bool IsModuleInsert(ModuleMaster response);


        ///// </summary>
        ///// <param name="response"></param>
        ///// <returns></returns>
        //bool IsSuccessCategoryInsert(FeedbackCategoryMaster response);

        //bool IsApkInserted(APKMaintainance response);
        //IList<APKMaintainance> GetApkMaintanceList();
        //#region GetAll Channel Master and Update

        ///// <summary>
        /////Adde by Tanuj(9-4-2014)
        ///// </summary>
        ///// <returns></returns>

        //IList<ChannelMaster> GetChannelMaster();
        //bool UpdateChannelMaster(List<int> channels);
        //#endregion
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="competitorID"></param>
        ///// <returns></returns>
        //Module ModuleDetail(int moduleID);


        //APKMaintainance ApkDetail(int Apkid);
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="moduleID"></param>
        ///// <returns></returns>
        //bool DeleteModules(List<int> modules);

        ////bool UpdateReportProfile(List<int> profiles,List<int>ISEffective);
        //bool UpdateReportProfile(List<RoleMaster> lstRoleMaster);
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="lstRoleMaster"></param>
        ///// <returns></returns>
        //bool UpdateReportProfileForRoleId(RoleMaster lstRoleMaster);
        ///// <summary>
        ///// Channel Master Exclusion(Added by Tanuj(9-4-2014))
        ///// </summary>
        ///// <param name="channels">channels</param>
        ///// <returns>bool</returns>

        //#endregion

        ///// <summary>
        ///// Function to bind the Questions base on Module
        ///// </summary>
        ///// <param name="moduleID">ModuleID</param>
        ///// <returns>0 or 1</returns>
        //bool ISDeleteQuestion(int questionID);

        ///// <summary>
        ///// Determines whether [is active question] [the specified question identifier].
        ///// </summary>
        ///// <param name="questionID">The question identifier.</param>
        ///// <returns></returns>
        //bool ISActiveQuestion(int questionID);


        ///// <summary>
        ///// Function to Add the Questions base on Module
        ///// </summary>
        ///// <param name="SurveyQuestion">SurveyQuestion</param>
        ///// <returns>0 or 1</returns>
        //bool SaveSurveyQuestion(SurveyQuestion record, List<AnswerAttribute> resultAnswer);

        ///// <summary>
        ///// Function to Update/Delete the Answers Details base on AnswerID.
        ///// </summary>
        ///// <param name="SurveyQuestionAttribute">record</param>
        ///// <returns>true or false</returns>

        //bool IsUpdatedAnswer(SurveyQuestionAttribute record);

        ///// <summary>
        ///// Function to Delete the Answers Details base on AnswerID.
        ///// </summary>
        ///// <param name="SurveyQuestionAttribute">record</param>
        ///// <returns>true or false</returns>

        //bool IsDeleteAnswer(int ansID);

        ///// <summary>
        ///// Function to Get the Questions Details base on QuestionID.
        ///// </summary>
        ///// <param name="questionID">questionID</param>
        ///// <returns>SurveyQuestion class</returns>
        //SurveyQuestion QuestionDetails(int questionID);

        ///// <summary>
        ///// Method to get image name of uploaded image for an entity
        ///// </summary>
        ///// <param name="entityID">entity ID</param>
        ///// <param name="imageType">image type</param>
        ///// <returns>returns file name</returns>
        //string GetEntityImageName(string entityID, AspectEnums.ImageFileTypes imageType);

        ///// <summary>
        ///// Method to image name of uploaded image for an entity
        ///// </summary>
        ///// <param name="entityID">entity ID</param>
        /////<param name="fileName">file name to update</param>
        /////<param name="fileType">file type</param>
        ///// <returns>returns file name</returns>
        //bool UpdateEntityImageName(string entityID, string fileName, AspectEnums.ImageFileTypes fileType);

        ///// <summary>
        ///// Method to fetch prouduct list
        ///// </summary>
        ///// <param name="companyID">company ID</param>
        ///// <returns>returns product list</returns>
        //IList<ProductMaster> GetProductList(int companyID);

        ///// <summary>
        ///// Method to get the pending entities which are not synced
        ///// </summary>
        ///// <param name="companyID">company primary ID</param>
        ///// <param name="userID">user primary ID</param>
        ///// <returns>returns pending item list</returns>
        //IList<SyncTable> GetPendingSyncEntities(int companyID, long userID);

        ///// <summary>
        ///// Method to update user table sysnc history
        ///// </summary>
        ///// <param name="syncTableID">sync table ID</param>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns boolean status</returns>
        //bool UpdateSyncEntity(int syncTableID, long userID);

        //#endregion


        ///// <summary>
        ///// Method to get product repository on the basis of company
        ///// </summary>
        ///// <param name="companyID">company primary id value</param>
        ///// <returns>returns product collection</returns>
        //IList<vwProductRepository> GetProductRepository(int companyID, long? userID);


        ///// <summary>
        ///// Method to get product repository on the basis of company for APK
        ///// </summary>
        ///// <param name="companyID">company primary id value</param>
        ///// <returns>returns product collection</returns>
        //IList<vwProductRepository> GetProductRepository(int companyID, long? userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        ///// <summary>
        ///// Method to get product group on the basis of product type
        ///// </summary>
        ///// <param name="productTypeID">product Type id value</param>
        ///// <returns>returns product collection</returns>
        //IList<vwProductRepository> GetProductGroup(int productTypeID);

        ///// <summary>
        ///// Method to Get Product Type Added By Vinay Kanojia
        ///// </summary>
        ///// <param name="companyID"> On Basis of Company</param>
        ///// <returns>List of the Product Type</returns>
        //IList<vwProductRepository> GetProductType(int companyID);

        ///// <summary>
        ///// Method to get payment mode list on the basis of company
        ///// </summary>
        ///// <param name="companyID">company ID</param>
        ///// <returns></returns>
        //IList<PaymentMode> GetPaymentModes(int companyID, long? userID);


        ///// <summary>
        ///// Method to get payment mode list on the basis of company for APK
        ///// </summary>
        ///// <param name="companyID">company ID</param>
        ///// <returns></returns>
        //IList<PaymentMode> GetPaymentModes(int companyID, long? userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);


        /// <summary>
        /// Function to get the Modules on the basis of role
        /// </summary>
        IList<RoleModule> GetModulesListByRole(int roleID);

        ///// <summary>
        ///// Method to get role module questions
        ///// </summary>
        ///// <param name="roleID">role id</param>
        ///// <param name="moduleID">module id</param>
        ///// <returns>rteurns list of questions</returns>
        //IList<RoleModuleQuestion> GetRoleModuleQuestions(int roleID, int moduleID);

        ///// <summary>
        ///// Method to submit role questions
        ///// </summary>
        ///// <param name="questions">question list</param>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns boolean response</returns>
        //bool SubmitRoleSurveyQuestions(IList<RoleModuleQuestion> questions, long userID);

        ///// <summary>
        ///// Method to get product details
        ///// </summary>
        ///// <param name="productID">product ID</param>
        ///// <returns>returns product details</returns>
        //ProductMaster GetProductDetails(int productID);

        ///// <summary>
        ///// Method to get email smtp server details
        ///// </summary>
        ///// <returns>returns smtp server entity instance</returns>
        //SMTPServer GetEmailServerDetails();

        ///// <summary>
        ///// Method to update email service status
        ///// </summary>
        ///// <param name="emailServiceID">email service ID</param>
        ///// <param name="status">status to update</param>
        ///// <param name="remarks">remarks</param>
        ///// <returns>boolean response</returns>
        //bool UpdateEmailServiceStatus(long emailServiceID, int status, string remarks);

        ///// <summary>
        ///// Insert email entry into database
        ///// </summary>
        ///// <param name="email">email</param>
        ///// <returns>returns response</returns>
        //bool InsertEmailRecord(EmailService email);

        ///// <summary>
        ///// Method to save announcement text into database
        ///// </summary>
        ///// <param name="startDate">start date</param>
        ///// <param name="endDate">end date</param>
        ///// <param name="announcement">announcement text to save</param>
        ///// <returns>returns int status</returns>
        //int SaveAnnouncements(DateTime startDate, DateTime? endDate, string webAnnouncement, string mobileAnnouncement, int RoleID);

        ///// <summary>
        ///// Method to get announcements from date provided
        ///// </summary>
        ///// <param name="currentDate">current date</param>
        ///// <returns>returns text</returns>
        //List<Announcement> GetAnnouncement(DateTime currentDate, int? RoleID);
        ///// <summary>
        ///// Method to get announcements from date provided
        ///// </summary>
        ///// <param name="currentDate">current date</param>
        ///// <returns>returns text</returns>
        //APKMaintainance IsApkVersionUpdated(string apkVersion);

        ///// <summary>
        ///// Method to update notifications
        ///// </summary>
        ///// <param name="service">service</param>
        ///// <returns>returns boolean status</returns>
        //bool UpdateNotification(long notificationServiceID, string remarks, short status);
        ///// <summary>
        ///// Method to save notifications text into database
        ///// </summary>
        ///// <param name="notificationString"></param>
        ///// <param name="regionGeoDefId"></param>
        ///// <param name="profileId"></param>
        ///// <returns></returns>
        //int SaveNotification(string notificationString, int regionGeoDefId, int profileId, long userId, DateTime startDate, DateTime endDate, int frequency, string NotificationSubject);

        ///// <summary>
        ///// Method to update push notification service response
        ///// </summary>
        ///// <param name="notificationServiceID">notification service ID</param>
        ///// <param name="response">response</param>
        ///// <param name="isSuccess">is success</param>
        ///// <returns>returns boolean response</returns>
        //bool UpdateNotificationServiceResponse(long notificationServiceID, string response, bool isSuccess);

        ///// <summary>
        ///// Method to update push notification service response
        ///// </summary>
        ///// <param name="notificationServiceID">notification service ID</param>
        ///// <param name="response">response</param>
        ///// <param name="isSuccess">is success</param>
        ///// <returns>returns boolean response</returns>
        //bool UpdateNotificationServiceResponse(List<long> NotificationServiceID, string response, long notificationID, DateTime datetime);


        ///// <summary>
        ///// Method to discard notification service
        ///// </summary>
        ///// <param name="notificationServiceID">notification service</param>
        ///// <returns>returns notification service</returns>
        //bool DiscardNotificationService(long notificationServiceID);
        /// <summary>
        /// This function will validate service token by userid
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        int GetServiceTokenUserID(string apiKey, string apiToken);

        ////VC20140819
        ///// <summary>
        ///// List of time slots for coverage Batch Notification
        ///// </summary>        
        ///// <returns>List of time slots for coverage Batch Notification</returns>
        ///// 
        //CoverageNotificationTimeSetting CoverageNotificationTimeSetting();

        ///// <summary>
        ///// List Coverage Notification Message data
        ///// </summary>                
        ///// 
        //List<CoverageNotificationService> GetCoverageNotificationService();


        ///// <summary>
        ///// Method to update push notification service response for Coverage
        ///// </summary>
        ///// <param name="CoveragenotificationServiceID">Coverage notification service ID</param>
        ///// <param name="response">response</param>
        ///// <param name="isSuccess">is success</param>
        ///// <returns>returns boolean response</returns>
        //bool UpdateCoverageNotificationServiceResponse(List<CoverageNotificationService> service);
        ////VC20140819

        ////VC20140826
        ///// <summary>
        ///// Method to get notificationservice data for current date
        ///// </summary>
        ///// <param name="notificationdate">currentdatetime</param>        
        ///// <returns></returns>
        //List<NotificationService> GetNotificationService(DateTime datetime);

        ////VC20140826

        //#region DataMaster for APK Download VC20140915
        ///// <summary>
        ///// Function to get the Datamaster for Download
        ///// </summary>
        //IList<DownloadDataMaster> GetDownloadDataMasterList();

        ///// <summary>
        ///// Get All Download Master data for particular Role ID
        ///// </summary>
        ///// <param name="RoleID">Particular Role ID for which data of Download Authorization needs to be fetched</param>
        ///// <returns>List of Master Data for a Role </returns>
        //List<DownloadMasterAuthorization> GetDownloadDataAuthorizationByRoleID(int RoleID);

        ///// <summary>
        ///// Insert APK Download Authorization data
        ///// </summary>        
        ///// <returns>Success or Failure</returns>
        //bool InsertAPKDataAuthorization(DownloadMasterAuthorization APKAuthorization);
        ///// <summary>
        ///// Delete APK Download Authorization data for a particular Role
        ///// </summary>        
        ///// <returns>Success or Failure</returns>
        //bool DeleteAPKDataAuthorization(DownloadMasterAuthorization APKAuthorization);
        //#endregion

        //#region Unfreeze Stores
        ///// <summary>
        ///// Validate StoreCode in StoreMaster
        ///// </summary>
        ///// <param name="StoreCodes"></param>
        ///// <returns>List of storecodes if not found in storemaster</returns>        
        //List<string> ValidateStoreCode(List<string> StoreCodes);


        ///// <summary>
        ///// Unfreeze stores
        ///// </summary>
        ///// <param name="StoreCodes"></param>
        ///// <returns>true or false</returns>        
        //bool UnfreezeGeoTag(List<string> StoreCodes);
        //#endregion

        //#region SDCE-579 Added by Niranjan (Product Group Category) 13-10-2014

        ///// <summary>
        ///// Function to bind the Product Group Category
        ///// </summary>
        ///// <param name="ProductGroupCategoryId">ProductGroupCategoryId</param>
        ///// <returns>Product List</returns>
        //IList<ProductGroupCategory> GetProductGroupCategory();


        ///// <summary>
        ///// Funtion to Insert Product Group Category
        ///// </summary>
        ///// <param name="response"></param>
        ///// <returns></returns>
        //bool IsProductGroupCategoryInsert(ProductGroupCategory response);

        ///// <summary>
        ///// Method to get detail on the basis of ProductGroupCategory Id ,ProductGroupCategoryName
        ///// </summary>
        ///// <param name="ProductGroupCategoryId"></param>
        ///// <returns></returns>
        //ProductGroupCategory ProductGroupCategoryDetail(int? ProductGroupCategoryId, string ProductGroupCategoryName);
        ///// <summary>
        ///// Funtion to Delete Product Group Category
        ///// </summary>
        ///// <param name="ProductGroupCategoryId"></param>
        ///// <returns></returns>
        //bool DeleteProductGroupCategories(List<int> ProductGroupCategory);
        //#endregion

        //#region SDCE-670 Added by Niranjan (Channel Type Mapping) 16-10-2014

        ///// <summary>
        /////GetAll Channel Type Mapping  and Update 
        ///// </summary>
        ///// <returns></returns>

        //IList<ChannelTypeDisplay> GetChannelTypeMappingList();
        //bool UpdateChannelTypeDisplay(List<ChannelTypeDisplay> response);
        //#endregion

        //#region SDCE-634 :: Activity Role Module Authorization  (17 Oct 2014)
        ///// <summary>
        ///// Get All activitymoudle data for particular roleID for SDCE-634
        ///// </summary>
        ///// <param name="RoleID">Particular Role ID for which data of role module needs to be fetched</param>
        ///// <returns>List of ActivityModule</returns>
        //List<ActivityModule> GetActivityModulesByRoleID(int RoleID);


        ///// <summary>
        ///// Insert activity Role Module data
        ///// </summary>        
        ///// <returns>Success or Failure</returns>
        //bool InsertactivityRoleModule(ActivityModule roleModule);
        ///// <summary>
        ///// Delete activity Role Module data
        ///// </summary>        
        ///// <returns>Success or Failure</returns>
        //bool DeleteactivityRoleModule(ActivityModule roleModule);

        //#endregion

        //#region Upload VOC Data for SDCE-892 by Vaishali on 12-Nov-2013
        ///// <summary>
        ///// VOC Upload
        ///// </summary>
        ///// <param name="VOC"></param>
        ///// <param name="userid"></param>
        ///// <returns></returns>
        ///// <returns>List of storecodes if not found in storemaster</returns>        
        //bool VOCUpload(string VOCxml, long userid);


        /// <summary>
        /// CommonSetup 
        /// </summary>
        /// <param name="MainType"></param>
        /// <param name="subtype"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        List<CommonSetup> GetCommonSetup(int DisplayValue, string subtype, string parentid);
        //#endregion

        //#region for SDCE -991 (FMS) by vaishali on 06 Dec 2014
        //bool QueueNotification(long userID, string notificationMessage, AspectEnums.NotificationType notificationType, int? NotificationID = null);
        //#endregion


        //#region Ageing support for End of life products
        ///*
        // Created By     ::      Vaishali Choudhary
        // Created Date   ::      09 March 2015
        // JIRA ID        ::      
        // Purpose        ::      Services for Ageing support 
        // */

        //#region Product selection
        ///// <summary>
        ///// Show the list of Product Types
        ///// </summary>
        ///// <returns>List of Product Types</returns>
        //List<ProductType> GetProductType();

        ///// <summary>
        ///// Show the list of Product Groups
        ///// </summary>
        ///// <returns>List of Product Groups</returns>
        //List<ProductGroup> GetProductGroup(string productTypeCode);

        ///// <summary>
        ///// Show the list of Product Category
        ///// </summary>
        ///// <returns>List of Product Category</returns>
        //List<ProductCategory> GetProductCategory(string productTypeCode, string productGroupCode);

        ///// <summary>
        ///// Show the list of Basic Models under ProductType, ProductGroup and ProductCategory
        ///// </summary>
        ///// <returns>List of Basic Models</returns>
        //List<ProductModel> GetBasicModel(string productTypeCode, string productGroupCode, string CategoryCode);
        //#endregion

        //#region scheme Implementation
        ///// <summary>
        ///// Get list of Schemes 
        ///// </summary>
        ///// <param name="SchemeID"></param>
        ///// <param name="SchemeNumber"></param>
        ///// <returns></returns>
        //List<EOLSchemeHeader> GetAllEOLSchemes(int? SchemeID, string SchemeNumber);

        ///// <summary>
        ///// Save or Update scheme in Database and return ID of registered Scheme
        ///// </summary>
        ///// <param name="scheme"></param>
        ///// <param name="ActionType"></param>
        ///// <returns></returns>
        //EOLSchemeHeader EOLSaveScheme(EOLSchemeHeader scheme, byte ActionType, long userID);

        ///// <summary>
        ///// Save Products details for a scheme
        ///// </summary>
        ///// <param name="schemeProducts"></param>
        ///// <returns></returns>
        //EOLSchemeHeader EOLSaveSchemeProducts(List<EOLSchemeDetail> schemeProducts, long userID, bool isSubmit);

        //#endregion

        //#region APK services
        ///// <summary>
        ///// Get list of Schemes active in current date
        ///// </summary>
        ///// <param name="SchemeID"></param>
        ///// <param name="SchemeNumber"></param>
        ///// <returns></returns>
        //List<EOLSchemeHeader> GetEOLSchemes(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        ///// <summary>
        ///// Capture order against scheme
        ///// </summary>
        ///// <param name="eolOrderBookings"></param>
        ///// <param name="userID"></param>
        ///// <param name="storeID"></param>        
        ///// <returns>true or false</returns>
        //List<EOLScheme> SubmitEOLOrder(List<EOLOrderBooking> eolOrderBookings, long userID);

        ///// <summary>
        ///// Select Last Order for a scheme
        ///// </summary>
        ///// <param name="schemeID"></param>
        ///// <returns></returns>
        //List<EOLOrderBooking> LastsavedEOLActivity(int schemeID, int StoreID, bool returnAllSchemes);
        //#endregion


        //#region Scheme Report

        //List<SPGetSchemeReport_Result> GetSchemeReport(long UserID, int SelectedRoleID, DateTime? schemePeriodFrom, DateTime? schemePeriodTo, DateTime? orderSubmissionFrom, DateTime? orderSubmissionTo);

        //#endregion

        //#endregion


        ///// <summary>
        ///// Returns list of users to which EOL scheme notification need to be sent
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <param name="userRole"></param>
        ///// <returns></returns>
        //long EOLNotificationUserID(long userID);

        //List<NotificationMessage> NotificationMessages(int NotificationType, int NotificationSubType, byte InterfaceType);


        //#region Maintain EOL Log
        ///// <summary>
        ///// Maintain Log of EOL Notifications
        ///// </summary>
        ///// <param name="eolscheme"></param>
        ///// <param name="SchemeNotificationType"></param>
        ///// <param name="NotificationUserID"></param>
        ///// <param name="PushNotificationMessage"></param>
        ///// <returns></returns>
        //bool EOLNotificationLog(int schemeID, AspectEnums.ODScheme SchemeNotificationType, int NotificationUserID, string PushNotificationMessage);

        ///// <summary>
        ///// Remove EOL Notification Log 
        ///// </summary>
        ///// <param name="schemeID"></param>
        ///// <returns></returns>
        //bool RemoveEOLNotificationLog(int schemeID, int? userID);

        //#endregion
        ///// <summary>
        ///// Select list of all users under given moduleCode
        ///// </summary>
        ///// <param name="ModuleCode"></param>
        ///// <returns></returns>
        //List<long> GetUsersUnderModule(int ModuleCode);

        //bool IsDownloadAuthorized(long userID, AspectEnums.DownloadService ServiceModule);


        //#region Beat Window settings Role Wise


        ///// <summary>
        ///// get Beat Window setting for selected Role
        ///// </summary>
        ///// <returns></returns>
        //BeatWindowSetting GetBeatWindowSettings(int RoleID);

        //#endregion

        //#region SmartDost Scheme with HTML content
        ///// <summary>
        ///// Displays Schemes valid in current date
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <param name="RoleID"></param>
        ///// <returns></returns>
        //List<SDScheme> GetTodaySchemes(long userID, long RoleID);


        ///// <summary>
        ///// Displays All available Schemes  
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <param name="RoleID"></param>
        ///// <returns></returns>
        //List<SDScheme> GetAllSchemes(long userID, long RoleID);


        ///// <summary>
        ///// Get scheme details against a scheme ID
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <param name="RoleID"></param>
        ///// <returns></returns>
        //SDScheme GetSchemeDetails(int SDSchemeID, string SchemeTitle);


        ///// <summary>
        ///// Deletes the schemes.
        ///// </summary>
        ///// <param name="schemeList">The scheme list.</param>
        ///// <returns></returns>
        //bool DeleteSchemes(List<SDScheme> schemeList);


        ///// <summary>
        ///// Adds the update scheme.
        ///// </summary>
        ///// <param name="scheme">The scheme.</param>
        ///// <returns></returns>
        //bool AddUpdateScheme(SDScheme scheme);
        //#endregion

        //#region Upload New VOC  Manoranjan
        ///// <summary>
        /////  Upload New VOC
        ///// </summary>
        ///// <param name="VOCxml"></param>
        ///// <param name="userid"></param>
        ///// <returns></returns>
        //bool NewVOCUpload(string VOCxml, long userid, byte uploadtype);

        ///// <summary>
        ///// get Product category for New voc sentiment  report
        ///// </summary>
        ///// <param name="MainType"></param>
        ///// <param name="subtype"></param>
        ///// <param name="parentid"></param>
        ///// <returns></returns>
        //List<CommonSetup> getNewVOCProductCategory(string MainType, string subtype, int parentid);


        //List<string> GetQuestionVOC(string UploadType);

        //#endregion

    }
}
