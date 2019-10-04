using Samsung.SmartDost.BusinessLayer.Services.BO;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.CommonLayer.Aspects.Exceptions;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using Samsung.SmartDost.CommonLayer.Resources;
using Samsung.SmartDost.PresentationLayer.ServiceImpl.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Samsung.SmartDost.PresentationLayer.ServiceImpl
{
    /// <summary>
    /// This partial class purpose is to implement the system related service methods
    /// </summary>
    public partial class SmartDost : BaseService
    {
        /// <summary>
        /// Method to save store survey response on the basis of coverage beat
        /// </summary>
        /// <param name="storeSurvey">store survey</param>
        /// <returns>returns status</returns>
       // [UserSecureOperation]
       // public JsonResponse<long> SaveStoreSurveyResponse(SurveyResponseDTO storeSurvey)
       // {
       //     JsonResponse<long> response = new JsonResponse<long>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             response.SingleResult = ActivityBusinessInstance.SaveStoreSurveyResponse(storeSurvey);
       //             response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;

       // }

       // /// <summary>
       // /// Method to save user activities on the basis of store survey data
       // /// </summary>
       // /// <param name="activities">activities performed</param>
       // /// <returns>returns status</returns>
       // [UserSecureOperation]
       // public JsonResponse<int> SaveSurveyUserResponse(IList<SurveyUserResponseDTO> activities, long userID)
       // {
       //     JsonResponse<int> response = new JsonResponse<int>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //        {
       //            response.SingleResult = ActivityBusinessInstance.SaveSurveyUserResponse(activities, userID,false);
       //            response.IsSuccess = true;
       //            response.Message = "Data submitted successfully.";
       //        }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // /// <summary>
       // /// Method to get survey questions on the basis of user profile selected
       // /// </summary>
       // /// <param name="userRoleID">user profile ID</param>
       // /// <param name="userID">user primary ID</param>
       // /// <returns>returns questions list</returns>
       // [UserSecureOperation]
       // public JsonResponse<SyncSurveyModuleDTO> GetSurveyQuestions(long userRoleID, long userID, int RowCount, int StartRowIndex, string LastUpdatedDate)
       // {
       //     JsonResponse<SyncSurveyModuleDTO> response = new JsonResponse<SyncSurveyModuleDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             bool HasMoreRows = false;
       //             DateTime? MaxModifiedDateTime;
       //             DateTime? LastUpdatedDateTime = null;
       //             if (LastUpdatedDate != null)
       //             {
       //                 LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
       //             }



       //             SyncSurveyModuleDTO output = new SyncSurveyModuleDTO();
       //             output.Result = ActivityBusinessInstance.GetSurveyQuestions(userRoleID, userID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
       //             output.HasMoreRows = HasMoreRows;

       //             #region convert date in dd/MM/yyyy format
       //             if (MaxModifiedDateTime != null)
       //                 output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
       //             else
       //                 output.MaxModifiedDate = null;

       //             #endregion

       //             response.SingleResult = output;
       //             response.IsSuccess = true;


       //             //response.Result = ActivityBusinessInstance.GetSurveyQuestions(userRoleID, userID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();

       //             response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // /// <summary>
       // /// Method to get survey partner questions on the basis of user profile selected
       // /// </summary>
       // /// <param name="userRoleID">user profile ID</param>
       // /// <param name="userID">user primary ID</param>
       // /// <returns>returns questions list</returns>
       // [UserSecureOperation]
       // public JsonResponse<SurveyModuleDTO> GetSurveyPartnerQuestions(long userRoleID, long userID)
       // {
       //     JsonResponse<SurveyModuleDTO> response = new JsonResponse<SurveyModuleDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             response.Result = ActivityBusinessInstance.GetSurveyPartnerQuestions(userRoleID, userID).ToList();
       //             response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // /// <summary>
       // /// Method to get survey questions attributes
       // /// </summary>
       // /// <returns>returns questions attribute list</returns>
       // [UserSecureOperation]
       // public JsonResponse<SurveyQuestionAttributeDTO> GetSurveyQuestionAttributes(long userID)
       // {
       //     JsonResponse<SurveyQuestionAttributeDTO> response = new JsonResponse<SurveyQuestionAttributeDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             response.Result = ActivityBusinessInstance.GetSurveyQuestionAttributes().ToList();
       //             response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // /// <summary>
       // /// Method to submit competition booked in survey
       // /// </summary>
       // /// <param name="competitions">competition booked</param>
       // /// <returns>returns boolean response</returns>
       // [UserSecureOperation]
       // public JsonResponse<long> SubmitCompetitionBooked(IList<CompetitionSurveyDTO> competitions)
       // {
       //     JsonResponse<long> response = new JsonResponse<long>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             response.SingleResult = ActivityBusinessInstance.SubmitCompetitionBooked(competitions);
       //             response.IsSuccess = true;
       //             if (response.SingleResult == 1)
       //             {
       //                 response.Message = "Booked Competition Data saved successfully.";
       //             }
       //             response.StatusCode = response.SingleResult.ToString().PadLeft(3, '0');
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // /// <summary>
       // /// Method to submit competition booked in survey
       // /// </summary>
       // /// <param name="competitions">competition booked</param>
       // /// <returns>returns boolean response</returns>
       // [UserSecureOperation]
       // public JsonResponse<long> SubmitCollectionSurvey(IList<CollectionSurveyDTO> collection)
       // {
       //     JsonResponse<long> response = new JsonResponse<long>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             response.SingleResult = ActivityBusinessInstance.SubmitCollectionSurvey(collection);
       //             response.Message = "Collection Data saved successfully.";
       //             response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // /// <summary>
       // /// Method to submit order booking
       // /// </summary>
       // /// <param name="orders">order survey collection</param>
       // /// <returns>returns response</returns>
       // [UserSecureOperation]
       // public JsonResponse<int> SubmitOrderBooking(IList<OrderBookingSurveyDTO> orders)
       // {
       //     JsonResponse<int> response = new JsonResponse<int>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             response.SingleResult = ActivityBusinessInstance.SubmitOrderBooking(orders);
       //             response.IsSuccess = true;
       //             response.Message = Messages.Updated;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // /// <summary>
       // /// Method to fetch competition group list from database
       // /// </summary>
       // /// <param name="companyID">company ID</param>
       // /// <returns>returns product group list</returns>
       //[UserSecureOperation]
       // public JsonResponse<SyncCompProductGroupDTO> GetCompetitionProductGroup(int companyID, long userID, int RowCount, int StartRowIndex, string LastUpdatedDate)
       // {
       //     JsonResponse<SyncCompProductGroupDTO> response = new JsonResponse<SyncCompProductGroupDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             bool HasMoreRows = false;
       //             DateTime? MaxModifiedDateTime;
       //             DateTime? LastUpdatedDateTime = null;
       //             if (LastUpdatedDate != null)
       //             {
       //                 LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
       //             }

       //             SyncCompProductGroupDTO output = new SyncCompProductGroupDTO();

       //             //response.Result = ActivityBusinessInstance.GetCompetitionProductGroup(companyID,userID).ToList();

       //             output.Result = ActivityBusinessInstance.GetCompetitionProductGroup(companyID,userID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
       //             output.HasMoreRows = HasMoreRows;

       //             #region convert date in dd/MM/yyyy format
       //             if (MaxModifiedDateTime != null)
       //                 output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
       //             else
       //                 output.MaxModifiedDate = null;

       //             #endregion

       //             response.SingleResult = output;
                    

       //             response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // /// <summary>
       // /// Method to submir partner meeting details
       // /// </summary>
       // /// <param name="partnerEntity">partner entity</param>
       // /// <returns>returns boolean status</returns>
       // [UserSecureOperation]
       // public JsonResponse<bool> SubmitPartnerMeeting(PartnerMeetingDTO partnerEntity)
       // {
       //     JsonResponse<bool> response = new JsonResponse<bool>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             response.SingleResult = ActivityBusinessInstance.SubmitPartnerMeeting(partnerEntity);
       //             response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // /// <summary>
       // /// Method to fetch report dashboard URL of the application
       // /// </summary>
       // /// <returns>returns dashboard url</returns>
       // [UserSecureOperation]
       // public JsonResponse<CoverageUserDTO> GetCoverageUsers(long userID)
       // {
       //     JsonResponse<CoverageUserDTO> response = new JsonResponse<CoverageUserDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             response.Result = UserBeatInstance.GetCoverageUsers(userID).ToList();
       //             response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // /// <summary>
       // /// Method to update pending user's coverage
       // /// </summary>
       // /// <param name="userIDList">user ID list</param>
       // /// <param name="status">status</param>
       // /// <returns>returns boolean status</returns>
       // [UserSecureOperation]
       // public JsonResponse<bool> UpdatePendingCoverage(long userID,List<long> userIDList, int status)
       // {
       //     JsonResponse<bool> response = new JsonResponse<bool>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             response.SingleResult = UserBeatInstance.UpdatePendingCoverage(userIDList, status);
       //             response.IsSuccess = true;
       //             if (status == 1)
       //                 response.Message = Messages.BeatApproved;
       //             else
       //                 response.Message = Messages.BeatRejected;

       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.IsSuccess = false;
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // /// <summary>
       // /// Method to fetch user beat details
       // /// </summary>
       // /// <param name="userID">user ID</param>
       // /// <returns>returns beat details</returns>
       // [UserSecureOperation]
       // public JsonResponse<UserBeatDetailsDTO> GetUserBeatDetails(long userID)
       // {
       //     JsonResponse<UserBeatDetailsDTO> response = new JsonResponse<UserBeatDetailsDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             response.SingleResult = UserBeatInstance.GetUserBeatDetails(userID); 
       //             response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // [UserSecureOperation]
       // public JsonResponse<UserBeatDetailsDTO> GetApprovedUserBeatDetails(long userID)
       // {
       //     JsonResponse<UserBeatDetailsDTO> response = new JsonResponse<UserBeatDetailsDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             response.SingleResult = UserBeatInstance.GetApprovedUserBeatDetails(userID);
       //             response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // /// <summary>
       // /// Method to get user territory 
       // /// </summary>
       // /// <param name="userID">user primary ID</param>
       // /// <returns>returns territory list</returns>
       // [UserSecureOperation]
       // public JsonResponse<MyTerritoryDTO> GetMyTerritory(long userID)
       // {
       //     JsonResponse<MyTerritoryDTO> response = new JsonResponse<MyTerritoryDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             response.Result = ActivityBusinessInstance.GetMyTerritory(userID).ToList();
       //             response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // /// <summary>
       // /// Get Rule Book Data (Cobined data from 3 tables ApproverTypeMaster, ActivityMaster, ActivityApporverMaster)
       // /// </summary>
       // /// <param name="userID">user primary ID</param>
       // /// <returns>returns rulebook object containing list of ApproverTypeMaster, ActivityMaster, ActivityApporverMaster</returns>
       // [UserSecureOperation]  
       // public JsonResponse<RuleBookDTO> GetRuleBook(long userID)
       // {
       //     JsonResponse<RuleBookDTO> response = new JsonResponse<RuleBookDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             response.SingleResult = ActivityBusinessInstance.GetRuleBook(userID);
       //             response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());

       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }


      
       // //[UserSecureOperation]
       // public JsonResponse<bool> SaveQuestionImages(long userID, int roleID, int storeID, string Image)
       // //public JsonResponse<bool> SaveQuestionImages(Stream Image)
       // {            
       //     JsonResponse<bool> response = new JsonResponse<bool>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             response.SingleResult = ActivityBusinessInstance.SaveQuestionImages(userID, roleID, storeID, Image);
       //             //response.SingleResult = ActivityBusinessInstance.SaveQuestionImages(Image);
       //             response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;

       // }
       // /// <summary>
       // /// Method to generate survey responses
       // /// </summary>
       // /// <param name="storeSurvey"></param>
       // /// <returns></returns>
       // [UserSecureOperation]
       // public JsonResponse<SurveyResponseDTO> SaveStoreSurveyResponses(List<SurveyResponseDTO> storeSurvey)
       // {
       //     JsonResponse<SurveyResponseDTO> response = new JsonResponse<SurveyResponseDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             List<SurveyResponseDTO> surveyResponseList = new List<SurveyResponseDTO>();
       //             foreach (var item in storeSurvey)
       //             {
       //                 item.Lattitude = null;
       //                 item.Longitude = null;
       //                 item.RaceProfile = item.RaceProfile;//Remove hardcoding
       //                 SurveyResponseDTO surveyDTO = new SurveyResponseDTO();
       //                 surveyDTO.UserID = item.UserID;
       //                 surveyDTO.StoreID = item.StoreID;
       //                 item.AssessmentStartTime = Convert.ToDateTime(item.strAssesmentStartTime);
       //                 item.AssessmentEndTime = Convert.ToDateTime(item.strAssesmentEndTime);
       //                 surveyDTO.AssessmentStartTime = item.AssessmentStartTime;
       //                 surveyDTO.AssessmentEndTime = item.AssessmentEndTime;
       //                 surveyDTO.SurveyResponseID = ActivityBusinessInstance.SaveStoreSurveyResponse(item);
       //                 surveyResponseList.Add(surveyDTO);
       //             }
       //             response.Result = surveyResponseList;
       //             response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }
       //     return response;

       // }

       // #region RACE
       // [UserSecureOperation]
       // public JsonResponse<RACEMastersDTO> GetRaceMasters(long userID, int roleID)
       // {
       //          JsonResponse<RACEMastersDTO> response = new JsonResponse<RACEMastersDTO>();
       //          try
       //          {
       //              ExceptionEngine.AppExceptionManager.Process(() =>
       //              {
       //                  response.SingleResult = ActivityBusinessInstance.GetRaceMasters(userID, roleID);
       //                  response.IsSuccess = true;
       //              }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //          }
       //          catch(Exception ex)
       //          {
       //              response.IsSuccess = false;
       //              response.Message = ex.Message;
       //          }

       //          return response;

       // }

        
       // //[UserSecureOperation]
       // //public JsonResponse<RaceProductMasterOutputDTO> GetRaceProductMasters(long userID, int roleID, int LastProductID, int rowcounter)
       // //{
       // //    JsonResponse<RaceProductMasterOutputDTO> response = new JsonResponse<RaceProductMasterOutputDTO>();
       // //    try
       // //    {
       // //        ExceptionEngine.AppExceptionManager.Process(() =>
       // //        {
       // //            response.SingleResult = ActivityBusinessInstance.GetRaceProductMasters(userID, roleID, LastProductID, rowcounter);                    
       // //            response.IsSuccess = true;
       // //        }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       // //    }
       // //    catch (Exception ex)
       // //    {
       // //        response.IsSuccess = false;
       // //        response.Message = ex.Message;
       // //    }

       // //    return response;

       // //}

       // [UserSecureOperation]
       // public JsonResponse<RaceProductMasterOutputDTO> GetRaceProductMasters(long userID, int roleID, int RowCount, int StartRowIndex, string LastUpdatedDate)
       // {
       //     JsonResponse<RaceProductMasterOutputDTO> response = new JsonResponse<RaceProductMasterOutputDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             bool HasMoreRows = false;
       //             DateTime? MaxModifiedDateTime;
       //             DateTime? LastUpdatedDateTime = null;
       //             if (LastUpdatedDate != null)
       //             {
       //                 LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
       //             }

       //             RaceProductMasterOutputDTO output = new RaceProductMasterOutputDTO();
       //             output.Products = ActivityBusinessInstance.GetRaceProductMasters(userID, roleID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
       //             output.HasMoreRows = HasMoreRows;

       //             #region convert date in dd/MM/yyyy format
       //             if (MaxModifiedDateTime != null)
       //                 output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
       //             else
       //                 output.MaxModifiedDate = null;

       //             #endregion

       //             response.SingleResult = output;
       //             response.IsSuccess = true;
       //             //response.SingleResult = ActivityBusinessInstance.GetRaceProductMasters(userID, roleID, LastProductID, rowcounter);
       //             //response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.IsSuccess = false;
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // [UserSecureOperation]
       // public JsonResponse<bool> SubmitAuditResponse(long userID, int roleID, long SurveyResponseID, StockAuditDTO auditResponse)
       // {
       //     JsonResponse<bool> response = new JsonResponse<bool>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             response.SingleResult = ActivityBusinessInstance.SubmitAuditResponse(userID, roleID,SurveyResponseID, auditResponse);
       //             response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.IsSuccess = false;
       //         response.Message = ex.Message;
       //     }

       //     return response;

       // }

       // #endregion


       // #region Race services for sync adaptor
       // [UserSecureOperation]
       // public JsonResponse<SyncRacePOSMMasterDTO> GetRACEPOSMMaster(long userID, int roleID, int RowCount, int StartRowIndex, string LastUpdatedDate)
       // {
       //     JsonResponse<SyncRacePOSMMasterDTO> response = new JsonResponse<SyncRacePOSMMasterDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             bool HasMoreRows = false;
       //             DateTime? MaxModifiedDateTime = null;
       //             DateTime? LastUpdatedDateTime = null;

       //             if (LastUpdatedDate != null)
       //             {
       //                 LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
       //             }

       //             SyncRacePOSMMasterDTO output = new SyncRacePOSMMasterDTO();
       //             output.Result = ActivityBusinessInstance.GetRACEPOSMMaster(userID, roleID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
       //             output.HasMoreRows = HasMoreRows;

       //             #region convert date in dd/MM/yyyy format
       //             if (MaxModifiedDateTime != null)
       //                 output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
       //             else
       //                 output.MaxModifiedDate = null;

       //             #endregion

       //             response.SingleResult = output;
       //             response.IsSuccess = true;


       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }

       //     return response;
       // }



       // [UserSecureOperation]
       // public JsonResponse<SyncRaceFixtureMasterDTO> GetRACEFixtureMaster(long userID, int roleID, int RowCount, int StartRowIndex, string LastUpdatedDate)
       // {
       //     JsonResponse<SyncRaceFixtureMasterDTO> response = new JsonResponse<SyncRaceFixtureMasterDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             bool HasMoreRows = false;
       //             DateTime? MaxModifiedDateTime = null;
       //             DateTime? LastUpdatedDateTime = null;

       //             if (LastUpdatedDate != null)
       //             {
       //                 LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
       //             }

       //             SyncRaceFixtureMasterDTO output = new SyncRaceFixtureMasterDTO();
       //             output.Result = ActivityBusinessInstance.GetRACEFixtureMaster(userID, roleID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
       //             output.HasMoreRows = HasMoreRows;

       //             #region convert date in dd/MM/yyyy format
       //             if (MaxModifiedDateTime != null)
       //                 output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
       //             else
       //                 output.MaxModifiedDate = null;

       //             #endregion

       //             response.SingleResult = output;
       //             response.IsSuccess = true;


       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }

       //     return response;
       // }




       // [UserSecureOperation]
       // public JsonResponse<SyncRacePOSMProductMappingDTO> GetRACEPOSMProductMapping(long userID, int roleID, int RowCount, int StartRowIndex, string LastUpdatedDate)
       // {
       //     JsonResponse<SyncRacePOSMProductMappingDTO> response = new JsonResponse<SyncRacePOSMProductMappingDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             bool HasMoreRows = false;
       //             DateTime? MaxModifiedDateTime = null;
       //             DateTime? LastUpdatedDateTime = null;

       //             if (LastUpdatedDate != null)
       //             {
       //                 LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
       //             }

       //             SyncRacePOSMProductMappingDTO output = new SyncRacePOSMProductMappingDTO();
       //             output.Result = ActivityBusinessInstance.GetRACEPOSMProductMapping(userID, roleID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
       //             output.HasMoreRows = HasMoreRows;

       //             #region convert date in dd/MM/yyyy format
       //             if (MaxModifiedDateTime != null)
       //                 output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
       //             else
       //                 output.MaxModifiedDate = null;

       //             #endregion

       //             response.SingleResult = output;
       //             response.IsSuccess = true;


       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.Message = ex.Message;
       //     }

       //     return response;
       // }
       // #endregion

       // #region RaceMaster
       // /// <summary>
       // /// Added by Prashant 18 Nov 2015
       // /// </summary>        
       // [UserSecureOperation]
       // public JsonResponse<SyncRaceBrandMasterDTO> GetRaceBrandMaster(long userID, int roleID, int RowCount, int StartRowIndex, string LastUpdatedDate)
       // {
       //     JsonResponse<SyncRaceBrandMasterDTO> response = new JsonResponse<SyncRaceBrandMasterDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             bool HasMoreRows = false;
       //             DateTime? MaxModifiedDateTime;
       //             DateTime? LastUpdatedDateTime = null;
       //             if (LastUpdatedDate != null)
       //             {
       //                 LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
       //             }

       //             SyncRaceBrandMasterDTO output = new SyncRaceBrandMasterDTO();
       //             output.Result = ActivityBusinessInstance.GetRaceBrandMaster(userID, roleID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
       //             output.HasMoreRows = HasMoreRows;

       //             #region convert date in dd/MM/yyyy format
       //             if (MaxModifiedDateTime != null)
       //                 output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
       //             else
       //                 output.MaxModifiedDate = null;

       //             #endregion

       //             response.SingleResult = output;
       //             response.IsSuccess = true;                  
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.IsSuccess = false;
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // [UserSecureOperation]
       // public JsonResponse<SyncRaceProductCategoryDTO> GetRaceProductCategory(long userID, int roleID, int RowCount, int StartRowIndex, string LastUpdatedDate)
       // {
       //     JsonResponse<SyncRaceProductCategoryDTO> response = new JsonResponse<SyncRaceProductCategoryDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             bool HasMoreRows = false;
       //             DateTime? MaxModifiedDateTime;
       //             DateTime? LastUpdatedDateTime = null;
       //             if (LastUpdatedDate != null)
       //             {
       //                 LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
       //             }

       //             SyncRaceProductCategoryDTO output = new SyncRaceProductCategoryDTO();
       //             output.Result = ActivityBusinessInstance.GetRaceProductCategory(userID, roleID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
       //             output.HasMoreRows = HasMoreRows;

       //             #region convert date in dd/MM/yyyy format
       //             if (MaxModifiedDateTime != null)
       //                 output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
       //             else
       //                 output.MaxModifiedDate = null;

       //             #endregion

       //             response.SingleResult = output;
       //             response.IsSuccess = true;                
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.IsSuccess = false;
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // [UserSecureOperation]
       // public JsonResponse<SyncRaceBrandCategoryMappingDTO> GetRaceBrandCategoryMapping(long userID, int roleID, int RowCount, int StartRowIndex, string LastUpdatedDate)
       // {
       //     JsonResponse<SyncRaceBrandCategoryMappingDTO> response = new JsonResponse<SyncRaceBrandCategoryMappingDTO>();
       //     try
       //     {
       //         ExceptionEngine.AppExceptionManager.Process(() =>
       //         {
       //             bool HasMoreRows = false;
       //             DateTime? MaxModifiedDateTime;
       //             DateTime? LastUpdatedDateTime = null;
       //             if (LastUpdatedDate != null)
       //             {
       //                 LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
       //             }

       //             SyncRaceBrandCategoryMappingDTO output = new SyncRaceBrandCategoryMappingDTO();
       //             output.Result = ActivityBusinessInstance.GetRaceBrandCategoryMapping(userID, roleID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();
       //             output.HasMoreRows = HasMoreRows;

       //             #region convert date in dd/MM/yyyy format
       //             if (MaxModifiedDateTime != null)
       //                 output.MaxModifiedDate = MaxModifiedDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
       //             else
       //                 output.MaxModifiedDate = null;

       //             #endregion

       //             response.SingleResult = output;
       //             response.IsSuccess = true;
       //         }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
       //     }
       //     catch (Exception ex)
       //     {
       //         response.IsSuccess = false;
       //         response.Message = ex.Message;
       //     }
       //     return response;
       // }

       // #endregion
    }
}
