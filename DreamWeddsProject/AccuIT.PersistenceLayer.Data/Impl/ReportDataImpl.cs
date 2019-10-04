using Samsung.SmartDost.PersistenceLayer.Repository.Contracts;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using Samsung.SmartDost.CommonLayer.Aspects.Security;
using System.Linq;
using System.Data.Objects.SqlClient;
#region Namespace Added for Role Master :Dhiraj 3-Dec-2013
using System.Collections.Generic;
using System.Collections;
using System;
using System.Data.Objects;
using System.Data.SqlClient;
using System.Data;
using System.Web;
using System.Transactions;
using Samsung.SmartDost.CommonLayer.Aspects.ReportBO;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Samsung.SmartDost.PersistenceLayer.Data.EDMX;

#endregion

namespace Samsung.SmartDost.PersistenceLayer.Data.Impl
{
    public static class Extenders
    {
        public static int ToDecimal(this string str)
        {
            // you can throw an exception or return a default value here
            if (string.IsNullOrEmpty(str))
                return 0;

            int d;

            // you could throw an exception or return a default value on failure
            if (!int.TryParse(str, out d))
                return 0;

            return d;
        }
    }

    /// <summary>
    /// Class to define methods for company system settings and services
    /// </summary>
    public class ReportDataImpl : BaseDataImpl, IReportRepository
    {
        #region Global Variables

        int? MaxHierarchyLevelDepth = null;

        //List<EmployeeHierarchyBO> employeeList = HttpContext.Current.Session[SessionVariables.EmployeeListUnderCurrentUser] as List<EmployeeHierarchyBO>;
        //List<RoleMasterBO> roleMaster = HttpContext.Current.Session[SessionVariables.RoleMasters] as List<RoleMasterBO>;

        #endregion



        #region Attendence Report

        /// <summary>
        /// Method to get Attendance Data for excel
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <param name="dataFrom">date from</param>
        /// <param name="dateTo">date To</param>
        /// <returns>returns Attendence Date for Export based on profile</returns>
        public IEnumerable<AttendanceExcelData> GetAttendanceDataExport(long? userID, DateTime dateFrom, DateTime dateTo)
        {
            ObjectResult<GetAttendanceResult_Result> attendances = SmartDostDbContext.GetAttendanceResult(userID, dateFrom, dateTo);
            List<AttendanceExcelData> result = new List<AttendanceExcelData>();
            foreach (var item in attendances)
            {
                result.Add(new AttendanceExcelData()
                {
                    AccountStatus = item.AccountStatus,
                    AttenTime = item.AttenTime,
                    AttenType = item.AttenType,
                    Date = item.Date.Value,
                    LastLoginDate = item.LastLoginDate,
                    Mobile_Calling = item.Mobile_Calling,

                    UserCode = item.UserCode,
                    UserID = item.UserID,
                    UserName = item.UserName
                });
            }
            return result;
        }



        //Added by rajat 
        public List<CoverageExport> GetAttendanceAndCoverageDataExport(DateTime dateFrom, DateTime dateTo)
        {
            return SmartDostDbContext.SPGetAttendanceAndCoverageExport(dateFrom, dateTo).ToList();

        }

        /// <summary>
        /// Get the attendance percentage for selected users for the given period
        /// </summary>
        /// <param name="UserIDs">Comman seperated list of UserIDs</param>
        /// <param name="StartDate">From Date</param>
        /// <param name="EndDate">To Date</param>
        /// <returns></returns>
        //public double GetAttendancePercentage(string UserIDs, DateTime StartDate, DateTime EndDate)
        public double GetAttendancePercentage(DateTime StartDate, DateTime EndDate, long userId, int selectedRoleID, byte IncludeAllChild = 1)
        {
            using (var db = new SmartDostEntities())
            {
                return db.SpGetAttendancePercentage(StartDate, EndDate, userId, selectedRoleID, IncludeAllChild).SingleOrDefault() ?? 0;
            }
            //return SmartDostDbContext.SpGetAttendancePercentage(StartDate, EndDate, userId, selectedRoleID, IncludeAllChild).SingleOrDefault() ?? 0;
        }

        /// <summary>
        /// Get the attendance report for selected users for the given period
        /// </summary>
        /// <param name="UserIDs">Comman seperated list of UserIDs</param>
        /// <param name="StartDate">From Date</param>
        /// <param name="EndDate">To Date</param>
        /// <returns></returns>
        public List<SpGetAttendanceExport_Result> GetAttendanceExport(DateTime StartDate, DateTime EndDate, long userId, int selectedRoleID, byte IncludeAllChild = 1)
        {

            return SmartDostDbContext.SpGetAttendanceExport(StartDate, EndDate, userId, selectedRoleID, IncludeAllChild).ToList();
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
            using (var db = new SmartDostEntities())
            {
                return db.SPGetCoveragePercentage(StartDate, EndDate, (byte)coverageType, (byte)coverageReportType, userId, selectedRoleID, IncludeAllChild).SingleOrDefault() ?? 0;
            }
            //return SmartDostDbContext.SPGetCoveragePercentage(StartDate, EndDate, (byte)coverageType, (byte)coverageReportType, userId, selectedRoleID, IncludeAllChild).SingleOrDefault() ?? 0;
        }

        /// <summary>
        ///  This Procedure return Coverage Export Data for Excel download
        /// </summary>
        /// <param name="UserIDs">Comman seperated list of UserIDs</param>
        /// <param name="StartDate">From Date</param>
        /// <param name="EndDate">To Date</param>
        /// <param name="coverageType">Coverage Type (Norm,Plan, Unique) </param>
        /// <returns></returns>
        public List<SPGetCoverageExport_Result> GetCoverageExport(DateTime StartDate, DateTime EndDate, AspectEnums.CoverageType coverageType, long userId, int selectedRoleID, byte IncludeAllChild = 1)
        {
            return SmartDostDbContext.SPGetCoverageExport(StartDate, EndDate, null, userId, selectedRoleID, IncludeAllChild).ToList();
        }
        #endregion

        #region Activity Report
        /// <summary>
        /// Function to bind the Questions for all Modules
        /// </summary>
        /// <returns>Question List</returns>
        public IList<SurveyQuestion> GetQuestionList()
        {
            var lstQuestionlst = new List<SurveyQuestion>();
            lstQuestionlst = SmartDostDbContext.SurveyQuestions.Where(k => !k.IsDeleted).OrderBy(k => k.Sequence).ToList();
            return lstQuestionlst;
        }
        /// <summary>
        /// Function to bind the Questions for all Modules
        /// </summary>
        /// <param name="moduleCode">The module code.</param>
        /// <returns>
        /// Question List
        /// </returns>
        public IList<SurveyQuestion> GetQuestionList(int moduleCode)
        {
            var lstQuestionlst = new List<SurveyQuestion>();
            var module = SmartDostDbContext.Modules.Where(x => x.ModuleCode == moduleCode && x.IsDeleted == false).FirstOrDefault();
            if (module != null)
                lstQuestionlst = SmartDostDbContext.SurveyQuestions.Where(k => k.ModuleID == module.ModuleID && !k.IsDeleted).OrderBy(k => k.CreatedDate).ToList();
            return lstQuestionlst;
        }
        #region SDCE-1066
        /// <summary>
        /// This function will return all the questions along with its parent question text for any particular module
        /// </summary>
        /// <param name="moduleCode"></param>
        /// <returns></returns>
        public IList<SurveyQuestion> GetQuestionListWithParent(int moduleCode)
        {
            var lstQuestionlst = new List<SurveyQuestion>();
            lstQuestionlst = SmartDostDbContext.GetQuestionsBasedOnModuleCode(moduleCode).ToList();
            return lstQuestionlst;
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
        public Module GetModuleDetails(int moduleCode, bool isMobile)
        {
            return SmartDostDbContext.Modules.FirstOrDefault(x => x.ModuleCode == moduleCode && !x.IsDeleted && x.IsMobile == isMobile);
        }
        #endregion
        #endregion

        #region SDCE-683  Bind by Modifiy Niranjan (Channel Type Mapping) 20-10-2014
        /// <summary>
        /// This function ChannelTypeTeamMappings Bind by Modify Niranjan (Channel Type Mapping) 22-10-2014 
        /// </summary>
        /// <returns></returns>
        /// 

        public List<ChannelTypeTeamMapping> Getselectedchannels(string SelectedTeam)
        {
            List<ChannelTypeTeamMapping> channelTypeDisplayteam = new List<ChannelTypeTeamMapping>();
            //List<ChannelMaster> lstchnl = SmartDostDbContext.ChannelMasters.Where(x => x.IsForExclusion == false && !string.IsNullOrEmpty(x.Code)).ToList();
            if (!string.IsNullOrEmpty(SelectedTeam))
            {
                //return SmartDostDbContext.ChannelTypeTeamMappings.Where(x => x.IsDeleted == false && !string.IsNullOrEmpty(x.ChannelType) && x.Team == SelectedTeam).ToList();
                channelTypeDisplayteam = (from CTM in SmartDostDbContext.ChannelTypeTeamMappings
                                          join CTD in SmartDostDbContext.ChannelTypeDisplays on CTM.ChannelType equals CTD.ChannelType
                                          where CTM.IsDeleted == false && !string.IsNullOrEmpty(CTM.ChannelType) && CTD.IsDisplayCounterShare == true && CTM.Team == SelectedTeam
                                          select CTM).ToList();
            }
            else
            {
                // return SmartDostDbContext.ChannelTypeTeamMappings.Where(x => x.IsDeleted == false && !string.IsNullOrEmpty(x.ChannelType)).ToList();
                channelTypeDisplayteam = (from CTM in SmartDostDbContext.ChannelTypeTeamMappings
                                          join CTD in SmartDostDbContext.ChannelTypeDisplays on CTM.ChannelType equals CTD.ChannelType
                                          where CTM.IsDeleted == false && !string.IsNullOrEmpty(CTM.ChannelType) && CTD.IsDisplayCounterShare == true
                                          select CTM).ToList();
            }
            return channelTypeDisplayteam;


        }

        //public List<ChannelTypeTeamMapping> Getselectedchannels()
        //{
        //    //List<ChannelMaster> lstchnl = SmartDostDbContext.ChannelMasters.Where(x => x.IsForExclusion == false && !string.IsNullOrEmpty(x.Code)).ToList();
        //    List<ChannelTypeTeamMapping> lstchnl = SmartDostDbContext.ChannelTypeTeamMappings.Where(x => x.IsDeleted == false && !string.IsNullOrEmpty(x.ChannelType)).ToList();
        //    return lstchnl;
        //}

        #endregion


        #region Common Functions For Report

        /// <summary>
        /// Function to Get TeamLevel.
        /// </summary>       
        /// <param name="userID">userID</param>
        /// <returns>TeamLevel List</returns>
        public List<TeamLevel> GetTeamLevelBasedOnRoleID(long userID, int roleID, int? teamID, bool includeAllRoles = false)
        {
            // List<RoleMasterBO> roleMaster = HttpContext.Current.Session[SessionVariables.RoleMasters] as List<RoleMasterBO>;
            //var distinctRoles = GetEmployeesHierachyUnderUser(
            //                                userId: userID,
            //                                roleId: roleID,
            //                                teamID: teamID,
            //                                maxHierarchyLevelDepth: null,
            //                                selectedRoleID: null,
            //                                includeParent: true,
            //                                extraParam: "").GroupBy
            //                                (x => new
            //                                {
            //                                    RoleID = (int)x.RoleID,
            //                                    TeamCode = x.RoleCode,
            //                                    ProfileLevelCount = (int)x.ProfileLevel,
            //                                    RoleCode = x.RoleCode
            //                                })
            //                                .Select(x => new TeamLevel
            //                                {
            //                                    RoleID = x.Key.RoleID,
            //                                    TeamCode = x.Key.TeamCode,
            //                                    ProfileLevelCount = x.Key.ProfileLevelCount,
            //                                    RoleCode = x.Key.RoleCode
            //                                }).OrderBy(x => x.RoleCode).ToList();

            //#region Effective Role ID Check
            ////Created by Dhiraj on 13-May-2014
            //if (includeAllRoles == false)
            //{
            //    List<int> effectiveRoleID = new List<int>();
            //    List<RoleMasterBO> ERD = roleMaster.Where(k => k.IsEffectiveProfile == true).ToList();
            //    foreach (var item in ERD)
            //    {
            //        effectiveRoleID.Add(item.RoleID);

            //    }

            //    distinctRoles = (from d in distinctRoles
            //                     join r in effectiveRoleID
            //                     on d.RoleID equals r
            //                     select d).ToList();
            //}
            //#endregion
            //return distinctRoles;


            if (HttpContext.Current.Session[SessionVariables.DistintRoles] == null)
            {

                HttpContext.Current.Session[SessionVariables.DistintRoles] = (from x in SmartDostDbContext.EmployeeJuniors
                                                                              where x.RootUserID == userID
                                                                              group x by
                                                                              new
                                                                              {
                                                                                  RoleID = (int)x.RoleID,
                                                                                  TeamCode = x.RoleCode,
                                                                                  ProfileLevelCount = (int)x.ProfileLevel,
                                                                                  RoleCode = x.RoleCode
                                                                              }
                                                                                  into g
                                                                                  select new TeamLevel
                                                                                  {
                                                                                      RoleID = g.Key.RoleID,
                                                                                      TeamCode = g.Key.TeamCode,
                                                                                      ProfileLevelCount = g.Key.ProfileLevelCount,
                                                                                      RoleCode = g.Key.RoleCode
                                                                                  }).OrderBy(x => x.RoleCode).ToList();

            }
            List<TeamLevel> distinctRoles = HttpContext.Current.Session[SessionVariables.DistintRoles] as List<TeamLevel>;
            #region Effective Role ID Check
            //Created by Dhiraj on 13-May-2014
            if (includeAllRoles == false)
            {
                //List<int> effectiveRoleID = new List<int>();
                //List<RoleMasterBO> ERD = roleMaster.Where(k => k.IsEffectiveProfile == true).ToList();
                //foreach (var item in ERD)
                //{
                //    effectiveRoleID.Add(item.RoleID);

                //}

                //distinctRoles = (from d in distinctRoles
                //                 join r in effectiveRoleID
                //                 on d.RoleID equals r
                //                 select d).ToList();

                List<RoleMasterBO> roleMaster = HttpContext.Current.Session[SessionVariables.RoleMasters] as List<RoleMasterBO>;

                // By Amit
                distinctRoles = (from d in distinctRoles
                                 join r in roleMaster
                                 on d.RoleID equals r.RoleID
                                 where r.IsEffectiveProfile
                                 select d).ToList();

            }
            #endregion
            return distinctRoles;



            /* Not in USE
            //var roles = from RoleMaster in SmartDostDbContext.RoleMasters.Where(x => x.IsDeleted == false && x.IsActive) select RoleMaster;
            //var query = from RoleMaster in roles
            //            where
            //              RoleMaster.RoleID == roleID
            //            select new
            //            {
            //                RoleMaster.ProfileLevel,
            //                RoleMaster.TeamID
            //            };

            //List<TeamLevel> result = new List<TeamLevel>();
            //if (teamID != null)
            //{

            //    var res = (from RoleMaster in roles
            //               where
            //                (RoleMaster.RoleID == roleID || RoleMaster.TeamID == teamID)

            //               select new
            //               {
            //                   RoleMaster.RoleID,
            //                   RoleMaster.Code,
            //                   RoleMaster.ProfileLevel
            //               }).OrderBy(x => x.ProfileLevel).ToList();



            //    //var record = from element in res
            //    //             group element by element.ProfileLevel
            //    //                 into groups
            //    //                 select groups.OrderBy(p => p.ProfileLevel).FirstOrDefault();

            //    int count = res.Count() + 1;
            //    foreach (var item in res)
            //    {
            //        result.Add(new TeamLevel { RoleID = item.RoleID, TeamCode = item.Code, ProfileLevelCount = count - (item.ProfileLevel) });
            //    }
            //}
            //else
            //{
            //    var res = (from RoleMaster in roles
            //               join x in query on RoleMaster.TeamID equals x.TeamID
            //               where
            //                 RoleMaster.ProfileLevel >= x.ProfileLevel

            //               select new
            //               {
            //                   RoleMaster.RoleID,
            //                   RoleMaster.Code,
            //                   RoleMaster.ProfileLevel
            //               }).OrderBy(x => x.ProfileLevel).ToList();


            //    int count = res.Count() + 1;
            //    foreach (var item in res)
            //    {
            //        result.Add(new TeamLevel { RoleID = item.RoleID, TeamCode = item.Code, ProfileLevelCount = count - (item.ProfileLevel) });
            //    }
            //}
            // return result;
             */
        }

        ///// <summary>
        ///// Function to Get UperTeamLevel.
        ///// </summary>       
        ///// <param name="userID">userID</param>
        ///// <returns>TeamLevel List</returns>
        //public List<TeamLevel> GetUperTeamLevelBasedOnRoleID(long userID, int roleID, int? teamID)
        //{
        //    //var distinctRoles = GetEmployeesHierachyUnderUser(
        //    //                               userId: userID,
        //    //                               roleId: roleID,
        //    //                               teamID: teamID,
        //    //                               maxHierarchyLevelDepth: null,
        //    //                               selectedRoleID: null,
        //    //                               includeParent: true,
        //    //                               extraParam: "").GroupBy
        //    //                               (x => new
        //    //                               {
        //    //                                   RoleID = (int)x.RoleID,
        //    //                                   TeamCode = x.RoleCode,
        //    //                                   ProfileLevelCount = (int)x.ProfileLevel
        //    //                               })
        //    //                               .Select(x => new TeamLevel
        //    //                               {
        //    //                                   RoleID = x.Key.RoleID,
        //    //                                   TeamCode = x.Key.TeamCode,
        //    //                                   ProfileLevelCount = x.Key.ProfileLevelCount
        //    //                               }).ToList();
        //    //return distinctRoles.OrderByDescending(x => x.ProfileLevelCount).ToList();

        //    List<TeamLevel> distinctRoles=(from x in SmartDostDbContext.EmployeeJuniors
        //                         where x.RootUserID == userID && x.TeamID == teamID
        //                         group x by
        //                         new { RoleID = (int)x.RoleID, TeamCode = x.RoleCode, ProfileLevelCount = (int)x.ProfileLevel }
        //                             into g
        //                             select new TeamLevel
        //                                                     {
        //                                                         RoleID = g.Key.RoleID,
        //                                                         TeamCode = g.Key.TeamCode,
        //                                                         ProfileLevelCount = g.Key.ProfileLevelCount
        //                                                     }).OrderByDescending(x => x.ProfileLevelCount).ToList();

        //    return distinctRoles;

        //    //var roles = from RoleMaster in SmartDostDbContext.RoleMasters.Where(x => x.IsDeleted == false && x.IsActive) select RoleMaster;
        //    //var query = from RoleMaster in roles
        //    //            where
        //    //              RoleMaster.RoleID == roleID
        //    //            select new
        //    //            {
        //    //                RoleMaster.ProfileLevel,
        //    //                RoleMaster.TeamID
        //    //            };

        //    //List<TeamLevel> result = new List<TeamLevel>();
        //    //if (teamID != null)
        //    //{

        //    //    var res = (from RoleMaster in roles
        //    //               where
        //    //                (RoleMaster.RoleID == roleID || RoleMaster.TeamID == teamID)

        //    //               select new
        //    //               {
        //    //                   RoleMaster.RoleID,
        //    //                   RoleMaster.Code,
        //    //                   RoleMaster.ProfileLevel
        //    //               }).OrderBy(x => x.ProfileLevel).ToList();



        //    //    //var record = from element in res
        //    //    //             group element by element.ProfileLevel
        //    //    //                 into groups
        //    //    //                 select groups.OrderBy(p => p.ProfileLevel).FirstOrDefault();

        //    //    int count = res.Count() + 1;
        //    //    foreach (var item in res)
        //    //    {
        //    //        result.Add(new TeamLevel { RoleID = item.RoleID, TeamCode = item.Code, ProfileLevelCount = count - (item.ProfileLevel) });
        //    //    }
        //    //}
        //    //else
        //    //{
        //    //    var res = (from RoleMaster in roles
        //    //               join x in query on RoleMaster.TeamID equals x.TeamID
        //    //               where
        //    //                 RoleMaster.ProfileLevel >= x.ProfileLevel

        //    //               select new
        //    //               {
        //    //                   RoleMaster.RoleID,
        //    //                   RoleMaster.Code,
        //    //                   RoleMaster.ProfileLevel
        //    //               }).OrderBy(x => x.ProfileLevel).ToList();


        //    //    int count = res.Count() + 1;
        //    //    foreach (var item in res)
        //    //    {
        //    //        result.Add(new TeamLevel { RoleID = item.RoleID, TeamCode = item.Code, ProfileLevelCount = count - (item.ProfileLevel) });
        //    //    }
        //    //}
        //    //return result;
        //}
        //public List<GetEmployeeHierarchy_Result> GetEmployeesHierachyUnderUser(long userId, int roleId, int? teamID, int? maxHierarchyLevelDepth, int? selectedRoleID, bool includeParent, string extraParam)
        //{
        //    return SmartDostDbContext.GetEmployeeHierarchy_Result(teamID, selectedRoleID, userId, roleId, maxHierarchyLevelDepth, includeParent, extraParam).ToList();
        //}

        /// <summary>
        /// New function to get the Employee hierarchy based on prepared data from SQL Job
        /// </summary>
        /// <param name="userId">Userid of the person who's hierarchy to be generated</param>
        /// <returns>List of All employee who reports to the user direcly or indirectly</returns>
        public List<EmployeeJunior> GetEmployeesHierachyUnderUser(long userId)
        {
            return SmartDostDbContext.EmployeeJuniors.Where(k => k.RootUserID == userId).ToList();
        }

        /// <summary>
        /// Return users under current role provided
        /// Created By :Dhiraj
        /// Created On :18-Dec-2013
        /// </summary>
        /// <param name="userId">UserId of Current Level</param>
        /// <param name="roleId">RoleId of Current Level</param>
        /// <param name="maxHierarchyLevelDepth">Fetch hierarchy upto this level including first level</param>
        /// <returns></returns>
        public List<UserRole> GetEmployeesUnderHierarchy(long userId, int roleId, int? teamID, int? maxHierarchyLevelDepth = null, bool includeParent = false)
        {
            this.MaxHierarchyLevelDepth = maxHierarchyLevelDepth;
            List<UserRole> users = new List<UserRole>();
            using (var t = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                var userList = SmartDostDbContext.UserRoles.Where(x => x.IsDeleted == false && x.IsActive);
                var reportingUser = userList.Where(x => x.UserID == userId && x.RoleID == roleId).FirstOrDefault();

                if (reportingUser != null && includeParent)
                {
                    //if (reportingUser.UserID == reportingUser.ReportingUserID)
                    //    reportingUser.ReportingUserID = new Nullable<long>();
                    users.Add(reportingUser);

                }
                //users.AddRange(BuildRoles(userList.Where(x => x.IsDeleted == false), userId, teamID ?? (int)reportingUser.RoleMaster.TeamID, 0).ToList());
                users.AddRange(BuildRoles(userList.Where(x => x.IsDeleted == false), userId, 0, 0).ToList());
                t.Complete();
            }
            return users;
        }

        /// <summary>
        /// Return users under current role provided
        /// Created By :Dhiraj
        /// Created On :18-Dec-2013
        /// </summary>
        /// <param name="userId">UserId of Current Level</param>
        /// <param name="roleId">RoleId of Current Level</param>
        /// <param name="maxHierarchyLevelDepth">Fetch hierarchy upto this level including first level</param>
        /// <returns></returns>
        public List<UserRole> GetSeniors(long userId)
        {

            List<UserRole> users = new List<UserRole>();
            var userRoles = SmartDostDbContext.UserRoles.Where(x => x.IsDeleted == false && x.IsActive);
            var reportingId = userId as long?;

            do
            {
                var person = userRoles.FirstOrDefault(p => p.UserID == reportingId);
                users.Add(person);
                reportingId = person.ReportingUserID;
                if (person.UserID == reportingId)
                    reportingId = null;
            } while (reportingId != null);
            return users;
        }

        /// <summary>
        /// Return users under current role provided
        /// Created By :Dhiraj
        /// Created On :18-Dec-2013
        /// </summary>
        /// <param name="userId">UserId of Current Level</param>
        /// <param name="roleId">RoleId of Current Level</param>
        /// <param name="maxHierarchyLevelDepth">Fetch hierarchy upto this level including first level</param>
        /// <returns></returns>
        private IEnumerable<UserRole> BuildRoles(IEnumerable<UserRole> allRoles, long? parentId, int teamId, int hierarchyLevelDepth)
        {

            var RoleTree = new List<UserRole>();
            if (MaxHierarchyLevelDepth == null || hierarchyLevelDepth < MaxHierarchyLevelDepth)
            {

                //var childRoles = allRoles.Where(o => o.ReportingUserID == parentId && o.RoleMaster.TeamID == teamId);
                var childRoles = allRoles.Where(o => o.ReportingUserID == parentId);
                if (childRoles != null)
                {
                    foreach (var Role in childRoles.ToList())
                    {

                        RoleTree.Add(Role);
                        if (Role.UserID == Role.ReportingUserID)
                            continue;
                        else
                            parentId = Role.UserID;
                        var children = BuildRoles(allRoles, parentId, 0, hierarchyLevelDepth + 1);
                        foreach (var item in children)
                        {
                            RoleTree.Add(item);
                        }
                    }
                }
            }
            return RoleTree;
        }

        #region GetSurveyUserResponse
        /// <summary>
        /// Return Survey User Response for particular question, and module code
        /// Created By :Dhiraj
        /// Created On :19-Dec-2013
        /// </summary>
        /// <param name="userId">UserId of Current Level</param>
        /// <param name="roleId">RoleId of Current Level</param>
        /// <param name="hierarchyLevelDepth">Difference of currentHierachy and totalHierachy of Current Level</param>
        /// <returns></returns>
        public List<vwSurveryResponseModuleWise> GetSurveyUserResponse(DateTime dtFrom, DateTime dtTo, int questionId, int moduleCode, bool includeAllQuestion = false, bool includeAllModules = false)
        {
            return SmartDostDbContext.SPGetSurveyUserResponse(dtFrom.Date, dtTo.Date).ToList<vwSurveryResponseModuleWise>().Where(x => (includeAllQuestion == true || x.SurveyQuestionID == questionId) &&
                (includeAllModules == true || x.ModuleCode == moduleCode)

                ).ToList();
            //.Where(x => (includeAllQuestion == true || x.SurveyQuestionID == questionId) &&
            //(includeAllModules == true || x.ModuleCode == moduleCode)
            //&& (EntityFunctions.TruncateTime(x.CreatedDate)
            //                                      >= EntityFunctions.TruncateTime(dtFrom)
            //                                      && EntityFunctions.TruncateTime(x.CreatedDate)
            //                                      <= EntityFunctions.TruncateTime(dtTo))
            //);

            //return SmartDostDbContext.vwSurveryResponseModuleWises.Where(x => (includeAllQuestion == true || x.SurveyQuestionID == questionId) &&
            //    (includeAllModules == true || x.ModuleCode == moduleCode)
            //    && (EntityFunctions.TruncateTime(x.CreatedDate)
            //                                          >= EntityFunctions.TruncateTime(dtFrom)
            //                                          && EntityFunctions.TruncateTime(x.CreatedDate)
            //                                          <= EntityFunctions.TruncateTime(dtTo))
            //    );
        }
        #endregion
        /// <summary>
        /// This function will fetch competition survey response based on given date range
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To</param>
        /// <returns>Collection of competition survey data based on given date range</returns>
        public List<vwCompetitionSurveyResponse> GetCompetitionSurveyRespone(DateTime dtFrom, DateTime dtTo)
        {
            return SmartDostDbContext.SPGetCompetitionResponse(dtFrom.Date, dtTo.Date).ToList();


        }
        ///// <summary>
        ///// This function will last response store wise for competition survey response based on given date 
        /////  Change By Amit Mishra (AM_16102014): Get data from table CompetitionSurveyResponseData instead of SP
        ///// </summary>        
        ///// <param name="dtTo">Date To</param>
        ///// <returns>Collection of competition survey data based on given date </returns>
        //public List<CompetitionSurveyResponseData> GetCompetitionSurveyRespone(DateTime dtTo, int compType, string channelID)
        //{

        //    //return SmartDostDbContext.SPCompetitionSurveyResponse(dtTo, compType).ToList();
        //    return SmartDostDbContext.CompetitionSurveyResponseDatas.Where(k => k.CompetitionType == (byte)compType && k.SurveyDate <= EntityFunctions.TruncateTime(dtTo)).ToList();

        //}

        /// <summary>
        /// This function will last response store wise for competition survey response based on given date from staging table CompetitionSurveyResponseData 
        /// Added By Amit Mishra (16 Oct 2014) ::AM1610
        /// </summary>        
        /// <returns>Collection of competition survey data </returns>
        public List<SPGetCompetitionSurveyResponseData_Result> GetCompetitionSurveyResponse()
        {
            //return SmartDostDbContext.CompetitionSurveyResponseDatas.ToList();
            return SmartDostDbContext.SPGetCompetitionSurveyResponseData().ToList();
        }

        /// <summary>
        /// Return Survey User Response for particular  module code
        /// Created By :Dhiraj
        /// Created On :19-Dec-2013
        /// </summary>
        /// <param name="userId">UserId of Current Level</param>
        /// <param name="roleId">RoleId of Current Level</param>
        /// <param name="hierarchyLevelDepth">Difference of currentHierachy and totalHierachy of Current Level</param>
        /// <returns></returns>
        public IEnumerable<vwSurveryResponseModuleWise> GetSurveyUserResponse(DateTime dtFrom, DateTime dtTo, int moduleCode)
        {
            return SmartDostDbContext.vwSurveryResponseModuleWises.Where(x => x.ModuleCode == moduleCode
                && (EntityFunctions.TruncateTime(x.CreatedDate)
                                                      >= EntityFunctions.TruncateTime(dtFrom)
                                                      && EntityFunctions.TruncateTime(x.CreatedDate)
                                                      <= EntityFunctions.TruncateTime(dtTo))
                );
        }

        ///// <summary>
        ///// Get Activity Report including Survey User Response & General User Response
        ///// </summary>
        ///// <param name="UserIDs">Comman Seperated User IDs</param>
        ///// <param name="dtFrom">Starting date to filer</param>
        ///// <param name="dtTo">To Date to Filter data</param>
        ///// <param name="IsStoreWise">Storewise or Not (1=Store Wise ,0=Non Store wise)</param>
        ///// <returns></returns>
        //public List<SPGetActivityReport_Result> GetActivityReport(string UserIDs, DateTime dtFrom, DateTime dtTo, int selectedModule, bool IsStoreWise, bool IsWithImage, int roleId)
        //{
        //    return SmartDostDbContext.SPGetActivityReport(UserIDs, dtFrom, dtTo, IsStoreWise, selectedModule, "", IsWithImage, roleId).ToList();
        //}

        /// <summary>
        /// Get Activity Report including Survey User Response & General User Response
        /// </summary>
        /// <param name="UserIDs">Comman Seperated User IDs</param>
        /// <param name="dtFrom">Starting date to filer</param>
        /// <param name="dtTo">To Date to Filter data</param>
        /// <param name="IsStoreWise">Storewise or Not (1=Store Wise ,0=Non Store wise)</param>
        /// <returns></returns>
        public DataSet GetActivityReport(long UserID, DateTime dtFrom, DateTime dtTo, int selectedModule, bool IsStoreWise, bool IsWithImage, int roleId, int selectedRoleID)
        {

            //public List<SPGetActivityReport_Result> GetActivityReport(long UserID, DateTime dtFrom, DateTime dtTo, int selectedModule, bool IsStoreWise, bool IsWithImage, int roleId,int selectedRoleID)
            //{



            List<SqlParameter> lstparam = new List<SqlParameter>();
            lstparam.Add(new SqlParameter("@DateFrom", dtFrom));
            lstparam.Add(new SqlParameter("@DateTo", dtTo));
            lstparam.Add(new SqlParameter("@IsStoreWise", IsStoreWise));
            lstparam.Add(new SqlParameter("@selectedModule", selectedModule));
            lstparam.Add(new SqlParameter("@ExtraFlag", ""));
            lstparam.Add(new SqlParameter("@IsWithImage", IsWithImage));
            lstparam.Add(new SqlParameter("@RoleId", roleId));
            lstparam.Add(new SqlParameter("@UserID", UserID));
            lstparam.Add(new SqlParameter("@SelectedRoleID", selectedRoleID));


            return ExecuteProcedure(StoredProcedureVariables.ActivityReport, lstparam);
        }

        //VC20140814
        /// <summary>
        /// Get User Store Mapping Report
        /// </summary>
        /// <param name="UserIDs">Employee Code to Fileter Mapping for Particular Employee</param>        
        /// <returns></returns>
        public List<SPUserStoreMapping_Result> GetUserStoreMappingReport(string EmployeeCode)
        {
            return SmartDostDbContext.SPUserStoreMapping(EmployeeCode).ToList();
        }
        //VC20140814

        /// <summary>
        /// Method to get approved Covergae Plan based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns Coverage Plans for specified date range</returns>
        public IEnumerable<CoveragePlan> GetCoveragePlans(DateTime dtFrom, DateTime dtTo)
        {
            return SmartDostDbContext.CoveragePlans.Where(x => (EntityFunctions.TruncateTime(x.CoverageDate)
                                                       >= EntityFunctions.TruncateTime(dtFrom)
                                                       && EntityFunctions.TruncateTime(x.CoverageDate)
                                                       <= EntityFunctions.TruncateTime(dtTo)
                                                       && x.StatusID == 1)
                 );
        }
        /// <summary>
        /// Method to get Covergae Plan based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns Coverage Plans for specified date range</returns>
        /// Add status by tanuj(6-3-2014)
        public IEnumerable<SPGetBeatCoveragePlan_Result> GetAllCoveragePlans(DateTime dtFrom, DateTime dtTo, int? status, long userId, int selectedRoleID, byte IncludeAllChild = 1)
        {
            byte Status = (byte)status;
            return SmartDostDbContext.SPGetBeatCoveragePlan(dtFrom, dtTo, Status, userId, selectedRoleID, IncludeAllChild).ToList();//.Where(x => (EntityFunctions.TruncateTime(x.CoverageDate)
        }


        public IEnumerable<SPGetPendingRejectedBeats_Result> GetPendingRejectedBeats(DateTime dtFrom, DateTime dtTo, byte status, long userId, int selectedRoleID)
        {

            return SmartDostDbContext.SPGetPendingRejectedBeats(dtFrom, dtTo, status, userId, selectedRoleID);
        }

        /// <summary>
        /// Method to get Survey Response based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns Survey Responses for specified date range</returns>
        public IEnumerable<SurveyResponse> GetSurveyResponses(DateTime dtFrom, DateTime dtTo, long currentUserId, int selectedRoleID)
        {
            DateTime dtNextDate = dtTo.AddDays(1).Date;
            IEnumerable<SurveyResponse> result = null;
            using (var t = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                result = (from s in SmartDostDbContext.SurveyResponses
                          join e in SmartDostDbContext.EmployeeJuniors
                          on s.UserID equals e.UserID
                          where !s.IsDeleted
                             && e.RootUserID == currentUserId
                             && e.RoleID == selectedRoleID
                             && s.CreatedDate >= dtFrom
                             && s.CreatedDate < dtNextDate
                          select s).Distinct();
                t.Complete();
            }
            return result;


            //return SmartDostDbContext.SurveyResponses.Where(x => (EntityFunctions.TruncateTime(x.CreatedDate)
            //                                           >= EntityFunctions.TruncateTime(dtFrom)
            //                                           && EntityFunctions.TruncateTime(x.CreatedDate)
            //                                           <= EntityFunctions.TruncateTime(dtTo)
            //                                           && !x.IsDeleted
            //                                           && x.UserID == currentUserId
            //                                           )
            //     );
        }

        /// <summary>
        /// Method to get Collection Survey Response based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns Survey Responses for specified date range</returns>
        public IEnumerable<CollectionSurvey> GetCollectionSurveys(DateTime dtFrom, DateTime dtTo)
        {
            return SmartDostDbContext.CollectionSurveys.Where(x => (EntityFunctions.TruncateTime(x.CreatedDate)
                                                       >= EntityFunctions.TruncateTime(dtFrom)
                                                       && EntityFunctions.TruncateTime(x.CreatedDate)
                                                       <= EntityFunctions.TruncateTime(dtTo))
                 );
        }

        /// <summary>
        /// Method to get Geo Tag Data based on date range
        /// Created By : Dhiraj
        /// Created On : 6-Jan-2014
        /// </summary>
        /// <param name="dtFrom">Date From </param>
        /// <param name="dtTo">Date To </param>
        /// <returns>returns list of store geo tagged for specified date range</returns>
        public IEnumerable<StoreGeoTag> GetStoreGeoTags(DateTime dtFrom, DateTime dtTo)
        {

            return SmartDostDbContext.SPStoreGeoTag(dtFrom, dtTo).ToList();

        }
        /// <summary>
        /// Gets the store masters.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<vwStoreMaster> GetStoreMasters()
        {
            return SmartDostDbContext.vwStoreMasters.ToList();
        }
        /// <summary>
        /// Gets the store masters.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<vwStoreMaster> GetStoreMasters(List<long> users)
        {
            List<StoreUserBO> userStores = HttpContext.Current.Session[SessionVariables.StoreUsers] as List<StoreUserBO>;
            List<vwStoreMaster> storeList = new List<vwStoreMaster>();

            var storesMaster = SmartDostDbContext.vwStoreMasters.ToList();

            var stores = (from k in userStores
                          join id in users on k.UserID equals id
                          join sm in storesMaster on k.StoreID equals sm.StoreID
                          where sm.IsActive && !sm.IsDeleted
                          group sm by new
                          {
                              StoreName = sm.StoreName,
                              StoreCode = sm.StoreCode,
                              //ShipToCode = sm.ShipToCode,
                              //ShipToName = sm.ShipToName,
                              //ShipToBranch = sm.ShipToBranch,
                              //ShipToRegion = sm.ShipToRegion,
                              StoreID = sm.StoreID,
                              LastGeoTagDate = sm.LastGeoTagDate,
                              PictureFileName = sm.PictureFileName,
                              Lattitude = sm.Lattitude,
                              Longitude = sm.Longitude,
                              ChannelType = sm.ChannelType

                          } into smGrp
                          select new vwStoreMaster
                          {
                              StoreName = smGrp.Key.StoreName,
                              StoreCode = smGrp.Key.StoreCode,
                              //ShipToCode = smGrp.Key.ShipToCode,
                              //ShipToName = smGrp.Key.ShipToName,
                              //ShipToBranch = smGrp.Key.ShipToBranch,
                              //ShipToRegion = smGrp.Key.ShipToRegion,
                              StoreID = smGrp.Key.StoreID,
                              LastGeoTagDate = smGrp.Key.LastGeoTagDate,
                              PictureFileName = smGrp.Key.PictureFileName,
                              Lattitude = smGrp.Key.Lattitude,
                              Longitude = smGrp.Key.Longitude,
                              ChannelType = smGrp.Key.ChannelType
                          }).ToList();

            //foreach (var item in stores)
            //{
            //    storeList.Add(new vwStoreMaster()
            //    {
            //        StoreName = item.StoreName,
            //        StoreCode = item.StoreCode,
            //        ShipToCode = item.ShipToCode,
            //        ShipToName = item.ShipToName,
            //        ShipToBranch = item.ShipToBranch,
            //        ShipToRegion = item.ShipToRegion,
            //        StoreID = item.StoreID,
            //        LastGeoTagDate = item.LastGeoTagDate,
            //        PictureFileName = item.PictureFileName,
            //        Lattitude = item.Lattitude,
            //        Longitude = item.Longitude,
            //    });
            //}
            return stores;

        }
        /// <summary>
        /// Returns Total number of stores which are active.
        /// Created By :Shalu Chaudhary
        /// Created On :08-Apr-2014
        /// </summary>
        public int GetStoreMastersCount()
        {
            using (var t = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                var storeCount = (from c in SmartDostDbContext.StoreMasters
                                  join parent in SmartDostDbContext.StoreParentMappings on c.StoreID equals parent.StoreID
                                  where c.IsActive == true && c.IsDeleted == false && parent.IsDeleted == false
                                  select c.StoreID).Distinct().Count();
                t.Complete();
                return storeCount;

            }

        }


        /// <summary>
        /// Returns Total number of stores which are active.
        /// Created By :Nishat
        /// Created On :05-Aug-2014
        /// </summary>
        public int GetStoreCountLevelwise(string region, int level, string branch, string accountname)
        {
            using (var t = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                if (level == 3)
                {
                    var storeCount = (from c in SmartDostDbContext.StoreMasters
                                      join parent in SmartDostDbContext.StoreParentMappings on c.StoreID equals parent.StoreID
                                      where parent.ShipToRegion == region && c.IsActive == true && c.IsDeleted == false && parent.IsDeleted == false
                                      select c.StoreID).Distinct().Count();


                    return storeCount;
                }

                if (level == 4)
                {
                    var storeCount = (from c in SmartDostDbContext.StoreMasters
                                      join parent in SmartDostDbContext.StoreParentMappings on c.StoreID equals parent.StoreID
                                      where parent.ShipToRegion == region && parent.ShipToBranch == branch && c.IsActive == true && c.IsDeleted == false && parent.IsDeleted == false
                                      select c.StoreID).Distinct().Count();


                    return storeCount;
                }
                if (level == 5)
                {
                    var storeCount = (from c in SmartDostDbContext.StoreMasters
                                      join parent in SmartDostDbContext.StoreParentMappings on c.StoreID equals parent.StoreID
                                      where parent.ShipToRegion == region && parent.ShipToBranch == branch && parent.AccountName == accountname && c.IsActive == true && c.IsDeleted == false && parent.IsDeleted == false
                                      select c.StoreID).Distinct().Count();


                    return storeCount;
                }

                //var storemastercount = SmartDostDbContext.StoreMasters.Count(sm => sm.IsActive && sm.IsDeleted == false);
                return 0;
            }

        }

        /// <summary>
        /// Method to fetch outlet chart structures
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <param name="moduleCode">module code</param>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <returns>returns chart structure</returns>
        public IList<OutletChartStructure> GetUserChartOutlets(long userID, int moduleCode, DateTime startDate, DateTime endDate, string selectedResponse)
        {

            IList<OutletChartStructure> userStores = new List<OutletChartStructure>();
            switch (moduleCode)
            {
                case (int)AspectEnums.WebModules.TotalCoverageNormDashBoard:
                    userStores = SmartDostDbContext.SPGetLastLevelOutlets(userID, startDate, endDate, (byte)AspectEnums.CoverageType.CoverageVsNorm).Select(k => new OutletChartStructure() { StoreID = k.StoreID, OutletName = k.StoreName, ChartValue = k.StoreCount }).ToList();
                    break;
                case (int)AspectEnums.WebModules.TotalCoverageDashBoard:
                    userStores = SmartDostDbContext.SPGetLastLevelOutlets(userID, startDate, endDate, (byte)AspectEnums.CoverageType.CoverageVsPlan).Select(k => new OutletChartStructure() { StoreID = k.StoreID, OutletName = k.StoreName, ChartValue = k.StoreCount }).ToList();
                    break;
                case (int)AspectEnums.WebModules.UniqueOutletCoverageDashBoard:
                    userStores = SmartDostDbContext.SPGetLastLevelOutlets(userID, startDate, endDate, (byte)AspectEnums.CoverageType.UniqueCoverage).Select(k => new OutletChartStructure() { StoreID = k.StoreID, OutletName = k.StoreName, ChartValue = k.StoreCount }).ToList();
                    break;
                case (int)AspectEnums.AppModules.Attendance:
                    string leave = "Leaves";
                    string marketWorking = "Market Working";
                    string absent = "Absent";
                    int selectedRoleID = SmartDostDbContext.UserRoles.FirstOrDefault(k => k.UserID == userID && !k.IsDeleted && k.IsActive).RoleID;

                    var attendanceData = SmartDostDbContext.SpGetAttendanceExport(startDate, endDate, userID, selectedRoleID, 0).ToList();
                    bool hasLeave = attendanceData.Any(x => x.Attendance.ToLower() == leave.ToLower());
                    bool hasMarketWorking = attendanceData.Any(x => x.Attendance.ToLower() == marketWorking.ToLower());
                    bool hasAbsent = attendanceData.Any(x => x.Attendance.ToLower() == absent.ToLower());

                    userStores = attendanceData.GroupBy(x => x.Attendance).Select(k => new OutletChartStructure() { OutletName = k.Key, ChartValue = k.Count() }).ToList();
                    if (!hasLeave)
                        userStores.Add(new OutletChartStructure { ChartValue = 0, OutletName = leave });
                    if (!hasMarketWorking)
                        userStores.Add(new OutletChartStructure { ChartValue = 0, OutletName = marketWorking });
                    if (!hasAbsent)
                        userStores.Add(new OutletChartStructure { ChartValue = 0, OutletName = absent });
                    break;



            }
            return userStores.Distinct().ToList();
        }

        /// <summary>
        /// Function to return list of users under current level
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <param name="roleId">Role Id</param>
        /// <param name="currentLevel">Current Level of report</param>
        /// <param name="selectedRoleID">Slected Role Id if any</param>
        /// <returns></returns>
        private List<UserRole> GetUsersUnderCurrentLevel(long userId, int roleId, int currentLevel, int? selectedRoleID, int? teamID)
        {
            var UsersUnderCurrentLevel = new List<UserRole>();

            if (currentLevel == 0)
            {
                UsersUnderCurrentLevel = GetEmployeesUnderHierarchy(userId, roleId, teamID, null, true).Where(x => x.RoleID == selectedRoleID).ToList();

            }
            else if (currentLevel == 1)
            {
                UsersUnderCurrentLevel = GetEmployeesUnderHierarchy(userId, roleId, teamID, 0, true);

            }
            else
            {
                UsersUnderCurrentLevel = GetEmployeesUnderHierarchy(userId, roleId, teamID, 1, false);
            }


            return UsersUnderCurrentLevel;
        }

        public List<SPStoreUsers_Result> GetStoreUsers(List<long> userIDs)
        {
            //List<vwStoreUser> storeUsers = new List<vwStoreUser>();
            ////var storeUsers = (from k in SmartDostDbContext.StoreUsers
            ////                  join h in userIDs on k.UserID equals h
            ////                  where k.IsDeleted == false
            ////                  select k).ToList();
            ////var stores = SmartDostDbContext.StoreUsers.Where(k => !k.IsDeleted).ToList();
            //var stores = SmartDostDbContext.vwStoreUsers.ToList();
            //var storeUsersList = (from s in stores
            //                      join e in userIDs
            //                      on s.UserID equals e
            //                      select s).ToList();
            ////foreach (long id in userIDs)
            ////{
            ////    var users = stores.Where(k => k.UserID == id).ToList();
            ////    storeUsers.AddRange(users);
            ////}
            //return storeUsersList;
            var userids = string.Join<long>(",", userIDs);
            var storeusers = SmartDostDbContext.SPStoreUsers(userids);
            return storeusers.ToList();
        }

        public IList<vwGetUserGeo> GetUserGeo()
        {
            var geouser = SmartDostDbContext.vwGetUserGeos.ToList();
            return geouser;
        }

        public List<StoreUser> GetStoreUsers()
        {
            var stores = SmartDostDbContext.StoreUsers.Where(k => k.IsDeleted == false).ToList();
            return stores;
        }
        public List<vwStoreUser> GetStoreUsersAll()
        {
            var stores = SmartDostDbContext.vwStoreUsers.ToList();
            return stores;
        }

        public List<Branch> GetAllBranches()
        {
            return SmartDostDbContext.Branches.Where(x => x.IsDeleted == false).ToList();
        }

        #endregion

        #region Freezed Geo Tag Report
        /// <summary>
        /// Get report for Geo Tag Freeze
        /// </summary>
        /// <returns></returns>        
        ///         
        public List<FreezedGeoTag> GetFreezGeoTagData()
        {
            var lstfreezegeotag = (from fgt in SmartDostDbContext.FreezeGeoTags
                                   join spm in SmartDostDbContext.StoreParentMappings
                                      on fgt.StoreID equals spm.StoreID
                                   join sm in SmartDostDbContext.StoreMasters
                                      on fgt.StoreID equals sm.StoreID
                                   where
                                    (
                                        spm.IsDeleted == false
                                    &&
                                        sm.IsDeleted == false
                                    &&
                                        sm.IsActive == true)
                                   select new FreezedGeoTag()
                                   {
                                       Zone = spm.ShipToRegion,
                                       Branch = spm.ShipToBranch,
                                       StoreCode = sm.StoreCode,
                                       StoreName = sm.StoreName,
                                       Lattitude = fgt.Lattitude,
                                       Longitude = fgt.Longitude,
                                       Occurance = fgt.Occurance
                                   }
                                 ).OrderBy(x => x.StoreName).ToList();
            return lstfreezegeotag;
        }
        #endregion


        #region For SDCE-871 (Get Display Share and Counter Share Response ) By Vaishali on 07 Nov 2014
        /// <summary>
        /// Get Display Share and Counter Share Response        
        /// 
        /// </summary>
        /// <param name="compititionType"></param>
        /// <param name="reportLevel"></param>
        /// <param name="firstLevelParam"></param>
        /// <param name="secondLevelParam"></param>
        /// <param name="thirdLevelParam"></param>
        /// <param name="channel"></param>
        /// <param name="userid"></param>
        /// <returns></returns>

        public List<SPGetDisplayCounterShareResponse_Result> SPGetDisplayCounterShareResponse(int compititionType, int reportLevel, string firstLevelParam, string secondLevelParam, string thirdLevelParam, string channel, long userid)
        {
            return SmartDostDbContext.SPGetDisplayCounterShareResponse(compititionType, reportLevel, firstLevelParam, secondLevelParam, thirdLevelParam, channel, userid).ToList();
        }

        public List<SPGetDisplayCounterShareResponseExport_Result> SPGetDisplayCounterShareResponseExport(int compititionType, int reportLevel, string firstLevelParam, string secondLevelParam, string thirdLevelParam, string channels, long userid)
        {
            return SmartDostDbContext.SPGetDisplayCounterShareResponseExport(compititionType, reportLevel, firstLevelParam, secondLevelParam, thirdLevelParam, channels, userid).ToList();
        }
        #endregion


        #region Download VOC Data for SDCE-892 by Vaishali on 12-Nov-2013
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
        public List<SPGetVOCSearch_Result> GetVOCSearch(DateTime DateFrom, DateTime DateTo, string BizDivision, string Region, string CustomerType, string ChannelCategory, string FeedbackType, string VOCClass, string Category, string SubCategory, string State, string VOCManagerName, string BizObjective, long userid)
        {
            BizDivision = string.IsNullOrEmpty(BizDivision) || BizDivision == "All selected" ? null : BizDivision;
            Region = string.IsNullOrEmpty(Region) || Region == "All selected" ? null : Region;
            CustomerType = string.IsNullOrEmpty(CustomerType) || CustomerType == "All selected" ? null : CustomerType;
            ChannelCategory = string.IsNullOrEmpty(ChannelCategory) || ChannelCategory == "All selected" ? null : ChannelCategory;
            FeedbackType = string.IsNullOrEmpty(FeedbackType) || FeedbackType == "All selected" ? null : FeedbackType;
            VOCClass = string.IsNullOrEmpty(VOCClass) || VOCClass == "All selected" ? null : VOCClass;
            //ConcenedDepartment = string.IsNullOrEmpty(ConcenedDepartment) || ConcenedDepartment == "All selected" ? null : ConcenedDepartment;            
            Category = string.IsNullOrEmpty(Category) || Category == "All selected" ? null : Category;
            SubCategory = string.IsNullOrEmpty(SubCategory) || SubCategory == "All selected" ? null : SubCategory;
            State = string.IsNullOrEmpty(State) || State == "All selected" ? null : State;
            VOCManagerName = string.IsNullOrEmpty(VOCManagerName) || VOCManagerName == "All selected" ? null : VOCManagerName;
            BizObjective = string.IsNullOrEmpty(BizObjective) || BizObjective == "All selected" ? null : BizObjective;

            return SmartDostDbContext.SPGetVOCSearch(DateFrom, DateTo, BizDivision, Region, CustomerType, ChannelCategory, FeedbackType, VOCClass, Category, SubCategory, State, VOCManagerName, BizObjective, userid, "").ToList();
        }
        #endregion

        /// <summary>
        /// This function will fetch senior of any user inside same team
        /// </summary>
        /// <param name="userID">user for whom senior needs to find</param>
        /// <param name="teamid">teamid of user for whom senior needs to find</param>
        /// <returns></returns>
        public List<SeniorBO> GetSeniorsDB(long userID, long teamid)
        {
            List<SeniorBO> users = new List<SeniorBO>();
            using (var t = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions() { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                int TeamIDInt = Convert.ToInt32(teamid);
                users = (from s in SmartDostDbContext.EmployeeSeniors
                         where s.ChildUserID == userID && s.TeamID == TeamIDInt
                         select new SeniorBO { RoleID = s.RoleID, FirstName = s.FirstName, UserID = s.UserID.Value }).Distinct().ToList();

                //var Seniors= SmartDostDbContext.EmployeeSeniors.Where(k => k.ChildUserID == userID && k.TeamID == teamid && k.ReportingTeamID==teamid).OrderByDescending(k=>k.ProfileLevel) ;

                //var userRoles = employeeList;
                //var reportingId = userID as long?;

                //do
                //{

                //    var person = SmartDostDbContext.EmployeeSeniors.Where(k => k.ChildUserID == userID && k.UserID == reportingId && k.TeamID == teamid);
                //    if (person != null)
                //    {
                //        EmployeeSenior p = person.FirstOrDefault(k => k.ReportingTeamID == teamid);
                //        if (p != null)
                //        {
                //            users.Add(new SeniorBO() { UserID = p.UserID.Value, FirstName = p.FirstName, RoleID = p.RoleID, ReportingTeamID = p.ReportingTeamID, ReportingUserID = p.ReportingUserID });
                //            reportingId = p.ReportingUserID;
                //            if (p.UserID == reportingId)
                //                reportingId = null;
                //            continue;
                //        }
                //        p = person.FirstOrDefault();
                //        users.Add(new SeniorBO() { UserID = p.UserID.Value, FirstName = p.FirstName, RoleID = p.RoleID, ReportingTeamID = p.ReportingTeamID, ReportingUserID = p.ReportingUserID });

                //        if (p.UserID == reportingId)
                //            reportingId = null;
                //    }
                //    else
                //        reportingId = null;
                //    //var person = (from ur in SmartDostDbContext.UserRoles
                //    //              join um in SmartDostDbContext.UserMasters on ur.UserID equals um.UserID
                //    //              join rm in SmartDostDbContext.RoleMasters on ur.RoleID equals rm.RoleID
                //    //              join sr in SmartDostDbContext.UserRoles on ur.ReportingUserID equals sr.UserID
                //    //              join srm in SmartDostDbContext.RoleMasters on sr.RoleID equals srm.RoleID
                //    //              where !ur.IsDeleted && ur.IsActive && !rm.IsDeleted && rm.IsActive &&
                //    //              srm.TeamID == teamid && ur.UserID == reportingId
                //    //              select new SeniorBO { UserID = ur.UserID, RoleID = ur.RoleID, FirstName = um.FirstName + " " + (um.LastName == null ? "" : um.LastName.Substring(0, 1)), ReportingTeamID = srm.TeamID, ReportingUserID = ur.ReportingUserID }).FirstOrDefault();


                //    //var person = SmartDostDbContext.UserRoles.FirstOrDefault(p => p.UserID == reportingId && p.ReportingTeamID == teamid);
                //    //if (person != null)
                //    //{
                //    //    users.Add(person);
                //    //    reportingId = person.ReportingUserID;
                //    //    if (person.UserID == reportingId)
                //    //        reportingId = null;
                //    //}
                //    //else
                //    //{
                //    //    reportingId = null;
                //    //}
                //} while (reportingId != null);
                t.Complete();
            }
            return users;

        }

        #region RLS -Data
        /// <summary>
        /// Get SoDetails 
        /// </summary>
        /// <param name="soNumber">SoNumber</param>
        /// <param name="userID">UserID</param>
        /// <returns></returns>
        public DataSet GetSODetails(string soNumber, long userID)
        {//@
            var MySingeID = SmartDostDbContext.UserMasters.FirstOrDefault(x => x.UserID == userID);
            List<SqlParameter> lstparam = new List<SqlParameter>();
            lstparam.Add(new SqlParameter("@CLAIM_NO", soNumber));
            lstparam.Add(new SqlParameter("@User_ID", MySingeID.MySingleID ?? ""));
            //lstparam.Add(new SqlParameter("@User_ID", "sonalika.s2"));
            return ExecuteProcedure(StoredProcedureVariables.SORLS, lstparam, "SmartDostRLS_DB_Connection");


        }
        #endregion

        /// <summary>
        /// Function is used to execute any storeprocedure using ADO.Net
        /// </summary>
        /// <param name="ReportName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public DataSet ExecuteProcedure(string ReportName, List<SqlParameter> parameters, string connectionString = "SmartDostLog_DB_Connection")
        {
            DataSet DSresult = new DataSet();

            Microsoft.Practices.EnterpriseLibrary.Data.Database DataAccess = EnterpriseLibraryContainer.Current.GetInstance<Microsoft.Practices.EnterpriseLibrary.Data.Database>(connectionString) as SqlDatabase;
            using (SqlConnection connection = new SqlConnection(DataAccess.ConnectionString))
            {

                SqlDataAdapter adapter;
                SqlCommand command = new SqlCommand();

                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = ReportName;
                command.CommandTimeout = 600;

                command.Parameters.AddRange(parameters.ToArray());

                adapter = new SqlDataAdapter(command);
                adapter.Fill(DSresult);

            }
            return DSresult;
        }


        #region added by manonrajan for Race Product Audit report

        public List<RaceProductCategory> GetProductGroup()
        {
            return SmartDostDbContext.RaceProductCategories.Where(x => x.IsDeleted == false).ToList();
        }

        public List<SpRaceReportModelWise_Result> GetModelWiseReport(long userID, string productGroup, DateTime strDateFrom, DateTime strDateTo)
        {

            //return SmartDostDbContext.SpRaceReportModelWise(fromDate: strDateFrom, toDate: strDateFrom, productgroup: productGroup).ToList();
            return SmartDostDbContext.SpRaceReportModelWise(productGroup, strDateFrom, strDateTo, userID).ToList();

        }
        /// <summary>
        /// Get model wise join
        /// </summary>
        /// <param name="productGroup"></param>
        /// <returns></returns>
        public List<RaceProductModel> GetModelWiseJoin(string productGroup)
        {
            List<RaceProductModel> lstRaceModelWise = new List<RaceProductModel>();
            var result = (from a in SmartDostDbContext.RaceProductMasters
                          join b in SmartDostDbContext.RaceBrandMasters
                              on a.BrandID equals b.BrandID
                          where a.ProductGroup == productGroup
                          select new RaceProductModel
                          {
                              ProductID = a.ProductID,
                              ProductType = a.ProductType,
                              ProductGroup = a.ProductGroup,
                              ProductCategory = a.ProductCategory,
                              ModelName = a.ModelName,
                              BrandID = a.BrandID,
                              IsDeleted = a.IsDeleted,
                              StartDate = a.StartDate,
                              EndDate = a.EndDate,
                              CreatedDate = a.CreatedDate,
                              ModifiedDate = a.ModifiedDate,
                              ProductSize = a.ProductSize,
                              BrandName = b.BrandName
                          }).ToList();
            lstRaceModelWise = result;
            return lstRaceModelWise;
        }
        /// <summary>
        /// Return list of VOC Report
        /// </summary>
        /// <param name="storecode"></param>
        /// <returns></returns>
        public List<VOCReportNew> GetNewVOCReport(string storecode, string strDateFrom, string strDateTo)
        {
            DateTime dateFrom = Convert.ToDateTime(strDateFrom).Date;
            DateTime dateTo = Convert.ToDateTime(strDateTo).AddDays(1).Date;
            List<VOCReportNew> lstVOCReportNew = new List<VOCReportNew>();
            var result = (from a in SmartDostDbContext.NewVOCUserResponses
                          join b in SmartDostDbContext.NewVOCUserResponses
                          on a.CEVOCResponseID equals b.CEVOCResponseID
                          join c in SmartDostDbContext.NewVOCResponses
                          on b.CEVOCResponseID equals c.CEVOCResponseID
                          where a.Response == storecode && (c.CreatdDate >= dateFrom && c.CreatdDate < dateTo)
                          select new VOCReportNew
                          {
                              Header = b.Header,
                              Question = b.Question,
                              Response = b.Response,
                              CreatedDate = c.CreatdDate,
                              CEVOCResponseID = b.CEVOCResponseID
                          }).OrderBy(c => c.CreatedDate).ToList();
            lstVOCReportNew = result;
            return lstVOCReportNew;

        }

        /// <summary>
        /// Return Store Procedure List of result 
        /// </summary>
        /// <param name="fromMonth"></param>
        /// <param name="toMonth"></param>
        /// <param name="fromYear"></param>
        /// <param name="toYear"></param>
        /// <returns></returns>
        public List<SpGetVOCReport_Result> GetNewVOCReportMonthWise(int countMonthFrom, int countMonthTo, int fromMonth, int toMonth, int fromYear, int toYear, string strProductCategory, string strTypeOfPartner, string strPartnerCode, string strCityTier, string strRegion, string strState, string strCity)
        {
            List<SpGetVOCReport_Result> lstSpGetVOCReport_Result = new List<SpGetVOCReport_Result>();
            return SmartDostDbContext.SpGetVOCReport(countMonthFrom, countMonthTo, fromMonth, toMonth, fromYear, toYear, strProductCategory, strTypeOfPartner, strPartnerCode, strCityTier, strRegion, strState, strCity).ToList();
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
        public List<SpGetVOCReportTotalCount_Result> GetSpGetVOCTotalCount(int fromMonth, int toMonth, int fromYear, int toYear, string strProductCategory, string strTypeOfPartner, string strPartnerCode, string strCityTier, string strRegion, string strState, string strCity)
        {
            return SmartDostDbContext.SpGetVOCReportTotalCount(fromMonth, toMonth, fromYear, toYear, strProductCategory, strTypeOfPartner, strPartnerCode, strCityTier, strRegion, strState, strCity).ToList();
        }

        /// <summary>
        /// Return GetVOCOpenEndedReport List
        /// </summary>
        /// <param name="productCategory"></param>
        /// <param name="fromMonth"></param>
        /// <param name="toMonth"></param>
        /// <param name="fromYear"></param>
        /// <param name="toYear"></param>
        /// <param name="Question"></param>
        /// <returns></returns>
        public List<spGetVOCOpenEndedReport_Result> GetVOCOpenEndedReport(string productCategory, DateTime fromDate, DateTime toDate, string Question)
        { // 
            return SmartDostDbContext.spGetVOCOpenEndedReport(productCategory, fromDate, toDate, Question).ToList();


        }

        /// <summary>
        /// Populate filter for Sentiment report
        /// </summary>
        /// <param name="header"></param>
        /// <returns></returns>
        public List<NewVOCUserResponse> GetVOCSentimentFilter(string header)
        {
            List<NewVOCUserResponse> lstNewVOCResponse = new List<NewVOCUserResponse>();
            var result = (from a in SmartDostDbContext.NewVOCUserResponses
                          where a.Header == header
                          select new
                          {
                              Response = a.Response,
                              Header = a.Header,
                              Question = a.Question
                          }).Distinct().AsEnumerable().Select(x => new NewVOCUserResponse
                          {
                              Response = x.Response,
                              Header = x.Header,
                              Question = x.Question
                          }).ToList();

            lstNewVOCResponse = result;
            return lstNewVOCResponse;
        }

        #endregion
    }

}
