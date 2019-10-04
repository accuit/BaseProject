using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Samsung.SmartDost.PersistenceLayer.Repository.Contracts
{
    public interface ISalesCatalystRepository
    {

        #region Created by Neeraj Started on 27th Nov 15 and finished on 10 Dec 15
        List<MOMMaster> GetMOMSearchData(long UserID, string MOMTitle);

        int SubmitMOMData(long UserID, string MOMTitle, string ActionItem, string Location, string Description, DateTime MOMDate, List<MOMAttendee> Attendees);

        long SubmitMOMList(long UserID, MOMMaster MOMList);

        int DeleteMOMData(List<int> momid);

        int UpdateMOMData(long MOMId, long UserID, string MOMTitle, string ActionItem, string Location, string Description, DateTime MOMDate, List<MOMAttendee> Attendees, bool IsIncremental);

        List<MOMMaster> GetUserMOMData(long UserID, bool IsIncremental);

        List<MOMAttendee> GetAttendeeList(long MOMID);

        int DeleteAttendee(long AttendeeId);

        #endregion

        #region To Master Data Upload  data from excel file Added By Neeraj on 19th December

        /// <summary>
        /// to export data to the server
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="isGeneral"></param>
        /// <returns></returns> 
        List<int> UploadMasterDataParking2Main(DataTable dtDemoValidation, int enumMaster);

        #endregion

        #region "Added By Vishnu For Expense Management System implementation on 23 Dec 2015"

        /// <summary>
        /// GetExpenseMasterData
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="companyID"></param>
        /// <param name="roleID"></param>
        /// <param name="RowCount"></param>
        /// <param name="StartRowIndex"></param>
        /// <param name="LastUpdatedDate"></param>
        /// <param name="HasMoreRows"></param>
        /// <param name="MaxModifiedDate"></param>
        /// <returns></returns>
        IList<EMSExpenseTypeMaster> GetExpenseMasterData(long userID, int companyID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        /// <summary>
        /// SubmitExpenseForApproval
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <param name="emsExpenseDetail"></param>
        /// <returns></returns>
        int SubmitExpenseForApproval(long userID, int roleID, EMSExpenseDetail emsExpenseDetail);

        List<EMSExpenseDetailDTO> SubmitExpenseAndBill(long userID, int roleID, List<EMSExpenseDetailDTO> emsExpenseDetail);
        /// <summary>
        /// GetExpenseList
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="emsExpenseDetailID"></param>
        /// <returns></returns>
        List<EMSExpenseDetail> GetExpenseList(long userID, int? emsExpenseDetailID);
        string GetPendingWithName(int EMSExpenseDetailID, byte expenseMainStatus);

        string GetCreatedByUsername(int emsExpenseDetailID);

        bool IsExpenseEditable(int emsExpenseDetailID);
        //List<EMSExpenseDetail> GetExpenseList();

        //int SubmitApprovalData(int companyID, int profileRole, int expenessType, bool approvalRequired, int approvalRole, int approveType, int sequence);
        int CreateApprovalPath(ApproverPathMaster ApproverPathMaster);

        bool UpdateBillIamge(List<EMSBillDocumentDetail> objEMSBillDocumentDetail);

        #region "Approval Master data based on user Company"
        /// <summary>
        /// GetExpenseTypeMaster 
        /// </summary>
        /// <returns></returns>
        List<EMSExpenseTypeMaster> GetExpenseTypeMaster(int companyID);

        /// <summary>
        /// GetApproverType
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        List<CommonSetup> GetApproverType(int companyID);

        /// <summary>
        /// Delete approver
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="approverPathMasterID"></param>
        /// <returns></returns>
        bool DeleteApprover(long UserID, int approverPathMasterID);
        
        #endregion

        /// <summary>
        /// GetExpenseApprovalPathData
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="companyID"></param>
        /// <param name="roleID"></param>
        /// <param name="approvalPathTypeID"></param>
        /// <param name="emsExpenseTypeMasterId"></param>
        /// <returns></returns>
        List<GetExpenseApprovalPath> GetExpenseApprovalPathData(long? userID, int companyID, int? roleID, int approvalPathTypeID, int? emsExpenseTypeMasterId);

        /// <summary>
        /// GetUserByTeamRole
        /// </summary>
        /// <param name="TeamID"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        List<spGetUserByTeamRole> GetUserByTeamRole(int? TeamID, int? RoleID);

        #endregion

        #region LMS Services
        /// <summary>
        /// Get LeaveType Master based on UserID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        List<LMSLeaveTypeMaster> GetLeaveTypeMaster(long userID, int roleID);
        /// <summary>
        /// Service to fetch leaves taken by any user in any financial year
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <param name="financialYearStart"></param>
        /// <returns></returns>
        List<LeaveBalanceEntity> GetLeavesTaken(long userID, int roleID, DateTime financialYearStart);
        /// <summary>
        /// Get LeaveTypeConfigurations based on RoleID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        List<LMSLeaveTypeConfiguration> GetLeaveTypeCofiguration(long userID, int roleID);
        /// <summary>
        /// Get Leave data based on request type or leaveTypeID
        /// </summary>
        /// <param name="userID">UserID for which leave needs to be fetched</param>
        /// <param name="LeaveTypeID"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        List<LMSLeaveMaster> GetLeaves(long userID, LMSLeaveRequestDTO LeaveRequest);
        /// <summary>
        /// Submit Leave Request
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <param name="LMSLeaveMasterData"></param>
        /// <returns></returns>
        bool SumbitLeaveRequest(long userID, int roleID, string ApproverRemarks,  List<LMSLeaveMaster> LMSLeaveMasterData);

        /// <summary>
        /// Leave Configuration for Console
        /// </summary>
        /// <param name="lmsconfig"></param>
        /// <returns></returns>
        int SubmitLeavetypeConfig(LMSLeaveTypeConfiguration lmsconfig);

        #endregion

        #region Sales Return System
        IList<ApprovalPathType> GetApprovalPathType();

        bool SumbitSalesReturnRequest(long userID, int roleID, string ApproverRemarks, List<SRRequest> SRRequestData);

        List<SRRequest> GetSalesReturn(long userID, GetSalesReturnRequestDTO SRRequest);

        #endregion

        #region Syster of Sales Forecasting (SOSF)

        long SubmitSOSFList(long UserID, List<SOSFMaster> SOSFList);

        List<SOSFMaster> GetUserSOSFData(long UserID, int StoreID, int ProductID, out int type);

        #endregion

    }
}
