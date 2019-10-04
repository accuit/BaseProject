using System;
using System.Collections.Generic;
using System.Linq;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using Samsung.SmartDost.PersistenceLayer.Repository.Contracts;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using Samsung.SmartDost.CommonLayer.Aspects.Security;
using System.Data.Objects;
using System.Transactions;

namespace Samsung.SmartDost.PersistenceLayer.Data.Impl
{
    /// <summary>
    /// Store Data Layer implementation
    /// </summary>
    public class StoreDataImpl : BaseDataImpl, IStoreRepository
    {

        // This code is commented by Amit, now this function is converted into SP for optimized result see below function  :-AM1030
        ///// <summary>
        ///// Displays the store profile.
        ///// </summary>
        ///// <param name="storeID">The store identifier.</param>
        ///// <returns></returns>
        //public StoreProfile DisplayStoreProfile(int storeID, long userID)
        //{
        //    StoreProfile storeDetail = new StoreProfile();
        //    if (storeID > 0)
        //    {
        //        StoreParentMapping parentDetails = null;
        //        UserMaster userDetails = SmartDostDbContext.UserMasters.FirstOrDefault(k => k.UserID == userID);
        //        if (userDetails != null)
        //        {
        //            string partnerCode = userDetails.DistyCode;
        //            if (!string.IsNullOrEmpty(partnerCode))
        //            {
        //                parentDetails = SmartDostDbContext.StoreParentMappings.FirstOrDefault(k => k.StoreID == storeID && k.ParentCode == partnerCode && (k.IsDeleted == null || k.IsDeleted == false));
        //            }
        //            storeDetail = (from St in SmartDostDbContext.StoreMasters
        //                           join S in SmartDostDbContext.StoreUsers on St.StoreID equals S.StoreID
        //                           join SMA in SmartDostDbContext.StoreMasterAttributes on St.StoreCode equals SMA.StoreCode //SDCE-684 New add Modifiy by Niranjan (Channel Type Status) 21-10-2014
        //                           join fgt in SmartDostDbContext.FreezeGeoTags on St.StoreID equals fgt.StoreID
        //                            into data
        //                           from customData in data.DefaultIfEmpty()
        //                           where S.UserID == userID && St.StoreID == storeID && S.IsDeleted == false && St.IsActive == true
        //                           select new StoreProfile
        //                           {
        //                               StoreID = St.StoreID,
        //                               CompanyID = St.CompanyID,
        //                               State = St.State,

        //                               AccId = St.AccId,
        //                               //SDCE-684 New add Modifiy by Niranjan (Channel Type Status) 21-10-2014
        //                               ChannelType = SMA.Attributes1 + ' ' + SMA.Attributes2,
        //                               //ChannelType = St.ChannelType,//SDCE-684 Comment Code
        //                               ContactPerson = St.ContactPerson,
        //                               MobileNo = St.MobileNo,
        //                               EmailID = St.EmailID,
        //                               StoreCode = St.StoreCode,
        //                               StoreName = St.StoreName,
        //                               GeoTagCount = St.GeoTagCount,
        //                               City = St.City,

        //                               CreatedDate = St.CreatedDate,
        //                               CreatedBy = St.CreatedBy,
        //                               ModifiedDate = St.ModifiedDate,
        //                               ModifiedBy = St.ModifiedBy,
        //                               IsActive = St.IsActive,
        //                               IsDeleted = St.IsDeleted,
        //                               Lattitude = St.Lattitude,
        //                               Longitude = St.Longitude,
        //                               PictureFileName = St.PictureFileName,
        //                               IsSynced = St.IsSynced,
        //                               SyncDate = St.SyncDate,
        //                               UserRoleID = S.UserRoleID,
        //                               LastVisitDate = S.LastVisitDate,
        //                               VisitSummary = S.VisitSummary,
        //                               IsFreeze = customData == null ? false : true,
        //                               FreezeLattitude = customData.Lattitude,
        //                               FreezeLongitude = customData.Longitude
        //                           }).FirstOrDefault();
        //            if (storeDetail != null)
        //            {
        //                storeDetail.ShipToRegion = parentDetails != null ? parentDetails.ShipToRegion : string.Empty;
        //                storeDetail.SoldToCode = parentDetails != null ? parentDetails.SoldToCode : string.Empty;
        //                storeDetail.ShipToCode = parentDetails != null ? parentDetails.ShipToCode : string.Empty;
        //                storeDetail.ShipToBranch = parentDetails != null ? parentDetails.ShipToBranch : string.Empty;
        //                storeDetail.AccountName = parentDetails != null ? parentDetails.AccountName : string.Empty;
        //                storeDetail.ParentCode = parentDetails != null ? parentDetails.ParentCode : string.Empty;
        //                storeDetail.ShipToName = parentDetails != null ? parentDetails.ShipToName : string.Empty;

        //                DateTime currentDate = System.DateTime.Today;
        //                DateTime fromDate = new DateTime(currentDate.Year, currentDate.Month, 1);

        //                DateTime LMcurrentDate = fromDate.AddDays(-1); //System.DateTime.Today.AddMonths(-1) ;
        //                DateTime LMfromDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-1);

        //                UserRole userRole = SmartDostDbContext.UserRoles.FirstOrDefault(k => !k.IsDeleted && k.UserID == userID);
        //                if (userRole != null)
        //                {
        //                    storeDetail.UserRoleID = userRole.UserRoleID;
        //                }
        //                var storeMTDSells = SmartDostDbContext.StoreSells.Where(k => k.StoreID == storeDetail.StoreID && EntityFunctions.TruncateTime(k.SellDate) >= fromDate && EntityFunctions.TruncateTime(k.SellDate) <= currentDate);
        //                if (storeMTDSells != null)
        //                {
        //                    var totalSells = (from k in storeMTDSells
        //                                      join p in SmartDostDbContext.ProductMasters on k.ProductID equals p.ProductID
        //                                      select new { Sells = k, ProductTypeCode = p.ProductTypeCode }).ToList();
        //                    var mtdPurchase = totalSells.Where(k => k.ProductTypeCode == "AV").Sum(k => k.Sells.MTDSellInValue);
        //                    var mtdSale = totalSells.Where(k => k.ProductTypeCode == "AV").Sum(k => k.Sells.MTDSellOutvalue);
        //                    var mtdHAPurchase = totalSells.Where(k => k.ProductTypeCode == "HA").Sum(k => k.Sells.MTDSellInValue);
        //                    var mtdHASale = totalSells.Where(k => k.ProductTypeCode == "HA").Sum(k => k.Sells.MTDSellOutvalue);
        //                    var mtdACPurchase = totalSells.Where(k => k.ProductTypeCode == "AC").Sum(k => k.Sells.MTDSellInValue);
        //                    var mtdACSale = totalSells.Where(k => k.ProductTypeCode == "AC").Sum(k => k.Sells.MTDSellOutvalue);
        //                    //    storeDetail.MTDPurchase = mtdPurchase.HasValue ? mtdPurchase.Value : 0;
        //                    //    storeDetail.MTDSale = mtdSale.HasValue ? mtdSale.Value : 0;
        //                    //    storeDetail.HAMTDPurchase = mtdHAPurchase.HasValue ? mtdPurchase.Value : 0;
        //                    //    storeDetail.HAMTDSale = mtdHASale.HasValue ? mtdSale.Value : 0;
        //                    //    storeDetail.ACMTDPurchase = mtdACPurchase.HasValue ? mtdPurchase.Value : 0;
        //                    //    storeDetail.ACMTDSale = mtdACSale.HasValue ? mtdSale.Value : 0;

        //                    storeDetail.AVMTDPurchase = mtdPurchase;
        //                    storeDetail.AVMTDSale = mtdSale;
        //                    storeDetail.HAMTDPurchase = mtdHAPurchase;
        //                    storeDetail.HAMTDSale = mtdHASale;
        //                    storeDetail.ACMTDPurchase = mtdACPurchase;
        //                    storeDetail.ACMTDSale = mtdACSale;
        //                }


        //                var storeLMTDSells = SmartDostDbContext.StoreSells.Where(k => k.StoreID == storeDetail.StoreID && EntityFunctions.TruncateTime(k.SellDate) >= LMfromDate && EntityFunctions.TruncateTime(k.SellDate) <= LMcurrentDate);
        //                if (storeLMTDSells != null)
        //                {
        //                    var totalSells = (from k in storeLMTDSells
        //                                      join p in SmartDostDbContext.ProductMasters on k.ProductID equals p.ProductID
        //                                      select new { Sells = k, ProductTypeCode = p.ProductTypeCode }).ToList();
        //                    var mtdPurchase = totalSells.Where(k => k.ProductTypeCode == "AV").Sum(k => k.Sells.MTDSellInValue);
        //                    var mtdSale = totalSells.Where(k => k.ProductTypeCode == "AV").Sum(k => k.Sells.MTDSellOutvalue);
        //                    var mtdHAPurchase = totalSells.Where(k => k.ProductTypeCode == "HA").Sum(k => k.Sells.MTDSellInValue);
        //                    var mtdHASale = totalSells.Where(k => k.ProductTypeCode == "HA").Sum(k => k.Sells.MTDSellOutvalue);
        //                    var mtdACPurchase = totalSells.Where(k => k.ProductTypeCode == "AC").Sum(k => k.Sells.MTDSellInValue);
        //                    var mtdACSale = totalSells.Where(k => k.ProductTypeCode == "AC").Sum(k => k.Sells.MTDSellOutvalue);
        //                    //    storeDetail.MTDPurchase = mtdPurchase.HasValue ? mtdPurchase.Value : 0;
        //                    //    storeDetail.MTDSale = mtdSale.HasValue ? mtdSale.Value : 0;
        //                    //    storeDetail.HAMTDPurchase = mtdHAPurchase.HasValue ? mtdPurchase.Value : 0;
        //                    //    storeDetail.HAMTDSale = mtdHASale.HasValue ? mtdSale.Value : 0;
        //                    //    storeDetail.ACMTDPurchase = mtdACPurchase.HasValue ? mtdPurchase.Value : 0;
        //                    //    storeDetail.ACMTDSale = mtdACSale.HasValue ? mtdSale.Value : 0;

        //                    storeDetail.AVLMTDPurchase = mtdPurchase;
        //                    storeDetail.AVLMTDSale = mtdSale;
        //                    storeDetail.HALMTDPurchase = mtdHAPurchase;
        //                    storeDetail.HALMTDSale = mtdHASale;
        //                    storeDetail.ACLMTDPurchase = mtdACPurchase;
        //                    storeDetail.ACLMTDSale = mtdACSale;
        //                }

        //                var storeTargets = SmartDostDbContext.StoreTargets.FirstOrDefault(k => k.StoreID == storeID);
        //                if (storeTargets != null)
        //                {
        //                    storeDetail.Target = storeTargets.Target.HasValue ? storeTargets.Target.Value : 0;
        //                    storeDetail.ACH = storeTargets.ACH.HasValue ? storeTargets.ACH.Value : 0;
        //                }
        //                //SDCE-684 New add Modifiy by Niranjan (Channel Type Status) 21-10-2014
        //                var storeChannelTypeStatus = SmartDostDbContext.ChannelTypeDisplays.Where(k => k.ChannelType == storeDetail.ChannelType).FirstOrDefault();
        //                if (storeChannelTypeStatus != null)
        //                {
        //                    storeDetail.IsDisplayCounterShare = storeChannelTypeStatus.IsDisplayCounterShare;
        //                    storeDetail.IsPlanogram = storeChannelTypeStatus.IsPlanogram;
        //                }
        //                //SDCE-684
        //            }
        //        }

        //    }


        //    return storeDetail;
        //}

        /// <summary>
        /// Displays the store profile & Today Beat if store id is passed null ( :-AM1030 )
        /// </summary>
        /// <param name="storeID">The store identifier.</param>
        /// <returns></returns>
        /// 
        public List<SPDisplayStoreProfile_Result> DisplayStoreProfile(int? storeID, long userID)
        {
            return SmartDostDbContext.SPDisplayStoreProfile(userID, storeID).ToList();
        }

        /// <summary>
        /// Updates the store profile.
        /// </summary>
        /// <param name="storeID">The store identifier.</param>
        /// <param name="contactPerson">The contact person.</param>
        /// <param name="mobileNo">The mobile no.</param>
        /// <param name="emailID">The email identifier.</param>
        /// <returns></returns>
        public StoreMaster UpdateStoreProfile(int storeID, string contactPerson, string mobileNo, string emailID, string imageName,string storeAddress)
        {
            StoreMaster store = new StoreMaster();
            if (storeID > 0)
                store = SmartDostDbContext.StoreMasters.FirstOrDefault(s => s.StoreID == storeID);

            if (store != null)
            {
                store.MobileNo = EncryptionEngine.EncryptString(mobileNo);
                store.EmailID = EncryptionEngine.EncryptString(emailID);
                store.ContactPerson = contactPerson;
                store.ModifiedDate = System.DateTime.Now;
               // store.StoreAddress = storeAddress; commented after discussion with race team
                if (!String.IsNullOrEmpty(imageName))
                {
                    store.PictureFileName = imageName;
                }
                SmartDostDbContext.Entry<StoreMaster>(store).State = System.Data.EntityState.Modified;
                SmartDostDbContext.SaveChanges();
            }
            return store;
        }

        /// <summary>
        /// return Partner list Based on City and user
        /// </summary>
        /// <param name="userid">User Id</param>   
        /// <returns>return list of partner  Based on  to userid and city</returns>
        public IList<PartnerList> GetpartnerList(long userId)
        {
            IList<PartnerList> partner = new List<PartnerList>();
            //IList<string> cities = GetCityList(userId);
            //foreach (var result in cities)
            //{
            UserMaster userDetails = SmartDostDbContext.UserMasters.FirstOrDefault(k => k.UserID == userId);
            if (userDetails != null)
            {
                var query = (from S in SmartDostDbContext.StoreUsers
                             join St in SmartDostDbContext.StoreMasters on S.StoreID equals St.StoreID
                             join pt in SmartDostDbContext.StoreParentMappings on St.StoreID equals pt.StoreID
                             where S.UserID == userId && S.IsDeleted == false && St.IsActive == true && (!pt.IsDeleted.HasValue || !pt.IsDeleted.Value)
                             select new { St.StoreID, pt.ShipToName, St.City, pt.ShipToCode, pt.ParentCode });
                if (string.IsNullOrEmpty(userDetails.DistyCode))
                {
                    var results = query.ToList();
                    foreach (var item in results)
                    {
                        partner.Add(new PartnerList() { City = string.IsNullOrEmpty(item.City) ? "" : item.City.ToUpper(), ShipToName = item.ShipToName, ShipToCode = item.ShipToCode });
                    }
                }
                else
                {
                    var results = query.Where(k => k.ParentCode == userDetails.DistyCode).ToList();
                    foreach (var item in results)
                    {
                        partner.Add(new PartnerList() { City = string.IsNullOrEmpty(item.City) ? "" : item.City.ToUpper(), ShipToName = item.ShipToName, ShipToCode = item.ShipToCode });
                    }
                }
            }
            //}


            return partner;

        }

        /// <summary>
        /// Method to return Dealer list Based on city
        /// </summary>
        /// <param name="city">City</param>
        public IList<SPGetAssignedDealers_Result> DealersListBasedOnCity(long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;

            List<SPGetAssignedDealers_Result> dealer = new List<SPGetAssignedDealers_Result>();
            if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.OtherBeat)).FirstOrDefault() == true)
            {                

                if (LastUpdatedDate == null)
                {
                    dealer = SmartDostDbContext.SPGetAssignedDealers(userID,0).ToList()                    
                       .OrderBy(k => k.MaxModifiedDate)
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       
                }
                else
                {

                    dealer = SmartDostDbContext.SPGetAssignedDealers(userID, 2).ToList()
                    .Where(k=>                    
                        ((k.MaxModifiedDate > LastUpdatedDate)
                        ||
                        (k.MaxModifiedDate == LastUpdatedDate.Value)
                        ))
                    .OrderBy(k => k.MaxModifiedDate)
                    .Skip(StartRowIndex)
                    .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                   
                }

                HasMoreRows = dealer.Count > RowCount ? true : false;
                dealer = dealer.Take(RowCount).ToList();

                // Update last modified data among the data if available, else send the same modifieddate back  
                if (dealer.Count > 0)
                {
                    //MaxModifiedDate = dealer.Max(x => x.MaxModifiedDate);

                    if (LastUpdatedDate == null && HasMoreRows == true)
                        MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
                    else if (LastUpdatedDate == null && HasMoreRows == false)
                        MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
                    else
                        MaxModifiedDate = dealer.Max(x => x.MaxModifiedDate);
                }

                
            }
            return dealer;
        }

        /// <summary>
        /// return Partner Details Based on UserID and partnetID
        /// </summary>
        /// <param name="userid">User Id</param>   
        /// <returns>return Partner Details  Based on  to userID and partnerID</returns>
        public StoreProfile DisplayPartnerDetails(long userID, long partnerID, string shipToCode)
        {
            StoreProfile storeDetails = null;
            StoreParentMapping parentDetails = null;
            var store = SmartDostDbContext.StoreMasters.FirstOrDefault(s => s.StoreCode == shipToCode && !s.IsDeleted);
            if (userID > 0 && store != null)
            {
                UserMaster userDetails = SmartDostDbContext.UserMasters.FirstOrDefault(k => k.UserID == userID);
                if (!string.IsNullOrEmpty(userDetails.DistyCode))
                {
                    parentDetails = SmartDostDbContext.StoreParentMappings.FirstOrDefault(k => k.StoreID == store.StoreID && k.ParentCode == userDetails.DistyCode && (!k.IsDeleted.HasValue || !k.IsDeleted.Value));
                }
            }
            storeDetails = new StoreProfile
                               {
                                   StoreID = store.StoreID,
                                   CompanyID = store.CompanyID,
                                   ShipToRegion = parentDetails != null ? parentDetails.ShipToRegion : string.Empty,
                                   ShipToBranch = parentDetails != null ? parentDetails.ShipToBranch : string.Empty,
                                   State = store.State,
                                   SoldToCode = parentDetails != null ? parentDetails.SoldToCode : string.Empty,
                                   ShipToCode = parentDetails != null ? parentDetails.ShipToCode : string.Empty,
                                   AccId = store.AccId,
                                   ChannelType = store.ChannelType,
                                   ContactPerson = store.ContactPerson,
                                   MobileNo = store.MobileNo,
                                   EmailID = store.EmailID,
                                   StoreCode = store.StoreCode,
                                   StoreName = store.StoreName,
                                   AccountName = parentDetails != null ? parentDetails.AccountName : string.Empty,
                                   ParentCode = parentDetails != null ? parentDetails.ParentCode : string.Empty,
                                   City = store.City,
                                   ShipToName = parentDetails != null ? parentDetails.ShipToName : string.Empty,
                                   CreatedDate = store.CreatedDate,
                                   CreatedBy = store.CreatedBy,
                                   ModifiedDate = store.ModifiedDate,
                                   ModifiedBy = store.ModifiedBy,
                                   IsActive = store.IsActive,
                                   IsDeleted = store.IsDeleted,
                                   Lattitude = store.Lattitude,
                                   Longitude = store.Longitude,
                                   PictureFileName = store.PictureFileName,
                                   IsSynced = store.IsSynced,
                                   SyncDate = store.SyncDate,

                               };
            DateTime currentDate = System.DateTime.Today;
            DateTime fromDate = new DateTime(currentDate.Year, currentDate.Month, 1);
            PartnerMeeting meeting = SmartDostDbContext.PartnerMeetings.Where(k => k.StoreID == storeDetails.StoreID && !k.IsDeleted).OrderByDescending(k => k.MeetingDate).FirstOrDefault();
            if (meeting != null)
            {
                storeDetails.LastVisitDate = meeting.MeetingDate;
                storeDetails.VisitSummary = meeting.Remarks;
            }
            var storeMTDSells = SmartDostDbContext.StoreSells.Where(k => k.StoreID == store.StoreID && EntityFunctions.TruncateTime(k.SellDate) >= fromDate && EntityFunctions.TruncateTime(k.SellDate) <= currentDate);
            if (storeMTDSells != null)
            {
                var totalSells = (from k in storeMTDSells
                                  join p in SmartDostDbContext.ProductMasters on k.ProductID equals p.ProductID
                                  select new { Sells = k, ProductTypeCode = p.ProductTypeCode }).ToList();
                var mtdPurchase = totalSells.Where(k => k.ProductTypeCode == "AV").Sum(k => k.Sells.MTDSellInValue);
                var mtdSale = totalSells.Where(k => k.ProductTypeCode == "AV").Sum(k => k.Sells.MTDSellOutvalue);
                var mtdHAPurchase = totalSells.Where(k => k.ProductTypeCode == "HA").Sum(k => k.Sells.MTDSellInValue);
                var mtdHASale = totalSells.Where(k => k.ProductTypeCode == "HA").Sum(k => k.Sells.MTDSellOutvalue);
                var mtdACPurchase = totalSells.Where(k => k.ProductTypeCode == "AC").Sum(k => k.Sells.MTDSellInValue);
                var mtdACSale = totalSells.Where(k => k.ProductTypeCode == "AC").Sum(k => k.Sells.MTDSellOutvalue);
                //storeDetails.MTDPurchase = mtdPurchase.HasValue ? mtdPurchase.Value : 0;
                //storeDetails.MTDSale = mtdSale.HasValue ? mtdSale.Value : 0;
                //storeDetails.HAMTDPurchase = mtdHAPurchase.HasValue ? mtdPurchase.Value : 0;
                //storeDetails.HAMTDSale = mtdHASale.HasValue ? mtdSale.Value : 0;
                //storeDetails.ACMTDPurchase = mtdACPurchase.HasValue ? mtdPurchase.Value : 0;
                //storeDetails.ACMTDSale = mtdACSale.HasValue ? mtdSale.Value : 0;

                storeDetails.AVMTDPurchase = mtdPurchase;
                storeDetails.AVMTDSale = mtdSale;
                storeDetails.HAMTDPurchase = mtdHAPurchase;
                storeDetails.HAMTDSale = mtdHASale;
                storeDetails.ACMTDPurchase = mtdACPurchase;
                storeDetails.ACMTDSale = mtdACSale;
            }

            var storeTargets = SmartDostDbContext.StoreTargets.FirstOrDefault(k => k.StoreID == storeDetails.StoreID);
            if (storeTargets != null)
            {
                storeDetails.Target = storeTargets.Target.HasValue ? storeTargets.Target.Value : 0;
                storeDetails.ACH = storeTargets.ACH.HasValue ? storeTargets.ACH.Value : 0;
            }
            return storeDetails;
        }

        /// <summary>
        /// return 1 in the case Updated and 0 notUpdated
        /// </summary>
        /// <param name="partnerID">partner id.</param>
        /// <param name="contactPerson">The contact person.</param>
        /// <param name="mobileNo">The mobile no.</param>
        /// <param name="emailID">The email identifier.</param>
        /// <returns></returns>
        public int UpdatePartnerDetails(long partnerID, string contactPerson, string mobileNo, string emailID, string shipToCode)
        {
            int status = 0;

            IList<StoreMaster> stores = SmartDostDbContext.StoreMasters.Where(s => s.StoreCode == shipToCode && !s.IsDeleted).ToList();
            if (stores != null && stores.Count > 0)
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    foreach (var store in stores)
                    {
                        store.ContactPerson = contactPerson;
                        store.MobileNo = EncryptionEngine.EncryptString(mobileNo);
                        store.EmailID = EncryptionEngine.EncryptString(emailID);
                        store.ModifiedDate = System.DateTime.Now;
                        SmartDostDbContext.Entry<StoreMaster>(store).State = System.Data.EntityState.Modified;
                        PartnerMeeting meeting = new PartnerMeeting()
                        {
                            StoreID = store.StoreID,
                            UserID = partnerID,
                            Remarks = string.Empty,
                            ShipToCode = shipToCode,
                            CreatedDate = System.DateTime.Now,
                            MeetingDate = System.DateTime.Now,
                        };
                        SmartDostDbContext.PartnerMeetings.Add(meeting);
                    }
                    SmartDostDbContext.SaveChanges();
                    status = 1;
                    scope.Complete();
                }
            }
            return status;
        }

        /// <summary>
        /// Method to get store schemes on the basis of start date in ascending order
        /// </summary>
        /// <param name="storeID">store primary key</param>
        /// <returns>returns scheme entity collection</returns>
        public IList<Scheme> GetStoreSchemes(int storeID)
        {
            DateTime currentDate = System.DateTime.Today;
            return SmartDostDbContext.Schemes.Where(k => k.DealerID == storeID && k.SchemeStartDate <= currentDate && k.SchemeExpiryDate >= currentDate && !k.IsDeleted)
                .OrderBy(k => k.SchemeStartDate).ToList();
        }

        // Commented By Amit: Now this funciton is Coverted into SP "[SPDisplayStoreProfile]" see above funtion DisplayStoreProfileFromSP  :-AM1030
        ///// <summary>
        ///// Gets the stores for today beat.
        ///// </summary>
        ///// <param name="userID">The user identifier.</param>
        ///// <param name="roleID">The role identifier.</param>
        ///// <returns></returns>
        //public IList<StoreProfile> GetStoresForTodayBeat(long userID)
        //{
        //    IList<StoreProfile> userStoreList = new List<StoreProfile>();
        //    // Amit: survey question based on user profile (24 Sep 2014)
        //    if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.TodaysBeat)).FirstOrDefault() == true)
        //    {
        //        DateTime currentDate = System.DateTime.Today;
        //        DateTime fromDate = new DateTime(currentDate.Year, currentDate.Month, 1);
        //        DateTime LMcurrentDate = fromDate.AddDays(-1); // last day of previous month
        //        DateTime LMfromDate = new DateTime(currentDate.Year, currentDate.Month, 1).AddMonths(-1); // first day of previous month
        //        userStoreList = (from k in SmartDostDbContext.vwUserStoresTodayBeats.Where(k => k.UserID == userID && EntityFunctions.TruncateTime(k.CoverageDate) == currentDate)
        //                         //join S in SmartDostDbContext.StoreUsers on k.StoreID equals S.StoreID
        //                         //where k.IsActive == true
        //                         select new
        //          {
        //              StoreID = k.StoreID,
        //              StoreName = k.StoreName,
        //              ContactPerson = k.ContactPerson,
        //              MobileNo = k.MobileNo,
        //              EmailID = k.EmailID,
        //              State = k.STATE,
        //              City = k.City,
        //              AccountName = k.AccountName,
        //              ShipToBranch = k.ShipToBranch,
        //              ShipToCode = k.ShipToCode,
        //              ShipToRegion = k.ShipToRegion,
        //              //LastVisitDate = S.LastVisitDate,
        //              //VisitSummary = S.VisitSummary,
        //              CoverageDate = k.CoverageDate,
        //              IsCoverage = k.IsCoverage,
        //              CoverageID = k.CoverageID,
        //              AccId = k.AccId,
        //              StoreCode = k.StoreCode,
        //              PictureFileName = k.PictureFileName,
        //              GeoTagCount = k.GeoTagCount,
        //              //Added by vaishali for SDCE-576(Send Frozen Geo tag information into APK)
        //              Lattitude=k.Lattitude,
        //              Longitude=k.Longitude
        //          })
        //            .GroupBy(k =>
        //                new
        //                {
        //                    StoreID = k.StoreID,
        //                    StoreName = k.StoreName,
        //                    ContactPerson = k.ContactPerson,
        //                    MobileNo = k.MobileNo,
        //                    EmailID = k.EmailID,
        //                    State = k.State,
        //                    City = k.City,
        //                    AccountName = k.AccountName,
        //                    ShipToBranch = k.ShipToBranch,
        //                    ShipToCode = k.ShipToCode,
        //                    ShipToRegion = k.ShipToRegion,
        //                    //LastVisitDate = k.LastVisitDate,
        //                    //VisitSummary = k.VisitSummary,
        //                    CoverageDate = k.CoverageDate,
        //                    IsCoverage = k.IsCoverage,
        //                    CoverageID = k.CoverageID,
        //                    AccId = k.AccId,
        //                    StoreCode = k.StoreCode,
        //                    PictureFileName = k.PictureFileName,
        //                    GeoTagCount = k.GeoTagCount,
        //                    //Added by vaishali for SDCE-576(Send Frozen Geo tag information into APK)
        //                    Latitude=k.Lattitude,
        //                    Longitude=k.Longitude
        //                }).Select(k =>
        //                    new StoreProfile()
        //            {
        //                StoreID = k.Key.StoreID,
        //                StoreName = k.Key.StoreName,
        //                ContactPerson = k.Key.ContactPerson,
        //                MobileNo = k.Key.MobileNo,
        //                EmailID = k.Key.EmailID,
        //                State = k.Key.State,
        //                City = k.Key.City,
        //                AccountName = k.Key.AccountName,
        //                ShipToBranch = k.Key.ShipToBranch,
        //                ShipToCode = k.Key.ShipToCode,
        //                ShipToRegion = k.Key.ShipToRegion,
        //                //LastVisitDate = k.Key.LastVisitDate,
        //                //VisitSummary = k.Key.VisitSummary,
        //                CoverageDate = k.Key.CoverageDate,
        //                IsCoverage = k.Key.IsCoverage,
        //                CoverageID = k.Key.CoverageID,
        //                AccId = k.Key.AccId,
        //                StoreCode = k.Key.StoreCode,
        //                PictureFileName = k.Key.PictureFileName,
        //                GeoTagCount = k.Key.GeoTagCount,
        //                //Added by vaishali for SDCE-576(Send Frozen Geo tag information into APK)
        //                IsFreeze=k.Key.Latitude==null?false:true,
        //                FreezeLattitude = k.Key.Latitude,
        //                FreezeLongitude = k.Key.Longitude
        //            })
        //            .ToList();
        //        if (userStoreList != null && userStoreList.Count == 0)
        //        {

        //            userStoreList = (from k in SmartDostDbContext.vwUserStoresTodayBeats.Where(k => k.UserID == userID && EntityFunctions.TruncateTime(k.CoverageDate) == currentDate)
        //                             //join S in SmartDostDbContext.StoreUsers on k.StoreID equals S.StoreID
        //                             where k.IsActive == true
        //                             select new
        //                             {
        //                                 StoreID = k.StoreID,
        //                                 StoreName = k.StoreName,
        //                                 ContactPerson = k.ContactPerson,
        //                                 MobileNo = k.MobileNo,
        //                                 EmailID = k.EmailID,
        //                                 State = k.STATE,
        //                                 City = k.City,
        //                                 AccountName = k.AccountName,
        //                                 ShipToBranch = k.ShipToBranch,
        //                                 ShipToCode = k.ShipToCode,
        //                                 ShipToRegion = k.ShipToRegion,
        //                                 //LastVisitDate = S.LastVisitDate,
        //                                 //VisitSummary = S.VisitSummary,
        //                                 CoverageDate = k.CoverageDate,
        //                                 IsCoverage = k.IsCoverage,
        //                                 CoverageID = k.CoverageID,
        //                                 AccId = k.AccId,
        //                                 StoreCode = k.StoreCode,
        //                                 PictureFileName = k.PictureFileName,
        //                                 GeoTagCount = k.GeoTagCount,
        //                                 //Added by vaishali for SDCE-576(Send Frozen Geo tag information into APK) :: VC20141010
        //                                 Latitude = k.Lattitude,
        //                                 Longitude = k.Longitude
        //                                 //VC20141010
        //                             })
        //            .GroupBy(k =>
        //                new
        //                {
        //                    StoreID = k.StoreID,
        //                    StoreName = k.StoreName,
        //                    ContactPerson = k.ContactPerson,
        //                    MobileNo = k.MobileNo,
        //                    EmailID = k.EmailID,
        //                    State = k.State,
        //                    City = k.City,
        //                    AccountName = k.AccountName,
        //                    ShipToBranch = k.ShipToBranch,
        //                    ShipToCode = k.ShipToCode,
        //                    ShipToRegion = k.ShipToRegion,
        //                    //LastVisitDate = k.LastVisitDate,
        //                    //VisitSummary = k.VisitSummary,
        //                    CoverageDate = k.CoverageDate,
        //                    IsCoverage = k.IsCoverage,
        //                    CoverageID = k.CoverageID,
        //                    AccId = k.AccId,
        //                    StoreCode = k.StoreCode,
        //                    PictureFileName = k.PictureFileName,
        //                    GeoTagCount = k.GeoTagCount,
        //                    //Added by vaishali for SDCE-576(Send Frozen Geo tag information into APK) :: VC20141010
        //                    Latitude = k.Latitude,
        //                    Longitude = k.Longitude
        //                    //VC20141010
        //                }).Select(k =>
        //                   new StoreProfile()
        //                   {
        //                       StoreID = k.Key.StoreID,
        //                       StoreName = k.Key.StoreName,
        //                       ContactPerson = k.Key.ContactPerson,
        //                       MobileNo = k.Key.MobileNo,
        //                       EmailID = k.Key.EmailID,
        //                       State = k.Key.State,
        //                       City = k.Key.City,
        //                       AccountName = k.Key.AccountName,
        //                       ShipToBranch = k.Key.ShipToBranch,
        //                       ShipToCode = k.Key.ShipToCode,
        //                       ShipToRegion = k.Key.ShipToRegion,
        //                       //LastVisitDate = k.Key.LastVisitDate,
        //                       //VisitSummary = k.Key.VisitSummary,
        //                       CoverageDate = k.Key.CoverageDate,
        //                       IsCoverage = k.Key.IsCoverage,
        //                       CoverageID = k.Key.CoverageID,
        //                       AccId = k.Key.AccId,
        //                       StoreCode = k.Key.StoreCode,
        //                       PictureFileName = k.Key.PictureFileName,
        //                       GeoTagCount = k.Key.GeoTagCount,
        //                       //Added by vaishali for SDCE-576(Send Frozen Geo tag information into APK) ::VC20141010
        //                       IsFreeze = k.Key.Latitude == null ? false : true,
        //                       FreezeLattitude = k.Key.Latitude,
        //                       FreezeLongitude = k.Key.Longitude
        //                       //VC20141010
        //                   })
        //            .ToList();
        //        }
        //        var storeUsers = SmartDostDbContext.StoreUsers.Where(k => k.UserID == userID && !k.IsDeleted);
        //        foreach (var item in userStoreList)
        //        {
        //            if (storeUsers.Count() > 0)
        //            {
        //                var store = storeUsers.FirstOrDefault(k => k.StoreID == item.StoreID && k.LastVisitDate != null);
        //                if (store != null)
        //                {
        //                    item.LastVisitDate = store.LastVisitDate;
        //                    item.VisitSummary = store.VisitSummary;
        //                }
        //                else
        //                {
        //                    item.LastVisitDate = null;
        //                    item.VisitSummary = null;
        //                }
        //            }
        //            item.IsCoverage = SmartDostDbContext.SurveyResponses.FirstOrDefault(k => k.StoreID == item.StoreID && EntityFunctions.TruncateTime(k.CreatedDate) == currentDate && k.UserID == userID) != null;
        //            //DateTime date = System.DateTime.Today.AddDays(-1);
        //            UserRole userRole = SmartDostDbContext.UserRoles.FirstOrDefault(k => !k.IsDeleted && k.UserID == userID);
        //            if (userRole != null)
        //            {
        //                item.UserRoleID = userRole.UserRoleID;
        //            }
        //            var storeMTDSells = SmartDostDbContext.StoreSells.Where(k => k.StoreID == item.StoreID && EntityFunctions.TruncateTime(k.SellDate) >= fromDate && EntityFunctions.TruncateTime(k.SellDate) <= currentDate);
        //            if (storeMTDSells != null)
        //            {
        //                var totalSells = (from k in storeMTDSells
        //                                  join p in SmartDostDbContext.ProductMasters on k.ProductID equals p.ProductID
        //                                  select new { Sells = k, ProductTypeCode = p.ProductTypeCode }).ToList();
        //                var mtdPurchase = totalSells.Where(k => k.ProductTypeCode == "AV").Sum(k => k.Sells.MTDSellInValue);
        //                var mtdSale = totalSells.Where(k => k.ProductTypeCode == "AV").Sum(k => k.Sells.MTDSellOutvalue);
        //                var mtdHAPurchase = totalSells.Where(k => k.ProductTypeCode == "HA").Sum(k => k.Sells.MTDSellInValue);
        //                var mtdHASale = totalSells.Where(k => k.ProductTypeCode == "HA").Sum(k => k.Sells.MTDSellOutvalue);
        //                var mtdACPurchase = totalSells.Where(k => k.ProductTypeCode == "AC").Sum(k => k.Sells.MTDSellInValue);
        //                var mtdACSale = totalSells.Where(k => k.ProductTypeCode == "AC").Sum(k => k.Sells.MTDSellOutvalue);
        //                //item.MTDPurchase = mtdPurchase.HasValue ? mtdPurchase.Value : 0;
        //                //item.MTDSale = mtdSale.HasValue ? mtdSale.Value : 0;
        //                //item.HAMTDPurchase = mtdHAPurchase.HasValue ? mtdPurchase.Value : 0;
        //                //item.HAMTDSale = mtdHASale.HasValue ? mtdSale.Value : 0;
        //                //item.ACMTDPurchase = mtdACPurchase.HasValue ? mtdPurchase.Value : 0;
        //                //item.ACMTDSale = mtdACSale.HasValue ? mtdSale.Value : 0;

        //                //Commented above line to make it not nullable on 26-Feb-2014 as Live db has not nullable
        //                item.AVMTDPurchase = mtdPurchase;
        //                item.AVMTDSale = mtdSale;
        //                item.HAMTDPurchase = mtdHAPurchase;
        //                item.HAMTDSale = mtdHASale;
        //                item.ACMTDPurchase = mtdACPurchase;
        //                item.ACMTDSale = mtdACSale;
        //            }
        //            var storeLMTDSells = SmartDostDbContext.StoreSells.Where(k => k.StoreID == item.StoreID && EntityFunctions.TruncateTime(k.SellDate) >= LMfromDate && EntityFunctions.TruncateTime(k.SellDate) <= LMcurrentDate);
        //            if (storeLMTDSells != null)
        //            {
        //                var totalSells = (from k in storeLMTDSells
        //                                  join p in SmartDostDbContext.ProductMasters on k.ProductID equals p.ProductID
        //                                  select new { Sells = k, ProductTypeCode = p.ProductTypeCode }).ToList();
        //                var mtdPurchase = totalSells.Where(k => k.ProductTypeCode == "AV").Sum(k => k.Sells.MTDSellInValue);
        //                var mtdSale = totalSells.Where(k => k.ProductTypeCode == "AV").Sum(k => k.Sells.MTDSellOutvalue);
        //                var mtdHAPurchase = totalSells.Where(k => k.ProductTypeCode == "HA").Sum(k => k.Sells.MTDSellInValue);
        //                var mtdHASale = totalSells.Where(k => k.ProductTypeCode == "HA").Sum(k => k.Sells.MTDSellOutvalue);
        //                var mtdACPurchase = totalSells.Where(k => k.ProductTypeCode == "AC").Sum(k => k.Sells.MTDSellInValue);
        //                var mtdACSale = totalSells.Where(k => k.ProductTypeCode == "AC").Sum(k => k.Sells.MTDSellOutvalue);
        //                item.AVLMTDPurchase = mtdPurchase;
        //                item.AVLMTDSale = mtdSale;
        //                item.HALMTDPurchase = mtdHAPurchase;
        //                item.HALMTDSale = mtdHASale;
        //                item.ACLMTDPurchase = mtdACPurchase;
        //                item.ACLMTDSale = mtdACSale;
        //            }
        //            var storeTargets = SmartDostDbContext.StoreTargets.FirstOrDefault(k => k.StoreID == item.StoreID);
        //            if (storeTargets != null)
        //            {
        //                item.Target = storeTargets.Target.HasValue ? storeTargets.Target.Value : 0;
        //                item.ACH = storeTargets.ACH.HasValue ? storeTargets.ACH.Value : 0;
        //            }
        //            //SDCE-684 New add Modifiy by Niranjan (Channel Type Status) 21-10-2014
        //            var storeChanneltype = SmartDostDbContext.StoreMasterAttributes.FirstOrDefault(k => k.StoreCode == item.StoreCode);
        //            if (storeChanneltype != null)
        //            {
        //                item.ChannelType = storeChanneltype.Attributes1 + ' ' + storeChanneltype.Attributes2;
        //                var channeltypedisplaystatus = SmartDostDbContext.ChannelTypeDisplays.FirstOrDefault(k => k.ChannelType == item.ChannelType);
        //                {
        //                    item.IsDisplayCounterShare = channeltypedisplaystatus.IsDisplayCounterShare;
        //                    item.IsPlanogram = channeltypedisplaystatus.IsPlanogram;
        //                }
        //            }
        //            //SDCE-684 
        //        }
        //    }
        //    return userStoreList;
        //}


        /// <summary>
        /// Gets the user stores.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public IList<StoreMaster> GetUserStores(long userID)
        {
            IList<StoreMaster> StoresList = new List<StoreMaster>();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
new TransactionOptions
{
    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
}))
            {

                StoresList = (from su in SmartDostDbContext.StoreUsers
                              join sm in SmartDostDbContext.vwStoreMasters on su.StoreID equals sm.StoreID
                              where su.IsDeleted == false && su.UserID == userID
                              select sm
                              ).ToList().Select(x => new StoreMaster()
                              {
                                  StoreName = x.StoreName,
                                  StoreCode = x.StoreCode,
                                  IsActive = x.IsActive,
                                  StoreID = x.StoreID,
                                  ChannelType = x.ChannelType,
                                  City = x.City
                              }).OrderBy(x => x.StoreName).ToList();



                scope.Complete();
            }
            return StoresList;
            /*Commented by Dhiraj
            var list = (from su in SmartDostDbContext.StoreUsers
                        join sm in SmartDostDbContext.StoreMasters on su.StoreID equals sm.StoreID
                        where su.UserID == userID && sm.IsActive == true && sm.IsDeleted == false && su.IsDeleted == false
                        select new { sm.StoreCode, sm.StoreName, su.UserID }).GroupBy(k => new { k.StoreName, k.StoreCode, k.UserID })
                        .Select(k => new { StoreCode = k.Key.StoreCode, StoreName = k.Key.StoreName }).Distinct().ToList();

            foreach (var item in list)
            {
                var detail = new StoreMaster() { StoreCode = item.StoreCode, StoreName = item.StoreName, IsActive = true };
                var store = SmartDostDbContext.StoreMasters.FirstOrDefault(k => k.StoreCode == item.StoreCode && !k.IsDeleted);
                if (store != null)
                {
                    detail.StoreID = store.StoreID;
                    //detail.ChannelType = store.ChannelType;
                    string channelType = store.ChannelType;
                    //var storeMasterAttribute = SmartDostDbContext.StoreMasterAttributes.Where(x => x.StoreCode == store.StoreCode).FirstOrDefault();
                    //if (storeMasterAttribute != null && storeMasterAttribute.SPP)
                    //{
                    //    channelType = storeMasterAttribute.Attributes1 + " " + storeMasterAttribute.Attributes2;
                    //}
                    // Change by Niranjan SDCE-579
                    var storeMasterChannel = (from SMA in SmartDostDbContext.StoreMasterAttributes
                                              join CTS in SmartDostDbContext.ChannelTypeSDs on SMA.ChannelTypeID equals CTS.ChannelTypeID
                                              where SMA.StoreCode == store.StoreCode && CTS.IsDeleted == false
                                              select new { SMA.StoreCode, CTS.ChannelType, CTS.ChannelType2 }).FirstOrDefault();
                    if (storeMasterChannel != null)
                    {
                        channelType = storeMasterChannel.ChannelType + " " + storeMasterChannel.ChannelType2;
                    }
                    detail.ChannelType = channelType.Trim();
                    detail.City = store.City;
                }
                if (StoresList.FirstOrDefault(k => k.StoreCode == item.StoreCode) == null)
                {
                    StoresList.Add(detail);
                }
            }
            StoresList = StoresList.OrderBy(x => x.StoreName).ToList();
            scope.Complete();
        }
             */
            //return StoresList;
            //IList<StoreMaster> StoresList = new List<StoreMaster>();
            //var list = (from su in SmartDostDbContext.StoreUsers
            //            join sm in SmartDostDbContext.StoreMasters on su.StoreID equals sm.StoreID
            //            where su.UserID == userID && sm.IsActive == true && sm.IsDeleted == false
            //            select new { sm.StoreID, sm.StoreCode, sm.StoreName, su.UserID, sm.CoveragePlans }).GroupBy(k => new { k.StoreName, k.StoreID, k.StoreCode, k.UserID })
            //            .Select(k => new { StoreCode = k.Key.StoreCode, StoreID = k.Key.StoreID, StoreName = k.Key.StoreName }).ToList();

            //foreach (var item in list)
            //{

            //    StoresList.Add(new StoreMaster { StoreID = item.StoreID, StoreCode = item.StoreCode, StoreName = item.StoreName, IsActive = true });
            //}
            //return StoresList;
        }

        /// <summary>
        /// Method to get store beat plan summary
        /// </summary>
        /// <param name="storeID">store ID</param>
        /// <param name="isCurrentMonth">is current month</param>
        /// <returns>returns beat plan summary</returns>
        public string GetStoreBeatPlanSummary(int storeID, bool isCurrentMonth)
        {
            string beatSummary = string.Empty;
            DateTime currentDate = System.DateTime.Now;
            if (!isCurrentMonth)
            {
                currentDate.AddMonths(1);
            }
            var coveragePlans = SmartDostDbContext.CoveragePlans.Where(k => k.StatusID == 1 && k.CoverageDate.Month == currentDate.Month && k.StoreID == storeID).OrderBy(k => k.CoverageDate).ToList();
            if (coveragePlans != null && coveragePlans.Count > 0)
            {
                List<int> days = coveragePlans.Select(k => k.CoverageDate.Day).ToList();
                foreach (int day in days)
                {
                    beatSummary += day.ToString() + ",";
                }
                beatSummary = beatSummary.TrimEnd(',');
            }
            return beatSummary;
        }

        /// <summary>
        /// Method to get ProductDefinitions
        /// </summary>
        /// <returns>returns ProductDefinitions</returns>
        public IList<ProductDefinition> GetProductDefinitions(int companyID)
        {
            IList<ProductDefinition> lstProductDefinations = new List<ProductDefinition>();
            lstProductDefinations = SmartDostDbContext.ProductDefinitions.Where(k => k.CompanyID == companyID).ToList();
            return lstProductDefinations;
        }
               

        public IList<Competitor> GetCompetitors(int companyID, long? userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;
                        
            IList<Competitor> result = new List<Competitor>();
            if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.Competitor)).FirstOrDefault() == true)
            {
                //lstcompetitors = SmartDostDbContext.Competitors.Where(k => k.CompanyID == companyID && k.IsDeleted == false && k.IsActive == true).ToList();


                if (LastUpdatedDate == null)
                {
                    result = SmartDostDbContext.Competitors.Where(k => k.IsDeleted == false && k.CompanyID == companyID).ToList().ToList()
                       .OrderBy(k => (k.ModifiedDate ?? k.CreatedDate))
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       
                }
                else
                {

                    result = SmartDostDbContext.Competitors.Where(k=> k.CompanyID == companyID).ToList()
                    .Where(k =>
                        (((k.ModifiedDate ?? k.CreatedDate) > LastUpdatedDate)
                        ||
                        ((k.ModifiedDate ?? k.CreatedDate) == LastUpdatedDate.Value)
                        ))
                    .OrderBy(k => (k.ModifiedDate ?? k.CreatedDate))
                    .Skip(StartRowIndex)
                    .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                   
                }

                HasMoreRows = result.Count > RowCount ? true : false;
                result = result.Take(RowCount).ToList();

                // Update last modified data among the data if available, else send the same modifieddate back  
                if (result.Count > 0)
                {
                    //MaxModifiedDate = dealer.Max(x => x.MaxModifiedDate);

                    if (LastUpdatedDate == null && HasMoreRows == true)
                        MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
                    else if (LastUpdatedDate == null && HasMoreRows == false)
                        MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
                    else
                        MaxModifiedDate = result.Max(k => (k.ModifiedDate ?? k.CreatedDate));
                }

            }
            return result;
        }

        /// <summary>
        /// Method to get selected outlet profile details
        /// </summary>
        /// <param name="userID">user primary ID</param>
        /// <param name="storeID">store ID</param>
        /// <returns>returns outlet entity instance</returns>
        public OutletProfile GetOutletProfile(long userID, long storeID)
        {
            return (from su in SmartDostDbContext.StoreUsers
                    join sm in SmartDostDbContext.StoreMasters on su.StoreID equals sm.StoreID
                    where su.StoreID == storeID && su.UserID == userID
                    select new OutletProfile() { OutletCode = sm.StoreCode, OutletName = sm.StoreName, UserRoleID = su.UserRoleID, RoleID = su.UserRole.RoleID }).ToList().FirstOrDefault();

        }

        /// <summary>
        /// Gets the dealers list.
        /// </summary>
        /// <returns></returns>
        public IList<StoreMaster> GetDealersList()
        {
            IList<StoreMaster> lstDealers = new List<StoreMaster>();
            string dealerType = AspectEnums.ChannelTypes.DEALER.ToString();
            lstDealers = SmartDostDbContext.StoreMasters.Where(k => k.ChannelType.ToUpper() == dealerType && k.IsActive == true && k.IsDeleted == false).OrderBy(o => o.StoreName).ToList();
            return lstDealers;
        }

        /// <summary>
        /// Gets the schemes by dealer identifier.
        /// </summary>
        /// <param name="dealerID">The dealer identifier.</param>
        /// <returns></returns>
        public List<Scheme> GetSchemesByDealerID(int dealerID)
        {
            List<Scheme> lstSchemes = new List<Scheme>();

            lstSchemes = SmartDostDbContext.Schemes.Where(k => k.DealerID == dealerID && k.IsActive == true && k.IsDeleted == false).OrderBy(o => o.Title).ToList();
            return lstSchemes;
        }

        /// <summary>
        /// Gets the scheme by scheme identifier.
        /// </summary>
        /// <param name="schemeID">The scheme identifier.</param>
        /// <returns></returns>
        public Scheme GetSchemeBySchemeID(int schemeID)
        {
            Scheme scheme = SmartDostDbContext.Schemes.SingleOrDefault(k => k.SchemeID == schemeID && k.IsActive == true && k.IsDeleted == false);
            return scheme;
        }

        /// <summary>
        /// Inserts the scheme.
        /// </summary>
        /// <param name="scheme">The scheme.</param>
        /// <returns></returns>
        public bool InsertScheme(Scheme scheme)
        {
            bool isSuccess = false;
            if (scheme != null)
            {
                SmartDostDbContext.Schemes.Add(scheme);
                SmartDostDbContext.Entry<Scheme>(scheme).State = System.Data.EntityState.Added;
                isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Updates the scheme.
        /// </summary>
        /// <param name="scheme">The scheme.</param>
        /// <returns></returns>
        public bool UpdateScheme(Scheme scheme)
        {
            bool isSuccess = false;
            Scheme dealerScheme = new Scheme();
            if (scheme != null)
                dealerScheme = SmartDostDbContext.Schemes.SingleOrDefault(k => k.SchemeID == scheme.SchemeID);

            if (dealerScheme.SchemeID > 0)
            {
                dealerScheme.Title = scheme.Title;
                dealerScheme.Description = scheme.Description;
                dealerScheme.SchemeStartDate = scheme.SchemeStartDate;
                dealerScheme.SchemeExpiryDate = scheme.SchemeExpiryDate;
                dealerScheme.ModifiedDate = DateTime.Now;
                dealerScheme.ModifiedBy = scheme.ModifiedBy;
                SmartDostDbContext.Entry<Scheme>(dealerScheme).State = System.Data.EntityState.Modified;
            }
            isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;
            return isSuccess;
        }

        /// <summary>
        /// Deletes the scheme.
        /// </summary>
        /// <param name="schemes">The schemes.</param>
        /// <returns></returns>
        public bool DeleteScheme(List<Scheme> schemes)
        {
            bool isSuccess = false;
            if (schemes.Count > 0)
            {
                foreach (Scheme item in schemes)
                {
                    var scheme = SmartDostDbContext.Schemes.FirstOrDefault(k => k.SchemeID == item.SchemeID);

                    if (scheme != null)
                    {
                        SmartDostDbContext.Schemes.Attach(scheme);
                        SmartDostDbContext.Entry<Scheme>(scheme).State = System.Data.EntityState.Deleted;
                    }
                }
                isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;
            }

            return isSuccess;
        }

        /// <summary>
        /// Method to get store details
        /// </summary>
        /// <param name="storeID">store ID</param>
        /// <returns></returns>
        public StoreMaster GetStoreDetails(int storeID)
        {
            return SmartDostDbContext.StoreMasters.FirstOrDefault(k => k.StoreID == storeID);
        }

        /// <summary>
        /// Method to identify that is geo tag require or not
        /// </summary>
        /// <param name="storeID">store ID</param>
        /// <returns>returns boolean status</returns>
        public bool IsGeoTagRequired(int storeID)
        {
            var store = SmartDostDbContext.StoreMasters.FirstOrDefault(k => k.StoreID == storeID && k.GeoTagCount > 2);
            return store == null;
        }

        /// <summary>
        /// Method to get store parent details for provided user and store ids
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <param name="storeID">store ID</param>
        /// <returns>returns store parent mapping instance</returns>
        public StoreParentMapping GetUserStoreParentDetails(long userID, int storeID)
        {
            UserMaster userDetails = SmartDostDbContext.UserMasters.FirstOrDefault(k => k.UserID == userID);
            if (userDetails != null)
            {
                return SmartDostDbContext.StoreParentMappings.FirstOrDefault(k => k.StoreID == storeID && k.ParentCode == userDetails.DistyCode && (k.IsDeleted == null || k.IsDeleted == false));
            }
            return null;
        }

        #region Planogram (Amit: 18 Sep 2014)
        ///// <summary>
        /////  This function returns list of Planogram Products in the table
        ///// </summary>
        ///// <returns>returns list of Planogram Products</returns>
        //public List<PlanogramProductMaster> GetPlanogramProductMasters(int companyID, long userID, long roleID, int PlanogramProductMasterID, int rowcounter, out int totalRow)
        //{
        //    List<PlanogramProductMaster> result = new List<PlanogramProductMaster>();
        //    int totalRowFinal = 0;
        //    if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.Planogram)).FirstOrDefault() == true)
        //    {

        //        DateTime CurrentDate = System.DateTime.Today.AddDays(1).Date;
        //        if (PlanogramProductMasterID == -1)
        //        {
        //            result = SmartDostDbContext.PlanogramProductMasters.Where(k => k.PlanogramProductMasterID > PlanogramProductMasterID && k.EffectiveFrom < CurrentDate && !k.IsDeleted).Take(rowcounter).OrderBy(x => x.PlanogramProductMasterID).ToList();
        //            totalRowFinal = SmartDostDbContext.PlanogramProductMasters.Where(k => k.PlanogramProductMasterID > PlanogramProductMasterID && k.EffectiveFrom < CurrentDate && !k.IsDeleted).Count();
        //        }
        //        else
        //        {
        //            result = SmartDostDbContext.PlanogramProductMasters.Where(k => k.PlanogramProductMasterID > PlanogramProductMasterID && k.EffectiveFrom < CurrentDate && !k.IsDeleted).OrderBy(x => x.PlanogramProductMasterID).Take(rowcounter).ToList();
        //            totalRowFinal = SmartDostDbContext.PlanogramProductMasters.Where(k => k.PlanogramProductMasterID > PlanogramProductMasterID && k.EffectiveFrom < CurrentDate && !k.IsDeleted).Count();
        //        }

        //    }
        //    totalRow = totalRowFinal;
        //    return result;
        //}

        /// <summary>
        ///  This function returns list of Planogram Products in the table
        /// </summary>
        /// <returns>returns list of Planogram Products</returns>
        public List<PlanogramProductMaster> GetPlanogramProductMasters(int companyID, long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
          
            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;

            List<PlanogramProductMaster> result = new List<PlanogramProductMaster>();
            if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.Planogram)).FirstOrDefault() == true)
            {

                if (LastUpdatedDate == null)
                {
                    result = SmartDostDbContext.PlanogramProductMasters.Where(k => k.IsDeleted == false).ToList().ToList()
                       .OrderBy(k => k.CreatedDate)
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       
                }
                else
                {

                    result = SmartDostDbContext.PlanogramProductMasters.ToList()
                    .Where(k =>
                        ((k.CreatedDate > LastUpdatedDate)
                        ||
                        (k.CreatedDate == LastUpdatedDate.Value)
                        ))
                    .OrderBy(k => k.CreatedDate)
                    .Skip(StartRowIndex)
                    .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                   
                }

                HasMoreRows = result.Count > RowCount ? true : false;
                result = result.Take(RowCount).ToList();

                // Update last modified data among the data if available, else send the same modifieddate back  
                if (result.Count > 0)
                {
                    //MaxModifiedDate = dealer.Max(x => x.MaxModifiedDate);

                    if (LastUpdatedDate == null && HasMoreRows == true)
                        MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
                    //else if (LastUpdatedDate == null && HasMoreRows == false)
                    else if (HasMoreRows == false)
                        MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
                    else
                        MaxModifiedDate = result.Max(x => x.CreatedDate);
                }
            }
            return result;
        }

        /// <summary>
        ///  This function returns list of Planogram Classes in the table
        /// </summary>
        /// <returns>returns list of Planogram Classes</returns>
        public List<PlanogramClassMaster> GetPlanogramClassMasters(int companyID, long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {


            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;

            List<PlanogramClassMaster> result = new List<PlanogramClassMaster>();
            if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.Planogram)).FirstOrDefault() == true)
            {

                if (LastUpdatedDate == null)
                {
                    result = SmartDostDbContext.PlanogramClassMasters.Where(k => k.IsDeleted == false).ToList().ToList()
                       .OrderBy(k => k.CreatedDate)
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       
                }
                else
                {
                  
                    result = SmartDostDbContext.PlanogramClassMasters.ToList()
                    .Where(k =>
                        ((k.CreatedDate > LastUpdatedDate)
                        ||
                        (k.CreatedDate == LastUpdatedDate.Value)
                        ))
                    .OrderBy(k => k.CreatedDate)
                    .Skip(StartRowIndex)
                    .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                   
                }

                HasMoreRows = result.Count > RowCount ? true : false;
                result = result.Take(RowCount).ToList();

                // Update last modified data among the data if available, else send the same modifieddate back  
                if (result.Count > 0)
                {
                    //MaxModifiedDate = dealer.Max(x => x.MaxModifiedDate);

                    if (LastUpdatedDate == null && HasMoreRows == true)
                        MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
                    else if (LastUpdatedDate == null && HasMoreRows == false)
                        MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
                    else
                        MaxModifiedDate = result.Max(x => x.CreatedDate);
                }


            }
            return result;


            //List<PlanogramClassMaster> result = new List<PlanogramClassMaster>();
            //if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.Planogram)).FirstOrDefault() == true)
            //{
            //    result = SmartDostDbContext.PlanogramClassMasters.Where(k => k.IsDeleted == false).ToList();
            //}
            //return result;
        }

        //Added by Vaishali on 18 Sep 2014 to Submit Planogram Response from APK
        /// <summary>
        /// Submit Planogram Response from APK
        /// </summary>
        /// <param name="PlanogramResponse"></param>
        /// <returns></returns>
        public bool SubmitPlanogramResponse(List<PlanogramResponse> planogramResponse, int companyID, long userID, long roleID)
        {
            DateTime dtCurrentDateTime = DateTime.Now;
            int IsInserted = 0;
            List<PlanogramResponse> planogramResponsechild = planogramResponse;
            foreach (var item in planogramResponse)
            {
                if (item.Class != null)
                {
                    item.CreatedDate = dtCurrentDateTime;
                    SmartDostDbContext.Entry<PlanogramResponse>(item).State = System.Data.EntityState.Added;
                }
            }
            IsInserted = SmartDostDbContext.SaveChanges();


            return true;

        }

        #endregion

        #region Private Functions

        /// <summary>
        /// return Cities based on User
        /// </summary>
        /// <param name="userID">User Id</param>   
        /// <returns>return Cities based on User</returns>

        private IList<string> GetCityList(long userId)
        {
            IList<string> cityList = new List<string>();
            string dealerType = AspectEnums.ChannelTypes.DEALER.ToString();
            cityList = (from S in SmartDostDbContext.StoreUsers
                        join St in SmartDostDbContext.StoreMasters on S.StoreID equals St.StoreID
                        where S.UserID == userId && S.IsDeleted == false && St.IsActive == true && St.ChannelType.ToUpper() == dealerType
                        select St.City).ToList();
            return cityList;
        }

        #endregion

        #region GeoTagUnfreeze
        public List<string> ValidateStores(List<string> Stores)
        {

            var result = (from s in Stores
                          where !SmartDostDbContext.StoreMasters.Any(sm => (sm.IsDeleted == false) && (sm.IsActive == true))
                          select s).ToList();

            return result;


        }
        #endregion
        #region CompetitorProductGroupMapping

        ///// <summary>
        /////  This function returns mapping between compititor and compprofuctgroup
        ///// </summary>
        ///// <returns>returns list of mapped data</returns>
        //public List<CompetitorProductGroupMapping> GetCompetitorProductGroupMapping(long userID)
        //{
        //    List<CompetitorProductGroupMapping> result = new List<CompetitorProductGroupMapping>();
        //    if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.DisplayShare)).FirstOrDefault() == true
        //        || SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.Planogram)).FirstOrDefault() == true)
        //    {
        //        result = SmartDostDbContext.CompetitorProductGroupMappings.Where(k => k.IsDeleted == false).ToList();
        //    }
        //    return result;
        //    //return SmartDostDbContext.CompetitorProductGroupMappings.Where(k => k.IsDeleted == false).ToList();
        //}

        /// <summary>
        ///  This function returns mapping between compititor and compprofuctgroup
        /// </summary>
        /// <returns>returns list of mapped data</returns>
        public List<CompetitorProductGroupMapping> GetCompetitorProductGroupMapping(int companyID, long userID, long roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;

            List<CompetitorProductGroupMapping> result = new List<CompetitorProductGroupMapping>();
            if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.DisplayShare)).FirstOrDefault() == true
                || SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.Planogram)).FirstOrDefault() == true)
            {

                if (LastUpdatedDate == null)
                {
                    result = SmartDostDbContext.CompetitorProductGroupMappings.Where(k => k.IsDeleted == false).ToList().ToList()
                       .OrderBy(k => k.CreatedDate)
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       
                }
                else
                {

                    result = SmartDostDbContext.CompetitorProductGroupMappings.ToList()
                    .Where(k =>
                        ((k.CreatedDate > LastUpdatedDate)
                        ||
                        (k.CreatedDate == LastUpdatedDate.Value)
                        ))
                    .OrderBy(k => k.CreatedDate)
                    .Skip(StartRowIndex)
                    .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                   
                }

                HasMoreRows = result.Count > RowCount ? true : false;
                result = result.Take(RowCount).ToList();

                // Update last modified data among the data if available, else send the same modifieddate back  
                if (result.Count > 0)
                {
                    //MaxModifiedDate = dealer.Max(x => x.MaxModifiedDate);

                    if (LastUpdatedDate == null && HasMoreRows == true)
                        MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
                    //else if (LastUpdatedDate == null && HasMoreRows == false)
                    else if (HasMoreRows == false)
                        MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
                    else
                        MaxModifiedDate = result.Max(x => x.CreatedDate);
                }
            }
            return result;         
          
            //return SmartDostDbContext.CompetitorProductGroupMappings.Where(k => k.IsDeleted == false).ToList();
        }
        #endregion

        #region Submit StoreGeoTag through multipart
        /// <summary>
        /// Submit Store geo tag using multipart
        /// </summary>
        /// <param name="geoTagBO"></param>
        /// <returns></returns>
        public int SubmitStoreGeoTag(StoreGeoTag storeGeoTag)
        {
            int status = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
new TransactionOptions
{
    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
}))
            {
                storeGeoTag.GeoTagDate = DateTime.Now;
                storeGeoTag.CreatedBy = storeGeoTag.UserID;
                storeGeoTag.CreatedDate = DateTime.Now;
                storeGeoTag.ISEligibleForFreeze = true;                
                SmartDostDbContext.StoreGeoTags.Add(storeGeoTag);
                SmartDostDbContext.SaveChanges();
                status = 1;
                //if (storeGeoTag.UserOption == null)
                //{
                //    if (SmartDostDbContext.FreezeGeoTags.FirstOrDefault(x => x.StoreID == storeGeoTag.StoreID) == null) /*For Defect SDCE-4236*/
                //    {
                //        SmartDostDbContext.SPFreezeGeoTag(storeGeoTag.StoreID);
                //    }
                //}
                scope.Complete();
            }

            return status;
        }
        #endregion

        /// <summary>
        /// Gets the dealer Creation.SDHHP-6114
        /// Added by Gourav Vishnoi on 31 July 2015
        /// </summary>
        #region Dealer Creation Form in SD and AX Service SDHHP-6114

        public List<spGetDistrictDealerCreation_Result> GetMDMDistrict(int userID)
        {
            //List<string> lstMDMDistrict = new List<string>();
            //lstMDMDistrict = (from dis in SmartDostDbContext.MDM2SDMSalesGeographyMaster
            //                  select dis.Level4Code).Distinct().ToList();
          
            //return lstMDMDistrict;
            
            return SmartDostDbContext.spGetDistrictDealerCreation(userID).ToList();
        }
        /// <summary>
        /// GetMDMCity
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        public List<spGetCityDealerCreation_Result> GetMDMCity(string district)
        {
            List<string> lstMDMCity = new List<string>();
            return SmartDostDbContext.spGetCityDealerCreation(district).ToList();
            
           // return lstMDMCity; 


           
        }
        /// <summary>
        /// GetMDMPinCode
        /// </summary>
        /// <param name="district"></param>
        /// <param name="city"></param>
        /// <returns></returns>

        //public List<string> GetMDMPinCode(string district, string city)
        //{
        //    List<string> lstMDMPinCode = new List<string>();
        //    lstMDMPinCode = (from dis in SmartDostDbContext.MDM2SDMSalesGeographyMaster.Where(x => x.Level4Code == district && x.Level5Code == city)
        //                     select dis.Level6Code).Distinct().ToList();
        //    return lstMDMPinCode;
        //}

        /// <summary>
        /// GetMDMParentDealerCode
        /// </summary>
        /// <returns></returns>
        public List<string> GetMDMParentDealerCode()
        {
            List<string> lstMDMParentDealerCode = new List<string>();
            lstMDMParentDealerCode = (from dc in SmartDostDbContext.DealerCreations.Where(x => x.APPROVALSTATUS == 1)
                                      select dc.PARENTDEALERCODE).Distinct().ToList();
            return lstMDMParentDealerCode;
        }
        
        /// <summary>
        /// SubmitMDMDealerCreation
        /// </summary>
        /// <param name="dealerCreation"></param>
        /// <param name="emplCode"></param>
        /// <returns></returns>
        public int SubmitMDMDealerCreation(DealerCreation dealerCreation, string emplCode)
        {
            
            var countryState = (from a in SmartDostDbContext.MDM2SDMSalesGeographyMaster
                                join b in SmartDostDbContext.MDM2SDPoliticalGeographyMaster
                                     on a.Level5Code equals b.DistrictCode
                                where (a.Level5Code == dealerCreation.DISTRICT)
                                select new { CountryCode = a.Level1Code, b.StateCode }).FirstOrDefault();
            if (countryState != null)
            {
                dealerCreation.STATE = countryState.StateCode.ToString();
                dealerCreation.COUNTRY = countryState.CountryCode.ToString();
            }
            SmartDostDbContext.DealerCreations.Add(dealerCreation);
            SmartDostDbContext.SaveChanges();
            return dealerCreation.DealerCreationID;

        }
        /// <summary>
        /// Get created dealer data
        /// </summary>
        /// <param name="dealerCreationID"></param>
        /// <returns></returns>
        public DealerCreation GetDealerCreationData(int dealerCreationID)
        {
            return SmartDostDbContext.DealerCreations.FirstOrDefault(x => x.DealerCreationID == dealerCreationID);
        }
        /// <summary>
        /// PhotoMDMDealerCreation
        /// </summary>
        /// <param name="dealerCreation"></param>
        /// <param name="dealerCreationID"></param>
        /// <returns></returns>
        public bool PhotoMDMDealerCreation(DealerCreation dealerCreation, int dealerCreationID)
        {
            bool status = false;
            DealerCreation check = SmartDostDbContext.DealerCreations.Where(x => x.DealerCreationID == dealerCreationID).SingleOrDefault();
            if (check != null)
            {
                check.TINPHOTO = dealerCreation.TINPHOTO;
                check.PANPHOTO = dealerCreation.PANPHOTO;
                check.GSBPHOTO = dealerCreation.GSBPHOTO;
                check.OWNERPHOTO = dealerCreation.OWNERPHOTO;
                check.CONTACTPERSONPHOTO = dealerCreation.CONTACTPERSONPHOTO;
                SmartDostDbContext.Entry<DealerCreation>(check).State = System.Data.EntityState.Modified;
                //status = SmartDostDbContext.SaveChanges() > 0 ? true : false;
            }
            return status = SmartDostDbContext.SaveChanges() > 0 ? true : false;
        }

        #endregion
    }

}
