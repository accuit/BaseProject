using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.PersistenceLayer.Repository.Contracts;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using System.Xml.Linq;

namespace Samsung.SmartDost.PersistenceLayer.Data.Impl
{/// <summary>
    /// Interface to define store related methods
    /// </summary>
    public class BeatDataImpl : BaseDataImpl, IBeatRepository
    {
        /// <summary>
        /// Method will execute the Beat query in the database       
        ///<param name="userBeatCollection">Beat collection info of the user</param>
        /// <returns>return true if entry inserted else false</returns>
        /// / </summary>
        public int SaveUserBeat(List<CoveragePlan> userCoveragePlanCollection)
        {

           // DateTime currentDateTime = DateTime.Now;
            int status = 0;
            if (userCoveragePlanCollection != null && userCoveragePlanCollection.Count > 0)
            {
              //  long currentUserId = userCoveragePlanCollection[0].UserID;
               // string currentDay = DateTime.Now.Day.ToString();
                //  UserSystemSetting userSettings = SmartDostDbContext.UserSystemSettings.FirstOrDefault(k => k.IsCoverageException && k.CoverageExceptionWindow == DateTime.Today);
                // if (userSettings != null)
                return SaveExceptionUserBeats(userCoveragePlanCollection);
                //using (TransactionScope scope = new TransactionScope())
                //{


                //    int month = userCoveragePlanCollection[0].CoverageDate.Month;
                //    int year = userCoveragePlanCollection[0].CoverageDate.Year;
                //    #region Earlier Market Off Delete as suggested by client on 13-Feb-2014 by Dhiraj
                //    List<UserLeavePlan> earlierMarketOff = SmartDostDbContext.UserLeavePlans.Where(k => k.LeaveDate.Month == month && k.UserID == currentUserId && k.LeaveDate.Year == year && k.LeaveTypeID == 2).ToList();
                //    foreach (var item in earlierMarketOff)
                //    {
                //        SmartDostDbContext.UserLeavePlans.Remove(item);
                //    }
                //    SmartDostDbContext.SaveChanges();
                //    #endregion
                //    List<CoveragePlan> earlierPlans = SmartDostDbContext.CoveragePlans.Where(k => k.CoverageDate.Month == month && k.UserID == currentUserId && k.CoverageDate.Year == year).ToList();
                //    if (earlierPlans != null)
                //    {
                //        if (earlierPlans.Count(k => k.StatusID == 0) > 0)
                //        {
                //            return -1;
                //        }
                //        if (earlierPlans.Count(k => k.StatusID == 1) > 0)
                //        {
                //            return -2;
                //        }
                //        foreach (var item in earlierPlans)
                //        {
                //            //if (item.StatusID == 1)
                //            //{
                //            item.StatusID = 2;
                //            item.Remarks = "New beat plan created";
                //            item.ModifiedDate = currentDateTime;
                //            item.ModifiedBy = Convert.ToInt32(currentUserId);
                //            SmartDostDbContext.Entry<CoveragePlan>(item).State = System.Data.EntityState.Modified;
                //            //}
                //            //else
                //            //{
                //            //    SmartDostDbContext.CoveragePlans.Remove(item);
                //            //}
                //        }
                //    }
                //    if (!String.IsNullOrEmpty(userCoveragePlanCollection[0].MarketOffDays))
                //    {
                //        string marketOffDays = userCoveragePlanCollection[0].MarketOffDays;
                //        string[] marketArray = marketOffDays.Split(',');
                //        if (marketArray.Length > 0)
                //        {
                //            foreach (string day in marketArray)
                //            {
                //                UserLeavePlan leavePlan = new UserLeavePlan()
                //                {
                //                    LeaveToDate = new DateTime(userCoveragePlanCollection[0].CoverageDate.Year, userCoveragePlanCollection[0].CoverageDate.Month, Convert.ToInt32(day)),
                //                    UserID = userCoveragePlanCollection[0].UserID,
                //                    CreatedDate = currentDateTime,
                //                    CreatedBy = userCoveragePlanCollection[0].UserID,
                //                    IsDeleted = false,
                //                    LeaveDate = new DateTime(userCoveragePlanCollection[0].CoverageDate.Year, userCoveragePlanCollection[0].CoverageDate.Month, Convert.ToInt32(day)),
                //                    LeaveTypeID = 2,
                //                    Remarks = "Market Off Day",
                //                };
                //                if (SmartDostDbContext.UserLeavePlans.FirstOrDefault(k => k.UserID == leavePlan.UserID && k.LeaveDate == leavePlan.LeaveDate) == null)
                //                {
                //                    SmartDostDbContext.UserLeavePlans.Add(leavePlan);
                //                }
                //            }
                //        }
                //    }
                //    foreach (CoveragePlan plan in userCoveragePlanCollection)
                //    {
                //        //var coverage = SmartDostDbContext.CoveragePlans.FirstOrDefault(k => EntityFunctions.TruncateTime(k.CoverageDate) == EntityFunctions.TruncateTime(plan.CoverageDate) && k.UserID == plan.UserID && k.StoreID == plan.StoreID && k.StatusID == 2);
                //        //if (coverage == null)
                //        //{
                //        SmartDostDbContext.CoveragePlans.Add(new CoveragePlan()
                //        {
                //            CompanyID = plan.CompanyID,
                //            UserID = plan.UserID,
                //            StoreID = plan.StoreID,
                //            CoverageDate = plan.CoverageDate.Date,
                //            IsCoverage = plan.IsCoverage,
                //            StatusID = 0,
                //            Remarks = plan.Remarks,
                //            CreatedDate = currentDateTime,
                //            CreatedBy = plan.CreatedBy,
                //        });
                //        //}
                //    }
                //    SmartDostDbContext.SaveChanges();
                //    scope.Complete();
                //    status = 1;
                //}
                //  return status;
            }
            else { return status; }
        }

        /// <summary>
        ///  Method to Get the Beat Details info on the basis of status id
        /// </summary>
        /// <param name="statusID">represent 0 if rejected, 1 if approved</param>
        /// <returns></returns>
        public IList<CoveragePlan> GetBeatInfoDetails(int statusID)
        {
            return SmartDostDbContext.CoveragePlans.Where(c => (EntityFunctions.TruncateTime(c.CoverageDate) >= EntityFunctions.TruncateTime(DateTime.Now) && EntityFunctions.TruncateTime(c.CoverageDate) <= EntityFunctions.AddDays(DateTime.Now, 15)) && c.IsCoverage == false && c.StatusID == statusID).ToList();
        }

        /// <summary>
        /// Approves/Rejectes beat submitted by user.
        /// </summary>
        /// <param name="coverageCollection">The coverage collection.</param>
        /// <returns></returns>
        public bool ApproveRejectBeat(Dictionary<int, int> coverageCollection)
        {
            bool isSuccess = false;
            SystemSetting systemSetting = SmartDostDbContext.SystemSettings.FirstOrDefault(k => k.SettingID > 0);

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
    new TransactionOptions
    {
        IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
    }))
            {
                foreach (KeyValuePair<int, int> item in coverageCollection)
                {
                    CoveragePlan coveragePlan = SmartDostDbContext.CoveragePlans.SingleOrDefault(c => c.CoverageID == item.Key);
                    if (coveragePlan != null && coveragePlan.CoverageID > 0)
                    {
                        DateTime firstwindowfirst = CustomFormatDate(systemSetting.CoveragePlanFirstWindow.Substring(0, 8));
                        DateTime firstwindowsecond = CustomFormatDate(systemSetting.CoveragePlanFirstWindow.Substring(9, 8));
                        DateTime secondwindowfirst = CustomFormatDate(systemSetting.CoveragePlanSecondWndow.Substring(0, 8));
                        DateTime secondwindowsecond = CustomFormatDate(systemSetting.CoveragePlanSecondWndow.Substring(9, 8));
                        coveragePlan.StatusID = item.Value;
                        if (coveragePlan.StatusID == 1)
                            coveragePlan.Remarks = "Beat Approved";
                        else if (coveragePlan.StatusID == 2)
                            coveragePlan.Remarks = "Beat Rejected";
                        coveragePlan.ModifiedDate = DateTime.Now.Date;

                        if (!(coveragePlan.CoverageDate.Date >= firstwindowfirst && coveragePlan.CoverageDate.Date <= firstwindowsecond) || !(coveragePlan.CoverageDate.Date >= secondwindowfirst && coveragePlan.CoverageDate.Date <= secondwindowsecond))
                        {
                            if (item.Value == 1)
                            {
                                SmartDostDbContext.UserSystemSettings.Add(new UserSystemSetting()
                                    {
                                        UserID = coveragePlan.UserID,
                                        IsAPKLoggingEnabled = false,
                                        CoverageExceptionWindow = null,
                                        IsCoverageException = true,
                                        CreatedDate = DateTime.Now,
                                        CreatedBy = coveragePlan.CreatedBy,
                                        ModifiedDate = null,
                                        ModifiedBy = null,
                                        IsDeleted = false
                                    });
                            }
                        }
                    }
                    isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;
                }
                scope.Complete();
            }
            return isSuccess;
        }

        /// <summary>
        /// Customise the format of date.
        /// </summary>
        /// <param name="date">The date.</param>
        /// <returns></returns>
        private DateTime CustomFormatDate(string date)
        {
            var dt = DateTime.ParseExact(date, "yy/MM/dd", CultureInfo.InvariantCulture).ToString("MM-dd-yyyy", CultureInfo.InvariantCulture);
            DateTime formattedDate = Convert.ToDateTime(dt);
            return formattedDate;
        }

        /// <summary>
        ///  Records which have Status id 2 will delete from the table.
        /// </summary>
        /// <param name="statusID">represent 0 if rejected, 1 if approved</param>
        /// <returns>Mehtod will return confirmation message if records deleted from the table.</returns>
        public string DeleteBeat(int statusID)
        {
            string statusMessage = string.Empty;

            if (statusID == 2)
            {
                var coveragePlanCollection = SmartDostDbContext.CoveragePlans.Where(w => w.StatusID == statusID);
                foreach (CoveragePlan coveragePlan in coveragePlanCollection)
                {
                    SmartDostDbContext.CoveragePlans.Remove(coveragePlan);
                }
                SmartDostDbContext.SaveChanges();
                return statusMessage = "Beat deleted!";
            }
            else
            {
                return statusMessage = "Beat not deleted!";
            }
        }

        /// <summary>
        /// Method to fetch pending coverage user list for a reporting user
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns coverage users</returns>
        public IList<vwPendingCoverageUser> GetCoverageUsers(long userID)
        {
            return SmartDostDbContext.vwPendingCoverageUsers.Where(k => k.ReportingUserID == userID && k.StatusID == 0).ToList();
        }

        /// <summary>
        /// Method to update pending user's coverage
        /// </summary>
        /// <param name="userIDList">user ID list</param>
        /// <param name="status">status</param>
        /// <returns>returns boolean status</returns>
        public bool UpdatePendingCoverage(List<long> userIDList, int status)
        {
            bool isSuccess = false;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
  new TransactionOptions
  {
      IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
  }))
            {
                foreach (long id in userIDList)
                {
                    var plans = SmartDostDbContext.CoveragePlans.Where(k => k.UserID == id && k.StatusID == 0).ToList();
                    if (plans != null && plans.Count > 0)
                    {
                        foreach (CoveragePlan plan in plans)
                        {
                            plan.ModifiedDate = System.DateTime.Now;
                            plan.StatusID = status;
                            if (plan.StatusID == (int)AspectEnums.BeatStatus.Approved)
                                plan.Remarks = "Beat Approved";
                            else if (plan.StatusID == 2)
                                plan.Remarks = "Beat Rejected";
                            SmartDostDbContext.Entry<CoveragePlan>(plan).State = System.Data.EntityState.Modified;

                        }
                        SmartDostDbContext.SaveChanges();
                    }
                    if (status == (int)AspectEnums.BeatStatus.Approved)
                    {
                        var userSettings = SmartDostDbContext.UserSystemSettings.FirstOrDefault(k => k.UserID == id && !k.IsDeleted);
                        if (userSettings != null)
                        {
                            userSettings.CoverageExceptionWindow = null;
                            userSettings.IsCoverageException = false;
                            userSettings.ModifiedDate = System.DateTime.Now;
                            SmartDostDbContext.Entry<UserSystemSetting>(userSettings).State = System.Data.EntityState.Modified;
                            SmartDostDbContext.SaveChanges();
                        }
                    }
                }
                scope.Complete();
                isSuccess = true;
            }
            return isSuccess;
        }

        /// <summary>
        /// Method to fetch user beat details
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns beat details</returns>
        public UserBeatDetailsDTO GetUserBeatDetails(long userID)
        {
            IList<UserBeatDetailDTO> finalList = new List<UserBeatDetailDTO>();
            UserBeatDetailsDTO beatDetail = new UserBeatDetailsDTO();
            //var itmsLst = SmartDostDbContext.vwUserBeatDetails.Where(k => k.UserID == userID).ToList();
            var itmsLst = SmartDostDbContext.SPUserBeatDetails(userID, (int)AspectEnums.BeatStatus.Pending).ToList();
            var coveragePlanCount = itmsLst.Count();
            var groupdedData = itmsLst.GroupBy(x => x.CoverageDate);
            var currentDate = groupdedData.OrderByDescending(x => x.Key).FirstOrDefault();
            foreach (var item in groupdedData)
            {
                UserBeatDetailDTO beat = new UserBeatDetailDTO();
                beat.CoverageDate = item.Key.ToString("dd MMM yyyy (dddd)");
                List<UserBeatStoreDetailDTO> storeData = new List<UserBeatStoreDetailDTO>();
                foreach (var subItem in item)
                {
                    UserBeatStoreDetailDTO store = new UserBeatStoreDetailDTO();
                    store.City = subItem.City;
                    store.StoreCode = subItem.StoreCode;
                    store.StoreName = subItem.StoreName;
                    store.StoreID = subItem.StoreID;
                    storeData.Add(store);
                }
                beat.StoreData = storeData;
                finalList.Add(beat);
            }
            //#region Split BY Date
            //foreach (var item in itmsLst)
            //{

            //    List<string> dateRange = new List<string>();
            //    //dateRange = item.DateRange.Split(',').ToList();
            //    //foreach (var item1 in dateRange)
            //    //{
            //    //    UserBeatDetailDTO btDetail = new UserBeatDetailDTO();
            //    //    btDetail.City = item.City;
            //    //    btDetail.DateRange = item1.Trim();
            //    //    btDetail.PlanMonth = item.PlanMonth;
            //    //    btDetail.StatusID = item.StatusID;
            //    //    btDetail.StoreCode = item.StoreCode;
            //    //    btDetail.StoreID = item.StoreID;
            //    //    btDetail.StoreName = item.StoreName;
            //    //    btDetail.UserID = item.UserID;
            //    //    finalList.Add(btDetail);
            //    //}

            //}
            //finalList = finalList.OrderBy(x => Convert.ToInt16(x.DateRange)).ToList();
            //#endregion

            //#region Count working Days
            //var query = from p in finalList
            //            group p by p.DateRange into groups
            //            select groups;
            //var workingDays = query.Count();
            //#endregion
            if (finalList.Count() > 0)
            {
                #region Count Leave Off
                var curDate = currentDate.Key;//DateTime.ParseExact(finalList[0].DateRange + " " + finalList[0].PlanMonth, "d MMM yyyy", null);
                int currentMonth = curDate.Month; //Convert.ToInt32(curDate.Month);
                int currentYear = curDate.Year;
                DateTime dtFrom = new DateTime(currentYear, currentMonth, 1);
                DateTime dtTo = new DateTime(currentYear, currentMonth, DateTime.DaysInMonth(currentYear, currentMonth));
                var leaves = SmartDostDbContext.UserLeavePlans.Where(x => x.UserID == userID && x.LeaveTypeID == 2 && EntityFunctions.TruncateTime(x.LeaveToDate) >= EntityFunctions.TruncateTime(dtFrom) && EntityFunctions.TruncateTime(x.LeaveToDate) <= EntityFunctions.TruncateTime(dtTo)).ToList();
                var leaveOff = leaves.Count();
                string leaveDetails = string.Empty;
                if (leaveOff > 0)
                {
                    leaves = leaves.OrderBy(x => x.LeaveToDate).ToList();
                    foreach (var l in leaves)
                    {
                        leaveDetails += l.LeaveToDate.ToString("dd MMM") + " " + l.LeaveToDate.DayOfWeek + ", ";
                    }
                    leaveDetails = leaveDetails.TrimEnd().TrimEnd(',');
                }
                #endregion

                beatDetail.UserBeatDetails = finalList;
                beatDetail.TotalWorkingDays = finalList.Count().ToString();
                beatDetail.TotalOutletPlanned = itmsLst.Count() != 0 ? itmsLst.Count().ToString() : "0";
                beatDetail.TotalOff = leaveOff.ToString();
                beatDetail.LeaveDetail = leaveDetails;
                beatDetail.TotalAssignedOutlet = SmartDostDbContext.vwStoreUsers.Where(X => X.UserID == userID && !X.IsDeleted).Select(x => x.StoreCode).Distinct().Count();
                //foreach (var item in finalList)
                //{
                //    var thisDate = DateTime.ParseExact(item.DateRange.Trim() + " " + item.PlanMonth, "d MMM yyyy", null);
                //    item.Day = thisDate.DayOfWeek.ToString();
                //}
                //beatDetail.UserBeatDetails = finalList.OrderBy(x=>x.;
            }
            return beatDetail;
        }

        /// <summary>
        /// Method to fetch approved user beat details
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <returns>returns beat details</returns>
        public UserBeatDetailsDTO GetApprovedUserBeatDetails(long userID)
        {
            // IList<UserBeatDetailDTO> finalList = new List<UserBeatDetailDTO>();
            //UserBeatDetailsDTO beatDetail = new UserBeatDetailsDTO();
            //var itmsLst = SmartDostDbContext.SPUserBeatDetails(userID,(int)AspectEnums.BeatStatus.Approved).ToList();
            ////var itmsLst = SmartDostDbContext.vwUserBeatDetailsApproves.Where(k => k.UserID == userID && k.StatusID == 1).ToList();

            //var coveragePlanCount = itmsLst.Count();
            //#region Split BY Date
            //foreach (var item in itmsLst)
            //{

            //    List<string> dateRange = new List<string>();
            //    dateRange = item.DateRange.Split(',').ToList();
            //    foreach (var item1 in dateRange)
            //    {
            //        UserBeatDetailDTO btDetail = new UserBeatDetailDTO();
            //        btDetail.City = item.City;
            //        btDetail.DateRange = item1.Trim();
            //        btDetail.PlanMonth = item.PlanMonth;
            //        btDetail.StatusID = item.StatusID;
            //        btDetail.StoreCode = item.StoreCode;
            //        btDetail.StoreID = item.StoreID;
            //        btDetail.StoreName = item.StoreName;
            //        btDetail.UserID = item.UserID;
            //        finalList.Add(btDetail);
            //    }

            //}
            //finalList = finalList.OrderBy(x => Convert.ToInt16(x.DateRange)).ToList();
            //#endregion

            //#region Count working Days
            //var query = from p in finalList
            //            group p by p.DateRange into groups
            //            select groups;
            //var workingDays = query.Count();
            //#endregion
            //if (finalList.Count() > 0)
            //{
            //    #region Count Leave Off
            //    var curDate = DateTime.ParseExact(finalList[0].DateRange + " " + finalList[0].PlanMonth, "d MMM yyyy", null);
            //    int currentMonth = curDate.Month; //Convert.ToInt32(curDate.Month);
            //    int currentYear = curDate.Year;
            //    DateTime dtFrom = new DateTime(currentYear, currentMonth, 1);
            //    DateTime dtTo = new DateTime(currentYear, currentMonth, DateTime.DaysInMonth(currentYear, currentMonth));
            //    var leaves = SmartDostDbContext.UserLeavePlans.Where(x => x.UserID == userID && x.LeaveTypeID == 2 && EntityFunctions.TruncateTime(x.LeaveToDate) >= EntityFunctions.TruncateTime(dtFrom) && EntityFunctions.TruncateTime(x.LeaveToDate) <= EntityFunctions.TruncateTime(dtTo)).ToList();
            //    var leaveOff = leaves.Count();
            //    string leaveDetails = string.Empty;
            //    if (leaveOff > 0)
            //    {
            //        leaves = leaves.OrderBy(x=>x.LeaveToDate).ToList();
            //        foreach (var l in leaves)
            //        {
            //            leaveDetails += l.LeaveToDate.ToString("dd MMM") + " " + l.LeaveToDate.DayOfWeek + ", ";
            //        }
            //        leaveDetails = leaveDetails.TrimEnd().TrimEnd(',');
            //    }
            //    #endregion

            //    beatDetail.UserBeatDetails = finalList;
            //    beatDetail.TotalWorkingDays = workingDays.ToString();
            //    beatDetail.TotalOutletPlanned = itmsLst.Count() != 0 ? itmsLst.Count().ToString() : "0";
            //    beatDetail.TotalOff = leaveOff.ToString();
            //    beatDetail.LeaveDetail = leaveDetails;
            //    beatDetail.TotalAssignedOutlet = SmartDostDbContext.vwStoreUsers.Where(X => X.UserID == userID && !X.IsDeleted).Select(x => x.StoreCode).Distinct().Count();
            //    foreach (var item in finalList)
            //    {
            //        var thisDate = DateTime.ParseExact(item.DateRange.Trim() + " " + item.PlanMonth, "d MMM yyyy", null);
            //        item.Day = thisDate.DayOfWeek.ToString();
            //    }

            //}
            //beatDetail.UserBeatDetails = beatDetail.UserBeatDetails.OrderBy(x => x.PlanMonth).ToList();

            IList<UserBeatDetailDTO> finalList = new List<UserBeatDetailDTO>();
            UserBeatDetailsDTO beatDetail = new UserBeatDetailsDTO();
            //var itmsLst = SmartDostDbContext.vwUserBeatDetails.Where(k => k.UserID == userID).ToList();
            var itmsLst = SmartDostDbContext.SPUserBeatDetails(userID, (int)AspectEnums.BeatStatus.Approved).ToList();
            var coveragePlanCount = itmsLst.Count();
            var groupdedData = itmsLst.GroupBy(x => x.CoverageDate);
            var currentDate = groupdedData.OrderByDescending(x => x.Key).FirstOrDefault();
            foreach (var item in groupdedData)
            {
                UserBeatDetailDTO beat = new UserBeatDetailDTO();
                beat.CoverageDate = item.Key.ToString("dd MMM yyyy (dddd)");
                List<UserBeatStoreDetailDTO> storeData = new List<UserBeatStoreDetailDTO>();
                foreach (var subItem in item)
                {
                    UserBeatStoreDetailDTO store = new UserBeatStoreDetailDTO();
                    store.City = subItem.City;
                    store.StoreCode = subItem.StoreCode;
                    store.StoreName = subItem.StoreName;
                    store.StoreID = subItem.StoreID;
                    storeData.Add(store);
                }
                beat.StoreData = storeData; //Dhiraj Tesintg
                finalList.Add(beat);
            }
            //#region Split BY Date
            //foreach (var item in itmsLst)
            //{

            //    List<string> dateRange = new List<string>();
            //    //dateRange = item.DateRange.Split(',').ToList();
            //    //foreach (var item1 in dateRange)
            //    //{
            //    //    UserBeatDetailDTO btDetail = new UserBeatDetailDTO();
            //    //    btDetail.City = item.City;
            //    //    btDetail.DateRange = item1.Trim();
            //    //    btDetail.PlanMonth = item.PlanMonth;
            //    //    btDetail.StatusID = item.StatusID;
            //    //    btDetail.StoreCode = item.StoreCode;
            //    //    btDetail.StoreID = item.StoreID;
            //    //    btDetail.StoreName = item.StoreName;
            //    //    btDetail.UserID = item.UserID;
            //    //    finalList.Add(btDetail);
            //    //}

            //}
            //finalList = finalList.OrderBy(x => Convert.ToInt16(x.DateRange)).ToList();
            //#endregion

            //#region Count working Days
            //var query = from p in finalList
            //            group p by p.DateRange into groups
            //            select groups;
            //var workingDays = query.Count();
            //#endregion
            if (finalList.Count() > 0)
            {
                #region Count Leave Off
                var curDate = currentDate.Key;//DateTime.ParseExact(finalList[0].DateRange + " " + finalList[0].PlanMonth, "d MMM yyyy", null);
                int currentMonth = curDate.Month; //Convert.ToInt32(curDate.Month);
                int currentYear = curDate.Year;
                DateTime dtFrom = new DateTime(currentYear, currentMonth, 1);
                DateTime dtTo = new DateTime(currentYear, currentMonth, DateTime.DaysInMonth(currentYear, currentMonth));
                var leaves = SmartDostDbContext.UserLeavePlans.Where(x => x.UserID == userID && x.LeaveTypeID == 2 && EntityFunctions.TruncateTime(x.LeaveToDate) >= EntityFunctions.TruncateTime(dtFrom) && EntityFunctions.TruncateTime(x.LeaveToDate) <= EntityFunctions.TruncateTime(dtTo)).ToList();
                var leaveOff = leaves.Count();
                string leaveDetails = string.Empty;
                if (leaveOff > 0)
                {
                    leaves = leaves.OrderBy(x => x.LeaveToDate).ToList();
                    foreach (var l in leaves)
                    {
                        leaveDetails += l.LeaveToDate.ToString("dd MMM") + " " + l.LeaveToDate.DayOfWeek + ", ";
                    }
                    leaveDetails = leaveDetails.TrimEnd().TrimEnd(',');
                }
                #endregion

                beatDetail.UserBeatDetails = finalList;
                beatDetail.TotalWorkingDays = finalList.Count().ToString();
                beatDetail.TotalOutletPlanned = itmsLst.Count() != 0 ? itmsLst.Count().ToString() : "0";
                beatDetail.TotalOff = leaveOff.ToString();
                beatDetail.LeaveDetail = leaveDetails;
                beatDetail.TotalAssignedOutlet = SmartDostDbContext.vwStoreUsers.Where(X => X.UserID == userID && !X.IsDeleted).Select(x => x.StoreCode).Distinct().Count();
            }
            return beatDetail;


        }

        ///<param name="userBeatCollection">Beat collection info of the user</param>
        /// <returns>return true if entry inserted else false</returns>
        ///  </summary>
        public bool InsertBeatException(List<UserSystemSetting> userSystemSettings, long currentUserId)
        {
            bool isSuccess = false;
            if (userSystemSettings != null && userSystemSettings.Count > 0)
            {
                var sysetemSet = SmartDostDbContext.UserSystemSettings.ToList();
                var userSystemSettingsForUsers = (from setting in sysetemSet
                                                  join user in userSystemSettings
                                                  on setting.UserID equals user.UserID
                                                  where !setting.IsDeleted
                                                  select setting).ToList();
                using (TransactionScope scope = new TransactionScope())
                {

                    foreach (var userSystemSetting in userSystemSettings)
                    {

                        isSuccess = InsertException(currentUserId, isSuccess, userSystemSettingsForUsers, userSystemSetting, userSystemSetting.UserID);


                        ////Changes done to add exception for reporting userid too
                        //var reportingUser = SmartDostDbContext.UserRoles.Where(x => x.UserID == userSystemSetting.UserID && x.IsActive && !x.IsDeleted).FirstOrDefault();
                        //if (reportingUser != null)
                        //{
                        //    if (reportingUser.ReportingUserID.HasValue)
                        //        isSuccess = InsertException(currentUserId, isSuccess, userSystemSettingsForUsers, userSystemSetting, reportingUser.ReportingUserID.Value);
                        //}

                    }
                    scope.Complete();
                }
            }

            return isSuccess;
        }

        private bool InsertException(long currentUserId, bool isSuccess, List<UserSystemSetting> userSystemSettingsForUsers, UserSystemSetting userSystemSetting, long userID)
        {
            var systemSetting = SmartDostDbContext.UserSystemSettings.FirstOrDefault(x => x.UserID == userID && x.IsDeleted == false);
            if (systemSetting != null)
            {
                systemSetting.IsCoverageException = userSystemSetting.IsCoverageException;
                systemSetting.CoverageExceptionWindow = userSystemSetting.CoverageExceptionWindow;
                systemSetting.ModifiedDate = System.DateTime.Now;
                systemSetting.ModifiedBy = currentUserId;
                systemSetting.UserID = userID;
                SmartDostDbContext.Entry<UserSystemSetting>(systemSetting).State = System.Data.EntityState.Modified;
            }
            else
            {
                userSystemSetting.UserID = userID;
                userSystemSetting.CreatedDate = DateTime.Now;
                userSystemSetting.CreatedBy = currentUserId;
                SmartDostDbContext.UserSystemSettings.Add(userSystemSetting);
            }
            isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;
            return isSuccess;
        }

        /// <summary>
        /// Method to override user saved beats for the month
        /// </summary>
        /// <param name="userCoveragePlanCollection">user coverage plan collection</param>
        /// <returns>returns status</returns>
        private int SaveExceptionUserBeats(List<CoveragePlan> userCoveragePlanCollection)
        {
            DateTime currentDateTime = DateTime.Now;
            int status = 1;
            long currentUserId = userCoveragePlanCollection[0].UserID;
                        
            var xmlfromLINQ = new XElement("CoveragePlans",
                      from c in userCoveragePlanCollection                      
                      select new XElement("CoveragePlan",                                                      
                new XElement("StoreID",c.StoreID ),
                new XElement("CoverageDate",c.CoverageDate ),
                new XElement("IsCoverage",c.IsCoverage ),
                new XElement("StatusID",c.StatusID ),
                new XElement("Remarks",c.Remarks )             
                ));


            SmartDostDbContext.SPSaveExceptionUserBeats(xmlfromLINQ.ToString(), (string)userCoveragePlanCollection[0].MarketOffDays, (int)currentUserId);
                return status;
    //         using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
    //new TransactionOptions
    //{
    //    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
    //}))
    //         {
    //            int month = userCoveragePlanCollection[0].CoverageDate.Month;
    //            int year = userCoveragePlanCollection[0].CoverageDate.Year;
    //            DateTime dtNow = DateTime.Now.Date.AddDays(1);
    //            #region Earlier Market Off Delete as suggested by client on 13-Feb-2014 by Dhiraj
    //            //List<UserLeavePlan> earlierMarketOff = SmartDostDbContext.UserLeavePlans.Where(k => k.LeaveDate.Month == month && k.UserID == currentUserId && k.LeaveDate.Year == year && k.LeaveTypeID == 2).ToList();
    //            //Added date check to remove leave greater than equal to current date
    //            List<UserLeavePlan> earlierMarketOff = SmartDostDbContext.UserLeavePlans.Where(k => k.LeaveDate.Month == month && k.UserID == currentUserId && k.LeaveDate.Year == year && k.LeaveTypeID == 2 && k.LeaveDate >= dtNow).ToList();
    //            foreach (var item in earlierMarketOff)
    //            {
    //                SmartDostDbContext.UserLeavePlans.Remove(item);
    //            }
    //            SmartDostDbContext.SaveChanges();
    //            #endregion

    //            //List<CoveragePlan> earlierPlans = SmartDostDbContext.CoveragePlans.Where(k => k.CoverageDate.Month == month && k.UserID == currentUserId && k.CoverageDate.Year == year).ToList();
    //            //Added date check to oreride beat plan greater than equal to current date
    //            List<CoveragePlan> earlierPlans = SmartDostDbContext.CoveragePlans.Where(k => k.CoverageDate.Month == month && k.UserID == currentUserId && k.CoverageDate.Year == year && k.CoverageDate >= dtNow).ToList();
    //            if (earlierPlans != null)
    //            {
    //                foreach (var item in earlierPlans)
    //                {
    //                    //if (item.StatusID == 1)
    //                    //{
    //                    item.StatusID = 2;
    //                    item.Remarks = "Beat overriden by user";
    //                    item.ModifiedDate = currentDateTime;
    //                    item.ModifiedBy = Convert.ToInt32(currentUserId);
    //                    SmartDostDbContext.Entry<CoveragePlan>(item).State = System.Data.EntityState.Modified;
    //                }
    //            }
    //            if (!String.IsNullOrEmpty(userCoveragePlanCollection[0].MarketOffDays))
    //            {
    //                string marketOffDays = userCoveragePlanCollection[0].MarketOffDays;
    //                string[] marketArray = marketOffDays.Split(',');
    //                if (marketArray.Length > 0)
    //                {
    //                    foreach (string day in marketArray)
    //                    {
    //                        UserLeavePlan leavePlan = new UserLeavePlan()
    //                        {
    //                            LeaveToDate = new DateTime(userCoveragePlanCollection[0].CoverageDate.Year, userCoveragePlanCollection[0].CoverageDate.Month, Convert.ToInt32(day)),
    //                            UserID = userCoveragePlanCollection[0].UserID,
    //                            CreatedDate = currentDateTime,
    //                            CreatedBy = userCoveragePlanCollection[0].UserID,
    //                            IsDeleted = false,
    //                            LeaveDate = new DateTime(userCoveragePlanCollection[0].CoverageDate.Year, userCoveragePlanCollection[0].CoverageDate.Month, Convert.ToInt32(day)),
    //                            LeaveTypeID = 2,
    //                            Remarks = "Market Off Day",
    //                        };
    //                        if (SmartDostDbContext.UserLeavePlans.FirstOrDefault(k => k.UserID == leavePlan.UserID && k.LeaveDate == leavePlan.LeaveDate) == null)
    //                        {
    //                            SmartDostDbContext.UserLeavePlans.Add(leavePlan);
    //                        }
    //                    }
    //                }
    //            }
    //            foreach (CoveragePlan plan in userCoveragePlanCollection)
    //            {
    //                //var coverage = SmartDostDbContext.CoveragePlans.FirstOrDefault(k => EntityFunctions.TruncateTime(k.CoverageDate) == EntityFunctions.TruncateTime(plan.CoverageDate) && k.UserID == plan.UserID && k.StoreID == plan.StoreID && k.StatusID == 2);
    //                //if (coverage == null)
    //                //{
    //                SmartDostDbContext.CoveragePlans.Add(new CoveragePlan()
    //                {
    //                    CompanyID = plan.CompanyID,
    //                    UserID = plan.UserID,
    //                    StoreID = plan.StoreID,
    //                    CoverageDate = plan.CoverageDate.Date,
    //                    IsCoverage = plan.IsCoverage,
    //                    StatusID = 0,
    //                    Remarks = plan.Remarks,
    //                    CreatedDate = currentDateTime,
    //                    CreatedBy = plan.CreatedBy,
    //                });
    //                }
    //           // }
    //            SmartDostDbContext.SaveChanges();
    //            scope.Complete();
    //            status = 1;
    //        }
            
        }
    }
}
