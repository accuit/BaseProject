using Samsung.SmartDost.BusinessLayer.Services.BO;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
#region Namespace Added for Role Master :Dhiraj 3-Dec-2013
using System.Collections.Generic;
using System.Collections;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using System;
using Samsung.SmartDost.CommonLayer.Aspects.ReportBO;
using System.Data;
#endregion
namespace Samsung.SmartDost.BusinessLayer.Services.Contracts
{
    public interface IReportService
    {
        #region Competition  Report
        /// <summary>
        /// This function will fetch competition survey response based on given date range
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To</param>
        /// <returns>Collection of competition survey data based on given date range</returns>
        IEnumerable<CompetitionSurveyResponseBO> GetCompetitionSurveyRespone(DateTime dtFrom, DateTime dt);

        /// <summary>
        /// This function will fetch competition survey response for counter share and display share
        /// Added By Amit Mishra (16 Oct 2014) ::AM1610
        /// </summary>
        /// <returns></returns>
        IEnumerable<CompetitionSurveyResponseBO> GetCompetitionSurveyResponse();
        #endregion

        #region Attendence Report
        /// <summary>
        /// Method to get first level Attendence according to role
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns Attendence percentage based on profile</returns>
        //List<AttendenceReportBO> GetAttendanceReportsLevel(List<EmployeeHierarchyBO> employees, int userID, int roleID, DateTime dateFrom, DateTime dateTo, int reportLevel, int? teamID, int selectedRoleID);
        /// <summary>
        /// Method to get Attendance Data for excel
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <param name="roleID">role ID</param>
        /// <param name="dataFrom">date from</param>
        /// <param name="dateTo">date To</param>
        /// <returns>returns Attendence Date for Export based on profile</returns>
        IEnumerable<AttendanceDataExcelBO> GetAttendancedataExcel(long? userID, DateTime dateFrom, DateTime dateTo);
        List<AttendanceAndCoverageDataaExcelBO> GetAttendanceAndCoverageDataExport(DateTime dateFrom, DateTime dateTo);

        /// <summary>
        /// Get the attendance percentage for selected users in the period
        /// </summary>
        /// <param name="UserIDs">Comman seperated list of UserIDs</param>
        /// <param name="StartDate">From Date</param>
        /// <param name="EndDate">To Date</param>
        /// <returns></returns>
        //double GetAttendancePercentage(string UserIDs ,DateTime StartDate ,DateTime EndDate );
        double GetAttendancePercentage(DateTime StartDate, DateTime EndDate, long userId, int selectedRoleID, byte IncludeAllChild = 1);

        /// <summary>
        /// Get the attendance report for selected users for the given period
        /// </summary>
        /// <param name="UserIDs">Comman seperated list of UserIDs</param>
        /// <param name="StartDate">From Date</param>
        /// <param name="EndDate">To Date</param>
        /// <returns></returns>
        List<AttendanceExportBO> GetAttendanceExport(DateTime StartDate, DateTime EndDate, long userId, int selectedRoleID, byte IncludeAllChild = 1);

        #endregion

        #region Coverage Report (Plan, Norm, Unique )
        /// <summary>
        ///  This Procedure return Coverage Export Percentage Chart
        /// </summary>
        /// <param name="UserIDs">Comman seperated list of UserIDs</param>
        /// <param name="StartDate">From Date</param>
        /// <param name="EndDate">To Date</param>
        /// <param name="coverageType">Coverage Type (Norm,Plan, Unique) </param>
        /// <returns></returns>
        double GetCoveragePercentage(DateTime StartDate, DateTime EndDate, AspectEnums.CoverageType coverageType, AspectEnums.CoverageReportType coverageReportType, long userId, int selectedRoleID, byte IncludeAllChild = 1);

        /// <summary>
        ///  This Procedure return Coverage Export Data for Excel download
        /// </summary>
        /// <param name="UserIDs">Comman seperated list of UserIDs</param>
        /// <param name="StartDate">From Date</param>
        /// <param name="EndDate">To Date</param>
        /// <param name="coverageType">Coverage Type (Norm,Plan, Unique) </param>
        /// <returns></returns>
        List<CoverageExportBO> GetCoverageExport(DateTime StartDate, DateTime EndDate, AspectEnums.CoverageType coverageType, long userId, int selectedRoleID);
        #endregion

        #region Activity Report
        /// <summary>
        /// Function to bind the Questions base For all Modules
        /// </summary>
        /// <returns>Question List</returns>
        IList<QuestionListBO> QuestionList();

        /// <summary>
        /// Function to bind the Questions for all Module
        /// </summary>
        /// <param name="moduleCode">The module code.</param>
        /// <returns>
        /// Question List
        /// </returns>
        IList<QuestionListBO> QuestionList(int moduleCode);

        /// <summary>
        /// Get Activity Report including Survey User Response & General User Response
        /// </summary>
        /// <param name="UserIDs">Comman Seperated User IDs</param>
        /// <param name="dtFrom">Starting date to filer</param>
        /// <param name="dtTo">To Date to Filter data</param>
        /// <param name="IsStoreWise">Storewise or Not (1=Store Wise ,0=Non Store wise)</param>
        /// <returns></returns>
        //IList<ActivityReportBO> GetActivityReport(long UserID, DateTime dtFrom, DateTime dtTo, int selectedModule, bool IsStoreWise, bool IsWithImage, int roleId,int selectedRoleID);
        DataSet GetActivityReport(long UserID, DateTime dtFrom, DateTime dtTo, int selectedModule, bool IsStoreWise, bool IsWithImage, int roleId, int selectedRoleID);

        #region SDCE-1066
        /// <summary>
        /// This function will return all the questions along with its parent question text for any particular module
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <returns></returns>
        IList<QuestionListBO> GetQuestionListWithParent(int moduleCode);

        /// <summary>
        /// Function to find details of module based on module code
        /// Created By : Dhiraj on 06-Feb-2015 for activity Report
        /// </summary>
        /// <param name="moduleCode">The module code.</param>
        /// <param name="isMobile">To filter whether, mobile modules is required or web module</param>
        /// <returns>
        /// Question List
        /// </returns>
        ModulesBO GetModuleDetails(int moduleCode, bool isMobile);

        #endregion
        #endregion

        //VC20140813
        #region UserStoreMapping
        /// <summary>
        /// Return User and store mapping
        /// Created By :Vaishali Choudhary
        /// Created On :13 - Aug - 2014
        /// </summary>
        /// <param name="EmployeeCode">EmployeeCode</param>        
        /// <returns></returns>
        /// 
        IList<UserStoreMappingBO> GetUserStoreMappingReport(string EmployeeCode);
        #endregion


        #region Common Functions
        /// <summary>
        /// Return users under current role provided
        /// Created By :Dhiraj
        /// Created On :18-Dec-2013
        /// </summary>
        /// <param name="userId">UserId of Current Level</param>
        /// <param name="roleId">RoleId of Current Level</param>
        /// <param name="hierarchyLevelDepth">Difference of currentHierachy and totalHierachy of Current Level</param>
        /// <returns></returns>
        List<UserRoleBO> GetEmployeesUnderHierarchy(long userId, int roleId, int? teamID, int? hierarchyLevelDepth = null, bool includeParent = false);
        //List<EmployeeHierarchyBO> GetEmployeesHierachyUnderUser(long userId, int roleId, int? teamID, int? maxHierarchyLevelDepth, int? selectedRoleID, bool includeParent, string extraParam);
        List<EmployeeHierarchyBO> GetEmployeesHierachyUnderUser(long userId);
        /// <summary>
        /// This function will fetch all the senior of any particluar employee
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<UserRoleBO> GetSeniors(long userId);
        /// <summary>
        /// Function to Get TeamLevel.
        /// </summary>       
        /// <param name="userID">userID</param>
        /// <returns>TeamLevel List</returns>
        List<TeamLevelBO> GetTeamLevelBasedOnRoleID(long userID, int roleID, int? teamID, bool includeAllRoles = false);

        #region SDCE-683 Bind Modify by Niranjan (Channel Type Mapping) 22-10-2014
        /// <summary>
        /// This function ChannelTypeTeamMappings Bind 
        /// </summary>
        /// <returns></returns>

        List<ChannelTypeTeamMappingBO> Getselectedchannels(string SelectedTeam);
        #endregion


        ///// <summary>
        ///// Function to Get Uper TeamLevel.
        ///// </summary>       
        ///// <param name="userID">userID</param>      
        ///// <returns>TeamLevel List</returns>
        //List<TeamLevelBO> GetUperTeamLevelBasedOnRoleID(long userID, int roleID, int? teamID);
        /// <summary>
        /// Return Survey User Response for particular question, and module code
        /// Created By :Dhiraj
        /// Created On :19-Dec-2013
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To</param>
        /// <param name="questionId">Question Id</param>
        /// <param name="moduleCode">Module code</param>
        /// <param name="includeAllQuestion">Include All Question irresptive of question id provided</param>
        /// <param name="includeAllModules">Include all modules irresptive of module id provided</param>
        /// <returns></returns>
        List<SurveryResponseModuleWiseBO> GetSurveyUserResponse(DateTime dtFrom, DateTime dtTo, int questionId, int moduleCode, bool includeAllQuestion = false, bool includeAllModules = false);

        /// <summary>
        /// Method to get approved Covergae Plan based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns Coverage Plans for specified date range</returns>
        IList<CoveragePlanBO> GetCoveragePlans(DateTime dtFrom, DateTime dtTo);
        /// <summary>
        /// Method to get All Covergae Plan based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns Coverage Plans for specified date range</returns>
        /// Add status by tanuj(6-3-2014)
        IList<CoverageBeatPlanBO> GetAllCoveragePlans(DateTime dtFrom, DateTime dtTo, int? status, long userId, int selectedRoleID, byte IncludeAllChild = 1);
        /// <summary>
        /// Method to get Survey Response based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns Survey Responses for specified date range</returns>
        IList<SurveyResponseBO> GetSurveyResponses(DateTime dtFrom, DateTime dtTo, long currentUserId, int selectedRoleID);
        //  IList<StoreBO> Storemaster();

        /// <summary>
        /// Method to get Collection Survey Response based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns Survey Responses for specified date range</returns>
        IList<CollectionSurveyBO> GetCollectionSurveys(DateTime dtFrom, DateTime dtTo);
        IList<GetUserGeoBO> GetUserGeo();
        /// <summary>
        /// Method to get Geo Tag Data based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns list of store geo tagged for specified date range</returns>
        IList<StoreGeoTagBO> GetStoreGeoTags(DateTime dtFrom, DateTime dtTo);

        /// <summary>
        /// Gets the store masters.
        /// </summary>
        /// <returns></returns>
        IList<StoreBO> GetStoreMasters(List<long> users);
        /// <summary>
        /// Gets the store masters.
        /// </summary>
        /// <returns></returns>
        IList<StoreBO> GetStoreMasters();

        /// <summary>
        /// Gets the store masters total stores.
        /// </summary>
        int GetStoreMastersCount();
        /// <summary>
        /// Gets the storeMaster total stores
        /// </summary>
        /// <returns></returns>
        int GetStoreCountLevelwise(string region, int level, string branch, string accountname);
        // IList<StoreBO> GEtStoreMastersCount(List<long>
        #endregion



        /// <summary>
        /// Method to fetch outlet chart structures
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <param name="moduleCode">module code</param>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <returns>returns chart structure</returns>
        IList<OutletChartStructureBO> GetUserChartOutlets(long userID, int moduleCode, DateTime startDate, DateTime endDate, string selectedResponse);

        List<StoreUserBO> GetStoreUsers(List<long> userIDs);
        List<StoreUserBO> GetStoreUsers();
        List<StoreUserBO> GetStoreUsersAll();

        List<BranchBO> GetAllBranches();

        List<FreezeGeoTagBO> GetFreezGeoTagData();

        #region For SDCE-871 (Get Display Share and Counter Share Response ) By Vaishali on 07 Nov 2014
        /// <summary>
        /// Get Display Share and Counter Share Response   
        /// </summary>
        /// <param name="compititionType"></param>
        /// <param name="reportLevel"></param>
        /// <param name="firstLevelParam"></param>
        /// <param name="secondLevelParam"></param>
        /// <param name="thirdLevelParam"></param>
        /// <param name="channelxml"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        List<DisplayCounterShareResponseBO> SPGetDisplayCounterShareResponse(int compititionType, int reportLevel, string firstLevelParam, string secondLevelParam, string thirdLevelParam, string channels, long userid);

        List<CompetitionSurveyResponseBO> SPGetDisplayCounterShareResponseExport(int compititionType, int reportLevel, string firstLevelParam, string secondLevelParam, string thirdLevelParam, string channels, long userid);

        List<VOCDownloadBO> GetVOCSearch(DateTime DateFrom, DateTime DateTo, string BizDivision, string Region, string CustomerType, string ChannelCategory, string FeedbackType, string VOCClass, string Category, string SubCategory, string State, string VOCManagerName, string BizObjective, long userid);

        #endregion
        /// <summary>
        /// This function will fetch senior of any user inside same team
        /// </summary>
        /// <param name="userID">user for whom senior needs to find</param>
        /// <param name="teamid">teamid of user for whom senior needs to find</param>
        /// <returns></returns>
        List<EmployeeHierarchyBO> GetSeniorsDB(long userID, long teamid);



        #region Race  Get Product group (added by manoranjan)
        List<RaceProductGroupBO> GetProductGroup();

        List<RaceReportModelWiseBO> GetModelWiseReport(long userID,string productGroup, DateTime strDateFrom, DateTime strDateTo);

        List<RaceProductModelBO> GetModelWiseJoin(string productGroup);

        /// <summary>
        /// Get voc report 
        /// </summary>
        /// <param name="storecode"></param>
        /// <returns></returns>
        List<VOCReportNewBO> GetNewVOCReport(string storecode, string strDateFrom, string strDateTo);
        

        /// <summary>
        /// Get New VOC Report Second Format(Month Comparision)
        /// </summary>
        /// <param name="fromMonth"></param>
        /// <param name="toMonth"></param>
        /// <param name="fromYear"></param>
        /// <param name="toYear"></param>
        /// <returns></returns>
        List<GetVOCReportBO> GetNewVOCReportMonthWise(int countMonthFrom,int countMonthTo,int fromMonth, int toMonth, int fromYear, int toYear, string strProductCategory, string strTypeOfPartner, string strPartnerCode, string strCityTier, string strRegion, string strState, string strCity);

        /// <summary>
        /// Get New VOC  Total Count of store visit 
        /// </summary>
        /// <param name="fromMonth"></param>
        /// <param name="toMonth"></param>
        /// <param name="fromYear"></param>
        /// <param name="toYear"></param>
        /// <param name="strProductCategory"></param>
        /// <param name="strTypeOfPartner"></param>
        /// <param name="strPartnerCode"></param>
        /// <param name="strCityTier"></param>
        /// <param name="strRegion"></param>
        /// <param name="strState"></param>
        /// <param name="strCity"></param>
        /// <returns></returns>
        List<GetVOCTotalCountBO> GetSpGetVOCTotalCount( int fromMonth, int toMonth, int fromYear, int toYear, string strProductCategory, string strTypeOfPartner, string strPartnerCode, string strCityTier, string strRegion, string strState, string strCity);

        /// <summary>
        /// Get voc open ended report(third format)
        /// </summary>
        /// <param name="productCategory"></param>
        /// <param name="fromMonth"></param>
        /// <param name="toMonth"></param>
        /// <param name="fromYear"></param>
        /// <param name="toYear"></param>
        /// <param name="Question"></param>
        /// <returns></returns>
        List<VOCOpenEndedReportBO> GetVOCOpenEndedReport(string productCategory, DateTime fromDate, DateTime toDate, string Question);

        /// <summary>
        /// Get VOC Sentiment filter
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        List<NewVOCUserResponseBO> GetVOCSentimentFilter(string header);

        /// <summary>
        /// Get SoDetails 
        /// </summary>
        /// <param name="soNumber">SoNumber</param>
        /// <param name="userID">UserID</param>
        /// <returns></returns>
     DataSet GetSODetails(string soNumber, long userID);



        #endregion
    }
}
