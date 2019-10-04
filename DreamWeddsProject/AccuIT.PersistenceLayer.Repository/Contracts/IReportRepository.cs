
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;

#region Namespace Added for Role Master :Dhiraj 3-Dec-2013
using System.Collections.Generic;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using System;
using Samsung.SmartDost.CommonLayer.Aspects.ReportBO;
using System.Data;
#endregion
namespace Samsung.SmartDost.PersistenceLayer.Repository.Contracts
{
    /// <summary>
    /// Interface to define methods for Application system services
    /// </summary>
    public interface IReportRepository
    {
        #region Competition Report
        /* Not in Use
        List<DishCompReport> GetCompReportFirstLevel(DateTime dtFrom, DateTime dtTo);
        List<DishCompReport> GetCompReportSecondLevel(DateTime dtFrom, DateTime dtTo);
        List<DishCompReport> GetCompReportThirdLevel(DateTime dtFrom, DateTime dtTo, string strSecondLevelParam);
        List<DishCompReport> GetCompReportForthLevel(DateTime dtFrom, DateTime dtTo, string strSecondLevelParam, string strThirdLevelParam);
        List<DishCompReport> GetCompReportFifthLevel(DateTime dtFrom, DateTime dtTo, string strSecondLevelParam, string strThirdLevelParam, string strForthLevelParam);
        List<DishCompReport> GetCompReportSixthLevel(DateTime dtFrom, DateTime dtTo, string strSecondLevelParam, string strThirdLevelParam, string strForthLevelParam, string strFifthLevelParam);
         */
        /// <summary>
        /// This function will fetch competition survey response based on given date range
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To</param>
        /// <returns>Collection of competition survey data based on given date range</returns>
        List<vwCompetitionSurveyResponse> GetCompetitionSurveyRespone(DateTime dtFrom, DateTime dtTo);

        ///// <summary>
        ///// This function will fetch competition survey response based on given date for counter share and display share
        ///// </summary>
        ///// <returns>Collection of competition survey data based on given date range</returns>
        //IEnumerable<vwCompetitionSurveyResponse> GetCompetitionSurveyRespone();
        ///// <summary>
        ///// This function will last response store wise for competition survey response based on given date 
        ///// </summary>        
        ///// <param name="dtTo">Date To</param>
        ///// <returns>Collection of competition survey data based on given date </returns>
        ///// Added by tanuj(14-4-2014)
        /////(GetCompetitionSurveyRespone)  in channelID
        ////List<SPCompetitionSurveyResponse_Result> GetCompetitionSurveyRespone(DateTime dtTo, int compType, string channelID);
        //List<CompetitionSurveyResponseData> GetCompetitionSurveyRespone(DateTime dtTo, int compType, string channelID);



        /// <summary>
        /// This function will last response store wise for competition survey response based on given date from staging table CompetitionSurveyResponseData 
        /// Added By Amit Mishra (16 Oct 2014) ::AM1610
        /// </summary>        
        /// <returns>Collection of competition survey data </returns>
        List<SPGetCompetitionSurveyResponseData_Result> GetCompetitionSurveyResponse();
        #endregion

        #region Attendence Report


        /// <summary>
        /// Method to get first level Attendence according to role
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns Attendence percentage based on profile</returns>
        //List<AttendenceReport> GetAttendanceReportsLevel(int userID, int roleID, DateTime dateFrom, DateTime dateTo, int reportLevel, int? teamID, int selectedRoleID);

        /// <summary>
        /// Method to get Attendance Data for excel
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <param name="roleID">role ID</param>
        /// <param name="dataFrom">date from</param>
        /// <param name="dateTo">date To</param>
        /// <returns>returns Attendence Date for Export based on profile</returns>
        IEnumerable<AttendanceExcelData> GetAttendanceDataExport(long? userID, DateTime dateFrom, DateTime dateTo);
        List<CoverageExport> GetAttendanceAndCoverageDataExport(DateTime dateFrom, DateTime dateTo);


        /// <summary>
        /// Get the attendance percentage for selected users in the period
        /// </summary>
        /// <param name="UserIDs">Comman seperated list of UserIDs</param>
        /// <param name="StartDate">From Date</param>
        /// <param name="EndDate">To Date</param>
        /// <returns></returns>
        //double GetAttendancePercentage(string UserIDs, DateTime StartDate, DateTime EndDate);
        double GetAttendancePercentage(DateTime StartDate, DateTime EndDate, long userId, int selectedRoleID, byte IncludeAllChild = 1);

        /// <summary>
        /// Get the attendance report for selected users for the given period
        /// </summary>
        /// <param name="UserIDs">Comman seperated list of UserIDs</param>
        /// <param name="StartDate">From Date</param>
        /// <param name="EndDate">To Date</param>
        /// <returns></returns>
        List<SpGetAttendanceExport_Result> GetAttendanceExport(DateTime StartDate, DateTime EndDate, long userId, int selectedRoleID, byte IncludeAllChild = 1);

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
        List<SPGetCoverageExport_Result> GetCoverageExport(DateTime StartDate, DateTime EndDate, AspectEnums.CoverageType coverageType, long userId, int selectedRoleID, byte IncludeAllChild = 1);
        #endregion



        #region Activity Report
        /// <summary>
        /// Function to bind the Questions for all Modules
        /// </summary>
        /// <returns>Question List</returns>
        IList<SurveyQuestion> GetQuestionList();

        /// <summary>
        /// Function to bind the Questions for all Modules
        /// </summary>
        /// <param name="moduleCode">The module code.</param>
        /// <returns>
        /// Question List
        /// </returns>
        IList<SurveyQuestion> GetQuestionList(int moduleCode);

        /// <summary>
        /// Get Activity Report including Survey User Response & General User Response
        /// </summary>
        /// <param name="UserIDs">Comman Seperated User IDs</param>
        /// <param name="dtFrom">Starting date to filer</param>
        /// <param name="dtTo">To Date to Filter data</param>
        /// <param name="IsStoreWise">Storewise or Not (1=Store Wise ,0=Non Store wise)</param>
        /// <returns></returns>
        //        List<SPGetActivityReport_Result> GetActivityReport(long UserID, DateTime dtFrom, DateTime dtTo, int selectedModule, bool IsStoreWise, bool IsWithImage, int roleId,int selectedRoleID);
        DataSet GetActivityReport(long UserID, DateTime dtFrom, DateTime dtTo, int selectedModule, bool IsStoreWise, bool IsWithImage, int roleId, int selectedRoleID);
        #region SDCE-1066
        /// <summary>
        /// This function will return all the questions along with its parent question text for any particular module
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <returns></returns>
        IList<SurveyQuestion> GetQuestionListWithParent(int moduleCode);

        /// <summary>
        /// Function to find details of module based on module code
        /// Created By : Dhiraj on 06-Feb-2015 for activity Report
        /// </summary>
        /// <param name="moduleCode">The module code.</param>
        /// <param name="isMobile">To filter whether, mobile modules is required or web module</param>
        /// <returns>
        /// Question List
        /// </returns>
        Module GetModuleDetails(int moduleCode, bool isMobile);

        #endregion
        #endregion

        //VC20140814
        #region User Store Mapping Report
        /// <summary>
        /// Get report for User Store Mapping
        /// </summary>
        /// <returns></returns>        
        /// 
        List<SPUserStoreMapping_Result> GetUserStoreMappingReport(string EmployeeCode);
        #endregion
        //VC20140814

       
        #region Unique Covereage

        List<SPStoreUsers_Result> GetStoreUsers(List<long> userIDs);
        List<StoreUser> GetStoreUsers();
        List<vwStoreUser> GetStoreUsersAll();

        IList<vwGetUserGeo> GetUserGeo();
        #endregion









        #region Common Functions
        /// <summary>
        /// Return users under current role provided
        /// Created By :Dhiraj
        /// Created On :18-Dec-2013
        /// </summary>
        /// <param name="userId">UserId of Current Level</param>
        /// <param name="roleId">RoleId of Current Level</param>
        /// <param name="maxHierarchyLevelDepth">Fetch hierarchy upto this level including first level</param>
        /// <returns></returns>
        List<UserRole> GetEmployeesUnderHierarchy(long userId, int roleId, int? teamID, int? maxHierarchyLevelDepth = null, bool includeParent = false);
        // Prepare Employee Seniors
        //List<GetEmployeeHierarchy_Result> GetEmployeesHierachyUnderUser(long userId, int roleId, int? teamID, int? maxHierarchyLevelDepth, int? selectedRoleID, bool includeParent, string extraParam);

        /// <summary>
        /// New function to get the Employee hierarchy based on prepared data from SQL Job
        /// </summary>
        /// <param name="userId">Userid of the person who's hierarchy to be generated</param>
        /// <returns>List of All employee who reports to the user direcly or indirectly</returns>
        List<EmployeeJunior> GetEmployeesHierachyUnderUser(long userId);

        /// <summary>
        /// This function will fetch all the senior of any particluar employee
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<UserRole> GetSeniors(long userId);
        /// <summary>
        /// Function to Get TeamLevel.
        /// </summary>       
        /// <param name="userID">userID</param>
        /// <returns>TeamLevel List</returns>
        List<TeamLevel> GetTeamLevelBasedOnRoleID(long userID, int roleID, int? teamID, bool includeAllRoles = false);

        #region SDCE-683 Bind  Modify by Niranjan (Channel Type Mapping) 22-10-2014
        /// <summary>
        ///  This function ChannelTypeTeamMappings 
        /// </summary>
        /// <returns></returns>
        List<ChannelTypeTeamMapping> Getselectedchannels(string SelectedTeam);
        #endregion

        ///// <summary>
        ///// Function to Get UperTeamLevel.
        ///// </summary>       
        ///// <param name="userID">userID</param>
        ///// <returns>TeamLevel List</returns>
        //List<TeamLevel> GetUperTeamLevelBasedOnRoleID(long userID, int roleID, int? teamID);

        /// <summary>
        /// Return Survey User Response for particular question, and module code
        /// Created By :Dhiraj
        /// Created On :19-Dec-2013
        /// </summary>
        /// <param name="userId">UserId of Current Level</param>
        /// <param name="roleId">RoleId of Current Level</param>
        /// <param name="hierarchyLevelDepth">Difference of currentHierachy and totalHierachy of Current Level</param>
        /// <returns></returns>
        List<vwSurveryResponseModuleWise> GetSurveyUserResponse(DateTime dtFrom, DateTime dtTo, int questionId, int moduleCode, bool includeAllQuestion = false, bool includeAllModules = false);
        /// <summary>
        /// Return Survey User Response for particular  module code
        /// Created By :Dhiraj
        /// Created On :19-Dec-2013
        /// </summary>
        /// <param name="userId">UserId of Current Level</param>
        /// <param name="roleId">RoleId of Current Level</param>
        /// <param name="hierarchyLevelDepth">Difference of currentHierachy and totalHierachy of Current Level</param>
        /// <returns></returns>
        IEnumerable<vwSurveryResponseModuleWise> GetSurveyUserResponse(DateTime dtFrom, DateTime dtTo, int moduleCode);

        /// <summary>
        /// Method to get Approved Covergae Plan based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns Coverage Plans for specified date range</returns>
        IEnumerable<CoveragePlan> GetCoveragePlans(DateTime dtFrom, DateTime dtTo);
        /// <summary>
        /// Method to get Covergae Plan based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns Coverage Plans for specified date range</returns>
        /// Add status by tanuj(6-3-2014)
        IEnumerable<SPGetBeatCoveragePlan_Result> GetAllCoveragePlans(DateTime dtFrom, DateTime dtTo, int? status, long userId, int selectedRoleID, byte IncludeAllChild = 1);

        IEnumerable<SPGetPendingRejectedBeats_Result> GetPendingRejectedBeats(DateTime dtFrom, DateTime dtTo, byte status, long userId, int selectedRoleID);
        /// <summary>
        /// Method to get Survey Response based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns Survey Responses for specified date range</returns>
        IEnumerable<SurveyResponse> GetSurveyResponses(DateTime dtFrom, DateTime dtTo, long currentUserId, int selectedRoleID);

        /// <summary>
        /// Method to get Collection Survey Response based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns Survey Responses for specified date range</returns>
        IEnumerable<CollectionSurvey> GetCollectionSurveys(DateTime dtFrom, DateTime dtTo);

        /// <summary>
        /// Method to get Geo Tag Data based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns list of store geo tagged for specified date range</returns>
        IEnumerable<StoreGeoTag> GetStoreGeoTags(DateTime dtFrom, DateTime dtTo);

        /// <summary>
        /// Gets the store masters.
        /// </summary>
        /// <returns></returns>
        IEnumerable<vwStoreMaster> GetStoreMasters(List<long> users);
        /// <summary>
        /// Gets the store masters.
        /// </summary>
        /// <returns></returns>
        IEnumerable<vwStoreMaster> GetStoreMasters();

        /// <summary>
        /// Gets the store masters count.
        /// Created By: Shalu
        /// Created On: 08-Apr-2014
        /// </summary>
        /// <returns></returns>
        int GetStoreMastersCount();

        /// <summary>
        /// Gets the store masters count.
        /// Created By: Nishat
        /// Created On: 05-Aug-2014
        /// </summary>
        /// <returns></returns>
        int GetStoreCountLevelwise(string region, int level, string branch, string accountname);
        /// <summary>
        /// Method to fetch outlet chart structures
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <param name="moduleCode">module code</param>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <returns>returns chart structure</returns>
        IList<OutletChartStructure> GetUserChartOutlets(long userID, int moduleCode, DateTime startDate, DateTime endDate, string selectedResponse);
        #endregion

     

        List<Branch> GetAllBranches();


        //VC20140814
        #region Freezed Geo Tag Report
        /// <summary>
        /// Get report for Geo Tag Freeze
        /// </summary>
        /// <returns></returns>        
        /// 
        List<FreezedGeoTag> GetFreezGeoTagData();
        #endregion
        //VC20140814


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
        /// <param name="userxml"></param>
        /// <returns></returns>
        List<SPGetDisplayCounterShareResponse_Result> SPGetDisplayCounterShareResponse(int compititionType, int reportLevel, string firstLevelParam, string secondLevelParam, string thirdLevelParam, string channel, long userid);

        List<SPGetDisplayCounterShareResponseExport_Result> SPGetDisplayCounterShareResponseExport(int compititionType, int reportLevel, string firstLevelParam, string secondLevelParam, string thirdLevelParam, string channels, long userid);
        #endregion

        #region Upload VOC Data for SDCE-892 by Vaishali on 12-Nov-2013
        /// <summary>
        /// VOC Report Download
        /// </summary>
        /// <param name="DateFrom"></param>
        /// <param name="DateTo"></param>
        /// <param name="BizDivision"></param>
        /// <param name="Region"></param>
        /// <param name="CustomerType"></param>
        /// <param name="ChannelCategory"></param>
        /// <param name="FeedbackType"></param>
        /// <param name="VOCClass"></param>
        /// <param name="ConcenedDepartment"></param>
        /// <param name="Category"></param>
        /// <param name="ExtraParam"></param>
        /// <returns></returns>
        List<SPGetVOCSearch_Result> GetVOCSearch(DateTime DateFrom, DateTime DateTo, string BizDivision, string Region, string CustomerType, string ChannelCategory, string FeedbackType, string VOCClass, string Category, string SubCategory, string State, string VOCManagerName, string BizObjective, long userid);
        #endregion
        /// <summary>
        /// This function will fetch senior of any user inside same team
        /// </summary>
        /// <param name="userID">user for whom senior needs to find</param>
        /// <param name="teamid">teamid of user for whom senior needs to find</param>
        /// <returns></returns>
        List<SeniorBO> GetSeniorsDB(long userID, long teamid);

        #region added by manoranjan
        /// <summary>
        /// Get ProductGroup
        /// </summary>
        /// <returns></returns>
        List<RaceProductCategory> GetProductGroup();


        /// <summary>
        /// GetModelWiseReport (Race Report)
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="productGroup"></param>
        /// <param name="strDateFrom"></param>
        /// <param name="strDateTo"></param>
        /// <returns></returns>
        List<SpRaceReportModelWise_Result> GetModelWiseReport(long userID, string productGroup, DateTime strDateFrom, DateTime strDateTo);

        /// <summary>
        /// GetModelWiseJoin ( operation for race report)
        /// </summary>
        /// <param name="productGroup"></param>
        /// <returns></returns>
        List<RaceProductModel> GetModelWiseJoin(string productGroup);

        /// <summary>
        /// Get voc report 
        /// </summary>
        /// <param name="storecode"></param>
        /// <returns></returns>
        List<VOCReportNew> GetNewVOCReport(string storecode, string strDateFrom, string strDateTo);

        /// <summary>
        /// Get New VOC Report Second Format(Month Comparision Sentiment Report)
        /// </summary>
        /// <param name="fromMonth"></param>
        /// <param name="toMonth"></param>
        /// <param name="fromYear"></param>
        /// <param name="toYear"></param>
        /// <returns></returns>
        List<SpGetVOCReport_Result> GetNewVOCReportMonthWise(int countMonthFrom, int countMonthTo, int fromMonth, int toMonth, int fromYear, int toYear, string strProductCategory, string strTypeOfPartner, string strPartnerCode, string strCityTier, string strRegion, string strState, string strCity);

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
       List<SpGetVOCReportTotalCount_Result> GetSpGetVOCTotalCount(int fromMonth, int toMonth, int fromYear, int toYear, string strProductCategory, string strTypeOfPartner, string strPartnerCode, string strCityTier, string strRegion, string strState, string strCity);


        
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
        List<spGetVOCOpenEndedReport_Result> GetVOCOpenEndedReport(string productCategory, DateTime fromDate,DateTime toDate, string Question);

        /// <summary>
        /// Get VOC Dropdown filter
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        List<NewVOCUserResponse> GetVOCSentimentFilter(string header);
        #endregion
        

        /// <summary>
        /// Get SODetails 
        /// </summary>
        /// <param name="soNumber"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        DataSet GetSODetails(string soNumber, long userID);

    }
}
