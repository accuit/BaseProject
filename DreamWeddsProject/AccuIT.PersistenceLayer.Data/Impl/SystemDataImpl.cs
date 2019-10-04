using AccuIT.PersistenceLayer.Repository.Contracts;
using AccuIT.PersistenceLayer.Repository.Entities;
using System.Linq;
#region Namespace Added for Role Master :Dhiraj 3-Dec-2013
using System.Collections.Generic;
using System.Collections;
using AccuIT.CommonLayer.Aspects.Utilities;
using System;
using System.Transactions;
using System.Data;
using System.Data.Objects;
using System.Web.Caching;
using System.Web;
using AccuIT.CommonLayer.Aspects.Logging;
using System.Xml.Linq;
using System.IO;
using System.Runtime.Serialization.Json;
using AccuIT.CommonLayer.Aspects.ReportBO;
using System.Data.SqlClient; //VC20140905
using AccuIT.BusinessLayer.Services.BO;
#endregion

namespace AccuIT.PersistenceLayer.Data.Impl
{
    /// <summary>
    /// Class to define methods for company system settings and services
    /// </summary>
    public class SystemDataImpl : BaseDataImpl, ISystemRepository
    {

        public List<int> UploadMasterDataParking2Main(System.Data.DataTable DT, int enumMaster)
        {
            string consString = AccuitAdminDbContext.Database.Connection.ConnectionString;
            List<int> result = new List<int>();
            ObjectParameter objParamRowsInserted = new ObjectParameter("RowsInserted", typeof(int));
            ObjectParameter objParamRowUpdated = new ObjectParameter("RowUpdated", typeof(int));

            string masterName = Enum.GetName(typeof(AspectEnums.enumExcelType), enumMaster);
            string parkingTable = masterName;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                // Delete existing data from the corresponding parking table
                // AccuitAdminDbContext.Database.ExecuteSqlCommand("DELETE FROM " + parkingTable);
                //AccuitAdminDbContext.SaveChanges();

                using (SqlConnection con = new SqlConnection(consString))
                {
                    #region Bulk Upload Excel File Data into DB
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        try
                        {

                            sqlBulkCopy.BulkCopyTimeout = 180;
                            sqlBulkCopy.DestinationTableName = masterName;
                            con.Open();
                            sqlBulkCopy.WriteToServer(DT);
                            con.Close();
                            result.Add(DT.Rows.Count);
                            result.Add(0);
                        }
                        catch
                        {
                            con.Close();
                            try
                            {
                                for (int i = 0; i < DT.Rows.Count; i++)
                                {
                                    string query = "";
                                    if (enumMaster == 2)
                                    {
                                        query = "INSERT INTO [ProductMaster] (ProductName, MeasureUnit, CategoryType, Availability, CategoryID, UnitPrice, Status,DateCreated, CreatedBy) "
                                      + "Values('" + DT.Rows[i][0].ToString() + "','" + DT.Rows[i][1].ToString() + "','" + DT.Rows[i][2].ToString() + "','" + DT.Rows[i][3].ToString() + "','" + DT.Rows[i][4].ToString() + "','" + DT.Rows[i][5].ToString() + "','" + DT.Rows[i][6].ToString() + "','" + DT.Rows[i][7].ToString() + "')";
                                    }
                                    else if (enumMaster == 3)
                                    {
                                        query = "INSERT INTO [CategoryMaster] (ParentCatgID, CategoryName, CategoryCode, Description, Picture, isDeleted, Created_By, Created_Date) "
                                      + "Values('" + DT.Rows[i][1].ToString() + "','" + DT.Rows[i][2].ToString() + "','" + DT.Rows[i][3].ToString() + "','" + DT.Rows[i][4].ToString() + "','" + DT.Rows[i][5].ToString() + "','" + DT.Rows[i][6].ToString() + "','" + DT.Rows[i][7].ToString() + "','" + DT.Rows[i][9].ToString() + "')";
                                    }
                                    else if (enumMaster == 4)
                                    {
                                        query = "INSERT INTO [ProductImages] (ProductImageID,Name, Caption, ImageUrl, IsPrime, CategoryID, UnitPrice, Status,DateCreated, CreatedBy) "
                                      + "Values('" + DT.Rows[i][0].ToString() + "','" + DT.Rows[i][1].ToString() + "','" + DT.Rows[i][2].ToString() + "','" + DT.Rows[i][3].ToString() + "','" + DT.Rows[i][4].ToString() + "','" + DT.Rows[i][5].ToString() + "','" + DT.Rows[i][6].ToString() + "','" + DT.Rows[i][7].ToString() + "')";
                                    }
                                    con.Open();
                                    SqlCommand cmd = new SqlCommand(query, con);
                                    cmd.ExecuteNonQuery();
                                    con.Close();

                                    result.Add(DT.Rows.Count);
                                    result.Add(0);
                                }
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }


                    }

                    #endregion
                }

                #region Call Entity SP based on master

                //switch (enumMaster)
                //{
                //    case 1:
                //       AccuitAdminDbContext.spUploadProductMaster(objParamRowsInserted, objParamRowUpdated);
                //        break;

                //}

                #endregion

                scope.Complete();

                // result.Add(Convert.ToInt32(objParamRowsInserted.Value));
                //result.Add(Convert.ToInt32(objParamRowUpdated.Value));
            }

            return result;
        }

        public AddressMaster GetAddressDetails(int addressID, int? ID, int? Type)
        {
            if (addressID > 0)
                return AccuitAdminDbContext.AddressMasters.Where(x => x.AddressID == addressID).FirstOrDefault();
            else
                return AccuitAdminDbContext.AddressMasters.Where(x => x.AddressOwnerType == Type && x.AddressOwnerTypePKID == ID).FirstOrDefault();
        }
        public AddressMaster GetAddressDetailsByType(int typeID, int typePkID)
        {
            return AccuitAdminDbContext.AddressMasters.Where(x => x.AddressOwnerType == typeID && x.AddressOwnerTypePKID == typePkID).FirstOrDefault();
        }

        public TemplateMaster GetTemplateData(int templateID, int? code)
        {
            TemplateMaster TM = new TemplateMaster();
            if (code > 0)
                TM = AccuitAdminDbContext.TemplateMasters.Where(x => x.TemplateCode == code && x.IsDeleted == false).FirstOrDefault();
            else
                TM = AccuitAdminDbContext.TemplateMasters.Where(x => x.TemplateID == templateID && x.IsDeleted == false).FirstOrDefault();
            return TM;
        }

        public TemplateMasterBO GetBasicTemplateInfo(int ID)
        {
            TemplateMaster template = new TemplateMaster();

            AccuitAdminDbContext.Configuration.LazyLoadingEnabled = false;
            var data = AccuitAdminDbContext.TemplateMasters.Where(x => x.TemplateID == ID)
                  .Select(x => new
                  {
                      TemplateName = x.TemplateName,
                      ThumbnailImageUrl = x.ThumbnailImageUrl,
                      TemplateFolderPath = x.TemplateFolderPath,
                      TemplateUrl = x.TemplateUrl
                    
                  });

            var dds = data.ToList().Select(r => new TemplateMasterBO
            {
                TemplateName = r.TemplateName,
                ThumbnailImageUrl = r.ThumbnailImageUrl
            }).ToList();


            return dds.FirstOrDefault();
        }

        public List<TemplateMaster> GetAllTemplates(int? type)
        {
            if (type > 0)
                return AccuitAdminDbContext.TemplateMasters
                    .Where(x => x.TemplateType == type && x.TemplateStatus == (int)AspectEnums.TemplateStatus.Active && x.IsDeleted == false)
                    .ToList();
            else
                return AccuitAdminDbContext.TemplateMasters
                    .Where(x => x.TemplateStatus == (int)AspectEnums.TemplateStatus.Active && x.IsDeleted == false)
                    .ToList();

        }
        public List<TemplateMergeField> GetTemplateMergeFields(int templateID)
        {
            List<TemplateMergeField> fields = new List<TemplateMergeField>();
            return AccuitAdminDbContext.TemplateMergeFields.Where(x => x.IsDeleted == false).ToList();
        }

        public List<TemplateImage> SubmitTemplateImages(List<TemplateImage> Images)
        {
            foreach (var image in Images)
            {
                AccuitAdminDbContext.TemplateImages.Add(image);
                image.ImageID = AccuitAdminDbContext.SaveChanges();
            }
            return Images;
        }

        public int SubmitNewOrder(OrderMaster Order)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                Order.CreatedDate = DateTime.Now;
                Order.OrderDate = DateTime.Now;
                Order.RequiredDate = DateTime.Now;
                Order.IsDeleted = false;
                Order.OrderStatus = AspectEnums.OrderStatus.Confirmed.ToString();
                AccuitAdminDbContext.OrderMasters.Add(Order);
                AccuitAdminDbContext.SaveChanges();
                scope.Complete();
            }
            return Order.OrderID;
        }
        public int UpdateOrder(OrderMaster Order)
        {
            int Id = 0;
            using (TransactionScope scope = new TransactionScope())
            {
                var myOrder = AccuitAdminDbContext.OrderMasters.Where(x => x.OrderID == Order.OrderID).FirstOrDefault();

                myOrder.ModifiedDate = DateTime.Now;
                myOrder.ModifiedBy = Order.UserID;
                myOrder.Amount = Order.Amount;
                myOrder.CGST = Order.CGST;
                myOrder.SGST = Order.SGST;
                myOrder.ReceivedAmount = Order.ReceivedAmount;
                myOrder.IsDeleted = Order.IsDeleted;
                myOrder.OrderDate = DateTime.Now;
                myOrder.RequiredDate = DateTime.Now;
                myOrder.OderNote = Order.OderNote;
                myOrder.PaymentMode = Order.PaymentMode;
                myOrder.PaymentTerms = Order.PaymentTerms;
                myOrder.Discount = Order.Discount;
                myOrder.OrderStatus = Order.OrderStatus;
                AccuitAdminDbContext.Entry<OrderMaster>(myOrder).State = System.Data.Entity.EntityState.Modified;
                Id = AccuitAdminDbContext.SaveChanges();
                scope.Complete();
            }
            return Id;
        }

        public int SubmitUserSubscription(UserWeddingSubscription Subscription)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                Subscription.StartDate = DateTime.Now;
                Subscription.IsDeleted = false;
                AccuitAdminDbContext.UserWeddingSubscriptions.Add(Subscription);
                AccuitAdminDbContext.SaveChanges();
                scope.Complete();
            }
            return Subscription.UserWeddingSubscrptionID;
        }
        public int UpdateUserSubscription(UserWeddingSubscription Subscription)
        {

            using (TransactionScope scope = new TransactionScope())
            {
                var mySubscription = AccuitAdminDbContext.UserWeddingSubscriptions.Where(x => x.UserWeddingSubscrptionID == Subscription.UserWeddingSubscrptionID).FirstOrDefault();

                mySubscription.StartDate = DateTime.Now;
                mySubscription.UserId = Subscription.UserId;
                mySubscription.StartDate = Subscription.StartDate;
                mySubscription.EndDate = Subscription.EndDate;
                mySubscription.InvoiceNo = Subscription.InvoiceNo;
                mySubscription.IsDeleted = Subscription.IsDeleted;
                mySubscription.WeddingID = Subscription.WeddingID;
                mySubscription.SubscriptionStatus = Subscription.SubscriptionStatus;
                mySubscription.SubscriptionType = Subscription.SubscriptionType;
                mySubscription.UpdatedDate = DateTime.Now;
                mySubscription.ReasonOfUpdate = Subscription.ReasonOfUpdate;
                AccuitAdminDbContext.Entry<UserWeddingSubscription>(mySubscription).State = System.Data.Entity.EntityState.Modified;
                AccuitAdminDbContext.SaveChanges();
                scope.Complete();
            }
            return Subscription.UserWeddingSubscrptionID;
        }

        /// <summary>
        /// Method to log error details onto server database
        /// </summary>
        /// <param name="errorLog">error log DTO instance</param>
        /// <returns>returns boolean response</returns>
        //public bool WriteLog(ErrorLog errorLog)
        //{
        //    AccuitAdminDbContext.ErrorLogs.Add(errorLog);
        //    int recordsAffected = AccuitAdminDbContext.SaveChanges();
        //    return recordsAffected > 0 ? true : false;
        //}
        //     int Interval = Convert.ToInt32(AppUtil.GetAppSettings(AspectEnums.ConfigKeys.Interval)); //VC20140909
        /// <summary>
        /// Method to validate WCF service user access 
        /// </summary>
        /// <param name="apiKey">provided API Key value</param>
        /// <param name="apiToken">provided API Token value</param>
        /// <returns>returns boolean status</returns>
        public bool IsValidServiceUser(string apiKey, string apiToken, string userID)
        {
            int userIDInt = 0;
            Int32.TryParse(userID, out userIDInt);
            return AccuitAdminDbContext.UserServiceAccesses.FirstOrDefault(k => k.APIKey == apiKey && k.APIToken == apiToken && k.UserID == userIDInt) != null;
        }

        /// <summary>
        /// Method to get company system setting values
        /// </summary>
        /// <param name="companyID">company ID</param>
        /// <returns>returns entity instance</returns>
        public SystemSetting GetCompanySystemSettings(int companyID)
        {
            return AccuitAdminDbContext.SystemSettings.FirstOrDefault(k => k.CompanyID == companyID);
        }



        //     /// <summary>
        //     /// Insert/Update System Settings
        //     /// </summary>
        //     /// <param name="systemSetting"></param>
        //     /// <returns></returns>
        //     public bool InsertUpdateSystemSetting(SystemSetting systemSetting, int? RoleID)
        //     {
        //         bool isSuccess = false;
        //         using (TransactionScope scope = new TransactionScope())
        //         {
        //             if (systemSetting != null)
        //             {
        //                 SystemSetting setting = AccuitAdminDbContext.SystemSettings.SingleOrDefault(k => k.SettingID == systemSetting.SettingID);
        //                 if (setting != null && setting.SettingID > 0)
        //                 {
        //                     setting.CoverageApproveDays = systemSetting.CoverageApproveDays;
        //                     setting.LogoutTime = systemSetting.LogoutTime;
        //                     setting.LoginFailedAttempt = systemSetting.LoginFailedAttempt;
        //                     setting.IdleSystemDay = systemSetting.IdleSystemDay;
        //                     setting.MaxStorePerDay = systemSetting.MaxStorePerDay;
        //                     //setting.CoveragePlanFirstWindow = systemSetting.CoveragePlanFirstWindow;
        //                     //setting.CoveragePlanSecondWndow = systemSetting.CoveragePlanSecondWndow;
        //                     setting.DataSyncInterval = systemSetting.DataSyncInterval;
        //                     setting.CoverageRejectionEditHours = systemSetting.CoverageRejectionEditHours;
        //                     setting.MaxLeaveMarkDays = systemSetting.MaxLeaveMarkDays;
        //                     setting.WeeklyOffDays = systemSetting.WeeklyOffDays;
        //                     setting.ModifiedDate = System.DateTime.Now;
        //                     setting.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]);
        //                     AccuitAdminDbContext.Entry<SystemSetting>(setting).State = System.Data.Entity.EntityState.Modified;
        //                 }
        //                 else
        //                 {
        //                     systemSetting.CreatedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]);
        //                     AccuitAdminDbContext.Entry<SystemSetting>(systemSetting).State = System.Data.Entity.EntityState.Added;
        //                 }



        //                 #region code added by vaishali to save Beat Window Settings
        //                 //if ((systemSetting.CoveragePlanFirstWindow != "" && systemSetting.CoveragePlanFirstWindow != null) &&
        //                 //    (systemSetting.CoveragePlanSecondWndow != "" && systemSetting.CoveragePlanSecondWndow != null))
        //                 if (RoleID != null)// #SDCE-3506 coverageplan window check removed with roleid null check for performance
        //                 {
        //                     BeatWindowSetting beatWindowSetting = AccuitAdminDbContext.BeatWindowSettings.Where(x => x.RoleID == RoleID).FirstOrDefault();

        //                     if (beatWindowSetting != null)
        //                     {
        //                         beatWindowSetting.CoveragePlanFirstWindow = systemSetting.CoveragePlanFirstWindow;
        //                         beatWindowSetting.CoveragePlanSecondWndow = systemSetting.CoveragePlanSecondWndow;
        //                         beatWindowSetting.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]);
        //                         beatWindowSetting.ModifyDate = System.DateTime.Now;
        //                         AccuitAdminDbContext.Entry<BeatWindowSetting>(beatWindowSetting).State = System.Data.Entity.EntityState.Modified;
        //                     }
        //                     else
        //                     {
        //                         BeatWindowSetting insertbeatWindowSetting = new BeatWindowSetting()
        //                         {
        //                             CoveragePlanFirstWindow = systemSetting.CoveragePlanFirstWindow,
        //                             CoveragePlanSecondWndow = systemSetting.CoveragePlanSecondWndow,
        //                             CreatedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]),
        //                             CreatedDate = System.DateTime.Now,
        //                             RoleID = RoleID.Value,
        //                         };

        //                         AccuitAdminDbContext.Entry<BeatWindowSetting>(insertbeatWindowSetting).State = System.Data.Entity.EntityState.Added;
        //                     }
        //                 }
        //                 #endregion

        //                 isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //             }

        //             scope.Complete();
        //         }
        //         return isSuccess;
        //     }

        //     /// <summary>
        //     /// Gets the system settings.
        //     /// </summary>
        //     /// <returns></returns>
        //     public SystemSetting GetSystemSettings()
        //     {
        //         SystemSetting setting = AccuitAdminDbContext.SystemSettings.OrderByDescending(k => k.CreatedDate).ThenByDescending(k => k.ModifiedDate).FirstOrDefault();
        //         return setting;
        //     }

        //     #region Methods Added for Role Master :Dhiraj 3-Dec-2013

        /// <summary>
        /// Get All role data
        /// </summary>
        /// <returns></returns>
        public List<RoleMaster> GetRoleMasters()
        {
            return AccuitAdminDbContext.RoleMasters.Where(k => !k.IsDeleted).OrderBy(o => o.Name).ToList();
        }


        //     /// <summary>
        //     /// Get All role based on user Company
        //     /// </summary>
        //     /// <returns></returns>
        //     public List<RoleMaster> GetRoleMasters(int companyID)
        //     {
        //         return AccuitAdminDbContext.RoleMasters.Where(k => !k.IsDeleted && k.CompanyID == companyID).OrderBy(o => o.Name).ToList();
        //     }

        //     /// <summary>
        //     /// Get All Role Name with team and Profile Level
        //     /// </summary>
        //     /// <param name="companyID"></param>
        //     /// <param name="profileLevel"></param>
        //     /// <returns></returns>
        //     public List<spGetRoleMaster_Result> GetRoleMasters(int companyID, int? profileLevel)
        //     {
        //         return AccuitAdminDbContext.spGetRoleMaster(companyID, profileLevel).ToList();
        //     }

        //     /// <summary>
        //     /// Get role data for roleid
        //     /// added by amjad on 24 may2015
        //     /// </summary>
        //     /// <returns></returns>
        //     public List<RoleMaster> GetRoleMastersByRoleId(Int32 RoleId)
        //     {
        //         return AccuitAdminDbContext.RoleMasters.Where(k => k.RoleID == RoleId &&
        //             !k.IsDeleted).ToList();

        //     }
        //     public List<ApproverPathMaster> GetApprovalParthMaster(int roleId, int expenseType)
        //     {
        //         return AccuitAdminDbContext.ApproverPathMasters.Where(k => k.RoleID == roleId &&
        //             k.EMSExpenseTypeMasterId == expenseType && !k.IsDeleted && k.IsActive).ToList();

        //     }
        /// <summary>
        /// Get All rolemoudle data for particular roleID
        /// </summary>
        /// <param name="RoleID">Particular Role ID for which data of role module needs to be fetched</param>
        /// <returns>List of RoleModule</returns>
        public List<RoleModule> GetRoleModulesByRoleID(int RoleID, int? ModuleID)
        {
            if (ModuleID != null)
            {
                return AccuitAdminDbContext.RoleModules.Where(x => x.RoleID == RoleID && x.ModuleID == ModuleID).ToList();
            }
            else
            {
                return AccuitAdminDbContext.RoleModules.Where(x => x.RoleID == RoleID).ToList();
            }

        }

        /// <summary>
        /// Gets the module name by module identifier.
        /// </summary>
        /// <param name="moduleID">The module identifier.</param>
        /// <returns></returns>
        public string GetModuleNameByModuleID(int moduleID)
        {
            string name = (from modules in AccuitAdminDbContext.ModuleMasters
                           where modules.ModuleID == moduleID && modules.IsDeleted == false
                           select modules.Name).SingleOrDefault();
            return name;
        }

        /// <summary>
        /// Get All Permissions
        /// </summary>        
        /// <returns>List of Valid permisions</returns>
        public List<Permission> GetPermissions()
        {
            return AccuitAdminDbContext.Permissions.ToList();
        }

        /// <summary>
        /// Get All RoleModulePermissions
        /// </summary>        
        /// <returns>List of Valid permisions</returns>
        public List<UserRoleModulePermission> GetUserRoleModulePermisions()
        {
            return AccuitAdminDbContext.UserRoleModulePermissions.ToList();
        }

        /// <summary>
        /// Insert update RoleModulePermission of user
        /// </summary>        
        /// <returns>Success or Failure</returns>
        public bool InsertUpdateUserRoleModulePermision(UserRoleModulePermission userRoleModulePermission)
        {
            bool isSuccess = false;
            if (userRoleModulePermission != null)
            {
                UserRoleModulePermission userRoleModulePermissionObj = AccuitAdminDbContext.UserRoleModulePermissions.SingleOrDefault(k => k.RoleModuleID == userRoleModulePermission.RoleModuleID && k.PermissionID == userRoleModulePermission.PermissionID);
                if (userRoleModulePermissionObj != null)//Record Exists
                {
                    userRoleModulePermissionObj.ModifiedDate = System.DateTime.Now;
                    userRoleModulePermissionObj.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]); //added by vaishali on 23 Feb 2014
                    userRoleModulePermissionObj.PermissionValue = userRoleModulePermission.PermissionValue;
                    AccuitAdminDbContext.Entry<UserRoleModulePermission>(userRoleModulePermissionObj).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    userRoleModulePermission.CreatedDate = System.DateTime.Now;
                    userRoleModulePermission.CreatedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]); //added by vaishali on 23 Feb 2014
                    AccuitAdminDbContext.Entry<UserRoleModulePermission>(userRoleModulePermission).State = System.Data.Entity.EntityState.Added;
                }
                isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Insert Role Module data
        /// </summary>        
        /// <returns>Success or Failure</returns>
        public bool InsertRoleModule(RoleModule roleModule)
        {
            bool isSuccess = false;
            if (roleModule != null)
            {
                AccuitAdminDbContext.Entry<RoleModule>(roleModule).State = System.Data.Entity.EntityState.Added;
                isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
            }
            return isSuccess;
        }

        /// <summary>
        /// Delete Role Module data
        /// </summary>        
        /// <returns>Success or Failure</returns>
        public bool DeleteRoleModule(RoleModule roleModule)
        {
            bool isSuccess = false;
            if (roleModule != null)
            {
                var roleModuleExisting = AccuitAdminDbContext.RoleModules.Where(x => x.RoleModuleID == roleModule.RoleModuleID).SingleOrDefault();
                if (roleModule != null)
                {
                    roleModuleExisting.ModifiedDate = System.DateTime.Now;
                    roleModuleExisting.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]);
                    roleModuleExisting.IsDeleted = roleModule.IsDeleted;

                    AccuitAdminDbContext.Entry<RoleModule>(roleModuleExisting).State = System.Data.Entity.EntityState.Modified;
                    isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
                }
                else
                {
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        //     #endregion

        //     #region Question Master Methods

        /// <summary>
        /// Function to get the Modules
        /// </summary>
        public IList<ModuleMaster> GetModulesList(bool? isMobile = false)
        {
            var list = new List<ModuleMaster>();
            if (isMobile != null)
            {
                //list = AccuitAdminDbContext.Modules.ToList();Commented by Dhiraj on 26-Dec-2013 to fetch only active modules
                list = AccuitAdminDbContext.ModuleMasters.Where(x => x.IsDeleted == false && (x.IsMobile == isMobile) && x.IsSystemModule == true).OrderBy(o => o.Name).ToList();
            }
            else
            {
                list = AccuitAdminDbContext.ModuleMasters.Where(x => x.IsDeleted == false).OrderBy(o => o.Name).ToList();
            }
            return list;
        }

        /// <summary>
        /// Function to get the Modules on the basis of role
        /// </summary>
        public IList<RoleModule> GetModulesListByRole(int roleID)
        {
            return AccuitAdminDbContext.RoleModules.Where(k => k.RoleID == roleID).ToList();
        }

        //     /// <summary>
        //     /// Function to bind the Questions base on Module
        //     /// </summary>
        //     /// <param name="moduleID">ModuleID</param>
        //     /// <returns>Question List</returns>
        //     public IList<SurveyQuestion> GetQuestionList(int moduleID)
        //     {
        //         var lstQuestionlst = new List<SurveyQuestion>();
        //         var module = AccuitAdminDbContext.Modules.Where(x => x.ModuleCode == moduleID && x.IsDeleted == false).FirstOrDefault();
        //         if (module != null)
        //             lstQuestionlst = AccuitAdminDbContext.SurveyQuestions.Where(k => k.ModuleID == module.ModuleID && !k.IsDeleted).OrderBy(k => k.Sequence).ToList();
        //         //lstQuestionlst = AccuitAdminDbContext.SurveyQuestions.Where(k => k.ModuleID == moduleID && k.IsDeleted == false && k.IsActive == true).OrderBy(k => k.Sequence).ToList();
        //         return lstQuestionlst;
        //     }

        //     /// <summary>
        //     /// /// Function to find the Questions base on search string data for particular module code
        //     /// added by amjad on 24 may2015
        //     /// </summary>
        //     /// <returns>Question List</returns>
        //     public IList<SurveyQuestion> GetQuestionList(int moduleCode, string strSearchData)
        //     {
        //         var lstQuestionlst = new List<SurveyQuestion>();
        //         var module = AccuitAdminDbContext.Modules.Where(x => x.ModuleCode == moduleCode && x.IsDeleted == false).FirstOrDefault();
        //         if (module != null)
        //             lstQuestionlst = AccuitAdminDbContext.SurveyQuestions.Where(k => (k.ModuleID == module.ModuleID && k.Question.Contains(strSearchData.ToLower())
        //                                                 && !k.IsDeleted)).OrderBy(k => k.Sequence).ToList();
        //         return lstQuestionlst;
        //     }

        //     /// <summary>
        //     /// Function to bind the Questions base on Module
        //     /// </summary>
        //     /// <param name="moduleID">ModuleID</param>
        //     /// <returns>Answer List</returns>
        //     public IList<SurveyQuestionAttribute> GetAnswerList(int questionID)
        //     {
        //         var lstAnswerlst = new List<SurveyQuestionAttribute>();
        //         lstAnswerlst = AccuitAdminDbContext.SurveyQuestionAttributes.Where(k => k.SurveyQuestionID == questionID && k.IsActive).OrderBy(k => k.Sequence).ToList();
        //         return lstAnswerlst;
        //     }


        //     /// <summary>
        //     /// Function to bind the Questions base on Module
        //     /// </summary>
        //     /// <param name="moduleID">ModuleID</param>
        //     /// <returns>Answer List</returns>
        //     public SurveyQuestionAttribute GetAnswer(int SurveyOptionID)
        //     {
        //         var lstAnswerlst = new SurveyQuestionAttribute();
        //         lstAnswerlst = AccuitAdminDbContext.SurveyQuestionAttributes.SingleOrDefault(k => k.SurveyOptionID == SurveyOptionID);
        //         return lstAnswerlst;
        //     }

        //     /// <summary>
        //     /// Function to bind the QuestionsType
        //     /// </summary>     
        //     /// <returns>Question Type List</returns>
        //     public IList<QuestionType> QuestionTypeList(bool isForRepeaterType)
        //     {
        //         var lstQuestionTypelst = new List<QuestionType>();
        //         if (isForRepeaterType == true)
        //         {
        //             lstQuestionTypelst = AccuitAdminDbContext.QuestionTypes.Where
        //                 (x => x.QuestionTypeID == (int)AspectEnums.QuestionTypes.PictureBox ||
        //                     x.QuestionTypeID == (int)AspectEnums.QuestionTypes.Rating ||
        //                     x.QuestionTypeID == (int)AspectEnums.QuestionTypes.NumericText ||
        //                     x.QuestionTypeID == (int)AspectEnums.QuestionTypes.Textbox ||
        //                     x.QuestionTypeID == (int)AspectEnums.QuestionTypes.Calendar ||
        //                     x.QuestionTypeID == (int)AspectEnums.QuestionTypes.ToggleButton
        //                 //                        ||x.QuestionTypeID == (int)AspectEnums.QuestionTypes.Label 
        //                     ).ToList();
        //         }
        //         else
        //             lstQuestionTypelst = AccuitAdminDbContext.QuestionTypes.ToList();
        //         return lstQuestionTypelst;
        //     }

        //     //#region added by vaishali to select question types for Repeater (SDCE-1175)
        //     ///// <summary>
        //     ///// Function to bind the QuestionsType for repeater
        //     ///// </summary>     
        //     ///// <returns>Question Type List</returns>
        //     //public IList<QuestionType> QuestionTypeList(bool forRepeater)
        //     //{
        //     //    var lstQuestionTypelst = new List<QuestionType>();
        //     //    lstQuestionTypelst = AccuitAdminDbContext.QuestionTypes.ToList();
        //     //    return lstQuestionTypelst;
        //     //}
        //     //#endregion

        //     /// <summary>
        //     /// Function to bind the Questions base on Module
        //     /// </summary>
        //     /// <param name="moduleID">ModuleID</param>
        //     /// <returns>0 or 1</returns>
        //     public bool ISDeleteQuestion(int questionID)
        //     {
        //         bool isSuccess = false;

        //         SurveyQuestion result = AccuitAdminDbContext.SurveyQuestions.SingleOrDefault(k => k.SurveyQuestionID == questionID);
        //         var resultAnswerLst = AccuitAdminDbContext.SurveyQuestionAttributes.Where(k => k.SurveyQuestionID == questionID);

        //         if (result != null)
        //         {
        //             result.IsDeleted = true;
        //             result.IsActive = false;
        //             AccuitAdminDbContext.Entry<SurveyQuestion>(result).State = System.Data.Entity.EntityState.Modified;
        //             if (resultAnswerLst != null)
        //             {
        //                 foreach (var item in resultAnswerLst)
        //                 {
        //                     item.IsActive = false;
        //                     SurveyQuestionAttribute resultAns = AccuitAdminDbContext.SurveyQuestionAttributes.SingleOrDefault(k => k.SurveyOptionID == item.SurveyOptionID);
        //                     resultAns.IsActive = false;
        //                     AccuitAdminDbContext.Entry<SurveyQuestionAttribute>(resultAns).State = System.Data.Entity.EntityState.Modified;
        //                 }
        //             }
        //         }
        //         isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //         return isSuccess;
        //     }

        //     /// <summary>
        //     /// Determines whether [is active question] [the specified question identifier].
        //     /// </summary>
        //     /// <param name="questionID">The question identifier.</param>
        //     /// <returns></returns>
        //     public bool ISActiveQuestion(int questionID)
        //     {
        //         bool isSuccess = false;

        //         SurveyQuestion result = AccuitAdminDbContext.SurveyQuestions.SingleOrDefault(k => k.SurveyQuestionID == questionID && !k.IsActive && !k.IsDeleted);
        //         if (result != null)
        //         {
        //             result.IsActive = true;
        //             AccuitAdminDbContext.Entry<SurveyQuestion>(result).State = System.Data.Entity.EntityState.Modified;
        //         }
        //         isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //         return isSuccess;
        //     }

        //     /// <summary>
        //     /// Method to save questions into database
        //     /// </summary>
        //     /// <param name="surveyQuestion">survey questions</param>
        //     /// <param name="answers">answers</param>
        //     /// <returns>returns boolean response</returns>
        //     public bool SaveSurveyQuestion(SurveyQuestion surveyQuestion, List<AnswerAttribute> answers)
        //     {
        //         bool isSuccess = false;
        //         using (TransactionScope scope = new TransactionScope())
        //         {
        //             SurveyQuestion question = AccuitAdminDbContext.SurveyQuestions.FirstOrDefault(k => k.SurveyQuestionID == surveyQuestion.SurveyQuestionID && k.IsDeleted == false);
        //             if (question != null)
        //             {
        //                 if (question.SurveyQuestionID > 0)
        //                 {
        //                     if (question.Question.Trim() != surveyQuestion.Question.Trim())
        //                     {
        //                         question.IsActive = false;
        //                         question.ModifiedDate = System.DateTime.Now;
        //                         AccuitAdminDbContext.Entry<SurveyQuestion>(question).State = System.Data.Entity.EntityState.Modified;
        //                         AccuitAdminDbContext.SaveChanges();
        //                         List<SurveyQuestionAttribute> options = AccuitAdminDbContext.SurveyQuestionAttributes.Where(k => k.SurveyQuestionID == surveyQuestion.SurveyQuestionID && k.IsActive).ToList();
        //                         surveyQuestion.CreatedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]); //added by vaishali on 23 Feb 2014
        //                         surveyQuestion.CreatedDate = System.DateTime.Now;
        //                         surveyQuestion.IsActive = true;
        //                         AccuitAdminDbContext.Entry<SurveyQuestion>(surveyQuestion).State = System.Data.Entity.EntityState.Added;
        //                         AccuitAdminDbContext.SaveChanges();
        //                         int questionID = surveyQuestion.SurveyQuestionID;
        //                         foreach (var item in options)
        //                         {
        //                             item.SurveyQuestionID = questionID;
        //                             item.IsActive = true;
        //                             AccuitAdminDbContext.Entry<SurveyQuestionAttribute>(item).State = System.Data.Entity.EntityState.Added;
        //                             AccuitAdminDbContext.SaveChanges();
        //                             if (answers.Count > 0)
        //                             {
        //                                 SaveQuestionOptions(answers, questionID);
        //                             }

        //                         }
        //                     }
        //                     else
        //                     {
        //                         question.TextLength = surveyQuestion.TextLength;
        //                         question.Sequence = surveyQuestion.Sequence;
        //                         question.ModifiedDate = System.DateTime.Now;
        //                         question.Question = surveyQuestion.Question;
        //                         question.ModuleID = surveyQuestion.ModuleID;
        //                         question.ProductGroupID = surveyQuestion.ProductGroupID;

        //                         question.ModifiedByUserID = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]); //added by vaishali on 23 Feb 2014

        //                         //SDCE-638 Added by Niranjan (Product Group Category In Question Master) 15-10-2014
        //                         question.ProductGroupCategoryId = surveyQuestion.ProductGroupCategoryId;
        //                         question.QuestionTypeID = surveyQuestion.QuestionTypeID;
        //                         question.DependentOptionID = surveyQuestion.DependentOptionID;
        //                         question.QuestionImage = surveyQuestion.QuestionImage; //VC20140904
        //                         #region--added by amjad on 24 may2015--
        //                         question.IsMandatory = surveyQuestion.IsMandatory;
        //                         question.RepeaterTypeID = surveyQuestion.RepeaterTypeID;
        //                         question.RepeaterText = surveyQuestion.RepeaterText;
        //                         question.RepeatMaxTimes = surveyQuestion.RepeatMaxTimes;
        //                         //question.RepeaterTextLength = surveyQuestion.RepeaterTextLength;
        //                         #endregion----------------------------------
        //                         AccuitAdminDbContext.Entry<SurveyQuestion>(question).State = System.Data.Entity.EntityState.Modified;
        //                         AccuitAdminDbContext.SaveChanges();
        //                         if (answers.Count > 0)
        //                         {
        //                             SaveQuestionOptions(answers, question.SurveyQuestionID);
        //                         }
        //                     }

        //                 }
        //             }
        //             else
        //             {
        //                 int oldQuestionID = surveyQuestion.SurveyQuestionID;
        //                 surveyQuestion.CreatedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]); //added by vaishali on 23 Feb 2014
        //                 surveyQuestion.CreatedDate = System.DateTime.Now;
        //                 surveyQuestion.IsActive = true;
        //                 if (surveyQuestion.SurveyQuestionID > 0)
        //                 {
        //                     surveyQuestion.ModifiedDate = System.DateTime.Now;
        //                     surveyQuestion.IsDeleted = false;
        //                 }

        //                 AccuitAdminDbContext.SurveyQuestions.Add(surveyQuestion);
        //                 AccuitAdminDbContext.SaveChanges();
        //                 int newQuestionID = surveyQuestion.SurveyQuestionID;
        //                 if (answers.Count > 0)
        //                 {
        //                     SaveQuestionOptions(answers, newQuestionID);
        //                 }
        //                 if (oldQuestionID > 0)
        //                 {
        //                     SurveyQuestion oldQuestion = AccuitAdminDbContext.SurveyQuestions.FirstOrDefault(k => k.SurveyQuestionID == oldQuestionID);
        //                     if (oldQuestion != null)
        //                     {
        //                         oldQuestion.IsDeleted = true;
        //                         oldQuestion.ModifiedDate = System.DateTime.Now;
        //                         AccuitAdminDbContext.Entry<SurveyQuestion>(oldQuestion).State = System.Data.Entity.EntityState.Modified;
        //                         AccuitAdminDbContext.SaveChanges();
        //                     }
        //                 }


        //             }
        //             isSuccess = true;
        //             scope.Complete();
        //         }
        //         return isSuccess;
        //     }

        //     /// <summary>
        //     /// Method to save question options into database
        //     /// </summary>
        //     /// <param name="answers">answers</param>
        //     /// <param name="questionID">question id</param>
        //     private void SaveQuestionOptions(List<AnswerAttribute> answers, int questionID)
        //     {
        //         foreach (var option in answers)
        //         {
        //             SurveyQuestionAttribute attribute = AccuitAdminDbContext.SurveyQuestionAttributes.FirstOrDefault(k => k.SurveyQuestionID == questionID);
        //             //if (attribute != null)
        //             //{
        //             attribute = new SurveyQuestionAttribute();
        //             attribute.IsActive = true;
        //             attribute.OptionValue = option.Answer;
        //             attribute.Sequence = Convert.ToInt32(string.IsNullOrEmpty(option.Sequence) ? "0" : option.Sequence);
        //             attribute.SurveyQuestionID = questionID;
        //             attribute.CreatedDate = System.DateTime.Now;
        //             attribute.IsAffirmative = option.IsAffirmative;// Added by Manoranjan SDCE-4241
        //             AccuitAdminDbContext.SurveyQuestionAttributes.Add(attribute);
        //             AccuitAdminDbContext.SaveChanges();
        //             //}
        //         }
        //     }
        //     /// <summary>   
        //     /// Function to Add the Questions base on Module
        //     /// </summary>
        //     /// <param name="SurveyQuestion">SurveyQuestion</param>
        //     /// <returns>0 or 1</returns>
        //     public bool IsSuccessfullInsert(SurveyQuestion record, List<AnswerAttribute> resultAnswer)
        //     {
        //         bool isSuccess = false;
        //         SurveyQuestionAttribute answers = new SurveyQuestionAttribute();
        //         var resultAnswerLst = AccuitAdminDbContext.SurveyQuestionAttributes.Where(k => k.SurveyQuestionID == record.SurveyQuestionID);

        //         if (record.SurveyQuestionID.Equals(0))
        //         {
        //             record.IsActive = true;
        //             record.CreatedDate = System.DateTime.Now;
        //             record.CreatedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]); //added by vaishali on 23 Feb 2014
        //             record.IsMandatory = false;
        //             record.IsDeleted = false;
        //             AccuitAdminDbContext.Entry<SurveyQuestion>(record).State = System.Data.Entity.EntityState.Added;
        //             AccuitAdminDbContext.SaveChanges();
        //             var question = AccuitAdminDbContext.SurveyQuestions.OrderByDescending(k => k.CreatedDate).FirstOrDefault();

        //             answers.SurveyQuestionID = question.SurveyQuestionID;
        //             foreach (var item in resultAnswer)
        //             {
        //                 answers.CreatedDate = System.DateTime.Now;
        //                 answers.OptionValue = item.Answer;
        //                 answers.Sequence = Convert.ToInt16(item.Sequence);
        //                 answers.IsActive = true;
        //                 record.CreatedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]); //added by vaishali on 23 Feb 2014
        //                 AccuitAdminDbContext.Entry<SurveyQuestionAttribute>(answers).State = System.Data.Entity.EntityState.Added;
        //                 AccuitAdminDbContext.SaveChanges();
        //             }
        //             isSuccess = true;
        //         }
        //         else
        //         {
        //             SurveyQuestion question = AccuitAdminDbContext.SurveyQuestions.SingleOrDefault(k => k.SurveyQuestionID == record.SurveyQuestionID);
        //             string strcreateddate = string.Empty;
        //             string strmodifydate = string.Empty;
        //             if (question != null)
        //             {
        //                 question.IsDeleted = true;
        //                 question.IsActive = false;
        //                 strcreateddate = Convert.ToString(question.CreatedDate);
        //                 strmodifydate = Convert.ToString(question.ModifiedDate);
        //                 AccuitAdminDbContext.Entry<SurveyQuestion>(question).State = System.Data.Entity.EntityState.Modified;
        //                 AccuitAdminDbContext.SaveChanges();

        //                 record.IsActive = true;
        //                 record.CreatedDate = string.IsNullOrEmpty(strcreateddate) ? DateTime.Now : Convert.ToDateTime(strcreateddate);
        //                 if (!string.IsNullOrEmpty(strcreateddate))
        //                     record.ModifiedDate = DateTime.Now;
        //                 record.ModifiedByUserID = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]); //added by vaishali on 23 Feb 2014
        //                 record.IsMandatory = false;
        //                 record.IsDeleted = false;
        //                 AccuitAdminDbContext.Entry<SurveyQuestion>(record).State = System.Data.Entity.EntityState.Added;
        //                 AccuitAdminDbContext.SaveChanges();
        //                 var lastquestionID = AccuitAdminDbContext.SurveyQuestions.Where(k => k.SurveyQuestionID == record.SurveyQuestionID).FirstOrDefault();
        //                 List<SurveyQuestionAttribute> answerLst = new List<SurveyQuestionAttribute>();
        //                 answerLst = resultAnswerLst.ToList();

        //                 if (answerLst.Count > 0)
        //                 {
        //                     foreach (var item in answerLst)
        //                     {
        //                         item.SurveyQuestionID = lastquestionID.SurveyQuestionID;

        //                         AccuitAdminDbContext.Entry<SurveyQuestionAttribute>(item).State = System.Data.Entity.EntityState.Modified;
        //                         AccuitAdminDbContext.SaveChanges();
        //                     }
        //                 }
        //             }

        //             else
        //             {
        //                 record.IsActive = true;
        //                 record.CreatedDate = string.IsNullOrEmpty(strcreateddate) ? DateTime.Now : Convert.ToDateTime(strcreateddate);
        //                 if (!string.IsNullOrEmpty(strcreateddate))
        //                     record.ModifiedDate = DateTime.Now;
        //                 record.CreatedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]); //added by vaishali on 23 Feb 2014
        //                 record.IsMandatory = false;
        //                 record.IsDeleted = false;
        //                 AccuitAdminDbContext.Entry<SurveyQuestion>(record).State = System.Data.Entity.EntityState.Added;
        //                 AccuitAdminDbContext.SaveChanges();
        //             }
        //             var recentCreatedQuestion = AccuitAdminDbContext.SurveyQuestions.Where(k => k.SurveyQuestionID == record.SurveyQuestionID).FirstOrDefault();
        //             if (resultAnswer.Count > 0)
        //             {
        //                 foreach (var item in resultAnswer)
        //                 {
        //                     answers.CreatedDate = System.DateTime.Now;
        //                     answers.OptionValue = item.Answer;
        //                     answers.Sequence = Convert.ToInt16(item.Sequence);
        //                     answers.SurveyQuestionID = recentCreatedQuestion.SurveyQuestionID;
        //                     answers.IsActive = true;
        //                     answers.CreatedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]); //added by vaishali on 23 Feb 2014
        //                     AccuitAdminDbContext.Entry<SurveyQuestionAttribute>(answers).State = System.Data.Entity.EntityState.Added;
        //                     AccuitAdminDbContext.SaveChanges();
        //                 }
        //             }
        //             isSuccess = true;
        //         }
        //         return isSuccess;
        //     }


        //     /// <summary>   
        //     /// Function to Add the Category 
        //     /// </summary>
        //     /// <param name="SurveyQuestion">FeedbackCategoryMaster</param>
        //     /// <returns>0 or 1</returns>
        //     public bool IsSuccessCategoryInsert(FeedbackCategoryMaster response)
        //     {
        //         try
        //         {
        //             FeedbackCategoryMaster feedbackCategoryMaster = new FeedbackCategoryMaster();

        //             feedbackCategoryMaster.FeedbackCategoryName = response.FeedbackCategoryName;
        //             feedbackCategoryMaster.TeamID = response.TeamID;
        //             AccuitAdminDbContext.Entry<FeedbackCategoryMaster>(response).State = System.Data.Entity.EntityState.Added;
        //             AccuitAdminDbContext.SaveChanges();

        //             return true;
        //         }
        //         catch (Exception)
        //         {
        //             return false;
        //         }
        //     }

        //     /// <summary>
        //     /// Function to Get the Questions Details base on QuestionID.
        //     /// </summary>
        //     /// <param name="questionID">questionID</param>
        //     /// <returns>SurveyQuestion class</returns>
        //     public SurveyQuestion QuestionDetails(int questionID)
        //     {
        //         SurveyQuestion result = AccuitAdminDbContext.SurveyQuestions.SingleOrDefault(k => k.SurveyQuestionID == questionID);
        //         return result;
        //     }

        //     /// <summary>
        //     /// Function to Update the Answers Details base on AnswerID.
        //     /// </summary>
        //     /// <param name="SurveyQuestionAttribute">record</param>
        //     /// <returns>true or false</returns>
        //     public bool IsUpdatedAnswer(SurveyQuestionAttribute record)
        //     {
        //         bool isSuccess = false;
        //         if (record.SurveyOptionID.Equals(0))
        //         {
        //             record.IsActive = true;
        //             record.CreatedDate = System.DateTime.Now;
        //             record.CreatedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]); //added by vaishali on 23 Feb 2014
        //             AccuitAdminDbContext.Entry<SurveyQuestionAttribute>(record).State = System.Data.Entity.EntityState.Added;
        //             AccuitAdminDbContext.SaveChanges();
        //             isSuccess = true;
        //         }
        //         else
        //         {
        //             SurveyQuestionAttribute answer = AccuitAdminDbContext.SurveyQuestionAttributes.SingleOrDefault(k => k.SurveyOptionID == record.SurveyOptionID);
        //             bool isOptionTextChanged = false;
        //             string strcreateddate = string.Empty;
        //             string strmodifydate = string.Empty;
        //             if (answer != null)
        //             {
        //                 if (answer.OptionValue != record.OptionValue)
        //                 {
        //                     isOptionTextChanged = true;
        //                     answer.IsActive = false;
        //                 }
        //                 strcreateddate = Convert.ToString(answer.CreatedDate);
        //                 strmodifydate = Convert.ToString(answer.ModifiedDate);
        //                 answer.ModifiedDate = DateTime.Now;
        //                 answer.Sequence = record.Sequence;
        //                 answer.OptionValue = record.OptionValue;
        //                 answer.IsAffirmative = record.IsAffirmative;
        //                 AccuitAdminDbContext.Entry<SurveyQuestionAttribute>(answer).State = System.Data.Entity.EntityState.Modified;
        //                 isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //                 if (!isOptionTextChanged)
        //                 {
        //                     return isSuccess;
        //                 }

        //             }

        //             record.IsActive = true;
        //             record.CreatedDate = string.IsNullOrEmpty(strcreateddate) ? DateTime.Now : Convert.ToDateTime(strcreateddate);
        //             if (!string.IsNullOrEmpty(strcreateddate))
        //                 record.ModifiedDate = DateTime.Now;
        //             record.CreatedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]); //added by vaishali on 23 Feb 2014
        //             AccuitAdminDbContext.Entry<SurveyQuestionAttribute>(record).State = System.Data.Entity.EntityState.Added;
        //             isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //         }
        //         return isSuccess;
        //     }

        //     /// <summary>
        //     /// Function to Delete the Answers Details base on AnswerID.
        //     /// </summary>
        //     /// <param name="SurveyQuestionAttribute">record</param>
        //     /// <returns>true or false</returns>
        //     public bool IsDeleteAnswer(int ansID)
        //     {
        //         bool isSuccess = false;

        //         SurveyQuestionAttribute answer = AccuitAdminDbContext.SurveyQuestionAttributes.SingleOrDefault(k => k.SurveyOptionID == ansID);
        //         if (answer != null)
        //         {
        //             answer.IsActive = false;
        //             // AccuitAdminDbContext.SurveyQuestionAttributes.Remove(answer);
        //             AccuitAdminDbContext.Entry<SurveyQuestionAttribute>(answer).State = System.Data.Entity.EntityState.Modified;
        //         }

        //         isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //         return isSuccess;
        //     }

        //     #endregion

        //     #region Added By Vinay Kanojia: Dated 17-Dec-2013

        //     public IList<Competitor> GetCompetitorList(int productTypeID)
        //     {
        //         //var res = AccuitAdminDbContext.Database.SqlQuery<User>("sp_GetUserDetails").ToList<User>();
        //         var lstCompetitor = new List<Competitor>();
        //         lstCompetitor = AccuitAdminDbContext.Competitors.Where(s => s.ProductTypeID == productTypeID && s.IsDeleted == false).OrderBy(k => k.Sequence).ToList();
        //         return lstCompetitor;
        //     }

        //     /// <summary>
        //     /// Function to Get the Competitor Details base on CompetitorID.
        //     /// </summary>
        //     /// <param name="competitorID"></param>
        //     /// <returns>Competitor Class object</returns>
        //     public Competitor CompetitorDetails(int competitorID)
        //     {
        //         Competitor result = AccuitAdminDbContext.Competitors.SingleOrDefault(k => k.CompetitorID == competitorID);
        //         return result;
        //     }

        //     /// <summary>
        //     /// Determines whether [is successfull insert] [the specified record].
        //     /// </summary>
        //     /// <param name="record">The record.</param>
        //     /// <returns></returns>
        //     public bool IsSuccessfullInsert(Competitor record)
        //     {
        //         bool isSuccess = false;
        //         if (record.CompetitorID.Equals(0))
        //         {
        //             record.IsActive = true;
        //             record.CreatedDate = System.DateTime.Now;
        //             record.CreatedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]); //added by vaishali on 23 Feb 2014
        //             record.IsDeleted = false;
        //             AccuitAdminDbContext.Entry<Competitor>(record).State = System.Data.Entity.EntityState.Added;
        //             isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //         }
        //         else
        //         {
        //             Competitor competitor = AccuitAdminDbContext.Competitors.SingleOrDefault(k => k.CompetitorID == record.CompetitorID);
        //             if (competitor != null)
        //             {
        //                 if (competitor.Name != record.Name)
        //                 {
        //                     competitor.IsDeleted = true;
        //                     competitor.ModifiedDate = System.DateTime.Now;
        //                     AccuitAdminDbContext.Entry<Competitor>(competitor).State = System.Data.Entity.EntityState.Modified;
        //                     AccuitAdminDbContext.SaveChanges();
        //                     record.CreatedDate = competitor.CreatedDate;
        //                     record.IsActive = true;
        //                     record.CreatedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]); //added by vaishali on 23 Feb 2014
        //                     record.ModifiedDate = System.DateTime.Now;
        //                     record.IsDeleted = false;
        //                     AccuitAdminDbContext.Entry<Competitor>(record).State = System.Data.Entity.EntityState.Added;
        //                     isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //                 }
        //             }
        //             isSuccess = true;

        //         }

        //         return isSuccess;
        //     }

        //     /// <summary>
        //     /// Funtion to Delete Competitor
        //     /// </summary>
        //     /// <param name="competitorID"></param>
        //     /// <returns></returns>
        //     public bool ISDeleteCompetitor(int competitorID)
        //     {
        //         bool isSuccess = false;

        //         Competitor result = AccuitAdminDbContext.Competitors.SingleOrDefault(k => k.CompetitorID == competitorID);
        //         if (result != null)
        //         {
        //             result.IsDeleted = true;
        //             AccuitAdminDbContext.Entry<Competitor>(result).State = System.Data.Entity.EntityState.Modified;
        //         }

        //         isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //         return isSuccess;
        //     }
        //     public IList<Module> GetParentModule(int ModuleID)
        //     {
        //         var module = new List<Module>();
        //         module = AccuitAdminDbContext.Modules.Where(k => k.ModuleID == ModuleID && !k.IsDeleted && k.IsMobile == true).ToList();
        //         return module;
        //     }
        //     /// <summary>
        //     /// </summary>
        //     /// <returns></returns>
        public IList<ModuleMaster> GetModuleList(bool? isMobile)
        {

            bool? mobileorweb = isMobile == true ? false : true;
            var lstModule = new List<ModuleMaster>();
            lstModule = AccuitAdminDbContext.ModuleMasters.Where(s => s.IsDeleted == false && (isMobile == null || s.IsMobile == isMobile)).OrderBy(x => x.Name).ToList();
            return lstModule;
        }
        //     /// <summary>
        //     /// find all module for mobile for modume name
        //     /// added by amjad on 24 may2015
        //     /// </summary>
        //     /// <param name="isMobile"></param>
        //     /// <param name="strModuleName"></param>
        //     /// <returns></returns>
        public IList<ModuleMaster> GetModuleList(bool? isMobile, string strModuleName)
        {

            bool? mobileorweb = isMobile == true ? false : true;
            var lstModule = new List<ModuleMaster>();
            lstModule = AccuitAdminDbContext.ModuleMasters.Where(s => s.IsDeleted == false && s.Name.Contains(strModuleName)
                && (isMobile == null || s.IsMobile == isMobile)).OrderBy(x => x.Name).ToList();
            return lstModule;
        }
        //     /// <summary>
        //     /// Gets the apk module list.
        //     /// </summary>
        //     /// <param name="isMobile">if set to <c>true</c> [is mobile].</param>
        //     /// <returns></returns>
        //     public IList<Module> GetAPKModuleList(bool isMobile)
        //     {
        //         var lstModule = new List<Module>();
        //         lstModule = AccuitAdminDbContext.Modules.Where(s => s.IsDeleted == false && s.IsMobile == isMobile).OrderBy(x => x.Name).ToList();
        //         return lstModule;
        //     }

        //     /// <summary>
        //     /// Method To Add Module
        //     /// </summary>
        //     /// <param name="record"></param>
        //     /// <returns></returns>
        //     /// 

        //     public IList<APKMaintainance> GetApkMaintanceList()
        //     {
        //         var lstApk = new List<APKMaintainance>();
        //         lstApk = AccuitAdminDbContext.APKMaintainances.ToList();
        //         return lstApk;
        //     }
        //     #region GetAll Channel Master and Update
        //     /// <summary>
        //     /// Added by Tanuj(9-4-2014)
        //     /// </summary>
        //     /// <returns></returns>

        //     public IList<ChannelMaster> GetChannelMaster()
        //     {
        //         var ChannelList = new List<ChannelMaster>();
        //         ChannelList = AccuitAdminDbContext.ChannelMasters.ToList();
        //         return ChannelList;

        //     }

        //     public bool UpdateChannelMaster(List<int> channels)
        //     {
        //         bool isSucess = false;
        //         var AllChannelMaster = AccuitAdminDbContext.ChannelMasters.ToList();
        //         using (TransactionScope scope = new TransactionScope())
        //         {
        //             foreach (var item in AllChannelMaster)
        //             {

        //                 if (channels.Any(x => x == item.ChannelID))
        //                 {
        //                     item.IsForExclusion = true;
        //                     AccuitAdminDbContext.Entry<ChannelMaster>(item).State = System.Data.Entity.EntityState.Modified;

        //                 }
        //                 else
        //                 {
        //                     item.IsForExclusion = false;
        //                     AccuitAdminDbContext.Entry<ChannelMaster>(item).State = System.Data.Entity.EntityState.Modified;
        //                 }


        //             }
        //             AccuitAdminDbContext.SaveChanges();
        //             scope.Complete();
        //             isSucess = true;
        //         }
        //         return isSucess;
        //     }


        //     #endregion
        //     public bool IsApkInserted(APKMaintainance record)
        //     {
        //         bool isSucess = false;


        //         var existingdata = AccuitAdminDbContext.APKMaintainances.FirstOrDefault(x => x.APKID == record.APKID);
        //         if (existingdata != null)
        //         {
        //             existingdata.APKURL = record.APKURL;
        //             existingdata.APKVersion = record.APKVersion;
        //             isSucess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;

        //         }

        //         else
        //         {

        //             List<APKMaintainance> LstApk = AccuitAdminDbContext.APKMaintainances.ToList();
        //             if (record.IsLatest == true)
        //             {
        //                 LstApk = LstApk.Where(s => s.IsLatest == record.IsLatest).ToList();
        //                 foreach (var ch in LstApk)
        //                 {
        //                     ch.IsLatest = false;
        //                     AccuitAdminDbContext.Entry<APKMaintainance>(ch).State = System.Data.Entity.EntityState.Modified;
        //                     isSucess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //                 }
        //             }
        //             AccuitAdminDbContext.Entry<APKMaintainance>(record).State = System.Data.Entity.EntityState.Added;
        //             return AccuitAdminDbContext.SaveChanges() > 0 ? true : false;


        //         }
        //         return isSucess;
        //     }




        public bool IsModuleInsert(ModuleMaster record)
        {
            bool isSuccess = false;
            if (record.ModuleID.Equals(0))
            {
                if (AccuitAdminDbContext.ModuleMasters.FirstOrDefault(k => k.ModuleCode == record.ModuleCode && !k.IsDeleted) == null)
                {
                    record.CreatedDate = System.DateTime.Now;
                    record.CreatedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]);
                    record.IsDeleted = false;
                    AccuitAdminDbContext.Entry<ModuleMaster>(record).State = System.Data.Entity.EntityState.Added;
                    isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
                }
            }
            else
            {
                var module = AccuitAdminDbContext.ModuleMasters.FirstOrDefault(k => k.ModuleID == record.ModuleID && !k.IsDeleted);
                if (module != null)
                {
                    module.ModifiedBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]); //added by vaishali on 23 Feb 2014
                    module.ModifiedDate = System.DateTime.Now;
                    module.Name = record.Name;
                    module.ParentModuleID = record.ParentModuleID;
                    module.Sequence = record.Sequence;
                    module.IsMobile = record.IsMobile;
                    //added by amjad on 24 may2015
                    module.IsMandatory = record.IsMandatory;
                    module.Icon = record.Icon;
                    module.IsSystemModule = record.IsSystemModule;
                    module.IsStoreWise = record.IsStoreWise;
                    module.ModuleCode = record.ModuleCode;
                    // Added by Navneet
                    module.ModuleDescription = record.ModuleDescription;
                    module.ModuleType = record.ModuleType;

                    module.PageURL = record.PageURL;

                    AccuitAdminDbContext.Entry<ModuleMaster>(module).State = System.Data.Entity.EntityState.Modified;
                    isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
                }
                else
                {
                    isSuccess = false;
                }
            }

            return isSuccess;
        }

        //     /// <summary>
        //     /// Method to get detail on the basis of module id 
        //     /// </summary>
        //     /// <param name="moduleID"></param>
        //     /// <returns>object of Module class </returns>
        //     public Module ModuleDetail(int moduleID)
        //     {
        //         Module result = AccuitAdminDbContext.Modules.SingleOrDefault(k => k.ModuleID == moduleID);
        //         return result;
        //     }

        //     public APKMaintainance ApkDetail(int apkid)
        //     {
        //         APKMaintainance result = AccuitAdminDbContext.APKMaintainances.SingleOrDefault(k => k.APKID == apkid);
        //         return result;
        //     }

        //     /// <summary>
        //     /// Cnannel Master Exclusion(Added By Tanuj(9-4-2014))
        //     /// </summary>
        //     /// <param name="channels">channels</param>
        //     /// <returns>bool</returns>

        //     public bool UpdateReportProfile(List<int> profiles, List<int> ISEffective)
        //     {
        //         bool isSuccess = false;
        //         var allRoles = AccuitAdminDbContext.RoleMasters.ToList();
        //         using (TransactionScope scope = new TransactionScope())
        //         {
        //             foreach (var item in allRoles)
        //             {
        //                 if (profiles.Any(x => x == item.RoleID))
        //                 {
        //                     item.IsReportProfile = true;

        //                     AccuitAdminDbContext.Entry<RoleMaster>(item).State = System.Data.Entity.EntityState.Modified;
        //                 }
        //                 else
        //                 {
        //                     item.IsReportProfile = false;

        //                     AccuitAdminDbContext.Entry<RoleMaster>(item).State = System.Data.Entity.EntityState.Modified;
        //                 }
        //                 if (ISEffective.Any(x => x == item.RoleID))
        //                 {
        //                     item.IsEffectiveProfile = true;

        //                     AccuitAdminDbContext.Entry<RoleMaster>(item).State = System.Data.Entity.EntityState.Modified;
        //                 }
        //                 else
        //                 {
        //                     item.IsEffectiveProfile = false;

        //                     AccuitAdminDbContext.Entry<RoleMaster>(item).State = System.Data.Entity.EntityState.Modified;
        //                 }
        //             }
        //             AccuitAdminDbContext.SaveChanges();
        //             scope.Complete();
        //             isSuccess = true;
        //         }
        //         return isSuccess;
        //     }


        //     public bool UpdateReportProfile(List<RoleMaster> lstRoleMaster)
        //     {
        //         bool isSuccess = false;

        //         var allRoles = AccuitAdminDbContext.RoleMasters.ToList();
        //         foreach (var item in lstRoleMaster)
        //         {
        //             var role = allRoles.FirstOrDefault(k => k.RoleID == item.RoleID);
        //             role.IsEffectiveProfile = item.IsEffectiveProfile;
        //             role.IsReportProfile = item.IsReportProfile;
        //             role.IsGeoTagMandate = item.IsGeoTagMandate;
        //             role.IsGeoPhotoMandate = item.IsGeoPhotoMandate;
        //             role.IsStoreProfileVisible = item.IsStoreProfileVisible;
        //             AccuitAdminDbContext.Entry<RoleMaster>(role).State = System.Data.Entity.EntityState.Modified;
        //         }

        //         isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;

        //         return isSuccess;
        //     }
        //     public bool UpdateReportProfileForRoleId(RoleMaster lstRoleMaster)
        //     {
        //         bool isSuccess = false;
        //         RoleMaster objRoles = AccuitAdminDbContext.RoleMasters.Where(a => a.RoleID == lstRoleMaster.RoleID).FirstOrDefault();
        //         objRoles.IsEffectiveProfile = lstRoleMaster.IsEffectiveProfile;
        //         objRoles.IsReportProfile = lstRoleMaster.IsReportProfile;
        //         objRoles.IsGeoTagMandate = lstRoleMaster.IsGeoTagMandate;
        //         objRoles.IsGeoPhotoMandate = lstRoleMaster.IsGeoPhotoMandate;
        //         objRoles.IsStoreProfileVisible = lstRoleMaster.IsStoreProfileVisible;
        //         objRoles.IsAttendanceMandate = lstRoleMaster.IsAttendanceMandate; //SDCE-4401
        //         objRoles.IsGeoFencingApplicable = lstRoleMaster.IsGeoFencingApplicable; //SDCE-4452
        //         #region--added by amjad on 24 may2015--
        //         objRoles.IsOfflineAccess = lstRoleMaster.IsOfflineAccess;
        //         objRoles.ShowPerformanceTab = lstRoleMaster.ShowPerformanceTab;
        //         objRoles.IsRaceProfile = lstRoleMaster.IsRaceProfile;
        //         #endregion------------------------------
        //         AccuitAdminDbContext.Entry<RoleMaster>(objRoles).State = System.Data.Entity.EntityState.Modified;
        //         isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //         return isSuccess;
        //     }

        //     /// <summary>
        //     /// Method to delelte a module on basis on module Id
        //     /// </summary>
        //     /// <param name="moduleID"></param>
        //     /// <returns></returns>
        //     public bool DeleteModules(List<int> modules)
        //     {
        //         bool isSuccess = false;
        //         using (TransactionScope scope = new TransactionScope())
        //         {
        //             foreach (int id in modules)
        //             {
        //                 Module module = AccuitAdminDbContext.Modules.FirstOrDefault(k => k.ModuleID == id);
        //                 if (module != null)
        //                 {
        //                     module.ModifiedDate = System.DateTime.Now;
        //                     module.IsDeleted = true;
        //                     AccuitAdminDbContext.Entry<Module>(module).State = System.Data.Entity.EntityState.Modified;
        //                 }
        //             }
        //             AccuitAdminDbContext.SaveChanges();
        //             scope.Complete();
        //             isSuccess = true;
        //         }
        //         return isSuccess;
        //     }

        //     #endregion

        //     /// <summary>
        //     /// Method to get image name of uploaded image for an entity
        //     /// </summary>
        //     /// <param name="entityID">entity ID</param>
        //     /// <param name="imageType">image type</param>
        //     /// <returns>returns file name</returns>
        //     public string GetEntityImageName(string entityID, AspectEnums.ImageFileTypes imageType)
        //     {
        //         string fileName = string.Empty;
        //         switch (imageType)
        //         {
        //             case AspectEnums.ImageFileTypes.Store:
        //                 int storeID = Convert.ToInt32(entityID);
        //                 var store = AccuitAdminDbContext.StoreMasters.FirstOrDefault(k => k.StoreID == storeID);
        //                 if (store != null)
        //                     fileName = store.PictureFileName;
        //                 break;
        //             case AspectEnums.ImageFileTypes.User:
        //                 long userID = Convert.ToInt64(entityID);
        //                 var user = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == userID);
        //                 if (user != null)
        //                     fileName = user.ProfilePictureFileName;
        //                 break;
        //             case AspectEnums.ImageFileTypes.Survey:
        //                 long questionID = Convert.ToInt64(entityID);
        //                 var question = AccuitAdminDbContext.SurveyQuestions.FirstOrDefault(k => k.SurveyQuestionID == questionID);
        //                 if (question != null)
        //                     fileName = question.HintImageName;
        //                 break;
        //         }
        //         return fileName;
        //     }

        //     /// <summary>
        //     /// Method to image name of uploaded image for an entity
        //     /// </summary>
        //     /// <param name="entityID">entity ID</param>
        //     ///<param name="fileName">file name to update</param>
        //     ///<param name="fileType">file type</param>
        //     /// <returns>returns file name</returns>
        //     public bool UpdateEntityImageName(string entityID, string fileName, AspectEnums.ImageFileTypes fileType)
        //     {
        //         bool isUpdated = false;
        //         switch (fileType)
        //         {
        //             case AspectEnums.ImageFileTypes.Store:
        //                 int storeID = Convert.ToInt32(entityID);
        //                 var store = AccuitAdminDbContext.StoreMasters.FirstOrDefault(k => k.StoreID == storeID);
        //                 store.PictureFileName = fileName;
        //                 store.ModifiedDate = System.DateTime.Now;
        //                 AccuitAdminDbContext.Entry<StoreMaster>(store).State = System.Data.Entity.EntityState.Modified;
        //                 break;
        //             case AspectEnums.ImageFileTypes.User:
        //                 long userID = Convert.ToInt64(entityID);
        //                 var user = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == userID);
        //                 user.ProfilePictureFileName = fileName;
        //                 user.ModifiedDate = System.DateTime.Now;
        //                 AccuitAdminDbContext.Entry<UserMaster>(user).State = System.Data.Entity.EntityState.Modified;
        //                 break;
        //             case AspectEnums.ImageFileTypes.Survey:
        //                 long questionID = Convert.ToInt64(entityID);
        //                 var question = AccuitAdminDbContext.SurveyQuestions.FirstOrDefault(k => k.SurveyQuestionID == questionID);
        //                 question.HintImageName = fileName;
        //                 question.ModifiedDate = System.DateTime.Now;
        //                 AccuitAdminDbContext.Entry<SurveyQuestion>(question).State = System.Data.Entity.EntityState.Modified;
        //                 break;
        //         }
        //         isUpdated = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //         return isUpdated;
        //     }

        //     /// <summary>
        //     /// Method to fetch prouduct list
        //     /// </summary>
        //     /// <param name="companyID">company ID</param>
        //     /// <returns>returns product list</returns>
        //     public IList<ProductMaster> GetProductList(int companyID)
        //     {
        //         return AccuitAdminDbContext.ProductMasters.Where(k => k.CompanyID == companyID).ToList();
        //     }

        //     /// <summary>
        //     /// Method to get the pending entities which are not synced
        //     /// </summary>
        //     /// <param name="companyID">company primary ID</param>
        //     /// <param name="userID">user primary ID</param>
        //     /// <returns>returns pending item list</returns>
        //     public IList<SyncTable> GetPendingSyncEntities(int companyID, long userID)
        //     {
        //         DateTime currentDate = System.DateTime.Now;
        //         List<SyncTable> lastSyncedEntities = AccuitAdminDbContext.SyncTables.Where(k => k.CompanyID == companyID && k.SyncDate <= currentDate).ToList();
        //         List<SyncTable> pendingSynEntities = new List<SyncTable>();
        //         foreach (SyncTable entity in lastSyncedEntities)
        //         {
        //             if (AccuitAdminDbContext.UserTableSyncHistories.FirstOrDefault(k => k.LastSyncDate < currentDate && k.LastSyncDate > entity.SyncDate) == null)
        //             {
        //                 pendingSynEntities.Add(entity);
        //             }
        //         }
        //         return pendingSynEntities;
        //     }

        //     /// <summary>
        //     /// Method to update user table sysnc history
        //     /// </summary>
        //     /// <param name="syncTableID">sync table ID</param>
        //     /// <param name="userID">user ID</param>
        //     /// <returns>returns boolean status</returns>
        //     public bool UpdateSyncEntity(int syncTableID, long userID)
        //     {
        //         var syncEntity = AccuitAdminDbContext.UserTableSyncHistories.FirstOrDefault(k => k.UserID == userID && k.SyncTableID == syncTableID);
        //         if (syncEntity == null)
        //         {
        //             syncEntity = new UserTableSyncHistory()
        //             {
        //                 UserID = userID,
        //                 SyncTableID = syncTableID,
        //                 CreatedBy = userID,
        //                 CreatedDate = System.DateTime.Now,
        //                 IsDeleted = false,
        //                 UserSyncHistoryID = 0,
        //                 LastSyncDate = System.DateTime.Now
        //             };
        //             AccuitAdminDbContext.UserTableSyncHistories.Add(syncEntity);
        //         }
        //         else
        //         {
        //             syncEntity.ModifiedBy = userID;
        //             syncEntity.ModifiedDate = System.DateTime.Now;
        //             syncEntity.LastSyncDate = System.DateTime.Now;
        //             AccuitAdminDbContext.Entry<UserTableSyncHistory>(syncEntity).State = System.Data.Entity.EntityState.Modified;
        //         }
        //         return AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //     }


        //     /// <summary>
        //     /// Method to get product repository on the basis of company
        //     /// </summary>
        //     /// <param name="companyID">company primary id value</param>
        //     /// <returns>returns product collection</returns>
        //     public IList<vwProductRepository> GetProductRepository(int companyID, long? userID)
        //     {
        //         IList<vwProductRepository> result = new List<vwProductRepository>();
        //         if (AccuitAdminDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.Product)).FirstOrDefault() == true)
        //         {
        //             result = AccuitAdminDbContext.vwProductRepositories.Where(k => k.CompanyID == companyID).Take(5).ToList();
        //         }
        //         return result;
        //     }


        //     /// <summary>
        //     /// Method to get product repository on the basis of company FOr APK
        //     /// </summary>
        //     /// <param name="companyID">company primary id value</param>
        //     /// <returns>returns product collection</returns>
        //     public IList<vwProductRepository> GetProductRepository(int companyID, long? userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        //     {
        //         HasMoreRows = false;
        //         MaxModifiedDate = LastUpdatedDate;
        //         DateTime CurrentDateTime = System.DateTime.Now;

        //         IList<vwProductRepository> result = new List<vwProductRepository>();
        //         if (AccuitAdminDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.Product)).FirstOrDefault() == true)
        //         {
        //             // result = AccuitAdminDbContext.vwProductRepositories.Where(k => k.CompanyID == companyID).Take(5).ToList();


        //             if (LastUpdatedDate == null)
        //             {
        //                 result = AccuitAdminDbContext.vwProductRepositories.Where(k => k.CompanyID == companyID && !k.IsDeleted) /*delete flag is there for APK user TBD*/
        //                    .OrderBy(k => k.ModifiedDate)
        //                    .Skip(StartRowIndex)
        //                    .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       

        //             }
        //             else
        //             {
        //                 result = AccuitAdminDbContext.vwProductRepositories.Where(k => k.CompanyID == companyID  /*delete flag is there for APK user TBD*/
        //                      &&
        //                         (
        //                         (k.ModifiedDate > LastUpdatedDate)
        //                         ||
        //                         (k.ModifiedDate == LastUpdatedDate.Value)
        //                         )
        //                         )
        //                    .OrderBy(k => k.ModifiedDate)
        //                    .Skip(StartRowIndex)
        //                    .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                                        
        //             }

        //             HasMoreRows = result.Count > RowCount ? true : false;
        //             result = result.Take(RowCount).ToList();

        //             // Update last modified data among the data if available, else send the same modifieddate back  
        //             if (result.Count > 0)
        //             {
        //                 if (LastUpdatedDate == null && HasMoreRows == true)
        //                     MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
        //                 else if (LastUpdatedDate == null && HasMoreRows == false)
        //                     MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
        //                 else
        //                     MaxModifiedDate = result.Max(x => x.ModifiedDate);

        //             }
        //         }

        //         return result;

        //     }

        //     /// <summary>
        //     /// Method to get product group basis on product type
        //     /// </summary>
        //     /// <param name="productTypeID"></param>
        //     /// <returns>return list contain product group</returns>
        //     public IList<vwProductRepository> GetProductGroup(int productTypeID)
        //     {
        //         var productGroup = new List<vwProductRepository>();
        //         var tmpLst = new List<vwProductRepository>();
        //         tmpLst = AccuitAdminDbContext.vwProductRepositories.Where(s => s.ProductTypeID == productTypeID).ToList();

        //         foreach (var l in tmpLst)
        //         {
        //             if (productGroup.Count > 0)
        //             {
        //                 var tmp = productGroup.FindAll(s => s.ProductGroupCode == l.ProductGroupCode).SingleOrDefault();
        //                 if (tmp == null)
        //                     productGroup.Add(l);
        //             }
        //             else
        //                 productGroup.Add(l);
        //         }
        //         return productGroup;
        //     }

        //     /// <summary>
        //     /// Method to Get Product Type Added By Vinay Kanojia
        //     /// </summary>
        //     /// <param name="companyID"> On Basis of Company</param>
        //     /// <returns>List of the Product Type</returns>
        //     public IList<vwProductRepository> GetProductType(int companyID)
        //     {
        //         var productType = new List<vwProductRepository>();
        //         var tmpLst = new List<vwProductRepository>();
        //         tmpLst = AccuitAdminDbContext.vwProductRepositories.Where(s => s.CompanyID == companyID).OrderBy(o => o.ProductTypeCode).ToList();

        //         foreach (var l in tmpLst)
        //         {
        //             if (productType.Count > 0)
        //             {
        //                 var tmp = productType.FindAll(s => s.ProductTypeCode == l.ProductTypeCode).SingleOrDefault();
        //                 if (tmp == null)
        //                     productType.Add(l);
        //             }
        //             else
        //                 productType.Add(l);
        //         }
        //         return productType;
        //     }

        //     /// <summary>
        //     /// Method to get payment mode list on the basis of company
        //     /// </summary>
        //     /// <param name="companyID">company ID</param>
        //     /// <returns></returns>
        //     public IList<PaymentMode> GetPaymentModes(int companyID, long? userID)
        //     {
        //         IList<PaymentMode> result = new List<PaymentMode>();
        //         if (AccuitAdminDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.PaymentMode)).FirstOrDefault() == true)
        //         {
        //             result = AccuitAdminDbContext.PaymentModes.Where(k => k.CompanyID == companyID).ToList();
        //         }
        //         return result;
        //     }

        //     /// <summary>
        //     /// Method to get payment mode list on the basis of company for APK
        //     /// </summary>
        //     /// <param name="companyID">company ID</param>
        //     /// <returns></returns>
        //     public IList<PaymentMode> GetPaymentModes(int companyID, long? userID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        //     {
        //         HasMoreRows = false;
        //         MaxModifiedDate = LastUpdatedDate;
        //         DateTime CurrentDateTime = System.DateTime.Now;

        //         IList<PaymentMode> result = new List<PaymentMode>();
        //         if (AccuitAdminDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.PaymentMode)).FirstOrDefault() == true)
        //         {
        //             //result = AccuitAdminDbContext.PaymentModes.Where(k => k.CompanyID == companyID).ToList();

        //             if (LastUpdatedDate == null)
        //             {
        //                 result = AccuitAdminDbContext.PaymentModes.Where(k => k.CompanyID == companyID && !k.IsDeleted) /*delete flag is there for APK user TBD*/
        //                    .OrderBy(k => k.CreatedDate)
        //                    .Skip(StartRowIndex)
        //                    .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       

        //             }
        //             else
        //             {
        //                 result = AccuitAdminDbContext.PaymentModes.Where(k => k.CompanyID == companyID  /*delete flag is there for APK user TBD*/
        //                      &&
        //                         (
        //                         (k.CreatedDate > LastUpdatedDate)
        //                         ||
        //                         (k.CreatedDate == LastUpdatedDate.Value)
        //                         )
        //                         )
        //                    .OrderBy(k => k.CreatedDate)
        //                    .Skip(StartRowIndex)
        //                    .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                                        
        //             }

        //             HasMoreRows = result.Count > RowCount ? true : false;
        //             result = result.Take(RowCount).ToList();

        //             // Update last modified data among the data if available, else send the same modifieddate back  
        //             if (result.Count > 0)
        //             {
        //                 if (LastUpdatedDate == null && HasMoreRows == true)
        //                     MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
        //                 else if (LastUpdatedDate == null && HasMoreRows == false)
        //                     MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
        //                 else
        //                     MaxModifiedDate = result.Max(x => x.CreatedDate);

        //             }

        //         }
        //         return result;
        //     }

        //     /// <summary>
        //     /// Method to get role module questions
        //     /// </summary>
        //     /// <param name="roleID">role id</param>
        //     /// <param name="moduleID">module id</param>
        //     /// <returns>rteurns list of questions</returns>
        //     public IList<RoleModuleQuestion> GetRoleModuleQuestions(int roleID, int moduleID)
        //     {
        //         var questions = AccuitAdminDbContext.SurveyQuestions.Where(k => k.ModuleID == moduleID && !k.IsDeleted).ToList();
        //         var roleQuestions = AccuitAdminDbContext.vwRoleModuleQuestions.Where(k => k.RoleID == roleID);
        //         List<RoleModuleQuestion> surveyQuestions = new List<RoleModuleQuestion>();
        //         foreach (var item in questions)
        //         {
        //             RoleModuleQuestion question = new RoleModuleQuestion()
        //             {
        //                 IsMandatory = item.IsMandatory,
        //                 ModuleID = item.ModuleID,
        //                 Question = item.Question,
        //                 SurveyQuestionID = item.SurveyQuestionID
        //             };
        //             var roleQuestion = roleQuestions.FirstOrDefault(k => k.RoleID == roleID && k.SurveyQuestionID == item.SurveyQuestionID);
        //             question.IsSelected = roleQuestion != null;
        //             if (question.IsSelected)
        //             {
        //                 question.RecurrenceExpression = roleQuestion.RecurrenceExpression;
        //                 question.RoleID = roleQuestion.RoleID;

        //             }
        //             surveyQuestions.Add(question);
        //         }
        //         return surveyQuestions;
        //     }

        //     /// <summary>
        //     /// Method to submit role questions
        //     /// </summary>
        //     /// <param name="questions">question list</param>
        //     /// <param name="userID">user ID</param>
        //     /// <returns>returns boolean response</returns>
        //     public bool SubmitRoleSurveyQuestions(IList<RoleModuleQuestion> questions, long userID)
        //     {
        //         bool isSuccess = false;
        //         using (TransactionScope scope = new TransactionScope())
        //         {
        //             foreach (RoleModuleQuestion question in questions)
        //             {
        //                 RoleSurveyQuestion survey = AccuitAdminDbContext.RoleSurveyQuestions.FirstOrDefault(k => k.RoleID == question.RoleID && k.SurveyQuestionID == question.SurveyQuestionID);
        //                 if (survey != null)
        //                 {
        //                     if (!question.IsSelected)
        //                     {
        //                         survey.IsDeleted = true;
        //                         survey.ModifiedDate = System.DateTime.Now;
        //                         survey.ModifiedBy = userID;
        //                         AccuitAdminDbContext.Entry<RoleSurveyQuestion>(survey).State = System.Data.Entity.EntityState.Modified;
        //                         AccuitAdminDbContext.SaveChanges();
        //                     }
        //                 }
        //                 else
        //                 {
        //                     survey = new RoleSurveyQuestion();
        //                     survey.CreatedBy = userID;
        //                     survey.CreatedDate = System.DateTime.Now;
        //                     survey.IsMandatory = question.IsMandatory.HasValue ? question.IsMandatory.Value : false;
        //                     survey.RecurrenceExpression = question.RecurrenceExpression;
        //                     survey.RoleID = question.RoleID.Value;
        //                     survey.SurveyQuestionID = question.SurveyQuestionID;
        //                     survey.IsDeleted = !question.IsSelected;
        //                     AccuitAdminDbContext.RoleSurveyQuestions.Add(survey);
        //                     AccuitAdminDbContext.SaveChanges();
        //                 }
        //             }
        //             isSuccess = true;
        //             scope.Complete();
        //         }
        //         return isSuccess;
        //     }

        //     /// <summary>
        //     /// Method to get product details
        //     /// </summary>
        //     /// <param name="productID">product ID</param>
        //     /// <returns>returns product details</returns>
        //     public ProductMaster GetProductDetails(int productID)
        //     {
        //         return AccuitAdminDbContext.ProductMasters.FirstOrDefault(k => k.ProductID == productID);
        //     }

        //     /// <summary>
        //     /// Method to get email smtp server details
        //     /// </summary>
        //     /// <returns>returns smtp server entity instance</returns>
        //     public SMTPServer GetEmailServerDetails()
        //     {
        //         return AccuitAdminDbContext.SMTPServers.FirstOrDefault();
        //     }

        //     /// <summary>
        //     /// Method to update email service status
        //     /// </summary>
        //     /// <param name="emailServiceID">email service ID</param>
        //     /// <param name="status">status to update</param>
        //     /// <param name="remarks">remarks</param>
        //     /// <returns>boolean response</returns>
        //     public bool UpdateEmailServiceStatus(long emailServiceID, int status, string remarks)
        //     {
        //         bool isUpdated = false;
        //         EmailService email = AccuitAdminDbContext.EmailServices.FirstOrDefault(k => k.EmailServiceID == emailServiceID);
        //         if (email != null)
        //         {
        //             email.Status = status;
        //             email.ModifiedDate = System.DateTime.Now;
        //             email.Remarks = remarks;
        //             AccuitAdminDbContext.Entry<EmailService>(email).State = System.Data.Entity.EntityState.Modified;
        //             isUpdated = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //         }
        //         return isUpdated;
        //     }

        //     /// <summary>
        //     /// Insert email entry into database
        //     /// </summary>
        //     /// <param name="email">email</param>
        //     /// <returns>returns response</returns>
        //     public bool InsertEmailRecord(EmailService email)
        //     {
        //         bool isSuccess = false;
        //         email.CreatedDate = System.DateTime.Now;
        //         AccuitAdminDbContext.EmailServices.Add(email);
        //         isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //         return isSuccess;
        //     }

        //     /// <summary>
        //     /// Method to save announcement text into database
        //     /// </summary>
        //     /// <param name="startDate">start date</param>
        //     /// <param name="endDate">end date</param>
        //     /// <param name="announcement">announcement text to save</param>
        //     /// <returns>returns int status</returns>
        //     public int SaveAnnouncements(DateTime startDate, DateTime? endDate, string webAnnouncement, string mobileAnnouncement, int RoleID)
        //     {
        //         int status = 0;
        //         DateTime date = startDate.Date;
        //         var content = AccuitAdminDbContext.Announcements.FirstOrDefault(k => k.RoleID == RoleID && !k.IsDeleted);
        //         if (content == null)
        //         {
        //             Announcement banner = new Announcement()
        //             {
        //                 BannerContent = webAnnouncement,
        //                 BannerContentMobile = mobileAnnouncement,
        //                 StartDate = startDate,
        //                 EndDate = endDate,
        //                 IsActive = true,
        //                 IsDeleted = false,
        //                 RoleID = RoleID,
        //             };
        //             AccuitAdminDbContext.Announcements.Add(banner);
        //         }
        //         else
        //         {
        //             content.BannerContent = webAnnouncement;
        //             content.BannerContentMobile = mobileAnnouncement;
        //             content.StartDate = startDate;
        //             content.EndDate = endDate;
        //             AccuitAdminDbContext.Entry<Announcement>(content).State = EntityState.Modified;
        //         }
        //         AccuitAdminDbContext.SaveChanges();
        //         status = 1;
        //         return status;
        //     }

        //     /// <summary>
        //     /// Method to get announcements from date provided
        //     /// </summary>
        //     /// <param name="currentDate">current date</param>
        //     /// <returns>returns text</returns>
        //     public List<Announcement> GetAnnouncement(DateTime currentDate, int? RoleID)
        //     {
        //         //    return AccuitAdminDbContext.Announcements.Where(k => EntityFunctions.TruncateTime(k.StartDate) >= currentDate && !k.IsDeleted && k.IsActive).ToList();
        //         List<Announcement> result = new List<Announcement>();
        //         result = AccuitAdminDbContext.Announcements.Where(k => !k.IsDeleted && k.IsActive && k.RoleID == RoleID).OrderByDescending(x => x.StartDate).ToList();
        //         if (result.Count == 0)
        //             result = AccuitAdminDbContext.Announcements.Where(k => !k.IsDeleted && k.IsActive && k.RoleID == null).OrderByDescending(x => x.StartDate).Take(1).ToList();//Take 1 added in order to avoid traversal of un-neccessary data to UI layer #SDCE-3505
        //         return result;
        //     }
        //     /// <summary>
        //     /// Method to get announcements from date provided
        //     /// </summary>
        //     /// <param name="currentDate">current date</param>
        //     /// <returns>returns text</returns>
        //     public APKMaintainance IsApkVersionUpdated(string apkVersion)
        //     {
        //         return AccuitAdminDbContext.APKMaintainances.FirstOrDefault(x => x.IsLatest);
        //     }

        //     /// <summary>
        //     /// Method to update notifications
        //     /// </summary>
        //     /// <param name="service">service</param>
        //     /// <returns>returns boolean status</returns>
        //     public bool UpdateNotification(long notificationServiceID, string remarks, short status)
        //     {
        //         NotificationService notification = AccuitAdminDbContext.NotificationServices.FirstOrDefault(k => k.NotificationServiceID == notificationServiceID);
        //         if (notification != null)
        //         {
        //             notification.Remarks = remarks;
        //             notification.DeliveryStatus = (byte)status;
        //             notification.ModifiedDate = System.DateTime.Now;
        //             AccuitAdminDbContext.Entry<NotificationService>(notification).State = EntityState.Modified;
        //             return AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //         }
        //         return false;
        //     }


        //     /// <summary>
        //     /// Method to save notifications text into database
        //     /// </summary>
        //     /// <param name="notificationString"></param>
        //     /// <param name="regionGeoDefId"></param>
        //     /// <param name="profileId"></param>
        //     /// <returns></returns>
        //     public int SaveNotification(string notificationString, int regionGeoDefId, int profileId, long userId, DateTime startDate, DateTime endDate, int frequency, string NotificationSubject)
        //     {
        //         int recordsAffected = 0;
        //         using (TransactionScope scope = new TransactionScope())
        //         {
        //             NotificationMaster notificationMaster = new NotificationMaster();
        //             notificationMaster.CompanyID = 1;
        //             notificationMaster.Content = System.Web.HttpUtility.HtmlEncode(notificationString).Replace("\r\n", "<br>").Replace(" ", "&nbsp;");
        //             notificationMaster.CreatedDate = DateTime.Now;
        //             notificationMaster.CreatedBy = userId;
        //             notificationMaster.RegionGeoDefID = null;
        //             notificationMaster.RoleID = profileId;
        //             notificationMaster.StartDate = startDate;
        //             notificationMaster.EndDate = endDate;
        //             notificationMaster.Frequency = Convert.ToByte(frequency);
        //             notificationMaster.Subject = NotificationSubject;
        //             AccuitAdminDbContext.Entry<NotificationMaster>(notificationMaster).State = EntityState.Added;
        //             recordsAffected = AccuitAdminDbContext.SaveChanges();
        //             AccuitAdminDbContext.SPInsertNotificationDataBasedOnFrequency(startDate, endDate, notificationMaster.NotificationID, profileId, NotificationSubject + "~" + notificationString, frequency);
        //             scope.Complete();
        //         }
        //         return recordsAffected;
        //     }

        //     /// <summary>
        //     /// Method to update push notification service response
        //     /// </summary>
        //     /// <param name="notificationServiceID">notification service ID</param>
        //     /// <param name="response">response</param>
        //     /// <param name="isSuccess">is success</param>
        //     /// <returns>returns boolean response</returns>
        //     public bool UpdateNotificationServiceResponse(long notificationServiceID, string response, bool isSuccess)
        //     {
        //         using (var context = new SmartDostEntities())
        //         {
        //             NotificationService service = context.NotificationServices.FirstOrDefault(k => k.NotificationServiceID == notificationServiceID);
        //             if (service != null)
        //             {
        //                 if (isSuccess)
        //                 {
        //                     context.NotificationServices.Remove(service);
        //                     return context.SaveChanges() > 0 ? true : false;
        //                 }
        //                 else
        //                 {
        //                     service.Remarks = response;
        //                     service.ModifiedDate = System.DateTime.Now;
        //                     service.DeliveryStatus = 4;
        //                     context.Entry<NotificationService>(service).State = EntityState.Modified;
        //                     return context.SaveChanges() > 0 ? true : false;
        //                 }
        //             }
        //         }
        //         return false;
        //     }

        //     /// <summary>
        //     /// Method to update push notification service response
        //     /// </summary>
        //     /// <param name="notificationServiceID">notification service ID</param>
        //     /// <param name="response">response</param>
        //     /// <param name="isSuccess">is success</param>
        //     /// <returns>returns boolean response</returns>
        //     //VC20140826
        //     public bool UpdateNotificationServiceResponse(List<long> NotificationServiceID, string response, long notificationID, DateTime datetime)
        //     {
        //         var allServices = AccuitAdminDbContext.NotificationServices.Where(a => NotificationServiceID.Contains(a.NotificationServiceID)).ToList();
        //         foreach (var item in allServices)
        //         {
        //             AccuitAdminDbContext.NotificationServices.Remove(item);
        //         }
        //         return AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //     }
        //     //VC20140826

        //     /// <summary>
        //     /// Method to discard notification service
        //     /// </summary>
        //     /// <param name="notificationServiceID">notification service</param>
        //     /// <returns>returns notification service</returns>
        //     public bool DiscardNotificationService(long notificationServiceID)
        //     {
        //         using (var context = new SmartDostEntities())
        //         {
        //             NotificationService service = context.NotificationServices.FirstOrDefault(k => k.NotificationServiceID == notificationServiceID);
        //             if (service != null)
        //             {
        //                 service.DeliveryStatus = 0;
        //                 service.ModifiedDate = System.DateTime.Now;
        //                 context.Entry<NotificationService>(service).State = EntityState.Modified;
        //                 return context.SaveChanges() > 0 ? true : false;
        //             }
        //         }
        //         return false;
        //     }


        //     /// <summary>
        //     /// Method to add data into notification service table
        //     /// </summary>
        //     /// <param name="notificationID">notification ID</param>
        //     /// <param name="profileID">profile ID</param>
        //     private void AddNotificationServiceRecords(long notificationID, int profileID, DateTime startDate, DateTime endDate, int frequenceType, string message)
        //     {

        //         List<long> userIds = AccuitAdminDbContext.UserRoles.Where(k => k.RoleID == profileID && !k.IsDeleted).Select(k => k.UserID).Distinct().ToList();
        //         foreach (long id in userIds)
        //         {
        //             UserMaster userDetails = AccuitAdminDbContext.UserMasters.FirstOrDefault(k => k.UserID == id);
        //             if (userDetails != null && !String.IsNullOrEmpty(userDetails.AndroidRegistrationId))
        //             {
        //                 List<DateTime> dates = GetNotificationDateRange(startDate, endDate, frequenceType);
        //                 foreach (DateTime date in dates)
        //                 {
        //                     NotificationService notification = new NotificationService()
        //                     {
        //                         AndroidID = userDetails.AndroidRegistrationId,
        //                         NotificationDate = date,
        //                         NotificationID = notificationID,
        //                         UserID = id,
        //                         CreatedDate = System.DateTime.Now,
        //                         DeliveryStatus = 0,
        //                         PushNotificationMessage = message,
        //                     };
        //                     AccuitAdminDbContext.NotificationServices.Add(notification);
        //                     AccuitAdminDbContext.SaveChanges();
        //                 }

        //             }
        //         }

        //     }

        //     /// <summary>
        //     /// Method to get notification date ranges on the basis of frequency selected
        //     /// </summary>
        //     /// <param name="startDate">start date</param>
        //     /// <param name="endDate">end date</param>
        //     /// <param name="frequency">frequency</param>
        //     /// <returns>returns collection of date</returns>
        //     private List<DateTime> GetNotificationDateRange(DateTime startDate, DateTime endDate, int frequency)
        //     {
        //         List<DateTime> dates = new List<DateTime>();
        //         dates.Add(startDate);
        //         AspectEnums.NotificationFrequency recurrence = AppUtil.NumToEnum<AspectEnums.NotificationFrequency>(frequency);
        //         switch (recurrence)
        //         {
        //             case AspectEnums.NotificationFrequency.Daily:
        //                 while (endDate > startDate)
        //                 {
        //                     startDate = startDate.AddDays(1);
        //                     dates.Add(startDate);
        //                 }
        //                 break;
        //             case AspectEnums.NotificationFrequency.Weekly:
        //                 while (endDate > startDate)
        //                 {
        //                     startDate = startDate.AddDays(7);
        //                     dates.Add(startDate);
        //                 }
        //                 break;
        //             case AspectEnums.NotificationFrequency.Hourly:
        //                 while (endDate > startDate)
        //                 {
        //                     startDate = startDate.AddHours(1);
        //                     dates.Add(startDate);
        //                 }
        //                 break;
        //             case AspectEnums.NotificationFrequency.Fortnightly:
        //                 while (endDate > startDate)
        //                 {
        //                     startDate = startDate.AddDays(15);
        //                     dates.Add(startDate);
        //                 }
        //                 break;
        //             case AspectEnums.NotificationFrequency.Monthly:
        //                 while (endDate > startDate)
        //                 {
        //                     startDate = startDate.AddMonths(1);
        //                     dates.Add(startDate);
        //                 }
        //                 break;
        //         }
        //         return dates;
        //     }
        /// <summary>
        /// This function will validate service token by userid
        /// </summary>
        /// <param name="apiKey"></param>
        /// <param name="apiToken"></param>
        /// <returns></returns>
        public int GetServiceTokenUserID(string apiKey, string apiToken)
        {
            UserServiceAccess serviceDetails = AccuitAdminDbContext.UserServiceAccesses.FirstOrDefault(k => k.APIKey == apiKey && k.APIToken == apiToken);
            if (serviceDetails != null)
                return serviceDetails.UserID;
            return -1;
        }
        //     //VC20140819

        //     private string LastMinute
        //     {
        //         get { return " 11:59 PM"; }

        //     }

        //     /// <summary>
        //     /// Get Time Slot settings for coverage Batch Notification
        //     /// </summary>        
        //     /// <returns>Get Time Slot settings for coverage Batch Notification</returns>
        //     /// 

        //     public CoverageNotificationTimeSetting CoverageNotificationTimeSetting()
        //     {


        //         try
        //         {
        //             #region Cache implementation for Time Slot
        //             //Save CoverageNotificationTimeSettings in Cache                           
        //             LogTraceEngine.WriteLogWithCategory("Coverage Notification service Called Step 2 ", AppVariables.AppLogTraceCategoryName.NotificationListener);

        //             if (HttpRuntime.Cache[DateTime.Now.Date.ToString(CacheVariables.ddmmyyyyCoverageBatchNotification)] == null)
        //             {
        //                 var todaysLastMinute = Convert.ToDateTime(DateTime.Now.Date.ToString("dd-MMM-yyyy") + LastMinute.ToString());
        //                 HttpRuntime.Cache.Insert(DateTime.Now.Date.ToString(CacheVariables.ddmmyyyyCoverageBatchNotification), AccuitAdminDbContext.CoverageNotificationTimeSettings.Where(x => x.IsDeleted == false).ToList(), null, todaysLastMinute, Cache.NoSlidingExpiration);
        //             }
        //             var CoverageNotificationTimeSettings = HttpRuntime.Cache[DateTime.Now.Date.ToString(CacheVariables.ddmmyyyyCoverageBatchNotification)] as List<CoverageNotificationTimeSetting>;
        //             LogTraceEngine.WriteLogWithCategory("Coverage Notification service Called step 3", AppVariables.AppLogTraceCategoryName.NotificationListener);
        //             #endregion

        //             //Calculate current TimeSlot to match with CoverageNotificationTimeSettings TimeSlot
        //             DateTime TimeFrom = System.DateTime.Now;
        //             DateTime TimeTo = TimeFrom.AddSeconds(Interval);
        //             var CoverageNotificationTimeSetting = CoverageNotificationTimeSettings.Select(x => new { CoverageNotificationTimeSettingsID = x.CoverageNotificationTimeSettingsID, TimeSlot = Convert.ToDateTime(x.TimeSlot), UserType = x.UserType, NotificationType = x.NotificationType }).Where(a => a.TimeSlot >= TimeFrom && a.TimeSlot < TimeTo).FirstOrDefault();
        //             LogTraceEngine.WriteLogWithCategory("Coverage Notification service Called Step 4", AppVariables.AppLogTraceCategoryName.NotificationListener);
        //             if (CoverageNotificationTimeSetting != null)
        //             {
        //                 LogTraceEngine.WriteLogWithCategory("Coverage Notification service Called Step 5", AppVariables.AppLogTraceCategoryName.NotificationListener);
        //                 //Generate Coverage Notification data if timeslot gets matched
        //                 AccuitAdminDbContext.SPGetNotificationCoverageForTheDay(CoverageNotificationTimeSetting.UserType, CoverageNotificationTimeSetting.NotificationType);
        //                 return CoverageNotificationTimeSettings.FirstOrDefault(x => x.CoverageNotificationTimeSettingsID == CoverageNotificationTimeSetting.CoverageNotificationTimeSettingsID);
        //             }
        //         }
        //         catch
        //         {
        //             LogTraceEngine.WriteLogWithCategory("Coverage Notification service Called Error while Calculating Time Setttings", AppVariables.AppLogTraceCategoryName.NotificationListener);
        //         }
        //         return null;
        //     }



        //     /// <summary>
        //     /// Method to list Coverage Notification service messages
        //     /// </summary>        
        //     /// <returns>list of Coverage notification messages</returns>    
        //     public List<CoverageNotificationService> GetCoverageNotificationService()
        //     {
        //         //AccuitAdminDbContext.Entry(AccuitAdminDbContext.CoverageNotificationServices).Reload();
        //         return AccuitAdminDbContext.CoverageNotificationServices.Where(a => a.DeliveryStatus == 0).ToList();
        //     }

        //     /// <summary>
        //     /// Method to update push notification service response for Coverage
        //     /// </summary>
        //     /// <param name="CoveragenotificationServiceID">Coverage notification service ID</param>
        //     /// <param name="response">response</param>
        //     /// <param name="isSuccess">is success</param>
        //     /// <returns>returns boolean response</returns>
        //     /// 
        //     public bool UpdateCoverageNotificationServiceResponse(List<CoverageNotificationService> service)
        //     {
        //         bool isSuccess = false;


        //         using (var context = new SmartDostEntities())
        //         {
        //             var allServices = context.CoverageNotificationServices.ToList();
        //             DateTime modifieddate = System.DateTime.Now;
        //             foreach (var item in service)
        //             {
        //                 var NotifyService = allServices.FirstOrDefault(k => k.CoverageNotificationServiceID == item.CoverageNotificationServiceID);
        //                 NotifyService.DeliveryStatus = item.DeliveryStatus;
        //                 NotifyService.Remarks = item.Remarks;
        //                 NotifyService.ModifiedDate = modifieddate;
        //                 context.Entry<CoverageNotificationService>(NotifyService).State = System.Data.Entity.EntityState.Modified;
        //             }

        //             isSuccess = context.SaveChanges() > 0 ? true : false;
        //         }
        //         return false;
        //     }
        //     //VC20140819

        //     //VC20140826
        //     public List<NotificationService> GetNotificationService(DateTime datetime)
        //     {
        //         DateTime NextDateTime = datetime.AddSeconds(Interval);
        //         var data = AccuitAdminDbContext.NotificationServices.Where(x => x.DeliveryStatus == 0);
        //         var findaldata = data.Where(a => a.NotificationDate >= datetime && a.NotificationDate < NextDateTime && a.DeliveryStatus == 0).ToList();
        //         return findaldata;
        //     }
        //     //VC20140826


        //     #region DataMaster for APK Download VC20140915
        //     /// <summary>
        //     /// Function to get the Datamaster for Download
        //     /// </summary>
        //     public IList<DownloadDataMaster> GetDownloadDataMasterList()
        //     {
        //         return AccuitAdminDbContext.DownloadDataMasters.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
        //     }

        //     /// <summary>
        //     /// Get All Download Master data for particular Role ID
        //     /// </summary>
        //     /// <param name="RoleID">Particular Role ID for which data of Download Authorization needs to be fetched</param>
        //     /// <returns>List of Master Data for a Role </returns>
        //     public List<DownloadMasterAuthorization> GetDownloadDataAuthorizationByRoleID(int RoleID)
        //     {
        //         return AccuitAdminDbContext.DownloadMasterAuthorizations.Where(x => x.RoleID == RoleID && x.IsDeleted == false && x.IsActive == true).ToList();
        //     }

        //     /// <summary>
        //     /// Insert APK Download Authorization data
        //     /// </summary>        
        //     /// <returns>Success or Failure</returns>
        //     public bool InsertAPKDataAuthorization(DownloadMasterAuthorization APKAuthorization)
        //     {
        //         bool isSuccess = false;
        //         if (APKAuthorization != null)
        //         {
        //             AccuitAdminDbContext.Entry<DownloadMasterAuthorization>(APKAuthorization).State = System.Data.Entity.EntityState.Added;
        //             isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //         }
        //         return isSuccess;
        //     }

        //     /// <summary>
        //     /// Delete APK Download Authorization data for a particular Role
        //     /// </summary>        
        //     /// <returns>Success or Failure</returns>
        //     public bool DeleteAPKDataAuthorization(DownloadMasterAuthorization APKAuthorization)
        //     {
        //         bool isSuccess = false;
        //         if (APKAuthorization != null)
        //         {
        //             var APKDataMasterExisting = AccuitAdminDbContext.DownloadMasterAuthorizations.Where(x => x.DownloadDataMasterID == APKAuthorization.DownloadDataMasterID && x.RoleID == APKAuthorization.RoleID).SingleOrDefault();
        //             if (APKAuthorization != null)
        //             {
        //                 AccuitAdminDbContext.DownloadMasterAuthorizations.Remove(APKDataMasterExisting);
        //                 isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //             }
        //             else
        //             {
        //                 isSuccess = true;
        //             }
        //         }
        //         return isSuccess;
        //     }
        //     #endregion

        //     #region Unfreeze Stores
        //     /// <summary>
        //     /// Validate StoreCode in StoreMaster
        //     /// </summary>
        //     /// <param name="StoreCodes"></param>
        //     /// <returns></returns>        
        //     public List<string> ValidateStoreCode(List<string> StoreCodes)
        //     {
        //         var result = (from s in StoreCodes
        //                       where !AccuitAdminDbContext.StoreMasters.Any(sm => sm.StoreCode == s && sm.IsDeleted == false && sm.IsActive == true)
        //                       select s).ToList();

        //         return result;
        //     }

        //     /// <summary>
        //     /// Unfreeze stores
        //     /// </summary>
        //     /// <param name="StoreCodes"></param>
        //     /// <returns>true or false</returns>        
        //     public bool UnfreezeGeoTag(List<string> StoreCodes)
        //     {
        //         string stores = String.Join(",", StoreCodes.Select(x => x.ToString()));
        //         return AccuitAdminDbContext.SpUnfreezeGeoTag(stores) == 0 ? false : true;

        //     }

        //     #endregion

        //     #region SDCE-579 Added by Niranjan (Product Group Category) 13-10-2014
        //     /// <summary>
        //     /// Function to bind the Product Group Category
        //     /// </summary>
        //     /// <returns></returns>
        //     public IList<ProductGroupCategory> GetProductGroupCategory()
        //     {
        //         var lstProductGroupCategory = new List<ProductGroupCategory>();
        //         lstProductGroupCategory = AccuitAdminDbContext.ProductGroupCategories.Where(s => s.IsDeleted == false).ToList();
        //         return lstProductGroupCategory;
        //     }

        //     /// <summary>
        //     /// Method to use for Insert Product Group Category
        //     /// </summary>
        //     /// <returns></returns>

        //     public bool IsProductGroupCategoryInsert(ProductGroupCategory record)
        //     {
        //         bool isSuccess = false;
        //         if (record.ProductGroupCategoryId.Equals(0))
        //         {
        //             record.CreatedDate = System.DateTime.Now;
        //             record.IsDeleted = false;
        //             AccuitAdminDbContext.Entry<ProductGroupCategory>(record).State = System.Data.Entity.EntityState.Added;
        //             isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;

        //         }
        //         else
        //         {
        //             var ProductGroupCategory = AccuitAdminDbContext.ProductGroupCategories.FirstOrDefault(k => k.ProductGroupCategoryId == record.ProductGroupCategoryId);
        //             if (ProductGroupCategory != null)
        //             {
        //                 ProductGroupCategory.ModifyBy = record.ModifyBy;
        //                 ProductGroupCategory.ModifyDate = System.DateTime.Now;
        //                 ProductGroupCategory.ProductGroupCategoryName = record.ProductGroupCategoryName;
        //                 ProductGroupCategory.ProductGroupCategoryDescription = record.ProductGroupCategoryDescription;
        //                 ProductGroupCategory.Sequence = record.Sequence;
        //                 isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //             }
        //             else
        //             {
        //                 isSuccess = false;
        //             }
        //         }

        //         return isSuccess;
        //     }

        //     /// <summary>
        //     ///  Funtion to Delete Product Group Category
        //     /// </summary>
        //     /// <param name="ProductGroupCategoryId"></param>
        //     /// <returns></returns>
        //     public bool DeleteProductGroupCategories(List<int> ProductGroupCategory)
        //     {
        //         bool isSuccess = false;
        //         using (TransactionScope scope = new TransactionScope())
        //         {
        //             foreach (int id in ProductGroupCategory)
        //             {
        //                 ProductGroupCategory Product = AccuitAdminDbContext.ProductGroupCategories.FirstOrDefault(k => k.ProductGroupCategoryId == id);
        //                 if (Product != null)
        //                 {
        //                     Product.IsDeleted = true;
        //                     AccuitAdminDbContext.Entry<ProductGroupCategory>(Product).State = System.Data.Entity.EntityState.Modified;
        //                 }
        //             }
        //             AccuitAdminDbContext.SaveChanges();
        //             scope.Complete();
        //             isSuccess = true;
        //         }
        //         return isSuccess;
        //     }


        //     /// <summary>
        //     /// Method to get detail on the basis of ProductGroupCategory Id ,ProductGroupCategoryName
        //     /// </summary>
        //     /// <param name="ProductGroupCategoryId"></param>
        //     /// <returns>object of ProductGroupCategory  class </returns>
        //     public ProductGroupCategory ProductGroupCategoryDetail(int? ProductGroupCategoryId, string ProductGroupCategoryName)
        //     {

        //         ProductGroupCategory result = ProductGroupCategoryId == null ?
        //             AccuitAdminDbContext.ProductGroupCategories.SingleOrDefault(k => k.ProductGroupCategoryName == ProductGroupCategoryName) :
        //             AccuitAdminDbContext.ProductGroupCategories.SingleOrDefault(k => k.ProductGroupCategoryId == ProductGroupCategoryId);
        //         return result;

        //     }
        //     #endregion

        //     #region SDCE-670 Added by Niranjan (Channel Type Mapping) 16-10-2014
        //     /// <summary>
        //     /// GetAll Channel Type
        //     /// </summary>
        //     /// <returns></returns>

        //     public IList<ChannelTypeDisplay> GetChannelTypeMappingList()
        //     {
        //         List<ChannelTypeDisplay> channelTypeDisplay = new List<ChannelTypeDisplay>();

        //         channelTypeDisplay = (from CTM in AccuitAdminDbContext.ChannelTypeTeamMappings
        //                               join CTD in AccuitAdminDbContext.ChannelTypeDisplays on CTM.ChannelType equals CTD.ChannelType into ej
        //                               from CTD in ej.DefaultIfEmpty()
        //                               where CTM.IsDeleted == false
        //                               select new { CType = CTM.ChannelType, IsDisplay = CTD == null ? false : CTD.IsDisplayCounterShare, IsPlanogram = CTD == null ? false : CTD.IsPlanogram, ChannelTypeDisplayID = CTD == null ? 0 : CTD.ChannelTypeDisplayID }
        //                               ).AsEnumerable().Select(d => new ChannelTypeDisplay() { ChannelType = d.CType, IsPlanogram = d.IsPlanogram, IsDisplayCounterShare = d.IsDisplay, ChannelTypeDisplayID = d.ChannelTypeDisplayID }).Distinct().ToList();

        //         return channelTypeDisplay;

        //     }

        //     /// <summary>
        //     /// GetAll Channel Type Display add and Update
        //     /// </summary>
        //     /// <returns></returns>
        //     public bool UpdateChannelTypeDisplay(List<ChannelTypeDisplay> record)
        //     {
        //         bool isSucess = false;


        //         //foreach (var item in record)
        //         //{
        //         //    if (item.ChannelTypeDisplayID > 0)
        //         //    {
        //         //        AccuitAdminDbContext.ChannelTypeDisplays.Add(item);
        //         //        AccuitAdminDbContext.Entry<ChannelTypeDisplay>(item).State = System.Data.Entity.EntityState.Modified;
        //         //    }
        //         //    else
        //         //    {
        //         //        AccuitAdminDbContext.ChannelTypeDisplays.Add(item);
        //         //        AccuitAdminDbContext.Entry<ChannelTypeDisplay>(item).State = System.Data.Entity.EntityState.Added;
        //         //    }

        //         //}


        //         //for (int i = 0; i < record.Count; i++)
        //         //{
        //         //    var item = record.ElementAtOrDefault(i);

        //         //    if (record.ElementAtOrDefault(i).ChannelTypeDisplayID > 0)
        //         //    {
        //         //        // Update Case //
        //         //        AccuitAdminDbContext.ChannelTypeDisplays.Add(item);
        //         //        AccuitAdminDbContext.Entry<ChannelTypeDisplay>(item).State = System.Data.Entity.EntityState.Modified;
        //         //    }
        //         //    // Insert Case //
        //         //    else
        //         //    {
        //         //        AccuitAdminDbContext.ChannelTypeDisplays.Add(item);
        //         //        AccuitAdminDbContext.Entry<ChannelTypeDisplay>(item).State = System.Data.Entity.EntityState.Added;
        //         //    }
        //         //}
        //         //AccuitAdminDbContext.SaveChanges();
        //         //isSucess = true;

        //         foreach (var item in record)
        //         {
        //             if (item.ChannelTypeDisplayID > 0)
        //             {
        //                 item.ModifyBy = Convert.ToInt32(HttpContext.Current.Session[SessionVariables.SessionUserID]);
        //                 item.ModifyDate = System.DateTime.Now;
        //                 AccuitAdminDbContext.ChannelTypeDisplays.Add(item);
        //                 AccuitAdminDbContext.Entry<ChannelTypeDisplay>(item).State = System.Data.Entity.EntityState.Modified;
        //             }
        //             else
        //             {
        //                 AccuitAdminDbContext.ChannelTypeDisplays.Add(item);
        //                 AccuitAdminDbContext.Entry<ChannelTypeDisplay>(item).State = System.Data.Entity.EntityState.Added;
        //             }
        //         }
        //         AccuitAdminDbContext.SaveChanges();
        //         isSucess = true;

        //         return isSucess;
        //     }


        //     #endregion

        //     #region SDCE-634 :: Activity Role Module Authorization  (17 Oct 2014)
        //     /// <summary> 
        //     /// Get All rolemoudle data for particular roleID for SDCE-634
        //     /// </summary>
        //     /// <param name="RoleID">Particular Role ID for which data of role module needs to be fetched</param>
        //     /// <returns>List of RoleModule</returns>
        //     public List<ActivityModule> GetActivityModulesByRoleID(int RoleID)
        //     {
        //         return AccuitAdminDbContext.ActivityModules.Where(x => x.RoleID == RoleID).ToList();
        //     }

        //     /// <summary>
        //     /// Insert activity Role Module data
        //     /// </summary>        
        //     /// <returns>Success or Failure</returns>
        //     public bool InsertactivityRoleModule(ActivityModule roleModule)
        //     {
        //         bool isSuccess = false;
        //         if (roleModule != null)
        //         {
        //             AccuitAdminDbContext.Entry<ActivityModule>(roleModule).State = System.Data.Entity.EntityState.Added;
        //             isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //         }
        //         return isSuccess;
        //     }

        //     /// <summary>
        //     /// Delete activity Role Module data
        //     /// </summary>        
        //     /// <returns>Success or Failure</returns>
        //     public bool DeleteactivityRoleModule(ActivityModule roleModule)
        //     {
        //         bool isSuccess = false;
        //         if (roleModule != null)
        //         {
        //             var roleModuleExisting = AccuitAdminDbContext.ActivityModules.Where(x => x.ActivityModuleid == roleModule.ActivityModuleid).SingleOrDefault();
        //             if (roleModule != null)
        //             {
        //                 AccuitAdminDbContext.ActivityModules.Attach(roleModuleExisting);
        //                 AccuitAdminDbContext.ActivityModules.Remove(roleModuleExisting);
        //                 isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //             }
        //             else
        //             {
        //                 isSuccess = true;
        //             }
        //         }
        //         return isSuccess;
        //     }
        //     #endregion

        //     #region Upload VOC Data for SDCE-892 by Vaishali on 12-Nov-2013
        //     /// <summary>
        //     /// VOC Upload
        //     /// </summary>
        //     /// <param name="VOC"></param>
        //     /// <param name="userid"></param>
        //     /// <returns></returns>      
        //     public bool VOCUpload(string VOCxml, long userid)
        //     {
        //         try
        //         {
        //             AccuitAdminDbContext.spVOCUpload(VOCxml, userid);
        //             return true;
        //         }
        //         catch
        //         {
        //             LogTraceEngine.WriteLogWithCategory("Error while uploading VOC data", AppVariables.AppLogTraceCategoryName.General);
        //             return false;
        //         }

        //     }



        /// <summary>
        /// CommonSetup 
        /// </summary>
        /// <param name="MainType"></param>
        /// <param name="subtype"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public List<CommonSetup> GetCommonSetup(int DisplayValue, string subtype, string parentid)
        {
            List<CommonSetup> commonSetupresult = new List<CommonSetup>();

            var parentids = parentid.Split(',');
            var parentIdsFinal = parentids.Select(x => int.Parse(x)).ToList();

            if (DisplayValue > 0 && subtype != null)
            {
                commonSetupresult = (from common in AccuitAdminDbContext.CommonSetups
                                     join lst in parentIdsFinal
                                     on common.ParentID equals lst
                                     where common.DisplayValue == DisplayValue && common.SubType == subtype //&& common.IsDeleted == false
                                     select common).ToList();
            }

            if (subtype != null)
            {
                commonSetupresult = (from common in AccuitAdminDbContext.CommonSetups
                                     join lst in parentIdsFinal
                                     on common.ParentID equals lst
                                     where common.SubType == subtype
                                     select common).ToList();
            }
            else
            {
                commonSetupresult = (from common in AccuitAdminDbContext.CommonSetups
                                     join lst in parentIdsFinal
                                     on common.ParentID equals lst
                                     //where common.IsDeleted == false
                                     select common).ToList();
            }

            return commonSetupresult;
        }
        //     #endregion

        //     public IList<FeedbackCategoryMaster> GetFeedbackCategoryList()
        //     {
        //         return AccuitAdminDbContext.FeedbackCategoryMasters.ToList();
        //     }

        //     #region for SDCE -991 (FMS) by vaishali on 06 Dec 2014
        //     public bool QueueNotification(long userID, string notificationMessage, AspectEnums.NotificationType notificationType, int? NotificationID = null)
        //     {
        //         return AccuitAdminDbContext.SPQueueNotification((int)notificationType, NotificationID, userID, notificationMessage, null) > 0 ? true : false;
        //     }
        //     #endregion

        //     #region Ageing support for End of life products
        //     /*
        //      Created By     ::      Vaishali Choudhary
        //      Created Date   ::      09 March 2015
        //      JIRA ID        ::      
        //      Purpose        ::      Services for Ageing support 
        //      */

        //     #region Product Selection
        //     /// <summary>
        //     /// Show the list of Product Types
        //     /// </summary>
        //     /// <returns>List of Product Types</returns>
        //     public List<ProductType> GetProductType()
        //     {
        //         List<ProductType> productType = new List<ProductType>();
        //         using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //                new TransactionOptions
        //                {
        //                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //                }))
        //         {

        //             productType = AccuitAdminDbContext.ProductMasters.Where(x => x.IsDeleted == false).Select(x => new ProductType() { ProductTypeCode = x.ProductTypeCode, ProductTypeName = x.ProductTypeName }
        //                                                                                             ).Distinct().ToList();

        //             scope.Complete();
        //         }

        //         return productType;

        //     }

        //     /// <summary>
        //     /// Show the list of Product Groups
        //     /// </summary>
        //     /// <returns>List of Product Groups</returns>
        //     public List<ProductGroup> GetProductGroup(string productTypeCode)
        //     {

        //         List<ProductGroup> productGroup = new List<ProductGroup>();
        //         using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //                new TransactionOptions
        //                {
        //                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //                }))
        //         {
        //             productGroup = AccuitAdminDbContext.ProductMasters.Where(x => x.IsDeleted == false && x.ProductTypeCode == productTypeCode)
        //                                                     .Select(x => new ProductGroup()
        //                                                     {
        //                                                         ProductTypeCode = x.ProductTypeCode,
        //                                                         ProductGroupCode = x.ProductGroupCode,
        //                                                         ProductGroupName = x.ProductGroupName
        //                                                     }
        //                                                     ).Distinct().ToList();
        //             scope.Complete();
        //         }
        //         return productGroup;

        //     }

        //     /// <summary>
        //     /// Show the list of Product Groups
        //     /// </summary>
        //     /// <returns>List of Product Groups</returns>
        //     public List<ProductCategory> GetProductCategory(string productTypeCode, string productGroupCode)
        //     {

        //         List<ProductCategory> productCategory = new List<ProductCategory>();
        //         using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //                new TransactionOptions
        //                {
        //                    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //                }))
        //         {

        //             productCategory = AccuitAdminDbContext.ProductMasters.Where(x => x.IsDeleted == false &&
        //                                                            x.ProductTypeCode == productTypeCode &&
        //                                                            x.ProductGroupCode == productGroupCode)
        //                                                     .Select(x => new ProductCategory()
        //                                                         {
        //                                                             ProductTypeCode = x.ProductTypeCode,
        //                                                             ProductGroupCode = x.ProductGroupCode,
        //                                                             CategoryCode = x.CategoryCode,
        //                                                             CategoryName = x.CategoryName
        //                                                         }
        //                                                         ).Distinct().ToList();
        //             scope.Complete();
        //         }
        //         return productCategory;

        //     }

        //     /// <summary>
        //     /// Show the list of Basic Models under ProductType, ProductGroup and ProductCategory
        //     /// </summary>
        //     /// <returns>List of Basic Models</returns>
        //     public List<ProductModel> GetBasicModel(string productTypeCode, string productGroupCode, string CategoryCode)
        //     {

        //         List<ProductModel> basicModels = new List<ProductModel>();
        //         using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //              new TransactionOptions
        //              {
        //                  IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //              }))
        //         {

        //             basicModels = AccuitAdminDbContext.ProductMasters.Where(x => x.IsDeleted == false &&
        //                                                                  x.ProductTypeCode == productTypeCode &&
        //                                                                  x.ProductGroupCode == productGroupCode &&
        //                                                                  x.CategoryCode == CategoryCode
        //                                                                  )
        //                                                           .Select(x => new ProductModel()
        //                                                           {
        //                                                               ProductTypeCode = x.ProductTypeCode,
        //                                                               ProductGroupCode = x.ProductGroupCode,
        //                                                               CategoryCode = x.CategoryCode,
        //                                                               BasicModelCode = x.BasicModelCode,
        //                                                               BasicModelName = x.BasicModelName
        //                                                           }
        //                                                               ).Distinct().ToList();

        //             scope.Complete();
        //         }
        //         return basicModels;

        //     }

        //     #endregion

        //     #region scheme Implementation
        //     /// <summary>
        //     /// Get list of Schemes 
        //     /// </summary>
        //     /// <param name="SchemeID"></param>
        //     /// <param name="SchemeNumber"></param>
        //     /// <returns></returns>
        //     public List<EOLSchemeHeader> GetAllEOLSchemes(int? SchemeID, string SchemeNumber)
        //     {
        //         List<EOLSchemeHeader> eolSchemeHeader = new List<EOLSchemeHeader>();
        //         using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //              new TransactionOptions
        //              {
        //                  IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //              }))
        //         {

        //             eolSchemeHeader = (from EOLHeader in AccuitAdminDbContext.EOLSchemeHeaders
        //                                where EOLHeader.IsDeleted == false
        //                                && EOLHeader.SchemeID == (SchemeID == null ? EOLHeader.SchemeID : SchemeID)
        //                                && EOLHeader.SchemeNumber == (string.IsNullOrEmpty(SchemeNumber) ? EOLHeader.SchemeNumber : SchemeNumber)
        //                                orderby EOLHeader.SchemeFrom descending
        //                                select EOLHeader).ToList();

        //             scope.Complete();
        //         }

        //         return eolSchemeHeader;


        //     }

        //     /// <summary>
        //     /// Get Last Scheme Number For Today
        //     /// </summary>
        //     /// <returns></returns>
        //     public int GetLastSchemeNumber()
        //     {
        //         int Result = 0;
        //         DateTime Today = DateTime.Today;
        //         DateTime Tomorrow = DateTime.Today.AddDays(1);
        //         using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //            new TransactionOptions
        //            {
        //                IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //            }))
        //         {
        //             EOLSchemeHeader obj = AccuitAdminDbContext.EOLSchemeHeaders.Where(k => k.CreatedDate >= Today && k.CreatedDate < Tomorrow).OrderByDescending(k => k.SchemeID).FirstOrDefault();
        //             if (obj != null)
        //             {
        //                 string[] schemparts = obj.SchemeNumber.Split('-');

        //                 Result = Convert.ToInt32(schemparts[schemparts.Length - 1]);
        //             }
        //             scope.Complete();
        //         }
        //         return Result;
        //     }

        //     /// <summary>
        //     /// Save or Update scheme in Database and return ID of registered Scheme
        //     /// </summary>
        //     /// <param name="scheme"></param>
        //     /// <param name="ActionType"></param>
        //     /// <returns></returns>
        //     public EOLSchemeHeader EOLSaveScheme(EOLSchemeHeader scheme, byte ActionType, long userID)
        //     {
        //         bool isSuccess = false;
        //         using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //            new TransactionOptions
        //            {
        //                IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //            }))
        //         {
        //             if (scheme != null)
        //             {
        //                 if (ActionType == (byte)AspectEnums.EntityActionType.Insert)
        //                 {
        //                     DateTime Today = DateTime.Today;
        //                     scheme.SchemeNumber = scheme.ProductType + "/" + scheme.ProductGroup + "/" + scheme.ProductCategory + "/" + scheme.PUMINumber + "/" + Today.ToString("ddMMyyyy") + "-" + (GetLastSchemeNumber() + 1);
        //                     scheme.CreatedDate = DateTime.Now;
        //                     scheme.CreatedBy = userID;
        //                     AccuitAdminDbContext.Entry<EOLSchemeHeader>(scheme).State = System.Data.Entity.EntityState.Added;
        //                     isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //                 }
        //                 else
        //                 {


        //                     var updatescheme = AccuitAdminDbContext.EOLSchemeHeaders.Where(x => x.SchemeID == scheme.SchemeID).FirstOrDefault();

        //                     updatescheme.OrderFrom = scheme.OrderFrom;
        //                     updatescheme.OrderTo = scheme.OrderTo;
        //                     updatescheme.SchemeFrom = scheme.SchemeFrom;
        //                     updatescheme.SchemeTo = scheme.SchemeTo;
        //                     updatescheme.ModifiedDate = System.DateTime.Now;
        //                     updatescheme.ModifiedBy = userID;

        //                     AccuitAdminDbContext.Entry<EOLSchemeHeader>(updatescheme).State = System.Data.Entity.EntityState.Modified;

        //                     isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //                 }
        //             }
        //             scope.Complete();
        //         }

        //         if (isSuccess == true)
        //             return scheme;
        //         else
        //             return null;
        //         //return isSuccess == true ? scheme.SchemeID : 0;
        //     }

        //     /// <summary>
        //     /// Save Products details for a scheme
        //     /// </summary>
        //     /// <param name="schemeProducts"></param>
        //     /// <returns></returns>
        //     public EOLSchemeHeader EOLSaveSchemeProducts(List<EOLSchemeDetail> schemeProducts, long userID, bool isSubmit)
        //     {
        //         bool isSuccess = false;
        //         EOLSchemeHeader eolSchemeHeader = new EOLSchemeHeader();
        //         if (schemeProducts != null && schemeProducts.Count > 0)
        //         {
        //             using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //           new TransactionOptions
        //           {
        //               IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //           }))
        //             {
        //                 //select schemeID from Product collection
        //                 var schemeID = schemeProducts.FirstOrDefault().SchemeID;

        //                 #region Delete existing products from Detail table
        //                 var eolSchemeDetail = AccuitAdminDbContext.EOLSchemeDetails.Where(x => x.SchemeID == schemeID).ToList();
        //                 foreach (var eolScheme in eolSchemeDetail)
        //                 {
        //                     AccuitAdminDbContext.EOLSchemeDetails.Remove(eolScheme);
        //                 }
        //                 #endregion

        //                 #region save new products in Detail table
        //                 foreach (var eolScheme in schemeProducts)
        //                 {
        //                     AccuitAdminDbContext.EOLSchemeDetails.Add(eolScheme);
        //                 }
        //                 #endregion

        //                 #region update scheme status and modification date
        //                 eolSchemeHeader = AccuitAdminDbContext.EOLSchemeHeaders.Where(x => x.SchemeID == schemeID).FirstOrDefault();
        //                 eolSchemeHeader.SaveStatus = isSubmit;
        //                 eolSchemeHeader.ModifiedDate = System.DateTime.Now;
        //                 eolSchemeHeader.ModifiedBy = userID;
        //                 AccuitAdminDbContext.Entry<EOLSchemeHeader>(eolSchemeHeader).State = System.Data.Entity.EntityState.Modified;
        //                 #endregion

        //                 isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //                 if (isSuccess)
        //                     scope.Complete();
        //             }
        //         }
        //         if (isSuccess)
        //             return eolSchemeHeader;
        //         else
        //             return null;
        //     }
        //     #endregion

        //     #region APK
        //     /// <summary>
        //     /// Get list of Schemes active for current Date
        //     /// </summary>
        //     /// <param name="SchemeID"></param>
        //     /// <param name="SchemeNumber"></param>
        //     /// <returns></returns>
        //     public List<EOLSchemeHeader> GetEOLSchemes(long userID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        //     {

        //         HasMoreRows = false;
        //         MaxModifiedDate = LastUpdatedDate;
        //         DateTime CurrentDateTime = System.DateTime.Now;


        //         List<EOLSchemeHeader> eolSchemeHeader = new List<EOLSchemeHeader>();
        //         if (AccuitAdminDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.ODScheme)).FirstOrDefault() == true)
        //         {

        //             System.DateTime CurrentDate = System.DateTime.Today.Date;

        //             using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //                  new TransactionOptions
        //                  {
        //                      IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //                  }))
        //             {


        //                 if (LastUpdatedDate == null)
        //                 {
        //                     eolSchemeHeader = AccuitAdminDbContext.EOLSchemeHeaders.Where(x => x.IsDeleted == false &&
        //                                     x.SaveStatus == true
        //                                     &&
        //                                     CurrentDate >= x.OrderFrom &&
        //                                     CurrentDate <= x.OrderTo
        //                                    ).Distinct().OrderBy(k => k.CreatedDate)
        //                        .Skip(StartRowIndex)
        //                        .Take(RowCount + 1).ToList();

        //                     //result = AccuitAdminDbContext.PaymentModes.Where(k => k.CompanyID == companyID && !k.IsDeleted) /*delete flag is there for APK user TBD*/
        //                     //   .OrderBy(k => k.CreatedDate)
        //                     //   .Skip(StartRowIndex)
        //                     //   .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       

        //                 }
        //                 else
        //                 {


        //                     eolSchemeHeader = AccuitAdminDbContext.EOLSchemeHeaders.Where(x => x.SaveStatus == true
        //                                    &&
        //                                    CurrentDate >= x.OrderFrom &&
        //                                    CurrentDate <= x.OrderTo &&
        //                             (
        //                         //(x.CreatedDate > LastUpdatedDate)
        //                             (LastUpdatedDate < (x.ModifiedDate ?? x.CreatedDate))
        //                             ||
        //                             (LastUpdatedDate == (x.ModifiedDate ?? x.CreatedDate))
        //                             )).Distinct().OrderBy(k => (k.ModifiedDate ?? k.CreatedDate))
        //                       .Skip(StartRowIndex)
        //                       .Take(RowCount + 1).ToList();


        //                     //result = AccuitAdminDbContext.PaymentModes.Where(k => k.CompanyID == companyID  /*delete flag is there for APK user TBD*/
        //                     //     &&
        //                     //        (
        //                     //        (k.CreatedDate > LastUpdatedDate)
        //                     //        ||
        //                     //        (k.CreatedDate == LastUpdatedDate.Value)
        //                     //        )
        //                     //        )
        //                     //   .OrderBy(k => k.CreatedDate)
        //                     //   .Skip(StartRowIndex)
        //                     //   .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                                        
        //                 }

        //                 HasMoreRows = eolSchemeHeader.Count > RowCount ? true : false;
        //                 eolSchemeHeader = eolSchemeHeader.Take(RowCount).ToList();

        //                 // Update last modified data among the data if available, else send the same modifieddate back  
        //                 if (eolSchemeHeader.Count > 0)
        //                 {
        //                     if (LastUpdatedDate == null && HasMoreRows == true)
        //                         MaxModifiedDate = null; //send maxmodifieddate null itself until the cycle not complete for first time data download
        //                     else if (LastUpdatedDate == null && HasMoreRows == false)
        //                         MaxModifiedDate = CurrentDateTime; //Current date returned to avoid garbage data in next cycle
        //                     else
        //                         MaxModifiedDate = eolSchemeHeader.Max(x => x.CreatedDate);

        //                 }


        //                 //eolSchemeHeader = AccuitAdminDbContext.EOLSchemeHeaders.Where(x => x.IsDeleted == false &&
        //                 //                    x.SaveStatus == true
        //                 //                    &&
        //                 //                    CurrentDate >= x.OrderFrom &&
        //                 //                    CurrentDate <= x.OrderTo
        //                 //                   ).Distinct().ToList();

        //                 scope.Complete();
        //             }

        //         }
        //         return eolSchemeHeader;



        //         //List<EOLSchemeHeader> eolSchemeHeader = new List<EOLSchemeHeader>();
        //         //if (AccuitAdminDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.ODScheme)).FirstOrDefault() == true)
        //         //{

        //         //    System.DateTime CurrentDate = System.DateTime.Today.Date;

        //         //    using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //         //         new TransactionOptions
        //         //         {
        //         //             IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //         //         }))
        //         //    {

        //         //        eolSchemeHeader = AccuitAdminDbContext.EOLSchemeHeaders.Where(x => x.IsDeleted == false &&
        //         //                            x.SaveStatus == true
        //         //                            &&
        //         //                            CurrentDate >= x.OrderFrom &&
        //         //                            CurrentDate <= x.OrderTo
        //         //                           ).Distinct().ToList();

        //         //        scope.Complete();
        //         //    }

        //         //}
        //         //return eolSchemeHeader;


        //     }

        //     /// <summary>
        //     /// Capture order against scheme
        //     /// </summary>
        //     /// <param name="eolOrderBookings"></param>
        //     /// <param name="userID"></param>
        //     /// <param name="storeID"></param>
        //     /// <returns>true or false</returns>
        //     public List<EOLScheme> SubmitEOLOrder(List<EOLOrderBooking> eolOrderBookings, long userID)
        //     {
        //         bool isSuccess = false;
        //         var groupschemes = eolOrderBookings.GroupBy(x => new { x.SchemeID });
        //         List<EOLScheme> eolSchemeHeaders = new List<EOLScheme>();

        //         using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //             new TransactionOptions
        //             {
        //                 IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //             }))
        //         {

        //             foreach (var itemscheme in groupschemes)
        //             {
        //                 EOLScheme eolSchemeHeader = new EOLScheme();

        //                 eolSchemeHeader = GetAllEOLSchemes(itemscheme.Key.SchemeID, null).Select(x => new EOLScheme()
        //                     {
        //                         CreatedBy = x.CreatedBy,
        //                         CreatedDate = x.CreatedDate,
        //                         EOLSchemeDetails = x.EOLSchemeDetails,
        //                         IsDeleted = x.IsDeleted,
        //                         ModifiedBy = x.ModifiedBy,
        //                         ModifiedDate = x.ModifiedDate,
        //                         OrderFrom = x.OrderFrom,
        //                         OrderTo = x.OrderTo,
        //                         ProductCategory = x.ProductCategory,
        //                         ProductGroup = x.ProductGroup,
        //                         ProductType = x.ProductType,
        //                         PUMIDate = x.PUMIDate,
        //                         PUMINumber = x.PUMINumber,
        //                         SaveStatus = x.SaveStatus,
        //                         SchemeFrom = x.SchemeFrom,
        //                         SchemeID = x.SchemeID,
        //                         SchemeNumber = x.SchemeNumber,
        //                         SchemeTo = x.SchemeTo

        //                     }).FirstOrDefault();

        //                 var groupOrderBookings = itemscheme.GroupBy(x => new { x.StoreID });
        //                 List<EOLOrders> NotificationOrders = new List<EOLOrders>();

        //                 foreach (var storewiseOrder in groupOrderBookings)
        //                 {
        //                     #region select existing order , Scheme Number and order Number
        //                     var existingOrders = AccuitAdminDbContext.EOLOrderBookings
        //                                         .Where(x => x.StoreID == storewiseOrder.Key.StoreID &&
        //                                         x.SchemeID == itemscheme.Key.SchemeID &&
        //                                         x.CreatedBy == userID
        //                                         ).ToList();

        //                     var orderNumber = existingOrders.Select(x => x.EOLOrderNumber).FirstOrDefault();
        //                     var schemeNumber = AccuitAdminDbContext.EOLSchemeHeaders.
        //                                         Where(x => x.SchemeID == itemscheme.Key.SchemeID).
        //                                         Select(x => x.SchemeNumber).FirstOrDefault();
        //                     #endregion


        //                     #region fecth Store Name
        //                     var storeName = AccuitAdminDbContext.StoreMasters.Where(x => x.StoreID == storewiseOrder.Key.StoreID).Select(x => x.StoreName).FirstOrDefault();
        //                     #endregion


        //                     foreach (EOLOrderBooking order in storewiseOrder)
        //                     {
        //                         //NotificationOrders.Add(order);
        //                         #region Fill Orders in entity EOLOrders, the new entity is created to keep the storename as well with store ID fot notification

        //                         var isNotificationRequired = existingOrders.Where(x => x.BasicModelCode == order.BasicModelCode &&
        //                                                                     x.ActualSupport == order.ActualSupport &&
        //                                                                     x.OrderQuantity == order.OrderQuantity).FirstOrDefault();
        //                         //restrict notification if there is no change in order of a basic model
        //                         if (isNotificationRequired == null)
        //                         {
        //                             NotificationOrders.Add(new EOLOrders()
        //                             {
        //                                 ActualSupport = order.ActualSupport,
        //                                 BasicModelCode = order.BasicModelCode,
        //                                 OrderQuantity = order.OrderQuantity,
        //                                 SchemeID = order.SchemeID,
        //                                 StoreID = order.StoreID,
        //                                 StoreName = storeName,
        //                             });
        //                         }
        //                         #endregion

        //                         #region check if basic model already exists, if yes then update Quantity and Support
        //                         var basicModelexists = existingOrders.Where(x => x.BasicModelCode == order.BasicModelCode).FirstOrDefault();

        //                         if (basicModelexists != null)
        //                         {
        //                             basicModelexists.OrderQuantity = order.OrderQuantity;
        //                             basicModelexists.ActualSupport = order.ActualSupport;
        //                             AccuitAdminDbContext.Entry<EOLOrderBooking>(basicModelexists).State = System.Data.Entity.EntityState.Modified;
        //                         }
        //                         #endregion

        //                         #region If new entry of basic model then save
        //                         else
        //                         {
        //                             if (string.IsNullOrEmpty(orderNumber))
        //                             {
        //                                 orderNumber = schemeNumber + "_" + storewiseOrder.Key.StoreID + "_" + System.DateTime.Now.Date.ToString("ddMMyyyy") + "_" + System.DateTime.Now.TimeOfDay.ToString("hhmmss");
        //                             }

        //                             order.CreatedDateTime = System.DateTime.Now;
        //                             order.CreatedBy = userID;
        //                             order.EOLOrderNumber = orderNumber;
        //                             AccuitAdminDbContext.EOLOrderBookings.Add(order);
        //                         }
        //                         #endregion
        //                     }

        //                 }
        //                 eolSchemeHeader.EOLOrderBooking = NotificationOrders;
        //                 eolSchemeHeaders.Add(eolSchemeHeader);
        //             }


        //             isSuccess = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;

        //             scope.Complete();
        //         }

        //         return isSuccess == false ? null : eolSchemeHeaders;

        //     }

        //     /// <summary>
        //     /// Select Last Order for a scheme
        //     /// </summary>
        //     /// <param name="schemeID"></param>
        //     /// <returns></returns>
        //     public List<EOLOrderBooking> LastsavedEOLActivity(int schemeID, int StoreID, bool returnAllSchemes)
        //     {
        //         List<EOLOrderBooking> eolOrderBooking = new List<EOLOrderBooking>();
        //         using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //       new TransactionOptions
        //       {
        //           IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //       }))
        //         {
        //             if (returnAllSchemes)
        //             {
        //                 eolOrderBooking = AccuitAdminDbContext.EOLOrderBookings
        //                               .Where(x =>
        //                                   x.StoreID == StoreID)
        //                                   .ToList();
        //             }
        //             else
        //                 eolOrderBooking = AccuitAdminDbContext.EOLOrderBookings
        //                                     .Where(x =>
        //                                         x.SchemeID == schemeID &&
        //                                         x.StoreID == StoreID)
        //                                         .ToList();

        //             scope.Complete();
        //         }
        //         return eolOrderBooking;
        //     }
        //     #endregion

        //     #region Scheme Notification
        //     #region select userid to which notification need to be sent
        //     /// <summary>
        //     /// Returns list of users to which EOL scheme notification need to be sent
        //     /// </summary>
        //     /// <param name="userID"></param>
        //     /// <param name="userRole"></param>
        //     /// <returns></returns>
        //     public long EOLNotificationUserID(long userID)
        //     {


        //         long NotificationuserID = userID;

        //         var userRole = AccuitAdminDbContext.UserRoles.Where(x => x.IsDeleted == false && x.UserID == userID).FirstOrDefault();

        //         if (userRole != null && userRole.RoleID == (int)AspectEnums.Roles.ASM)
        //         {
        //            // ReportDataImpl reportInstance = new ReportDataImpl();
        //             var teamofRole = AccuitAdminDbContext.RoleMasters.FirstOrDefault(x => x.RoleID == (int)AspectEnums.Roles.ASM);
        //            // var Seniorslst = reportInstance.GetSeniorsDB(userID, (long)teamofRole.TeamID);
        //             var BM = "";//Seniorslst.Where(x => x.RoleID == (int)AspectEnums.Roles.BM).FirstOrDefault();
        //             //if (BM != null)
        //                 //NotificationuserID = BM.UserID;
        //         }

        //         return NotificationuserID;

        //     }
        //     #endregion

        //     #region Select Notification Header and Body from Database
        //     public List<NotificationMessage> NotificationMessages(int NotificationType, int NotificationSubType, byte InterfaceType)
        //     {
        //         return AccuitAdminDbContext.NotificationMessages.Where(x => x.NotificationType == NotificationType &&
        //                                                                 x.NotificationSubType == NotificationSubType &&
        //                                                                 x.ForWebOrExe == InterfaceType &&
        //                                                                 x.IsDeleted == false).ToList();
        //     }
        //     #endregion

        //     #region Maintain EOL Log
        //     public bool EOLNotificationLog(int schemeID, AspectEnums.ODScheme SchemeNotificationType, int NotificationUserID, string PushNotificationMessage)
        //     {
        //         using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //         new TransactionOptions
        //         {
        //             IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //         }))
        //         {
        //             AccuitAdminDbContext.EOLNotificationServiceLogs.Add(new EOLNotificationServiceLog()
        //                 {
        //                     CreatedDate = System.DateTime.Now,
        //                     NotificationID = (byte)SchemeNotificationType,
        //                     SchemeID = schemeID,
        //                     UserID = NotificationUserID,
        //                 });

        //             AccuitAdminDbContext.SaveChanges();
        //             scope.Complete();
        //         }
        //         return true;
        //     }
        //     #endregion

        //     #endregion

        //     #region Scheme Report

        //     public List<SPGetSchemeReport_Result> GetSchemeReport(long UserID, int SelectedRoleID, DateTime? schemePeriodFrom, DateTime? schemePeriodTo, DateTime? orderSubmissionFrom, DateTime? orderSubmissionTo)
        //     {
        //         return AccuitAdminDbContext.SPGetSchemeReport(UserID, SelectedRoleID, schemePeriodFrom, schemePeriodTo, orderSubmissionFrom, orderSubmissionTo).OrderByDescending(k => k.SchemeFrom).ToList();
        //     }

        //     #endregion

        //     #region Remove EOL Notification LOG
        //     public bool RemoveEOLNotificationLog(int schemeID, int? userID)
        //     {
        //         bool isSuccess = false;
        //         using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //          new TransactionOptions
        //          {
        //              IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //          }))
        //         {

        //             var EOLNotificationLog = AccuitAdminDbContext.EOLNotificationServiceLogs.
        //                             Where(x => x.SchemeID == schemeID &&
        //                                 x.UserID == (userID == null ? x.UserID : userID)).ToList();

        //             foreach (var item in EOLNotificationLog)
        //             {
        //                 AccuitAdminDbContext.EOLNotificationServiceLogs.Remove(item);
        //             }
        //             AccuitAdminDbContext.SaveChanges();
        //             scope.Complete();
        //         }
        //         return isSuccess;
        //     }
        //     #endregion
        //     #endregion


        //     #region Usser list under given module
        //     public List<long> GetUsersUnderModule(int ModuleCode)
        //     {
        //         return (from user in AccuitAdminDbContext.UserMasters
        //                 join roles in AccuitAdminDbContext.UserRoles
        //                     on user.UserID equals roles.UserID
        //                 join rmodules in AccuitAdminDbContext.RoleModules
        //                     on roles.RoleID equals rmodules.RoleID
        //                 join m in AccuitAdminDbContext.Modules
        //                     on rmodules.ModuleID equals m.ModuleID
        //                 where
        //                     m.ModuleCode == ModuleCode &&
        //                     user.IsDeleted == false &&
        //                     roles.IsDeleted == false &&
        //                     m.IsDeleted == false
        //                 select user.UserID).Distinct().ToList();
        //     }
        //     #endregion


        //     public bool IsDownloadAuthorized(long userID, AspectEnums.DownloadService ServiceModule)
        //     {
        //         return AccuitAdminDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(ServiceModule)).FirstOrDefault().Value;
        //     }

        //     #region Beat Window settings Role Wise

        //     /// <summary>
        //     /// get Beat Window setting for selected Role
        //     /// </summary>
        //     /// <returns></returns>
        //     public BeatWindowSetting GetBeatWindowSettings(int RoleID)
        //     {
        //         BeatWindowSetting result = new BeatWindowSetting();
        //         result = AccuitAdminDbContext.BeatWindowSettings.Where(x => x.RoleID == RoleID).FirstOrDefault();
        //         return result;
        //     }
        //     #endregion

        //     #region SmartDost Scheme with HTML content


        //     /// <summary>
        //     /// Displays Schemes valid in current date
        //     /// </summary>
        //     /// <param name="userID"></param>
        //     /// <param name="RoleID"></param>
        //     /// <returns></returns>
        //     public List<SDScheme> GetTodaySchemes(long userID, long RoleID)
        //     {

        //         DateTime currentdate = System.DateTime.Now.Date;
        //         List<SDScheme> result = new List<SDScheme>();
        //         result = (from scheme in AccuitAdminDbContext.SDSchemes
        //                   join map in AccuitAdminDbContext.schemeRoleMappings
        //                   on scheme.SDSchemeID equals map.SDSchemeID
        //                   where
        //                   map.RoleID == RoleID &&
        //                   scheme.IsActive == true &&
        //                   scheme.IsDeleted == false &&
        //                   EntityFunctions.TruncateTime(scheme.DateValidFrom) <= EntityFunctions.TruncateTime(currentdate) &&
        //                   EntityFunctions.TruncateTime(scheme.DateValidTo) >= EntityFunctions.TruncateTime(currentdate)
        //                   select scheme).OrderBy(o => o.DateValidFrom).ToList();

        //         return result;

        //         //return AccuitAdminDbContext.SDSchemes.Where(k => k.IsActive == true && 
        //         //                                              k.IsDeleted == false &&
        //         //                                              EntityFunctions.TruncateTime(k.DateValidFrom) <= EntityFunctions.TruncateTime(currentdate) &&
        //         //                                              EntityFunctions.TruncateTime(k.DateValidTo) >= EntityFunctions.TruncateTime(currentdate))
        //         //                                              .OrderBy(o => o.DateValidFrom).ToList();            

        //     }


        //     /// <summary>
        //     /// Displays All available Schemes  
        //     /// </summary>
        //     /// <param name="userID"></param>
        //     /// <param name="RoleID"></param>
        //     /// <returns></returns>
        //     public List<SDScheme> GetAllSchemes(long userID, long RoleID)
        //     {
        //         return AccuitAdminDbContext.SDSchemes.Where(x => x.IsDeleted == false).ToList();
        //     }



        //     /// <summary>
        //     /// Get scheme details against a scheme ID
        //     /// </summary>
        //     /// <param name="userID"></param>
        //     /// <param name="RoleID"></param>
        //     /// <returns></returns>
        //     public SDScheme GetSchemeDetails(int SDSchemeID, string SchemeTitle)
        //     {
        //         SDScheme result = new SDScheme();
        //         DateTime currentdate = System.DateTime.Now.Date;

        //         if (SchemeTitle == null)
        //             result = AccuitAdminDbContext.SDSchemes.Where(x => x.SDSchemeID == SDSchemeID).FirstOrDefault();
        //         else
        //             result = AccuitAdminDbContext.SDSchemes.Where(x => x.Title == SchemeTitle && !x.IsDeleted).FirstOrDefault();
        //         return result;
        //     }


        //     /// <summary>
        //     /// Deletes the schemes.
        //     /// </summary>
        //     /// <param name="schemeList">The scheme list.</param>
        //     /// <returns></returns>
        //     public bool DeleteSchemes(List<SDScheme> schemeList)
        //     {
        //         bool status = false;

        //         if (schemeList.Count > 0)
        //         {
        //             using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
        //new TransactionOptions
        //{
        //    IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted
        //}))
        //             {
        //                 foreach (SDScheme item in schemeList)
        //                 {
        //                     SDScheme itemcurrent = AccuitAdminDbContext.SDSchemes.FirstOrDefault(k => k.SDSchemeID == item.SDSchemeID);
        //                     if (itemcurrent != null)
        //                     {
        //                         itemcurrent.ModifiedDate = DateTime.Now;
        //                         itemcurrent.ModifiedBy = item.ModifiedBy;
        //                         itemcurrent.IsDeleted = true;
        //                         AccuitAdminDbContext.Entry<SDScheme>(itemcurrent).State = System.Data.Entity.EntityState.Modified;
        //                         status = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //                     }
        //                 }
        //                 scope.Complete();
        //             }
        //         }
        //         return status;
        //     }

        //     /// <summary>
        //     /// Adds the update scheme.
        //     /// </summary>
        //     /// <param name="scheme">The scheme.</param>
        //     /// <returns></returns>
        //     public bool AddUpdateScheme(SDScheme scheme)
        //     {
        //         #region save new scheme
        //         if (scheme.SDSchemeID.Equals(0))
        //         {
        //             scheme.ModifiedBy = null;
        //             scheme.ModifiedDate = null;
        //             scheme.IsActive = true;
        //             scheme.IsDeleted = false;
        //             AccuitAdminDbContext.Entry<SDScheme>(scheme).State = System.Data.Entity.EntityState.Added;

        //             List<schemeRoleMapping> schemeRoleMapping = new List<schemeRoleMapping>();
        //             DateTime currentdatetime = System.DateTime.Now;
        //             if (scheme.schemeRoleMappings.Count() > 0)
        //             {
        //                 foreach (var item in scheme.schemeRoleMappings)
        //                 {
        //                     item.CreateDate = currentdatetime;
        //                 }
        //             }


        //             return AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //         }
        //         #endregion
        //         #region update existing scheme
        //         else
        //         {
        //             SDScheme schemeEntity = new SDScheme();

        //             schemeEntity = AccuitAdminDbContext.SDSchemes.FirstOrDefault(k => k.SDSchemeID == scheme.SDSchemeID);

        //             #region delete existing mapping
        //             List<schemeRoleMapping> existingschemeRoleMapping = new List<schemeRoleMapping>();
        //             existingschemeRoleMapping = schemeEntity.schemeRoleMappings.ToList();
        //             if (existingschemeRoleMapping.Count() > 0)
        //             {
        //                 foreach (var item in existingschemeRoleMapping)
        //                 {
        //                     AccuitAdminDbContext.Entry<schemeRoleMapping>(item).State = System.Data.Entity.EntityState.Deleted;
        //                 }
        //                 AccuitAdminDbContext.SaveChanges();
        //             }
        //             #endregion


        //             #region update mapping creation date
        //             DateTime currentdatetime = System.DateTime.Now;
        //             if (scheme.schemeRoleMappings.Count() > 0)
        //             {
        //                 foreach (var item in scheme.schemeRoleMappings)
        //                 {
        //                     item.CreateDate = currentdatetime;
        //                 }
        //             }
        //             #endregion


        //             #region save data
        //             if (schemeEntity != null)
        //             {
        //                 schemeEntity.Title = scheme.Title;
        //                 schemeEntity.DateValidFrom = scheme.DateValidFrom;
        //                 schemeEntity.DateValidTo = scheme.DateValidTo;
        //                 schemeEntity.HTMLFilename = scheme.HTMLFilename;
        //                 schemeEntity.ModifiedBy = scheme.ModifiedBy;
        //                 schemeEntity.ModifiedDate = scheme.ModifiedDate;
        //                 schemeEntity.schemeRoleMappings = scheme.schemeRoleMappings;
        //                 AccuitAdminDbContext.Entry<SDScheme>(schemeEntity).State = System.Data.Entity.EntityState.Modified;
        //                 return AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        //             }
        //             #endregion
        //             return false;
        //         }
        //         #endregion
        //     }

        //     #endregion

        //     #region New VOC Upload Manoranjan
        //     /// <summary>
        //     /// Return StoreProcedure value(bool)
        //     /// </summary>
        //     /// <param name="VOCxml"></param>
        //     /// <param name="userid"></param>
        //     /// <returns></returns>
        //     public bool NewVOCUpload(string VOCxml, long userid, byte uploadtype)
        //     {
        //         try
        //         {
        //             AccuitAdminDbContext.SPNewVOCUpload(VOCxml, userid, uploadtype);
        //             return true;
        //         }
        //         catch
        //         {
        //             LogTraceEngine.WriteLogWithCategory("Error while uploading VOC data", AppVariables.AppLogTraceCategoryName.General);
        //             return false;
        //         }

        //     }

        //     /// <summary>
        //     /// Return List of Category New voc Sentiment report 
        //     /// </summary>
        //     /// <param name="MainType"></param>
        //     /// <param name="subtype"></param>
        //     /// <param name="parentid"></param>
        //     /// <returns></returns>
        //     public List<CommonSetup> getNewVOCProductCategory(string MainType, string subtype, int parentid)
        //     {
        //         List<CommonSetup> objCommonSetUp = new List<CommonSetup>();
        //         var result = AccuitAdminDbContext.CommonSetups.Where(x => x.MainType == MainType && x.SubType == subtype && x.ParentID == parentid && x.IsDeleted == false).ToList();

        //         objCommonSetUp = result;
        //         return objCommonSetUp;
        //     }

        //     /// <summary>
        //     ///  Get New VOC Question 
        //     /// </summary>
        //     /// <param name="UploadType"></param>
        //     /// <returns></returns>
        //     public List<string> GetQuestionVOC(string UploadType)
        //     {
        //         List<string> lststring = new List<string>();
        //         string[] type = UploadType.Split(',');

        //         if (type.Contains("CE") == true && type.Contains("HHP") == true)
        //         {
        //             var result = (from a in AccuitAdminDbContext.NewVOCUserResponses
        //                           join b in AccuitAdminDbContext.NewVOCResponses
        //                               on a.CEVOCResponseID equals b.CEVOCResponseID
        //                           where a.Question != "" && a.Question.Contains(":")
        //                           select a.Question).Distinct().ToList();
        //             lststring = result.ToList();
        //             return lststring;
        //         }
        //         if (type.Contains("CE") == true)
        //         {
        //             var result = (from a in AccuitAdminDbContext.NewVOCUserResponses
        //                           join b in AccuitAdminDbContext.NewVOCResponses
        //                               on a.CEVOCResponseID equals b.CEVOCResponseID
        //                           where b.UploadType == 1 && a.Question.Contains(":")
        //                           select a.Question).Distinct().ToList();
        //             lststring = result.ToList();
        //             return lststring;
        //         }
        //         if (type.Contains("HHP") == true)
        //         {
        //             var result = (from a in AccuitAdminDbContext.NewVOCUserResponses
        //                           join b in AccuitAdminDbContext.NewVOCResponses
        //                               on a.CEVOCResponseID equals b.CEVOCResponseID
        //                           where b.UploadType == 2 && a.Question.Contains(":")
        //                           select a.Question).Distinct().ToList();
        //             lststring = result.ToList();
        //             return lststring;
        //         }
        //         return lststring;
        //     }
        //     #endregion

    }
}
