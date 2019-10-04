using AccuIT.BusinessLayer.Services.BO;
using AccuIT.CommonLayer.Aspects.DTO;
#region Namespace Added for Role Master :Dhiraj 3-Dec-2013
using System.Collections.Generic;
using System.Collections;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.CommonLayer.Aspects.ReportBO;
using System;
using AccuIT.CommonLayer.Aspects.DTO;
using System.Data;
using System.Threading.Tasks;
#endregion
namespace AccuIT.BusinessLayer.Services.Contracts
{
    public interface ISystemService
    {

        /// <summary>
        /// CommonSetup 
        /// </summary>
        /// <param name="MainType"></param>
        /// <param name="subtype"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        List<CommonSetupDTO> GetCommonSetup(int DisplayValue, string subtype, string parentid);

        /// <summary>
        /// Method to validate WCF service user access 
        /// </summary>
        /// <param name="apiKey">provided API Key value</param>
        /// <param name="apiToken">provided API Token value</param>
        /// <returns>returns boolean status</returns>
        /// 
        List<int> UploadMasterDataParking2Main(DataTable dtDemoValidation, int enumMaster);

        AddressMasterBO GetAddressDetails(int addressID, int? ID, int? Type);
        AddressMasterBO GetAddressDetailsByType(int typeID, int typePkID);
        TemplateMasterBO GetTemplateData(int templateID, int? code);

        List<TemplateMergeFieldBO> GetTemplateMergeFields(int templateID);
        List<TemplateMasterBO> GetAllTemplates(int? type);

        List<TemplateImageBO> SubmitTemplateImages(List<TemplateImageBO> Images);

        int SubmitNewOrder(OrderMasterBO Order);
        int UpdateOrder(OrderMasterBO Order);

        int SubmitUserSubscription(UserWeddingSubscriptionBO Subscription);
        int UpdateUserSubscription(UserWeddingSubscriptionBO Subscription);

        bool IsValidServiceUser(string apiKey, string apiToken, string userID);

        ///// <summary>
        ///// Method to get company system setting values
        ///// </summary>
        ///// <param name="companyID">company ID</param>
        ///// <returns>returns entity instance</returns>
        //SystemSettingBO GetCompanySystemSettings(int companyID);

        ///// <summary>
        ///// Method to log error details onto server database
        ///// </summary>
        ///// <param name="errorLog">error log DTO instance</param>
        ///// <returns>returns boolean response</returns>
        //bool WriteLog(ErrorLogDTO errorLog);

        //#region Admin Module

        ///// <summary>
        ///// Inserts the update system setting.
        ///// </summary>
        ///// <param name="systemSetting">The system setting.</param>
        ///// <returns></returns>
        //bool InsertUpdateSystemSetting(SystemSettingBO systemSetting, int? RoleID);

        ///// <summary>
        ///// Gets the system settings.
        ///// </summary>
        ///// <returns></returns>
        //SystemSettingBO GetSystemSettings();

        //#region Methods Added for Role Master :Dhiraj 3-Dec-2013
        /// <summary>
        /// Get All role data
        /// </summary>
        /// <returns></returns>
        List<RoleMasterBO> GetRoleMasters();


        ///// <summary>
        ///// Get All role based on user Company
        ///// </summary>
        ///// <param name="companyID"></param>
        ///// <returns></returns>
        //List<RoleMasterBO> GetRoleMasters(int companyID);

        ///// <summary>
        ///// Get All Role Name with team and Profile Level
        ///// </summary>
        ///// <param name="companyID"></param>
        ///// <param name="profileLevel"></param>
        ///// <returns></returns>
        //List<spGetRoleMasterBO> GetRoleMasters(int companyID, int? profileLevel);

        ///// <summary>
        ///// Ge tApproval Parth Master for roleId and expenseType
        ///// </summary>
        ///// <param name="roleId"></param>
        ///// <param name="expenseType"></param>
        ///// <returns></returns>
        //List<ApproverPathMasterBO> GetApprovalParthMaster(int roleId, int expenseType);
        ///// <summary>
        ///// Get role data for roleid
        ///// </summary>
        ///// <returns></returns>
        //List<RoleMasterBO> GetRoleMastersByRoleId(Int32 RoleId);
        /// <summary>
        /// Get All rolemoudle data for particular roleID
        /// </summary>
        /// <param name="RoleID">Particular Role ID for which data of role module needs to be fetched</param>
        /// <returns>List of RoleModule</returns>
        List<RoleModuleBO> GetRoleModulesByRoleID(int RoleID, int? ModuleID);

        /// <summary>
        /// Get All Permissions
        /// </summary>        
        /// <returns>List of Valid permisions</returns>
        List<PermissionBO> GetPermissions();
        /// <summary>
        /// Get All RoleModulePermissions
        /// </summary>        
        /// <returns>List of Valid permisions</returns>
        IList<UserRoleModulePermissionBO> GetUserRoleModulePermisions();
        /// <summary>
        /// Get All RoleModulePermissions based on RoleModules
        /// </summary>                
        /// <param name="roleModules">Collection of RoleModule</param>
        /// <returns>List of Valid permisions</returns>
        List<UserRoleModulePermissionBO> GetUserRoleModulePermisions(List<RoleModuleBO> roleModules);
        /// <summary>
        /// Insert update RoleModulePermission of user
        /// </summary>        
        /// <returns>List of Valid permisions</returns>
        bool InsertUpdateUserRoleModulePermision(List<UserRoleModulePermissionBO> userRoleModulePermissions);
        /// <summary>
        /// Insert Role Module data
        /// </summary>        
        /// <returns>Success or Failure</returns>
        bool InsertRoleModules(List<RoleModuleBO> roleModules);
        ///// <summary>
        /// Delete Role Module data for a particular Role
        /// </summary>        
        /// <returns>Success or Failure</returns>
        bool DeleteRoleModule(int roleID);
        /// <summary>
        /// Delete Role Module data for a particular Role
        /// </summary>        
        /// <returns>Success or Failure</returns>
        bool DeleteRoleModuleByRoleModule(RoleModuleBO roleModule);
        //#endregion


        //#region Added By Vinay Kanojia Dated : 17/12/2013


        ///// <summary>
        ///// Function to bind the Competitor
        ///// </summary>
        ///// <param name="ProductTypeID">productTypeID</param>
        ///// <returns>Question List</returns>
        //IList<CompetitorListBO> GetCompetitorList(int productTypeID);

        ///// <summary>
        ///// Function to Get the Competitor Details base on CompetitorID.
        ///// </summary>
        ///// <param name="questionID">questionID</param>
        ///// <returns>CompetitorListBO</returns>
        //CompetitorListBO CompetitorDetails(int competitorID);

        ///// <summary>
        ///// Function to Add the Competitor base on Product Type
        ///// </summary>
        ///// <param name="competitorID">CompetitorListBO</param>
        ///// <returns>true or false</returns>
        //bool IsInsert(CompetitorListBO record);

        ///// <summary>
        ///// Function to bind the Competitor base on Product Type
        ///// </summary>
        ///// <param name="competitorID">CompetitorID</param>
        ///// <returns>0 or 1</returns>
        //bool DeleteCompetitor(int competitorID);

        /// <summary>
        /// Method to get Module List
        /// </summary>
        /// <returns></returns>
        IList<ModuleMasterBO> GetModuleList(bool? isMobile = null);
        //IList<ModuleMasterBO> GetParentModule(int ModelID);
        ///// <summary>
        ///// Gets the apk module list.
        ///// </summary>
        ///// <param name="isMobile">The is mobile.</param>
        ///// <returns></returns>
        //IList<ModuleMasterBO> GetAPKModuleList(bool isMobile);
        ///// <summary>
        ///// Gets the Feedback Category Master list.
        ///// </summary>
        ///// <param></param>
        ///// <returns></returns>
        //IList<FeedbackCategoryMasterBO> GetFeedbackCategoryMaster();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        bool IsModuleInsert(ModuleMasterBO record);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="record"></param>
        ///// <returns></returns>
        //bool IsCategoryInsert(FeedbackCategoryMasterBO record);
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="ModuleID"></param>
        ///// <returns></returns>
        //ModuleMasterBO ModuleDetail(int moduleID);
        //APKMaintainanceBO ApkDetail(int apkId);


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="moduleID"></param>
        ///// <returns></returns>
        //bool DeleteModule(List<int> modules);
        //#endregion

        ////bool UpdateReportProfile(List<int> profiles,List<int>ISEffective);
        //bool UpdateReportProfile(List<RoleMasterBO> lstProfile);
        ///// <summary>
        ///// update for particular profile
        ///// </summary>
        ///// <param name="input"></param>
        ///// <returns></returns>
        //bool UpdateReportProfileForRoleId(RoleMasterBO input);
        ///// <summary>
        ///// Channel Master Exclusion
        ///// </summary>
        ///// <param name="channels"> channels</param>
        ///// <returns>bool</returns>



        //bool IsApkInserted(APKMaintainanceBO record);
        //IList<APKMaintainanceBO> GetApkMaintanceList();
        //#region GetALL Channel Master and Update
        ///// <summary>
        ///// Added By Tanuj(9-4-2014)
        ///// </summary>
        ///// <returns></returns>
        //IList<ChannelMasterBO> GetChannelMaster();
        //bool UpdateChannelMaster(List<int> channels);
        //#endregion


        //#region Question Master Methods
        ///// <summary>
        ///// Function to get the Modules
        ///// </summary>
        //IList<ModulesBO> ModulesList(bool? isMobile = null);

        ///// <summary>
        ///// Function to bind the Questions base on Module
        ///// </summary>
        ///// <param name="moduleID">ModuleID</param>
        ///// <returns>Question List</returns>
        //IList<QuestionListBO> QuestionList(int moduleID);

        ///// <summary>
        ///// Function to find the Questions base on Module code
        ///// </summary>
        ///// <param name="moduleID">ModuleID</param>
        ///// <returns>Question List</returns>
        //IList<QuestionListBO> QuestionList(int moduleCode, string strSearchData);

        ///// <summary>
        ///// Function to bind the Answer base on QuestionID
        ///// </summary>
        ///// <param name="questionID">QuestionID</param>
        ///// <returns>Answer List</returns>
        //IList<AnswerListBO> AnswersList(int questionID);

        ///// <summary>
        ///// function to bind answer by option id
        ///// Added By Nishat on 2July 2014
        ///// </summary>
        ///// <param name="SurveyOptionID"></param>
        ///// <returns></returns>
        //AnswerListBO GetAnswer(int SurveyOptionID);

        ///// <summary>
        ///// Function to bind the QuestionsType
        ///// </summary>     
        ///// <returns>Question Type List</returns>
        //IList<QuestionTypeBO> QuestionTypeList(bool isForRepeaterType);

        ///// <summary>
        ///// Function to bind the Questions base on Module
        ///// </summary>
        ///// <param name="moduleID">ModuleID</param>
        ///// <returns>0 or 1</returns>
        //bool DeleteQuestion(int questionID);

        ///// <summary>
        ///// Function to bind the Questions base on Module
        ///// </summary>
        ///// <param name="questionID">The question identifier.</param>
        ///// <returns>
        ///// 0 or 1
        ///// </returns>
        //bool ActiveQuestion(int questionID);


        ///// <summary>
        ///// Function to Add the Questions base on Module
        ///// </summary>
        ///// <param name="moduleID">QuestionListBO</param>
        ///// <returns>true or false</returns>

        //bool IsInsert(QuestionListBO record, List<AnswerAttributeBO> resultAnswer);

        ///// <summary>
        ///// Function to Get the Questions Details base on QuestionID.
        ///// </summary>
        ///// <param name="questionID">questionID</param>
        ///// <returns>QuestionListBO</returns>

        //QuestionListBO QuestionDetails(int questionID);

        ///// <summary>
        ///// Function to Update/Delete the Answers Details base on AnswerID.
        ///// </summary>
        ///// <param name="SurveyQuestionAttribute">record</param>
        ///// <returns>true or false</returns>

        //bool IsUpdatedAnswer(AnswerListBO record);

        ///// <summary>
        ///// Function to Delete the Answers Details base on AnswerID.
        ///// </summary>
        ///// <param name="SurveyQuestionAttribute">record</param>
        ///// <returns>true or false</returns>

        //bool IsDeleteAnswer(int ansID);

        //#endregion

        //#endregion

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
        //IList<ProductDTO> GetProductList(int companyID, long? userID);


        ///// <summary>
        ///// Method to fetch prouduct list for APK
        ///// </summary>
        ///// <param name="companyID">company ID</param>
        ///// <returns>returns product list</returns>
        //IList<ProductDTO> GetProductList(int companyID, long? userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        ///// Added By Vinay Kanojia
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="productTypeID"></param>
        ///// <returns></returns>
        //IList<ProductGroupBO> GetProductGroup(int productTypeID);

        ///// <summary>
        ///// Method to get the pending entities which are not synced
        ///// </summary>
        ///// <param name="companyID">company primary ID</param>
        ///// <param name="userID">user primary ID</param>
        ///// <returns>returns pending item list</returns>
        //IList<SyncTableDTO> GetPendingSyncEntities(int companyID, long userID);

        ///// <summary>
        ///// Method to update user table sysnc history
        ///// </summary>
        ///// <param name="syncTableID">sync table ID</param>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns boolean status</returns>
        //bool UpdateSyncEntity(int syncTableID, long userID);

        ///// <summary>
        ///// Method to get payment mode list on the basis of company
        ///// </summary>
        ///// <param name="companyID">company ID</param>
        ///// <returns></returns>
        //IList<PaymentModeDTO> GetPaymentModes(int companyID, long? userID);


        ///// <summary>
        ///// Method to get payment mode list on the basis of company for APK
        ///// </summary>
        ///// <param name="companyID">company ID</param>
        ///// <returns></returns>
        //IList<PaymentModeDTO> GetPaymentModes(int companyID, long? userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);


        ///// <summary>
        ///// Method to Get Product Type Added By Vinay Kanojia
        ///// </summary>
        ///// <param name="companyID"> On Basis of Company</param>
        ///// <returns>List of the Product Type</returns>
        //IList<ProductGroupBO> GetProductType(int CompanyID);

        /// <summary>
        /// Function to get the Modules on the basis of role
        /// </summary>
        IList<ModuleMasterBO> GetModulesListByRole(int roleID);

        ///// <summary>
        ///// Method to get role module questions
        ///// </summary>
        ///// <param name="roleID">role id</param>
        ///// <param name="moduleID">module id</param>
        ///// <returns>rteurns list of questions</returns>
        //IList<RoleModuleQuestionBO> GetRoleModuleQuestions(int roleID, int moduleID);

        ///// <summary>
        ///// Method to submit role questions
        ///// </summary>
        ///// <param name="questions">question list</param>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns boolean response</returns>
        //bool SubmitRoleSurveyQuestions(IList<RoleModuleQuestionBO> questions, long userID);

        ///// <summary>
        ///// Method to validate coverage window
        ///// </summary>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns boolean response</returns>
        //CoverageWindowDTO IsCoverageFirstWindow(long userID, int RoleID);

        ///// <summary>
        ///// Method to sent user message on GCM server
        ///// </summary>
        ///// <param name="userID">user id</param>
        ///// <param name="message">message</param>
        ///// <param name="registrationKey">registration key</param>
        ///// <returns>returns boolean response</returns>
        //bool SendPushNotification(long userID, string message, string registrationKey);

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
        //List<AnnouncementBO> GetAnnouncement(DateTime currentDate, int? RoleID);
        ///// <summary>
        ///// Method to get announcements from date provided
        ///// </summary>
        ///// <param name="currentDate">current date</param>
        ///// <returns>returns text</returns>
        //APKMaintainanceDTO IsApkVersionUpdated(string apkVersion);
        ///// <summary>
        ///// Method to save notifications text into database
        ///// </summary>
        ///// <param name="notificationString"></param>
        ///// <param name="regionGeoDefId"></param>
        ///// <param name="profileId"></param>
        ///// <returns></returns>
        //int SaveNotification(string notificationString, int regionGeoDefId, int profileId, long userId, DateTime startDate, DateTime endDate, int frequency, string NotificationSubject);
        /// <summary>
        /// This function will validate service token by userid
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        int GetServiceTokenUserID(string apiKey, string apiToken);




    }
}
