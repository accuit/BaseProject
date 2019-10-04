using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samsung.SmartDost.PersistenceLayer.Repository.Contracts;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using System.Transactions;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;

namespace Samsung.SmartDost.PersistenceLayer.Data.Impl
{
    public class RaceDataImpl : BaseDataImpl, IRaceRepository
    {

        /// <summary>
        /// Method to logout web user from the application
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public bool LogoutWebUser(long userID, string sessionID)
        {
            bool isSuccess = false;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                    }))
            {
                DailyLoginHistory dailyLoginHistory = SmartDostDbContext.DailyLoginHistories.FirstOrDefault(k => k.UserID == userID && k.SessionID == sessionID && k.IsLogin == true);
                if (dailyLoginHistory != null)
                {
                    dailyLoginHistory.IsLogin = false;
                    dailyLoginHistory.LogOutTime = System.DateTime.Now;
                    SmartDostDbContext.Entry<DailyLoginHistory>(dailyLoginHistory).State = System.Data.EntityState.Modified;
                    isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;
                }
                scope.Complete();
            }
            return isSuccess;
        }
        /// <summary>
        /// Update user login history on the basis of user login status and history
        /// </summary>
        /// <param name="history">login history instance</param>
        /// <param name="userID">user ID</param>
        /// <param name="lastLoginDate">last login date</param>
        /// <param name="lattitude">lattitude value</param>
        /// <param name="longitude"> longitude value</param>
        /// <param name="isLogin">is successfull login</param>
        private void UpdateUserLoginHistory(LoginAttemptHistory history, long userID, DateTime? lastLoginDate, string lattitude, string longitude, bool isLogin)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                  new TransactionOptions
                  {
                      IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                  }))
            {
                if (history == null)
                    history = new LoginAttemptHistory();
                history.UserID = userID;
                history.Lattitude = lattitude;
                history.Longitude = longitude;
                history.LoginDate = System.DateTime.Now;
                history.LastLoginDate = isLogin ? System.DateTime.Now : history.LastLoginDate;
                history.FailedAttempt = isLogin ? 0 : history.FailedAttempt + 1;
                if (history.LoginAttemptID == 0)
                    SmartDostDbContext.LoginAttemptHistories.Add(history);
                else
                    SmartDostDbContext.Entry<LoginAttemptHistory>(history).State = System.Data.EntityState.Modified;
                SmartDostDbContext.SaveChanges();
                if (history.FailedAttempt >= 3)
                {
                    UserMaster userDetail = SmartDostDbContext.UserMasters.FirstOrDefault(k => k.UserID == userID);
                    if (userDetail != null)
                    {
                        userDetail.AccountStatus = (int)AspectEnums.UserLoginStatus.Locked;
                        userDetail.ModifiedDate = System.DateTime.Now;
                        SmartDostDbContext.Entry<UserMaster>(userDetail).State = System.Data.EntityState.Modified;
                        SmartDostDbContext.SaveChanges();
                    }
                }
                scope.Complete();
            }
        }

        /// <summary>
        /// Method to update user password on the basis of user employeeid
        /// </summary>
        /// <param name="usermaster">usermaster table instance</param>
        /// <param name="employeeid">employee ID</param>
        /// <param name="newpassword">New Password</param>
        private void UpdateUserPassword(UserMaster usermaster, string employeeid, string newpassword)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions
                   {
                       IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                   }))
            {
                if (usermaster == null)
                    usermaster = new UserMaster();
                usermaster.Password = newpassword;
                usermaster.AccountStatus = 1;
                usermaster.IsPinRegistered = true;
                usermaster.ModifiedDate = System.DateTime.Now;
                SmartDostDbContext.SaveChanges();
                scope.Complete();
            }
        }

        public List<SpAuditSummarySearch_Result> GetSearchAuditdata(AuditSearch auditSearch, long userID, long RoleID)
        {
            List<SpAuditSummarySearch_Result> result = new List<SpAuditSummarySearch_Result>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                  new TransactionOptions
                  {
                      IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                  }))
            {
                result = SmartDostDbContext.SpAuditSummarySearch(fromDate: auditSearch.Assessment_From_Date, toDate: auditSearch.Assessment_To_Date, currentStatus: null, state: auditSearch.Unit,
               city: auditSearch.Region, supFromDate: auditSearch.SupdateFrom, supToDate: auditSearch.SupdateTo, supStatus: auditSearch.supStatus, qC1FromDate: auditSearch.QClevel1datefrom, qC1ToDate: auditSearch.QClevel1dateto,
               qC1Status: auditSearch.qC1Status, qC2FromDate: auditSearch.QClevel2DateFrom, qC2ToDate: auditSearch.QClevel2DateTo, qC2Status: auditSearch.qC2Status, storeCode: auditSearch.storeCode, advanceFilterRequired: auditSearch.advanceFilterRequired,
               loggedUserID : userID, loggedUserRoleID: RoleID).ToList();
                scope.Complete();
            }
            return result;
        }

        /// <summary>
        /// Get product Audit data
        /// </summary>
        /// <param name="productAuditDTO"></param>
        /// <returns></returns>
        public List<SpProductAuditSummary_Result> GetProductAuditdata(int auditID)
        {
            List<SpProductAuditSummary_Result> result = new List<SpProductAuditSummary_Result>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions
                   {
                       IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                   }))
            {
                result = SmartDostDbContext.SpProductAuditSummary(auditID).ToList();
                scope.Complete();
            }
            return result;
        }

        /// <summary>
        /// Select list of modules in a survey Response
        /// </summary>
        /// <param name="surveyResponseID"></param>
        /// <returns></returns>
        public List<SPGetAuditQuestionDetails_Result> GetSurveyModulesList(long surveyResponseID)
        {
            List<SPGetAuditQuestionDetails_Result> result = new List<SPGetAuditQuestionDetails_Result>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                 new TransactionOptions
                 {
                     IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                 }))
            {
                result = SmartDostDbContext.SPGetAuditQuestionDetails(surveyResponseID).ToList();
                scope.Complete();
            }
            return result;
        }


        public bool submitReviewerResponse(ReviewerResponse reviewerResponse, long userID, long RoleID)
        {
            bool IsSuccess = false;

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
new TransactionOptions
{
    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
}))
            {
                AuditLog log = new AuditLog()
                {
                    AuditID = reviewerResponse.AuditID,
                    CreatedBy = reviewerResponse.userID,
                    CreatedDate = System.DateTime.Now,
                    IsDeleted = false,
                    Remarks = reviewerResponse.Remarks,
                    ReviewBy = reviewerResponse.userID,
                    Status = reviewerResponse.Status
                };

                SmartDostDbContext.AuditLogs.Add(log);

                if (reviewerResponse.Status != (byte)AspectEnums.RaceAuditStatus.Comments)
                {
                    var auditsummary = SmartDostDbContext.AuditSummaries.Where(x => x.AuditID == reviewerResponse.AuditID).ToList().FirstOrDefault();
                    auditsummary.CurrentStatus = reviewerResponse.Status;
                    auditsummary.ModifiedBy = reviewerResponse.userID;
                    auditsummary.ModifiedDate = DateTime.Now;
                    if (RoleID == (int)AspectEnums.Roles.QC1)
                    {
                        auditsummary.QC1Status=reviewerResponse.Status;
                        auditsummary.QC1Remarks=reviewerResponse.Remarks;
                        auditsummary.qc1ActionDate = System.DateTime.Now;
                        
                    }

                    if (RoleID == (int)AspectEnums.Roles.QC2)
                    {
                        auditsummary.QC2Status = reviewerResponse.Status;
                        auditsummary.QC2Remarks = reviewerResponse.Remarks;
                        auditsummary.qc2ActionDate = System.DateTime.Now;
                    }

                    if (RoleID == (int)AspectEnums.Roles.Superior)
                    {
                        auditsummary.SupStatus = reviewerResponse.Status;
                        auditsummary.SupRemarks = reviewerResponse.Remarks;
                        auditsummary.supActionDate = System.DateTime.Now;
                    }

                    SmartDostDbContext.Entry<AuditSummary>(auditsummary).State = System.Data.EntityState.Modified;
                }
                IsSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;
                scope.Complete();
            }
            return IsSuccess;
        }

       
        /// <summary>
        ///Get geo definitions 
        /// </summary>
        /// <returns></returns>
        public List<GeoDefinition> GetGeoDefinitions()
        {
            List<GeoDefinition> result = new List<GeoDefinition>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                 new TransactionOptions
                 {
                     IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                 }))
            {
                result = SmartDostDbContext.GeoDefinitions.Where(x=>x.IsDeleted==false).ToList();//Deleted check added after MDM by Dhiraj on 18-Aug-2015 SDCE-3790
            }
            return result;
        }
        public StoreGeoTag GetStoreDetails(long surveyResponseID)
        {
            StoreGeoTag result = new StoreGeoTag();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                 new TransactionOptions
                 {
                     IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                 }))
            {
                result = (from response in SmartDostDbContext.SurveyResponses
                          join tag in SmartDostDbContext.StoreGeoTags
                          on new { UserID = response.UserID, StoreID = response.StoreID.Value } equals new { UserID = tag.UserID, StoreID = tag.StoreID }
                          where response.SurveyResponseID == surveyResponseID
                          select tag
                      ).ToList().OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                scope.Complete();
            }
            return result;

        }

        public List<AuditLogDetails> getauditLogDetails(int auditID)
        {
            List<AuditLogDetails> auditLog = new List<AuditLogDetails>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                  new TransactionOptions
                  {
                      IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                  }))
            {
                auditLog = (from log in SmartDostDbContext.AuditLogs
                            join user in SmartDostDbContext.UserMasters
                            on log.ReviewBy equals user.UserID
                            where log.AuditID == auditID &&
          log.IsDeleted == false &&
          string.IsNullOrEmpty(log.Remarks) == false
                            select new { log.AuditLogID, log.ReviewBy, log.Remarks, log.Status, user.Username, log.CreatedDate }).ToList().Select(
                                             x => new AuditLogDetails()
                                             {
                                                 AuditID = auditID,
                                                 AuditLogID = x.AuditLogID,
                                                 ReviewBy = x.ReviewBy,
                                                 ReviewByName = x.Username,
                                                 Remarks = x.Remarks,
                                                 Status = x.Status,
                                                 CreatedDate = x.CreatedDate,
                                             }
                                      ).Distinct().OrderByDescending(x => x.CreatedDate).ToList();
                scope.Complete();
            }

            return auditLog;
        }

        /// <summary>
        /// Save APIKey and Token for QC user
        /// </summary>
        /// <param name="APIKey"></param>
        /// <param name="APIToken"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool generateAPIKeyToken(string APIKey, string APIToken, long userID)
        {
            bool isSuccess = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions
                   {
                       IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
                   }))
            {
                var userservice = SmartDostDbContext.UserServiceAccesses.Where(x => x.UserID == userID).FirstOrDefault();
                if (userservice != null)
                {
                    userservice.APIKey = APIKey;
                    userservice.APIToken = APIToken;
                    userservice.ModifiedBy = userID;
                    userservice.ModifiedDate = System.DateTime.Now;
                    SmartDostDbContext.Entry<UserServiceAccess>(userservice).State = System.Data.EntityState.Modified;                    
                }
                else
                {
                    UserServiceAccess userServiceAccess = new UserServiceAccess();
                    userServiceAccess.UserID = userID;
                    userServiceAccess.APIKey = APIKey;
                    userServiceAccess.APIToken = APIToken;
                    userServiceAccess.CreatedBy = userID;
                    userServiceAccess.CreatedDate = System.DateTime.Now;

                    SmartDostDbContext.UserServiceAccesses.Add(userServiceAccess);

                }
                isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;
                scope.Complete();
            }
            return isSuccess;
        }
    }
}
