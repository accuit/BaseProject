using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;
using Samsung.SmartDost.BusinessLayer.Services.BO;

using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.CommonLayer.Aspects.Exceptions;
using Samsung.SmartDost.CommonLayer.Aspects.Security;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using Samsung.SmartDost.CommonLayer.Resources;
using Samsung.SmartDost.PresentationLayer.ServiceImpl.Security;
using Samsung.SmartDost.CommonLayer.Aspects.ReportBO;
using Samsung.SmartDost.CommonLayer.Aspects.Extensions;
using System.IO;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities.HttpMultipartParser;

namespace Samsung.SmartDost.PresentationLayer.ServiceImpl
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "SmartDost" in both code and config file together.

    public partial class SmartDost : BaseService
    {

        #region MOM Created by Neeraj Singh on 24th November

        //Search MOM details and Attendees
        [UserSecureOperation]
        public JsonResponse<MOMDTO> GetMOMSearchData(long UserID, string MOMTitle)
        {
            JsonResponse<MOMDTO> response = new JsonResponse<MOMDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    List<MOMDTO> result = new List<MOMDTO>();
                    result = SalesCatalystInstance.GetMOMSearchData(UserID, MOMTitle).ToList();
                    foreach (var item in result)
                    {
                        item.MomDateStr = item.MomDate.ToString("dd-MMM-yyyy");
                    }
                    response.Result = result;
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
        //Get list of Meetings of a User
        [UserSecureOperation]
        public JsonResponse<MOMDTO> GetUserMOMData(long UserID, bool IsIncremental)
        {
            JsonResponse<MOMDTO> response = new JsonResponse<MOMDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    List<MOMDTO> result = new List<MOMDTO>();
                    result = SalesCatalystInstance.GetUserMOMData(UserID, false).ToList();
                    foreach (var datestr in result)
                    {
                        datestr.MomDateStr = datestr.MomDate.ToString("dd-MMM-yyyy");
                    }
                    response.Result = result as List<MOMDTO>;
                    response.IsSuccess = true;

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;

            }
            return response;
        }

        // Insert MOM data and Attendeed details
        [UserSecureOperation]
        public JsonResponse<int> SubmitMOMData(long UserID, string MOMTitle, string ActionItem, string Location, string Description, string MOMDate, List<MOMAttendeeDTO> Attendees)
        {
            JsonResponse<int> response = new JsonResponse<int>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    int result = SalesCatalystInstance.SubmitMOMData(UserID, MOMTitle, ActionItem, Location, Description, DateTime.ParseExact(MOMDate, "dd/mm/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None), Attendees);
                    response.SingleResult = result;
                    response.IsSuccess = true;
                    response.Message = "Meeting successfully created.";
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        // Insert MOM List and corresponding Attendeed details
        [UserSecureOperation]
        public JsonResponse<MOMOutputDTO> SubmitMOMList(long UserID, List<MOMDTO> MOMList)
        {
            JsonResponse<MOMOutputDTO> response = new JsonResponse<MOMOutputDTO>();
            List<MOMOutputDTO> output = new List<MOMOutputDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    foreach (var item in MOMList)
                    {

                        item.MomDate = Convert.ToDateTime(item.MomDateStr);
                        MOMOutputDTO data = new MOMOutputDTO();
                        data.MOMIdClient = item.MOMIdClient;

                        // Insert Data if MOMIdServer = -1 else Update 
                        if (item.MOMIdServer == -1)
                        {
                            data.MOMIdServer = SalesCatalystInstance.SubmitMOMList(UserID, item);
                            output.Add(data);
                            response.Result = output;
                            response.IsSuccess = true;
                            response.Message = "Meeting successfully created.";
                        }
                        else
                        {
                            SalesCatalystInstance.UpdateMOMData(item.MOMIdServer, UserID, item.MOMTitle, item.ActionItem, item.Location, item.Description, item.MomDate, item.MOMAttendees, false);
                            data.MOMIdServer = item.MOMIdServer;
                            output.Add(data);
                            response.Result = output;
                            response.IsSuccess = true;
                            response.Message = "Meeting successfully updated.";
                        }

                    }


                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }

        //Delete MOM data along with Attendees attached
        [UserSecureOperation]
        public JsonResponse<int> DeleteMOMData(List<int> MOMId)
        {
            JsonResponse<int> response = new JsonResponse<int>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    // List<MOMDTO> result = new List<MOMDTO>();
                    int result = SalesCatalystInstance.DeleteMOMData(MOMId);
                    response.SingleResult = result;
                    response.IsSuccess = true;
                    response.Message = "Meeting successfully deleted.";
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }

        //Update MOM data along with Attendees attached
        [UserSecureOperation]
        public JsonResponse<int> UpdateMOMData(long MOMId, long UserID, string MOMTitle, string ActionItem, string Location, string Description, string MOMDate, List<MOMAttendeeDTO> Attendees, bool IsIncremental)
        {
            JsonResponse<int> response = new JsonResponse<int>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    int result = SalesCatalystInstance.UpdateMOMData(MOMId, UserID, MOMTitle, ActionItem, Location, Description, DateTime.ParseExact(MOMDate, "dd-MMM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None), Attendees, false);
                    response.SingleResult = result;
                    response.IsSuccess = true;
                    response.Message = "Meeting successfully updated.";
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }

            return response;
        }
        #endregion

        #region Expense Master management created by Vishnu


        /// <summary>
        /// Get ExpenseTypeMaster data by Company Id By Vishnu on 28 Dec 2015
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="companyID"></param>
        /// <param name="roleID"></param>
        /// <param name="RowCount"></param>
        /// <param name="StartRowIndex"></param>
        /// <param name="LastUpdatedDate"></param>
        /// <returns></returns>
        [UserSecureOperation]
        public JsonResponse<SyncExpenseTypeMasterDTO> GetExpenseMasterData(long userID, int companyID, int roleID, int RowCount, int StartRowIndex, string LastUpdatedDate)
        {
            JsonResponse<SyncExpenseTypeMasterDTO> response = new JsonResponse<SyncExpenseTypeMasterDTO>();

            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    bool HasMoreRows = false;
                    DateTime? MaxModifiedDateTime = null;
                    DateTime? LastUpdatedDateTime = null;

                    if (LastUpdatedDate != null)
                    {
                        LastUpdatedDateTime = DateTime.ParseExact(LastUpdatedDate, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                    }

                    SyncExpenseTypeMasterDTO output = new SyncExpenseTypeMasterDTO();
                    output.Result = SalesCatalystInstance.GetExpenseMasterData(userID, companyID, roleID, RowCount, StartRowIndex, LastUpdatedDateTime, out HasMoreRows, out MaxModifiedDateTime).ToList();

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
        /// Created By Vishnu Narayan Mishra 06 Jan 2016 to Submit Expense for Approval.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <param name="emsExpenseDetail"></param>
        /// <returns></returns>
        [UserSecureOperation]
        public JsonResponse<EMSExpenseDetailOutputDTO> SubmitExpenseForApproval(long userID, int roleID, EMSExpenseDetailDTO emsExpenseDetail)
        {
            JsonResponse<EMSExpenseDetailOutputDTO> response = new JsonResponse<EMSExpenseDetailOutputDTO>();
            List<EMSExpenseDetailOutputDTO> output = new List<EMSExpenseDetailOutputDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    emsExpenseDetail.CreatedBy = Convert.ToInt32(userID);
                    EMSExpenseDetailOutputDTO outputdata = new EMSExpenseDetailOutputDTO();
                    outputdata.EMSExpenseDetailIDClient = emsExpenseDetail.EMSExpenseDetailIDClient;

                    foreach (var bill in emsExpenseDetail.EMSBillDetails)
                    {
                        bill.BillDate = Convert.ToDateTime(bill.BillDateStr);
                    }

                    if (emsExpenseDetail.EMSExpenseDetailIDServer == 0)
                    {
                        outputdata.EMSExpenseDetailIDServer = SalesCatalystInstance.SubmitExpenseForApproval(userID, roleID, emsExpenseDetail);

                        output.Add(outputdata);
                        response.Result = output;
                        response.IsSuccess = true;
                        response.Message = "Expense submitted successfully.";
                    }
                    else
                    {
                        emsExpenseDetail.EMSExpenseDetailID = emsExpenseDetail.EMSExpenseDetailIDServer;
                        SalesCatalystInstance.SubmitExpenseForApproval(userID, roleID, emsExpenseDetail);
                        response.Result = output;
                        response.IsSuccess = true;
                        response.Message = "Expense updated successfully.";
                    }



                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;

            }
            return response;
        }


        [UserSecureOperation]
        public JsonResponse<EMSExpenseDetailOutputDTO> SubmitExpenseAndBill(long userID, int roleID, List<EMSExpenseDetailDTO> emsExpenseDetail)
        {
            JsonResponse<EMSExpenseDetailOutputDTO> response = new JsonResponse<EMSExpenseDetailOutputDTO>();
            List<EMSExpenseDetailOutputDTO> output = new List<EMSExpenseDetailOutputDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    //foreach (var details in emsExpenseDetail)
                    //{
                    //    details.CreatedBy = Convert.ToInt32(userID);
                    //    EMSExpenseDetailDTO outputdata = new EMSExpenseDetailDTO();
                    //    outputdata.EMSExpenseDetailIDClient = details.EMSExpenseDetailIDClient;

                    //    foreach (var bill in details.EMSBillDetails)
                    //    {
                    //        EMSBillDetailDTO outputbill = new EMSBillDetailDTO();
                    //        outputbill.EMSBillDetailIDClient = bill.EMSBillDetailIDClient;
                    //        bill.BillDate = Convert.ToDateTime(bill.BillDateStr);
                    //        foreach (var doc in bill.EMSBillDocumentDetails)
                    //        {
                    //            EMSBillDocumentDetailDTO outputdoc = new EMSBillDocumentDetailDTO();
                    //            outputdoc.EMSBillDocumentDetailIDClient = doc.EMSBillDocumentDetailIDClient;
                    //        }
                    //    }
                    //}
                    
                    List<EMSExpenseDetailOutputDTO> result = new List<EMSExpenseDetailOutputDTO>();

                    EntityMapper.Map(SalesCatalystInstance.SubmitExpenseAndBill(userID, roleID, emsExpenseDetail), result);
                    response.Result = result;
                    response.IsSuccess = true;
                    response.Message = "Expense submitted successfully.";

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;

            }
            return response;
        }

        public JsonResponse<string> UploadBillImageStream(Stream image)
        {
            JsonResponse<string> response = new JsonResponse<string>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    // with the HTTP request. We can do that in WCF using the WebOperationConext:
                    // var type = System.ServiceModel.Web.WebOperationContext.Current.IncomingRequest.Headers["Content-Type"];
                    //Stream stream = new MemoryStream(image);
                    var parser = new MultipartFormDataParser(image);

                    string apiKey = parser.Parameters["APIKey"].Data;
                    string apiToken = parser.Parameters["APIToken"].Data;
                    string userid = parser.Parameters["userID"].Data;
                    bool isValid = SystemBusinessInstance.IsValidServiceUser(apiKey, apiToken, userid);
                    if (isValid)
                    {

                        // From this point the data is parsed, we can retrieve the
                        // form data from the Parameters dictionary:

                        int EMSExpenseDetailId = Convert.ToInt32(parser.Parameters["EMSExpenseDetailIDServer"].Data);
                        int EMSBillDetailID = Convert.ToInt32(parser.Parameters["EMSBillDetailIDServer"].Data);
                        int EMSBillDocumentDetailID = Convert.ToInt32(parser.Parameters["EMSBillDocumentDetailIDServer"].Data);
                        int UserID = Convert.ToInt32(parser.Parameters["userID"].Data);
                        int roelID = Convert.ToInt32(parser.Parameters["roleID"].Data);
                        FileStream fileData = null;
                        MemoryStream ms = null;

                        int counter = 1;
                        string fileDirectory = string.Empty;

                        fileDirectory = AppUtil.GetUploadDirectory(AspectEnums.ImageFileTypes.Expense);


                        List<EMSBillDocumentDetailDTO> BillDocumentDetailDTOList = new List<EMSBillDocumentDetailDTO>();
                        foreach (var file in parser.Files)
                        {
                            string filename = file.FileName;

                            if (Directory.Exists(fileDirectory))
                            {
                                string newFileName = UserID.ToString() + "_" + EMSBillDocumentDetailID.ToString() + "_" + AppUtil.GetUniqueKey().ToUpper() + DateTime.Now.ToString().Replace(" ", "").Replace(":", "").Replace("/", "") + counter.ToString() + ".jpeg";
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

                                            BillDocumentDetailDTOList.Add(new EMSBillDocumentDetailDTO { EMSBillDocumentDetailID = EMSBillDocumentDetailID, DocumentName = newFileName, CreatedBy = UserID });

                                            file.Data.Close();

                                            if (ms != null)
                                            {
                                                ms.Close();
                                                ms.Dispose();
                                            }
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

                                counter++;
                            }
                        }


                        bool isSuccess = false;
                        isSuccess = SalesCatalystInstance.UpdateBillIamge(BillDocumentDetailDTOList);

                        response.IsSuccess = true;
                    }
                    else
                        throw new System.Security.SecurityException(Messages.ApiAccessDenied);
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
            }
            return response;
        }

        /// <summary>
        /// GetExpenseList
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="emsExpenseDetailID"></param>
        /// <returns></returns>
        [UserSecureOperation]
        public JsonResponse<EMSExpenseDetailDTO> GetExpenseList(long userID, int? emsExpenseDetailID)
        {
            JsonResponse<EMSExpenseDetailDTO> response = new JsonResponse<EMSExpenseDetailDTO>();

            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    List<EMSExpenseDetailDTO> finalresult = new List<EMSExpenseDetailDTO>();


                    finalresult = SalesCatalystInstance.GetExpenseList(userID, emsExpenseDetailID).ToList();//finalresult;
                    foreach (var item in finalresult)
                    {
                        item.CreatedByUsername = SalesCatalystInstance.GetCreatedByUsername(item.EMSExpenseDetailID);

                        if (item.Status == (byte)AspectEnums.ApprovalStatus.Pending)
                            item.IsExpenseEditable = SalesCatalystInstance.IsExpenseEditable(item.EMSExpenseDetailID);
                        else item.IsExpenseEditable = false;
                        foreach (var bill in item.EMSBillDetails)
                        {
                            bill.BillDateStr = bill.BillDate.ToString("dd-MMM-yyyy");
                        }

                        if (item.CreatedBy == userID)
                        {

                            var approve = item.ApprovalStatusHistories.LastOrDefault();

                            if (approve != null)
                            {
                                item.AssignedToUser = approve.AssignedToUser;
                                item.AssignedStatus = approve.Status;
                                try
                                {
                                    if (item.Status == (byte)AspectEnums.ApprovalStatus.Cancelled)
                                        item.PendingWith = "Cancelled";
                                    else if (item.Status == (byte)AspectEnums.ApprovalStatus.ApprovalNotRequired)
                                        item.PendingWith = "Approval Not Required";
                                    else
                                        item.PendingWith = SalesCatalystInstance.GetPendingWithName(approve.EMSExpenseDetailID, item.Status);

                                    //else
                                    //    item.PendingWith = "";
                                }
                                catch
                                {
                                    item.PendingWith = "";
                                }
                            }
                            else if (item.Status == (byte)AspectEnums.ApprovalStatus.ApprovalNotRequired)
                            {
                                item.PendingWith = "Approval Not Required";
                            }
                            else if (item.Status == (byte)AspectEnums.ApprovalStatus.Cancelled)
                            {
                                item.PendingWith = "Cancelled";
                            }
                            else if (item.Status == (byte)AspectEnums.ApprovalStatus.Pending)
                            {
                                item.PendingWith = "Pending";
                            }

                        }
                        else
                        {
                            var approver = item.ApprovalStatusHistories.Where(x => x.AssignedToUser == userID).FirstOrDefault();
                            
                                item.AssignedToUser = approver.AssignedToUser;
                                item.AssignedStatus = approver.Status;
                                item.PendingWith = SalesCatalystInstance.GetPendingWithName(approver.EMSExpenseDetailID, item.Status);
                            
                        }
                        item.ExpenseDate = item.CreatedDate.ToString("dd-MMM-yyyy");
                    }
                    
                    response.Result = finalresult;
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

        #region LMS Services
        /// <summary>
        /// Submit Leave Request
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <param name="LMSLeaveMasterData"></param>
        /// <returns></returns>
        [UserSecureOperation]
        public JsonResponse<bool> SumbitLeaveRequest(long userID, int roleID, string ApproverRemarks, List<LMSLeaveMasterDTO> leaves)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    response.IsSuccess = SalesCatalystInstance.SumbitLeaveRequest(userID, roleID, ApproverRemarks, leaves);
                    response.SingleResult = response.IsSuccess;
                    response.Message = Messages.LMSSUSSESS;

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;

            }
            return response;


        }
        /// <summary>
        /// Get LeaveType Master based on RoleID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        /// 
        [UserSecureOperation]
        public JsonResponse<LMSLeaveTypeMasterDTO> GetLeaveTypeMaster(long userID, int roleID)
        {
            JsonResponse<LMSLeaveTypeMasterDTO> response = new JsonResponse<LMSLeaveTypeMasterDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    response.Result = SalesCatalystInstance.GetLeaveTypeMaster(userID, roleID);//finalresult;                    
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
        /// Get LeaveType Configurations based on RoleID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        /// 
        [UserSecureOperation]
        public JsonResponse<LMSLeaveTypeConfigurationDTO> GetLeaveTypeCofiguration(long userID, int roleID)
        {
            JsonResponse<LMSLeaveTypeConfigurationDTO> response = new JsonResponse<LMSLeaveTypeConfigurationDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    response.Result = SalesCatalystInstance.GetLeaveTypeCofiguration(userID, roleID);//finalresult;                    
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
        /// Get Leave data based on request type or leaveTypeID
        /// </summary>
        /// <param name="userID">UserID for which leave needs to be fetched</param>
        /// <param name="LeaveTypeID"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        /// 
        [UserSecureOperation]
        public JsonResponse<LMSLeaveMasterDTO> GetLeaves(long userID, LMSLeaveRequestDTO LMSLeaveRequest)
        {
            JsonResponse<LMSLeaveMasterDTO> response = new JsonResponse<LMSLeaveMasterDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    response.Result = SalesCatalystInstance.GetLeaves(userID, LMSLeaveRequest);//finalresult;                    
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

        #region Sales Return System

        /// <summary>
        /// Submit Sales Return Request
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <param name="ApproverRemarks"></param>
        /// <param name="SRRequestData"></param>
        /// <returns></returns>
        //[UserSecureOperation]
        public JsonResponse<bool> SumbitSalesReturnRequest(long userID, int roleID, string ApproverRemarks, List<SRRequestDTO> SRRequestData)
        {
            JsonResponse<bool> response = new JsonResponse<bool>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {

                    response.IsSuccess = SalesCatalystInstance.SumbitSalesReturnRequest(userID, roleID, ApproverRemarks, SRRequestData);
                    response.SingleResult = response.IsSuccess;
                    response.Message = Messages.SalesReturnSuccess;

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;

            }
            return response;

        }

        /// <summary>
        /// Get Sales Return
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="SRRequest"></param>
        /// <returns></returns>
        //[UserSecureOperation]
        public JsonResponse<SRRequestDTO> GetSalesReturn(long userID, GetSalesReturnRequestDTO SRRequest)
        {
            JsonResponse<SRRequestDTO> response = new JsonResponse<SRRequestDTO>();

            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.Result = SalesCatalystInstance.GetSalesReturn(userID, SRRequest);
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.InnerException.Message.ToString();
            }
            return response;
        }
        #endregion

        #region Syster of Sales Forecasting (SOSF)

        public JsonResponse<long> SubmitSOSFList(long UserID, List<SOSFMasterDTO> SOSFList)
        {

            JsonResponse<long> response = new JsonResponse<long>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.SingleResult = SalesCatalystInstance.SubmitSOSFList(UserID, SOSFList);

                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.InnerException.Message.ToString();
            }
            return response;
        }

        public JsonResponse<SOSFOutPutDTO> GetUserSOSFData(long UserID, int StoreID, int ProductID)
        {
            JsonResponse<SOSFOutPutDTO> response = new JsonResponse<SOSFOutPutDTO>();

            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    response.SingleResult = SalesCatalystInstance.GetUserSOSFData(UserID, StoreID, ProductID);
                    
                    response.IsSuccess = true;
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.InnerException.Message.ToString();
            }
            return response;
        }

        #endregion
    }
}

