using AccuIT.BusinessLayer.Base;
using AccuIT.BusinessLayer.Services.BO;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.PersistenceLayer.Repository.Contracts;
using AccuIT.PersistenceLayer.Repository.Entities;
#region Namespace Added for Role Master :Dhiraj 3-Dec-2013
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using AccuIT.CommonLayer.Aspects.Utilities;
using System;
using AccuIT.BusinessLayer.IC.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.CommonLayer.Aspects.ReportBO;
using AccuIT.CommonLayer.Aspects.Logging;
using System.IO;
using System.Runtime.Serialization.Json;
using AccuIT.CommonLayer.EntityMapper;
using System.Data;
#endregion

namespace AccuIT.BusinessLayer.ServiceImpl
{
    /// <summary>
    /// Business class to define system services and settings
    /// </summary>
    public class ServiceManager : ServiceBase, ISystemService
    {
        #region Properties

        /// <summary>
        /// Property to inject the user persistence layer object invocation
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(AccuIT.BusinessLayer.Base.ServiceBase.ContainerDataLayerInstanceNames.SYSTEM_REPOSITORY)]
        public ISystemRepository SystemRepository { get; set; }

        /// <summary>
        /// Property to inject the user persistence layer object invocation
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(AccuIT.BusinessLayer.Base.ServiceBase.ContainerDataLayerInstanceNames.EMP_REPOSITORY)]
        public IUserRepository UserRepository { get; set; }

        #endregion

        /// <summary>
        /// CommonSetup 
        /// </summary>
        /// <param name="MainType"></param>
        /// <param name="subtype"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public List<CommonSetupDTO> GetCommonSetup(int DisplayValue, string subtype, string parentid)
        {
            List<CommonSetupDTO> commonSetupBO = new List<CommonSetupDTO>();
            ObjectMapper.Map(SystemRepository.GetCommonSetup(DisplayValue, subtype, parentid), commonSetupBO);
            return commonSetupBO.OrderBy(x => x.DisplayText).Distinct().ToList();
        }

        public AddressMasterBO GetAddressDetails(int addressID, int? ID, int? Type)
        {
            AddressMasterBO addressBO = new AddressMasterBO();
            ObjectMapper.Map(SystemRepository.GetAddressDetails(addressID, ID, Type), addressBO);
            return addressBO;
        }
        public AddressMasterBO GetAddressDetailsByType(int typeID, int typePkID)
        {
            AddressMasterBO addressBO = new AddressMasterBO();
            ObjectMapper.Map(SystemRepository.GetAddressDetailsByType(typeID, typePkID), addressBO);
            return addressBO;
        }
        public List<int> UploadMasterDataParking2Main(DataTable dtDemoValidation, int enumMaster)
        {
            return SystemRepository.UploadMasterDataParking2Main(dtDemoValidation, enumMaster);
        }

        public TemplateMasterBO GetTemplateData(int templateID, int? code)
        {
            TemplateMasterBO template = new TemplateMasterBO();
            ObjectMapper.Map(SystemRepository.GetTemplateData(templateID, code), template);
            return template;
        }

        public List<TemplateMergeFieldBO> GetTemplateMergeFields(int templateID)
        {
            List<TemplateMergeFieldBO> fields = new List<TemplateMergeFieldBO>();
            ObjectMapper.Map(SystemRepository.GetTemplateMergeFields(templateID), fields);
            return fields;
        }

        public List<TemplateMasterBO> GetAllTemplates(int? type)
        {
            List<TemplateMasterBO> templates = new List<TemplateMasterBO>();
            ObjectMapper.Map(SystemRepository.GetAllTemplates(type), templates);
            return templates;
        }

        public List<TemplateImageBO> SubmitTemplateImages(List<TemplateImageBO> Images)
        {
            List<TemplateImage> images = new List<TemplateImage>();
            ObjectMapper.Map(Images, images);
            ObjectMapper.Map(SystemRepository.SubmitTemplateImages(images), Images);
            return Images;
        }

        public int SubmitNewOrder(OrderMasterBO OrderBO)
        {
            OrderMaster order = new OrderMaster();
            double GST = Convert.ToDouble(OrderBO.Amount) * .18;
            OrderBO.CGST = Convert.ToInt32(GST / 2);
            OrderBO.SGST = Convert.ToInt32(GST / 2);
            OrderBO.Amount = Convert.ToDecimal(OrderBO.CGST + OrderBO.SGST);
            OrderBO.ReceivedAmount = OrderBO.Amount;
            OrderBO.OrderStatus = AspectEnums.OrderStatus.Confirmed.ToString();
            ObjectMapper.Map(OrderBO, order);
            return SystemRepository.SubmitNewOrder(order);
        }
        public int UpdateOrder(OrderMasterBO OrderBO)
        {
            OrderMaster order = new OrderMaster();
            ObjectMapper.Map(OrderBO, order);
            return SystemRepository.UpdateOrder(order);
        }

        public int SubmitUserSubscription(UserWeddingSubscriptionBO Subscription)
        {
            UserWeddingSubscription subscription = new UserWeddingSubscription();
            ObjectMapper.Map(Subscription, subscription);
            return SystemRepository.SubmitUserSubscription(subscription);
        }
        public int UpdateUserSubscription(UserWeddingSubscriptionBO SubscriptionBO)
        {
            UserWeddingSubscription Subscription = new UserWeddingSubscription();
            ObjectMapper.Map(SubscriptionBO, Subscription);
            return SystemRepository.UpdateUserSubscription(Subscription);
        }

        /// <summary>
        /// Get All role data
        /// </summary>
        /// <returns></returns>
        public List<RoleMasterBO> GetRoleMasters()
        {
            List<RoleMasterBO> roleMastersBO = new List<RoleMasterBO>();
            foreach (var item in SystemRepository.GetRoleMasters())
            {
                RoleMasterBO objRoleMaster = new RoleMasterBO();
                ObjectMapper.Map(item, objRoleMaster);
                roleMastersBO.Add(objRoleMaster);
            }

            return roleMastersBO;
        }

        /// <summary>
        /// Method to validate WCF service user access 
        /// </summary>
        /// <param name="apiKey">provided API Key value</param>
        /// <param name="apiToken">provided API Token value</param>
        /// <returns>returns boolean status</returns>
        public bool IsValidServiceUser(string apiKey, string apiToken, string userID)
        {
            return SystemRepository.IsValidServiceUser(apiKey, apiToken, userID);
        }

        ///// <summary>
        ///// Method to get company system setting values
        ///// </summary>
        ///// <param name="companyID">company ID</param>
        ///// <returns>returns entity instance</returns>
        //public SystemSettingBO GetCompanySystemSettings(int companyID)
        //{
        //    SystemSettingBO settings = new SystemSettingBO();
        //    ObjectMapper.Map(SystemRepository.GetCompanySystemSettings(companyID), settings);
        //    return settings;
        //}

        ///// <summary>
        ///// Method to log error details onto server database
        ///// </summary>
        ///// <param name="errorLog">error log DTO instance</param>
        ///// <returns>returns boolean response</returns>
        //public bool WriteLog(ErrorLogDTO errorLog)
        //{
        //    ErrorLog logEntity = new ErrorLog();
        //    ObjectMapper.Map(errorLog, logEntity);
        //    return SystemRepository.WriteLog(logEntity);
        //}

        ///// <summary>
        ///// Method to get image name of uploaded image for an entity
        ///// </summary>
        ///// <param name="entityID">entity ID</param>
        ///// <param name="imageType">image type</param>
        ///// <returns>returns file name</returns>
        //public string GetEntityImageName(string entityID, AspectEnums.ImageFileTypes imageType)
        //{
        //    return SystemRepository.GetEntityImageName(entityID, imageType);
        //}

        ///// <summary>
        ///// Method to image name of uploaded image for an entity
        ///// </summary>
        ///// <param name="entityID">entity ID</param>
        /////<param name="fileName">file name to update</param>
        /////<param name="fileType">file type</param>
        ///// <returns>returns file name</returns>
        //public bool UpdateEntityImageName(string entityID, string fileName, AspectEnums.ImageFileTypes fileType)
        //{
        //    return SystemRepository.UpdateEntityImageName(entityID, fileName, fileType);
        //}

        //#region Admin Module

        ///// <summary>
        ///// Inserts the update system setting.
        ///// </summary>
        ///// <param name="systemSetting">The system setting.</param>
        ///// <returns></returns>
        //public bool InsertUpdateSystemSetting(SystemSettingBO systemSetting, int? RoleID)
        //{
        //    bool isSuccess = false;
        //    SystemSetting setting = new SystemSetting();
        //    ObjectMapper.Map(systemSetting, setting);
        //    return isSuccess = SystemRepository.InsertUpdateSystemSetting(setting, RoleID);
        //}

        ///// <summary>
        ///// Gets the system settings.
        ///// </summary>
        ///// <returns></returns>
        //public SystemSettingBO GetSystemSettings()
        //{
        //    SystemSettingBO setting = new SystemSettingBO();
        //    ObjectMapper.Map(SystemRepository.GetSystemSettings(), setting);
        //    return setting;
        //}

        //#region Methods Added for Role Master :Dhiraj 3-Dec-2013

        /// <summary>
        /// Get All role data of active role
        /// </summary>
        /// <returns></returns>
        //public List<RoleMasterBO> GetActiveRoleMasters()
        //{
        //    List<RoleMasterBO> roleMastersBO = new List<RoleMasterBO>();
        //    foreach (var item in SystemRepository.GetRoleMasters().Where(x => x.IsActive == true && x.IsDeleted == false))//To fetch only active record
        //    {
        //        RoleMasterBO objRoleMaster = new RoleMasterBO();
        //        ObjectMapper.Map(item, objRoleMaster);
        //        roleMastersBO.Add(objRoleMaster);
        //    }

        //    return roleMastersBO;
        //}

        ///// <summary>
        ///// Get All role based on user Company
        ///// </summary>
        ///// <returns></returns>
        //public List<RoleMasterBO> GetRoleMasters(int companyID)
        //{
        //    List<RoleMasterBO> roleMastersBO = new List<RoleMasterBO>();
        //    foreach (var item in SystemRepository.GetRoleMasters(companyID))
        //    {
        //        RoleMasterBO objRoleMaster = new RoleMasterBO();
        //        ObjectMapper.Map(item, objRoleMaster);
        //        roleMastersBO.Add(objRoleMaster);
        //    }

        //    return roleMastersBO;
        //}

        ///// <summary>
        ///// Get All Role Name with team and Profile Level
        ///// </summary>
        ///// <param name="companyID"></param>
        ///// <param name="profileLevel"></param>
        ///// <returns></returns>
        //public List<spGetRoleMasterBO> GetRoleMasters(int companyID, int? profileLevel)
        //{
        //    List<spGetRoleMasterBO> roleMastersBO = new List<spGetRoleMasterBO>();
        //    foreach (var item in SystemRepository.GetRoleMasters(companyID, profileLevel))
        //    {
        //        spGetRoleMasterBO objRoleMaster = new spGetRoleMasterBO();
        //        ObjectMapper.Map(item, objRoleMaster);
        //        roleMastersBO.Add(objRoleMaster);
        //    }

        //    return roleMastersBO;
        //}

        ///// <summary>
        ///// Get All role data
        ///// </summary>
        ///// <returns></returns>
        //public List<RoleMasterBO> GetRoleMasters()
        //{
        //    List<RoleMasterBO> roleMastersBO = new List<RoleMasterBO>();
        //    foreach (var item in SystemRepository.GetRoleMasters())
        //    {
        //        RoleMasterBO objRoleMaster = new RoleMasterBO();
        //        ObjectMapper.Map(item, objRoleMaster);
        //        roleMastersBO.Add(objRoleMaster);
        //    }

        //    return roleMastersBO;
        //}
        ///// <summary>
        ///// Ge tApproval Parth Master for roleId and expenseType
        ///// </summary>
        ///// <param name="roleId"></param>
        ///// <param name="expenseType"></param>
        ///// <returns></returns>
        //public List<ApproverPathMasterBO> GetApprovalParthMaster(int roleId, int expenseType)
        //{
        //    List<ApproverPathMasterBO> ApproverPathMasterBO = new List<ApproverPathMasterBO>();
        //    ObjectMapper.Map(SystemRepository.GetApprovalParthMaster(roleId, expenseType), ApproverPathMasterBO);
        //    return ApproverPathMasterBO;
        //}
        ///// <summary>
        ///// Get role data for roleid
        ///// </summary>
        ///// <returns></returns>
        //public List<RoleMasterBO> GetRoleMastersByRoleId(Int32 RoleId)
        //{
        //    List<RoleMasterBO> objRoleMaster = new List<RoleMasterBO>();
        //    ObjectMapper.Map(SystemRepository.GetRoleMastersByRoleId(RoleId), objRoleMaster);
        //    return objRoleMaster;
        //}

        /// <summary>
        /// Get All rolemoudle data for particular roleID
        /// </summary>
        /// <param name="RoleID">Particular Role ID for which data of role module needs to be fetched</param>
        /// <returns>List of RoleModule</returns>
        public List<RoleModuleBO> GetRoleModulesByRoleID(int RoleID, int? ModuleID)
        {
            List<RoleModuleBO> roleModulesBO = new List<RoleModuleBO>();
            ObjectMapper.Map(SystemRepository.GetRoleModulesByRoleID(RoleID, ModuleID), roleModulesBO);
            foreach (var item in roleModulesBO.ToList())
            {
                // RoleModuleBO objRoleModuleBO = new RoleModuleBO();
                //Get module name by module id
                item.ModuleName = SystemRepository.GetModuleNameByModuleID(item.ModuleID);
                // ObjectMapper.Map(item, objRoleModuleBO);
                // roleModulesBO.Add(objRoleModuleBO);
            }
            return roleModulesBO.OrderBy(o => o.ModuleName).ToList();
        }
        /// <summary>
        /// Get All Permissions
        /// </summary>        
        /// <returns>List of Valid permisions</returns>
        public List<PermissionBO> GetPermissions()
        {
            List<PermissionBO> permissionsBO = new List<PermissionBO>();
            foreach (var item in SystemRepository.GetPermissions())
            {
                PermissionBO objPermissionBO = new PermissionBO();
                ObjectMapper.Map(item, objPermissionBO);
                permissionsBO.Add(objPermissionBO);
            }

            return permissionsBO;
        }
        /// <summary>
        /// Get All RoleModulePermissions
        /// </summary>        
        /// <returns>List of Valid permisions</returns>
        public IList<UserRoleModulePermissionBO> GetUserRoleModulePermisions()
        {
            #region Not in Use
            //List<UserRoleModulePermissionBO> userRoleModulePermissionsBO = new List<UserRoleModulePermissionBO>();
            //foreach (var item in SystemRepository.GetUserRoleModulePermisions())
            //{
            //    UserRoleModulePermissionBO objUserRoleModulePermissionBO = new UserRoleModulePermissionBO();
            //    ObjectMapper.Map(item, objUserRoleModulePermissionBO);
            //    userRoleModulePermissionsBO.Add(objUserRoleModulePermissionBO);
            //}

            //return userRoleModulePermissionsBO;
            #endregion
            IList<UserRoleModulePermissionBO> lstpermissions = new List<UserRoleModulePermissionBO>();
            ObjectMapper.Map(SystemRepository.GetUserRoleModulePermisions(), lstpermissions);
            return lstpermissions;
        }
        /// <summary>
        /// Get All RoleModulePermissions based on RoleModules
        /// </summary>                
        /// <param name="roleModules">Collection of RoleModule</param>
        /// <returns>List of Valid permisions</returns>
        public List<UserRoleModulePermissionBO> GetUserRoleModulePermisions(List<RoleModuleBO> roleModules)
        {
            List<UserRoleModulePermissionBO> userRoleModulePermissionsBO = new List<UserRoleModulePermissionBO>();
            foreach (var item in SystemRepository.GetUserRoleModulePermisions())
            {
                UserRoleModulePermissionBO objUserRoleModulePermissionBO = new UserRoleModulePermissionBO();
                ObjectMapper.Map(item, objUserRoleModulePermissionBO);
                userRoleModulePermissionsBO.Add(objUserRoleModulePermissionBO);
            }

            var matchedData = from UserRoleModulePermissions in userRoleModulePermissionsBO
                              join roleModule in roleModules on UserRoleModulePermissions.RoleModuleID equals roleModule.RoleModuleID
                              select UserRoleModulePermissions;

            return matchedData.ToList();
        }
        /// <summary>
        /// Insert update RoleModulePermission of user
        /// </summary>        
        /// <returns>List of Valid permisions</returns>
        public bool InsertUpdateUserRoleModulePermision(List<UserRoleModulePermissionBO> userRoleModulePermissions)
        {
            bool isSuccess = false;
            foreach (UserRoleModulePermissionBO item in userRoleModulePermissions)
            {
                UserRoleModulePermission objRoleModulePermissionDB = new UserRoleModulePermission();
                ObjectMapper.Map(item, objRoleModulePermissionDB);
                isSuccess = SystemRepository.InsertUpdateUserRoleModulePermision(objRoleModulePermissionDB);
                if (!isSuccess)
                    break;
            }
            return isSuccess;
        }
        /// <summary>
        /// Insert Role Module data
        /// </summary>        
        /// <returns>Success or Failure</returns>
        public bool InsertRoleModules(List<RoleModuleBO> roleModules)
        {
            bool isSuccess = false;
            foreach (RoleModuleBO item in roleModules)
            {
                RoleModule objRoleModuleDB = new RoleModule();
                ObjectMapper.Map(item, objRoleModuleDB);
                isSuccess = SystemRepository.InsertRoleModule(objRoleModuleDB);
                if (!isSuccess)
                    break;
            }
            return isSuccess;
        }
        /// <summary>
        /// Delete Role Module data for a particular Role
        /// </summary>        
        /// <returns>Success or Failure</returns>
        public bool DeleteRoleModule(int roleID)
        {
            bool isSuccess = false;
            var roleModules = GetRoleModulesByRoleID(roleID, null);
            if (roleModules.Count == 0)
                return true;
            foreach (RoleModuleBO item in roleModules)
            {
                RoleModule objRoleModuleDB = new RoleModule();
                ObjectMapper.Map(item, objRoleModuleDB);
                isSuccess = SystemRepository.DeleteRoleModule(objRoleModuleDB);
                if (!isSuccess)
                    break;
            }
            return isSuccess;
        }


        /// <summary>
        /// Delete Role Module data for a particular Role
        /// </summary>        
        /// <returns>Success or Failure</returns>
        public bool DeleteRoleModuleByRoleModule(RoleModuleBO roleModule)
        {
            bool isSuccess = false;
            if (roleModule != null && roleModule.RoleModuleID > 0)
            {
                RoleModule objRoleModuleDB = new RoleModule();
                ObjectMapper.Map(roleModule, objRoleModuleDB);
                isSuccess = SystemRepository.DeleteRoleModule(objRoleModuleDB);
            }
            return isSuccess;
        }
        //#endregion

        //#region Added By Vinay Kanojia Dated : 17/12/2013

        ///// <summary>
        ///// Function to get the Competitor List
        ///// </summary>
        //public IList<CompetitorListBO> GetCompetitorList(int productTypeID)
        //{
        //    IList<CompetitorListBO> lstCompetitor = new List<CompetitorListBO>();
        //    ObjectMapper.Map(SystemRepository.GetCompetitorList(productTypeID), lstCompetitor);
        //    return lstCompetitor;
        //}

        ///// <summary>
        ///// Function to Get the Competitor Details base on CompetitorID.
        ///// </summary>
        ///// <param name="questionID">questionID</param>
        ///// <returns>CompetitorListBO</returns>
        //public CompetitorListBO CompetitorDetails(int competitorID)
        //{
        //    CompetitorListBO result = new CompetitorListBO();
        //    Competitor response = SystemRepository.CompetitorDetails(competitorID);
        //    ObjectMapper.Map(response, result);
        //    return result;
        //}

        ///// <summary>
        ///// Funtion to Add Competitor 
        ///// </summary>
        ///// <param name="record"></param>
        ///// <returns></returns>
        //public bool IsInsert(CompetitorListBO record)
        //{
        //    bool isSuccess = false;
        //    Competitor response = new Competitor();
        //    ObjectMapper.Map(record, response);
        //    isSuccess = SystemRepository.IsSuccessfullInsert(response);
        //    return isSuccess;
        //}



        ///// <summary>
        ///// Function to Delete Competitor
        ///// </summary>
        ///// <param name="competitorID"></param>
        ///// <returns></returns>
        //public bool DeleteCompetitor(int competitorID)
        //{
        //    bool isDeleted = false;
        //    isDeleted = SystemRepository.ISDeleteCompetitor(competitorID);
        //    return isDeleted;
        //}
        //public IList<ModuleMasterBO> GetParentModule(int ModuleID)
        //{
        //    IList<ModuleMasterBO> lstMBO = new List<ModuleMasterBO>();
        //    ObjectMapper.Map(SystemRepository.GetParentModule(ModuleID), lstMBO);
        //    return lstMBO;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IList<ModuleMasterBO> GetModuleList(bool? isMobile = null)
        {
            IList<ModuleMasterBO> lstModule = new List<ModuleMasterBO>();
            ObjectMapper.Map(SystemRepository.GetModuleList(isMobile), lstModule);
            return lstModule;
        }

        ///// <summary>
        ///// Gets the apk module list.
        ///// </summary>
        ///// <param name="isMobile">The is mobile.</param>
        ///// <returns></returns>
        ///// 
        //public IList<APKMaintainanceBO> GetApkMaintanceList()
        //{
        //    IList<APKMaintainanceBO> lstApkMaint = new List<APKMaintainanceBO>();
        //    ObjectMapper.Map(SystemRepository.GetApkMaintanceList(), lstApkMaint);
        //    return lstApkMaint;
        //}

        //#region GetAll Channel Master and Update
        ///// <summary>
        /////  Added by Tanuj (9-4-2014)
        ///// </summary>
        ///// <returns></returns>

        //public IList<ChannelMasterBO> GetChannelMaster()
        //{
        //    IList<ChannelMasterBO> lstchannel = new List<ChannelMasterBO>();
        //    ObjectMapper.Map(SystemRepository.GetChannelMaster(), lstchannel);
        //    return lstchannel;
        //}
        //public bool UpdateChannelMaster(List<int> Channels)
        //{
        //    bool IsUpdated = false;
        //    IsUpdated = SystemRepository.UpdateChannelMaster(Channels);
        //    return IsUpdated;
        //}
        //#endregion

        //public IList<ModuleMasterBO> GetAPKModuleList(bool isMobile)
        //{
        //    IList<ModuleMasterBO> lstModule = new List<ModuleMasterBO>();
        //    ObjectMapper.Map(SystemRepository.GetModuleList(isMobile), lstModule);
        //    return lstModule;
        //}
        //public IList<FeedbackCategoryMasterBO> GetFeedbackCategoryMaster()
        //{
        //    IList<FeedbackCategoryMasterBO> lstCategory = new List<FeedbackCategoryMasterBO>();
        //    ObjectMapper.Map(SystemRepository.GetFeedbackCategoryList(), lstCategory);
        //    return lstCategory;
        //}


        /// <summary>
        /// 
        /// </summary>
        /// <param name="record"></param>
        /// <returns></returns>
        public bool IsModuleInsert(ModuleMasterBO record)
        {
            bool isSuccess = false;
            ModuleMaster response = new ModuleMaster();
            ObjectMapper.Map(record, response);
            isSuccess = SystemRepository.IsModuleInsert(response);
            return isSuccess;
        }


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="record"></param>
        ///// <returns></returns>
        //public bool IsCategoryInsert(FeedbackCategoryMasterBO record)
        //{
        //    bool isSuccess = false;
        //    FeedbackCategoryMaster response = new FeedbackCategoryMaster();
        //    ObjectMapper.Map(record, response);
        //    isSuccess = SystemRepository.IsSuccessCategoryInsert(response);
        //    return isSuccess;
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="moduleID"></param>
        ///// <returns></returns>
        //public ModuleMasterBO ModuleDetail(int moduleID)
        //{
        //    ModuleMasterBO result = new ModuleMasterBO();
        //    Module response = SystemRepository.ModuleDetail(moduleID);
        //    ObjectMapper.Map(response, result);
        //    return result;
        //}



        //public APKMaintainanceBO ApkDetail(int apkid)
        //{
        //    APKMaintainanceBO result = new APKMaintainanceBO();
        //    APKMaintainance response = SystemRepository.ApkDetail(apkid);
        //    ObjectMapper.Map(response, result);
        //    return result;
        //}

        //public bool IsApkInserted(APKMaintainanceBO record)
        //{
        //    bool isSucess = false;
        //    APKMaintainance response = new APKMaintainance();
        //    ObjectMapper.Map(record, response);
        //    isSucess = SystemRepository.IsApkInserted(response);
        //    return isSucess;
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="moduleID"></param>
        ///// <returns></returns>
        //public bool DeleteModule(List<int> modules)
        //{
        //    bool isDeleted = false;
        //    isDeleted = SystemRepository.DeleteModules(modules);
        //    return isDeleted;
        //}


        ////public bool UpdateReportProfile(List<int> profiles,List<int> ISEffective)
        ////{

        ////    bool isDeleted = false;
        ////    isDeleted = SystemRepository.UpdateReportProfile(profiles, ISEffective);
        ////    return isDeleted;
        ////}

        //public bool UpdateReportProfile(List<RoleMasterBO> input)
        //{
        //    bool isSucess = false;
        //    List<RoleMaster> persistantObj = new List<RoleMaster>();
        //    ObjectMapper.Map(input, persistantObj);

        //    isSucess = SystemRepository.UpdateReportProfile(persistantObj);
        //    return isSucess;
        //}
        //public bool UpdateReportProfileForRoleId(RoleMasterBO input)
        //{
        //    bool isSucess = false;
        //    RoleMaster persistantObj = new RoleMaster();
        //    ObjectMapper.Map(input, persistantObj);
        //    isSucess = SystemRepository.UpdateReportProfileForRoleId(persistantObj);
        //    return isSucess;
        //}

        ///// <summary>
        ///// Channel Master Exclusion
        ///// </summary>
        ///// <param name="Channels"> channels</param>
        ///// <returns> bool</returns>

        //#endregion

        //#region Question Master Methods
        /// <summary>
        /// Function to get the Modules
        /// </summary>
        public IList<ModulesBO> ModulesList(bool? isMobile = null)
        {
            IList<ModulesBO> module = new List<ModulesBO>();
            ObjectMapper.Map(SystemRepository.GetModulesList(isMobile), module);
            return module;
        }
        ///// <summary>
        ///// Function to bind the Questions base on Module
        ///// </summary>
        ///// <param name="moduleID">ModuleID</param>
        ///// <returns>Question List</returns>
        //public IList<QuestionListBO> QuestionList(int moduleID)
        //{
        //    IList<QuestionListBO> lstQuestion = new List<QuestionListBO>();
        //    ObjectMapper.Map(SystemRepository.GetQuestionList(moduleID), lstQuestion);
        //    return lstQuestion;
        //}
        ///// <summary>
        ///// Function to find the Questions on search string data fro particular modulecode
        ///// </summary>
        ///// <returns>Question List</returns>
        //public IList<QuestionListBO> QuestionList(int moduleCode, string strSearchData)
        //{
        //    IList<QuestionListBO> lstQuestion = new List<QuestionListBO>();
        //    ObjectMapper.Map(SystemRepository.GetQuestionList(moduleCode, strSearchData), lstQuestion);
        //    return lstQuestion;
        //}
        ///// <summary>
        ///// Function to bind the Answer base on QuestionID
        ///// </summary>
        ///// <param name="moduleID">ModuleID</param>
        ///// <returns>Answer List</returns>
        //public IList<AnswerListBO> AnswersList(int questionID)
        //{
        //    IList<AnswerListBO> lstAnswers = new List<AnswerListBO>();
        //    ObjectMapper.Map(SystemRepository.GetAnswerList(questionID), lstAnswers);
        //    return lstAnswers;
        //}


        ///// <summary>
        ///// Function to bind the Answer base on QuestionID
        ///// </summary>
        ///// <param name="moduleID">ModuleID</param>
        ///// <returns>Answer List</returns>
        //public AnswerListBO GetAnswer(int SurveyOptionID)
        //{
        //    AnswerListBO lstAnswers = new AnswerListBO();
        //    ObjectMapper.Map(SystemRepository.GetAnswer(SurveyOptionID), lstAnswers);
        //    return lstAnswers;
        //}

        ///// <summary>
        ///// Function to bind the QuestionsType
        ///// </summary>     
        ///// <returns>Question Type List</returns>
        //public IList<QuestionTypeBO> QuestionTypeList(bool isForRepeaterType)
        //{
        //    IList<QuestionTypeBO> lstQuestionType = new List<QuestionTypeBO>();
        //    ObjectMapper.Map(SystemRepository.QuestionTypeList(isForRepeaterType), lstQuestionType);
        //    return lstQuestionType;
        //}

        ///// <summary>
        ///// Function to bind the Questions base on Module
        ///// </summary>
        ///// <param name="moduleID">ModuleID</param>
        ///// <returns>0 or 1</returns>
        //public bool DeleteQuestion(int questionID)
        //{
        //    bool isDeleted = false;
        //    isDeleted = SystemRepository.ISDeleteQuestion(questionID);
        //    return isDeleted;
        //}


        ///// <summary>
        ///// Function to bind the Questions base on Module
        ///// </summary>
        ///// <param name="questionID">The question identifier.</param>
        ///// <returns>
        ///// 0 or 1
        ///// </returns>
        //public bool ActiveQuestion(int questionID)
        //{
        //    bool isDeleted = false;
        //    isDeleted = SystemRepository.ISActiveQuestion(questionID);
        //    return isDeleted;
        //}


        ///// <summary>
        ///// Function to Add the Questions base on Module
        ///// </summary>
        ///// <param name="moduleID">QuestionListBO</param>
        ///// <returns>0 or 1</returns>

        //public bool IsInsert(QuestionListBO record, List<AnswerAttributeBO> resultAnswer)
        //{
        //    bool isSuccess = false;
        //    SurveyQuestion response = new SurveyQuestion();
        //    List<AnswerAttribute> answerResult = new List<AnswerAttribute>();

        //    ObjectMapper.Map(record, response);
        //    ObjectMapper.Map(resultAnswer, answerResult);
        //    isSuccess = SystemRepository.SaveSurveyQuestion(response, answerResult);
        //    return isSuccess;
        //}

        ///// <summary>
        ///// Function to Get the Questions Details base on QuestionID.
        ///// </summary>
        ///// <param name="questionID">questionID</param>
        ///// <returns>QuestionListBO</returns>

        //public QuestionListBO QuestionDetails(int questionID)
        //{
        //    QuestionListBO result = new QuestionListBO();
        //    SurveyQuestion response = SystemRepository.QuestionDetails(questionID);
        //    ObjectMapper.Map(response, result);
        //    return result;
        //}

        ///// <summary>
        ///// Function to Update/Delete the Answers Details base on AnswerID.
        ///// </summary>
        ///// <param name="SurveyQuestionAttribute">record</param>
        ///// <returns>true or false</returns>

        //public bool IsUpdatedAnswer(AnswerListBO record)
        //{
        //    SurveyQuestionAttribute response = new SurveyQuestionAttribute();
        //    ObjectMapper.Map(record, response);
        //    bool isSucess = SystemRepository.IsUpdatedAnswer(response);
        //    return isSucess;
        //}

        ///// <summary>
        ///// Function to Delete the Answers Details base on AnswerID.
        ///// </summary>
        ///// <param name="SurveyQuestionAttribute">record</param>
        ///// <returns>true or false</returns>

        //public bool IsDeleteAnswer(int ansID)
        //{
        //    bool isSucess = SystemRepository.IsDeleteAnswer(ansID);
        //    return isSucess;
        //}

        //#endregion

        //#endregion

        ///// <summary>
        ///// Method to fetch prouduct list
        ///// </summary>
        ///// <param name="companyID">company ID</param>
        ///// <returns>returns product list</returns>
        //public IList<ProductDTO> GetProductList(int companyID, long? userID)
        //{
        //    List<ProductDTO> products = new List<ProductDTO>();
        //    ObjectMapper.Map(SystemRepository.GetProductRepository(companyID, userID), products);
        //    return products;
        //}


        ///// <summary>
        ///// Method to fetch prouduct list
        ///// </summary>
        ///// <param name="companyID">company ID</param>
        ///// <returns>returns product list</returns>
        //public IList<ProductDTO> GetProductList(int companyID, long? userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        //{
        //    List<ProductDTO> products = new List<ProductDTO>();
        //    ObjectMapper.Map(SystemRepository.GetProductRepository(companyID, userID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out MaxModifiedDate), products);
        //    return products;
        //}

        /////Added By Vinay Kanojia Dated: 20- Dec-2013
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="productTypeID"></param>
        ///// <returns></returns>
        //public IList<ProductGroupBO> GetProductGroup(int productTypeID)
        //{
        //    List<ProductGroupBO> productGroups = new List<ProductGroupBO>();
        //    ObjectMapper.Map(SystemRepository.GetProductGroup(productTypeID), productGroups);
        //    return productGroups;
        //}

        ///// <summary>
        ///// Method to Get Product Type Added By Vinay Kanojia
        ///// </summary>
        ///// <param name="companyID"> On Basis of Company</param>
        ///// <returns>List of the Product Type</returns>
        //public IList<ProductGroupBO> GetProductType(int companyID)
        //{
        //    List<ProductGroupBO> productGroups = new List<ProductGroupBO>();
        //    ObjectMapper.Map(SystemRepository.GetProductType(companyID), productGroups);
        //    return productGroups;
        //}

        ///// <summary>
        ///// Method to get the pending entities which are not synced
        ///// </summary>
        ///// <param name="companyID">company primary ID</param>
        ///// <param name="userID">user primary ID</param>
        ///// <returns>returns pending item list</returns>
        //public IList<SyncTableDTO> GetPendingSyncEntities(int companyID, long userID)
        //{
        //    List<SyncTableDTO> entities = new List<SyncTableDTO>();
        //    ObjectMapper.Map(SystemRepository.GetPendingSyncEntities(companyID, userID), entities);
        //    return entities;
        //}

        ///// <summary>
        ///// Method to update user table sysnc history
        ///// </summary>
        ///// <param name="syncTableID">sync table ID</param>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns boolean status</returns>
        //public bool UpdateSyncEntity(int syncTableID, long userID)
        //{
        //    return SystemRepository.UpdateSyncEntity(syncTableID, userID);
        //}

        ///// <summary>
        ///// Method to get payment mode list on the basis of company
        ///// </summary>
        ///// <param name="companyID">company ID</param>
        ///// <returns></returns>
        //public IList<PaymentModeDTO> GetPaymentModes(int companyID, long? userID)
        //{
        //    List<PaymentModeDTO> paymentModes = new List<PaymentModeDTO>();
        //    ObjectMapper.Map(SystemRepository.GetPaymentModes(companyID, userID), paymentModes);
        //    return paymentModes;
        //}


        ///// <summary>
        ///// Method to get payment mode list on the basis of company for APK
        ///// </summary>
        ///// <param name="companyID">company ID</param>
        ///// <returns></returns>
        //public IList<PaymentModeDTO> GetPaymentModes(int companyID, long? userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        //{
        //    List<PaymentModeDTO> paymentModes = new List<PaymentModeDTO>();
        //    ObjectMapper.Map(SystemRepository.GetPaymentModes(companyID, userID, RowCount, StartRowIndex, LastUpdatedDate, out  HasMoreRows, out  MaxModifiedDate), paymentModes);
        //    return paymentModes;
        //}

        /// <summary>
        /// Function to get the Modules on the basis of role
        /// </summary>
        public IList<ModuleMasterBO> GetModulesListByRole(int roleID)
        {
            List<ModuleMasterBO> modules = new List<ModuleMasterBO>();
            ObjectMapper.Map(SystemRepository.GetModulesListByRole(roleID), modules);
            return modules;
        }

        ///// <summary>
        ///// Method to get role module questions
        ///// </summary>
        ///// <param name="roleID">role id</param>
        ///// <param name="moduleID">module id</param>
        ///// <returns>rteurns list of questions</returns>
        //public IList<RoleModuleQuestionBO> GetRoleModuleQuestions(int roleID, int moduleID)
        //{
        //    List<RoleModuleQuestionBO> questions = new List<RoleModuleQuestionBO>();
        //    ObjectMapper.Map(SystemRepository.GetRoleModuleQuestions(roleID, moduleID), questions);
        //    return questions;
        //}

        ///// <summary>
        ///// Method to submit role questions
        ///// </summary>
        ///// <param name="questions">question list</param>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns boolean response</returns>
        //public bool SubmitRoleSurveyQuestions(IList<RoleModuleQuestionBO> questions, long userID)
        //{
        //    List<RoleModuleQuestion> surveyQuestions = new List<RoleModuleQuestion>();
        //    ObjectMapper.Map(questions, surveyQuestions);
        //    return SystemRepository.SubmitRoleSurveyQuestions(surveyQuestions, userID);
        //}

        ///// <summary>
        ///// Method to validate coverage window
        ///// </summary>
        ///// <param name="userID">user ID</param>
        ///// <returns>returns boolean response</returns>
        //public CoverageWindowDTO IsCoverageFirstWindow(long userID, int RoleID)
        //{
        //    List<int> days = new List<int>();
        //    CoverageWindowDTO coverageWindowDTO = new CoverageWindowDTO();
        //    coverageWindowDTO.IsCurrentMonth = false;
        //    coverageWindowDTO.ExceptionFlag = false;
        //    //SystemSetting settings = SystemRepository.GetSystemSettings();
        //    BeatWindowSetting settings = SystemRepository.GetBeatWindowSettings(RoleID);
        //    if (settings != null)
        //    {
        //        if (!String.IsNullOrEmpty(settings.CoveragePlanFirstWindow))
        //        {
        //            string[] dayArray = settings.CoveragePlanFirstWindow.Trim().Split('-');
        //            if (dayArray.Length > 0)
        //            {
        //                int firstSlot = Convert.ToInt32(dayArray[0]);
        //                int secondSlot = Convert.ToInt32(dayArray[1]);

        //                for (int i = firstSlot; i <= secondSlot; i++)
        //                {
        //                    days.Add(i);
        //                }
        //                if (days.Contains(System.DateTime.Today.Day))
        //                {
        //                    coverageWindowDTO.IsCurrentMonth = true;

        //                    // newly added.
        //                    coverageWindowDTO.ExceptionFlag = true;
        //                    //return true;
        //                    return coverageWindowDTO;
        //                }
        //                if (!String.IsNullOrEmpty(settings.CoveragePlanSecondWndow))
        //                {
        //                    dayArray = settings.CoveragePlanSecondWndow.Trim().Split('-');
        //                    firstSlot = Convert.ToInt32(dayArray[0]);
        //                    secondSlot = Convert.ToInt32(dayArray[1]);
        //                    days.Clear();
        //                    for (int i = firstSlot; i <= secondSlot; i++)
        //                    {
        //                        days.Add(i);
        //                    }
        //                    int day = days[0];
        //                    if (days.Contains(System.DateTime.Today.Day))
        //                    {
        //                        //   return false;
        //                        coverageWindowDTO.IsCurrentMonth = false;
        //                        return coverageWindowDTO;
        //                    }
        //                    else
        //                    {
        //                        UserSystemSetting userSettings = UserRepository.GetUserSystemSettings(userID);
        //                        if (userSettings != null)
        //                        {
        //                            if (userSettings.IsCoverageException && userSettings.CoverageExceptionWindow.HasValue)
        //                            {
        //                                List<int> coverageDays = new List<int>();
        //                                dayArray = new string[] { userSettings.CoverageExceptionWindow.Value.Day.ToString() };
        //                                if (dayArray.Length > 0)
        //                                {
        //                                    firstSlot = Convert.ToInt32(dayArray[0]);
        //                                    if (dayArray.Length > 1)
        //                                    {
        //                                        secondSlot = Convert.ToInt32(dayArray[1]);

        //                                        for (int i = firstSlot; i <= secondSlot; i++)
        //                                        {
        //                                            coverageDays.Add(i);
        //                                        }
        //                                    }
        //                                    else
        //                                    {
        //                                        coverageDays.Add(firstSlot);
        //                                    }
        //                                }

        //                                if (coverageDays.Contains(System.DateTime.Today.Day))
        //                                {
        //                                    if (System.DateTime.Today.Day < day)
        //                                    {
        //                                        //   return true;
        //                                        coverageWindowDTO.IsCurrentMonth = true;
        //                                        coverageWindowDTO.ExceptionFlag = true;
        //                                        return coverageWindowDTO;
        //                                    }
        //                                    else
        //                                    {
        //                                        coverageWindowDTO.IsCurrentMonth = false;
        //                                        //coverageWindowDTO.ExceptionFlag = true;                                                
        //                                        coverageWindowDTO.ExceptionFlag = false;
        //                                        return coverageWindowDTO;
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    //return false;
        //    return coverageWindowDTO;
        //}

        ///// <summary>
        ///// Method to sent user message on GCM server
        ///// </summary>
        ///// <param name="userID">user id</param>
        ///// <param name="message">message</param>
        ///// <param name="registrationKey">registration key</param>
        ///// <returns>returns boolean response</returns>
        //public bool SendPushNotification(long userID, string message, string registrationKey)
        //{
        //    string deviceID = registrationKey;
        //    if (String.IsNullOrEmpty(deviceID))
        //    {
        //        UserMaster userDetails = UserRepository.DisplayUserProfile(userID);
        //        if (userDetails != null)
        //            deviceID = userDetails.AndroidRegistrationId;
        //    }
        //    if (!String.IsNullOrEmpty(deviceID))
        //    {
        //        INotification notificationInstance = AopEngine.Resolve<INotification>("Samsung_AndroiddNotificationManager");
        //        string response = notificationInstance.SendNotification(deviceID, message);
        //        if (!String.IsNullOrEmpty(response) && response.Contains("id"))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            if (response != null)
        //                LogTraceEngine.WriteLog(response);
        //        }
        //    }
        //    return false;
        //}

        ///// <summary>
        ///// Method to save announcement text into database
        ///// </summary>
        ///// <param name="startDate">start date</param>
        ///// <param name="endDate">end date</param>
        ///// <param name="announcement">announcement text to save</param>
        ///// <returns>returns int status</returns>
        //public int SaveAnnouncements(DateTime startDate, DateTime? endDate, string webAnnouncement, string mobileAnnouncement, int RoleID)
        //{
        //    return SystemRepository.SaveAnnouncements(startDate, endDate, webAnnouncement, mobileAnnouncement, RoleID);
        //}

        ///// <summary>
        ///// Method to get announcements from date provided
        ///// </summary>
        ///// <param name="currentDate">current date</param>
        ///// <returns>returns text</returns>
        //public List<AnnouncementBO> GetAnnouncement(DateTime currentDate, int? RoleID)
        //{
        //    List<AnnouncementBO> announcements = new List<AnnouncementBO>();
        //    ObjectMapper.Map(SystemRepository.GetAnnouncement(currentDate, RoleID), announcements);
        //    return announcements;
        //}
        ///// <summary>
        ///// Method to get announcements from date provided
        ///// </summary>
        ///// <param name="currentDate">current date</param>
        ///// <returns>returns text</returns>
        //public APKMaintainanceDTO IsApkVersionUpdated(string apkVersion)
        //{

        //    APKMaintainanceBO aPKmaintenanceBO = new APKMaintainanceBO();
        //    APKMaintainanceDTO aPKMaintenanceDTO = new APKMaintainanceDTO();
        //    ObjectMapper.Map(SystemRepository.IsApkVersionUpdated(apkVersion), aPKmaintenanceBO);
        //    if (aPKmaintenanceBO == null || aPKmaintenanceBO.APKID == 0 || apkVersion == aPKmaintenanceBO.APKVersion)
        //    {
        //        aPKMaintenanceDTO.APKURL = "";
        //        aPKMaintenanceDTO.IsUpdated = true;

        //    }
        //    else
        //    {
        //        //aPKMaintenanceDTO.APKURL = AppUtil.GetAPKDownloadURL(aPKmaintenanceBO.APKVersion, aPKmaintenanceBO.APKURL);
        //        aPKMaintenanceDTO.APKURL = aPKmaintenanceBO.APKURL;
        //        aPKMaintenanceDTO.IsUpdated = false;

        //    }
        //    return aPKMaintenanceDTO;

        //}
        ///// <summary>
        ///// Method to save notifications text into database
        ///// </summary>
        ///// <param name="notificationString"></param>
        ///// <param name="regionGeoDefId"></param>
        ///// <param name="profileId"></param>
        ///// <returns></returns>
        //public int SaveNotification(string notificationString, int regionGeoDefId, int profileId, long userId, DateTime startDate, DateTime endDate, int frequency, string NotificationSubject)
        //{
        //    return SystemRepository.SaveNotification(notificationString, regionGeoDefId, profileId, userId, startDate, endDate, frequency, NotificationSubject);
        //}
        /// <summary>
        /// This service will check usertoken id
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        public int GetServiceTokenUserID(string apiKey, string apiToken)
        {
            return SystemRepository.GetServiceTokenUserID(apiKey, apiToken);
        }






    }
}
