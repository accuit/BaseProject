using Samsung.SmartDost.BusinessLayer.Base;
using Samsung.SmartDost.BusinessLayer.Services.BO;
using Samsung.SmartDost.BusinessLayer.Services.Contracts;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.PersistenceLayer.Repository.Contracts;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
#region Namespace Added for Role Master :Dhiraj 3-Dec-2013
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using System;
using Samsung.SmartDost.CommonLayer.Aspects.ReportBO;
using System.Data;
#endregion

namespace Samsung.SmartDost.BusinessLayer.ServiceImpl
{
    /// <summary>
    /// Business class to define system services and settings
    /// </summary>
    public class ReportManager : ReportBaseService, IReportService
    {
        #region Properties

        /// <summary>
        /// Property to inject the user persistence layer object invocation
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.Report_REPOSITORY)]
        public IReportRepository ReportRepository { get; set; }

        #endregion

        #region Dish Comp Report
        /*Not in use
        public List<DishCompReportBO> GetCompReportFirstLevel(DateTime dtFrom, DateTime dtTo)
        {
            List<DishCompReportBO> DishCompReportList = new List<DishCompReportBO>();
            foreach (var item in ReportRepository.GetCompReportFirstLevel(dtFrom, dtTo))//To fetch only active record
            {
                DishCompReportBO objDishCompReportBO = new DishCompReportBO();
                ObjectMapper.Map(item, objDishCompReportBO);
                DishCompReportList.Add(objDishCompReportBO);
            }
            return DishCompReportList;
        }
        public List<DishCompReportBO> GetCompReportSecondLevel(DateTime dtFrom, DateTime dtTo)
        {
            List<DishCompReportBO> DishCompReportList = new List<DishCompReportBO>();
            foreach (var item in ReportRepository.GetCompReportSecondLevel(dtFrom, dtTo))//To fetch only active record
            {
                DishCompReportBO objDishCompReportBO = new DishCompReportBO();
                ObjectMapper.Map(item, objDishCompReportBO);
                DishCompReportList.Add(objDishCompReportBO);
            }
            return DishCompReportList;
        }
        public List<DishCompReportBO> GetCompReportThirdLevel(DateTime dtFrom, DateTime dtTo, string strSecondLevelParam)
        {
            List<DishCompReportBO> DishCompReportList = new List<DishCompReportBO>();
            foreach (var item in ReportRepository.GetCompReportThirdLevel(dtFrom, dtTo, strSecondLevelParam))//To fetch only active record
            {
                DishCompReportBO objDishCompReportBO = new DishCompReportBO();
                ObjectMapper.Map(item, objDishCompReportBO);
                DishCompReportList.Add(objDishCompReportBO);
            }
            return DishCompReportList;
        }
        public List<DishCompReportBO> GetCompReportForthLevel(DateTime dtFrom, DateTime dtTo, string strSecondLevelParam, string strThirdLevelParam)
        {
            List<DishCompReportBO> DishCompReportList = new List<DishCompReportBO>();
            foreach (var item in ReportRepository.GetCompReportForthLevel(dtFrom, dtTo, strSecondLevelParam, strThirdLevelParam))//To fetch only active record
            {
                DishCompReportBO objDishCompReportBO = new DishCompReportBO();
                ObjectMapper.Map(item, objDishCompReportBO);
                DishCompReportList.Add(objDishCompReportBO);
            }
            return DishCompReportList;
        }
        public List<DishCompReportBO> GetCompReportFifthLevel(DateTime dtFrom, DateTime dtTo, string strSecondLevelParam, string strThirdLevelParam, string strForthLevelParam)
        {
            List<DishCompReportBO> DishCompReportList = new List<DishCompReportBO>();
            foreach (var item in ReportRepository.GetCompReportFifthLevel(dtFrom, dtTo, strSecondLevelParam, strThirdLevelParam, strForthLevelParam))//To fetch only active record
            {
                DishCompReportBO objDishCompReportBO = new DishCompReportBO();
                ObjectMapper.Map(item, objDishCompReportBO);
                DishCompReportList.Add(objDishCompReportBO);
            }
            return DishCompReportList;
        }
        public List<DishCompReportBO> GetCompReportSixthLevel(DateTime dtFrom, DateTime dtTo, string strSecondLevelParam, string strThirdLevelParam, string strForthLevelParam, string strFifthLevelParam)
        {
            List<DishCompReportBO> DishCompReportList = new List<DishCompReportBO>();
            foreach (var item in ReportRepository.GetCompReportSixthLevel(dtFrom, dtTo, strSecondLevelParam, strThirdLevelParam, strForthLevelParam, strFifthLevelParam))//To fetch only active record
            {
                DishCompReportBO objDishCompReportBO = new DishCompReportBO();
                ObjectMapper.Map(item, objDishCompReportBO);
                DishCompReportList.Add(objDishCompReportBO);
            }
            return DishCompReportList;
        }
         */
        /// <summary>
        /// This function will fetch competition survey response based on given date range
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To</param>
        /// <returns>Collection of competition survey data based on given date range</returns>
        public IEnumerable<CompetitionSurveyResponseBO> GetCompetitionSurveyRespone(DateTime dtFrom, DateTime dtTo)
        {
            IEnumerable<CompetitionSurveyResponseBO> competitionSurveyRespone = new List<CompetitionSurveyResponseBO>();
            ObjectMapper.Map(ReportRepository.GetCompetitionSurveyRespone(dtFrom, dtTo), competitionSurveyRespone);
            return competitionSurveyRespone;
        }

        ///// <summary>
        ///// This function will fetch competition survey response based on given date for counter share and display share
        ///// </summary>
        ///// <param name="dt">Date</param>
        ///// <returns>Collection of competition survey data based on given date range</returns>
        //public IEnumerable<CompetitionSurveyResponseBO> GetCompetitionSurveyRespone()
        //{
        //    IEnumerable<CompetitionSurveyResponseBO> competitionSurveyRespone = new List<CompetitionSurveyResponseBO>();
        //    ObjectMapper.Map(ReportRepository.GetCompetitionSurveyRespone(), competitionSurveyRespone);
        //    return competitionSurveyRespone;
        //}
        ///// <summary>
        ///// This function will fetch competition survey response based on given date for counter share and display share
        ///// </summary>
        ///// <param name="dt">Date</param>
        ///// <returns>Collection of competition survey data based on given date range</returns>
        ///// /// added by tanuj(14-4-2014)
        ///// add channel id parameter
        //public IEnumerable<CompetitionSurveyResponseBO> GetCompetitionSurveyRespone(DateTime dtTo, int compType, string channelID)
        //{
        //    IEnumerable<CompetitionSurveyResponseBO> competitionSurveyRespone = new List<CompetitionSurveyResponseBO>();
        //    ObjectMapper.Map(ReportRepository.GetCompetitionSurveyRespone(dtTo, compType, channelID), competitionSurveyRespone);
        //    return competitionSurveyRespone;
        //}

        /// <summary>
        /// This function will fetch competition survey response for counter share and display share
        /// Added By Amit Mishra (16 Oct 2014) ::AM1610
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CompetitionSurveyResponseBO> GetCompetitionSurveyResponse()
        {
            IEnumerable<CompetitionSurveyResponseBO> competitionSurveyRespone = new List<CompetitionSurveyResponseBO>();
            ObjectMapper.Map(ReportRepository.GetCompetitionSurveyResponse(), competitionSurveyRespone);
            return competitionSurveyRespone;
        }

        #endregion

        #region Attendence Report
        /// <summary>
        /// Method to get first level Attendence according to role
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns Attendence percentage based on profile</returns>
        //public List<AttendenceReportBO> GetAttendanceReportsLevel(List<EmployeeHierarchyBO> employees, int userID, int roleID, DateTime dateFrom, DateTime dateTo, int reportLevel, int? teamID, int selectedRoleID)
        //{
        //    List<AttendenceReportBO> attendanceList = new List<AttendenceReportBO>();
        //    ObjectMapper.Map(ReportRepository.GetAttendanceReportsLevel(userID, roleID, dateFrom, dateTo, reportLevel, teamID, selectedRoleID), attendanceList);
        //    return attendanceList;
        //}

        /// <summary>
        /// Method to get Attendance Data for excel
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <param name="roleID">role ID</param>
        /// <param name="dataFrom">date from</param>
        /// <param name="dateTo">date To</param>
        /// <returns>returns Attendence Date for Export based on profile</returns>
        public IEnumerable<AttendanceDataExcelBO> GetAttendancedataExcel(long? userID, DateTime dateFrom, DateTime dateTo)
        {
            List<AttendanceDataExcelBO> attendanceDataList = new List<AttendanceDataExcelBO>();
            ObjectMapper.Map(ReportRepository.GetAttendanceDataExport(userID, dateFrom, dateTo), attendanceDataList);
            return attendanceDataList;
        }

        public List<AttendanceAndCoverageDataaExcelBO> GetAttendanceAndCoverageDataExport(DateTime dateFrom, DateTime dateTo)
        {
            List<AttendanceAndCoverageDataaExcelBO> attendanceCoverageDataList = new List<AttendanceAndCoverageDataaExcelBO>();
            ObjectMapper.Map(ReportRepository.GetAttendanceAndCoverageDataExport(dateFrom, dateTo), attendanceCoverageDataList);
            return attendanceCoverageDataList;
        }

        /// <summary>
        /// Get the attendance percentage for selected users in the period
        /// </summary>
        /// <param name="UserIDs">Comman seperated list of UserIDs</param>
        /// <param name="StartDate">From Date</param>
        /// <param name="EndDate">To Date</param>
        /// <returns></returns>
        //public double GetAttendancePercentage(string UserIDs ,DateTime StartDate ,DateTime EndDate )
        public double GetAttendancePercentage(DateTime StartDate, DateTime EndDate, long userId, int selectedRoleID, byte IncludeAllChild = 1)
        {
            return ReportRepository.GetAttendancePercentage(StartDate, EndDate, userId, selectedRoleID, IncludeAllChild);
        }


        /// <summary>
        /// Get the attendance report for selected users for the given period
        /// </summary>
        /// <param name="UserIDs">Comman seperated list of UserIDs</param>
        /// <param name="StartDate">From Date</param>
        /// <param name="EndDate">To Date</param>
        /// <returns></returns>
        public List<AttendanceExportBO> GetAttendanceExport(DateTime StartDate, DateTime EndDate, long userId, int selectedRoleID, byte IncludeAllChild = 1)
        {
            List<AttendanceExportBO> listAttendanceExportBO = new List<AttendanceExportBO>();
            ObjectMapper.Map(ReportRepository.GetAttendanceExport(StartDate, EndDate, userId, selectedRoleID, IncludeAllChild), listAttendanceExportBO);
            return listAttendanceExportBO;
        }

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
        public double GetCoveragePercentage(DateTime StartDate, DateTime EndDate, AspectEnums.CoverageType coverageType, AspectEnums.CoverageReportType coverageReportType, long userId, int selectedRoleID, byte IncludeAllChild = 1)
        {
            return ReportRepository.GetCoveragePercentage(StartDate, EndDate, coverageType, coverageReportType, userId, selectedRoleID, IncludeAllChild);
        }

        /// <summary>
        ///  This Procedure return Coverage Export Data for Excel download
        /// </summary>
        /// <param name="UserIDs">Comman seperated list of UserIDs</param>
        /// <param name="StartDate">From Date</param>
        /// <param name="EndDate">To Date</param>
        /// <param name="coverageType">Coverage Type (Norm,Plan, Unique) </param>
        /// <returns></returns>
        public List<CoverageExportBO> GetCoverageExport(DateTime StartDate, DateTime EndDate, AspectEnums.CoverageType coverageType, long userId, int selectedRoleID)
        {
            List<CoverageExportBO> listCoverageBO = new List<CoverageExportBO>();
            ObjectMapper.Map(ReportRepository.GetCoverageExport(StartDate, EndDate, coverageType, userId, selectedRoleID), listCoverageBO);
            return listCoverageBO;
        }
        #endregion


        #region Activity Report
        /// <summary>
        /// Function to bind the Questions for all Module
        /// </summary>
        /// <returns>Question List</returns>
        public IList<QuestionListBO> QuestionList()
        {
            IList<QuestionListBO> lstQuestion = new List<QuestionListBO>();
            ObjectMapper.Map(ReportRepository.GetQuestionList(), lstQuestion);
            return lstQuestion;
        }

        /// <summary>
        /// Function to bind the Questions for all Module
        /// </summary>
        /// <param name="moduleCode">The module code.</param>
        /// <returns>
        /// Question List
        /// </returns>
        public IList<QuestionListBO> QuestionList(int moduleCode)
        {
            IList<QuestionListBO> lstQuestion = new List<QuestionListBO>();
            ObjectMapper.Map(ReportRepository.GetQuestionList(moduleCode), lstQuestion);
            return lstQuestion;
        }

        /// <summary>
        /// Get Activity Report including Survey User Response & General User Response
        /// </summary>
        /// <param name="UserIDs">Comman Seperated User IDs</param>
        /// <param name="dtFrom">Starting date to filer</param>
        /// <param name="dtTo">To Date to Filter data</param>
        /// <param name="IsStoreWise">Storewise or Not (1=Store Wise ,0=Non Store wise)</param>
        /// <returns></returns>
        //public IList<ActivityReportBO> GetActivityReport(long UserID, DateTime dtFrom, DateTime dtTo, int selectedModule, bool IsStoreWise, bool IsWithImage, int roleId, int selectedRoleID)
        //{

        //    IList<ActivityReportBO> lstActivityData = new List<ActivityReportBO>();
        //    ObjectMapper.Map(ReportRepository.GetActivityReport(UserID, dtFrom, dtTo, selectedModule, IsStoreWise, IsWithImage, roleId,selectedRoleID), lstActivityData);
        //    return lstActivityData;

        //}

        public DataSet GetActivityReport(long UserID, DateTime dtFrom, DateTime dtTo, int selectedModule, bool IsStoreWise, bool IsWithImage, int roleId, int selectedRoleID)
        {

            return ReportRepository.GetActivityReport(UserID, dtFrom, dtTo, selectedModule, IsStoreWise, IsWithImage, roleId, selectedRoleID);

        }

        #region SDCE-1066
        /// <summary>
        /// This function will return all the questions along with its parent question text for any particular module
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <returns></returns>
        public IList<QuestionListBO> GetQuestionListWithParent(int moduleCode)
        {
            IList<QuestionListBO> lstQuestion = new List<QuestionListBO>();
            ObjectMapper.Map(ReportRepository.GetQuestionListWithParent(moduleCode), lstQuestion);
            return lstQuestion;

        }
        /// <summary>
        /// Function to find details of module based on module code
        /// Created By : Dhiraj on 06-Feb-2015 for activity Report
        /// </summary>
        /// <param name="moduleCode">The module code.</param>
        /// <param name="isMobile">To filter whether, mobile modules is required or web module</param>
        /// <returns>
        /// Question List
        /// </returns>
        public ModulesBO GetModuleDetails(int moduleCode, bool isMobile)
        {
            ModulesBO module = new ModulesBO();
            ObjectMapper.Map(ReportRepository.GetModuleDetails(moduleCode, isMobile), module);
            return module;
        }
        #endregion

        #endregion

        //VC20140814
        #region User Store Mapping Report
        /// <summary>
        /// Get User Store Mapping Report For and Employee
        /// </summary>
        /// <param name="EmployeeCode">EmployeeCode to filer mapping</param>        
        /// <returns></returns>
        public IList<UserStoreMappingBO> GetUserStoreMappingReport(string EmployeeCode)
        {

            IList<UserStoreMappingBO> lstUserStoreMappingData = new List<UserStoreMappingBO>();
            //ObjectMapper.Map(ReportRepository.GetActivityReport(UserIDs, dtFrom, dtTo, selectedModule, IsStoreWise, IsWithImage), lstActivityData);
            ObjectMapper.Map(ReportRepository.GetUserStoreMappingReport(EmployeeCode), lstUserStoreMappingData);
            return lstUserStoreMappingData;

        }

        #endregion
        //VC20140814


        #region Reports common Functions

        /// <summary>
        /// Return users under current role provided
        /// Created By :Dhiraj
        /// Created On :18-Dec-2013
        /// </summary>
        /// <param name="userId">UserId of Current Level</param>
        /// <param name="roleId">RoleId of Current Level</param>
        /// <param name="hierarchyLevelDepth">Difference of currentHierachy and totalHierachy of Current Level</param>
        /// <returns></returns>
        public List<UserRoleBO> GetEmployeesUnderHierarchy(long userId, int roleId, int? teamID, int? hierarchyLevelDepth = null, bool includeParent = false)
        {
            List<UserRoleBO> UserRolesBO = new List<UserRoleBO>();
            foreach (var item in ReportRepository.GetEmployeesUnderHierarchy(userId, roleId, teamID, hierarchyLevelDepth, includeParent))//To fetch only active record
            {

                UserRoleBO objUserRoleBo = new UserRoleBO();
                objUserRoleBo.UserRoleID = item.UserRoleID;
                objUserRoleBo.RoleID = item.RoleID;
                objUserRoleBo.UserID = item.UserID;
                objUserRoleBo.UserName = item.UserMaster != null ? item.UserMaster.FirstName : string.Empty;
                objUserRoleBo.IsActive = item.IsActive;
                objUserRoleBo.ProductTypeID = item.ProductTypeID;
                objUserRoleBo.ProductGroupID = item.ProductGroupID;
                objUserRoleBo.GeoID = item.GeoID;
                objUserRoleBo.GeoLevelValue = item.GeoLevelValue;
                objUserRoleBo.ReportingUserID = item.ReportingUserID;
                objUserRoleBo.CreatedDate = item.CreatedDate;
                objUserRoleBo.CreatedBy = item.CreatedBy;
                objUserRoleBo.ModifiedDate = item.ModifiedDate;
                objUserRoleBo.ModifiedBy = item.ModifiedBy;
                objUserRoleBo.IsDeleted = item.IsDeleted;
                UserRolesBO.Add(objUserRoleBo);
            }
            return UserRolesBO;
        }
        //public List<EmployeeHierarchyBO> GetEmployeesHierachyUnderUser(long userId, int roleId, int? teamID, int? maxHierarchyLevelDepth, int? selectedRoleID, bool includeParent, string extraParam)
        //{
        //    List<EmployeeHierarchyBO> employeesList = new List<EmployeeHierarchyBO>();
        //    ObjectMapper.Map(ReportRepository.GetEmployeesHierachyUnderUser(userId, roleId, teamID, maxHierarchyLevelDepth, selectedRoleID, includeParent, extraParam), employeesList);
        //    return employeesList;
        //}
        // Prepare Employee Seniors
        public List<EmployeeHierarchyBO> GetEmployeesHierachyUnderUser(long userId)
        {
            List<EmployeeHierarchyBO> employeesList = new List<EmployeeHierarchyBO>();
            ObjectMapper.Map(ReportRepository.GetEmployeesHierachyUnderUser(userId), employeesList);
            return employeesList;
        }
        /// <summary>
        /// This function will fetch all the senior of any particluar employee
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<UserRoleBO> GetSeniors(long userId)
        {
            List<UserRoleBO> users = new List<UserRoleBO>();
            foreach (var item in ReportRepository.GetSeniors(userId))
            {
                UserRoleBO objUserRoleBo = new UserRoleBO();
                objUserRoleBo.UserRoleID = item.UserRoleID;
                objUserRoleBo.RoleID = item.RoleID;
                objUserRoleBo.UserID = item.UserID;
                objUserRoleBo.UserName = item.UserMaster.FirstName;
                objUserRoleBo.IsActive = item.IsActive;
                objUserRoleBo.ProductTypeID = item.ProductTypeID;
                objUserRoleBo.ProductGroupID = item.ProductGroupID;
                objUserRoleBo.GeoID = item.GeoID;
                objUserRoleBo.GeoLevelValue = item.GeoLevelValue;
                objUserRoleBo.ReportingUserID = item.ReportingUserID;
                objUserRoleBo.CreatedDate = item.CreatedDate;
                objUserRoleBo.CreatedBy = item.CreatedBy;
                objUserRoleBo.ModifiedDate = item.ModifiedDate;
                objUserRoleBo.ModifiedBy = item.ModifiedBy;
                objUserRoleBo.IsDeleted = item.IsDeleted;
                users.Add(objUserRoleBo);

            }
            //ObjectMapper.Map(ReportRepository.GetSeniors(userId), users);
            return users;
        }
        /// <summary>
        /// Function to Get TeamLevel.
        /// </summary>       
        /// <param name="userID">userID</param>
        /// <returns>TeamLevel List</returns>
        public List<TeamLevelBO> GetTeamLevelBasedOnRoleID(long userID, int roleID, int? teamID, bool includeAllRoles = false)
        {
            List<TeamLevelBO> teamLevelList = new List<TeamLevelBO>();
            ObjectMapper.Map(ReportRepository.GetTeamLevelBasedOnRoleID(userID, roleID, teamID, includeAllRoles), teamLevelList);
            return teamLevelList;
        }

        #region SDCE-683 Bind Modify by Niranjan (Channel Type Mapping) 22-10-2014

        /// <summary>
        /// This function ChannelTypeTeamMappings Bind 
        /// </summary>
        /// <returns></returns>

        public List<ChannelTypeTeamMappingBO> Getselectedchannels(string SelectedTeam)
        {
            List<ChannelTypeTeamMappingBO> chnlst = new List<ChannelTypeTeamMappingBO>();
            ObjectMapper.Map(ReportRepository.Getselectedchannels(SelectedTeam), chnlst);
            return chnlst;
        }

        #endregion
        ///// <summary>
        ///// Function to Get Uper TeamLevel.
        ///// </summary>       
        ///// <param name="userID">userID</param>
        ///// <returns>TeamLevel List</returns>
        //public List<TeamLevelBO> GetUperTeamLevelBasedOnRoleID(long userID, int roleID, int? teamID)
        //{
        //    List<TeamLevelBO> teamLevelList = new List<TeamLevelBO>();
        //    ObjectMapper.Map(ReportRepository.GetUperTeamLevelBasedOnRoleID(userID, roleID, teamID), teamLevelList);
        //    return teamLevelList;
        //}

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
        public List<SurveryResponseModuleWiseBO> GetSurveyUserResponse(DateTime dtFrom, DateTime dtTo, int questionId, int moduleCode, bool includeAllQuestion = false, bool includeAllModules = false)
        {
            List<SurveryResponseModuleWiseBO> userSurveyResponseList = new List<SurveryResponseModuleWiseBO>();
            ObjectMapper.Map(ReportRepository.GetSurveyUserResponse(dtFrom, dtTo, questionId, moduleCode, includeAllQuestion, includeAllModules), userSurveyResponseList);
            return userSurveyResponseList;
        }
        /// <summary>
        /// Method to get Approved Covergae Plan based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns Coverage Plans for specified date range</returns>
        public IList<CoveragePlanBO> GetCoveragePlans(DateTime dtFrom, DateTime dtTo)
        {
            List<CoveragePlanBO> coveragePlanList = new List<CoveragePlanBO>();
            ObjectMapper.Map(ReportRepository.GetCoveragePlans(dtFrom, dtTo), coveragePlanList);
            return coveragePlanList;
        }
        /// <summary>
        /// Method to get All Covergae Plan based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns Coverage Plans for specified date range</returns>
        /// //Add status by tanuj(6-3-2014)
        public IList<CoverageBeatPlanBO> GetAllCoveragePlans(DateTime dtFrom, DateTime dtTo, int? status, long userId, int selectedRoleID, byte IncludeAllChild = 1)
        {
            List<CoverageBeatPlanBO> coveragePlanList = new List<CoverageBeatPlanBO>();
            if (status.Value == (int)AspectEnums.BeatStatus.Approved)
                ObjectMapper.Map(ReportRepository.GetAllCoveragePlans(dtFrom, dtTo, status, userId, selectedRoleID, IncludeAllChild), coveragePlanList);
            else
                ObjectMapper.Map(ReportRepository.GetPendingRejectedBeats(dtFrom, dtTo, (byte)status.Value, userId, selectedRoleID).ToList(), coveragePlanList);
            return coveragePlanList;
        }
        /// <summary>
        /// Method to get Survey Response based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns Survey Responses for specified date range</returns>
        public IList<SurveyResponseBO> GetSurveyResponses(DateTime dtFrom, DateTime dtTo, long currentUserId, int selectedRoleID)
        {
            List<SurveyResponseBO> surveyResponseList = new List<SurveyResponseBO>();
            ObjectMapper.Map(ReportRepository.GetSurveyResponses(dtFrom, dtTo, currentUserId, selectedRoleID), surveyResponseList);
            return surveyResponseList;
        }

        /// <summary>
        /// Method to get Collection Survey Response based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns Survey Responses for specified date range</returns>
        public IList<CollectionSurveyBO> GetCollectionSurveys(DateTime dtFrom, DateTime dtTo)
        {
            List<CollectionSurveyBO> CollectionSurveyist = new List<CollectionSurveyBO>();
            ObjectMapper.Map(ReportRepository.GetCollectionSurveys(dtFrom, dtTo), CollectionSurveyist);
            return CollectionSurveyist;
        }
        /// <summary>
        /// Method to get Geo Tag Data based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns list of store geo tagged for specified date range</returns>
        public IList<StoreGeoTagBO> GetStoreGeoTags(DateTime dtFrom, DateTime dtTo)
        {
            List<StoreGeoTagBO> storeGeoTagList = new List<StoreGeoTagBO>();
            ObjectMapper.Map(ReportRepository.GetStoreGeoTags(dtFrom, dtTo), storeGeoTagList);
            return storeGeoTagList;
        }
        /// <summary>
        /// Gets the store masters.
        /// </summary>
        /// <returns></returns>
        public IList<StoreBO> GetStoreMasters()
        {
            List<StoreBO> storeMasterList = new List<StoreBO>();
            ObjectMapper.Map(ReportRepository.GetStoreMasters(), storeMasterList);
            return storeMasterList;
        }
        /// <summary>
        /// Gets the store masters.
        /// </summary>
        /// <returns></returns>
        public IList<StoreBO> GetStoreMasters(List<long> users)
        {
            List<StoreBO> storeMasterList = new List<StoreBO>();
            ObjectMapper.Map(ReportRepository.GetStoreMasters(users), storeMasterList);
            return storeMasterList;
        }
        /// <summary>
        /// Method to fetch count stores from storemaster

        /// Created By : Shalu 
        /// Created On : 08-Apr-2014
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <param name="moduleCode">module code</param>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <returns>returns Total Stores</returns>
        public int GetStoreMastersCount()
        {

            return ReportRepository.GetStoreMastersCount();

        }


        public int GetStoreCountLevelwise(string region, int level, string branch, string accountname)
        {

            return ReportRepository.GetStoreCountLevelwise(region, level, branch, accountname);

        }
        /// <summary>
        /// Method to fetch outlet chart structures
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <param name="moduleCode">module code</param>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <returns>returns chart structure</returns>
        public IList<OutletChartStructureBO> GetUserChartOutlets(long userID, int moduleCode, DateTime startDate, DateTime endDate, string selectedResponse)
        {
            IList<OutletChartStructureBO> outlets = new List<OutletChartStructureBO>();
            ObjectMapper.Map(ReportRepository.GetUserChartOutlets(userID, moduleCode, startDate, endDate, selectedResponse), outlets);
            return outlets;
        }

        public List<StoreUserBO> GetStoreUsers(List<long> userIDs)
        {
            List<StoreUserBO> TotalCoverageList = new List<StoreUserBO>();
            ObjectMapper.Map(ReportRepository.GetStoreUsers(userIDs), TotalCoverageList);
            return TotalCoverageList;
        }
        public List<StoreUserBO> GetStoreUsers()
        {
            List<StoreUserBO> TotalCoverageList = new List<StoreUserBO>();
            ObjectMapper.Map(ReportRepository.GetStoreUsers(), TotalCoverageList);
            return TotalCoverageList;
        }
        public List<StoreUserBO> GetStoreUsersAll()
        {
            List<StoreUserBO> TotalCoverageList = new List<StoreUserBO>();
            ObjectMapper.Map(ReportRepository.GetStoreUsersAll(), TotalCoverageList);
            return TotalCoverageList;
        }
        public List<BranchBO> GetAllBranches()
        {
            List<BranchBO> TotalBranches = new List<BranchBO>();
            ObjectMapper.Map(ReportRepository.GetAllBranches(), TotalBranches);
            return TotalBranches;
        }

        #endregion






        /// <summary>
        /// Get userwise Geo data
        /// </summary>
        /// <returns></returns>
        public IList<GetUserGeoBO> GetUserGeo()
        {
            List<GetUserGeoBO> UserGeoList = new List<GetUserGeoBO>();
            ObjectMapper.Map(ReportRepository.GetUserGeo(), UserGeoList);
            return UserGeoList;
        }

        public List<FreezeGeoTagBO> GetFreezGeoTagData()
        {
            List<FreezeGeoTagBO> lstFreezeGeoTag = new List<FreezeGeoTagBO>();
            ObjectMapper.Map(ReportRepository.GetFreezGeoTagData(), lstFreezeGeoTag);
            return lstFreezeGeoTag;
        }

        #region For SDCE-871 (Get Display Share and Counter Share Response ) By Vaishali on 07 Nov 2014
        /// <summary>
        /// Get Display Share and Counter Share Response   
        /// </summary>
        /// <param name="compititionType"></param>
        /// <param name="reportLevel"></param>
        /// <param name="firstLevelParam"></param>
        /// <param name="secondLevelParam"></param>
        /// <param name="thirdLevelParam"></param>
        /// <param name="channels"></param>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<DisplayCounterShareResponseBO> SPGetDisplayCounterShareResponse(int compititionType, int reportLevel, string firstLevelParam, string secondLevelParam, string thirdLevelParam, string channels, long userid)
        {
            List<DisplayCounterShareResponseBO> lstDisplayCounterShareResponseData = new List<DisplayCounterShareResponseBO>();
            ObjectMapper.Map(ReportRepository.SPGetDisplayCounterShareResponse(compititionType, reportLevel, firstLevelParam, secondLevelParam, thirdLevelParam, channels, userid), lstDisplayCounterShareResponseData);
            return lstDisplayCounterShareResponseData;
        }
        public List<CompetitionSurveyResponseBO> SPGetDisplayCounterShareResponseExport(int compititionType, int reportLevel, string firstLevelParam, string secondLevelParam, string thirdLevelParam, string channels, long userid)
        {
            List<CompetitionSurveyResponseBO> lstDisplayCounterShareResponseData = new List<CompetitionSurveyResponseBO>();
            ObjectMapper.Map(ReportRepository.SPGetDisplayCounterShareResponseExport(compititionType, reportLevel, firstLevelParam, secondLevelParam, thirdLevelParam, channels, userid), lstDisplayCounterShareResponseData);
            return lstDisplayCounterShareResponseData;
        }
        #endregion

        #region Download VOC Data for SDCE-892
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
        public List<VOCDownloadBO> GetVOCSearch(DateTime DateFrom, DateTime DateTo, string BizDivision, string Region, string CustomerType, string ChannelCategory, string FeedbackType, string VOCClass, string Category, string SubCategory, string State, string VOCManagerName, string BizObjective, long userId)
        {
            List<VOCDownloadBO> lstResult = new List<VOCDownloadBO>();
            ObjectMapper.Map(ReportRepository.GetVOCSearch(DateFrom, DateTo, BizDivision, Region, CustomerType, ChannelCategory, FeedbackType, VOCClass, Category, SubCategory, State, VOCManagerName, BizObjective, userId).ToList(), lstResult);
            return lstResult;

        }
        #endregion
        /// <summary>
        /// This function will fetch senior of any user inside same team
        /// </summary>
        /// <param name="userID">user for whom senior needs to find</param>
        /// <param name="teamid">teamid of user for whom senior needs to find</param>
        /// <returns></returns>
        public List<EmployeeHierarchyBO> GetSeniorsDB(long userID, long teamid)
        {
            List<EmployeeHierarchyBO> lstResult = new List<EmployeeHierarchyBO>();
            ObjectMapper.Map(ReportRepository.GetSeniorsDB(userID, teamid), lstResult);
            return lstResult;
        }


        #region added by manonrajan for Race Product Audit report

        public List<RaceProductGroupBO> GetProductGroup()
        {
            List<RaceProductGroupBO> productGroupRaceProductMaster = new List<RaceProductGroupBO>();
            ObjectMapper.Map(ReportRepository.GetProductGroup(), productGroupRaceProductMaster);
            return productGroupRaceProductMaster;
        }

        public List<RaceReportModelWiseBO> GetModelWiseReport(long userID, string productGroup, DateTime strDateFrom, DateTime strDateTo)
        {
            List<RaceReportModelWiseBO> raceReportModelWiseBO = new List<RaceReportModelWiseBO>();
            //ObjectMapper.Map(ReportRepository.GetModelWiseReport(), raceReportModelWiseBO);
            ObjectMapper.Map(ReportRepository.GetModelWiseReport(userID,productGroup, strDateFrom, strDateTo), raceReportModelWiseBO);
            return raceReportModelWiseBO;

        }

      public   List<RaceProductModelBO> GetModelWiseJoin(string productGroup)
        {
            List<RaceProductModelBO> raceProductModelBO = new List<RaceProductModelBO>();
            ObjectMapper.Map(ReportRepository.GetModelWiseJoin(productGroup), raceProductModelBO);
            return raceProductModelBO;
        }
        /// <summary>
        /// Get New VOC Report
        /// </summary>
        /// <param name="storecode"></param>
        /// <returns></returns>
        /// 
      public List<VOCReportNewBO> GetNewVOCReport(string storecode, string strDateFrom, string strDateTo)
      {
          List<VOCReportNewBO> objVOCReportNewBO = new List<VOCReportNewBO>();
          ObjectMapper.Map(ReportRepository.GetNewVOCReport(storecode,strDateFrom,strDateTo), objVOCReportNewBO);
          return objVOCReportNewBO;
      }

        /// <summary>
        /// Get New VOC Report Second Format(Month Comparision)
        /// </summary>
        /// <param name="fromMonth"></param>
        /// <param name="toMonth"></param>
        /// <param name="fromYear"></param>
        /// <param name="toYear"></param>
        /// <returns></returns>
      public List<GetVOCReportBO> GetNewVOCReportMonthWise(int countMonthFrom, int countMonthTo, int fromMonth, int toMonth, int fromYear, int toYear, string strProductCategory, string strTypeOfPartner, string strPartnerCode, string strCityTier, string strRegion, string strState, string strCity)
      {
          List<GetVOCReportBO> objGetVOCReportBO = new List<GetVOCReportBO>();
          ObjectMapper.Map(ReportRepository.GetNewVOCReportMonthWise( countMonthFrom,countMonthTo, fromMonth, toMonth, fromYear, toYear,strProductCategory,strTypeOfPartner,strPartnerCode,strCityTier,strRegion,strState,strCity), objGetVOCReportBO);
          return objGetVOCReportBO;
      }

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
      public List<GetVOCTotalCountBO> GetSpGetVOCTotalCount( int fromMonth, int toMonth, int fromYear, int toYear, string strProductCategory, string strTypeOfPartner, string strPartnerCode, string strCityTier, string strRegion, string strState, string strCity)
      {
          List<GetVOCTotalCountBO> objGetVOCTotalCountBO = new List<GetVOCTotalCountBO>();
          ObjectMapper.Map(ReportRepository.GetSpGetVOCTotalCount( fromMonth, toMonth, fromYear, toYear, strProductCategory, strTypeOfPartner, strPartnerCode, strCityTier, strRegion, strState, strCity), objGetVOCTotalCountBO);
          return objGetVOCTotalCountBO;

      }

        /// <summary>
        /// Get VOC Open Ended Report (third format)
        /// </summary>
        /// <param name="productCategory"></param>
        /// <param name="fromMonth"></param>
        /// <param name="toMonth"></param>
        /// <param name="fromYear"></param>
        /// <param name="toYear"></param>
        /// <param name="Question"></param>
        /// <returns></returns>
      public List<VOCOpenEndedReportBO> GetVOCOpenEndedReport(string productCategory, DateTime fromDate, DateTime toDate, string Question)
      {
          List<VOCOpenEndedReportBO> objVOCOpenEndedReport = new List<VOCOpenEndedReportBO>();
          ObjectMapper.Map(ReportRepository.GetVOCOpenEndedReport( productCategory,fromDate,toDate, Question), objVOCOpenEndedReport);
          return objVOCOpenEndedReport;
      }

        /// <summary>
      /// Get VOC Dropdown filter for Sentiment Report 
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
      public List<NewVOCUserResponseBO> GetVOCSentimentFilter(string header)
      {
          List<NewVOCUserResponseBO> objNewVOCResponse = new List<NewVOCUserResponseBO>();
          ObjectMapper.Map(ReportRepository.GetVOCSentimentFilter(header), objNewVOCResponse);
          return objNewVOCResponse;
      }

        #endregion

        /// <summary>
        ///  Return SODetails 
        /// </summary>
        /// <param name="soNumber"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public DataSet GetSODetails(string soNumber, long userID)
        {
            return ReportRepository.GetSODetails(soNumber, userID);
        }
    }
}
