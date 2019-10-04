using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Web;
using AccuIT.BusinessLayer.Services.BO;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.CommonLayer.Aspects.Exceptions;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.CommonLayer.Aspects.Logging;
using System.Text;
using System.Data;
using System.Globalization;
using AccuIT.CommonLayer.Aspects.Security;
using AccuIT.CommonLayer.Aspects.ReportBO;
using System.Net;
using System.Configuration;
using System.Data.SqlClient;
using System.Xml.Linq;
using AccuIT.PresentationLayer.WebAdmin.Core;
using System.Runtime.Serialization.Json;
using AccuIT.CommonLayer.Aspects.Extensions;
using System.Threading.Tasks;

namespace AccuIT.PresentationLayer.WebApdmin.Ajax
{

    /// <summary>
    /// Created By     ::      Amit Mishra
    /// Created Date   ::      10 March 2015
    /// JIRA ID        ::      SDCE-2317
    /// Purpose        ::      Services for Ageing support 
    /// </summary>
    public partial class SmartDostWeb : AccuIT.PresentationLayer.WebAdmin.Controllers.BaseController, ISmartDostWeb
    {

        private string SchemeDateFormat = "dd-MMM-yyyy";
       // #region Get All Schemes
       // /*
       //  Created By     ::      Vaishali Choudhary
       //  Created Date   ::      10 March 2015
       //  JIRA ID        ::      SDCE-2317
       //  Purpose        ::      Services for Ageing support 
       //  */
       // /// <summary>
       // /// Get list of Schemes 
       // /// </summary>
       // /// <param name="SchemeID">scheme id to return based on schemeid</param>
       // /// <param name="SchemeNumber"></param>
       // /// <returns></returns>       
       // public JsonResponse<EOLSchemeDTO> GetAllEOLSchemes(int? SchemeID, string SchemeNumber)
       // {
       //     JsonResponse<EOLSchemeDTO> response = new JsonResponse<EOLSchemeDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             response.IsSuccess = true;
       //             List<EOLSchemeDTO> lstSchemes = SystemBusinessInstance.GetAllEOLSchemes(SchemeID, SchemeNumber);
       //             foreach (var k in lstSchemes)
       //             {
       //                 k.strSchemeFrom = k.SchemeFrom.ToString(SchemeDateFormat);
       //                 k.strSchemeTo = k.SchemeTo.ToString(SchemeDateFormat);
       //                 k.strOrderFrom = k.OrderFrom.ToString(SchemeDateFormat);
       //                 k.strOrderTo = k.OrderTo.ToString(SchemeDateFormat);
       //                 k.strPUMIDate = k.PUMIDate.ToString(SchemeDateFormat);
       //             }
       //             response.Result = lstSchemes;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;

       // }
       // #endregion

       // #region Get Product Type
       // /*
       // Created By     ::      Amit mishra
       // Created Date   ::      10 March 2015
       // JIRA ID        ::      SDCE-2317
       // Purpose        ::      Get list of Product Type, Group, Category
       // */
       // /// <summary>
       // /// get list of Product Type, Group, Category
       // /// </summary>
       // public JsonResponse<ProductTypeHierarchyDTO> GetProductTypeHierarchy()
       // {
       //     JsonResponse<ProductTypeHierarchyDTO> response = new JsonResponse<ProductTypeHierarchyDTO>();
       //     List<ProductTypeHierarchyDTO> objTypes = new List<ProductTypeHierarchyDTO>();

       //     List<ProductTypeBO> Types = SystemBusinessInstance.GetProductType();
       //     foreach (ProductTypeBO type in Types)
       //     {
       //         ProductTypeHierarchyDTO typeDTO = new ProductTypeHierarchyDTO();
       //         typeDTO.ProductTypeCode = type.ProductTypeCode;
       //         typeDTO.ProductTypeName = type.ProductTypeName;
       //         objTypes.Add(typeDTO);

       //     }
       //     #region Commented Code to get the hierarchy of Products
       //     //List<ProductTypeBO> Types = SystemBusinessInstance.GetProductType();
       //     //foreach (ProductTypeBO type in Types)
       //     //{
       //     //    ProductTypeHierarchyDTO typeDTO = new ProductTypeHierarchyDTO();

       //     //    List<ProductGroupsBO> Groups = SystemBusinessInstance.GetProductGroup(type.ProductTypeCode);
       //     //    foreach (ProductGroupsBO group in Groups)
       //     //    {
       //     //        ProductGroupDTO groupDTO = new ProductGroupDTO();

       //     //        List<ProductCategoryBO> Categories = SystemBusinessInstance.GetProductCategory(type.ProductTypeCode, group.ProductGroupCode);
       //     //        foreach (ProductCategoryBO category in Categories)
       //     //        {
       //     //            ProductCategoryDTO categoryDTO = new ProductCategoryDTO();
       //     //            categoryDTO.CategoryCode = category.CategoryCode;
       //     //            categoryDTO.CategoryName = category.CategoryName;
       //     //            groupDTO.ProductCategories.Add(categoryDTO);
       //     //        }
       //     //        groupDTO.ProductGroupCode = group.ProductGroupCode;
       //     //        groupDTO.ProductGroupName = group.ProductGroupName;
       //     //        typeDTO.ProductGroups.Add(groupDTO);
       //     //        //groupDTO.ProductCategories.Add(c
       //     //    }
       //     //    typeDTO.ProductTypeCode = type.ProductTypeCode;
       //     //    typeDTO.ProductTypeName = type.ProductTypeName;
       //     //    objTypes.Add(typeDTO);

       //     //}
       //     #endregion
       //     response.Result = objTypes;
       //     return response;
       // }
       // #endregion

       // #region Get Product Group
       // /*
       // Created By     ::      Amit mishra
       // Created Date   ::      10 March 2015
       // JIRA ID        ::      SDCE-2317
       // Purpose        ::      Get list of Product Group
       // */
       // /// <summary>
       // /// get list of Product Group
       // /// </summary>
       // /// <param name="ProductTypeCode"></param>
       // /// <returns></returns>
       // public JsonResponse<ProductGroupDTO> GetProductGroup(string ProductTypeCode)
       // {
       //     JsonResponse<ProductGroupDTO> response = new JsonResponse<ProductGroupDTO>();

       //     List<ProductGroupDTO> objGroups = new List<ProductGroupDTO>();
       //     List<ProductGroupsBO> Groups = SystemBusinessInstance.GetProductGroup(ProductTypeCode);
       //     foreach (ProductGroupsBO group in Groups)
       //     {
       //         objGroups.Add(new ProductGroupDTO() { ProductGroupCode = group.ProductGroupCode, ProductGroupName = group.ProductGroupName });
       //     }

       //     response.Result = objGroups;

       //     return response;
       // }
       // #endregion

       // #region Get Product Category
       // /*
       // Created By     ::      Amit mishra
       // Created Date   ::      10 March 2015
       // JIRA ID        ::      SDCE-2317
       // Purpose        ::      Get list of Product Category
       // */
       // /// <summary>
       // /// get list of Product Category
       // /// </summary>
       // /// <param name="ProductTypeCode"></param>
       // /// <returns></returns>
       // public JsonResponse<ProductCategoryDTO> GetProductCategory(string ProductTypeCode, string ProductGroupCode)
       // {
       //     JsonResponse<ProductCategoryDTO> response = new JsonResponse<ProductCategoryDTO>();

       //     List<ProductCategoryDTO> objGroups = new List<ProductCategoryDTO>();
       //     List<ProductCategoryBO> Categories = SystemBusinessInstance.GetProductCategory(ProductTypeCode, ProductGroupCode);
       //     foreach (ProductCategoryBO group in Categories)
       //     {
       //         objGroups.Add(new ProductCategoryDTO() { CategoryCode = group.CategoryCode, CategoryName = group.CategoryName });
       //     }

       //     response.Result = objGroups;

       //     return response;
       // }
       // #endregion

       // #region Save Scheme

       // /*
       //Created By     ::      Amit mishra
       //Created Date   ::      11 March 2015
       //JIRA ID        ::      SDCE-2317
       //Purpose        ::      Save Scheme
       //*/
       // /// <summary>
       // /// Save Scheme
       // /// </summary>
       // /// <param name="ProductTypeCode"></param>
       // /// <returns></returns>
       // public JsonResponse<int> EOLSaveScheme(EOLSchemeDTO scheme, byte ActionType, string PrevSchemePeriodFrom)
       // {

       //     JsonResponse<int> response = new JsonResponse<int>() { IsSuccess = false };
       //     try
       //     {
       //         DateTime Today = DateTime.Today;
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             scheme.SchemeFrom = Convert.ToDateTime(scheme.strSchemeFrom);
       //             scheme.SchemeTo = Convert.ToDateTime(scheme.strSchemeTo);
       //             scheme.OrderFrom = Convert.ToDateTime(scheme.strOrderFrom);
       //             scheme.OrderTo = Convert.ToDateTime(scheme.strOrderTo);
       //             scheme.PUMIDate = Convert.ToDateTime(scheme.strPUMIDate);
       //             DateTime PrevSchemeFrom = PrevSchemePeriodFrom.ConvertToNullableDateTime() ?? scheme.SchemeFrom;


       //             #region Date Validations

       //             bool isSuccess = true;

       //             isSuccess = scheme.SchemeFrom <= scheme.SchemeTo
       //                 && scheme.OrderFrom <= scheme.OrderTo
       //                 && scheme.SchemeFrom >= (ActionType == (byte)AspectEnums.EntityActionType.Insert ? Today : PrevSchemeFrom)
       //                 && scheme.OrderFrom >= scheme.SchemeFrom
       //                 && scheme.OrderFrom <= scheme.SchemeTo &&
       //                 scheme.OrderTo <= scheme.SchemeTo;

       //             #endregion

       //             if (isSuccess)   // if validation succeeded
       //             {
       //                 var result = SystemBusinessInstance.EOLSaveScheme(scheme, ActionType, UserID);

       //                 if (result.SchemeID > 0)
       //                 {
       //                     if (ActionType == (byte)AspectEnums.EntityActionType.Update && result.SaveStatus == true)
       //                     {
       //                         Task newtask = Task.Run(() =>
       //                         {
       //                             QueueEOLNotification(result, AspectEnums.ODScheme.Update);
       //                         });
       //                     }
       //                 }
       //                 response.IsSuccess = result.SchemeID > 0;
       //                 response.SingleResult = result.SchemeID;
       //             }
       //             else
       //                 response.IsSuccess = false;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }



       // #endregion

       // #region Get Basic Models for selected Scheme
       // /// <summary>
       // /// Get List of Basic Models for selected product
       // /// </summary>
       // /// <param name="productTypeCode"></param>
       // /// <param name="productGroupCode"></param>
       // /// <param name="CategoryCode"></param>
       // /// <returns></returns>
       // public JsonResponse<BasicModelDTO> GetBasicModels(string productTypeCode, string productGroupCode, string CategoryCode, int? SchemeID)
       // {
       //     JsonResponse<BasicModelDTO> response = new JsonResponse<BasicModelDTO>();

       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {

       //             EOLSchemeDTO objScheme = SystemBusinessInstance.GetAllEOLSchemes(SchemeID, null).FirstOrDefault();
       //             List<String> schemeBasicModels = objScheme.EOLSchemeDetails.Select(k => k.BasicModelCode).ToList();

       //             var basicModels = from m in SystemBusinessInstance.GetBasicModel(productTypeCode, productGroupCode, CategoryCode)
       //                               select new BasicModelDTO { BasicModelCode = m.BasicModelCode, BasicModelName = m.BasicModelName };
       //             // Remove all the basic models that are alreay present the list
       //             //response.Result = basicModels.Where( k => !schemeBasicModels.Any(b=>b==k.BasicModelCode) ).ToList();
       //             response.Result = basicModels.Where(k => !schemeBasicModels.Contains(k.BasicModelCode)).ToList();

       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }
       // #endregion

       // #region  Save Scheme Products

       // //AddProductsToScheme
       // /// <summary>
       // /// Save the scheme Products 
       // /// </summary>
       // /// <param name="schemeProducts"></param>
       // /// <returns></returns>
       // public JsonResponse<bool> EOLSaveSchemeProducts(List<EOLSchemeDetailDTO> schemeProducts, bool isSubmit, bool isUpdate)
       // {

       //     JsonResponse<bool> response = new JsonResponse<bool>() { SingleResult = false };

       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             EOLSchemeDTO result = new EOLSchemeDTO();
       //             result = SystemBusinessInstance.EOLSaveSchemeProducts(schemeProducts, UserID, isSubmit);
       //             response.SingleResult = result == null ? false : true;

       //             if (result != null && isSubmit == true)
       //             {
       //                 Task newtask = Task.Run(() =>
       //                 {
       //                     if (isUpdate == false)
       //                         QueueEOLNotification(result, AspectEnums.ODScheme.Create);
       //                     else
       //                         QueueEOLNotification(result, AspectEnums.ODScheme.Update);
       //                 });
       //             }

       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }
       // #endregion


       // public DataTable GetSchemeReportData(string strSchemePeriodFrom, string strSchemePeriodTo, string strOrderSubmissionFrom, string strOrderSubmissionTo, int SelectedRoleID)
       // {

       //     //int selectedRoleID = selectedRoleID; // Currenly asm will only place order for scheme
       //     List<TeamLevelBO> navigateList = null;// GetNavigationList(SelectedRoleID, false);
       //     var teamofRole = roleMaster.FirstOrDefault(x => x.RoleID == SelectedRoleID);
       //     long selectedRoleTeamID = Convert.ToInt64(teamofRole.TeamID);
       //     List<EmployeeHierarchyBO> seniors = new List<EmployeeHierarchyBO>();
       //     DateTime? schemePeriodFrom = strSchemePeriodFrom.ConvertToNullableDateTime(); // string.IsNullOrEmpty(strSchemePeriodFrom) ?(DateTime?)null: Convert.ToDateTime(strSchemePeriodFrom);
       //     DateTime? schemePeriodTo = strSchemePeriodTo.ConvertToNullableDateTime();
       //     DateTime? orderSubmissionFrom = strOrderSubmissionFrom.ConvertToNullableDateTime();
       //     DateTime? orderSubmissionTo = strOrderSubmissionTo.ConvertToNullableDateTime();
       //     List<SchemeReportBO> lstScheme = SystemBusinessInstance.GetSchemeReport(UserID, SelectedRoleID, schemePeriodFrom, schemePeriodTo, orderSubmissionFrom, orderSubmissionTo);
       //     DataTable dtData = new DataTable();

       //     dtData.Columns.Add("SCHEME No.", typeof(string));
       //     dtData.Columns.Add("PUMI No.", typeof(string));
       //     dtData.Columns.Add("PUMI DATE", typeof(string));
       //     dtData.Columns.Add("SCHEME FROM", typeof(string));
       //     dtData.Columns.Add("SCHEME TO", typeof(string));

       //     dtData.Columns.Add("BRANCH", typeof(string));
       //     #region Draw Seniors Columns
       //     foreach (var team in navigateList)
       //     {
       //         if (team.TeamID == teamofRole.TeamID)
       //             dtData.Columns.Add(team.TeamCode, typeof(string));
       //     }
       //     #endregion

       //     dtData.Columns.Add("USER TYPE", typeof(string));

       //     dtData.Columns.Add("USER CODE", typeof(string));
       //     dtData.Columns.Add("USER NAME", typeof(string));

       //     dtData.Columns.Add("ORDER PLACED ON", typeof(string));
       //     dtData.Columns.Add("PROD. TYPE", typeof(string));
       //     dtData.Columns.Add("PARTNER CODE", typeof(string));
       //     dtData.Columns.Add("PARTNER NAME", typeof(string));
       //     dtData.Columns.Add("BASIC MODEL", typeof(string));
       //     dtData.Columns.Add("QTY", typeof(string));
       //     dtData.Columns.Add("MAX SUPPORT", typeof(string));
       //     dtData.Columns.Add("SUPPORT REQUIRED", typeof(string));


       //     long currentUserId = 0;
       //     long previousUserId = 0;
       //     var geolevel = new GetUserGeoBO();
       //     foreach (var item in lstScheme)
       //     {
       //         DataRow dr = dtData.NewRow();
       //         currentUserId = item.createdBy;
       //         if (currentUserId != previousUserId)
       //         {
       //             previousUserId = currentUserId;
       //             seniors.Clear();
       //             seniors.AddRange(GetSeniors(previousUserId, selectedRoleTeamID));
       //             geolevel = userGeoMapping.FirstOrDefault(x => x.UserID == previousUserId && x.RoleID == SelectedRoleID);


       //         }
       //         if (geolevel != null)
       //         {
       //             //dr["Region"] = geolevel.Region;
       //             dr["BRANCH"] = geolevel.Branch;
       //         }
       //         else
       //         {
       //             //dr["Region"] = "";
       //             dr["BRANCH"] = "";
       //         }

       //         dr["SCHEME No."] = item.SchemeNumber;
       //         dr["PUMI No."] = item.PUMINumber;
       //         dr["PUMI DATE"] = item.PUMIDate.ToString(SchemeDateFormat);
       //         dr["SCHEME FROM"] = item.SchemeFrom.ToString(SchemeDateFormat);
       //         dr["SCHEME TO"] = item.schemeTo.ToString(SchemeDateFormat);
       //         dr["USER TYPE"] = item.UserType;
       //         dr["USER NAME"] = item.NAME;
       //         dr["USER CODE"] = item.UserCode;

       //         if (geolevel != null)
       //         {
       //             //dr["Region"] = geolevel.Region;
       //             dr["Branch"] = geolevel.Branch;
       //         }

       //         #region Draw Seniors Values
       //         foreach (var team in navigateList)
       //         {

       //             if (team.TeamID == teamofRole.TeamID)
       //             {
       //                 var seniorCurrent = seniors.Where(x => x.RoleID == team.RoleID).FirstOrDefault();
       //                 if (seniorCurrent != null)
       //                 {
       //                     dr[team.TeamCode] = seniorCurrent.FirstName;
       //                 }
       //                 else
       //                 {
       //                     dr[team.TeamCode] = "";
       //                 }
       //             }
       //         }
       //         #endregion

       //         dr["ORDER PLACED ON"] = item.OrderPlaced.ToString(SchemeDateFormat);
       //         dr["PROD. TYPE"] = item.ProductType;
       //         dr["PARTNER CODE"] = item.StoreCode;
       //         dr["PARTNER NAME"] = item.StoreName;
       //         dr["BASIC MODEL"] = item.BasicModelCode;
       //         dr["QTY"] = item.OrderQuantity;
       //         dr["MAX SUPPORT"] = item.MaxSupport;
       //         dr["SUPPORT REQUIRED"] = item.SupportRequired;

       //         dtData.Rows.Add(dr);
       //     }
       //     return dtData;


       // }
       // #region Queue OD Notifications
       // private void QueueEOLNotification(EOLSchemeDTO result, AspectEnums.ODScheme SchemeNotificationType)
       // {
       //     DateTime todayDate = System.DateTime.Now.Date;
       //     if (todayDate >= result.OrderFrom && todayDate <= result.OrderTo)
       //     {
       //         #region convert date in dd/MM/yyyy format
       //         result.strSchemeFrom = result.SchemeFrom.ToString("dd/MM/yyyy");
       //         result.strSchemeTo = result.SchemeTo.ToString("dd/MM/yyyy");
       //         result.strOrderFrom = result.OrderFrom.ToString("dd/MM/yyyy");
       //         result.strOrderTo = result.OrderTo.ToString("dd/MM/yyyy");
       //         result.strCreatedDate = result.CreatedDate.ToString("dd/MM/yyyy");
       //         result.strPUMIDate = result.PUMIDate.ToString("dd/MM/yyyy");
       //         result.strModifiedDate = result.ModifiedDate == null ? null : result.ModifiedDate.Value.ToString("dd/MM/yyyy");
       //         #endregion

       //         SystemBusinessInstance.QueueEOLNotification(result, SchemeNotificationType, UserID);
       //     }
       //     else
       //     {
       //         SystemBusinessInstance.RemoveEOLNotificationLog(result.SchemeID, null);

       //     }
       // }
       // #endregion

       // #region Added by manoranjan for Race Report
       // /// <summary>
       // /// Get Race ModelWise Report
       // /// </summary>
       // /// <param name="productGroup"></param>
       // /// <param name="strDateFrom"></param>
       // /// <param name="strDateTo"></param>
       // /// <returns></returns>
       // public DataTable GetModelWiseRaceReport(long UserID, string productGroup, string strDateFrom, string strDateTo)
       // {
       //     DataTable dtData = new DataTable();
       //     List<RaceReportModelWiseBO> list = new List<RaceReportModelWiseBO>();
       //     DateTime? DateFrom = strDateFrom.ConvertToNullableDateTime();
       //     DateTime? DateTo = strDateTo.ConvertToNullableDateTime();
            
       //     //list = "";//ReportBusinessInstance.GetModelWiseReport(UserID, productGroup, DateFrom.Value, DateTo.Value).ToList();
       //     //dtData = list.ToDataTable();
       //     dtData = DisplayShareCategoryWise(list, productGroup);
       //     return dtData;
       // }
       // /// <summary>
       // /// Generate data from List for ModelWiseRaceReport
       // /// </summary>
       // /// <param name="list"></param>
       // /// <param name="productGroup"></param>
       // /// <returns></returns>
       // private DataTable CompetitionDumpModelWise(List<RaceReportModelWiseBO> list, string productGroup)
       // {
       //     DataTable dt = new DataTable();

       //     var result = (from f in list
       //                   group f by new
       //                   {
       //                       f.auditid,
       //                       f.StoreCode,
       //                       f.AuditTime,
       //                       f.ChannelType,
       //                       f.City,
       //                       f.STATE,
       //                       f.StoreName,
       //                       f.ShipToBranch,
       //                       f.ShipToRegion,
       //                       f.StoreAddress,
       //                       f.TransType
       //                   }
       //                      );
       //     //ModelName = myGroup.GroupBy(f => f.ModelName).Select
       //     //(m => new { Sub = m.Key, TotalCounter = m.Sum(c => c.TotalCounter) })

       //     dt.Columns.Add("Audit", typeof(string));
       //     dt.Columns.Add("StoreCode", typeof(string));
       //     dt.Columns.Add("StoreName", typeof(string));
       //     dt.Columns.Add("StoreAddress", typeof(string));
       //     dt.Columns.Add("Branch", typeof(string));
       //     dt.Columns.Add("Region", typeof(string));
       //     dt.Columns.Add("City", typeof(string));
       //     dt.Columns.Add("State", typeof(string));
       //     dt.Columns.Add("ChannelType", typeof(string));
       //     dt.Columns.Add("AuditTime", typeof(string));


       //     var products = "";//ReportBusinessInstance.GetModelWiseJoin(productGroup).ToList();
       //     var brands = products.GroupBy(x => new { x.BrandID, x.BrandName });
       //     foreach (var brand in brands)
       //     {

       //         try
       //         {
       //             dt.Columns.Add("Total " + productGroup + " in " + brand.Key.BrandName, typeof(string));
       //         }
       //         catch (DuplicateNameException)
       //         {
       //         }
       //     }
       //     foreach (var prod in products)
       //     {
       //         try
       //         {
       //             dt.Columns.Add(prod.BrandName + prod.ModelName + prod.ProductSize, typeof(string));
       //         }
       //         catch (DuplicateNameException)
       //         {
       //         }
       //     }

       //     foreach (var item in result)
       //     {
       //         DataRow dr = dt.NewRow();
       //         dr["Audit"] = item.Key.auditid;
       //         dr["StoreCode"] = item.Key.StoreCode;
       //         dr["AuditTime"] = item.Key.AuditTime;
       //         dr["ChannelType"] = item.Key.ChannelType;
       //         dr["City"] = item.Key.City;
       //         dr["State"] = item.Key.STATE;
       //         dr["StoreName"] = item.Key.StoreName;
       //         dr["Branch"] = item.Key.ShipToBranch;
       //         dr["Region"] = item.Key.ShipToRegion;
       //         dr["StoreAddress"] = item.Key.StoreAddress;

       //         foreach (var brand in brands)
       //         {
       //             var responseInAudit = item.Where(x => x.BrandID == brand.Key.BrandID).Sum(x => x.ModelCounter);
       //             if (responseInAudit != null)
       //             {
       //                 dr["Total " + productGroup + " in " + brand.Key.BrandName] = responseInAudit.Value;
       //             }
       //             else
       //             {
       //                 dr["Total " + productGroup + " in " + brand.Key.BrandName] = 0;
       //             }
       //         }
       //         foreach (var prod in products)
       //         {
       //             var responseInAudit = item.FirstOrDefault(x => x.ProductID == prod.ProductID);
       //             if (responseInAudit != null)
       //             {
       //                 dr[prod.BrandName + prod.ModelName + prod.ProductSize] = responseInAudit.ModelCounter;
       //             }
       //             else
       //             {
       //                 dr[prod.BrandName + prod.ModelName + prod.ProductSize] = 0;
       //             }

       //         }
       //         dt.Rows.Add(dr);
       //     }
       //     return dt;
       // }
       // /// <summary>
       // /// Generate data from List for ModelWiseRaceReport
       // /// </summary>
       // /// <param name="list"></param>
       // /// <param name="productGroup"></param>
       // /// <returns></returns>
       // private DataTable DisplayShareCategoryWise(List<RaceReportModelWiseBO> list, string productGroup)
       // {
       //     DataTable dt = new DataTable();

       //     var result = (from f in list
       //                   group f by new
       //                   {
       //                       f.auditid,
       //                       f.StoreCode,
       //                       f.AuditTime,
       //                       f.ChannelType,
       //                       f.City,
       //                       f.STATE,
       //                       f.StoreName,
       //                       f.ShipToBranch,
       //                       f.ShipToRegion,
       //                       f.StoreAddress,
       //                       f.TransType
       //                   }
       //                      );
       //     //ModelName = myGroup.GroupBy(f => f.ModelName).Select
       //     //(m => new { Sub = m.Key, TotalCounter = m.Sum(c => c.TotalCounter) })

       //     dt.Columns.Add("Audit", typeof(string));
       //     dt.Columns.Add("StoreCode", typeof(string));
       //     dt.Columns.Add("StoreName", typeof(string));
       //     dt.Columns.Add("StoreAddress", typeof(string));
       //     dt.Columns.Add("Branch", typeof(string));
       //     dt.Columns.Add("Region", typeof(string));
       //     dt.Columns.Add("City", typeof(string));
       //     dt.Columns.Add("State", typeof(string));
       //     dt.Columns.Add("ChannelType", typeof(string));
       //     dt.Columns.Add("AuditTime", typeof(string));

       //     dt.Columns.Add("Total_" + productGroup + "_Count(including competition data)", typeof(string));

       //     var products = "";//ReportBusinessInstance.GetModelWiseJoin(productGroup).ToList();
       //     var brands = products.GroupBy(x => new { x.BrandID, x.BrandName });
       //     var categories = products.GroupBy(x => new { x.ProductCategory });
       //     foreach (var brand in brands)
       //     {

       //         try
       //         {
       //             dt.Columns.Add("Total " + brand.Key.BrandName + " " + productGroup + " Count ", typeof(string));
       //         }
       //         catch (DuplicateNameException)
       //         {
       //         }
       //         foreach (var category in categories)
       //         {
       //             try
       //             {
       //                 dt.Columns.Add(brand.Key.BrandName + " " + category.Key.ProductCategory + " Count ", typeof(string));
       //                 dt.Columns.Add(brand.Key.BrandName + " " + category.Key.ProductCategory + " Count% ", typeof(string));
       //             }
       //             catch (DuplicateNameException)
       //             {
       //             }
       //         }
       //     }
       //     //foreach (var prod in products)
       //     //{
       //     //    try
       //     //    {
       //     //        dt.Columns.Add(prod.BrandName + prod.ModelName + prod.ProductSize, typeof(string));
       //     //    }
       //     //    catch (DuplicateNameException)
       //     //    {
       //     //    }
       //     //}

       //     foreach (var item in result)
       //     {
       //         DataRow dr = dt.NewRow();
       //         dr["Audit"] = item.Key.auditid;
       //         dr["StoreCode"] = item.Key.StoreCode;
       //         dr["AuditTime"] = item.Key.AuditTime;
       //         dr["ChannelType"] = item.Key.ChannelType;
       //         dr["City"] = item.Key.City;
       //         dr["State"] = item.Key.STATE;
       //         dr["StoreName"] = item.Key.StoreName;
       //         dr["Branch"] = item.Key.ShipToBranch;
       //         dr["Region"] = item.Key.ShipToRegion;
       //         dr["StoreAddress"] = item.Key.StoreAddress;

       //         dr["Total_" + productGroup + "_Count(including competition data)"] = item.Sum(x => x.ModelCounter);

       //         foreach (var brand in brands)
       //         {

       //             var totalCountBrandWise = item.Where(x => x.BrandID == brand.Key.BrandID).Sum(x => x.ModelCounter);
       //             dr["Total " + brand.Key.BrandName + " " + productGroup + " Count "] = totalCountBrandWise;

       //             foreach (var category in categories)
       //             {
       //                 var totalCountBrandCategoryWise = item.Where(x => x.BrandID == brand.Key.BrandID && x.ProductCategory==category.Key.ProductCategory).Sum(x => x.ModelCounter);
       //                 dr[brand.Key.BrandName + " " + category.Key.ProductCategory + " Count "] = totalCountBrandCategoryWise;
       //                 if (totalCountBrandWise == 0)
       //                 {
       //                     dr[brand.Key.BrandName + " " + category.Key.ProductCategory + " Count% "] = "0";
       //                 }
       //                 else
       //                 {
       //                     var totalCountBrandCategoryWisePercentage = Math.Round((Convert.ToDecimal((totalCountBrandCategoryWise * 100)) / Convert.ToDecimal(totalCountBrandWise)),2);
       //                     dr[brand.Key.BrandName + " " + category.Key.ProductCategory + " Count% "] = totalCountBrandCategoryWisePercentage;
       //                 }
       //             }
       //         }

       //         //foreach (var brand in brands)
       //         //{
       //         //    var responseInAudit = item.Where(x => x.BrandID == brand.Key.BrandID).Sum(x => x.ModelCounter);
       //         //    if (responseInAudit != null)
       //         //    {
       //         //        dr["Total " + productGroup + " in " + brand.Key.BrandName] = responseInAudit.Value;
       //         //    }
       //         //    else
       //         //    {
       //         //        dr["Total " + productGroup + " in " + brand.Key.BrandName] = 0;
       //         //    }
       //         //}
       //         //foreach (var prod in products)
       //         //{
       //         //    var responseInAudit = item.FirstOrDefault(x => x.ProductID == prod.ProductID);
       //         //    if (responseInAudit != null)
       //         //    {
       //         //        dr[prod.BrandName + prod.ModelName + prod.ProductSize] = responseInAudit.ModelCounter;
       //         //    }
       //         //    else
       //         //    {
       //         //        dr[prod.BrandName + prod.ModelName + prod.ProductSize] = 0;
       //         //    }

       //         //}
       //         dt.Rows.Add(dr);
       //     }
       //     return dt;
       // }

       // /// <summary>
       // /// Store History Report for New VOC
       // /// </summary>
       // /// <param name="storecode"></param>
       // /// <param name="strDateFrom"></param>
       // /// <param name="strDateTo"></param>
       // /// <returns></returns>
       // public DataTable GetNewVOCReport(string storecode, string strDateFrom, string strDateTo)
       // {
       //     DataTable dt = new DataTable();
       //     List<VOCReportNewBO> lstVOCReportNewBO = new List<VOCReportNewBO>();
       //     VOCReportNewBO objVOCReportNewBO = new VOCReportNewBO();
       //     lstVOCReportNewBO = "";//ReportBusinessInstance.GetNewVOCReport(storecode, strDateFrom, strDateTo);
       //     var voc = lstVOCReportNewBO.GroupBy(x => new { Date = x.CreatedDate.Value });

       //     if (lstVOCReportNewBO.Count == 0)
       //     {
       //         objVOCReportNewBO.Response = "No Result Found ";
       //         objVOCReportNewBO.CEVOCResponseID = null;
       //         lstVOCReportNewBO.Add(objVOCReportNewBO);
       //         dt = lstVOCReportNewBO.ToDataTable();
       //     }
       //     else
       //     {
       //         int counter = 0;
       //         foreach (var item in voc)
       //         {
       //             counter++;
       //             string date = item.Key.Date.ToString("dd-MMM-yyyy hh:mm:ss");
       //             dt.Columns.Add(date + " Header", typeof(string));
       //             dt.Columns.Add(date + " Response", typeof(string));
       //             dt.Columns.Add(date + " Question", typeof(string));
       //             dt.Columns.Add(counter.ToString(), typeof(string));
       //         }
       //         var iteration = 0;
       //         foreach (var item in voc)
       //         {
       //             string date = item.Key.Date.ToString("dd-MMM-yyyy hh:mm:ss");
       //             var data = lstVOCReportNewBO.Where(x => x.CreatedDate == item.Key.Date);
       //             var rows = 0;
       //             foreach (var subItem in data)
       //             {
       //                 DataRow dr;

       //                 if (iteration == 0)
       //                 {
       //                     dr = dt.NewRow();
       //                 }
       //                 else
       //                 {
       //                     dr = dt.Rows[rows];
       //                 }

       //                 dr[date + " Header"] = subItem.Header;
       //                 dr[date + " Response"] = subItem.Response;
       //                 dr[date + " Question"] = subItem.Question;
       //                 if (iteration == 0)
       //                 {
       //                     dt.Rows.Add(dr);
       //                 }
       //                 rows++;
       //             }
       //             iteration++;
       //         }
       //         //return dt;
       //     }
       //     return dt;
       // }
       // /// <summary>
       // ///  Get New voc report( Month compare ,Percentage , Total count in Month) 
       // /// </summary>
       // /// <param name="fromMonth"></param>
       // /// <param name="toMonth"></param>
       // /// <param name="fromYear"></param>
       // /// <param name="toYear"></param>
       // /// <returns></returns>
       // public DataTable GetNewVOCReportMonthWise(int fromMonth, int toMonth, int fromYear, int toYear, string strProductCategory, string strTypeOfPartner, string strPartnerCode, string strCityTier, string strRegion, string strState, string strCity)
       // {
       //     string month1 = null;
       //     string month2 = null;
       //     int countMonthFrom = 0;
       //     int countMonthTo = 0;
       //     DataTable dtData = new DataTable();
       //     List<GetVOCReportBO> list = new List<GetVOCReportBO>();
       //     List<GetVOCTotalCountBO> lstTotalcount = new List<GetVOCTotalCountBO>();
       //     GetVOCReportBO getVOCReportBO = new GetVOCReportBO();
       //     lstTotalcount = "";//ReportBusinessInstance.GetSpGetVOCTotalCount(fromMonth, toMonth, fromYear, toYear, strProductCategory, strTypeOfPartner, strPartnerCode, strCityTier, strRegion, strState, strCity);
       //     foreach (var item in lstTotalcount)
       //     {
       //         month1 = item.Count_Month_1.ToString();
       //         month2 = item.Count_Month_2.ToString();
       //     }
       //     getVOCReportBO.Header = "Total Store Visit";
       //     getVOCReportBO.Month1 = Convert.ToInt32(month1);
       //     getVOCReportBO.Month2 = Convert.ToInt32(month2);

       //     countMonthFrom = Convert.ToInt32(month1);
       //     countMonthTo = Convert.ToInt32(month2);
       //     list = "";//ReportBusinessInstance.GetNewVOCReportMonthWise(countMonthFrom, countMonthTo, fromMonth, toMonth, fromYear, toYear, strProductCategory, strTypeOfPartner, strPartnerCode, strCityTier, strRegion, strState, strCity).ToList();
       //     if (list.Count == 0)
       //     {
       //         getVOCReportBO.Header = "No Record Found";
       //         getVOCReportBO.Month1 = null;
       //         getVOCReportBO.Month2 = null;
       //         list.Add(getVOCReportBO);
       //         dtData = list.ToDataTable();
       //     }
       //     else
       //     {
       //         list.Add(getVOCReportBO);
       //         dtData = list.ToDataTable();
       //     }

       //     return dtData;
       // }

       // /// <summary>
       // /// GetVOCOpenEndedReport 
       // /// </summary>
       // /// <param name="fromMonth"></param>
       // /// <param name="toMonth"></param>
       // /// <param name="fromYear"></param>
       // /// <param name="toYear"></param>
       // /// <param name="Question"></param>
       // /// <returns></returns>
       // public DataTable GetVOCOpenEndedReport(string productCategory, DateTime fromDate, DateTime toDate, string Question)
       // {
       //     DataTable dtData = new DataTable();
       //     List<VOCOpenEndedReportBO> list = new List<VOCOpenEndedReportBO>();
       //     VOCOpenEndedReportBO objOpenEnded = new VOCOpenEndedReportBO();
       //     list = "";//ReportBusinessInstance.GetVOCOpenEndedReport(productCategory, fromDate, toDate, Question).ToList();
       //     if (list.Count == 0)
       //     {
       //         objOpenEnded.Response = "No Record Found ";
       //         list.Add(objOpenEnded);
       //     }
       //     dtData = list.ToDataTable();
       //     return dtData;
       // }
       // #endregion
    }
}
