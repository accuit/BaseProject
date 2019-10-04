using Samsung.SmartDost.BusinessLayer.Services.BO;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.CommonLayer.Aspects.Exceptions;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using Samsung.SmartDost.PresentationLayer.ServiceImpl.Security;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Samsung.SmartDost.CommonLayer.Resources;
using Samsung.SmartDost.CommonLayer.Aspects.ReportBO;
using System.IO;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities.HttpMultipartParser;
using Samsung.SmartDost.ExternalServiceLayer.MDMServices.BO;

namespace Samsung.SmartDost.PresentationLayer.ServiceImpl
{
    public partial class SmartDost : BaseService
    {
        /// <summary>
        /// Displays the store profile.
        /// </summary>
        /// <param name="storeId">The store identifier.</param>
        /// <param name="storeId">The store identifier.</param>
        /// <returns></returns>
        [UserSecureOperation]
        public JsonResponse<StoreDTO> DisplayStoreProfile(int storeID, long userID)
        {
            JsonResponse<StoreDTO> response = new JsonResponse<StoreDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    StoreDTO objStoreDTO = new StoreDTO();

                    //StoreBO objStoreBO = StoreBusinessInstance.DisplayStoreProfile(storeID, userID);
                    //EntityMapper.Map(objStoreBO, objStoreDTO);
                    //if (objStoreBO.LastVisitDate.HasValue)
                    //{
                    //    objStoreDTO.LastVisitedDate = objStoreBO.LastVisitDate.Value.ToString("dd/MM/yyyy HH:mm:ss",
                    //            CultureInfo.InvariantCulture);
                    //}
                    //if (objStoreDTO.StoreID > 0)
                    //{
                    //    response.IsSuccess = true;
                    //    response.SingleResult = objStoreDTO;
                    //}
                    //else
                    //{
                    //    response.IsSuccess = false;
                    //    response.Message = "Invalid Store ID";
                    //}
                    objStoreDTO = StoreBusinessInstance.DisplayStoreProfile(storeID, userID).FirstOrDefault();

                    //if (objStoreDTO.StoreID > 0)
                    if (objStoreDTO != null)
                    {
                        response.IsSuccess = true;
                        response.SingleResult = objStoreDTO;
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Invalid Store ID";
                    }
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Updates the store profile.
        /// </summary>
        /// <param name="storeID">The store identifier.</param>
        /// <param name="contactPerson">The contact person.</param>
        /// <param name="mobileNo">The mobile no.</param>
        /// <param name="emailID">The email identifier.</param>
        /// <returns></returns>
        [UserSecureOperation]
        public JsonResponse<StoreDTO> UpdateStoreProfile(long userID, int storeID, string contactPerson, string mobileNo, string emailID, string imageName, string storeAddress)
        {
            JsonResponse<StoreDTO> response = new JsonResponse<StoreDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    StoreDTO storeDTO = new StoreDTO();
                    contactPerson = System.Web.HttpUtility.HtmlEncode(contactPerson);
                    mobileNo = System.Web.HttpUtility.HtmlEncode(mobileNo);
                    emailID = System.Web.HttpUtility.HtmlEncode(emailID);
                    imageName = System.Web.HttpUtility.HtmlEncode(imageName);
                    storeAddress = System.Web.HttpUtility.HtmlEncode(storeAddress);
                    StoreBO storeBO = StoreBusinessInstance.UpdateStoreProfile(storeID, contactPerson, mobileNo, emailID, imageName, storeAddress);
                    EntityMapper.Map(storeBO, storeDTO);
                    if (storeDTO != null)
                    {
                        response.IsSuccess = true;
                        response.Message = "Store details updated successfully";
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.Message = "Store details couldn't be updated";
                    }
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Method to get store schemes on the basis of start date in ascending order
        /// </summary>
        /// <param name="storeID">store primary key</param>
        /// <returns>returns scheme entity collection</returns>
        [UserSecureOperation]
        public JsonResponse<SchemeDTO> GetStoreSchemes(int storeID, long userID)
        {
            JsonResponse<SchemeDTO> response = new JsonResponse<SchemeDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.IsSuccess = true;
                    response.Result = StoreBusinessInstance.GetStoreSchemes(storeID).ToList();
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Gets the stores for today beat.
        /// </summary>
        /// <param name="userID">The user identifier.</param>

        /// <returns></returns>
        [UserSecureOperation]
        //public JsonResponse<UserStoresTodayBeatDTO> GetStoresForTodayBeat(long userID)
        public JsonResponse<StoreDTO> GetStoresForTodayBeat(long userID)
        {
            //JsonResponse<UserStoresTodayBeatDTO> response = new JsonResponse<UserStoresTodayBeatDTO>();
            JsonResponse<StoreDTO> response = new JsonResponse<StoreDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    //IList<UserStoresTodayBeatDTO> lstStores = StoreBusinessInstance.GetStoresForTodayBeat(userID);

                    ////EntityMapper.Map(lstStores, lstUserStores);
                    //response.Result = lstStores.ToList();

                    List<StoreDTO> lstStores = new List<StoreDTO>();
                    lstStores = StoreBusinessInstance.DisplayStoreProfile(null, userID);


                    response.Result = lstStores.ToList();
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Gets the user stores.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        [UserSecureOperation]
        public JsonResponse<UserStoresDTO> GetUserStores(long userID)
        {
            JsonResponse<UserStoresDTO> response = new JsonResponse<UserStoresDTO>();
            try
            {
                IList<UserStoresDTO> lstUserStores = new List<UserStoresDTO>();
                IList<UserStoresBO> lstStores = StoreBusinessInstance.GetUserStores(userID);
                EntityMapper.Map(lstStores, lstUserStores);
                response.Result = lstUserStores.ToList();
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Method to get selected outlet profile details
        /// </summary>
        /// <param name="userID">user primary ID</param>
        /// <param name="storeID">store ID</param>
        /// <returns>returns outlet entity instance</returns>
        [UserSecureOperation]
        public JsonResponse<OutletProfileDTO> GetOutletProfile(long userID, long storeID)
        {
            JsonResponse<OutletProfileDTO> response = new JsonResponse<OutletProfileDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    OutletProfileDTO profile = StoreBusinessInstance.GetOutletProfile(userID, storeID);
                    if (profile != null)
                    {
                        profile.ACH = "10%";
                        //profile.MTDPurchase = 100;
                        //profile.MTDSale = 200;
                        //profile.Target = 300;
                    }
                    response.SingleResult = profile;

                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        ///  Method will delete the Beat which has statusid 2
        /// </summary>
        /// <param name="statusID">represent 0 if rejected, 1 if approved</param>
        /// <returns>Return true if Beat is deleted ,false if not deleted</returns>
        [UserSecureOperation]
        public JsonResponse<bool> IsBeatDeleted(int statusID, long userID)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.Message = UserBeatInstance.DeleteUsersBeat(statusID);
                    response.IsSuccess = true;

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// User to send the Beat Details info to the business layer
        /// </summary>
        ///<param name="userID">Indicates the user id</param>
        ///<param name="userBeatCollection">Beat collection info of the user</param>
        /// <returns>return true if entry inserted else false</returns>
        [UserSecureOperation]
        public JsonResponse<bool> InsertUserBeatDetailsInfo(long userID, List<UserBeatDTO> userBeatCollection, string MarketOffDays, string CoverageType)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            string offDays = System.Web.HttpUtility.HtmlEncode(MarketOffDays);
            string coverageType = System.Web.HttpUtility.HtmlEncode(CoverageType);
            try
            {
                if (userBeatCollection != null && userBeatCollection.Count > 0)
                {
                    userBeatCollection.ForEach(k =>
                    {
                        k.MarketOffDays = offDays;
                        CoverageType = coverageType;
                    });
                    int status = UserBeatInstance.InsertUserBeatDetailsInfo(userID, userBeatCollection);
                    response.SingleResult = false;
                    switch (status)
                    {
                        case 1:
                            response.Message = Messages.BeatExecuted;
                            response.SingleResult = true;
                            break;
                        case -1:
                            string employeeName = UserBusinessInstance.GetSeniorName(userBeatCollection[0].UserID);
                            if (String.IsNullOrEmpty(employeeName))
                            {
                                employeeName = "Senior";
                            }
                            response.Message = String.Format("You have already submitted your beat which is pending for approval with {0}. In case you want to re-submit your beat request ask {1} to reject existing beat.", employeeName, employeeName);
                            break;
                        case -2:
                            response.Message = "Beat already submitted and approved for month.";
                            break;
                        default:
                            response.Message = Messages.BeatNotExecuted;
                            break;
                    }
                }
                else
                {
                    response.Message = Messages.BeatNotSelected;
                }
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Method to return Dealer list Based on city
        /// </summary>
        /// <param name="userID">userID</param>
        [UserSecureOperation]
        public JsonResponse<SyncDealerListDTO> DealersListBasedOnCity(long userID, long roleID, int RowCount, int StartRowIndex, string LastUpdatedDate)
        {
            JsonResponse<SyncDealerListDTO> response = new JsonResponse<SyncDealerListDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    bool HasMoreRows = false;
                    DateTime? MaxModifiedDateTime;                    
                    DateTime? LastUpdatedDateTime = null;
                    if (LastUpdatedDate != null)
                    {
                        LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }

                    SyncDealerListDTO output = new SyncDealerListDTO();
                    output.Result = StoreBusinessInstance.DealersListBasedOnCity(userID, roleID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
                    output.HasMoreRows = HasMoreRows;

                    #region convert date in dd/MM/yyyy format
                    if (MaxModifiedDateTime != null)
                        output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");                        
                    else
                        output.MaxModifiedDate = null;
                    
                    #endregion
                    
                    response.SingleResult = output;
                    response.IsSuccess = true;

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        ///  User to Get the Beat Details info on the basis of status id
        /// </summary>
        /// <param name="statusID">represent 0 if rejected, 1 if approved</param>
        /// <returns></returns>
        [UserSecureOperation]
        public JsonResponse<UserBeatDTO> GetBeatDetails(int statusID, long userID)
        {
            JsonResponse<UserBeatDTO> response = new JsonResponse<UserBeatDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.Result = UserBeatInstance.GetBeatDetails(statusID).ToList();
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Method to identify that is geo tag require or not
        /// </summary>
        /// <param name="storeID">store ID</param>
        /// <returns>returns boolean status</returns>
        [UserSecureOperation]
        public JsonResponse<bool> IsGeoTagRequired(int storeID, long userID)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.SingleResult = StoreBusinessInstance.IsGeoTagRequired(storeID);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        #region Planogram (Amit: 18 Sep 2014)
        /// <summary>
        ///  This function returns list of Planogram Products in the table
        /// </summary>
        /// <returns>returns list of Planogram Products</returns>
        [UserSecureOperation]
        public JsonResponse<SyncPlanogramProductMasterDTO> GetPlanogramProductMasters(int companyID, long userID, long roleID, int RowCount, int StartRowIndex, string LastUpdatedDate)
        {
            JsonResponse<SyncPlanogramProductMasterDTO> response = new JsonResponse<SyncPlanogramProductMasterDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    bool HasMoreRows = false;
                    DateTime? MaxModifiedDateTime;
                    DateTime? LastUpdatedDateTime = null;
                    if (LastUpdatedDate != null)
                    {
                        LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }

                    SyncPlanogramProductMasterDTO output = new SyncPlanogramProductMasterDTO();
                    output.Result = StoreBusinessInstance.GetPlanogramProductMasters(companyID, userID, roleID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
                    output.HasMoreRows = HasMoreRows;

                    #region convert date in dd/MM/yyyy format
                    if (MaxModifiedDateTime != null)
                        output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    else
                        output.MaxModifiedDate = null;

                    #endregion

                    response.SingleResult = output;
                    response.IsSuccess = true;

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        ///// <summary>
        /////  This function returns list of Planogram Products in the table
        ///// </summary>
        ///// <returns>returns list of Planogram Products</returns>
        //[UserSecureOperation]
        //public JsonResponse<PlanogramProductMasterOutputDTO> GetPlanogramProductMasters(int companyID, long userID, long roleID, int PlanogramProductMasterID, int rowcounter)
        //{
        //    JsonResponse<PlanogramProductMasterOutputDTO> response = new JsonResponse<PlanogramProductMasterOutputDTO>();
        //    try
        //    {
        //        ExceptionEngine.AppExceptionManager.Process(() =>
        //        {
        //            int totalRow = 0;
        //            PlanogramProductMasterOutputDTO output = new PlanogramProductMasterOutputDTO();
        //            output.PlanogramProductMasterList = StoreBusinessInstance.GetPlanogramProductMasters(companyID, userID, roleID, PlanogramProductMasterID, rowcounter, out totalRow);
        //            output.TotalRow = totalRow;

        //            //response.Result = StoreBusinessInstance.GetPlanogramProductMasters(companyID, userID, roleID, PlanogramProductMasterID, rowcounter);
        //            response.SingleResult = output;

        //            response.IsSuccess = true;

        //        }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}

        /// <summary>
        ///  This function returns list of Planogram Classes in the table
        /// </summary>
        /// <returns>returns list of Planogram Classes</returns>
        [UserSecureOperation]
        public JsonResponse<SyncPlanogramClassMasterDTO> GetPlanogramClassMasters(int companyID, long userID, long roleID, int RowCount, int StartRowIndex, string LastUpdatedDate)
        {

            JsonResponse<SyncPlanogramClassMasterDTO> response = new JsonResponse<SyncPlanogramClassMasterDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    bool HasMoreRows = false;
                    DateTime? MaxModifiedDateTime;
                    DateTime? LastUpdatedDateTime = null;
                    if (LastUpdatedDate != null)
                    {
                        LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }

                    SyncPlanogramClassMasterDTO output = new SyncPlanogramClassMasterDTO();
                    output.Result = StoreBusinessInstance.GetPlanogramClassMasters(companyID, userID, roleID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
                    output.HasMoreRows = HasMoreRows;

                    #region convert date in dd/MM/yyyy format
                    if (MaxModifiedDateTime != null)
                        output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    else
                        output.MaxModifiedDate = null;

                    #endregion

                    response.SingleResult = output;
                    response.IsSuccess = true;


                    //response.Result = StoreBusinessInstance.GetPlanogramClassMasters(companyID, userID, roleID);

                    //response.IsSuccess = true;

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }


        //Added by Vaishali on 18 Sep 2014 to Submit Planogram Response from APK
        /// <summary>
        /// Submit Planogram Response from APK
        /// </summary>
        /// <param name="PlanogramResponse"></param>
        /// <returns>success or Failure</returns>
        [UserSecureOperation]
        public JsonResponse<bool> SubmitPlanogram(List<PlanogramResponseDTO> PlanogramResponse, int companyID, long userID, long roleID)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.SingleResult = StoreBusinessInstance.SubmitPlanogramResponse(PlanogramResponse, companyID, userID, roleID);
                    response.IsSuccess = true;

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion

        ///// <summary>
        /////  This function returns mapping between compititor and compprofuctgroup
        ///// </summary>
        ///// <returns>returns list of mapped data</returns>
        //[UserSecureOperation]
        //public JsonResponse<CompetitorProductGroupMappingDTO> GetCompetitorProductGroupMapping(int companyID, long userID, long roleID)
        //{

        //    JsonResponse<CompetitorProductGroupMappingDTO> response = new JsonResponse<CompetitorProductGroupMappingDTO>();
        //    try
        //    {
        //        ExceptionEngine.AppExceptionManager.Process(() =>
        //        {
        //            response.Result = StoreBusinessInstance.GetCompetitorProductGroupMapping(userID);
        //            response.IsSuccess = true;

        //        }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}

        /// <summary>
        ///  This function returns mapping between compititor and compprofuctgroup
        /// </summary>
        /// <returns>returns list of mapped data</returns>
        [UserSecureOperation]
        public JsonResponse<SyncCompetitorProductGroupMappingDTO> GetCompetitorProductGroupMapping(int companyID, long userID, long roleID, int RowCount, int StartRowIndex, string LastUpdatedDate)
        {
            JsonResponse<SyncCompetitorProductGroupMappingDTO> response = new JsonResponse<SyncCompetitorProductGroupMappingDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    bool HasMoreRows = false;
                    DateTime? MaxModifiedDateTime;
                    DateTime? LastUpdatedDateTime = null;
                    if (LastUpdatedDate != null)
                    {
                        LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }

                    SyncCompetitorProductGroupMappingDTO output = new SyncCompetitorProductGroupMappingDTO();
                    output.Result = StoreBusinessInstance.GetCompetitorProductGroupMapping(companyID, userID, roleID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
                    output.HasMoreRows = HasMoreRows;

                    #region convert date in dd/MM/yyyy format
                    if (MaxModifiedDateTime != null)
                        output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
                    else
                        output.MaxModifiedDate = null;

                    #endregion

                    response.SingleResult = output;
                    response.IsSuccess = true;

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Upload Geo tag Image through Stream 
        /// </summary>
        /// <param name="image">geo image stream</param>
        /// <returns>returns image name if successfully uploaded else blank</returns>
        //[SecureOperation]
        public JsonResponse<string> UploadGeoImageStream(Stream image)
        {
            JsonResponse<string> response = new JsonResponse<string>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    var parser = new MultipartFormDataParser(image);

                    var StoreID = Convert.ToInt32(parser.Parameters["StoreID"].Data);
                    var Latitude = Convert.ToString(parser.Parameters["Latitude"].Data);
                    var Longitude = Convert.ToString(parser.Parameters["Longitude"].Data);
                    var UserID = Convert.ToInt64(parser.Parameters["userid"].Data);
                    var UserOption = parser.Parameters["UserOption"].Data;

                    bool? UserOptionBool = null;
                    if (UserOption != null && UserOption!="null")
                    {
                        UserOptionBool = Convert.ToBoolean(UserOption);
                    }
                    // Files are stored in a list:
                    //var file = parser.Files.First();
                    // Loop through all the files
                    int counter = 0;
                    string fileDirectory = string.Empty;
                    fileDirectory = AppUtil.GetUploadDirectory(AspectEnums.ImageFileTypes.Store);
                    string newFileName = null;
                    foreach (var file in parser.Files)
                    {
                        string filename = file.FileName;
                        if (Directory.Exists(fileDirectory))
                        {
                            FileStream fileData = null;
                            newFileName = AppUtil.GetUniqueKey().ToUpper() + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + counter.ToString() + ".jpeg";
                            string uploadedFileName = fileDirectory + @"\" + newFileName;

                            #region Step 1: Save Image
                            //fileData = new FileStream(file.Data);
                            using (fileData = new FileStream(uploadedFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                file.Data.CopyTo(fileData);
                                // fileData.Close();
                                file.Data.Close();
                                //fileData.Write(file.Data);

                            }

                            #endregion

                           


                        }


                        //  response.SingleResult = newFileName;
                    }
                            #region Step 2: Save the Content
                    StoreGeoTagBO storeGeoTagBO = new StoreGeoTagBO()
                            {
                        StoreID = StoreID,
                        UserID = UserID,
                        Lattitude = Latitude,
                        Longitude = Longitude,
                        PictureFileName = newFileName,
                               UserOption = UserOptionBool

                            };
                            int dataCount = StoreBusinessInstance.SubmitStoreGeoTag(storeGeoTagBO);
                            if (dataCount > 0)
                            {
                                response.IsSuccess = true;
                            }

                            #endregion


                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// Gets the dealer Creation.SDHHP-6114
        /// Added by Gourav Vishnoi on 31 July 2015
        /// </summary>
        #region Dealer Creation Form in SD and AX Service SDHHP-6114
        [UserSecureOperation]
        /// <summary>
        /// GetMDMDistrict
        /// </summary>
        /// <returns></returns>
        public JsonResponse<GetDistrictDealerCreationDTO> GetMDMDistrict(int userID)
        {
            JsonResponse<GetDistrictDealerCreationDTO> response = new JsonResponse<GetDistrictDealerCreationDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.Result = StoreBusinessInstance.GetMDMDistrict(userID);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        [UserSecureOperation]
        /// <summary>
        /// GetMDMCity
        /// </summary>
        /// <param name="district"></param>
        /// <returns></returns>
        public JsonResponse<GetCityDealerCreationDTO> GetMDMCity(string district)
        {
            JsonResponse<GetCityDealerCreationDTO> response = new JsonResponse<GetCityDealerCreationDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    district = System.Web.HttpUtility.HtmlEncode(district);
                    response.Result = StoreBusinessInstance.GetMDMCity(district);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        //[UserSecureOperation]
        /// <summary>
        /// GetMDMPinCode
        /// </summary>
        /// <param name="district"></param>
        /// <param name="city"></param>
        /// <returns></returns>
        //public JsonResponse<string> GetMDMPinCode(string district, string city)
        //{
        //    JsonResponse<string> response = new JsonResponse<string>();
        //    try
        //    {
        //        ExceptionEngine.AppExceptionManager.Process(() =>
        //        {
        //            response.Result = StoreBusinessInstance.GetMDMPinCode(district, city);
        //            response.IsSuccess = true;
        //        }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = ex.Message;
        //    }
        //    return response;
        //}

        [UserSecureOperation]
        /// <summary>
        /// GetMDMParentDealerCode
        /// </summary>
        /// <returns></returns>
        public JsonResponse<string> GetMDMParentDealerCode()
        {
            JsonResponse<string> response = new JsonResponse<string>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                 //   response.Result = StoreBusinessInstance.GetMDMParentDealerCode();
                    response.Result = MDMServiceInstance.GetMDMParentDealerCode();
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        [UserSecureOperation]
        /// <summary>
        /// GetMDMDuplicateCheck
        /// </summary>
        /// <param name="FirmMobile"></param>
        /// <param name="FirmEmail"></param>
        /// <param name="Pan"></param>
        /// <param name="Tin"></param>
        /// <param name="Ischild"></param>
        /// <returns></returns>
        public JsonResponse<bool> GetMDMDuplicateCheck(string FirmMobile, string FirmEmail, string Pan, string Tin, bool Ischild)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    DuplicateDealerCheckBO duplicateDealer = new DuplicateDealerCheckBO();
                    duplicateDealer.FirmMobile = System.Web.HttpUtility.HtmlEncode(FirmMobile);
                    duplicateDealer.FirmEmail = System.Web.HttpUtility.HtmlEncode(FirmEmail);
                    if (Ischild == false)
                    {
                        duplicateDealer.PAN = System.Web.HttpUtility.HtmlEncode(Pan);
                        duplicateDealer.TIN = System.Web.HttpUtility.HtmlEncode(Tin);
                    }
                    response.SingleResult = MDMServiceInstance.IsDuplicateDealer(duplicateDealer, Ischild);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        [UserSecureOperation]
        /// <summary>
        /// SubmitMDMDealerCreation
        /// </summary>
        /// <param name="dealerCreation"></param>
        /// <param name="emplCode"></param>
        /// <returns></returns>
        public JsonResponse<int> SubmitMDMDealerCreation(DealerCreationDTO dealerCreation, string emplCode)
        {
            JsonResponse<int> response = new JsonResponse<int>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    DealerCreationBO dealer = new DealerCreationBO();
                    EntityMapper.Map(dealerCreation, dealer);
                    emplCode = System.Web.HttpUtility.HtmlEncode(emplCode);
                    var dealerCreationID = StoreBusinessInstance.SubmitMDMDealerCreation(dealer, emplCode);
                    response.SingleResult = dealerCreationID;

                    if (response.SingleResult > 0)
                    {
                        response.IsSuccess = true;
                        response.Message = "Dealer created successfully.";
                    }
                    else
                        response.IsSuccess = false;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;



        }

        /// <summary>
        /// UploadMDMDealerCreationImage
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public JsonResponse<string> UploadMDMDealerCreationImage(Stream image)
        {
            JsonResponse<string> response = new JsonResponse<string>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    var parser = new MultipartFormDataParser(image);
                    string apiKey = parser.Parameters["APIKey"].Data;
                    string apiToken = parser.Parameters["APIToken"].Data;
                    string userid = parser.Parameters["userid"].Data;
                    DealerCreationBO dealer = new DealerCreationBO();
                    int dealerCreationID = Convert.ToInt32(parser.Parameters["dealerCreationID"].Data);

                    MDMDealerCreationBO dealerMDM = new MDMDealerCreationBO();
                    bool isValid = SystemBusinessInstance.IsValidServiceUser(apiKey, apiToken, userid);
                    if (isValid)
                    {
                        FileStream fileData = null;
                        MemoryStream ms = null;
                        int counter = 0;
                        string fileDirectory = AppUtil.GetUploadDirectory(AspectEnums.ImageFileTypes.DealerCreation);
                   
                        foreach (var file in parser.Files)
                        {
                            string filename = file.FileName;
                            if (Directory.Exists(fileDirectory))
                            {
                                string newFileName = AppUtil.GetUniqueKey().ToUpper() + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + counter.ToString() + ".jpeg";
                                string uploadedFileName = fileDirectory + @"\" + newFileName;
                                #region Step 1: Save Image
                                byte[] arrBite;
                                using (ms = new MemoryStream())
                                {
                                    file.Data.CopyTo(ms);
                                    arrBite = ms.ToArray();
                                    if (MimeType.GetMimeType(arrBite, filename))
                                    {
                                        using (fileData = new FileStream(uploadedFileName, FileMode.Create, FileAccess.Write, FileShare.None))
                                        {
                                            ms.Position = 0;
                                            if (ms.Length != 0)
                                                ms.CopyTo(fileData);
                                            file.Data.Close();
                                            if (ms != null)
                                            {
                                                ms.Close();
                                                ms.Dispose();
                                            }
                                        }
                                        switch (filename)
                                        {
                                            case "ContactPersonPhoto": dealer.CONTACTPERSONPHOTO = newFileName; dealerMDM.ByteCONTACTPERSONPHOTO = arrBite; break;
                                            case "GSBPhoto": dealer.GSBPHOTO = newFileName; dealerMDM.ByteGSBPHOTO = arrBite; break;
                                            case "OwnerPhoto": dealer.OWNERPHOTO = newFileName; dealerMDM.ByteOWNERPHOTO = arrBite; break;
                                            case "PanPhoto": dealer.PANPHOTO = newFileName; dealerMDM.BytePANPHOTO = arrBite; break;
                                            case "Tinphoto": dealer.TINPHOTO = newFileName; dealerMDM.ByteTINPHOTO = arrBite; break;
                                        }
                                    }
                                    else
                                    {
                                        file.Data.Close();
                                        if (ms != null)
                                        {
                                            ms.Close();
                                            ms.Dispose();
                                        }
                                        response.Message = "Not a valid image type";
                                        return;
                                        //throw new System.Security.SecurityException("Not a valid image type");
                                    }
                                }
                                #endregion
                            }
                        }
                        #region Step 2: Save the Content
                        response.IsSuccess = StoreBusinessInstance.PhotoMDMDealerCreation(dealer, dealerCreationID);
                        //var dealerDBData = StoreBusinessInstance.GetDealerCreationData(dealerCreationID);
                        //dealerDBData.ByteCONTACTPERSONPHOTO = dealerMDM.ByteCONTACTPERSONPHOTO;
                        #region Filldata for dealercreation request in MDM
                        var tempCONTACTPERSONPHOTO = dealerMDM.ByteCONTACTPERSONPHOTO;
                        var tempGSBPHOTO = dealerMDM.ByteGSBPHOTO;
                        var tempOWNERPHOTO = dealerMDM.ByteOWNERPHOTO;
                        var tempPANPHOTO = dealerMDM.BytePANPHOTO;
                        var tempTINPHOTO = dealerMDM.ByteTINPHOTO;

                        EntityMapper.Map(StoreBusinessInstance.GetDealerCreationData(dealerCreationID), dealerMDM);
                        dealerMDM.ByteCONTACTPERSONPHOTO = tempCONTACTPERSONPHOTO;
                        dealerMDM.ByteGSBPHOTO = tempGSBPHOTO;
                        dealerMDM.ByteOWNERPHOTO = tempOWNERPHOTO;
                        dealerMDM.BytePANPHOTO = tempPANPHOTO;
                        dealerMDM.ByteTINPHOTO = tempTINPHOTO;
                        MDMServiceInstance.CreateDealer(dealerMDM);
                        #endregion
                        #endregion
                    }
                    else
                        throw new System.Security.SecurityException(Messages.ApiAccessDenied);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
        [UserSecureOperation]
        /// <summary>
        /// GetMDMDealerHistory
        /// </summary>
        /// <param name="emplCode"></param>
        /// <returns></returns>
        public JsonResponse<DealerCreationDTO> GetMDMDealerHistory(string emplCode)
        {
            JsonResponse<DealerCreationDTO> response = new JsonResponse<DealerCreationDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    List<DealerCreationDTO> dealer = new List<DealerCreationDTO>();
                    emplCode = System.Web.HttpUtility.HtmlEncode(emplCode);
                    EntityMapper.Map(MDMServiceInstance.DealerHistory(emplCode), dealer);
                    response.Result = dealer;
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }


        //Added By Gourav for varyfying uploaded files from  service.
        /// <summary>
        /// MimeType
        /// </summary>
        public class MimeType
        {
            private static readonly byte[] JPG = { 255, 216, 255 };
            private static readonly byte[] PNG = { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82 };
            public static bool GetMimeType(byte[] file, string fileName)
            {
                bool flag = false;
                //Ensure that the filename isn't empty or null
                if (string.IsNullOrWhiteSpace(fileName))
                    return flag;
                //Get the MIME Type
                if (file.Take(3).SequenceEqual(JPG))
                {
                    flag = true;
                    return flag;
                }
                else if (file.Take(16).SequenceEqual(PNG))
                {
                    flag = true;
                    return flag;
                }
                return flag;
            }
        }
        #endregion



    }
}
