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
using System.Threading.Tasks;
using System.Web.Routing;

namespace AccuIT.PresentationLayer.WebApdmin.Ajax
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class SmartDostWeb : AccuIT.PresentationLayer.WebAdmin.Controllers.BaseController, ISmartDostWeb
    {

        #region Global Variables

        List<EmployeeHierarchyBO> employeeList = new List<EmployeeHierarchyBO>();
        List<RoleMasterBO> roleMaster = new List<RoleMasterBO>();
        List<GetUserGeoBO> userGeoMapping = new List<GetUserGeoBO>();
        string DateFormat = "ddMMMyyyy";
        List<int> effectiveRoleID = new List<int>();

        #endregion

        #region Service Constructor
        public SmartDostWeb()
        {
            List<EmployeeHierarchyBO> employeeList = (List<EmployeeHierarchyBO>)Session[SessionVariables.EmployeeListUnderCurrentUser];
            List<RoleMasterBO> roleMaster = HttpContext.Session[SessionVariables.RoleMasters] as List<RoleMasterBO>;
            List<GetUserGeoBO> userGeoMapping = HttpContext.Session[SessionVariables.EmployeeListGeoUnderCurrentUser] as List<GetUserGeoBO>;
       
            Object AuthSession = Session[SessionVariables.AuthToken];
            HttpCookie AuthCookie = HttpContext.Request.Cookies[CookieVariables.AuthToken];
            if (AuthSession == null || AuthCookie == null || !AuthSession.ToString().Equals(AuthCookie.Value))
            {
                //returns exception for missing Authentication Credentials
                throw new System.Security.SecurityException(AccuIT.CommonLayer.Resources.Messages.CredentialsNotFound);

            }

            if (roleMaster != null)
            {
                List<RoleMasterBO> ERD = roleMaster.Where(k => k.IsAdmin == true).ToList();

                foreach (var item in ERD)
                {
                    effectiveRoleID.Add(item.RoleID);

                }
            }
            #region To Added Session Specific Data

            if (!(Session[PageConstants.SESSION_COMPANY_ID] == null || Session[PageConstants.SESSION_USER_ID] == null))
            {
                UserProfile = (UserProfileBO)Session[PageConstants.SESSION_PROFILE_KEY];
                UserID = (int)Session[PageConstants.SESSION_USER_ID];
                CompanyID = (int)Session[PageConstants.SESSION_COMPANY_ID];
                USERRoleID = (int)UserProfile.RoleID;

            }



            #endregion
        }
        #endregion

 

        //#region Dropdown Binding for Location
        //public JsonResponse<List<DrpGeoDefinitionBO>> FillDropDownData(int roleId)
        //{
        //    JsonResponse<List<DrpGeoDefinitionBO>> response = new JsonResponse<List<DrpGeoDefinitionBO>>();
        //    var lstGeoMaster = EmpBusinessInstance.GetGeoMaster(roleId).OrderBy(x => x.Level).ToList();            
        //    List<DrpGeoDefinitionBO> lstDrpGeoDefinitionBO = new List<DrpGeoDefinitionBO>();
        //    IList<GeoDefinitionBO> lst;//= new List<GeoMasterBO>();
        //    try
        //    {
        //        for (int i = 0; i < lstGeoMaster.Count(); i++)
        //        {
        //            if (i == 0)
        //            {
        //                lst = new List<GeoDefinitionBO>();
        //                lst = UserBusinessInstance.GetGeoDefinition(lstGeoMaster[i].GeoID).OrderBy(X => X.GeoDefName).ToList();
        //            }
        //            else
        //            {
        //                lst = new List<GeoDefinitionBO>();
        //            }
        //            DrpGeoDefinitionBO itm = new DrpGeoDefinitionBO();
        //            itm.GeoId = lstGeoMaster[i].Level.Value;

        //            itm.Name = lstGeoMaster[i].Name;
        //            itm.geoDefinition = lst;
        //            if ((i + 1) < lstGeoMaster.Count)
        //            {
        //                itm.NextGeoId = lstGeoMaster[i + 1].Level.Value;
        //            }
        //            else
        //                itm.NextGeoId = -2;

        //            lstDrpGeoDefinitionBO.Add(itm);
        //        }
        //        response.IsSuccess = true;

        //        response.SingleResult = lstDrpGeoDefinitionBO;
        //    }
        //    catch (Exception ex)
        //    {

        //        response.Message = ex.Message;
        //    }

        //    return response;
        //}

        //public JsonResponse<List<GeoDefinitionBO>> FillNextDropDownData(int currentGeoDefId, int nextGeoId)
        //{

        //    JsonResponse<List<GeoDefinitionBO>> response = new JsonResponse<List<GeoDefinitionBO>>();
        //    try
        //    {
        //        List<GeoDefinitionBO> lst = new List<GeoDefinitionBO>();
        //        if (nextGeoId == 4)
        //        {
        //            lst = null;//ReportBusinessInstance.GetAllBranches().Select(x => new GeoDefinitionBO { GeoDefinitionID = x.BranchID, GeoDefName = x.Name }).OrderBy(x => x.GeoDefName).ToList();
        //        }
        //        else
        //        {
        //            lst = UserBusinessInstance.GetNextGeoDefinition(currentGeoDefId, nextGeoId).OrderBy(x => x.GeoDefName).ToList();
        //        }


        //        if (lst.Count > 0)
        //        {
        //            response.IsSuccess = true;
        //            response.SingleResult = lst;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        response.Message = ex.Message;
        //    }

        //    return response;

        //}
        //#endregion

        //#region Admin-> Downlaod Authorization / KAS Authorization
        //public bool SubmitAPKDownloadAuthorization(int roleID, string strSelectedData, int userID)
        //{
        //    bool isSuccess = false;
        //    List<DownloadMasterAuthorizationBO> APKDataAuthorization = new List<DownloadMasterAuthorizationBO>();

        //    var APKData = SystemBusinessInstance.GetDownloadDataMasterList().ToList();
        //    APKDataAuthorization = (
        //                    from APKAuthorization in SystemBusinessInstance.GetDownloadDataAuthorizationByRoleID(roleID)
        //                    join APKDataList in APKData on APKAuthorization.DownloadDataMasterID equals APKDataList.DownloadDataMasterID
        //                    select APKAuthorization
        //                    ).ToList();

        //    List<DownloadMasterAuthorizationBO> toBeAdded = new List<DownloadMasterAuthorizationBO>();
        //    List<DownloadMasterAuthorizationBO> toBeRemove = new List<DownloadMasterAuthorizationBO>();
        //    string[] NewAssigned = strSelectedData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //    //Collect Data to Insert
        //    foreach (string item in NewAssigned)
        //    {
        //        int DownloadDataMasterID = Convert.ToInt32(item);
        //        List<DownloadDataMasterBO> temp = new List<DownloadDataMasterBO>();
        //        var existingData = APKDataAuthorization.Where(s => s.DownloadDataMasterID == DownloadDataMasterID).FirstOrDefault();
        //        if (existingData == null)
        //        {
        //            toBeAdded.Add(new DownloadMasterAuthorizationBO { DownloadDataMasterID = DownloadDataMasterID, RoleID = roleID, CreatedDate = DateTime.Now, CreatedBy = userID, IsDeleted = false, IsActive = true });
        //        }
        //    }
        //    // Collect Data to delete
        //    foreach (var item in APKDataAuthorization)
        //    {
        //        var notexistingModule = NewAssigned.Where(s => s == item.DownloadDataMasterID.ToString()).FirstOrDefault();
        //        if (notexistingModule == null)
        //        {
        //            toBeRemove.Add(item);
        //        }
        //    }
        //    //Insert Records
        //    isSuccess = SystemBusinessInstance.InsertAPKDataAuthorization(toBeAdded);
        //    //Delete records
        //    if (toBeRemove.Count > 0)
        //    {
        //        foreach (var item in toBeRemove)
        //        {
        //            isSuccess = SystemBusinessInstance.DeleteAPKDataAuthorization(item);
        //            if (!isSuccess)
        //                break;
        //        }
        //    }
        //    return isSuccess;
        //}

        //public bool SubmitKASAuthorization(int roleID, string strSelectedData, int userID)
        //{
        //    bool isSuccess = false;
        //    List<KASModuleAuthorizationBO> kasModuleAuthorization = new List<KASModuleAuthorizationBO>();

        //    //var APKData = FeedbackBusinessInstance.GetKASModulesList().ToList();
        //    //kasModuleAuthorization = (
        //    //                from KASAuthorization in FeedbackBusinessInstance.GetKASAuthorizationByRoleID(roleID)
        //    //                join KASModuleList in APKData on KASAuthorization.KASModuleID equals KASModuleList.KASModuleID
        //    //                select KASAuthorization
        //    //                ).ToList();

        //    List<KASModuleAuthorizationBO> toBeAdded = new List<KASModuleAuthorizationBO>();
        //    List<KASModuleAuthorizationBO> toBeRemove = new List<KASModuleAuthorizationBO>();
        //    string[] NewAssigned = strSelectedData.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        //    //Collect Data to Insert
        //    foreach (string item in NewAssigned)
        //    {
        //        int KASModuleID = Convert.ToInt32(item);
        //        List<KASModuleBO> temp = new List<KASModuleBO>();
        //        var existingData = kasModuleAuthorization.Where(s => s.KASModuleID == KASModuleID).FirstOrDefault();
        //        if (existingData == null)
        //        {
        //            toBeAdded.Add(new KASModuleAuthorizationBO { KASModuleID = KASModuleID, RoleID = roleID, CreatedDate = DateTime.Now, CreatedBy = userID, IsDeleted = false, IsActive = true });
        //        }
        //    }
        //    // Collect Data to delete
        //    foreach (var item in kasModuleAuthorization)
        //    {
        //        var notexistingModule = NewAssigned.Where(s => s == item.KASModuleID.ToString()).FirstOrDefault();
        //        if (notexistingModule == null)
        //        {
        //            toBeRemove.Add(item);
        //        }
        //    }
        //    //Delete records
        //    if (toBeRemove.Count > 0)
        //    {
        //        isSuccess = true;//FeedbackBusinessInstance.DeleteKASAuthorization(toBeRemove);
        //    }
        //    else
        //        isSuccess = true;


        //    //Insert Records
        //    if (toBeAdded.Count > 0 && isSuccess)
        //    {
        //        isSuccess = true;// FeedbackBusinessInstance.InsertKASAuthorization(toBeAdded);
        //    }
        //    return isSuccess;

        //}

        //#endregion
       

        //#region Reports Functions
        //#region DisplayShare Dashboard
        ///// <summary>
        ///// This function will return product wise display share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <param name="strDateTo">To Date</param>
        ///// <returns>JSON data for BAR chart</returns>
        //public JsonResponse<ReportChartBO> GetCompReportSecondLevel(string strDateFrom)
        //{
        //    return CompReportSecondLevel(strDateFrom, AspectEnums.CompetitionSurveyType.DisplayShare);
        //}

        //private JsonResponse<ReportChartBO> CompReportSecondLevel(string strDateFrom, AspectEnums.CompetitionSurveyType compType)
        //{
        //    JsonResponse<ReportChartBO> response = new JsonResponse<ReportChartBO>();
        //    try
        //    {
        //        ReportChartBO objReportStackDrilChartBO = new ReportChartBO();
        //        objReportStackDrilChartBO.Title = "Product Wise Data";
        //        objReportStackDrilChartBO.SubTitle = "Data till : " + DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
        //        objReportStackDrilChartBO.ReportText = "Product Wise Report";

        //        #region For SDCE-871 By Vaishali on 07 Nov 2014
        //        string Channellst = HttpContext.Current.Session[SessionVariables.CounterShareDateChannelTypes].ToString();
        //        var displayShareData = "";// ReportBusinessInstance.SPGetDisplayCounterShareResponse((int)compType, 1, null, null, null, Channellst, UserID);
        //        #endregion
        //        List<DishCompReportBO> compReportList = new List<DishCompReportBO>();
        //        foreach (var item in displayShareData)
        //        {
        //            compReportList.Add(new DishCompReportBO { CompetitorName = item.Brand, SecondLevelColumn = item.PrdocutCategory, Value = item.Response, Stores = item.storecount });
        //        }
        //        objReportStackDrilChartBO.Data = FillCompReportData(objReportStackDrilChartBO, compReportList, 0);
        //        response.IsSuccess = true;
        //        response.SingleResult = objReportStackDrilChartBO;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}
        ///// <summary>
        ///// This function will required data for display share report
        ///// </summary>
        ///// <param name="objReportStackDrilChartBO">Chart Holder Paramater</param>
        ///// <param name="DishCompReportsBO">Collection of competitionBO</param>
        ///// <returns></returns>
        //private List<ChartDataStructure> FillCompReportData(ReportChartBO objReportStackDrilChartBO, List<DishCompReportBO> DishCompReportsBO, int level)
        //{
        //    List<ChartDataStructure> DataList = new List<ChartDataStructure>();
        //    int totalSums = 0;
        //    List<string> xAxisNames = new List<string>();
        //    xAxisNames = DishCompReportsBO.Select(x => x.SecondLevelColumn).OrderBy(x => x).Distinct().ToList();
        //    objReportStackDrilChartBO.XAxisValues = xAxisNames;
        //    foreach (var item in DishCompReportsBO.GroupBy(x => x.CompetitorName))
        //    {
        //        var totalSum = item.Sum(x => x.Value);

        //        List<double> valueList = new List<double>();
        //        List<string> StoreList = new List<string>();
        //        var orderedItem = item.OrderBy(x => x.SecondLevelColumn);
        //        foreach (var value in orderedItem)
        //        {
        //            valueList.Add(value.Value);
        //            if (level == 3)
        //            {

        //                totalSums = "";//ReportBusinessInstance.GetStoreCountLevelwise(value.SecondLevelColumn.ToString(), level, "", "");
        //                StoreList.Add(value.Stores.ToString() + " / " + totalSums);
        //            }
        //            else if (level == 4)
        //            {
        //                totalSums = "";//ReportBusinessInstance.GetStoreCountLevelwise(value.Region.ToString(), level, value.SecondLevelColumn.ToString(), "");
        //                StoreList.Add(value.Stores.ToString() + " / " + totalSums);
        //            }
        //            else if (level == 5)
        //            {
        //                totalSums = "";//ReportBusinessInstance.GetStoreCountLevelwise(value.Region.ToString(), level, value.Branch.ToString(), value.SecondLevelColumn.ToString());
        //                StoreList.Add(value.Stores.ToString() + " / " + totalSums);
        //            }
        //            else
        //            {

        //                StoreList.Add(value.Stores.ToString() + " / " + storeMasterCount);
        //            }
        //        }
        //        DataList.Add(new ChartDataStructure { Name = item.Key, Values = valueList, StoresCount = StoreList });
        //    }
        //    return DataList;
        //}
        ///// <summary>
        ///// This function will return state wise display share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <param name="strDateTo">To Date</param>
        ///// <param name="secondLevelParam">Selected Product</param>
        ///// <returns>JSON data for BAR chart</returns>
        //public JsonResponse<ReportChartBO> GetCompReportThirdLevel(string strDateFrom, string secondLevelParam)
        //{
        //    return CompReportThirdLevel(strDateFrom, secondLevelParam, AspectEnums.CompetitionSurveyType.DisplayShare);
        //}

        //private JsonResponse<ReportChartBO> CompReportThirdLevel(string strDateFrom, string secondLevelParam, AspectEnums.CompetitionSurveyType compType)
        //{
        //    JsonResponse<ReportChartBO> response = new JsonResponse<ReportChartBO>();
        //    try
        //    {
        //        ReportChartBO objReportStackDrilChartBO = new ReportChartBO();
        //        objReportStackDrilChartBO.Title = "Region Wise Data";
        //        objReportStackDrilChartBO.SubTitle = "Data till : " + DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
        //        objReportStackDrilChartBO.ReportText = "Region Wise Report";



        //        #region For SDCE-871 By Vaishali on 07 Nov 2014
        //        string Channellst = HttpContext.Current.Session[SessionVariables.CounterShareDateChannelTypes].ToString();
        //        var displayShareData = "";//ReportBusinessInstance.SPGetDisplayCounterShareResponse((int)compType, 3, secondLevelParam, null, null, Channellst, UserID);
        //        #endregion
        //        List<DishCompReportBO> compReportList = new List<DishCompReportBO>();
        //        foreach (var item in displayShareData)
        //        {
        //            compReportList.Add(new DishCompReportBO { CompetitorName = item.Brand, SecondLevelColumn = item.PrdocutCategory, Value = item.Response, Stores = item.storecount });
        //        }
        //        objReportStackDrilChartBO.Data = FillCompReportData(objReportStackDrilChartBO, compReportList, 3);
        //        response.IsSuccess = true;
        //        response.SingleResult = objReportStackDrilChartBO;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}
        ///// <summary>
        ///// This function will return city wise display share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <param name="strDateTo">To Date</param>
        ///// <param name="secondLevelParam">Selected Product</param>
        ///// <param name="thirdLevelParam">Selected Region</param>
        ///// <returns>JSON data for Bar chart</returns>
        //public JsonResponse<ReportChartBO> GetCompReportForthLevel(string strDateFrom, string secondLevelParam, string thirdLevelParam)
        //{
        //    return CompReportForthLevel(strDateFrom, secondLevelParam, thirdLevelParam, AspectEnums.CompetitionSurveyType.DisplayShare);
        //}

        //private JsonResponse<ReportChartBO> CompReportForthLevel(string strDateFrom, string secondLevelParam, string thirdLevelParam, AspectEnums.CompetitionSurveyType compType)
        //{
        //    JsonResponse<ReportChartBO> response = new JsonResponse<ReportChartBO>();
        //    try
        //    {
        //        ReportChartBO objReportStackDrilChartBO = new ReportChartBO();
        //        objReportStackDrilChartBO.Title = "Branch Wise Data";
        //        objReportStackDrilChartBO.SubTitle = "Data till : " + DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
        //        objReportStackDrilChartBO.ReportText = "Branch Wise Report";
        //        #region For SDCE-871 By Vaishali on 07 Nov 2014
        //        string Channellst = HttpContext.Current.Session[SessionVariables.CounterShareDateChannelTypes].ToString();
        //        var displayShareData = "";//ReportBusinessInstance.SPGetDisplayCounterShareResponse((int)compType, 4, secondLevelParam, thirdLevelParam, null, Channellst, UserID);
        //        #endregion
        //        List<DishCompReportBO> compReportList = new List<DishCompReportBO>();
        //        foreach (var item in displayShareData)
        //        {
        //            compReportList.Add(new DishCompReportBO { CompetitorName = item.Brand, SecondLevelColumn = item.PrdocutCategory, Region = item.region, Value = item.Response, Stores = item.storecount });
        //        }
        //        objReportStackDrilChartBO.Data = FillCompReportData(objReportStackDrilChartBO, compReportList, 4);

        //        response.IsSuccess = true;
        //        response.SingleResult = objReportStackDrilChartBO;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}
        ///// <summary>
        ///// This function will return city wise display share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <param name="strDateTo">To Date</param>
        ///// <param name="secondLevelParam">Selected Product</param>
        ///// <param name="thirdLevelParam">Selected Region</param>
        ///// <param name="forthLevelParam">Selected State</param>
        ///// <returns>JSON data for Bar chart</returns>

        ///// <summary>
        ///// This function will return partner wise display share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <param name="strDateTo">To Date</param>
        ///// <param name="secondLevelParam">Selected Product</param>
        ///// <param name="thirdLevelParam">Selected Region</param>
        ///// <param name="forthLevelParam">Selected State</param>
        ///// <param name="fifthLevelParam">Selected City</param>
        ///// <returns>JSON data for Bar chart</returns>
        //public JsonResponse<ReportChartBO> GetCompReportSixthLevel(string strDateFrom, string secondLevelParam, string thirdLevelParam, string forthLevelParam, string fifthLevelParam)
        //{
        //    return CompReportSixthLevel(strDateFrom, secondLevelParam, thirdLevelParam, forthLevelParam, fifthLevelParam, AspectEnums.CompetitionSurveyType.DisplayShare);
        //}

        //private JsonResponse<ReportChartBO> CompReportSixthLevel(string strDateFrom, string secondLevelParam, string thirdLevelParam, string forthLevelParam, string fifthLevelParam, AspectEnums.CompetitionSurveyType compType)
        //{
        //    JsonResponse<ReportChartBO> response = new JsonResponse<ReportChartBO>();
        //    try
        //    {
        //        ReportChartBO objReportStackDrilChartBO = new ReportChartBO();
        //        objReportStackDrilChartBO.Title = "Partner Wise Data";
        //        objReportStackDrilChartBO.SubTitle = "Data till : " + DateTime.Now.AddDays(-1).ToString("dd-MMM-yyyy");
        //        objReportStackDrilChartBO.ReportText = "Partner Wise Report";
        //        #region For SDCE-871 By Vaishali on 07 Nov 2014
        //        string Channellst = HttpContext.Current.Session[SessionVariables.CounterShareDateChannelTypes].ToString();
        //        var displayShareData = "";//ReportBusinessInstance.SPGetDisplayCounterShareResponse((int)compType, 5, secondLevelParam, thirdLevelParam, forthLevelParam, Channellst, UserID);
        //        #endregion
        //        List<DishCompReportBO> compReportList = new List<DishCompReportBO>();
        //        foreach (var item in displayShareData)
        //        {
        //            compReportList.Add(new DishCompReportBO { CompetitorName = item.Brand, SecondLevelColumn = item.PrdocutCategory, Value = item.Response, Region = item.region, Branch = item.branch, Stores = item.storecount });
        //        }
        //        objReportStackDrilChartBO.Data = FillCompReportData(objReportStackDrilChartBO, compReportList, 5);
        //        response.IsSuccess = true;
        //        response.SingleResult = objReportStackDrilChartBO;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}
        ///// <summary>
        ///// This function will report export excel data for display share
        ///// </summary>
        ///// <param name="streamBody">Request Stream Data</param>
        ///// <returns>Dataset containing both user details and question wise response data</returns>
        //public DataSet ExportDisplayShare(string streamBody, out long userID)
        //{
        //    int exportLevel = 0;
        //    long userId = 0;
        //    int roleId = 0;
        //    string strDateFrom = string.Empty; string secondLevelParam = string.Empty;
        //    string thirdLevelParam = string.Empty; string forthLevelParam = string.Empty; string fifthLevelParam = string.Empty;
        //    var @params = HttpUtility.ParseQueryString(streamBody);
        //    exportLevel = Convert.ToInt32(@params["exportLevel"]);
        //    strDateFrom = @params["strDateFrom"];
        //    secondLevelParam = @params["secondLevelParam"];
        //    thirdLevelParam = @params["thirdLevelParam"];
        //    forthLevelParam = @params["forthLevelParam"];
        //    fifthLevelParam = @params["fifthLevelParam"];
        //    userId = Convert.ToInt64(@params["userId"]);
        //    roleId = Convert.ToInt32(@params["roleId"]);
        //    userID = userId;



        //    #region For SDCE-871 By Vaishali on 07 Nov 2014

        //    if (exportLevel == 2)
        //    {
        //        secondLevelParam = null; thirdLevelParam = null; forthLevelParam = null;
        //    }
        //    if (exportLevel == 3)
        //    {
        //        thirdLevelParam = null; forthLevelParam = null;
        //    }
        //    else if (exportLevel == 4)
        //    {
        //        forthLevelParam = null;
        //    }
        //    string Channellst = HttpContext.Current.Session[SessionVariables.CounterShareDateChannelTypes].ToString();
        //    var CompetitionSurveyResponse = "";//ReportBusinessInstance.SPGetDisplayCounterShareResponseExport(Convert.ToInt16(AspectEnums.CompetitionSurveyType.DisplayShare), exportLevel, secondLevelParam, thirdLevelParam, forthLevelParam, Channellst, userID);
        //    #endregion


        //    var lowestProfileRoleid = (from counterResult in CompetitionSurveyResponse
        //                               join empHierarchy in employeeList on counterResult.UserID equals empHierarchy.UserID
        //                               orderby empHierarchy.ProfileLevel descending
        //                               select empHierarchy).First().RoleID;

        //    DataSet dsData = new DataSet();
        //    DataTable dtParentData = new DataTable();
        //    dtParentData.Columns.Add("Date", typeof(string));
        //    dtParentData.Columns.Add("Region", typeof(string));
        //    dtParentData.Columns.Add("Branch", typeof(string));
        //    var teamofRole = "";//roleMaster.FirstOrDefault(x => x.RoleID == (int)lowestProfileRoleid);

        //    List<TeamLevelBO> navigateList = GetNavigationList((int)lowestProfileRoleid, true);
        //    foreach (var team in navigateList)
        //    {
        //       // if (team.TeamID == teamofRole.TeamID)
        //         //   dtParentData.Columns.Add(team.TeamCode, typeof(string));
        //    }
        //    dtParentData.Columns.Add("Disty", typeof(string));
        //    dtParentData.Columns.Add("User Type", typeof(string));
        //    dtParentData.Columns.Add("User Code", typeof(string));
        //    dtParentData.Columns.Add("User Name", typeof(string));
        //    dtParentData.Columns.Add("Store Code", typeof(string));
        //    dtParentData.Columns.Add("Store Name", typeof(string));
        //    dtParentData.Columns.Add("Prd Category", typeof(string));
        //    dtParentData.Columns.Add("Brand", typeof(string));
        //    dtParentData.Columns.Add("Response", typeof(string));
        //    dtParentData.Columns.Add("Question", typeof(string));
        //    List<EmployeeHierarchyBO> seniors = new List<EmployeeHierarchyBO>();
        //    long previousUserId = 0;
        //    long currentUserId = 0;


        //    #region Commented Grouping Code
        //    //var groupedData = CompetitionSurveyResponse.GroupBy(x => new { x.SurveyDate, x.userType, x.UserID, x.userCode, x.userName, x.userMobile, x.StoreCode, x.StoreName, Region = x.ShipToRegion, Disty = x.Disty, Branch = x.ShipToBranch });
        //    //int rowCounter = 0;
        //    //foreach (var item in groupedData)
        //    //{
        //    //    foreach (var subitem in item)
        //    //    {
        //    //        currentUserId = item.Key.UserID;
        //    //        if (currentUserId != previousUserId)
        //    //        {
        //    //            previousUserId = currentUserId;
        //    //            seniors.Clear();
        //    //            seniors.AddRange(GetSeniors(previousUserId, teamofRole.TeamID.Value));
        //    //        }
        //    //        DataRow dr = dtParentData.NewRow();
        //    //        dr["Date"] = item.Key.SurveyDate.Value.ToString("dd-MMM-yyyy");
        //    //        dr["Region"] = item.Key.Region;
        //    //        dr["Branch"] = item.Key.Branch;
        //    //        dr["Disty"] = item.Key.Disty;
        //    //        dr["User Type"] = item.Key.userType;
        //    //        dr["User Code"] = item.Key.userCode;
        //    //        dr["User Name"] = item.Key.userName;
        //    //        foreach (var team in navigateList)
        //    //        {
        //    //            if (team.TeamID == teamofRole.TeamID)
        //    //            {
        //    //                var senior = seniors.Where(x => x.RoleID == team.RoleID).FirstOrDefault();
        //    //                if (senior != null)
        //    //                {
        //    //                    dr[team.TeamCode] = senior.FirstName;
        //    //                }
        //    //                else
        //    //                {
        //    //                    dr[team.TeamCode] = "";
        //    //                }
        //    //            }
        //    //        }
        //    //        dr["Store Code"] = item.Key.StoreCode;
        //    //        dr["Store Name"] = item.Key.StoreName;
        //    //        dr["Prd Category"] = subitem.prd_Category;
        //    //        dr["Brand"] = subitem.Brand;
        //    //        dr["Response"] = subitem.UserResponse;
        //    //        dr["Question"] = subitem.Question;
        //    //        dtParentData.Rows.Add(dr);
        //    //    }
        //    //    rowCounter++;
        //    //}
        //    #endregion

        //    #region New Code without grouping
        //    foreach (var item in CompetitionSurveyResponse)
        //    {
        //        currentUserId = item.UserID;
        //        if (currentUserId != previousUserId)
        //        {
        //            previousUserId = currentUserId;
        //            seniors.Clear();
        //            seniors.AddRange(GetSeniors(previousUserId, teamofRole.TeamID.Value));
        //        }
        //        DataRow dr = dtParentData.NewRow();
        //        dr["Date"] = item.SurveyDate.Value.ToString("dd-MMM-yyyy");
        //        dr["Region"] = item.ShipToRegion;
        //        dr["Branch"] = item.ShipToBranch;
        //        dr["Disty"] = item.Disty;
        //        dr["User Type"] = item.userType;
        //        dr["User Code"] = item.userCode;
        //        dr["User Name"] = item.userName;
        //        foreach (var team in navigateList)
        //        {
        //            if (team.TeamID == teamofRole.TeamID)
        //            {
        //                var senior = seniors.Where(x => x.RoleID == team.RoleID).FirstOrDefault();
        //                if (senior != null)
        //                {
        //                    dr[team.TeamCode] = senior.FirstName;
        //                }
        //                else
        //                {
        //                    dr[team.TeamCode] = "";
        //                }
        //            }
        //        }
        //        dr["Store Code"] = item.StoreCode;
        //        dr["Store Name"] = item.StoreName;
        //        dr["Prd Category"] = item.prd_Category;
        //        dr["Brand"] = item.Brand;
        //        dr["Response"] = item.UserResponse;
        //        dr["Question"] = item.Question;
        //        dtParentData.Rows.Add(dr);
        //    }
        //    #endregion

        //    dsData.Tables.Add(dtParentData);
        //    return dsData;
        //}

        //#endregion

        //#region Counter Share Dashboard

        ///// <summary>
        ///// This function will return product wise counter share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <param name="strDateTo">To Date</param>
        ///// <returns>JSON data for BAR chart</returns>
        //public JsonResponse<ReportChartBO> GetCounterShareReportSecondLevel(string strDateFrom)
        //{

        //    return CompReportSecondLevel(strDateFrom, AspectEnums.CompetitionSurveyType.CounterShare);

        //}
        ///// <summary>
        ///// This function will return state wise counter share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <param name="strDateTo">To Date</param>
        ///// <param name="secondLevelParam">Selected Product</param>
        ///// <returns>JSON data for BAR chart</returns>
        //public JsonResponse<ReportChartBO> GetCounterShareReportThirdLevel(string strDateFrom, string secondLevelParam)
        //{

        //    return CompReportThirdLevel(strDateFrom, secondLevelParam, AspectEnums.CompetitionSurveyType.CounterShare);

        //}
        ///// <summary>
        ///// This function will return state wise counter share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <param name="strDateTo">To Date</param>
        ///// <param name="secondLevelParam">Selected Product</param>
        ///// <returns>JSON data for BAR chart</returns>
        //public JsonResponse<ReportChartBO> GetCounterShareReportForthLevel(string strDateFrom, string secondLevelParam, string thirdLevelParam)
        //{

        //    return CompReportForthLevel(strDateFrom, secondLevelParam, thirdLevelParam, AspectEnums.CompetitionSurveyType.CounterShare);

        //}


        ///// <summary>
        ///// This function will return partner wise counter share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <param name="strDateTo">To Date</param>
        ///// <param name="secondLevelParam">Selected Product</param>
        ///// <param name="thirdLevelParam">Selected Region</param>
        ///// <param name="forthLevelParam">Selected State</param>
        ///// <param name="fifthLevelParam">Selected City</param>
        ///// <returns>JSON data for Bar chart</returns>
        //public JsonResponse<ReportChartBO> GetCounterShareReportSixthLevel(string strDateFrom, string secondLevelParam, string thirdLevelParam, string forthLevelParam, string fifthLevelParam)
        //{

        //    return CompReportSixthLevel(strDateFrom, secondLevelParam, thirdLevelParam, forthLevelParam, fifthLevelParam, AspectEnums.CompetitionSurveyType.CounterShare);
        //}
        ///// <summary>
        ///// This function will required data for counter share report
        ///// </summary>
        ///// <param name="objReportStackDrilChartBO">Chart Holder Paramater</param>
        ///// <param name="DishCompReportsBO">Collection of competitionBO</param>
        ///// <returns></returns>
        //private List<ChartDataStructure> FillCounterShareReportData(ReportChartBO objReportStackDrilChartBO, List<DishCompReportBO> DishCompReportsBO)
        //{
        //    List<ChartDataStructure> DataList = new List<ChartDataStructure>();
        //    List<string> xAxisNames = DishCompReportsBO.Select(x => x.SecondLevelColumn).OrderBy(x => x).Distinct().ToList();
        //    objReportStackDrilChartBO.XAxisValues = xAxisNames;
        //    foreach (var item in DishCompReportsBO.GroupBy(x => x.CompetitorName))
        //    {
        //        List<double> valueList = new List<double>();
        //        foreach (var value in item.OrderBy(x => x.SecondLevelColumn))
        //        {
        //            valueList.Add(value.Value);
        //        }
        //        DataList.Add(new ChartDataStructure { Name = item.Key, Values = valueList });
        //    }
        //    return DataList;
        //}
        ///// <summary>
        ///// This function will report export excel data for counter share
        ///// </summary>
        ///// <param name="streamBody">Request Stream Data</param>
        ///// <returns>Dataset containing both user details and question wise response data</returns>
        //public DataSet ExportCounterShare(string streamBody, out long userID)
        //{
        //    int exportLevel = 0;
        //    long userId = 0;
        //    int roleId = 0;
        //    string strDateFrom = string.Empty; string strDateTo = string.Empty; string secondLevelParam = string.Empty;
        //    string thirdLevelParam = string.Empty; string forthLevelParam = string.Empty; string fifthLevelParam = string.Empty;
        //    var @params = HttpUtility.ParseQueryString(streamBody);
        //    exportLevel = Convert.ToInt32(@params["exportLevel"]);
        //    strDateFrom = @params["strDateFrom"];
        //    strDateTo = @params["strDateTo"];
        //    secondLevelParam = @params["secondLevelParam"];
        //    thirdLevelParam = @params["thirdLevelParam"];
        //    forthLevelParam = @params["forthLevelParam"];
        //    fifthLevelParam = @params["fifthLevelParam"];
        //    userId = Convert.ToInt64(@params["userId"]);
        //    roleId = Convert.ToInt32(@params["roleId"]);
        //    userID = userId;
        //    #region Commented by vaishali for SDCE-871
        //    /*
        //    if (exportLevel == 3)
        //    {
        //        CompetitionSurveyResponse = CompetitionSurveyResponse.Where(x => x.prd_Category == secondLevelParam).ToList();
        //    }
        //    else if (exportLevel == 4)
        //    {
        //        CompetitionSurveyResponse = CompetitionSurveyResponse.Where(x => x.prd_Category == secondLevelParam && x.ShipToRegion == thirdLevelParam).ToList();
        //    }
        //    else if (exportLevel == 5)
        //    {
        //        CompetitionSurveyResponse = CompetitionSurveyResponse.Where(x => x.prd_Category == secondLevelParam && x.ShipToRegion == thirdLevelParam && x.ShipToBranch == forthLevelParam).ToList();
        //    }
        //    else if (exportLevel == 6)
        //    {
        //        CompetitionSurveyResponse = CompetitionSurveyResponse.Where(x => x.prd_Category == secondLevelParam && x.ShipToRegion == thirdLevelParam && x.ShipToBranch == forthLevelParam && x.City == fifthLevelParam).ToList();
        //    }
        //     */
        //    #endregion

        //    #region For SDCE-871 By Vaishali on 07 Nov 2014

        //    if (exportLevel == 2)
        //    {
        //        secondLevelParam = null; thirdLevelParam = null; forthLevelParam = null;
        //    }
        //    if (exportLevel == 3)
        //    {
        //        thirdLevelParam = null; forthLevelParam = null;
        //    }
        //    else if (exportLevel == 4)
        //    {
        //        forthLevelParam = null;
        //    }

        //    string Channellst = HttpContext.Current.Session[SessionVariables.CounterShareDateChannelTypes].ToString();

        //    var CompetitionSurveyResponse = "";//ReportBusinessInstance.SPGetDisplayCounterShareResponseExport(Convert.ToInt16(AspectEnums.CompetitionSurveyType.CounterShare), exportLevel, secondLevelParam, thirdLevelParam, forthLevelParam, Channellst, userID);
        //    #endregion


        //    var lowestProfileRoleid = (from counterResult in CompetitionSurveyResponse
        //                               join empHierarchy in employeeList on counterResult.UserID equals empHierarchy.UserID
        //                               orderby empHierarchy.ProfileLevel descending
        //                               select empHierarchy).First().RoleID;

        //    DataSet dsData = new DataSet();

        //    DataTable dtParentData = new DataTable();
        //    var teamofRole = "";// roleMaster.FirstOrDefault(x => x.RoleID == (int)lowestProfileRoleid);

        //    dtParentData.Columns.Add("Date", typeof(string));
        //    dtParentData.Columns.Add("Region", typeof(string));
        //    dtParentData.Columns.Add("Branch", typeof(string));
        //    List<TeamLevelBO> navigateList = GetNavigationList((int)lowestProfileRoleid, true);
        //    foreach (var team in navigateList)
        //    {
        //        if (team.TeamID == teamofRole.TeamID)
        //        {
        //            dtParentData.Columns.Add(team.TeamCode, typeof(string));
        //        }
        //    }
        //    dtParentData.Columns.Add("Disty", typeof(string));
        //    dtParentData.Columns.Add("User Type", typeof(string));
        //    dtParentData.Columns.Add("User Code", typeof(string));
        //    dtParentData.Columns.Add("User Name", typeof(string));
        //    dtParentData.Columns.Add("Store Code", typeof(string));
        //    dtParentData.Columns.Add("Store Name", typeof(string));
        //    dtParentData.Columns.Add("Prd Category", typeof(string));
        //    dtParentData.Columns.Add("Brand", typeof(string));
        //    dtParentData.Columns.Add("Response", typeof(string));
        //    dtParentData.Columns.Add("Question", typeof(string));
        //    List<EmployeeHierarchyBO> seniors = new List<EmployeeHierarchyBO>();
        //    long previousUserId = 0;
        //    long currentUserId = 0;
        //    //var groupedData = CompetitionSurveyResponse.GroupBy(x => new { x.SurveyDate, x.userType, x.UserID, x.userCode, x.userName, x.userMobile, x.StoreCode, x.StoreName, Region = x.ShipToRegion, Disty = x.Disty, Branch = x.ShipToBranch });
        //    //int rowCounter = 0;

        //    //foreach (var item in groupedData)
        //    //{
        //    //    foreach (var subitem in item)
        //    //    {
        //    //        currentUserId = item.Key.UserID;
        //    //        if (currentUserId != previousUserId)
        //    //        {
        //    //            previousUserId = currentUserId;
        //    //            seniors.Clear();
        //    //            seniors.AddRange(GetSeniors(previousUserId, teamofRole.TeamID.Value));
        //    //        }
        //    //        DataRow dr = dtParentData.NewRow();
        //    //        dr["Date"] = item.Key.SurveyDate.Value.ToString("dd-MMM-yyyy");
        //    //        dr["Region"] = item.Key.Region;
        //    //        dr["Branch"] = item.Key.Branch;

        //    //        dr["Disty"] = item.Key.Disty;
        //    //        dr["User Type"] = item.Key.userType;
        //    //        dr["User Code"] = item.Key.userCode;
        //    //        dr["User Name"] = item.Key.userName;
        //    //        foreach (var team in navigateList)
        //    //        {
        //    //            if (team.TeamID == teamofRole.TeamID)
        //    //            {
        //    //                var senior = seniors.Where(x => x.RoleID == team.RoleID).FirstOrDefault();
        //    //                if (senior != null)
        //    //                {
        //    //                    dr[team.TeamCode] = senior.FirstName;
        //    //                }
        //    //                else
        //    //                {
        //    //                    dr[team.TeamCode] = "";
        //    //                }
        //    //            }
        //    //        }
        //    //        dr["Store Code"] = item.Key.StoreCode;
        //    //        dr["Store Name"] = item.Key.StoreName;
        //    //        dr["Prd Category"] = subitem.prd_Category;
        //    //        dr["Brand"] = subitem.Brand;
        //    //        dr["Response"] = subitem.UserResponse;
        //    //        dr["Question"] = subitem.Question;
        //    //        dtParentData.Rows.Add(dr);
        //    //    }
        //    //    rowCounter++;
        //    //}
        //    foreach (var item in CompetitionSurveyResponse)
        //    {
        //        currentUserId = item.UserID;
        //        if (currentUserId != previousUserId)
        //        {
        //            previousUserId = currentUserId;
        //            seniors.Clear();
        //            seniors.AddRange(GetSeniors(previousUserId, teamofRole.TeamID.Value));
        //        }
        //        DataRow dr = dtParentData.NewRow();
        //        dr["Date"] = item.SurveyDate.Value.ToString("dd-MMM-yyyy");
        //        dr["Region"] = item.ShipToRegion;
        //        dr["Branch"] = item.ShipToBranch;

        //        dr["Disty"] = item.Disty;
        //        dr["User Type"] = item.userType;
        //        dr["User Code"] = item.userCode;
        //        dr["User Name"] = item.userName;
        //        foreach (var team in navigateList)
        //        {
        //            if (team.TeamID == teamofRole.TeamID)
        //            {
        //                var senior = seniors.Where(x => x.RoleID == team.RoleID).FirstOrDefault();
        //                if (senior != null)
        //                {
        //                    dr[team.TeamCode] = senior.FirstName;
        //                }
        //                else
        //                {
        //                    dr[team.TeamCode] = "";
        //                }
        //            }
        //        }
        //        dr["Store Code"] = item.StoreCode;
        //        dr["Store Name"] = item.StoreName;
        //        dr["Prd Category"] = item.prd_Category;
        //        dr["Brand"] = item.Brand;
        //        dr["Response"] = item.UserResponse;
        //        dr["Question"] = item.Question;
        //        dtParentData.Rows.Add(dr);
        //    }
        //    dsData.Tables.Add(dtParentData);
        //    return dsData;
        //}
        //#endregion

        //#region Attendence Report
        ///// <summary>
        ///// First level Attendance Report
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <param name="roleID"></param>
        ///// <param name="strDateFrom"></param>
        ///// <param name="strDateTo"></param>
        ///// <param name="teamID"></param>
        ///// <param name="selectedRoleID"></param>
        ///// <returns></returns>
        //public JsonResponse<ReportChartBO> GetAttendenceReportFirstLevelNew(int userID, int roleID, string strDateFrom, string strDateTo, string teamID, int selectedRoleID)
        //{
        //    JsonResponse<ReportChartBO> response = new JsonResponse<ReportChartBO>();
        //    try
        //    {
        //        ExceptionEngine.AppExceptionManager.Process(() =>
        //        {
        //            ReportChartBO barChartBO = new ReportChartBO();
        //            barChartBO.Title = "Profile Wise Summary";
        //            barChartBO.SubTitle = "From : " + Convert.ToDateTime(strDateFrom).ToString("dd-MMM-yyyy") + " To : " + Convert.ToDateTime(strDateTo).ToString("dd-MMM-yyyy");
        //            barChartBO.ReportText = "Attendance %";
        //            barChartBO.IsLastLevel = false;
        //            // var distinctRoles = employeeList.GroupBy(x => new { x.RoleID, x.RoleCode });
        //            List<ChartDataStructure> DataList = new List<ChartDataStructure>();
        //            #region Effective Role ID Check
        //            //Created by Dhiraj on 26-Mar-2015 SDCE-2679
        //            var effectiveRolesEmployeeList = from emp in employeeList
        //                                             join role in effectiveRoleID
        //                                             on emp.RoleID equals role
        //                                             select emp;
        //            var distinctRoles = effectiveRolesEmployeeList.GroupBy(x => new { x.RoleID, x.RoleCode }).ToList();
        //            #endregion

        //            //foreach (var users in distinctRoles)
        //            Parallel.ForEach(distinctRoles, users =>
        //            {
        //                double Percentage = 0;
        //                Percentage = "";//ReportBusinessInstance.GetAttendancePercentage(Convert.ToDateTime(strDateFrom), Convert.ToDateTime(strDateTo), userID, users.Key.RoleID.Value);
        //                DataList.Add(new ChartDataStructure { Name = users.Key.RoleCode, Value = Percentage, UserId = users.Key.RoleID.Value });

        //            });
        //            barChartBO.Data = DataList.OrderByDescending(x => x.Value).ToList();
        //            response.IsSuccess = true;
        //            response.SingleResult = barChartBO;
        //        }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}
        ///// <summary>
        ///// OTher Level Attendance Report
        ///// </summary>
        ///// <param name="userID"></param>
        ///// <param name="roleID"></param>
        ///// <param name="strDateFrom"></param>
        ///// <param name="strDateTo"></param>
        ///// <param name="reportLevel"></param>
        ///// <param name="teamID"></param>
        ///// <param name="selectedRoleID"></param>
        ///// <returns></returns>
        //public JsonResponse<ReportChartBO> GetAttendenceReportOtherLevelNew(int userID, int roleID, string strDateFrom, string strDateTo, int reportLevel, string teamID, int selectedRoleID)
        //{
        //    JsonResponse<ReportChartBO> response = new JsonResponse<ReportChartBO>();
        //    try
        //    {

        //        ReportChartBO barDrilChartBO = new ReportChartBO();
        //        barDrilChartBO.SubTitle = "From : " + Convert.ToDateTime(strDateFrom).ToString("dd-MMM-yyyy") + " To : " + Convert.ToDateTime(strDateTo).ToString("dd-MMM-yyyy");
        //        barDrilChartBO.ReportText = "Attendance %";
        //        List<ChartDataStructure> DataList = new List<ChartDataStructure>();
        //        List<int> yAxisValueList = new List<int>();
        //        var UsersUnderCurrentLevel = GetEmployeesUnderCurrentUser(userID, reportLevel, selectedRoleID);

        //        var distinctRoles = UsersUnderCurrentLevel.GroupBy(x => x.RoleCode);
        //        StringBuilder rolesUnderUsers = new StringBuilder();
        //        string seprator = distinctRoles.Count() > 1 ? "," : "";

        //        foreach (var role in distinctRoles)
        //        {

        //            rolesUnderUsers.Append(role.Key + seprator);
        //        }
        //        barDrilChartBO.Title = "Data for - " + rolesUnderUsers.ToString();
        //        //foreach (var users in UsersUnderCurrentLevel)
        //        //{

        //        Parallel.ForEach(UsersUnderCurrentLevel, users =>
        //        {
        //            double Percentage = 0;
        //            Percentage = "";//ReportBusinessInstance.GetAttendancePercentage(Convert.ToDateTime(strDateFrom), Convert.ToDateTime(strDateTo), users.UserID.Value, selectedRoleID);
        //            var geolevel = userGeoMapping.FirstOrDefault(x => x.UserID == users.UserID && x.RoleID == users.RoleID);
        //            var GeoDef = string.Empty;
        //            if (geolevel != null)
        //            {
        //                GeoDef = geolevel.GEO;
        //            }


        //            DataList.Add(new ChartDataStructure { Name = users.FirstName, Value = Percentage, UserId = (long)users.UserID, ProfilePictureFileName = AppUtil.GetServerMobileImages(users.ProfilePictureFileName, AspectEnums.ImageFileTypes.User), RoleId = users.RoleID.Value, RoleName = users.RoleCode, Mobile_Calling = EncryptionEngine.DecryptString(users.Mobile_Calling), GeoDefName = GeoDef });

        //        });

        //        barDrilChartBO.Data = DataList.OrderByDescending(x => x.Value).ToList();
        //        response.IsSuccess = true;
        //        response.SingleResult = barDrilChartBO;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;

        //}
        ///// <summary>
        ///// Gets the attendance data export.
        ///// </summary>
        ///// <param name="strDateFrom">The string date from.</param>
        ///// <param name="strDateTo">The string date to.</param>
        ///// <param name="userId">The user identifier.</param>
        ///// <param name="roleId">The role identifier.</param>
        ///// <param name="teamID">The team identifier.</param>
        ///// <param name="selectedRoleID">The selected role identifier.</param>
        ///// <returns></returns>
        //public DataTable GetAttendancedataExportNew(string strDateFrom, string strDateTo, long userId, int roleId, int? teamID, int selectedRoleID)
        //{
        //    DataTable dt = new DataTable();
        //    List<UserRoleBO> senior = new List<UserRoleBO>();
        //    dt.Columns.Add("Date", typeof(string));
        //    dt.Columns.Add("Region", typeof(string));
        //    dt.Columns.Add("Branch", typeof(string));
        //    var teamofRole = roleMaster.FirstOrDefault(x => x.RoleID == selectedRoleID);
        //    long selectedRoleTeamID = teamofRole.TeamID.Value;
        //    List<TeamLevelBO> navigateList = GetNavigationList(selectedRoleID, false);
        //    foreach (var team in navigateList)
        //    {
        //        if (team.TeamID == selectedRoleTeamID)
        //            dt.Columns.Add(team.TeamCode, typeof(string));
        //    }
        //    if (selectedRoleID == (int)AspectEnums.Roles.FOS)
        //    {
        //        dt.Columns.Add("Disty", typeof(string));
        //    }
        //    dt.Columns.Add("User Type", typeof(string));
        //    dt.Columns.Add("User Code", typeof(string));
        //    dt.Columns.Add("User Name", typeof(string));
        //    dt.Columns.Add("Login Time", typeof(string));
        //    dt.Columns.Add("Atten. Time", typeof(string));
        //    dt.Columns.Add("Atten. Type", typeof(string));
        //    List<AttendanceExportBO> lstAttendance = "";//ReportBusinessInstance.GetAttendanceExport(Convert.ToDateTime(strDateFrom), Convert.ToDateTime(strDateTo), userId, selectedRoleID);
        //    var attendanceData = lstAttendance.OrderBy(x => x.UserID).ThenBy(x => x.Date).ToList();
        //    long currentUserId = 0;
        //    long previousUserId = 0;
        //    List<EmployeeHierarchyBO> seniors = new List<EmployeeHierarchyBO>();
        //    foreach (var item in attendanceData)
        //    {
        //        currentUserId = Convert.ToInt64(item.UserID);

        //        if (currentUserId != previousUserId)
        //        {
        //            previousUserId = currentUserId;
        //            seniors.Clear();
        //            seniors.AddRange(GetSeniors(previousUserId, selectedRoleTeamID));
        //        }
        //        var userDesignation = roleMaster.Where(k => k.RoleID == selectedRoleID).FirstOrDefault();
        //        DataRow dr = dt.NewRow();

        //        foreach (var team in navigateList)
        //        {
        //            if (team.TeamID == teamofRole.TeamID)
        //            {
        //                var seniorCurrent = seniors.Where(x => x.RoleID == team.RoleID).FirstOrDefault();
        //                if (seniorCurrent != null)
        //                {
        //                    dr[team.TeamCode] = seniorCurrent.FirstName;
        //                }
        //                else
        //                {
        //                    dr[team.TeamCode] = "";
        //                }
        //            }
        //        }
        //        dr["Date"] = Convert.ToDateTime(item.Date).ToString(DateFormat);
        //        var geolevel = userGeoMapping.FirstOrDefault(x => x.UserID == item.UserID && x.RoleID == selectedRoleID);
        //        if (geolevel != null)
        //        {
        //            dr["Region"] = geolevel.Region;
        //            dr["Branch"] = geolevel.Branch;
        //        }
        //        else
        //        {
        //            dr["Region"] = "";
        //            dr["Branch"] = "";
        //        }
        //        if (selectedRoleID == (int)AspectEnums.Roles.FOS)
        //        {

        //            dr["Disty"] = employeeList.FirstOrDefault(x => x.UserID == item.UserID).DistyName;
        //        }
        //        dr["User Type"] = userDesignation.Code == null ? "" : userDesignation.Name.ToString(CultureInfo.CurrentCulture);
        //        dr["User Code"] = item.UserCode.ToString(CultureInfo.CurrentCulture);
        //        dr["User Name"] = item.UserName.ToString(CultureInfo.CurrentCulture);
        //        dr["Login Time"] = string.IsNullOrEmpty(item.LastLoginTime.ToString()) ? "" : item.LastLoginTime.Value.ToString("hh:mm:ss");
        //        dr["Atten. Time"] = string.IsNullOrEmpty(item.attendanceDate.ToString()) ? "" : item.attendanceDate.Value.ToString("hh:mm:ss");
        //        dr["Atten. Type"] = item.Attendance;
        //        dt.Rows.Add(dr);
        //    }
        //    return dt;

        //}
        //#endregion

        //#region Coverage Report
        //#region Coverage Vs plan & Coverage vs Norm
        ///// <summary>
        ///// Gets the total coverage first level report.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="levelCount">The level count.</param>
        ///// <param name="strDateFrom">The string date from.</param>
        ///// <param name="strDateTo">The string date to.</param>
        ///// <returns></returns>
        //public JsonResponse<ReportChartBO> GetTotalCoverageFirstLevelReport(int userID, int roleID, int levelCount, string strDateFrom, string strDateTo, int selectedRoleID, string teamID)
        //{
        //    JsonResponse<ReportChartBO> response = new JsonResponse<ReportChartBO>();
        //    try
        //    {
        //        ExceptionEngine.AppExceptionManager.Process(() =>
        //        {
        //            DateTime dtFrom = Convert.ToDateTime(strDateFrom);
        //            DateTime dtTo = Convert.ToDateTime(strDateTo);
        //            ReportChartBO barChartBO = new ReportChartBO();
        //            barChartBO.Title = "Profile Wise Summary";
        //            barChartBO.SubTitle = "From : " + dtFrom.ToString("dd-MMM-yyyy") + " To : " + dtTo.ToString("dd-MMM-yyyy");
        //            barChartBO.ReportText = "Coverage vs Plan Report";
        //            barChartBO.IsLastLevel = false;
        //            int? teamId = null;
        //            if (!string.IsNullOrEmpty(teamID))
        //                teamId = Convert.ToInt32(teamID);
        //            #region Effective Role ID Check
        //            //Created by Dhiraj on 13-May-2014
        //            var effectiveRolesEmployeeList = from emp in employeeList
        //                                             join role in effectiveRoleID
        //                                             on emp.RoleID equals role
        //                                             select emp;
        //            var distinctRoles = effectiveRolesEmployeeList.GroupBy(x => new { x.RoleID, x.RoleCode }).ToList();
        //            #endregion
        //            List<ChartDataStructure> DataList = new List<ChartDataStructure>();
        //            //foreach (var users in distinctRoles)
        //            //{
        //            Parallel.ForEach(distinctRoles, users =>
        //            {

        //                Double Percentage = 0;
        //                Percentage = "";//ReportBusinessInstance.GetCoveragePercentage(Convert.ToDateTime(strDateFrom), Convert.ToDateTime(strDateTo), AspectEnums.CoverageType.CoverageVsPlan, AspectEnums.CoverageReportType.Coverage, userID, users.Key.RoleID.Value);
        //                DataList.Add(new ChartDataStructure { Name = users.Key.RoleCode, Value = Percentage, UserId = users.Key.RoleID.Value });

        //            });


        //            barChartBO.Data = DataList.OrderByDescending(x => x.Value).ToList();
        //            response.IsSuccess = true;
        //            response.SingleResult = barChartBO;
        //        }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}

        ///// <summary>
        ///// Gets the total coverage first level report.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="levelCount">The level count.</param>
        ///// <param name="strDateFrom">The string date from.</param>
        ///// <param name="strDateTo">The string date to.</param>
        ///// <returns></returns>
        //public JsonResponse<ReportChartBO> GetCoverageNormFirstLevelReport(int userID, int roleID, int levelCount, string strDateFrom, string strDateTo, int selectedRoleID, string teamID)
        //{
        //    JsonResponse<ReportChartBO> response = new JsonResponse<ReportChartBO>();
        //    try
        //    {
        //        ExceptionEngine.AppExceptionManager.Process(() =>
        //        {
        //            DateTime dtFrom = Convert.ToDateTime(strDateFrom);
        //            DateTime dtTo = Convert.ToDateTime(strDateTo);
        //            ReportChartBO barChartBO = new ReportChartBO();
        //            barChartBO.Title = "Profile Wise Summary";
        //            barChartBO.SubTitle = "From : " + dtFrom.ToString("dd-MMM-yyyy") + " To : " + dtTo.ToString("dd-MMM-yyyy");
        //            barChartBO.ReportText = "Coverage vs Norm Report";
        //            barChartBO.IsLastLevel = false;

        //            #region Effective Role ID Check
        //            //Created by Dhiraj on 13-May-2014
        //            var effectiveRolesEmployeeList = from emp in employeeList
        //                                             join role in effectiveRoleID
        //                                             on emp.RoleID equals role
        //                                             select emp;
        //            var distinctRoles = effectiveRolesEmployeeList.GroupBy(x => new { x.RoleID, x.RoleCode }).ToList();
        //            #endregion
        //            List<ChartDataStructure> DataList = new List<ChartDataStructure>();
        //            Parallel.ForEach(distinctRoles, users =>
        //            {

        //                Double Percentage = 0;
        //                Percentage = "";//ReportBusinessInstance.GetCoveragePercentage(Convert.ToDateTime(strDateFrom), Convert.ToDateTime(strDateTo), AspectEnums.CoverageType.CoverageVsNorm, AspectEnums.CoverageReportType.Coverage, userID, users.Key.RoleID.Value);
        //                DataList.Add(new ChartDataStructure { Name = users.Key.RoleCode, Value = Percentage, UserId = users.Key.RoleID.Value });
        //            });


        //            barChartBO.Data = DataList.OrderByDescending(x => x.Value).ToList();
        //            response.IsSuccess = true;
        //            response.SingleResult = barChartBO;
        //        }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}

        ///// <summary>
        ///// Gets the total coverage report other level.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="levelCount">The level count.</param>
        ///// <param name="strDateFrom">The string date from.</param>
        ///// <param name="strDateTo">The string date to.</param>
        ///// <param name="reportLevel">The report level.</param>
        ///// <returns></returns>
        //public JsonResponse<ReportChartBO> GetTotalCoverageReportOtherLevel(int userID, int roleID, int levelCount, string strDateFrom, string strDateTo, int reportLevel, string teamID, int selectedRoleID)
        //{
        //    JsonResponse<ReportChartBO> response = new JsonResponse<ReportChartBO>();
        //    try
        //    {
        //        DateTime dtFrom = Convert.ToDateTime(strDateFrom);
        //        DateTime dtTo = Convert.ToDateTime(strDateTo);
        //        int? teamId = null;
        //        if (!string.IsNullOrEmpty(teamID))
        //            teamId = Convert.ToInt32(teamID);
        //        ReportChartBO barDrilChartBO = new ReportChartBO();
        //        barDrilChartBO.SubTitle = "From : " + Convert.ToDateTime(strDateFrom).ToString("dd-MMM-yyyy") + " To : " + Convert.ToDateTime(strDateTo).ToString("dd-MMM-yyyy");
        //        barDrilChartBO.ReportText = "Coverage vs Plan Report";
        //        List<ChartDataStructure> DataList = new List<ChartDataStructure>();
        //        List<int> yAxisValueList = new List<int>();
        //        var UsersUnderCurrentLevel = GetEmployeesUnderCurrentUser(userID, reportLevel, selectedRoleID);

        //        var distinctRoles = UsersUnderCurrentLevel.GroupBy(x => x.RoleCode);

        //        StringBuilder rolesUnderUsers = new StringBuilder();
        //        string seprator = distinctRoles.Count() > 1 ? "," : "";
        //        foreach (var role in distinctRoles)
        //        {

        //            rolesUnderUsers.Append(role.Key + seprator);
        //        }
        //        barDrilChartBO.Title = "Data for - " + rolesUnderUsers.ToString();
        //        #region Get Team By Selected ROle
        //        var roles = roleMaster.FirstOrDefault(x => x.RoleID == selectedRoleID);
        //        long selectedRoleTeamID = Convert.ToInt64(roles.TeamID);
        //        #endregion
        //        //foreach (var users in UsersUnderCurrentLevel)
        //        //{
        //        Parallel.ForEach(UsersUnderCurrentLevel, users =>
        //        {
        //            var DefName = "";
        //            #region User Geo retreived By Tanuj on 27-Feb-2014
        //            var geoLevel = userGeoMapping.FirstOrDefault(x => x.UserID == users.UserID && x.RoleID == users.RoleID);
        //            if (geoLevel != null)
        //            {
        //                DefName = geoLevel.GEO;
        //            }
        //            #endregion
        //            Double Percentage = 0;
        //            Percentage = "";//ReportBusinessInstance.GetCoveragePercentage(Convert.ToDateTime(strDateFrom), Convert.ToDateTime(strDateTo), AspectEnums.CoverageType.CoverageVsPlan, AspectEnums.CoverageReportType.Coverage, users.UserID.Value, selectedRoleID);
        //            DataList.Add(new ChartDataStructure { Name = users.FirstName, Value = Percentage, UserId = (long)users.UserID, ProfilePictureFileName = AppUtil.GetServerMobileImages(users.ProfilePictureFileName, AspectEnums.ImageFileTypes.User), RoleId = users.RoleID.Value, RoleName = users.RoleCode, Mobile_Calling = EncryptionEngine.DecryptString(users.Mobile_Calling), GeoDefName = DefName });
        //        });

        //        barDrilChartBO.Data = DataList.OrderByDescending(x => x.Value).ToList();
        //        response.IsSuccess = true;
        //        response.SingleResult = barDrilChartBO;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}



        ///// <summary>
        ///// Gets the total coverage report other level.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="levelCount">The level count.</param>
        ///// <param name="strDateFrom">The string date from.</param>
        ///// <param name="strDateTo">The string date to.</param>
        ///// <param name="reportLevel">The report level.</param>
        ///// <returns></returns>
        //public JsonResponse<ReportChartBO> GetCoverageNormReportOtherLevel(int userID, int roleID, int levelCount, string strDateFrom, string strDateTo, int reportLevel, string teamID, int selectedRoleID)
        //{
        //    JsonResponse<ReportChartBO> response = new JsonResponse<ReportChartBO>();
        //    try
        //    {
        //        DateTime dtFrom = Convert.ToDateTime(strDateFrom);
        //        DateTime dtTo = Convert.ToDateTime(strDateTo);
        //        int? teamId = null;
        //        if (!string.IsNullOrEmpty(teamID))
        //            teamId = Convert.ToInt32(teamID);
        //        ReportChartBO barDrilChartBO = new ReportChartBO();
        //        barDrilChartBO.SubTitle = "From : " + dtFrom.ToString("dd-MMM-yyyy") + " To : " + dtTo.ToString("dd-MMM-yyyy");
        //        barDrilChartBO.ReportText = "Coverage vs Norm Report";
        //        List<ChartDataStructure> DataList = new List<ChartDataStructure>();
        //        List<int> yAxisValueList = new List<int>();

        //        var UsersUnderCurrentLevel = GetEmployeesUnderCurrentUser(userID, reportLevel, selectedRoleID);

        //        var distinctRoles = UsersUnderCurrentLevel.GroupBy(x => x.RoleCode);

        //        StringBuilder rolesUnderUsers = new StringBuilder();
        //        string seprator = distinctRoles.Count() > 1 ? "," : "";
        //        foreach (var role in distinctRoles)
        //        {

        //            rolesUnderUsers.Append(role.Key + seprator);
        //        }
        //        barDrilChartBO.Title = "Data for - " + rolesUnderUsers.ToString();
        //        #region Get Team By Selected ROle
        //        var roles = roleMaster.FirstOrDefault(x => x.RoleID == selectedRoleID);
        //        long selectedRoleTeamID = Convert.ToInt64(roles.TeamID);

        //        #endregion
        //        //foreach (var users in UsersUnderCurrentLevel)
        //        Parallel.ForEach(UsersUnderCurrentLevel, users =>
        //        {
        //            Double Percentage = 0;
        //            Percentage = "";//ReportBusinessInstance.GetCoveragePercentage(Convert.ToDateTime(strDateFrom), Convert.ToDateTime(strDateTo), AspectEnums.CoverageType.CoverageVsNorm, AspectEnums.CoverageReportType.Coverage, users.UserID.Value, selectedRoleID);
        //            var GeoLevel = userGeoMapping.FirstOrDefault(x => x.UserID == users.UserID && x.RoleID == users.RoleID);
        //            var GeoDef = string.Empty;
        //            if (GeoLevel != null)
        //            {
        //                GeoDef = GeoLevel.GEO;
        //            }
        //            DataList.Add(new ChartDataStructure { Name = users.FirstName, Value = Percentage, UserId = (long)users.UserID, ProfilePictureFileName = AppUtil.GetServerMobileImages(users.ProfilePictureFileName, AspectEnums.ImageFileTypes.User), RoleId = users.RoleID.Value, RoleName = EncryptionEngine.DecryptString(users.RoleCode), Mobile_Calling = EncryptionEngine.DecryptString(users.Mobile_Calling), GeoDefName = GeoDef });

        //        });
        //        barDrilChartBO.Data = DataList.OrderByDescending(x => x.Value).ToList();
        //        response.IsSuccess = true;
        //        response.SingleResult = barDrilChartBO;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}






        //private DataTable GetCoverageExport(string strDateFrom, string strDateTo, long userId, int roleId, int? teamID, int selectedRoleID)
        //{
        //    DateTime dtFrom = Convert.ToDateTime(strDateFrom);
        //    DateTime dtTo = Convert.ToDateTime(strDateTo);
        //    DataTable dt = new DataTable();
        //    List<UserRoleBO> senior = new List<UserRoleBO>();
        //    dt.Columns.Add("Date", typeof(string));
        //    dt.Columns.Add("Region", typeof(string));
        //    dt.Columns.Add("Branch", typeof(string));
        //    List<TeamLevelBO> navigateList = GetNavigationList(selectedRoleID, false);
        //    var teamofRole = roleMaster.FirstOrDefault(x => x.RoleID == selectedRoleID);
        //    long selectedRoleTeamID = Convert.ToInt64(teamofRole.TeamID);
        //    foreach (var team in navigateList)
        //    {
        //        if (team.TeamID == teamofRole.TeamID)
        //            dt.Columns.Add(team.TeamCode, typeof(string));
        //    }
        //    if (selectedRoleID == (int)AspectEnums.Roles.FOS)
        //    {
        //        dt.Columns.Add("Disty", typeof(string));
        //    }
        //    dt.Columns.Add("User Type", typeof(string));
        //    dt.Columns.Add("User Code", typeof(string));
        //    dt.Columns.Add("User Name", typeof(string));
        //    dt.Columns.Add("Mobile No", typeof(string));
        //    dt.Columns.Add("TTL Stores", typeof(string));
        //    dt.Columns.Add("Attendance", typeof(string));
        //    dt.Columns.Add("Cov. Norm", typeof(string));

        //    dt.Columns.Add("TTL. Cov.", typeof(string));
        //    dt.Columns.Add("TTL SPP Cov.", typeof(string));
        //    dt.Columns.Add("TTL Other Cov.", typeof(string));

        //    dt.Columns.Add("Cov. Plan", typeof(string));
        //    dt.Columns.Add("Cov. Vs. Plan", typeof(string));
        //    dt.Columns.Add("MTD TTL Unique Cov.", typeof(string));

        //    dt.Columns.Add("MTD SPP Unique Cov.", typeof(string));
        //    dt.Columns.Add("MTD Other Unique Cov.", typeof(string));

        //    var coverageExportData = "";//ReportBusinessInstance.GetCoverageExport(dtFrom, dtTo, AspectEnums.CoverageType.CoverageVsNorm, userId, selectedRoleID);
        //    var AllData = (from coverage in coverageExportData
        //                   select coverage).OrderBy(x => x.userid).ThenBy(x => x.DATE).ToList();

        //    long currentUserId = 0;
        //    long previousUserId = 0;
        //    List<EmployeeHierarchyBO> seniors = new List<EmployeeHierarchyBO>();
        //    foreach (var item in AllData)
        //    {
        //        EmployeeHierarchyBO userProfile = employeeList.Where(x => x.UserID == item.userid).FirstOrDefault();
        //        currentUserId = Convert.ToInt64(item.userid);

        //        if (currentUserId != previousUserId)
        //        {
        //            previousUserId = currentUserId;
        //            seniors.Clear();
        //            seniors.AddRange(GetSeniors(previousUserId, selectedRoleTeamID));
        //        }
        //        var userDesignation = roleMaster.Where(k => k.RoleID == item.roleid).FirstOrDefault();
        //        DataRow dr = dt.NewRow();

        //        foreach (var team in navigateList)
        //        {
        //            if (team.TeamID == teamofRole.TeamID)
        //            {
        //                var seniorCurrent = seniors.Where(x => x.RoleID == team.RoleID).FirstOrDefault();
        //                if (seniorCurrent != null)
        //                {
        //                    dr[team.TeamCode] = seniorCurrent.FirstName;
        //                }
        //                else
        //                {
        //                    dr[team.TeamCode] = "";
        //                }
        //            }
        //        }

        //        dr["Date"] = Convert.ToDateTime(item.DATE).ToString(DateFormat);
        //        var geolevel = userGeoMapping.FirstOrDefault(x => x.UserID == item.userid && x.RoleID == item.roleid);

        //        if (geolevel != null)
        //        {
        //            dr["Region"] = geolevel.Region;
        //            dr["Branch"] = geolevel.Branch;
        //        }
        //        else
        //        {
        //            dr["Region"] = "";
        //            dr["Branch"] = "";
        //        }


        //        dr["User Code"] = item.UserCode.ToString(CultureInfo.CurrentCulture);
        //        dr["Mobile No"] = EncryptionEngine.DecryptString(userProfile.Mobile_Calling);
        //        dr["User Name"] = userProfile.FirstName;
        //        if (selectedRoleID == (int)AspectEnums.Roles.FOS)
        //        {
        //            dr["Disty"] = employeeList.FirstOrDefault(x => x.UserID == item.userid).DistyName;
        //        }
        //        dr["User Type"] = userProfile.RoleCode;
        //        dr["TTL Stores"] = item.TotalOutletAssigned;
        //        dr["Attendance"] = item.Attendance;
        //        dr["Cov. Norm"] = item.NormTargetForDay;
        //        dr["Cov. Plan"] = item.VisitsPlanned;
        //        dr["TTL. Cov."] = item.TotalVisitsDone;
        //        dr["TTL SPP Cov."] = item.TotalVisitsDoneSPP;
        //        dr["TTL Other Cov."] = item.TotalVisitsDoneNonSPP;
        //        dr["Cov. Vs. Plan"] = item.VisitsAgainstPlanTotal;
        //        dr["MTD TTL Unique Cov."] = item.MTDUniqueCoverageTotal;
        //        dr["MTD SPP Unique Cov."] = item.MTDUniqueCoverageSPP;
        //        dr["MTD Other Unique Cov."] = item.MTDUniqueCoverageNONSPP;
        //        dt.Rows.Add(dr);
        //    }

        //    return dt;

        //}

        //#endregion

        //# region Unique Outlet Coverage Report

        ///// <summary>
        ///// Gets the total coverage first level report.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="levelCount">The level count.</param>
        ///// <param name="strDateFrom">The string date from.</param>
        ///// <param name="strDateTo">The string date to.</param>
        ///// <returns></returns>
        //public JsonResponse<ReportChartBO> GetUniqueCoverageFirstLevelReport(int userID, int roleID, int levelCount, string strDateFrom, string strDateTo, int selectedRoleID, string teamID)
        //{
        //    JsonResponse<ReportChartBO> response = new JsonResponse<ReportChartBO>();
        //    try
        //    {
        //        ExceptionEngine.AppExceptionManager.Process(() =>
        //        {
        //            DateTime dtTo = Convert.ToDateTime(strDateTo);
        //            ReportChartBO barChartBO = new ReportChartBO();
        //            barChartBO.Title = "Profile Wise Summary";
        //            barChartBO.SubTitle = "From : " + Convert.ToDateTime(strDateFrom).ToString("dd-MMM-yyyy") + " To : " + dtTo.ToString("dd-MMM-yyyy");
        //            barChartBO.ReportText = "Unique Store Coverage %";
        //            barChartBO.IsLastLevel = false;

        //            int? teamId = null;
        //            if (!string.IsNullOrEmpty(teamID))
        //                teamId = Convert.ToInt32(teamID);
        //            #region Effective Role ID Check
        //            //Created by Dhiraj on 13-May-2014
        //            var effectiveRolesEmployeeList = from emp in employeeList
        //                                             join role in effectiveRoleID
        //                                             on emp.RoleID equals role
        //                                             select emp;
        //            var distinctRoles = effectiveRolesEmployeeList.GroupBy(x => new { x.RoleID, x.RoleCode }).ToList();
        //            #endregion
        //            List<ChartDataStructure> DataList = new List<ChartDataStructure>();
        //            //foreach (var users in distinctRoles)
        //            Parallel.ForEach(distinctRoles, users =>
        //            {
        //                Double CoveragPercentage = 0;
        //                CoveragPercentage = "";//ReportBusinessInstance.GetCoveragePercentage(Convert.ToDateTime(strDateFrom), Convert.ToDateTime(strDateTo), AspectEnums.CoverageType.UniqueCoverage, AspectEnums.CoverageReportType.Coverage, userID, users.Key.RoleID.Value);
        //                DataList.Add(new ChartDataStructure { Name = users.Key.RoleCode, Value = CoveragPercentage, UserId = users.Key.RoleID.Value });
        //            });
        //            barChartBO.Data = DataList.OrderByDescending(x => x.Value).ToList();
        //            response.IsSuccess = true;
        //            response.SingleResult = barChartBO;
        //        }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}

        ///// <summary>
        ///// Gets the Unique coverage report other level.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="levelCount">The level count.</param>
        ///// <param name="strDateFrom">The string date from.</param>
        ///// <param name="strDateTo">The string date to.</param>
        ///// <param name="reportLevel">The report level.</param>
        ///// <returns></returns>
        //public JsonResponse<ReportChartBO> GetUniqueCoverageReportOtherLevel(int userID, int roleID, int levelCount, string strDateFrom, string strDateTo, int reportLevel, string teamID, int selectedRoleID)
        //{
        //    JsonResponse<ReportChartBO> response = new JsonResponse<ReportChartBO>();
        //    try
        //    {
        //        ExceptionEngine.AppExceptionManager.Process(() =>
        //        {
        //            DateTime dtTo = Convert.ToDateTime(strDateTo);
        //            ReportChartBO barDrilChartBO = new ReportChartBO();
        //            int? teamId = null;
        //            if (!string.IsNullOrEmpty(teamID))
        //                teamId = Convert.ToInt32(teamID);
        //            barDrilChartBO.SubTitle = "From : " + Convert.ToDateTime(strDateFrom).ToString("dd-MMM-yyyy") + " To : " + dtTo.ToString("dd-MMM-yyyy");
        //            barDrilChartBO.ReportText = "Unique Store Coverage %";

        //            List<ChartDataStructure> DataList = new List<ChartDataStructure>();
        //            List<int> yAxisValueList = new List<int>();

        //            var UsersUnderCurrentLevel = GetEmployeesUnderCurrentUser(userID, reportLevel, selectedRoleID);

        //            var distinctRoles = UsersUnderCurrentLevel.GroupBy(x => x.RoleCode);

        //            StringBuilder rolesUnderUsers = new StringBuilder();
        //            string seprator = distinctRoles.Count() > 1 ? "," : "";
        //            foreach (var role in distinctRoles)
        //            {

        //                rolesUnderUsers.Append(role.Key + seprator);
        //            }
        //            barDrilChartBO.Title = "Data for - " + rolesUnderUsers.ToString();
        //            var isLastLevel = UsersUnderCurrentLevel.All(x => x.RoleID == selectedRoleID);
        //            barDrilChartBO.IsLastLevel = isLastLevel;
        //            #region Get Team By Selected ROle
        //            var roles = roleMaster.FirstOrDefault(x => x.RoleID == selectedRoleID);
        //            long selectedRoleTeamID = Convert.ToInt64(roles.TeamID);

        //            #endregion
        //            //foreach (var users in UsersUnderCurrentLevel)
        //            Parallel.ForEach(UsersUnderCurrentLevel, users =>
        //            {
        //                var GeoDef = string.Empty;
        //                var GeoLevel = userGeoMapping.FirstOrDefault(x => x.UserID == users.UserID && x.RoleID == users.RoleID);
        //                if (GeoLevel != null)
        //                {
        //                    GeoDef = GeoLevel.GEO;
        //                }
        //                Double CoveragPercentage = 0;
        //                CoveragPercentage = "";//ReportBusinessInstance.GetCoveragePercentage(Convert.ToDateTime(strDateFrom), Convert.ToDateTime(strDateTo), AspectEnums.CoverageType.UniqueCoverage, AspectEnums.CoverageReportType.Coverage, users.UserID.Value, selectedRoleID);
        //                DataList.Add(new ChartDataStructure { Name = users.FirstName, Value = CoveragPercentage, UserId = (long)users.UserID, ProfilePictureFileName = AppUtil.GetServerMobileImages(users.ProfilePictureFileName, AspectEnums.ImageFileTypes.User), RoleId = users.RoleID.Value, RoleName = users.RoleCode, Mobile_Calling = EncryptionEngine.DecryptString(users.Mobile_Calling), GeoDefName = GeoDef });
        //            });
        //            barDrilChartBO.Data = DataList.OrderByDescending(x => x.Value).ToList();
        //            response.IsSuccess = true;
        //            response.SingleResult = barDrilChartBO;
        //        }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}

        //#endregion
        //#endregion

        //#region Activity Dashboard

        //public DataTable GetActivityReportExcelImport(string strDateFrom, string strDateTo, long userId, int roleId, int? teamID, int selectedRoleID, int selectedModule, bool IsStorewise, bool IsWithImage) //VC20140806
        //{
        //    DataTable dtData = new DataTable();
        //    try
        //    {
        //        ExceptionEngine.AppExceptionManager.Process(() =>
        //        {
        //            dtData = ActivityExportData(strDateFrom, strDateTo, userId, roleId, dtData, teamID, selectedRoleID, selectedModule, IsStorewise, IsWithImage); //VC20140806
        //        }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
        //    }
        //    catch (Exception)
        //    {

        //    }
        //    return dtData;
        //}

        //private DataTable ActivityExportData(string strDateFrom, string strDateTo, long userId, int roleId, DataTable dtData, int? teamID, int selectedRoleID, int selectedModule, bool IsStorewise, bool IsWithImage) //VC20140806
        //{

        //    var role = roleMaster.FirstOrDefault(x => x.RoleID == selectedRoleID);
        //    DataTable dtActivityDateCurrent = new DataTable();
        //    long selectedRoleTeamID = Convert.ToInt64(role.TeamID);
        //    DateTime dateFrom = Convert.ToDateTime(strDateFrom);
        //    DateTime dateTo = Convert.ToDateTime(strDateTo);
        //    DataSet DSresult = new DataSet();
        //    DSresult = "";//ReportBusinessInstance.GetActivityReport(userId, dateFrom, dateTo, selectedModule, IsStorewise, IsWithImage, roleId, selectedRoleID);
        //    ModulesBO moduleDetails = "";//ReportBusinessInstance.GetModuleDetails(selectedModule, true);
        //    if (moduleDetails != null && DSresult.Tables.Count > 0)
        //    {
        //        List<EmployeeHierarchyBO> seniors = new List<EmployeeHierarchyBO>();
        //        #region Draw Common Columns
        //        List<TeamLevelBO> navigateList = GetNavigationList(selectedRoleID, false);
        //        var teamofRole = roleMaster.FirstOrDefault(x => x.RoleID == selectedRoleID);
        //        dtData = DSresult.Tables[0];
        //        dtActivityDateCurrent.Columns.Add("Date", typeof(string));
        //        dtActivityDateCurrent.Columns.Add("Region", typeof(string));
        //        dtActivityDateCurrent.Columns.Add("Branch", typeof(string));
        //        #region Draw Seniors Columns
        //        foreach (var team in navigateList)
        //        {
        //            if (team.TeamID == teamofRole.TeamID)
        //                dtActivityDateCurrent.Columns.Add(team.TeamCode, typeof(string));
        //        }
        //        #endregion
        //        dtActivityDateCurrent.Columns.Add("User Type", typeof(string));
        //        dtActivityDateCurrent.Columns.Add("User Code", typeof(string));
        //        dtActivityDateCurrent.Columns.Add("User Name", typeof(string));
        //        dtActivityDateCurrent.Columns.Add("Main Module", typeof(string));
        //        dtActivityDateCurrent.Columns.Add("Sub Module", typeof(string));
        //        if (moduleDetails.IsStoreWise == true || selectedModule == (int)AspectEnums.AppModules.GeoTagging)
        //        {
        //            dtActivityDateCurrent.Columns.Add("Store Code", typeof(string));
        //            dtActivityDateCurrent.Columns.Add("Store Name", typeof(string));
        //        }
        //        #endregion

        //        if (moduleDetails.IsQuestionModule == true)
        //        {
        //            #region Questionnaire Modules
        //            var activityData = dtData.AsEnumerable();
        //            var groupedActivityData = activityData.GroupBy(x => new
        //            {
        //                //Date = moduleDetails.IsStoreWise == true ? x.Field<DateTime>("ActivityDate").Date : x.Field<DateTime>("ActivityDate"),
        //                Date = x.Field<DateTime>("ActivityDate"),//SDCE-3772 ActivityDate changed with SurveyResponseDate to make combination unique.
        //                RoleCode = x.Field<string>("RoleCode"),
        //                UserID = x.Field<long>("UserID"),
        //                EmplCode = x.Field<string>("EmplCode"),
        //                FirstName = x.Field<string>("FirstName"),
        //                ModuleName = x.Field<string>("NAME"),
        //                ParentModule = x.Field<string>("ParentModule"),
        //                StoreID = x.Field<int>("StoreID"),
        //                StoreCode = x.Field<string>("StoreCode"),
        //                StoreName = x.Field<string>("StoreName")
        //            }).OrderBy(x => x.Key.UserID).ThenBy(x => x.Key.Date);

        //            #region Draw Questions Columns
        //            List<ActivityQuestionsBO> lstQuestion = new List<ActivityQuestionsBO>();
        //            if (moduleDetails.IsQuestionModule)//Draw Questions only for Questionnaire module
        //            {
        //                var listQuestionMaster = "";//ReportBusinessInstance.GetQuestionListWithParent(selectedModule).Where(x => ((IsWithImage == true && x.QuestionTypeID == (int)AspectEnums.QuestionTypes.PictureBox) || (IsWithImage == false && x.QuestionTypeID != (int)AspectEnums.QuestionTypes.PictureBox)) && x.QuestionTypeID != (int)AspectEnums.QuestionTypes.Label).OrderBy(k => k.Sequence);
        //                lstQuestion = listQuestionMaster.GroupBy(x => new { QuestionID = x.SurveyQuestionID, QuestionText = x.Question }).Select(x => new ActivityQuestionsBO { QuestionID = x.Key.QuestionID, QuestionText = x.Key.QuestionText }).ToList();
        //                foreach (var questions in lstQuestion)
        //                {

        //                    try
        //                    {
        //                        dtActivityDateCurrent.Columns.Add(questions.QuestionText, typeof(string));
        //                    }
        //                    catch (System.Data.DuplicateNameException)
        //                    {
        //                        LogTraceEngine.WriteLogWithCategory("Duplicate Question in module = " + selectedModule + "", AppVariables.AppLogTraceCategoryName.General);
        //                    }
        //                }
        //            }
        //            #endregion

        //            long currentUserId = 0;
        //            long previousUserId = 0;
        //            var geolevel = new GetUserGeoBO();
        //            foreach (var rows in groupedActivityData)
        //            {
        //                #region draw Common questionare rows
        //                DataRow dr = dtActivityDateCurrent.NewRow();
        //                currentUserId = rows.Key.UserID;
        //                if (currentUserId != previousUserId)
        //                {
        //                    previousUserId = currentUserId;
        //                    seniors.Clear();
        //                    seniors.AddRange(GetSeniors(previousUserId, selectedRoleTeamID));
        //                    geolevel = userGeoMapping.FirstOrDefault(x => x.UserID == previousUserId && x.RoleID == selectedRoleID);


        //                }
        //                if (geolevel != null)
        //                {
        //                    dr["Region"] = geolevel.Region;
        //                    dr["Branch"] = geolevel.Branch;
        //                }
        //                else
        //                {
        //                    dr["Region"] = "";
        //                    dr["Branch"] = "";
        //                }
        //                //dr["Date"] = moduleDetails.IsStoreWise == true ? rows.Key.Date.ToString("dd-MMM-yyyy") : rows.Key.Date.ToString("dd-MMM-yyyy HH:mm:ss");
        //                dr["Date"] = rows.Key.Date.ToString("dd-MMM-yyyy HH:mm:ss");
        //                dr["User Type"] = rows.Key.RoleCode;
        //                dr["User Code"] = rows.Key.EmplCode;
        //                dr["User Name"] = rows.Key.FirstName;
        //                if (moduleDetails.IsStoreWise == true || selectedModule == (int)AspectEnums.AppModules.GeoTagging)
        //                {
        //                    dr["Store Code"] = rows.Key.StoreCode;
        //                    dr["Store Name"] = rows.Key.StoreName;
        //                }
        //                dr["Main Module"] = rows.Key.ParentModule;
        //                dr["Sub Module"] = rows.Key.ModuleName;

        //                #region Draw Seniors Values
        //                foreach (var team in navigateList)
        //                {

        //                    if (team.TeamID == teamofRole.TeamID)
        //                    {
        //                        var seniorCurrent = seniors.Where(x => x.RoleID == team.RoleID).FirstOrDefault();
        //                        if (seniorCurrent != null)
        //                        {
        //                            dr[team.TeamCode] = seniorCurrent.FirstName;
        //                        }
        //                        else
        //                        {
        //                            dr[team.TeamCode] = "";
        //                        }
        //                    }
        //                }
        //                #endregion
        //                #endregion

        //                #region Draw Question's Answers
        //                foreach (var questions in lstQuestion)
        //                {
        //                    var answerOfQuestion = rows.FirstOrDefault(x => x.Field<int>("SurveyQuestionID") == questions.QuestionID);
        //                    if (answerOfQuestion != null)
        //                    {
        //                        string answer = answerOfQuestion.Field<string>("UserResponse");
                                
        //                        #region Draw Images of responses
        //                        if (IsWithImage && (!string.IsNullOrEmpty(answer)) && answer.EndsWith(".jpeg"))//If response is image type
        //                        {
        //                            if (IsStorewise)
        //                            {
        //                                dr[questions.QuestionText] = string.Format("<img src='{0}' height=\"50\" width=\"100\" align=\"right\"/>", AppUtil.GetServerMobileImages(answer, AspectEnums.ImageFileTypes.Survey));

        //                            }
        //                            else
        //                            {
        //                                dr[questions.QuestionText] = string.Format("<img src='{0}' height=\"50\" width=\"100\" align=\"right\"/>", AppUtil.GetServerMobileImages(answer, AspectEnums.ImageFileTypes.General));
        //                            }

        //                        }
        //                        else
        //                        {
        //                            dr[questions.QuestionText] =Server.HtmlDecode(answer);
        //                        }
        //                        #endregion
        //                    }
        //                    else
        //                    {
        //                        dr[questions.QuestionText] = "";
        //                    }

        //                }
        //                #endregion


        //                dtActivityDateCurrent.Rows.Add(dr);

        //            }
        //            #endregion
        //        }
        //        else
        //        {
        //            #region Non-Questionnaire Module

        //            #region Draw Specific Columns
        //            if (selectedModule == (int)AspectEnums.AppModules.GeoTagging)//Geo Tagging
        //            {
        //                dtActivityDateCurrent.Columns.Add("Updated Geo Tag", typeof(string));
        //                dtActivityDateCurrent.Columns.Add("Freeze GeoTag", typeof(string));
        //                dtActivityDateCurrent.Columns.Add("Deviation", typeof(string));
        //                dtActivityDateCurrent.Columns.Add("Distance", typeof(string));
        //                dtActivityDateCurrent.Columns.Add("User Option", typeof(string));
        //                if (IsWithImage)//To show image 
        //                    dtActivityDateCurrent.Columns.Add("Image", typeof(string));

        //            }
        //            else if (selectedModule == (int)AspectEnums.AppModules.OrderBooking)//Order Booking
        //            {
        //                dtActivityDateCurrent.Columns.Add("Order No", typeof(string));
        //                dtActivityDateCurrent.Columns.Add("Product Code", typeof(string));
        //                dtActivityDateCurrent.Columns.Add("Qty Booked", typeof(string));

        //            }
        //            else if (selectedModule == (int)AspectEnums.AppModules.StockEsclation)//StockEsclation
        //            {
        //                dtActivityDateCurrent.Columns.Add("Order No", typeof(string));
        //                dtActivityDateCurrent.Columns.Add("Product Code", typeof(string));
        //                dtActivityDateCurrent.Columns.Add("Qty Booked", typeof(string));
        //            }
        //            else if (selectedModule == (int)AspectEnums.AppModules.SelloutProjection)//Sellout Projection
        //            {
        //                dtActivityDateCurrent.Columns.Add("Order No", typeof(string));
        //                dtActivityDateCurrent.Columns.Add("Product Code", typeof(string));
        //                dtActivityDateCurrent.Columns.Add("Projection Quantity", typeof(string));
        //            }
        //            #endregion

        //            var groupedActivityData = dtData.AsEnumerable().OrderBy(x => x.Field<long>("UserID")).ThenBy(x => x.Field<DateTime>("ActivityDate"));
        //            long currentUserId = 0;
        //            long previousUserId = 0;
        //            var geolevel = new GetUserGeoBO();
        //            foreach (var rows in groupedActivityData)
        //            {
        //                #region Draw Common Rows
        //                DataRow dr = dtActivityDateCurrent.NewRow();
        //                currentUserId = rows.Field<long>("UserID");
        //                if (currentUserId != previousUserId)
        //                {
        //                    previousUserId = currentUserId;
        //                    seniors.Clear();
        //                    seniors.AddRange(GetSeniors(previousUserId, selectedRoleTeamID));
        //                    geolevel = userGeoMapping.FirstOrDefault(x => x.UserID == previousUserId && x.RoleID == selectedRoleID);

        //                }
        //                if (geolevel != null)
        //                {
        //                    dr["Region"] = geolevel.Region;
        //                    dr["Branch"] = geolevel.Branch;
        //                }
        //                else
        //                {
        //                    dr["Region"] = "";
        //                    dr["Branch"] = "";
        //                }
        //                dr["Date"] = rows.Field<DateTime>("ActivityDate");
        //                dr["User Type"] = rows.Field<string>("RoleCode");
        //                dr["User Code"] = rows.Field<string>("EmplCode");
        //                dr["User Name"] = rows.Field<string>("FirstName");
        //                if (moduleDetails.IsStoreWise == true || selectedModule == (int)AspectEnums.AppModules.GeoTagging)
        //                {
        //                    dr["Store Code"] = rows.Field<string>("StoreCode");
        //                    dr["Store Name"] = rows.Field<string>("StoreName");
        //                }
        //                dr["Main Module"] = rows.Field<string>("ParentModule");
        //                dr["Sub Module"] = rows.Field<string>("NAME");

        //                #region Draw Seniors Values
        //                foreach (var team in navigateList)
        //                {
        //                    if (team.TeamID == teamofRole.TeamID)
        //                    {
        //                        var seniorCurrent = seniors.Where(x => x.RoleID == team.RoleID).FirstOrDefault();
        //                        if (seniorCurrent != null)
        //                        {
        //                            dr[team.TeamCode] = seniorCurrent.FirstName;
        //                        }
        //                        else
        //                        {
        //                            dr[team.TeamCode] = "";
        //                        }
        //                    }
        //                }
        //                #endregion
        //                #endregion

        //                #region Draw Specific rows
        //                if (selectedModule == (int)AspectEnums.AppModules.GeoTagging)//Geo Tagging
        //                {
        //                    dr["Updated Geo Tag"] = "Lat-" + rows.Field<string>("Lattitude") + " : Lon-" + rows.Field<string>("Longitude");
        //                    dr["Freeze GeoTag"] = "Lat-" + rows.Field<double?>("FreezeLattitude") + " : Lon-" + rows.Field<double?>("FreezeLongitude");

        //                    dr["Deviation"] = rows.Field<string>("Deviation");
        //                    dr["Distance"] = rows.Field<double>("Distance");
        //                    dr["User Option"] = rows.Field<string>("userOption");
        //                    if (IsWithImage)
        //                    {
        //                        string pictureFileName = rows.Field<string>("PictureFileName");
        //                        if (!string.IsNullOrEmpty(pictureFileName) && (pictureFileName.Contains(".jpeg")))
        //                        {
        //                            dr["Image"] = string.Format("<img src='{0}' height=\"50\" width=\"100\" align=\"right\"/>", AppUtil.GetServerMobileImages(pictureFileName, AspectEnums.ImageFileTypes.Store));

        //                        }
        //                    }

        //                }
        //                else if (selectedModule == (int)AspectEnums.AppModules.OrderBooking || selectedModule == (int)AspectEnums.AppModules.StockEsclation || selectedModule == (int)AspectEnums.AppModules.SelloutProjection)//Order Booking(78) & Stock Esclation (16)
        //                {
        //                    dr["Order No"] = rows.Field<string>("OrderNo");
        //                    dr["Product Code"] = rows.Field<string>("SKUCode");
        //                    if (selectedModule == (int)AspectEnums.AppModules.SelloutProjection)//Sellout Projection
        //                    {
        //                        dr["Projection Quantity"] = rows.Field<int>("Quantity");
        //                    }
        //                    else
        //                    {
        //                        dr["Qty Booked"] = rows.Field<int>("Quantity");
        //                    }

        //                }

        //                #endregion

        //                dtActivityDateCurrent.Rows.Add(dr);
        //            }

        //            #endregion
        //        }
        //    }
        //    return dtActivityDateCurrent;


        //}

        //#endregion

        //#region User Store Mapping Export
        ////VC20140814
        ///// <summary>
        ///// Gets User Store Mapping Export
        ///// </summary>
        ///// <param name="strEmployeeCode">The Employee Code for which mapping need to download.</param>        
        ///// <returns></returns>
        //public DataTable GetUserStoreMappingData(string strEmployeeCode)
        //{
        //    DataTable dtData = new DataTable();
        //    dtData.Columns.Add("EmployeeCode", typeof(string));
        //    dtData.Columns.Add("EmployeeName", typeof(string));
        //    dtData.Columns.Add("Profile", typeof(string));
        //    dtData.Columns.Add("StoreCode", typeof(string));
        //    dtData.Columns.Add("StoreName", typeof(string));
        //    dtData.Columns.Add("StoreStatus", typeof(string));
        //    dtData.Columns.Add("MappingStatus", typeof(string));
        //    dtData.Columns.Add("MappingDate", typeof(string));

        //    var UserStoreMapping = "";//ReportBusinessInstance.GetUserStoreMappingReport(strEmployeeCode);

        //    foreach (var item in UserStoreMapping)
        //    {
        //        DataRow dr = dtData.NewRow();
        //        dr["EmployeeCode"] = item.EmplCode;
        //        dr["EmployeeName"] = item.Username;
        //        dr["Profile"] = item.UserProfile;
        //        dr["StoreCode"] = item.StoreCode;
        //        dr["StoreName"] = item.StoreName;
        //        dr["StoreStatus"] = item.StoreStatus;
        //        dr["MappingStatus"] = item.MappingStatus;
        //        dr["MappingDate"] = item.MappingDate;

        //        dtData.Rows.Add(dr);
        //    }
        //    return dtData;
        //}

        //#endregion

        //#region My Dashboard Report

        ///// <summary>
        ///// Method to get first level Attendence according to role
        ///// </summary>
        ///// <param name="userID">user ID</param>
        ///// <param name="strDateFrom">strDateFrom</param>
        ///// <param name="strDateTo">strDateTo</param>        
        ///// <returns>returns Attendence percentage based on profile</returns>
        //public JsonResponse<ReportChartBO> GetMyDashBoardReport(int userID, int roleID, string strDateFrom, string strDateTo)
        //{
        //    JsonResponse<ReportChartBO> response = new JsonResponse<ReportChartBO>();
        //    try
        //    {
        //        ExceptionEngine.AppExceptionManager.Process(() =>
        //        {
        //            DateTime dtFrom = Convert.ToDateTime(strDateFrom);
        //            DateTime dtTo = Convert.ToDateTime(strDateTo);
        //            ReportChartBO barChartBO = new ReportChartBO();
        //            barChartBO.Title = "My Summary";
        //            barChartBO.SubTitle = "From : " + dtFrom.ToString("dd-MMM-yyyy") + " To : " + dtTo.ToString("dd-MMM-yyyy");
        //            barChartBO.ReportText = "My Report";
        //            barChartBO.IsLastLevel = false;
        //            List<ChartDataStructure> DataList = new List<ChartDataStructure>();

        //            #region Vinay
        //            GetMyAttendanceDashboard(userID, DataList, dtFrom.ToString("dd-MMM-yyyy"), dtTo.ToString("dd-MMM-yyyy"));
        //            GetMyCoverageDashboard(userID, DataList, dtFrom, dtTo, AspectEnums.CoverageType.CoverageVsNorm, AspectEnums.CoverageReportType.MyDashboard);
        //            GetMyCoverageDashboard(userID, DataList, dtFrom, dtTo, AspectEnums.CoverageType.CoverageVsPlan, AspectEnums.CoverageReportType.MyDashboard);
        //            GetMyCoverageDashboard(userID, DataList, dtFrom, dtTo, AspectEnums.CoverageType.UniqueCoverage, AspectEnums.CoverageReportType.MyDashboard);


        //            #endregion
        //            barChartBO.Data = DataList.OrderByDescending(x => x.Value).ToList();
        //            response.IsSuccess = true;
        //            response.SingleResult = barChartBO;
        //        }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;

        //}

        //public JsonResponse<List<MyDashboardRecordBO>> GetMyDashboardRecordList(int userID, int moduleCode, string strDateFrom, string strDateTo)
        //{
        //    JsonResponse<List<MyDashboardRecordBO>> response = new JsonResponse<List<MyDashboardRecordBO>>();
        //    try
        //    {
        //        ExceptionEngine.AppExceptionManager.Process(() =>
        //        {
        //            DateTime dtFrom = Convert.ToDateTime(strDateFrom);
        //            DateTime dtTo = Convert.ToDateTime(strDateTo);
        //            List<MyDashboardRecordBO> lstRecords = new List<MyDashboardRecordBO>();
        //            #region Vinay
        //            if (moduleCode == (int)AspectEnums.WebModules.AttendanceDashBoard)
        //            {

        //                List<AttendanceExportBO> lstAttendances = new List<AttendanceExportBO>();
        //                string today = DateTime.Now.ToString("dd-MMM-yyyy");
        //                #region Session Removal Dhiraj 10-Nov-2014
        //                lstAttendances = "";//ReportBusinessInstance.GetAttendanceExport(Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtTo), userID, RoleID, 0);

        //                var recordsofCurrentDate = (from attendance in lstAttendances
        //                                            where attendance.UserID == userID
        //                                            orderby attendance.Date
        //                                            select new MyDashboardRecordBO { Date = attendance.Date.Value.ToString("dd-MMM-yyyy"), Value = attendance.Attendance }).ToList<MyDashboardRecordBO>();
        //                lstRecords = recordsofCurrentDate.OrderBy(x => x.Date).ToList();
        //                #endregion

        //            }
        //            else if (moduleCode == (int)AspectEnums.WebModules.TotalCoverageNormDashBoard)
        //            {
        //                var storeData = "";//ReportBusinessInstance.GetUserChartOutlets(userID, moduleCode, Convert.ToDateTime(strDateFrom), Convert.ToDateTime(strDateTo), "");
        //                lstRecords = storeData.Select(k => new MyDashboardRecordBO() { StoreName = k.OutletName, Value = k.ChartValue.ToString() }).ToList();



        //            }
        //            else if (moduleCode == (int)AspectEnums.WebModules.TotalCoverageDashBoard)
        //            {
        //                var storeData = "";//ReportBusinessInstance.GetUserChartOutlets(userID, moduleCode, Convert.ToDateTime(strDateFrom), Convert.ToDateTime(strDateTo), "");
        //                lstRecords = storeData.Select(k => new MyDashboardRecordBO() { StoreName = k.OutletName, Value = k.ChartValue.ToString() }).ToList();
        //            }
        //            else if (moduleCode == (int)AspectEnums.WebModules.UniqueOutletCoverageDashBoard)
        //            {
        //                var storeData = "";//ReportBusinessInstance.GetUserChartOutlets(userID, moduleCode, Convert.ToDateTime(strDateFrom), Convert.ToDateTime(strDateTo), "");
        //                lstRecords = storeData.Select(k => new MyDashboardRecordBO() { StoreName = k.OutletName, Value = k.ChartValue.ToString() }).ToList();
        //            }

        //            #endregion

        //            response.IsSuccess = true;
        //            response.SingleResult = lstRecords;
        //        }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }

        //    return response;
        //}

        //private static void FillTotal(List<MyDashboardRecordBO> lstRecords, string replaceValue)
        //{
        //    if (lstRecords.Count > 0)
        //    {
        //        var total = lstRecords.Sum(x => Convert.ToDecimal(x.Value.Replace(replaceValue, "")));
        //        lstRecords.Add(new MyDashboardRecordBO { Date = "", StoreName = "", ProductName = "", Value = replaceValue + total.ToString() });
        //    }
        //}

        //private static void FillEmptyData(DateTime dtFrom, DateTime dtTo, List<MyDashboardRecordBO> lstRecords, int moduleCode)
        //{
        //    #region Fill Data if data for any partiuclar data not exists
        //    for (var dt = dtFrom; dt <= dtTo; dt = dt.AddDays(1))
        //    {
        //        if (!lstRecords.Any(x => x.Date == dt.ToString("dd-MMM-yyyy")))
        //        {
        //            lstRecords.Add(new MyDashboardRecordBO { Date = dt.ToString("dd-MMM-yyyy"), StoreName = " - ", ProductName = " - ", Value = " - " });
        //        }
        //    }
        //    #endregion
        //}



        //private void GetMyAttendanceDashboard(int userID, List<ChartDataStructure> DataList, string dtFrom, string dtTo)
        //{
        //    double Percentage = "";//ReportBusinessInstance.GetAttendancePercentage(Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtTo), userID, RoleID, 0);
        //    DataList.Add(new ChartDataStructure { Name = "Check In", Value = Percentage, UserId = (int)AspectEnums.WebModules.AttendanceDashBoard });


        //}


        //private void GetMyCoverageDashboard(int userID, List<ChartDataStructure> DataList, DateTime dtFrom, DateTime dtTo, AspectEnums.CoverageType coverageType, AspectEnums.CoverageReportType coverageReportType)
        //{
        //    double Percentage = "";//ReportBusinessInstance.GetCoveragePercentage(Convert.ToDateTime(dtFrom), Convert.ToDateTime(dtTo), coverageType, coverageReportType, userID, RoleID, 0);
        //    switch (coverageType)
        //    {
        //        case AspectEnums.CoverageType.CoverageVsNorm:
        //            DataList.Add(new ChartDataStructure { Name = "Cov. vs Norm", Value = Percentage, UserId = (int)AspectEnums.WebModules.TotalCoverageNormDashBoard });
        //            break;
        //        case AspectEnums.CoverageType.CoverageVsPlan:
        //            DataList.Add(new ChartDataStructure { Name = "Cov. vs Plan", Value = Percentage, UserId = (int)AspectEnums.WebModules.TotalCoverageDashBoard });
        //            break;
        //        case AspectEnums.CoverageType.UniqueCoverage:
        //            DataList.Add(new ChartDataStructure { Name = "Unique Store Cov.", Value = Percentage, UserId = (int)AspectEnums.WebModules.UniqueOutletCoverageDashBoard });
        //            break;

        //    }

        //}
        //#endregion

        //#region Freeze Geo Tag Report
        //private DataTable GetFreezGeoTagData()
        //{
        //    DataTable dtData = new DataTable();
        //    dtData.Columns.Add("Zone", typeof(string));
        //    dtData.Columns.Add("Branch", typeof(string));
        //    dtData.Columns.Add("StoreCode", typeof(string));
        //    dtData.Columns.Add("StoreName", typeof(string));
        //    dtData.Columns.Add("CommonGeoTag", typeof(string));
        //    dtData.Columns.Add("No. of Times Outlets Geotags", typeof(int));

        //    var freezegeotag = "";//ReportBusinessInstance.GetFreezGeoTagData();

        //    foreach (var item in freezegeotag)
        //    {
        //        DataRow dr = dtData.NewRow();
        //        dr["Zone"] = item.Zone;
        //        dr["Branch"] = item.Branch;
        //        dr["StoreCode"] = item.StoreCode;
        //        dr["StoreName"] = item.StoreName;
        //        dr["CommonGeoTag"] = "lat - " + item.Lattitude + " : lon-" + item.Longitude;
        //        dr["No. of Times Outlets Geotags"] = item.Occurance;

        //        dtData.Rows.Add(dr);
        //    }
        //    return dtData;
        //}
        //#endregion

        //#region VOC Download
        //private DataTable GetVocDownlaodData(string strDateFrom, string strDateTo, string BizDivision, string Region, string CustomerType, string ChannelCategory, string FeedbackType, string VOCClass, string Category, string SubCategory, string State, string VOCManagerName, string BizObjective)
        //{

        //    DateTime DateFrom = Convert.ToDateTime(strDateFrom);
        //    DateTime DateTo = Convert.ToDateTime(strDateTo);
        //    int VOCManagerRoleID = 0;
        //    string vocManagerName = AppUtil.GetEnumDescription(AspectEnums.RoleCode.VOCManager);
        //    var VOCManagerRole = roleMaster.FirstOrDefault(k => k.Code == vocManagerName);
        //    if (VOCManagerRole != null)
        //    {
        //        VOCManagerRoleID = VOCManagerRole.RoleID;
        //    }
        //    //Select VOC Manager Role users
        //    //string userIdsUnderCurrentUser = String.Join(",", employeeList.Where(k => k.RoleID == VOCManagerRoleID).Select(x => x.UserID).ToArray());

        //    List<VOCDownloadBO> lstVOC = "";//ReportBusinessInstance.GetVOCSearch(DateFrom, DateTo, BizDivision, Region, CustomerType, ChannelCategory, FeedbackType, VOCClass, Category, SubCategory, State, VOCManagerName, BizObjective, UserID);
        //    DataTable dtData = new DataTable();
        //    dtData.Columns.Add("VOC Date", typeof(string));
        //    dtData.Columns.Add("VOC Month", typeof(string));
        //    dtData.Columns.Add("VOC Year", typeof(string));
        //    dtData.Columns.Add("Region", typeof(string));
        //    dtData.Columns.Add("State", typeof(string));
        //    dtData.Columns.Add("VOC Manager Name", typeof(string));
        //    dtData.Columns.Add("Customer Type", typeof(string));
        //    dtData.Columns.Add("Channel Category", typeof(string));
        //    dtData.Columns.Add("Feedback Type", typeof(string));
        //    dtData.Columns.Add("VOC Class", typeof(string));
        //    dtData.Columns.Add("Biz Division", typeof(string));
        //    dtData.Columns.Add("Category", typeof(string));
        //    dtData.Columns.Add("Sub Category", typeof(string));
        //    dtData.Columns.Add("Biz Objective", typeof(string));
        //    dtData.Columns.Add("Narration", typeof(string));
        //    dtData.Columns.Add("Customer city", typeof(string));
        //    dtData.Columns.Add("Name of Firm", typeof(string));
        //    dtData.Columns.Add("Customer  Code ", typeof(string));
        //    dtData.Columns.Add("Name of Customer", typeof(string));
        //    dtData.Columns.Add("Customer Contact no", typeof(string));
        //    dtData.Columns.Add("ZSM/ZKAM/BM Name", typeof(string));
        //    dtData.Columns.Add("RSM Name", typeof(string));

        //    dtData.Columns.Add("Discussion With", typeof(string));
        //    dtData.Columns.Add("Status", typeof(string));
        //    dtData.Columns.Add("Guide", typeof(string));
        //    dtData.Columns.Add("VOC NO", typeof(string));


        //    foreach (var item in lstVOC)
        //    {
        //        DataRow dr = dtData.NewRow();
        //        dr["VOC Date"] = item.VOCDate;
        //        dr["VOC Month"] = item.VOCMonth;
        //        dr["VOC Year"] = item.VOCYear;
        //        dr["Region"] = item.Region;
        //        dr["State"] = item.STATE;
        //        dr["VOC Manager Name"] = item.VOCManagerName;
        //        dr["Customer Type"] = item.CustomerType;
        //        dr["Channel Category"] = item.ChannelCategory;
        //        dr["Feedback Type"] = item.FeedbackType;
        //        dr["VOC Class"] = item.VOCClass;
        //        dr["Biz Division"] = item.BizDivision;
        //        dr["Category"] = item.Category;
        //        dr["Sub Category"] = item.SubCategory;
        //        dr["Biz Objective"] = item.BizObjective;
        //        dr["Narration"] = item.Narration;
        //        dr["Customer city"] = item.Customercity;
        //        dr["Name of Firm"] = item.NameofFirm;
        //        dr["Customer  Code "] = item.CustomerCode;
        //        dr["Name of Customer"] = item.NameofCustomer;
        //        dr["Customer Contact no"] = item.CustomerContactno;
        //        dr["ZSM/ZKAM/BM Name"] = item.ZSM_ZKAM_BMName;
        //        dr["RSM Name"] = item.RSMName;
        //        dr["Discussion With"] = item.DiscussionWith;
        //        dr["Status"] = item.STATUS;
        //        dr["Guide"] = item.Guide;
        //        dr["VOC NO"] = item.VOCNO;


        //        dtData.Rows.Add(dr);
        //    }
        //    return dtData;


        //}
        //#endregion

        //#endregion

        //#region Common Report Functions

        //#region WriteExcel
        //public JsonResponse<string> EmailExcel(System.IO.Stream stream)
        //{
        //    JsonResponse<string> response = new JsonResponse<string>();
        //    try
        //    {
        //        ExportExcel(stream, true);
        //        response.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;


        //}
        //public void WriteExcel(System.IO.Stream stream)
        //{
        //    ExportExcel(stream, false);
        //}
        //private void ExportExcel(System.IO.Stream stream, bool sendEmail = false)
        //{
        //    try
        //    {
        //        ExceptionEngine.ProcessAction(() =>
        //        {

        //            long userId; int roleId; int selectedRoleID;
        //            string strFileName = string.Empty, strDateFrom = string.Empty, strDateTo = string.Empty, exportType = string.Empty, strEmployeeCode = string.Empty;
        //            using (StreamReader reader = new StreamReader(stream))
        //            {
        //                string body = reader.ReadToEnd();
        //                var @params = HttpUtility.ParseQueryString(body);
        //                exportType = @params["exportType"];
        //                strFileName = @params["strFileName"];

        //                if (exportType.ToLower() == "activity")
        //                {
        //                    strDateFrom = @params["strDateFrom"];
        //                    strDateTo = @params["strDateTo"];
        //                    userId = Convert.ToInt64(@params["userId"]);
        //                    roleId = Convert.ToInt32(@params["roleId"]);
        //                    int selectedModule = Convert.ToInt32(@params["selectedModule"]);
        //                    exportType = @params["exportType"];
        //                    string teamId = Convert.ToString(@params["teamID"]).Replace("undefined", "");
        //                    int? teamID = null;
        //                    if (!string.IsNullOrEmpty(teamId))
        //                        teamID = Convert.ToInt32(teamId);
        //                    selectedRoleID = Convert.ToInt32(@params["selectedRoleID"]);
        //                    bool IsStorewise = Convert.ToBoolean(@params["IsStoreWise"]);
        //                    bool IsWithImage = Convert.ToBoolean(@params["IsWithImage"]); //VC20140806 
        //                    DataTable dtData = GetActivityReportExcelImport(strDateFrom, strDateTo, userId, roleId, teamID, selectedRoleID, selectedModule, IsStorewise, IsWithImage);
        //                    ExporttoExcel(dtData, strFileName, userId, sendEmail, IsWithImage); //VC20140808 - manage export file format on basis of with or withoutimage
        //                }


        //                else if (exportType.ToLower() == "attendancenew")
        //                {
        //                    strDateFrom = @params["strDateFrom"];
        //                    strDateTo = @params["strDateTo"];
        //                    userId = Convert.ToInt64(@params["userId"]);
        //                    roleId = Convert.ToInt32(@params["roleId"]);
        //                    string teamId = Convert.ToString(@params["teamID"]).Replace("undefined", "");
        //                    int? teamID = null;
        //                    if (!string.IsNullOrEmpty(teamId))
        //                    {
        //                        teamID = Convert.ToInt32(teamId);
        //                    }
        //                    selectedRoleID = Convert.ToInt32(@params["selectedRoleID"]);
        //                    DataTable dtData = GetAttendancedataExportNew(strDateFrom, strDateTo, userId, roleId, teamID, selectedRoleID);
        //                    ExporttoExcel(dtData, strFileName, userId, sendEmail);
        //                }
        //                else if (exportType.ToLower() == "totalcoverage")
        //                {
        //                    strDateFrom = @params["strDateFrom"];
        //                    strDateTo = @params["strDateTo"];
        //                    userId = Convert.ToInt64(@params["userId"]);
        //                    roleId = Convert.ToInt32(@params["roleId"]);
        //                    string teamId = Convert.ToString(@params["teamID"]).Replace("undefined", "");
        //                    int? teamID = null;
        //                    if (!string.IsNullOrEmpty(teamId))
        //                        teamID = Convert.ToInt32(teamId);
        //                    selectedRoleID = Convert.ToInt32(@params["selectedRoleID"]);
        //                    DataTable dtData = GetCoverageExport(strDateFrom, strDateTo, userId, roleId, teamID, selectedRoleID);
        //                    ExporttoExcel(dtData, strFileName, userId, sendEmail);
        //                }
        //                else if (exportType.ToLower() == "totalcoveragenorm")
        //                {
        //                    strDateFrom = @params["strDateFrom"];
        //                    strDateTo = @params["strDateTo"];
        //                    userId = Convert.ToInt64(@params["userId"]);
        //                    roleId = Convert.ToInt32(@params["roleId"]);
        //                    string teamId = Convert.ToString(@params["teamID"]).Replace("undefined", "");
        //                    int? teamID = null;
        //                    if (!string.IsNullOrEmpty(teamId))
        //                        teamID = Convert.ToInt32(teamId);
        //                    selectedRoleID = Convert.ToInt32(@params["selectedRoleID"]);
        //                    DataTable dtData = GetCoverageExport(strDateFrom, strDateTo, userId, roleId, teamID, selectedRoleID);
        //                    ExporttoExcel(dtData, strFileName, userId, sendEmail);
        //                }
        //                else if (exportType.ToLower() == "uniquecoverage")
        //                {
        //                    strDateFrom = @params["strDateFrom"];
        //                    strDateTo = @params["strDateTo"];
        //                    userId = Convert.ToInt64(@params["userId"]);
        //                    roleId = Convert.ToInt32(@params["roleId"]);
        //                    string teamId = Convert.ToString(@params["teamID"]).Replace("undefined", "");
        //                    int? teamID = null;
        //                    if (!string.IsNullOrEmpty(teamId))
        //                        teamID = Convert.ToInt32(teamId);
        //                    selectedRoleID = Convert.ToInt32(@params["selectedRoleID"]);
        //                    DataTable dtData = GetCoverageExport(strDateFrom, strDateTo, userId, roleId, teamID, selectedRoleID);
        //                    ExporttoExcel(dtData, strFileName, userId, sendEmail);
        //                }

        //                else if (exportType.ToLower() == "displayshare")
        //                {
        //                    DataSet dsData = ExportDisplayShare(body, out userId);

        //                    ExporttoExcel(dsData.Tables[0], "Display Share", userId, sendEmail);

        //                }
        //                else if (exportType.ToLower() == "countershare")
        //                {
        //                    DataSet dsData = ExportCounterShare(body, out userId);
        //                    ExporttoExcel(dsData.Tables[0], "Counter Share", userId, sendEmail);

        //                }

        //                //VC20140814
        //                else if (exportType.ToLower() == "userstoremapping")
        //                {
        //                    strEmployeeCode = @params["strEmployeeCode"];
        //                    exportType = @params["exportType"];
        //                    DataTable dtData = GetUserStoreMappingData(strEmployeeCode);
        //                    ExporttoExcel(dtData, strFileName, 0, sendEmail);
        //                }
        //                //VC20141006
        //                else if (exportType.ToLower() == "frozengeotag")
        //                {
        //                    DataTable dtData = GetFreezGeoTagData();
        //                    ExporttoExcel(dtData, strFileName, 0, sendEmail);
        //                }

        //                else if (exportType.ToLower() == "voc")
        //                {
                          
        //                        strDateFrom = @params["dateFrom"];
        //                        strDateTo = @params["dateTo"];
        //                        string strDivision = @params["division"];
        //                        string strRegion = @params["region"];
        //                        string strCustomerType = @params["customertype"];
        //                        string strChannelCategory = @params["channelcategory"];
        //                        string strFeedBackType = @params["feedbacktype"];
        //                        string strVOCClass = @params["vocclass"];
        //                        string strCategory = @params["category"];
        //                        //added by vaishali for SDCE - 1324
        //                        string strSubCategory = @params["subcategory"];
        //                        string strState = @params["state"];
        //                        string strVOCManagerName = @params["vocmanagername"];
        //                        string strBizObjective = @params["bizobjective"];

        //                        DataTable dtData = GetVocDownlaodData(strDateFrom, strDateTo, strDivision, strRegion, strCustomerType, strChannelCategory, strFeedBackType, strVOCClass, strCategory, strSubCategory, strState, strVOCManagerName, strBizObjective);

        //                        ExporttoExcel(dtData, strFileName, 0, sendEmail);
                            
        //                }

        //                else if (exportType.ToLower() == "odscheme")
        //                {
        //                    string strSchemePeriodFrom = @params["strSchemePeriodFrom"];
        //                    string strSchemePeriodTo = @params["strSchemePeriodTo"];
        //                    string strOrderSubmissionFrom = @params["strOrderSubmissionFrom"];
        //                    string strOrderSubmissionTo = @params["strOrderSubmissionTo"];
        //                    selectedRoleID = Convert.ToInt32(@params["selectedRoleID"]);
        //                    DataTable dtData = GetSchemeReportData(strSchemePeriodFrom, strSchemePeriodTo, strOrderSubmissionFrom, strOrderSubmissionTo, selectedRoleID);

        //                    ExporttoExcel(dtData, strFileName, 0, sendEmail);
        //                }
        //                else if (exportType.ToLower() == "racereport")
        //                { //@
        //                    string productGroup = @params["ProductGroupName"];
        //                    strDateFrom = @params["strDateFrom"];
        //                    strDateTo = @params["strDateTo"];
        //                    userId = Convert.ToInt64(@params["userId"]);
        //                    DataTable dtData = GetModelWiseRaceReport(userId, productGroup, strDateFrom, strDateTo);
        //                    ExporttoExcel(dtData, strFileName, 0, sendEmail);

        //                }
        //                else if (exportType.ToLower() == "newvocreport")
        //                {
        //                    if (IsPermission(0, AspectEnums.WebModules.VOCReportNew))//For security module check
        //                    {
        //                        string radioValue = @params["RadioBtnReportType"];

        //                        if (radioValue == "1")
        //                        {
        //                            string storecode = @params["Storecode"];
        //                            strDateFrom = @params["DateFrom"];
        //                            strDateTo = @params["DateTo"];
        //                            DataTable dtData = GetNewVOCReport(storecode, strDateFrom, strDateTo);
        //                            ExporttoExcel(dtData, strFileName, 0, sendEmail);
        //                        }

        //                        if (radioValue == "2")
        //                        {
        //                            strDateFrom = @params["DateFrom"];
        //                            strDateTo = @params["DateTo"];
        //                            DateTime datefrom = Convert.ToDateTime(strDateFrom);
        //                            int fromMonth = datefrom.Month;
        //                            int fromYear = datefrom.Year;
        //                            DateTime dateTo = Convert.ToDateTime(strDateTo);
        //                            int toMonth = dateTo.Month;
        //                            int toYear = dateTo.Year;
        //                            string strProductCategory = @params["ProductCategory"];
        //                            string strTypeOfPartner = @params["TypeOfPartner"];
        //                            string strPartnerCode = @params["PartnerCode"];
        //                            string strCityTier = @params["CityTier"];
        //                            string strRegion = @params["Region"];
        //                            string strState = @params["State"];
        //                            string strCity = @params["City"];
        //                            DataTable dtDataReport = GetNewVOCReportMonthWise(fromMonth, toMonth, fromYear, toYear, strProductCategory, strTypeOfPartner, strPartnerCode, strCityTier, strRegion, strState, strCity);
        //                            ExporttoExcel(dtDataReport, strFileName, 0, sendEmail);
        //                            //New Functions

        //                        }
        //                        if (radioValue == "3")
        //                        {
        //                            strDateFrom = @params["DateFrom"];
        //                            strDateTo = @params["DateTo"];
        //                            DateTime fromDate = Convert.ToDateTime(strDateFrom);
        //                            DateTime toDate = Convert.ToDateTime(strDateTo);
        //                            string Question = @params["Question"];
        //                            string strProductCategory = @params["ProductCategory"];
        //                            DataTable dtQuestion = GetVOCOpenEndedReport(strProductCategory, fromDate, toDate, Question);
        //                            ExporttoExcel(dtQuestion, strFileName, 0, sendEmail);
        //                        }
        //                    }
        //                }

        //            }
        //        }, AspectEnums.ExceptionPolicyName.AssistingAdministrators.ToString());
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //private void ExporttoExcel(DataTable table, string Tittle, long userId, bool sendEmail = false, bool hasImage = false)
        //{
        //    if (sendEmail)
        //    {


        //        string fileDirectory = Server.MapPath("~/ReportFiles");
        //        if (Directory.Exists(fileDirectory))
        //        {
        //            int columnscount = table.Columns.Count;
        //            FileStream writer = null;
        //            string newFileName = null;
        //            // Added By Tanuj(19-3-2014)
        //            //if (Tittle == "Activity") // Commented by Amit Mishra (03-Sep-2014) as other questionaire modules also has .html reports
        //            //{
        //            //VC20140808 - manage export file format on basis of with or withoutimage
        //            if (hasImage == true)
        //            {
        //                newFileName = Tittle + "_" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + ".html";
        //            }
        //            //if (hasImage == false)
        //            else
        //            {
        //                newFileName = Tittle + "_" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + ".txt";
        //            }
        //            //}
        //            //else
        //            //{
        //            //    newFileName = Tittle + "_" + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + ".txt";
        //            //}
        //            string uploadedFileName = fileDirectory + @"\" + newFileName;
        //            using (writer = new FileStream(uploadedFileName, FileMode.Create, FileAccess.Write, FileShare.None))
        //            {
        //                //if (Tittle == "Beats")
        //                //{
        //                WriteContentCSV(table, columnscount, new StreamWriter(writer), hasImage);

        //                WriteContentCSVMultipart(table, columnscount, new StreamWriter(writer), hasImage);
        //                //}
        //                //else
        //                //{
        //                //    WriteContent(table, columnscount, new StreamWriter(writer), hasImage);
        //                //}

        //            }

        //            //Send Email

        //            //var loggedUserID = HttpContext.Current.Session[Smasung.SmartDost.PresentationLayer.WebApp.Core.PageConstants.SESSION_USER_ID] as long?;
        //            var userProfile = UserBusinessInstance.DisplayUserProfile(UserID);
        //            EmailServiceDTO emailService = new EmailServiceDTO();
        //            emailService.AttachmentFileName = newFileName;
        //            emailService.Body = "Please find attached file for " + Tittle + " report";
        //            emailService.IsAttachment = true;
        //            emailService.Priority = 1;
        //            emailService.Status = (int)AspectEnums.EmailStatus.Pending;
        //            emailService.ToName = userProfile.FirstName;
        //            emailService.ToEmail = userProfile.EmailID;
        //            emailService.FromEmail = userProfile.EmailID;
        //            emailService.Subject = Tittle + " Report";
        //            //BatchBusinessInstance.InsertEmailRecord(emailService);



        //        }
        //    }
        //    else
        //    {
        //        HttpContext.Current.Response.Clear();
        //        HttpContext.Current.Response.ClearContent();
        //        HttpContext.Current.Response.ClearHeaders();
        //        HttpContext.Current.Response.Buffer = true;
        //        //HttpContext.Current.Response.ContentType = "application/ms-excel";
        //        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
        //        string newFileName = null;
        //        //if (Tittle == "Activity")     // Commented by Amit Mishra (03-Sep-2014) as other questionaire modules also has .html reports
        //        //{
        //        //VC20140808 - manage export file format on basis of with or withoutimage
        //        if (hasImage == true)
        //        {
        //            HttpContext.Current.Response.ContentType = "application/html";
        //            newFileName = Tittle + ".html";
        //        }
        //        else
        //        {
        //            newFileName = Tittle + ".txt";
        //        }
        //        //}
        //        //else
        //        //{
        //        //    newFileName = Tittle + ".txt";
        //        //}
        //        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + newFileName);
        //        HttpContext.Current.Response.Charset = "utf-8";
        //        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
        //        int columnscount = table.Columns.Count;
        //        using (StreamWriter writer = new StreamWriter(HttpContext.Current.Response.OutputStream))
        //        {
        //            if (!hasImage)
        //            {
        //                //WriteContentCSV(table, columnscount, writer, hasImage);
        //                WriteContentCSVMultipart(table, columnscount, writer, hasImage);
        //            }
        //            else
        //            {
        //                WriteContent(table, columnscount, writer, hasImage);
        //            }



        //        }
        //        //   HttpContext.Current.Response.End();
        //    }
        //    HttpContext.Current.ApplicationInstance.CompleteRequest();
        //}
        ////private void ExporttoExcels(DataSet dsData, string Tittle, long userId, bool sendEmail = false)
        ////{
        ////    if (sendEmail)
        ////    {
        ////        DataTable dtParentTable = dsData.Tables[0];
        ////        DataTable dtChildTable = dsData.Tables[1];
        ////        int columnscount = dtParentTable.Columns.Count;
        ////        string fileDirectory = Server.MapPath("~/ReportFiles");
        ////        if (Directory.Exists(fileDirectory))
        ////        {
        ////            FileStream writer = null;
        ////            string newFileName = DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + ".xls";
        ////            string uploadedFileName = fileDirectory + @"\" + newFileName;
        ////            using (writer = new FileStream(uploadedFileName, FileMode.Create, FileAccess.Write, FileShare.None))
        ////                WriteContent(dsData.Tables[0], columnscount, new StreamWriter(writer));
        ////            //Send Email
        ////            var userProfile = UserBusinessInstance.DisplayUserProfile(userId);
        ////            EmailServiceDTO emailService = new EmailServiceDTO();
        ////            emailService.AttachmentFileName = uploadedFileName;
        ////            emailService.Body = "Please find attached file for " + Tittle + " report";
        ////            emailService.IsAttachment = true;
        ////            emailService.Priority = 1;
        ////            emailService.Status = (int)AspectEnums.EmailStatus.Pending;
        ////            emailService.ToName = userProfile.FirstName;
        ////            emailService.ToEmail = userProfile.EmailID;
        ////            emailService.FromEmail = "vkanojia@q3tech.com";
        ////            emailService.Subject = Tittle + " Report";
        ////            BatchBusinessInstance.InsertEmailRecord(emailService);


        ////        }
        ////    }
        ////    else
        ////    {


        ////        HttpContext.Current.Response.Clear();
        ////        HttpContext.Current.Response.ClearContent();
        ////        HttpContext.Current.Response.ClearHeaders();
        ////        HttpContext.Current.Response.Buffer = true;
        ////        HttpContext.Current.Response.ContentType = "application/ms-excel";

        ////        HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + Tittle + ".xls");

        ////        HttpContext.Current.Response.Charset = "utf-8";
        ////        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("windows-1250");
        ////        DataTable dtParentTable = dsData.Tables[0];
        ////        int columnscount = dtParentTable.Columns.Count;
        ////        using (StreamWriter writer = new StreamWriter(HttpContext.Current.Response.OutputStream))
        ////            WriteContent(dsData.Tables[0], columnscount, writer);
        ////    }
        ////    HttpContext.Current.ApplicationInstance.CompleteRequest();
        ////}
        //#region Added by Tanuj WriteContentCSV(19-3-2014)

        //private static void WriteContentCSV(DataTable table, int columnscount, StreamWriter writer, bool hasImage = false)
        //{
        //    #region Old Commented Code
        //    //StringBuilder sb = new StringBuilder();

        //    //IEnumerable<string> columnNames = table.Columns.Cast<DataColumn>().
        //    //                       Select(column => column.ColumnName);
        //    //sb.AppendLine(string.Join("\t", columnNames));

        //    //foreach (DataRow row in table.Rows)
        //    //{
        //    //    IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
        //    //    sb.AppendLine(string.Join("\t", fields));
        //    //}

        //    //writer.Write(sb.ToString());
        //    #endregion

        //    #region New Code Directly write string to file
        //    writer.AutoFlush = true;
        //    IEnumerable<string> columnNames = table.Columns.Cast<DataColumn>().
        //                           Select(column => column.ColumnName);
        //    writer.Write(string.Join("\t", columnNames) + "\n");
        //    foreach (DataRow row in table.Rows)
        //    {
        //        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
        //        writer.Write(string.Join("\t", fields) + "\n");
        //    }
        //    #endregion

        //    //String s = sb.ToString();
        //    //writer.Write(s);
        //    //writer.Write(sb);

        //}
        //private static void WriteContentCSVMultipart(DataTable table, int columnscount, StreamWriter writer, bool hasImage = false)
        //{


        //    writer.AutoFlush = true;
        //    //StringBuilder sb = new StringBuilder();

        //    IEnumerable<string> columnNames = table.Columns.Cast<DataColumn>().
        //                           Select(column => column.ColumnName);
        //    //sb.AppendLine(string.Join("\t", columnNames));
        //    writer.Write(string.Join("\t", columnNames) + "\n");
        //    foreach (DataRow row in table.Rows)
        //    {
        //        IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
        //        //sb.AppendLine(string.Join("\t", fields));
        //        writer.Write(string.Join("\t", fields) + "\n");
        //    }
        //    //String s = sb.ToString();
        //    //writer.Write(s);
        //    //writer.Write(sb);

        //}
        //#endregion

        //private static void WriteContent(DataTable table, int columnscount, StreamWriter writer, bool hasImage = false)
        //{
        //    writer.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        //    //sets font
        //    writer.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        //    writer.Write("<BR><BR><BR>");
        //    //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        //    writer.Write("<Table border='1' bgColor='#ffffff' " +
        //      "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
        //      "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR  style='font-size:12.0pt; font-family:Calibri;' >");

        //    for (int j = 0; j < columnscount; j++)
        //    {      //write in new column
        //        writer.Write("<Td>");
        //        //Get column headers  and make it as bold in excel columns
        //        writer.Write("<B>");
        //        writer.Write(table.Columns[j].ToString());
        //        writer.Write("</B>");
        //        writer.Write("</Td>");
        //    }
        //    writer.Write("</TR>");

        //    if (table.Rows.Count > 0)
        //    {
        //        foreach (DataRow row in table.Rows)
        //        {//write in new row
        //            if (hasImage)
        //                writer.Write("<TR style='height:50px;'>");
        //            else
        //                writer.Write("<TR>");
        //            for (int i = 0; i < table.Columns.Count; i++)
        //            {
        //                writer.Write("<Td>");
        //                writer.Write(row[i].ToString());
        //                writer.Write("</Td>");
        //            }

        //            writer.Write("</TR>");
        //        }
        //    }

        //    else
        //    {
        //        writer.Write("<TR>");
        //        for (int i = 0; i < table.Columns.Count; i++)
        //        {
        //            writer.Write("<Td>");
        //            writer.Write("");
        //            writer.Write("</Td>");
        //        }

        //        writer.Write("</TR>");
        //    }
        //    writer.Write("</Table>");
        //    writer.Write("</font>");
        //    writer.Flush();
        //}
        //private static void WriteContent(DataTable dtParentTable, DataTable dtChildTable, int columnscount, StreamWriter writer)
        //{
        //    writer.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 4.0 Transitional//EN"">");
        //    //sets font
        //    writer.Write("<font style='font-size:10.0pt; font-family:Calibri;'>");
        //    writer.Write("<BR><BR><BR>");
        //    //sets the table border, cell spacing, border color, font of the text, background, foreground, font height
        //    writer.Write("<Table border='1' bgColor='#ffffff' " +
        //      "borderColor='#000000' cellSpacing='0' cellPadding='0' " +
        //      "style='font-size:10.0pt; font-family:Calibri; background:white;'> <TR  style='font-size:12.0pt; font-family:Calibri;' >");

        //    for (int j = 1; j < columnscount; j++)
        //    {      //write in new column
        //        writer.Write("<Td>");
        //        //Get column headers  and make it as bold in excel columns
        //        writer.Write("<B>");
        //        writer.Write(dtParentTable.Columns[j].ToString());
        //        writer.Write("</B>");
        //        writer.Write("</Td>");
        //    }
        //    writer.Write("</TR>");

        //    if (dtParentTable.Rows.Count > 0)
        //    {
        //        foreach (DataRow row in dtParentTable.Rows)
        //        {
        //            //write in new row
        //            writer.Write("<TR>");
        //            for (int i = 1; i < dtParentTable.Columns.Count; i++)
        //            {

        //                writer.Write("<Td>");
        //                writer.Write(row[i].ToString());
        //                writer.Write("</Td>");
        //            }

        //            writer.Write("</TR>");
        //            writer.Write("<TR>");
        //            writer.Write("<Td>");
        //            writer.Write(" ");
        //            writer.Write("</Td>");
        //            for (int j = 1; j < dtChildTable.Columns.Count; j++)
        //            {

        //                //write in new column
        //                writer.Write("<Td>");
        //                //Get column headers  and make it as bold in excel columns
        //                writer.Write("<B>");
        //                writer.Write(dtChildTable.Columns[j].ToString());
        //                writer.Write("</B>");
        //                writer.Write("</Td>");
        //            }
        //            writer.Write("</TR>");
        //            var filteredData = dtChildTable.Select("ParentRowIndex=" + row["RowIndex"]).CopyToDataTable();
        //            foreach (DataRow childrow in filteredData.Rows)
        //            {
        //                writer.Write("<TR>");
        //                writer.Write("<Td>");
        //                writer.Write(" ");
        //                writer.Write("</Td>");
        //                for (int j = 1; j < filteredData.Columns.Count; j++)
        //                {
        //                    writer.Write("<Td>");
        //                    writer.Write(childrow[j].ToString());
        //                    writer.Write("</Td>");
        //                }
        //                writer.Write("</TR>");
        //            }
        //        }
        //    }

        //    else
        //    {
        //        writer.Write("<TR>");
        //        for (int i = 1; i < dtParentTable.Columns.Count; i++)
        //        {
        //            writer.Write("<Td>");
        //            writer.Write("");
        //            writer.Write("</Td>");
        //        }

        //        writer.Write("</TR>");
        //    }
        //    writer.Write("</Table>");
        //    writer.Write("</font>");
        //    writer.Flush();
        //}

        //#endregion

        ///// <summary>
        ///// Method to get Attendence according to role and base on Selected UserID
        ///// </summary>
        ///// <param name="userID">user ID</param>
        ///// <param name="strDateFrom">strDateFrom</param>
        ///// <param name="strDateTo">strDateTo</param>
        ///// <returns>returns Attendence percentage based on profile(Selected Person userID)</returns>
        //public JsonResponse<ReportChartBO> FillLastLevelData(int userID, string strDateFrom, string strDateTo, int moduleCode, string selectedResponse)
        //{
        //    JsonResponse<ReportChartBO> response = new JsonResponse<ReportChartBO>();
        //    try
        //    {

        //        var storeData = "";//ReportBusinessInstance.GetUserChartOutlets(userID, moduleCode, Convert.ToDateTime(strDateFrom), Convert.ToDateTime(strDateTo), selectedResponse);
        //        ReportChartBO reportBarDrilChartBO = new ReportChartBO();
        //        string Title = string.Empty;
        //        if (moduleCode == (int)AspectEnums.AppModules.Attendance)
        //        {
        //            reportBarDrilChartBO.Title = "Attendance Data From : " + Convert.ToDateTime(strDateFrom).ToString("dd-MMM-yyyy") + "  To  " + Convert.ToDateTime(strDateTo).ToString("dd-MMM-yyyy");
        //            reportBarDrilChartBO.ReportText = "Attendance Data";
        //        }
        //        else
        //        {
        //            reportBarDrilChartBO.Title = "Store Wise Data From : " + Convert.ToDateTime(strDateFrom).ToString("dd-MMM-yyyy") + "  To  " + Convert.ToDateTime(strDateTo).ToString("dd-MMM-yyyy");
        //            reportBarDrilChartBO.ReportText = "Store Wise Data";
        //        }


        //        List<ChartDataStructure> DataList = new List<ChartDataStructure>();
        //        foreach (var item in storeData)
        //        {

        //            DataList.Add(new ChartDataStructure { Name = item.OutletName, Value = (int)item.ChartValue });
        //        }
        //        reportBarDrilChartBO.Data = DataList;
        //        response.IsSuccess = true;
        //        response.SingleResult = reportBarDrilChartBO;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}

        //public JsonResponse<string> ResetSession(long userID, int roleID, string dateFrom, string dateTo)
        //{

        //    JsonResponse<string> response = new JsonResponse<string>();
        //    try
        //    {
        //        SetSessionData(userID, roleID, dateFrom, dateTo);

        //        response.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;


        //}

        //private void SetSessionData(long userID, int roleID, string dateFrom, string dateTo)
        //{
        //    IList<UserModuleDTO> modules = Session[Smasung.SmartDost.PresentationLayer.WebApp.Core.PageConstants.SESSION_MODULES] as List<UserModuleDTO>;
        //    List<int> assingnedAuthorizedModule = modules.Select(x => x.ModuleCode.Value).Distinct().ToList();
        //    List<int> modulesToBeChecked = new List<int>();
        //    DateTime DateFrom = Convert.ToDateTime(dateFrom);
        //    DateTime DateTo = Convert.ToDateTime(dateTo);
        //    #region Set Selected Date in Session used for MyDashboard Report
        //    HttpContext.Current.Session[SessionVariables.SelectedReportDateFrom] = dateFrom;
        //    HttpContext.Current.Session[SessionVariables.SelectedReportDateTo] = dateTo;
        //    #endregion
        //    var employeeList = new List<EmployeeHierarchyBO>();
        //    if (HttpContext.Current.Session[SessionVariables.EmployeeListUnderCurrentUser] == null)
        //    {
        //        employeeList = "";//ReportBusinessInstance.GetEmployeesHierachyUnderUser(userID);
        //        HttpContext.Current.Session[SessionVariables.EmployeeListUnderCurrentUser] = employeeList;
        //    }
        //    else
        //    {
        //        employeeList = HttpContext.Current.Session[SessionVariables.EmployeeListUnderCurrentUser] as List<EmployeeHierarchyBO>;
        //    }
        //    var uniqueEmployeeList = employeeList.Select(x => new { x.UserID }).Distinct().ToList();

        //}

        //public new bool IsModuleAuthorized(List<int> assignedModules, List<int> modulesToBeChecked)
        //{
        //    var l = assignedModules.Intersect(modulesToBeChecked);
        //    return l.Count() == 0 ? false : true;
        //}

        //#region SetCompetitionData
        ///// <summary>
        ///// Add channelID
        ///// </summary>
        ///// <param name="14-4-2014"></param>
        ///// <param name="channelID"></param>
        ///// <returns></returns>
        //public JsonResponse<string> SetCompetitionData(string date, string channelID)
        //{
        //    JsonResponse<string> response = new JsonResponse<string>();
        //    try
        //    {
        //        HttpContext.Current.Session[SessionVariables.CounterShareDateChannelTypes] = channelID;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}
        //#endregion

        //private List<EmployeeHierarchyBO> GetSeniors(long userID, long teamid)
        //{
        //    return ReportBusinessInstance.GetSeniorsDB(userID, teamid);
        //    /*Code is commented to find seniors from DB by Dhiraj on 14-Jan-2015
        //    List<EmployeeHierarchyBO> users = new List<EmployeeHierarchyBO>();
        //    var userRoles = employeeList;
        //    var reportingId = userID as long?;

        //    do
        //    {
        //        var person = userRoles.FirstOrDefault(p => p.UserID == reportingId && p.ReportingTeamID == teamid);
        //        if (person != null)
        //        {
        //            users.Add(person);
        //            reportingId = person.ReportingUserID;
        //            if (person.UserID == reportingId)
        //                reportingId = null;
        //        }
        //        else
        //        {
        //            reportingId = null;
        //        }
        //    } while (reportingId != null);
        //    return users;
        //     */
        //}
        //#region GetEmpUsingHierachy ReportingTeam


        //private List<EmployeeHierarchyBO> GetEmployeesUnderCurrentUser(int userID, int reportLevel, int selectedRoleID)
        //{

        //    var UsersUnderCurrentLevel = new List<EmployeeHierarchyBO>();
        //    if (reportLevel == 1 || reportLevel == 0)
        //        UsersUnderCurrentLevel = employeeList.Where(x => x.EmpLevel == 1).Take(1).ToList();
        //    else
        //    {
        //        var team = roleMaster.FirstOrDefault(x => x.RoleID == selectedRoleID);
        //        UsersUnderCurrentLevel = employeeList.Where(x => x.ReportingUserID == userID && x.EmpLevel > 1 && x.TeamID == team.TeamID).ToList();


        //    }
        //    return UsersUnderCurrentLevel;
        //}


        //private List<EmployeeHierarchyBO> GetEmployeeListByReportingTeamID(List<RoleMasterBO> roleMaster, List<EmployeeHierarchyBO> employeeList, int selectedRoleID = 0)
        //{
        //    var FilterEmpListTeamWise = (from emp in employeeList
        //                                 join role in roleMaster
        //                                 on new { RoleID = emp.RoleID.Value, TeamID = emp.ReportingTeamID } equals new { RoleID = role.RoleID, TeamID = role.TeamID }
        //                                 where role.RoleID == selectedRoleID
        //                                 select emp).ToList();
        //    return FilterEmpListTeamWise;

        //}
        //private List<TeamLevelBO> GetNavigationList(int selectedRoleID, bool includeCurrent)
        //{
        //    List<TeamLevelBO> navigateList = new List<TeamLevelBO>();
        //    var currentProfileLevel = roleMaster.Where(x => x.RoleID == selectedRoleID).First().ProfileLevel;
        //    if (includeCurrent)
        //    {
        //        navigateList = employeeList.Where(x => x.ProfileLevel <= currentProfileLevel).GroupBy(x => new { RoleID = x.RoleID.Value, TeamCode = x.RoleCode, TeamID = x.TeamID }).Select(x => new TeamLevelBO { RoleID = x.Key.RoleID, TeamCode = x.Key.TeamCode, TeamID = x.Key.TeamID.Value }).ToList<TeamLevelBO>();
        //    }
        //    else
        //    {
        //        navigateList = employeeList.Where(x => x.ProfileLevel < currentProfileLevel).GroupBy(x => new { RoleID = x.RoleID.Value, TeamCode = x.RoleCode, TeamID = x.TeamID }).Select(x => new TeamLevelBO { RoleID = x.Key.RoleID, TeamCode = x.Key.TeamCode, TeamID = x.Key.TeamID.Value }).ToList<TeamLevelBO>();
        //    }

        //    return navigateList;
        //}
        ////#region For SDCE-871 By Vaishali on 07 Nov 2014
        ////private string GenerateXmlfromList<T>(List<T> lst, string elementName, string attributeName)
        ////{
        ////    return new XElement("Root", lst.Select(x => new XElement(elementName, new XAttribute(attributeName, x)))).ToString();
        ////}
        ////#endregion

        //#endregion

        //public List<CommonSetupDTO> getCommonSetupChild(string ParentId)
        //{
        //    List<CommonSetupDTO> output = new List<CommonSetupDTO>();
        //    ExceptionEngine.ProcessAction(() =>
        //    {
        //        if ((!string.IsNullOrEmpty(ParentId)) && ParentId != "null")
        //        {

        //            output = SystemBusinessInstance.GetCommonSetup(null, null, ParentId);
        //            output = SystemBusinessInstance.GetCommonSetup(null, null, ParentId);

        //        }

        //    }, AspectEnums.ExceptionPolicyName.AssistingAdministrators.ToString());

        //    return output;
        //}
        //#endregion

        //#region  Add by Manoranjan
        ///// <summary>
        ///// Get Product Category for New VOC Sentiment Report
        ///// </summary>
        ///// <param name="MainType"></param>
        ///// <returns></returns>
        //public List<CommonSetupDTO> getCommonSetupNewVOC(string MainType)
        //{
        //    List<CommonSetupDTO> objOutPut = new List<CommonSetupDTO>();
        //    ExceptionEngine.ProcessAction(() =>
        //    {
        //        // objOutPut = SystemBusinessInstance.GetCommonSetup(null, null, ParentId);

        //        objOutPut = SystemBusinessInstance.getNewVOCProductCategory(MainType, "NewVOCReport", 0);


        //    }, AspectEnums.ExceptionPolicyName.AssistingAdministrators.ToString());

        //    return objOutPut;

        //}

        ///// <summary>
        ///// Get Question for VOC Report
        ///// </summary>
        ///// <param name="UploadType"></param>
        ///// <returns></returns>
        //public List<string> GetQuestionVOC(string UploadType)
        //{
        //    List<string> lststring = new List<string>();
        //    ExceptionEngine.ProcessAction(() =>
        //    {
        //        lststring = SystemBusinessInstance.GetQuestionVOC(UploadType);

        //    }, AspectEnums.ExceptionPolicyName.AssistingAdministrators.ToString());
        //    return lststring;
        //}

        //#endregion


    }
}
