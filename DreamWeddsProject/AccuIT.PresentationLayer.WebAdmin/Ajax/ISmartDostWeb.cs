using AccuIT.BusinessLayer.Services.BO;
using AccuIT.CommonLayer.Aspects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.PresentationLayer.WebApdmin.Ajax
{
    [ServiceContract]
    interface ISmartDostWeb
    {
        //#region Display Share

        ///// <summary>
        ///// Submits the role modules.
        ///// </summary>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="strSelectedModules">The string selected modules.</param>
        ///// <returns></returns>
        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/SubmitRoleModules")]
        //bool SubmitRoleModules(int roleID, string strSelectedModules, int isMobile);

       
        ///// <summary>
        ///// This function will return product wise display share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <returns>JSON data for BAR chart</returns>
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/GetCompReportSecondLevel")]
        //JsonResponse<ReportChartBO> GetCompReportSecondLevel(string strDateFrom);

        ///// <summary>
        ///// This function will return state wise display share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <param name="secondLevelParam">Selected Product</param>
        ///// <returns>JSON data for BAR chart</returns>
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/GetCompReportThirdLevel")]
        //JsonResponse<ReportChartBO> GetCompReportThirdLevel(string strDateFrom, string secondLevelParam);

        ///// <summary>
        ///// This function will return city wise display share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <param name="secondLevelParam">Selected Product</param>
        ///// <param name="thirdLevelParam">Selected Region</param>
        ///// <returns>JSON data for Bar chart</returns>
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/GetCompReportForthLevel")]
        //JsonResponse<ReportChartBO> GetCompReportForthLevel(string strDateFrom, string secondLevelParam, string thirdLevelParam);

        //  /// <summary>
        ///// This function will return partner wise display share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <param name="secondLevelParam">Selected Product</param>
        ///// <param name="thirdLevelParam">Selected Region</param>
        ///// <param name="forthLevelParam">Selected State</param>
        ///// <param name="fifthLevelParam">Selected City</param>
        ///// <returns>JSON data for Bar chart</returns>
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/GetCompReportSixthLevel")]
        //JsonResponse<ReportChartBO> GetCompReportSixthLevel(string strDateFrom, string secondLevelParam, string thirdLevelParam, string forthLevelParam, string fifthLevelParam);

        //#endregion

        //#region Common Functions

        ///// <summary>
        ///// Method to get selected outlet profile details
        ///// </summary>
        ///// <param name="userID">user primary ID</param>
        ///// <param name="storeID">store ID</param>
        ///// <returns>returns outlet entity instance</returns>
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.Bare,
        //    UriTemplate = "/WriteExcel")]
        //void WriteExcel(System.IO.Stream stream);

        ///// <summary>
        ///// Method to get selected outlet profile details
        ///// </summary>
        ///// <param name="userID">user primary ID</param>
        ///// <param name="storeID">store ID</param>
        ///// <returns>returns outlet entity instance</returns>
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.Bare,
        //    UriTemplate = "/EmailExcel")]
        //JsonResponse<string> EmailExcel(System.IO.Stream stream);

        //#endregion

        //#region AttendenceDashboard

      
        ///// <summary>
        ///// Method to get first level Attendence according to role
        ///// </summary>
        ///// <param name="userID">user primary ID</param>
        ///// <param name="RoleID">role ID</param>
        ///// <returns>returns Attendence percentage based on profile</returns>
        //[WebInvoke(Method = "POST",
        //BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //ResponseFormat = WebMessageFormat.Json,
        //UriTemplate = "/GetAttendenceReportFirstLevelNew")]
        //JsonResponse<ReportChartBO> GetAttendenceReportFirstLevelNew(int userID, int roleID, string strDateFrom, string strDateTo, string teamID, int selectedRoleID);

        ///// <summary>
        ///// Method to get Attendence according to role and base on Selected UserID
        ///// </summary>
        ///// <param name="userID">user ID</param>
        ///// <param name="strDateFrom">strDateFrom</param>
        ///// <param name="strDateTo">strDateTo</param>
        ///// <returns>returns Collection percentage based on profile(Selected Person userID)</returns>
        //[WebInvoke(Method = "POST",
        //BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //ResponseFormat = WebMessageFormat.Json,
        //UriTemplate = "/GetAttendenceReportOtherLevelNew")]
        //JsonResponse<ReportChartBO> GetAttendenceReportOtherLevelNew(int userID, int roleID, string strDateFrom, string strDateTo, int reportLevel, string teamID, int selectedRoleID);

        //#endregion

        

      

        //#region Total Coverage Report

        ///// <summary>
        ///// Gets the total coverage first level report.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="childRoleID">The child role identifier.</param>
        ///// <param name="strDateFrom">The string date from.</param>
        ///// <param name="strDateTo">The string date to.</param>
        ///// <returns></returns>
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/GetTotalCoverageFirstLevelReport")]
        //JsonResponse<ReportChartBO> GetTotalCoverageFirstLevelReport(int userID, int roleID, int childRoleID, string strDateFrom, string strDateTo, int selectedRoleID, string teamID);

        ///// <summary>
        ///// Gets the total coverage report other level.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="childRoleID">The child role identifier.</param>
        ///// <param name="strDateFrom">The string date from.</param>
        ///// <param name="strDateTo">The string date to.</param>
        ///// <param name="reportLevel">The report level.</param>
        ///// <returns></returns>
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/GetTotalCoverageReportOtherLevel")]
        //JsonResponse<ReportChartBO> GetTotalCoverageReportOtherLevel(int userID, int roleID, int childRoleID, string strDateFrom, string strDateTo, int reportLevel, string teamID, int selectedRoleID);

        //#endregion

        //#region Unique Coverage Report

        ///// <summary>
        ///// Gets the total coverage first level report.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="childRoleID">The child role identifier.</param>
        ///// <param name="strDateFrom">The string date from.</param>
        ///// <param name="strDateTo">The string date to.</param>
        ///// <returns></returns>
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/GetUniqueCoverageFirstLevelReport")]
        //JsonResponse<ReportChartBO> GetUniqueCoverageFirstLevelReport(int userID, int roleID, int childRoleID, string strDateFrom, string strDateTo, int selectedRoleID, string teamID);

        ///// <summary>
        ///// Gets the total coverage report other level.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="childRoleID">The child role identifier.</param>
        ///// <param name="strDateFrom">The string date from.</param>
        ///// <param name="strDateTo">The string date to.</param>
        ///// <param name="reportLevel">The report level.</param>
        ///// <returns></returns>
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/GetUniqueCoverageReportOtherLevel")]
        //JsonResponse<ReportChartBO> GetUniqueCoverageReportOtherLevel(int userID, int roleID, int childRoleID, string strDateFrom, string strDateTo, int reportLevel, string teamID, int selectedRoleID);

        //#endregion

     
        //#region Counter Share Share

        ///// <summary>
        ///// This function will return product wise counter share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <returns>JSON data for BAR chart</returns>
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/GetCounterShareReportSecondLevel")]
        //JsonResponse<ReportChartBO> GetCounterShareReportSecondLevel(string strDateFrom);

        ///// <summary>
        ///// This function will return state wise counter share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <param name="secondLevelParam">Selected Product</param>
        ///// <returns>JSON data for BAR chart</returns>
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/GetCounterShareReportThirdLevel")]
        //JsonResponse<ReportChartBO> GetCounterShareReportThirdLevel(string strDateFrom, string secondLevelParam);

        ///// <summary>
        ///// This function will return state wise counter share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <param name="secondLevelParam">Selected Product</param>
        ///// <returns>JSON data for BAR chart</returns>
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/GetCounterShareReportForthLevel")]
        //JsonResponse<ReportChartBO> GetCounterShareReportForthLevel(string strDateFrom, string secondLevelParam, string thirdLevelParam);

       
        ///// <summary>
        ///// This function will return partner wise counter share report based on given date range
        ///// </summary>
        ///// <param name="strDateFrom">From  Date</param>
        ///// <param name="secondLevelParam">Selected Product</param>
        ///// <param name="thirdLevelParam">Selected Region</param>
        ///// <param name="forthLevelParam">Selected State</param>
        ///// <param name="fifthLevelParam">Selected City</param>
        ///// <returns>JSON data for Bar chart</returns>
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/GetCounterShareReportSixthLevel")]
        //JsonResponse<ReportChartBO> GetCounterShareReportSixthLevel(string strDateFrom, string secondLevelParam, string thirdLevelParam, string forthLevelParam, string fifthLevelParam);

        //#endregion

        ///// <summary>
        ///// Fills the store data.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="levelCount">The level count.</param>
        ///// <param name="strDateFrom">The string date from.</param>
        ///// <param name="reportLevel">The report level.</param>
        ///// <param name="isTeam">if set to <c>true</c> [is team].</param>
        ///// <param name="teamID">The team identifier.</param>
        ///// <returns></returns>
        //[WebInvoke(Method = "POST",
        //   BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //   RequestFormat = WebMessageFormat.Json,
        //   ResponseFormat = WebMessageFormat.Json,
        //   UriTemplate = "/FillLastLevelData")]
        //JsonResponse<ReportChartBO> FillLastLevelData(int userID, string strDateFrom, string strDateTo, int moduleCode, string selectedResponse);
        ///// <summary>
        ///// Fills the store data.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="levelCount">The level count.</param>
        ///// <param name="strDateFrom">The string date from.</param>
        ///// <param name="strDateTo">The string date to.</param>
        ///// <param name="reportLevel">The report level.</param>
        ///// <param name="isTeam">if set to <c>true</c> [is team].</param>
        ///// <param name="teamID">The team identifier.</param>
        ///// <returns></returns>
        //[WebInvoke(Method = "POST",
        //   BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //   RequestFormat = WebMessageFormat.Json,
        //   ResponseFormat = WebMessageFormat.Json,
        //   UriTemplate = "/ResetSession")]
        //JsonResponse<string> ResetSession(long userID, int roleID, string dateFrom, string dateTo);
        //#region MyDashboard
        ///// <summary>
        ///// Method to get first level Attendence according to role
        ///// </summary>
        ///// <param name="userID">user primary ID</param>
        ///// <param name="RoleID">role ID</param>
        ///// <returns>returns Attendence percentage based on profile</returns>
        //[WebInvoke(Method = "POST",
        //            BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //            ResponseFormat = WebMessageFormat.Json,
        //            UriTemplate = "/GetMyDashBoardReport")]
        //JsonResponse<ReportChartBO> GetMyDashBoardReport(int userID, int roleID, string strDateFrom, string strDateTo);

        //[WebInvoke(Method = "POST",
        //        BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //        ResponseFormat = WebMessageFormat.Json,
        //        UriTemplate = "/GetMyDashboardRecordList")]
        //JsonResponse<List<MyDashboardRecordBO>> GetMyDashboardRecordList(int userID, int moduleCode, string strDateFrom, string strDateTo);
        //#endregion

        //#region Coverage vs Norm
        //#region Total Coverage Report

        ///// <summary>
        ///// Gets the total coverage first level report.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="childRoleID">The child role identifier.</param>
        ///// <param name="strDateFrom">The string date from.</param>
        ///// <param name="strDateTo">The string date to.</param>
        ///// <returns></returns>
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/GetCoverageNormFirstLevelReport")]
        //JsonResponse<ReportChartBO> GetCoverageNormFirstLevelReport(int userID, int roleID, int childRoleID, string strDateFrom, string strDateTo, int selectedRoleID, string teamID);

        ///// <summary>
        ///// Gets the total coverage report other level.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="childRoleID">The child role identifier.</param>
        ///// <param name="strDateFrom">The string date from.</param>
        ///// <param name="strDateTo">The string date to.</param>
        ///// <param name="reportLevel">The report level.</param>
        ///// <returns></returns>
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/GetCoverageNormReportOtherLevel")]
        //JsonResponse<ReportChartBO> GetCoverageNormReportOtherLevel(int userID, int roleID, int childRoleID, string strDateFrom, string strDateTo, int reportLevel, string teamID, int selectedRoleID);

        //#endregion
        //#endregion

        ///// <summary>
        ///// Fills the drop down data.
        ///// </summary>
        ///// <param name="roleId">The role identifier.</param>
        ///// <returns></returns>
        //[WebInvoke(Method = "POST",
        //            BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //            ResponseFormat = WebMessageFormat.Json,
        //            UriTemplate = "/FillDropDownData")]
        //JsonResponse<List<DrpGeoDefinitionBO>> FillDropDownData(int roleId);

        ///// <summary>
        ///// Fills the next drop down data.
        ///// </summary>
        ///// <param name="currentGeoId">The current geo identifier.</param>
        ///// <param name="nextGeoId">The next geo identifier.</param>
        ///// <returns></returns>
        //[WebInvoke(Method = "POST",
        //            BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //            ResponseFormat = WebMessageFormat.Json,
        //            UriTemplate = "/FillNextDropDownData")]
        //JsonResponse<List<GeoDefinitionBO>> FillNextDropDownData(int currentGeoId, int nextGeoId);
        //#region
        ///// Added channeID as parameter(14-4-2014)
        ///// SetCompetitionData
        ///// </summary>
        ///// <param name="date"></param>
        ///// <param name="channelID"></param>
        ///// <returns></returns>
        //[WebInvoke(Method = "POST",
        //           BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //           ResponseFormat = WebMessageFormat.Json,
        //           UriTemplate = "/SetCompetitionData")]
        //JsonResponse<string> SetCompetitionData(string date, string channelID);
        //#endregion


        ///// <summary>
        ///// Submits the APK Download Master Data.
        ///// </summary>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="strSelectedModules">The string selected Data.</param>
        ///// <returns></returns>
        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/SubmitAPKDownloadAuthorization")]
        //bool SubmitAPKDownloadAuthorization(int roleID, string strSelectedData, int userID);


        ///// <summary>
        ///// Submits the APK Download Master Data.
        ///// </summary>
        ///// <param name="roleID">The role identifier.</param>
        ///// <param name="strSelectedModules">The string selected Data.</param>
        ///// <returns></returns>
        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/SubmitKASAuthorization")]
        //bool SubmitKASAuthorization(int roleID, string strSelectedData, int userID);

        /// <summary>
        /// get list of child 
        /// </summary>
        /// <param name="ParentId"></param>
        //[OperationContract]
        //[WebInvoke(Method = "POST",
        //    BodyStyle = WebMessageBodyStyle.WrappedRequest,
        //    RequestFormat = WebMessageFormat.Json,
        //    ResponseFormat = WebMessageFormat.Json,
        //    UriTemplate = "/getCommonSetupChild")]
        //List<CommonSetupDTO> getCommonSetupChild(string ParentId);

       // #region Add by Manoranjan Gupta
       // /// <summary>
       // /// Get Product Category for NEW VOC Sentiment Report
       // /// </summary>
       // /// <param name="mainType"></param>
       // /// <returns></returns>
       // [OperationContract]
       // [WebInvoke(Method = "POST",
       //     BodyStyle = WebMessageBodyStyle.WrappedRequest,
       //     RequestFormat = WebMessageFormat.Json,
       //     ResponseFormat = WebMessageFormat.Json,
       //     UriTemplate = "/getCommonSetupNewVOC")]
       // List<CommonSetupDTO> getCommonSetupNewVOC(string mainType);

        
       // /// <summary>
       // /// Get Question  for NEW VOC  Report
       // /// </summary>
       // /// <param name="mainType"></param>
       // /// <returns></returns>
       // [OperationContract]
       // [WebInvoke(Method = "POST",
       //     BodyStyle = WebMessageBodyStyle.WrappedRequest,
       //     RequestFormat = WebMessageFormat.Json,
       //     ResponseFormat = WebMessageFormat.Json,
       //     UriTemplate = "/GetQuestionVOC")]
       //  List<string> GetQuestionVOC(string UploadType);


       // #endregion
       // #region Ageing support for End of life products

       // #region Get list of Product Type
       // /*
       // Created By     ::      Amit mishra
       // Created Date   ::      10 March 2015
       // JIRA ID        ::      SDCE-2317
       // Purpose        ::      Get list of Product Type, Group, Category
       // */
       // /// <summary>
       // /// get list of Product Type, Group, Category
       // /// </summary>
       // [OperationContract]
       // [WebInvoke(Method = "POST",
       //     BodyStyle = WebMessageBodyStyle.WrappedRequest,
       //     RequestFormat = WebMessageFormat.Json,
       //     ResponseFormat = WebMessageFormat.Json,
       //     UriTemplate = "/GetProductTypeHierarchy")]
       // JsonResponse<ProductTypeHierarchyDTO> GetProductTypeHierarchy();
       // #endregion

       // #region Get list of Product Group
       // /*
       // Created By     ::      Amit mishra
       // Created Date   ::      10 March 2015
       // JIRA ID        ::      SDCE-2317
       // Purpose        ::      Get list of Product Group
       // */
       // /// <summary>
       // /// get list of Product Group
       // /// </summary>
       // [OperationContract]
       // [WebInvoke(Method = "POST",
       //     BodyStyle = WebMessageBodyStyle.WrappedRequest,
       //     RequestFormat = WebMessageFormat.Json,
       //     ResponseFormat = WebMessageFormat.Json,
       //     UriTemplate = "/GetProductGroup")]
       // JsonResponse<ProductGroupDTO> GetProductGroup(string ProductTypeCode);
       // #endregion

       // #region Get list of Product Category
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
       // [OperationContract]
       // [WebInvoke(Method = "POST",
       //     BodyStyle = WebMessageBodyStyle.WrappedRequest,
       //     RequestFormat = WebMessageFormat.Json,
       //     ResponseFormat = WebMessageFormat.Json,
       //     UriTemplate = "/GetProductCategory")]
       // JsonResponse<ProductCategoryDTO> GetProductCategory(string ProductTypeCode, string ProductGroupCode);
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
       // [OperationContract]
       // [WebInvoke(Method = "POST",
       //     BodyStyle = WebMessageBodyStyle.WrappedRequest,
       //     RequestFormat = WebMessageFormat.Json,
       //     ResponseFormat = WebMessageFormat.Json,
       //     UriTemplate = "/EOLSaveScheme")]
       // JsonResponse<int> EOLSaveScheme(EOLSchemeDTO scheme, byte ActionType,string PrevSchemePeriodFrom);
       // #endregion

       // #region Get All EOL Schemes

       // /*
       //  Created By     ::      Vaishali Choudhary
       //  Created Date   ::      10 March 2015
       //  JIRA ID        ::      
       //  Purpose        ::      Get All EOL Schemes List for Gridview
       //  */
       // /// <summary>
       // /// Get list of Schemes 
       // /// </summary>
       // /// <param name="SchemeID"></param>
       // /// <param name="SchemeNumber"></param>
       // /// <returns></returns>
       // [OperationContract]
       // [WebInvoke(Method = "POST",
       //     BodyStyle = WebMessageBodyStyle.WrappedRequest,
       //     RequestFormat = WebMessageFormat.Json,
       //     ResponseFormat = WebMessageFormat.Json,
       //     UriTemplate = "/GetAllEOLSchemes")]
       // JsonResponse<EOLSchemeDTO> GetAllEOLSchemes(int? SchemeID = null, string SchemeNumber = "");
       // #endregion

       // #region Get Basic Models for selected Scheme
       // /// <summary>
       // /// Get List of Basic Models for selected product
       // /// </summary>
       // /// <param name="productTypeCode">selected productTypeCode</param>
       // /// <param name="productGroupCode">selected productGroupCode</param>
       // /// <param name="CategoryCode"> selected CategoryCode</param>
       // /// <returns></returns>
       // [OperationContract]
       // [WebInvoke(Method = "POST",
       //     BodyStyle = WebMessageBodyStyle.WrappedRequest,
       //     RequestFormat = WebMessageFormat.Json,
       //     ResponseFormat = WebMessageFormat.Json,
       //     UriTemplate = "/GetBasicModels")]
       // JsonResponse<BasicModelDTO> GetBasicModels(string productTypeCode, string productGroupCode, string CategoryCode, int? SchemeID);
       // #endregion


       // #region  Save Scheme Products

       // //AddProductsToScheme
       // /// <summary>
       // /// Save the scheme Products 
       // /// </summary>
       // /// <param name="schemeProducts"></param>
       // /// <returns></returns>
       // [OperationContract]
       // [WebInvoke(Method = "POST",
       //     BodyStyle = WebMessageBodyStyle.WrappedRequest,
       //     RequestFormat = WebMessageFormat.Json,
       //     ResponseFormat = WebMessageFormat.Json,
       //     UriTemplate = "/EOLSaveSchemeProducts")]
       // JsonResponse<bool> EOLSaveSchemeProducts(List<EOLSchemeDetailDTO> schemeProducts, bool isSubmit, bool isUpdate);
       // #endregion


       // #endregion

    }
}
