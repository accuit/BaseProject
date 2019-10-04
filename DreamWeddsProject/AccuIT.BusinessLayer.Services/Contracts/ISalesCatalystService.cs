using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Samsung.SmartDost.BusinessLayer.Services.BO;
using System.Data;

namespace Samsung.SmartDost.BusinessLayer.Services.Contracts
{

    public interface ISalesCatalystService
    {

        #region MOM - Added BY Neeraj Singh started on 27th Nov 2015

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="MOMTitle"></param>
        /// <returns></returns>
        List<MOMDTO> GetMOMSearchData(long UserID, string MOMTitle);

        /// <summary>
        /// Add new meeting from Console
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="MOMTitle"></param>
        /// <param name="ActionItem"></param>
        /// <param name="Location"></param>
        /// <param name="Description"></param>
        /// <param name="MOMDate"></param>
        /// <param name="Attendees"></param>
        /// <returns></returns>
        int SubmitMOMData(long UserID, string MOMTitle, string ActionItem, string Location, string Description, DateTime MOMDate, List<MOMAttendeeDTO> Attendees);
        /// <summary>
        /// Add list of new meetings from APK
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="MOMList"></param>
        /// <returns></returns>
        long SubmitMOMList(long UserID, MOMDTO MOMList);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MOMId"></param>
        /// <returns></returns>
        int DeleteMOMData(List<int> momid);

        int DeleteAttendee(long AttendeeId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MOMId"></param>
        /// <param name="UserID"></param>
        /// <param name="MOMTitle"></param>
        /// <param name="ActionItem"></param>
        /// <param name="Location"></param>
        /// <param name="Description"></param>
        /// <param name="MOMDate"></param>
        /// <param name="Attendees"></param>
        /// <returns></returns>
        int UpdateMOMData(long MOMId, long UserID, string MOMTitle, string ActionItem, string Location, string Description, DateTime MOMDate, List<MOMAttendeeDTO> Attendees, bool IsIncremental);

        /// <summary>
        /// Created by Neeraj on 3rd Dec 2015 for getting MOM of a User in Console Gridview 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        IList<MOMDTO> GetUserMOMData(long UserID, bool IsIncremental);

        IList<MOMAttendeeBO> GetAttendeeList(long MOMID);

        #endregion

        #region Upload Master data by Neeraj Singh on 26 dec 2015

        /// <summary>
        /// to export data to the server by ankit saxena on 29 dec 2014
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="isGeneral"></param>
        /// <returns></returns> 
        List<int> UploadMasterDataParking2Main(DataTable dtDemoValidation, int enumMaster);

        /// <summary>
        /// to Upload SecInfoData by amjad on 05 April 2015
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="isGeneral"></param>
        /// <returns></returns> 
        //int ExportSecInfoData(DataTable dtDemoValidation);


        #endregion

        #region "Added By Vishnu for Expense Management System ON 23 Dec 2015"

        /// <summary>
        /// Get All Expense Type Master for the company.
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        List<ExpenseTypeMasterDTO> GetExpenseMasterData(long userID, int companyID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate);

        /// <summary>
        /// SubmitExpenseForApproval
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <param name="emsExpenseDetail"></param>
        /// <returns></returns>
        int SubmitExpenseForApproval(long userID, int roleID, EMSExpenseDetailDTO emsExpenseDetail);

        List<EMSExpenseDetailDTO> SubmitExpenseAndBill(long userID, int roleID, List<EMSExpenseDetailDTO> emsExpenseDetail);



        /// <summary>
        /// GetExpenseList
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="emsExpenseDetailID"></param>
        /// <returns></returns>
        List<EMSExpenseDetailDTO> GetExpenseList(long userID, int? emsExpenseDetailID);
        string GetPendingWithName(int EMSExpenseDetailID, byte expenseMainStatus);

        bool IsExpenseEditable(int emsExpenseDetailID);

        string GetCreatedByUsername(int emsExpenseDetailID);

        //List<EMSExpenseDetailDTO> GetExpenseList();

        //Used to update EMS bill document after uploading image
        bool UpdateBillIamge(List<EMSBillDocumentDetailDTO> objEMSBillDocumentDetailDTO);

        #region "Approval Master data based on user Company"
        /// <summary>
        /// GetExpenseTypeMaster 
        /// </summary>
        /// <returns></returns>
        List<ExpenseTypeMasterBO> GetExpenseTypeMaster(int companyID);

        /// <summary>
        /// GetApproverType
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        List<CommonSetupBO> GetApproverType(int companyID);

        #endregion

        List<GetExpenseApprovalPathBO> GetExpenseApprovalPathData(long? userID, int companyID, int? roleID, int approvalPathTypeID, int? emsExpenseTypeMasterId);

        /// <summary>
        /// Submit Approval Data
        /// </summary>
        /// <param name="ApproverPathMaster"></param>
        /// <returns></returns>
        int CreateApprovalPath(ApproverPathMasterBO ApproverPathMaster);

        /// <summary>
        /// Delete Approver
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="approverPathMasterID"></param>
        /// <returns></returns>
        bool DeleteApprover(long UserID, int approverPathMasterID);

        /// <summary>
        /// GetUserByTeamRole
        /// </summary>
        /// <param name="TeamID"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        List<spGetUserByTeamRoleBO> GetUserByTeamRole(int? TeamID, int? RoleID);

        #endregion

        #region LMS Services
        /// <summary>
        /// Get LeaveType Master based on UserID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        List<LMSLeaveTypeMasterDTO> GetLeaveTypeMaster(long userID, int roleID);
        /// <summary>
        /// Get LeaveTypeConfigurations based on RoleID        
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        List<LMSLeaveTypeConfigurationDTO> GetLeaveTypeCofiguration(long userID, int roleID);

        

          /// <summary>
        /// Get Leave data based on request type or leaveTypeID
        /// </summary>
        /// <param name="userID">UserID for which leave needs to be fetched</param>
        /// <param name="LeaveTypeID"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        List<LMSLeaveMasterDTO> GetLeaves(long userID, LMSLeaveRequestDTO LeaveRequest);

        /// <summary>
        /// Submit leaves
        /// </summary>
        /// <param name="userID">UserID for which leave needs to be fetched</param>
        /// <param name="LeaveTypeID"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        bool SumbitLeaveRequest(long userID, int roleID, string ApproverRemarks, List<LMSLeaveMasterDTO> leaves);

        /// <summary>
        /// For Console
        /// </summary>
        /// <param name="lmsconfig"></param>
        /// <returns></returns>
        int SubmitLeavetypeConfig(LMSLeaveTypeConfigurationBO lmsconfig);
        #endregion

        #region Sales Return System
        List<ApprovalPathTypeBO> GetApprovalPathType();

        bool SumbitSalesReturnRequest(long userID, int roleID, string ApproverRemarks, List<SRRequestDTO> SRRequestData);

        List<SRRequestDTO> GetSalesReturn(long userID, GetSalesReturnRequestDTO SRRequest);

        #endregion

        #region Syster of Sales Forecasting (SOSF)

        long SubmitSOSFList(long UserID, List<SOSFMasterDTO> SOSFList);

        SOSFOutPutDTO GetUserSOSFData(long UserID, int StoreID, int ProductID);

        #endregion
    }
}
