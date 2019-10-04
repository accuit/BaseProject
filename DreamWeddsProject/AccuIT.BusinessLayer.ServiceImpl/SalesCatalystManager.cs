using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using Samsung.SmartDost.BusinessLayer.Base;
using Samsung.SmartDost.BusinessLayer.Services.Contracts;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using Samsung.SmartDost.PersistenceLayer.Repository.Contracts;
using Samsung.SmartDost.BusinessLayer.Services.BO;
using System.Data;
using System.Xml.Linq;
using System.Xml;

namespace Samsung.SmartDost.BusinessLayer.ServiceImpl
{

    public class SalesCatalystManager : SalesCatalystBaseService, ISalesCatalystService
    {
        /// <summary>
        /// Property to inject the store persistence layer object invocation
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.SalesCatalyst_REPOSITORY)]
        public ISalesCatalystRepository SalesCatalystRepository { get; set; }

        #region MOM Services and methods

        public List<MOMDTO> GetMOMSearchData(long UserID, string MOMTitle)
        {
            List<MOMDTO> result = new List<MOMDTO>();

            ObjectMapper.Map(SalesCatalystRepository.GetMOMSearchData(UserID, MOMTitle), result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="MOMTitle"></param>
        /// <param name="ActionItem"></param>
        /// <param name="Location"></param>
        /// <param name="Description"></param>
        /// <param name="MOMDate"></param>
        /// <param name="Attendees"></param>
        /// <returns></returns>
        public int SubmitMOMData(long UserID, string MOMTitle, string ActionItem, string Location, string Description, DateTime MOMDate, List<MOMAttendeeDTO> Attendees)
        {
            List<MOMAttendee> MOMAttendee = new List<MOMAttendee>();
            ObjectMapper.Map(Attendees, MOMAttendee);
            return SalesCatalystRepository.SubmitMOMData(UserID, MOMTitle, ActionItem, Location, Description, MOMDate, MOMAttendee);
            //ObjectMapper.Map(SalesCatalystRepository.SubmitMOMData(objMommaster),result);           
        }

        public long SubmitMOMList(long UserID, MOMDTO MOMList)
        {
            MOMMaster MOMList1 = new MOMMaster();
            ObjectMapper.Map(MOMList, MOMList1);
            return SalesCatalystRepository.SubmitMOMList(UserID, MOMList1);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="MOMId"></param>
        /// <returns></returns>
        public int DeleteMOMData(List<int> momid)
        {
            int result = 0;
            ObjectMapper.Map(SalesCatalystRepository.DeleteMOMData(momid), result);
            return result;
        }

        public int DeleteAttendee(long AttendeeId)
        {
            int result = 0;
            ObjectMapper.Map(SalesCatalystRepository.DeleteAttendee(AttendeeId), result);
            return result;
        }

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
        public int UpdateMOMData(long MOMId, long UserID, string MOMTitle, string ActionItem, string Location, string Description, DateTime MOMDate, List<MOMAttendeeDTO> Attendees, bool IsIncremental)
        {
            List<MOMAttendee> MOMAttendee = new List<MOMAttendee>();
            ObjectMapper.Map(Attendees, MOMAttendee);
            return SalesCatalystRepository.UpdateMOMData(MOMId, UserID, MOMTitle, ActionItem, Location, Description, MOMDate, MOMAttendee, IsIncremental);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public IList<MOMDTO> GetUserMOMData(long UserID, bool IsIncremental)
        {
            List<MOMDTO> result = new List<MOMDTO>();
            ObjectMapper.Map(SalesCatalystRepository.GetUserMOMData(UserID, IsIncremental), result);
            return result;
        }

        public IList<MOMAttendeeBO> GetAttendeeList(long MOMID)
        {
            List<MOMAttendeeBO> result = new List<MOMAttendeeBO>();
            ObjectMapper.Map(SalesCatalystRepository.GetAttendeeList(MOMID), result);
            return result;
        }

        #endregion

        #region Upload Master data by Neeraj Singh on 26 dec 2014

        /// <summary>
        /// to export data to the server 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="isGeneral"></param>
        /// <returns></returns> 
        public List<int> UploadMasterDataParking2Main(DataTable dtDemoValidation, int enumMaster)
        {
            return SalesCatalystRepository.UploadMasterDataParking2Main(dtDemoValidation, enumMaster);
        }

        #endregion

        #region "Expense Management System" Created by Vishnu

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
        public List<ExpenseTypeMasterDTO> GetExpenseMasterData(long userID, int companyID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            List<ExpenseTypeMasterDTO> result = new List<ExpenseTypeMasterDTO>();
            ObjectMapper.Map(SalesCatalystRepository.GetExpenseMasterData(userID, companyID, roleID, RowCount, StartRowIndex, LastUpdatedDate, out HasMoreRows, out MaxModifiedDate), result);
            return result;
        }

        /// <summary>
        /// SubmitExpenseForApproval
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <param name="emsExpenseDetail"></param>
        /// <returns></returns>
        public int SubmitExpenseForApproval(long userID, int roleID, EMSExpenseDetailDTO emsExpenseDetail)
        {
            EMSExpenseDetail result = new EMSExpenseDetail();

            EMSExpenseDetailBO bo = new EMSExpenseDetailBO();
            ObjectMapper.Map(emsExpenseDetail, bo);
            ObjectMapper.Map(emsExpenseDetail, result);

            return (SalesCatalystRepository.SubmitExpenseForApproval(userID, roleID, result));


        }

        public List<EMSExpenseDetailDTO> SubmitExpenseAndBill(long userID, int roleID, List<EMSExpenseDetailDTO> emsExpenseDetail)
        {

            return SalesCatalystRepository.SubmitExpenseAndBill(userID, roleID, emsExpenseDetail);

        }


        /// <summary>
        /// GetExpenseList
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="emsExpenseDetailID"></param>
        /// <returns></returns>
        public List<EMSExpenseDetailDTO> GetExpenseList(long userID, int? emsExpenseDetailID)
        {
            #region old code
            //            //  List<EMSExpenseDetailBO> emsExpenseDetailBO = new List<EMSExpenseDetailBO>();
            //            var result = SalesCatalystRepository.GetExpenseList(userID, emsExpenseDetailID);

            //            List<EMSExpenseDetailDTO> emsExpenseDetailDTO = result.GroupBy(k => new
            //            {
            //                k.EMSExpenseDetailID,
            //                k.Billable,
            //                k.BillableTo,
            //                k.Comment,
            //                k.EMSExpenseTypeMasterID,
            //                k.ExpenseTypeMasterText,
            //                k.ExpenseTypeStatusID,
            //                k.ExpenseStatus,
            //                k.ExpenseCreateDate,
            //                k.ExpenseModifiedDate,
            //                k.userID,
            //                k.ModifiedByuserID
            //            }).Select
            //                (x => new EMSExpenseDetailDTO
            //                {
            //                    EMSExpenseDetailID = x.Key.EMSExpenseDetailID,

            //                    EMSExpenseTypeMasterID = x.Key.EMSExpenseTypeMasterID,

            //                    ExpenseType = x.Key.ExpenseTypeMasterText,

            //                    Billable = x.Key.Billable
            //                    ,
            //                    BillableTo = x.Key.BillableTo
            //                    ,
            //                    Comment = x.Key.Comment
            //                    ,

            //                    Status = x.Key.ExpenseTypeStatusID,

            //                    ExpenseStatus = x.Key.ExpenseStatus,

            //                    // CreatedDate = Convert.ToDateTime(x.Key.ExpenseCreateDate)
            //                    // ,
            //                    CreatedDateStr = x.Key.ExpenseCreateDate.ToString("dd-MMM-yyyy HH:mm:ss"),

            //                    CreatedBy = x.Key.userID,

            //                    // ModifiedDate = Convert.ToDateTime(x.Key.ExpenseModifiedDate),

            //                    ModifiedDateStr = Convert.ToDateTime(x.Key.ExpenseModifiedDate).ToString("dd-MMM-yyyy HH:mm:ss"),
            //                    ModifiedBy = x.Key.ModifiedByuserID

            //                }).ToList();

            // }).ToList();

            //            foreach (var item in emsExpenseDetailDTO)
            //            {
            //                var response = result.Where(k => k.EMSExpenseDetailID == item.EMSExpenseDetailID).GroupBy(k =>
            //                    new { k.BillDate, k.EMSBillDetailID, k.Comment, k.Amount, k.BillNo }).Select(k =>
            //                        new EMSBillDetailDTO
            //                        {
            //                            EMSBillDetailID = k.Key.EMSBillDetailID,
            //                            BillNo = k.Key.BillNo,
            //                            Description = k.Key.Comment,
            //                            Amount = Convert.ToDecimal(k.Key.Amount),
            //                            BillDate = k.Key.BillDate,
            //                            EMSExpenseDetailID = k.Key.EMSBillDetailID

            //                        });
            //                List<EMSBillDetailDTO> emsDetailDTO = new List<EMSBillDetailDTO>();

            //                foreach (var subitem in response)
            //                {
            //                    List<EMSBillDocumentDetailDTO> emsDocumentDetailDTO = new List<EMSBillDocumentDetailDTO>();

            //                    var response1 = result.Where(k => k.EMSExpenseDetailID == item.EMSExpenseDetailID && k.EMSBillDetailID == subitem.EMSBillDetailID);
            //                    foreach (var subitem1 in response1)
            //                    {
            //                        if (subitem1.EMSBillDocumentDetailID > 0)
            //                            emsDocumentDetailDTO.Add(new EMSBillDocumentDetailDTO
            //                            {
            //                                EMSBillDocumentDetailID = subitem1.EMSBillDocumentDetailID,
            //                                DocumentName = subitem1.DocumentName,
            //                                EMSBillDetailID = subitem1.EMSBillDetailID,
            //                                CreatedBy = subitem1.userID

            //                            });
            //                    }

            //                    emsDetailDTO.Add(new EMSBillDetailDTO
            //                    {
            //                        EMSExpenseDetailID = subitem.EMSExpenseDetailID,
            //                        EMSBillDetailID = subitem.EMSBillDetailID,
            //                        BillDate = subitem.BillDate,
            //                        CreatedBy = subitem.CreatedBy,
            //                        ModifiedBy = subitem.ModifiedBy,
            //                        Description = subitem.Description,
            //                        Amount = Convert.ToDecimal(subitem.Amount),
            //                        BillNo = subitem.BillNo,
            //                        EMSBillDocumentDetails = emsDocumentDetailDTO
            //                    });
            //                }

            //                item.EMSBillDetails = emsDetailDTO.ToList();
            //            }
            #endregion
            List<EMSExpenseDetailDTO> Expenses = new List<EMSExpenseDetailDTO>();
            ObjectMapper.Map(SalesCatalystRepository.GetExpenseList(userID, emsExpenseDetailID), Expenses);
            return Expenses;
        }

        public string GetPendingWithName(int EMSExpenseDetailID, byte expenseMainStatus)
        {
            return (SalesCatalystRepository.GetPendingWithName(EMSExpenseDetailID,expenseMainStatus).ToString());
        }

        public bool IsExpenseEditable(int emsExpenseDetailID)
        {
            return (SalesCatalystRepository.IsExpenseEditable(emsExpenseDetailID));
        }

        public string GetCreatedByUsername(int emsExpenseDetailID)
        {
            return (SalesCatalystRepository.GetCreatedByUsername(emsExpenseDetailID));
        }
        /// <summary>
        /// //Used to update EMS bill document after uploading image (UpdateBillIamge)
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="billDocumentDetailID"></param>
        /// <returns></returns>
        public bool UpdateBillIamge(List<EMSBillDocumentDetailDTO> objEMSBillDocumentDetailDTO)
                        {
            bool isSuccess = false;
            List<EMSBillDocumentDetail> objEMSBillDocumentDetail = new List<EMSBillDocumentDetail>();
            ObjectMapper.Map(objEMSBillDocumentDetailDTO, objEMSBillDocumentDetail);

            return isSuccess = SalesCatalystRepository.UpdateBillIamge(objEMSBillDocumentDetail);
        }

        /// <summary>
        /// GetApproverType
        /// </summary>
        /// <param name="companyID"></param>
        /// <returns></returns>
        public List<CommonSetupBO> GetApproverType(int companyID)
        {
            List<CommonSetupBO> commonSetupBO = new List<CommonSetupBO>();

            foreach (var item in SalesCatalystRepository.GetApproverType(companyID))
                {
                CommonSetupBO objcommonSetupBO = new CommonSetupBO();
                ObjectMapper.Map(item, objcommonSetupBO);
                commonSetupBO.Add(objcommonSetupBO);
            }

            return commonSetupBO;
        }

        /// <summary>
        /// GetExpenseTypeMaster 
        /// </summary>
        /// <returns></returns>
        public List<ExpenseTypeMasterBO> GetExpenseTypeMaster(int companyID)
                    {
            List<ExpenseTypeMasterBO> expenseTypeMasterBO = new List<ExpenseTypeMasterBO>();

            foreach (var item in SalesCatalystRepository.GetExpenseTypeMaster(companyID))
                            {
                ExpenseTypeMasterBO objExpenseTypeMasterBO = new ExpenseTypeMasterBO();
                ObjectMapper.Map(item, objExpenseTypeMasterBO);
                expenseTypeMasterBO.Add(objExpenseTypeMasterBO);
            }

            return expenseTypeMasterBO;
                    }

        public List<GetExpenseApprovalPathBO> GetExpenseApprovalPathData(long? userID, int companyID, int? roleID, int approvalPathTypeID, int? emsExpenseTypeMasterId)
                    {
            List<GetExpenseApprovalPathBO> GetExpenseApprovalPathList = new List<GetExpenseApprovalPathBO>();
            ObjectMapper.Map(SalesCatalystRepository.GetExpenseApprovalPathData(userID, companyID, roleID, approvalPathTypeID, emsExpenseTypeMasterId), GetExpenseApprovalPathList);
            return GetExpenseApprovalPathList;
                }


        /// <summary>
        /// Submit Approval Data
        /// </summary>
        /// <param name="ApproverPathMaster"></param>
        /// <returns></returns>
        public int CreateApprovalPath(ApproverPathMasterBO ApproverPathMaster)
        {
            ApproverPathMaster ApproverPathMasterDTO = new ApproverPathMaster();
            ObjectMapper.Map(ApproverPathMaster, ApproverPathMasterDTO);
            return SalesCatalystRepository.CreateApprovalPath(ApproverPathMasterDTO);
            }

        /// <summary>
        /// Delete Approver
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="approverPathMasterID"></param>
        /// <returns></returns>
        public bool DeleteApprover(long UserID, int approverPathMasterID)
        {
            return SalesCatalystRepository.DeleteApprover(UserID, approverPathMasterID);
        }

        /// <summary>
        /// GetUserByTeamRole
        /// </summary>
        /// <param name="TeamID"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public List<spGetUserByTeamRoleBO> GetUserByTeamRole(int? TeamID, int? RoleID)
        {
            List<spGetUserByTeamRoleBO> objspGetUserByTeamRoleBO =  new List<spGetUserByTeamRoleBO>();
            ObjectMapper.Map(SalesCatalystRepository.GetUserByTeamRole(TeamID, RoleID), objspGetUserByTeamRoleBO);
            return objspGetUserByTeamRoleBO;
        }

        #endregion

        #region LMS Services
        /// <summary>
        /// Get LeaveType Master based on UserID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<LMSLeaveTypeMasterDTO> GetLeaveTypeMaster(long userID, int roleID)
        {
            List<LMSLeaveTypeMasterDTO> leaveMasters = new List<LMSLeaveTypeMasterDTO>();
            List<LeaveBalanceEntity> leavesTaken = SalesCatalystRepository.GetLeavesTaken(userID, roleID, DateTime.Now.AddDays(-4));
            ObjectMapper.Map(SalesCatalystRepository.GetLeaveTypeMaster(userID, roleID), leaveMasters);
            foreach (var item in leaveMasters)
            {
                var leaves = leavesTaken.FirstOrDefault(x => x.LMSLeaveTypeMasterID == item.LMSLeaveTypeMasterID);
                item.LeavesTaken = leaves == null ? 0 : leaves.LeavesTaken;
            }
            return leaveMasters;
        }
        /// <summary>
        /// Get LeaveTypeConfigurations based on RoleID        
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<LMSLeaveTypeConfigurationDTO> GetLeaveTypeCofiguration(long userID, int roleID)
        {
            List<LMSLeaveTypeConfigurationDTO> configurations = new List<LMSLeaveTypeConfigurationDTO>();
            ObjectMapper.Map(SalesCatalystRepository.GetLeaveTypeCofiguration(userID, roleID), configurations);
            return configurations;
        }
        /// <summary>
        /// Get Leave data based on request type or leaveTypeID
        /// </summary>
        /// <param name="userID">UserID for which leave needs to be fetched</param>
        /// <param name="LeaveTypeID"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        public List<LMSLeaveMasterDTO> GetLeaves(long userID, LMSLeaveRequestDTO LeaveRequest)
        {
            List<LMSLeaveMasterDTO> Leaves = new List<LMSLeaveMasterDTO>();
            ObjectMapper.Map(SalesCatalystRepository.GetLeaves(userID, LeaveRequest), Leaves);

            return Leaves;
        }
        /// <summary>
        /// Submit leaves
        /// </summary>
        /// <param name="userID">UserID for which leave needs to be fetched</param>
        /// <param name="LeaveTypeID"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        public bool SumbitLeaveRequest(long userID, int roleID, string ApproverRemarks, List<LMSLeaveMasterDTO> leaves)
        {
            List<LMSLeaveMaster> LeavesEntity = new List<LMSLeaveMaster>();
            ObjectMapper.Map(leaves, LeavesEntity);
            return SalesCatalystRepository.SumbitLeaveRequest(userID, roleID, ApproverRemarks, LeavesEntity);
        }

        public int SubmitLeavetypeConfig(LMSLeaveTypeConfigurationBO lmsconfig)
        {
            LMSLeaveTypeConfiguration config = new LMSLeaveTypeConfiguration();
            ObjectMapper.Map(lmsconfig, config);
            return SalesCatalystRepository.SubmitLeavetypeConfig(config);
        }

        #endregion

        #region Sales Return System

        public List<ApprovalPathTypeBO> GetApprovalPathType()
        {
            List<ApprovalPathTypeBO> result = new List<ApprovalPathTypeBO>();
            ObjectMapper.Map(SalesCatalystRepository.GetApprovalPathType(), result);
            return result;
        }

        public bool SumbitSalesReturnRequest(long userID, int roleID, string ApproverRemarks, List<SRRequestDTO> SRRequestData)
        {
            List<SRRequest> SRRequestEntity = new List<SRRequest>();
            ObjectMapper.Map(SRRequestData, SRRequestEntity);
            return SalesCatalystRepository.SumbitSalesReturnRequest(userID, roleID, ApproverRemarks, SRRequestEntity);
        }

        public List<SRRequestDTO> GetSalesReturn(long userID, GetSalesReturnRequestDTO SRRequest)
        {
            List<SRRequestDTO> SalesReturnRequest = new List<SRRequestDTO>();
            ObjectMapper.Map(SalesCatalystRepository.GetSalesReturn(userID, SRRequest), SalesReturnRequest);

            return SalesReturnRequest;
        }

        #endregion

        #region Syster of Sales Forecasting (SOSF)

        public long SubmitSOSFList(long UserID, List<SOSFMasterDTO> SOSFList)
        {
            List<SOSFMaster> SOSFList1 = new List<SOSFMaster>();
            ObjectMapper.Map(SOSFList, SOSFList1);
            return SalesCatalystRepository.SubmitSOSFList(UserID, SOSFList1);
        }

        public SOSFOutPutDTO GetUserSOSFData(long UserID, int StoreID, int ProductID)
        {
            SOSFOutPutDTO result = new SOSFOutPutDTO();
            int reqtype = 0;
            List<SOSFMasterDTO> res = new List<SOSFMasterDTO>();
            ObjectMapper.Map(SalesCatalystRepository.GetUserSOSFData(UserID, StoreID, ProductID, out reqtype), res);
            result.ReqType = reqtype;
            result.SOSFList = res;
            return result;
        }

        #endregion
    }
}
