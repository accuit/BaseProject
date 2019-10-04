using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using Samsung.SmartDost.PersistenceLayer.Repository.Contracts;
using Samsung.SmartDost.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System.Web;
using System.Transactions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using Samsung.SmartDost.CommonLayer.Aspects.Logging;
using System.ComponentModel;
using System.Data.Entity;
using System.Configuration;
using System.Data.EntityClient;
using System.Collections;
using System.Reflection;
using System.Data.Objects;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;

namespace Samsung.SmartDost.PersistenceLayer.Data.Impl
{
    public class SalesCatalystDataImpl : BaseDataImpl, ISalesCatalystRepository
    {
        #region MOM Creation, Updation and Search created by Neeraj Singh on 10 Dec 2015

        /// <summary>
        /// Search MOM using userid and MOM Title(Like)
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="MOMtitle"></param>
        /// <returns></returns>
        public List<MOMMaster> GetMOMSearchData(long UserID, string MOMTitle)
        {
            List<MOMMaster> result = SmartDostDbContext.MOMMasters.Where(x => x.IsDeleted == false && x.UserId == UserID && x.MomTitle.Contains(MOMTitle)).ToList();
            return result;
        }

        /// <summary>
        /// Get list of MOM created by a user
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        public List<MOMMaster> GetUserMOMData(long UserID, bool IsIncremental)
        {

            if (IsIncremental == false) // for APK
            {
                return SmartDostDbContext.MOMMasters.Where(m => m.UserId == UserID).OrderByDescending(m => m.MomDate).ToList();
            }
            else
            {
                return SmartDostDbContext.MOMMasters.Where(m => m.IsDeleted == false && m.UserId == UserID).OrderByDescending(m => m.MomDate).ToList();
            }

        }

        /// <summary>
        /// Get list of Attendees of Meeting in cosole.
        /// </summary>
        /// <param name="MOMID"></param>
        /// <returns></returns>
        public List<MOMAttendee> GetAttendeeList(long MOMID)
        {
            return SmartDostDbContext.MOMAttendees.Where(m => m.MomId == MOMID).OrderByDescending(m => m.AttendeeId).ToList();
        }

        /// <summary>
        /// Insert MOM data 
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="MOMtitle"></param>
        /// <param name="ActionItem"></param>
        /// <param name="Location"></param>
        /// <param name="Description"></param>
        /// <param name="MOMDate"></param>
        /// <returns></returns>
        public int SubmitMOMData(long UserID, string MOMTitle, string ActionItem, string Location, string Description, DateTime MOMDate, List<MOMAttendee> Attendees)
        {
            int status = 0;
            var result = SmartDostDbContext.MOMMasters.Add(new MOMMaster()
            {
                UserId = UserID,
                MomTitle = MOMTitle,
                ActionItem = ActionItem,
                Location = Location,
                Description = Description,
                MomDate = MOMDate,
                DateCreated = DateTime.Now,
                IsDeleted = false,
                MOMAttendees = Attendees
            });
            SmartDostDbContext.SaveChanges();
            status = 1;
            return status;
        }

        /// <summary>
        /// Insert List of meetings from APK
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="MOMList"></param>
        /// <returns></returns>
        public long SubmitMOMList(long UserID, MOMMaster MOMList)
        {

            MOMList.DateCreated = DateTime.Now;

            SmartDostDbContext.MOMMasters.Add(MOMList);
            SmartDostDbContext.SaveChanges();
            return MOMList.MomId;
        }

        /// <summary>
        /// Delete multiple or single meetings using service and console.
        /// </summary>
        /// <param name="momid"></param>
        /// <returns></returns>
        public int DeleteMOMData(List<int> momid)
        {
            int status = 0;
            foreach (int id in momid)
            {
                if (id > 0)
                {
                    MOMMaster mom = SmartDostDbContext.MOMMasters.FirstOrDefault(k => k.MomId == id);
                    mom.DateModified = System.DateTime.Now;
                    mom.IsDeleted = true;
                    SmartDostDbContext.SaveChanges();
                    status = 1;
                }
                else
                {
                    status = 0;
                }
            }
            return status;
        }

        /// <summary>
        /// Delete attendees from console.
        /// </summary>
        /// <param name="AttendeeId"></param>
        /// <returns></returns>
        public int DeleteAttendee(long AttendeeId)
        {
            int status = 0;
            var deletemomdata = SmartDostDbContext.MOMAttendees.Where(x => x.AttendeeId == AttendeeId);
            foreach (MOMAttendee mommaster in deletemomdata)
            {
                SmartDostDbContext.MOMAttendees.Remove(mommaster);
            }
            SmartDostDbContext.SaveChanges();
            status = 1;
            return status;
        }

        /// <summary>
        /// Update Meeting details along with attendees from Service and console.
        /// </summary>
        /// <param name="MOMId"></param>
        /// <param name="UserID"></param>
        /// <param name="MOMTitle"></param>
        /// <param name="ActionItem"></param>
        /// <param name="Location"></param>
        /// <param name="Description"></param>
        /// <param name="MOMDate"></param>
        /// <param name="Attendees"></param>
        /// <param name="IsIncremental"></param>
        /// <returns></returns>
        public int UpdateMOMData(long MOMId, long UserID, string MOMTitle, string ActionItem, string Location, string Description, DateTime MOMDate, List<MOMAttendee> Attendees, bool IsIncremental)
        {
            int status = 0;

            MOMMaster MOMdata = SmartDostDbContext.MOMMasters.FirstOrDefault(x => x.MomId == MOMId);
            if (MOMdata != null)
            {
                if (IsIncremental == false)
                {
                    var deleteoldattendee = SmartDostDbContext.MOMAttendees.Where(x => x.MomId == MOMId);
                    foreach (MOMAttendee momattendee in deleteoldattendee)
                    {
                        SmartDostDbContext.MOMAttendees.Remove(momattendee);
                    }
                }
                MOMdata.UserId = UserID;
                MOMdata.MomTitle = MOMTitle;
                MOMdata.ActionItem = ActionItem;
                MOMdata.Location = Location;
                MOMdata.Description = Description;
                MOMdata.MomDate = MOMDate;
                MOMdata.MOMAttendees = Attendees;
                MOMdata.DateModified = DateTime.Now;
                MOMdata.ModifiedBy = UserID;
                SmartDostDbContext.Entry<MOMMaster>(MOMdata).State = System.Data.EntityState.Modified;
                SmartDostDbContext.SaveChanges();
                status = 1;
            }
            return status;
        }

        #endregion


        #region Expense Management System By Vishnu on 10 Jan 2016

        /// <summary>
        /// Get All Expense Type Master for the company.
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
        public IList<EMSExpenseTypeMaster> GetExpenseMasterData(long userID, int companyID, int roleID, int RowCount, int StartRowIndex, DateTime? LastUpdatedDate, out bool HasMoreRows, out DateTime? MaxModifiedDate)
        {
            HasMoreRows = false;
            MaxModifiedDate = LastUpdatedDate;
            DateTime CurrentDateTime = System.DateTime.Now;

            IList<EMSExpenseTypeMaster> result = new List<EMSExpenseTypeMaster>();


            if (SmartDostDbContext.SPIsDownloadAuthorized(userID, Convert.ToInt32(AspectEnums.DownloadService.ExpenseMaster)).FirstOrDefault() == true)
            {
                if (LastUpdatedDate == null)
                {
                    result = SmartDostDbContext.EMSExpenseTypeMasters.Where(k => k.IsDeleted == false && k.IsActive == true && k.CompanyId == companyID)
                       .OrderBy(k => (k.ModifiedDate ?? k.CreatedDate))
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList(); //+1 is used to know that "is data more than rowcount is available" or not                       

                }
                else
                {

                    result = SmartDostDbContext.EMSExpenseTypeMasters.Where(k => (
                            (LastUpdatedDate < (k.CreatedDate))
                            ||
                            (LastUpdatedDate.Value == (k.ModifiedDate ?? k.CreatedDate))
                            ))
                       .OrderBy(k => (k.ModifiedDate ?? k.CreatedDate))
                       .Skip(StartRowIndex)
                       .Take(RowCount + 1).ToList();//+1 is used to know that "is data more than rowcount is available" or not            


                }

                HasMoreRows = result.Count > RowCount ? true : false;
                result = result.Take(RowCount).ToList();

                // Update last modified data among the data if available, else send the same modifieddate back  
                if (result.Count > 0)
                {
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
        /// Created By Vishnu Narayan Mishra 06 Jan 2016 to Submit Expense for Approval.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <param name="emsExpenseDetail"></param>
        /// <returns></returns>
        public int SubmitExpenseForApproval(long userID, int roleID, EMSExpenseDetail emsExpenseDetail)
        {
            DateTime createddate = DateTime.Now;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions
                   {
                       IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                   }))
            {
                if (emsExpenseDetail.EMSExpenseDetailID == 0)
                {
                    emsExpenseDetail.CreatedDate = createddate;
                    emsExpenseDetail.IsActive = true;
                    emsExpenseDetail.IsDeleted = false;
                    foreach (var bill in emsExpenseDetail.EMSBillDetails)
                    {
                        bill.CreatedDate = createddate;
                        bill.CreatedBy = Convert.ToInt32(userID);

                        foreach (var doc in bill.EMSBillDocumentDetails)
                        {
                            doc.CreatedDate = createddate;
                            doc.CreatedBy = Convert.ToInt32(userID);
                        }

                    }

                    SmartDostDbContext.EMSExpenseDetails.Add(emsExpenseDetail);
                    SmartDostDbContext.SaveChanges();
                    scope.Complete();
                    return emsExpenseDetail.EMSExpenseDetailID;
                }
                else
                {

                    int expenseDetailID = UpdateAddEMSExpenseDetails(emsExpenseDetail, userID, createddate);
                    // scope.Complete();
                    //  return emsExpenseDetail.EMSExpenseDetailID;

                    #region Bill details Add/Update

                    foreach (var item in emsExpenseDetail.EMSBillDetails)
                    {
                        bool IsSuccess = false;
                        int expenseBillDetailID = 0;
                        EMSBillDetail billDetail = new EMSBillDetail();
                        billDetail = item;
                        billDetail.EMSExpenseDetailID = emsExpenseDetail.EMSExpenseDetailID;
                        expenseBillDetailID = UpdateAddEMSBillDetail(billDetail, userID, createddate);
                        foreach (var subitem in billDetail.EMSBillDocumentDetails)
                        {
                            EMSBillDocumentDetail documentDetail = new EMSBillDocumentDetail();
                            documentDetail = subitem;
                            documentDetail.EMSBillDetailID = expenseBillDetailID;
                            IsSuccess = UpdateAddEMSBillDocument(documentDetail, userID, createddate);

                        }
                    }
                    scope.Complete();
                }
            }
                    #endregion

            return emsExpenseDetail.EMSExpenseDetailID;

            #region OLD Logic Update Expense and Bill/Document Details
            /*
            if (expenseDetailID > 0)//update
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                    new TransactionOptions
                    {
                        IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                    }))
                {

                    var result = SmartDostDbContext.EMSExpenseDetails.Where(x => x.EMSExpenseDetailID == expenseDetailID).ToList();

                    #region Update Expense

                    foreach (var itemExpense in result)
                    {
                        itemExpense.Status = emsExpenseDetail.Status;
                        itemExpense.ModifiedBy = Convert.ToInt32(userID);
                        itemExpense.ModifiedDate = System.DateTime.Now;
                        itemExpense.Billable = emsExpenseDetail.Billable;
                        itemExpense.Description = emsExpenseDetail.Description;
                        itemExpense.BillableTo = emsExpenseDetail.BillableTo;

                        SmartDostDbContext.Entry<EMSExpenseDetail>(itemExpense).State = System.Data.EntityState.Modified;

                        var emsBillDetails = SmartDostDbContext.EMSBillDetails.Where(x => x.EMSExpenseDetailID == itemExpense.EMSExpenseDetailID).ToList();

                        //Delete all Bills for a expense
                        foreach (var itemBill in emsBillDetails)
                        {
                            itemBill.IsActive = false;
                            itemBill.ModifiedBy = Convert.ToInt32(userID);
                            itemBill.ModifiedDate = System.DateTime.Now;

                            SmartDostDbContext.Entry<EMSBillDetail>(itemBill).State = System.Data.EntityState.Modified;

                            var EMSBillDocumentDetails = SmartDostDbContext.EMSBillDocumentDetails.Where(x => x.EMSBillDetailID == itemBill.EMSBillDetailID).ToList();


                            if (EMSBillDocumentDetails != null && EMSBillDocumentDetails.Count > 0)
                            {
                                //Delete all Bill Documents for a Bill
                                foreach (var billdocDetail in EMSBillDocumentDetails)
                                {
                                    billdocDetail.IsActive = false;
                                    billdocDetail.ModifiedBy = Convert.ToInt32(userID);
                                    billdocDetail.ModifiedDate = System.DateTime.Now;
                                    SmartDostDbContext.Entry<EMSBillDocumentDetail>(billdocDetail).State = System.Data.EntityState.Modified;

                                }
                            }

                        }

                        //Add New bills for a Expense
                        foreach (var newitemExpense in result)
                        {
                            foreach (var newItemBill in newitemExpense.EMSBillDetails)
                            {

                                EMSBillDetail newEMSBillDetail = new EMSBillDetail()
                                {

                                    EMSExpenseDetailID = newItemBill.EMSExpenseDetailID,
                                    Amount = newItemBill.Amount,
                                    BillDate = newItemBill.BillDate,
                                    CreatedBy = newItemBill.CreatedBy,
                                    Description = newItemBill.Description,
                                    BillNo = newItemBill.BillNo,
                                    IsActive = newItemBill.IsActive,
                                    IsDeleted = newItemBill.IsDeleted
                                };
                                SmartDostDbContext.EMSBillDetails.Add(newEMSBillDetail);

                                foreach (var newEMSBillDocDet in newEMSBillDetail.EMSBillDocumentDetails)
                                {

                                    EMSBillDocumentDetail newEMSBillDocumentDetail = new EMSBillDocumentDetail()
                                    {
                                        EMSBillDetailID = newItemBill.EMSBillDetailID,
                                        DocumentName = newEMSBillDocDet.DocumentName,
                                        IsActive = newEMSBillDocDet.IsActive,
                                        IsDeleted = newEMSBillDocDet.IsDeleted,
                                        CreatedBy = newEMSBillDocDet.CreatedBy
                                    };
                                    SmartDostDbContext.EMSBillDocumentDetails.Add(newEMSBillDocumentDetail);
                                }
                            }
                        }

                    }
                    #endregion
                }
            }

            return IsSuccess;*/

            #endregion
        }

        public List<EMSExpenseDetailDTO> SubmitExpenseAndBill(long userID, int roleID, List<EMSExpenseDetailDTO> emsExpenseDetail)
        {
            byte approvalPathTypeID = (byte)AspectEnums.CheckModule.ExpenseManagementSystem; //1. EMS 2. LMS


            DateTime createddate = DateTime.Now;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
                   new TransactionOptions
                   {
                       IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
                   }))
            {
                foreach (var emslist in emsExpenseDetail)
                {

                    if (emslist.EMSExpenseDetailIDServer == 0)
                    {
                        #region Expense Submission Method
                        EMSExpenseDetail EMS = new EMSExpenseDetail();
                        EMS.CreatedDate = createddate;
                        EMS.Billable = emslist.Billable;
                        EMS.BillableTo = emslist.BillableTo;

                        EMS.EMSExpenseDetailID = emslist.EMSExpenseDetailID;
                        EMS.EMSExpenseTypeMasterID = emslist.EMSExpenseTypeMasterID;

                        var approverType = SmartDostDbContext.ApproverPathMasters.Where(a => a.EMSExpenseTypeMasterId == emslist.EMSExpenseTypeMasterID &&
                            a.RoleID == roleID && a.ApproverTypeID == (byte)AspectEnums.ApproverType.Approval_Not_required
                            && a.IsActive && !a.IsDeleted).FirstOrDefault();
                        if (approverType != null)
                        {
                            EMS.Status = (byte)AspectEnums.ApprovalStatus.ApprovalNotRequired;
                            EMS.Comment = emslist.Comment + " (Auto Approved) ";
                        }
                        else
                        {
                            EMS.Status = emslist.Status;
                            EMS.Comment = emslist.Comment;
                        }
                        //EMS.Status = emslist.Status;

                        EMS.CreatedBy = Convert.ToInt32(userID);
                        EMS.IsActive = true;
                        EMS.IsDeleted = false;
                        //EMS.EMSBillDetails = emslist.EMSBillDetails;
                        SmartDostDbContext.EMSExpenseDetails.Add(EMS);
                        SmartDostDbContext.SaveChanges();


                        emslist.EMSExpenseDetailIDServer = EMS.EMSExpenseDetailID;
                        List<EMSBillDetail> bills = new List<EMSBillDetail>();
                        foreach (var bill in emslist.EMSBillDetails)
                        {

                            EMSBillDetail Bill = new EMSBillDetail();
                            Bill.EMSExpenseDetailID = bill.EMSExpenseDetailID;
                            Bill.BillNo = bill.BillNo;
                            Bill.BillDate = Convert.ToDateTime(bill.BillDateStr);
                            Bill.Description = bill.Description;
                            Bill.Amount = bill.Amount;
                            Bill.CreatedDate = createddate;
                            Bill.IsActive = true;
                            Bill.IsDeleted = false;
                            Bill.CreatedBy = Convert.ToInt32(userID);
                            Bill.EMSExpenseDetailID = EMS.EMSExpenseDetailID;
                            SmartDostDbContext.EMSBillDetails.Add(Bill);
                            SmartDostDbContext.SaveChanges();
                            bill.EMSBillDetailIDServer = Bill.EMSBillDetailID;

                            foreach (var doc in bill.EMSBillDocumentDetails)
                            {
                                EMSBillDocumentDetail DOC = new EMSBillDocumentDetail();
                                DOC.EMSBillDetailID = Bill.EMSBillDetailID;
                                //DOC.EMSBillDetailID = EMS.EMSExpenseDetailID;
                                DOC.DocumentName = "";
                                DOC.CreatedDate = createddate;
                                DOC.CreatedBy = Convert.ToInt32(userID);
                                DOC.IsActive = true;
                                DOC.IsDeleted = false;
                                SmartDostDbContext.EMSBillDocumentDetails.Add(DOC);
                                SmartDostDbContext.SaveChanges();
                                doc.EMSBillDocumentDetailIDServer = DOC.EMSBillDocumentDetailID;
                            }
                        }

                        // Approver History for New Approval
                        #endregion

                        #region Assign Approver to Next User

                        ObjectParameter AssignedToUser = new ObjectParameter("AssignedToUser", typeof(int));
                        ObjectParameter ApproverTypeID = new ObjectParameter("ApproverTypeID", typeof(int));


                        //SmartDostDbContext.spAssignApprovalToApprover(Convert.ToInt32(EMS.CreatedBy), approvalPathTypeID, EMS.EMSExpenseTypeMasterID, EMS.EMSExpenseDetailID, AssignedToUser, ApproverTypeID);

                        //if (AssignedToUser.Value != null && ApproverTypeID.Value != null && emslist.Status != (int)AspectEnums.ApprovalStatus.Rejected && emslist.Status != (int)AspectEnums.ApprovalStatus.Cancelled)
                        //{
                        //    while (Convert.ToByte(ApproverTypeID.Value) == (byte)AspectEnums.ApproverType.Notification && int.Parse(AssignedToUser.Value.ToString()) > 0) //Notification in the approval assigned to next approver.
                        //    {
                        //        SmartDostDbContext.spAssignApprovalToApprover(Convert.ToInt32(EMS.CreatedBy), approvalPathTypeID, EMS.EMSExpenseTypeMasterID, EMS.EMSExpenseDetailID, AssignedToUser, ApproverTypeID);
                        //    }
                        //}

                        //Asigned Approval to Next Approver
                        var pendingUser = SmartDostDbContext.ApprovalStatusHistories.Where(a => a.EMSExpenseDetailID == EMS.EMSExpenseDetailID && a.IsActive && !a.IsDeleted && a.Status == (int)AspectEnums.ApprovalStatus.Pending).OrderByDescending(a => a.Sequence).FirstOrDefault();

                        //Prevent assignment to next user untill approval status is pending for next user
                        if (pendingUser == null || pendingUser.Status != (int)AspectEnums.ApprovalStatus.Pending)
                        {
                            SmartDostDbContext.spAssignApprovalToApprover(Convert.ToInt32(EMS.CreatedBy), approvalPathTypeID, EMS.EMSExpenseTypeMasterID, EMS.EMSExpenseDetailID, AssignedToUser, ApproverTypeID);

                            if (Convert.ToInt32(AssignedToUser.Value) > 0 && Convert.ToInt32(ApproverTypeID.Value) > 0 && emslist.Status != (int)AspectEnums.ApprovalStatus.Rejected || emslist.Status != (int)AspectEnums.ApprovalStatus.Cancelled)
                            {
                                while (Convert.ToByte(ApproverTypeID.Value) == (byte)AspectEnums.ApproverType.Notification && int.Parse(AssignedToUser.Value.ToString()) > 0) //Notification in the approval assigned to next approver.
                                {
                                    SmartDostDbContext.spAssignApprovalToApprover(Convert.ToInt32(EMS.CreatedBy), approvalPathTypeID, EMS.EMSExpenseTypeMasterID, EMS.EMSExpenseDetailID, AssignedToUser, ApproverTypeID);
                                }
                            }
                        }
                        #endregion

                    }

                    else
                    {
                        var EMS = SmartDostDbContext.EMSExpenseDetails.FirstOrDefault(x => x.EMSExpenseDetailID == emslist.EMSExpenseDetailIDServer && x.IsActive == true && x.IsDeleted == false);

                        if (emslist.Status == (int)AspectEnums.ApprovalStatus.Rejected)
                        {
                            // if expense is cancelled or rejected: next approver will not be notified and status will be updated
                            var lastuser = SmartDostDbContext.ApprovalStatusHistories.Where(a => a.EMSExpenseDetailID == EMS.EMSExpenseDetailID && a.IsActive && !a.IsDeleted && a.Status != (int)AspectEnums.ApprovalStatus.Notification).OrderByDescending(a => a.Sequence).FirstOrDefault();
                            lastuser.Status = emslist.Status;
                            lastuser.ModifiedBy = Convert.ToInt32(userID);
                            lastuser.ModifiedDate = createddate;
                            SmartDostDbContext.Entry<ApprovalStatusHistory>(lastuser).State = System.Data.EntityState.Modified;

                            //update main expense status
                            EMS.Status = (int)AspectEnums.ApprovalStatus.Rejected;
                        }

                        else if (emslist.Status == (int)AspectEnums.ApprovalStatus.Approved)
                        {
                            var lastuser = SmartDostDbContext.ApprovalStatusHistories.Where(a => a.EMSExpenseDetailID == EMS.EMSExpenseDetailID && a.IsActive && !a.IsDeleted && a.Status != (int)AspectEnums.ApprovalStatus.Notification).OrderByDescending(a => a.Sequence).FirstOrDefault();
                            lastuser.Status = emslist.Status;
                            lastuser.ModifiedBy = Convert.ToInt32(userID);
                            lastuser.ModifiedDate = createddate;
                            SmartDostDbContext.Entry<ApprovalStatusHistory>(lastuser).State = System.Data.EntityState.Modified;
                        }

                        #region Expense Updation

                        EMS.ModifiedDate = createddate;
                        EMS.Billable = emslist.Billable;
                        EMS.BillableTo = emslist.BillableTo;
                        EMS.Comment = emslist.Comment;
                        //EMS.Status = emslist.Status;
                        EMS.ModifiedBy = Convert.ToInt32(userID);
                        EMS.IsActive = true;
                        EMS.IsDeleted = false;
                        SmartDostDbContext.Entry<EMSExpenseDetail>(EMS).State = System.Data.EntityState.Modified;
                        SmartDostDbContext.SaveChanges();
                        emslist.EMSExpenseDetailIDServer = EMS.EMSExpenseDetailID;
                        List<EMSBillDetail> bills = new List<EMSBillDetail>();
                        foreach (var bill in emslist.EMSBillDetails)
                        {
                            EMSBillDetail Bill = new EMSBillDetail();
                            if (bill.EMSBillDetailIDServer == 0)
                            {
                                #region create bill if not created
                                Bill.EMSExpenseDetailID = bill.EMSExpenseDetailID;
                                Bill.BillNo = bill.BillNo;
                                Bill.BillDate = Convert.ToDateTime(bill.BillDateStr);
                                Bill.Description = bill.Description;
                                Bill.Amount = bill.Amount;
                                Bill.CreatedDate = createddate;
                                Bill.IsActive = true;
                                Bill.IsDeleted = false;
                                Bill.CreatedBy = Convert.ToInt32(userID);
                                Bill.EMSExpenseDetailID = EMS.EMSExpenseDetailID;
                                SmartDostDbContext.EMSBillDetails.Add(Bill);
                                SmartDostDbContext.SaveChanges();
                                bill.EMSBillDetailIDServer = Bill.EMSBillDetailID;
                                foreach (var doc in bill.EMSBillDocumentDetails)
                                {

                                    EMSBillDocumentDetail DOC = new EMSBillDocumentDetail();
                                    DOC.EMSBillDetailID = Bill.EMSBillDetailID;
                                    DOC.DocumentName = "";
                                    DOC.CreatedDate = createddate;
                                    DOC.CreatedBy = Convert.ToInt32(userID);
                                    DOC.IsActive = true;
                                    DOC.IsDeleted = false;
                                    SmartDostDbContext.EMSBillDocumentDetails.Add(DOC);
                                    SmartDostDbContext.SaveChanges();
                                    doc.EMSBillDocumentDetailIDServer = DOC.EMSBillDocumentDetailID;
                                }
                                #endregion
                            }
                            else
                            {
                                #region Update bill if already exist
                                var UBill = SmartDostDbContext.EMSBillDetails.FirstOrDefault(x => x.EMSBillDetailID == bill.EMSBillDetailIDServer);

                                UBill.BillNo = bill.BillNo;
                                UBill.Description = bill.Description;
                                UBill.Amount = bill.Amount;
                                UBill.ModifiedDate = createddate;
                                UBill.IsActive = bill.IsActive;
                                UBill.IsDeleted = bill.IsDeleted;
                                UBill.ModifiedBy = Convert.ToInt32(userID);
                                SmartDostDbContext.Entry<EMSBillDetail>(UBill).State = System.Data.EntityState.Modified;
                                SmartDostDbContext.SaveChanges();
                                bill.EMSBillDetailIDServer = UBill.EMSBillDetailID;
                                foreach (var doc in bill.EMSBillDocumentDetails)
                                {
                                    if (doc.EMSBillDocumentDetailIDServer == 0)
                                    {
                                        EMSBillDocumentDetail NewDOC = new EMSBillDocumentDetail();
                                        NewDOC.EMSBillDetailID = bill.EMSBillDetailIDServer;
                                        NewDOC.DocumentName = doc.DocumentName;
                                        NewDOC.CreatedDate = createddate;
                                        NewDOC.CreatedBy = Convert.ToInt32(userID);
                                        NewDOC.IsActive = true;
                                        NewDOC.IsDeleted = false;
                                        SmartDostDbContext.EMSBillDocumentDetails.Add(NewDOC);
                                        SmartDostDbContext.SaveChanges();
                                        doc.EMSBillDocumentDetailIDServer = NewDOC.EMSBillDocumentDetailID;
                                    }
                                    else
                                    {
                                        var DOC = SmartDostDbContext.EMSBillDocumentDetails.FirstOrDefault(x => x.EMSBillDocumentDetailID == doc.EMSBillDocumentDetailIDServer);
                                        if (DOC != null)
                                        {
                                            DOC.DocumentName = doc.DocumentName;
                                            DOC.ModifiedDate = createddate;
                                            DOC.ModifiedBy = Convert.ToInt32(userID);
                                            DOC.IsActive = doc.IsActive;
                                            DOC.IsDeleted = doc.IsDeleted;
                                            SmartDostDbContext.Entry<EMSBillDocumentDetail>(DOC).State = System.Data.EntityState.Modified;
                                            SmartDostDbContext.SaveChanges();
                                            doc.EMSBillDocumentDetailIDServer = DOC.EMSBillDocumentDetailID;
                                        }
                                    }
                                }
                                #endregion
                            }

                        }

                        #endregion

                        #region Assign Approver to Next User in case of Update
                        //Asigned Approval to Next Approver
                        ObjectParameter AssignedToUser = new ObjectParameter("AssignedToUser", typeof(int));
                        ObjectParameter ApproverTypeID = new ObjectParameter("ApproverTypeID", typeof(int));

                        //if (emslist.Status != (int)AspectEnums.ApprovalStatus.Rejected || emslist.Status != (int)AspectEnums.ApprovalStatus.Cancelled)
                        //{
                        //    EMS.Status = emslist.Status;
                        //    SmartDostDbContext.Entry<EMSExpenseDetail>(EMS).State = System.Data.EntityState.Modified;
                        //    SmartDostDbContext.SaveChanges();
                        //}
                        //else
                        //{
                        //    //Asigned Approval to Next Approver
                        var pendingUser = SmartDostDbContext.ApprovalStatusHistories.Where(a => a.EMSExpenseDetailID == EMS.EMSExpenseDetailID && a.IsActive && !a.IsDeleted).OrderByDescending(a => a.Sequence).FirstOrDefault(); //&& a.Status == (int)AspectEnums.ApprovalStatus.Pending

                        //Prevent assignment to next user untill approval status is pending for next user
                        if (pendingUser == null || pendingUser.Status == (int)AspectEnums.ApprovalStatus.Approved)
                        {
                            SmartDostDbContext.spAssignApprovalToApprover(Convert.ToInt32(EMS.CreatedBy), approvalPathTypeID, EMS.EMSExpenseTypeMasterID, EMS.EMSExpenseDetailID, AssignedToUser, ApproverTypeID);

                            if (Convert.ToInt32(AssignedToUser.Value) > 0 && Convert.ToInt32(ApproverTypeID.Value) > 0 && (emslist.Status != (int)AspectEnums.ApprovalStatus.Rejected || emslist.Status != (int)AspectEnums.ApprovalStatus.Cancelled))
                            {
                                while (Convert.ToByte(ApproverTypeID.Value) == (byte)AspectEnums.ApproverType.Notification && int.Parse(AssignedToUser.Value.ToString()) > 0) //Notification in the approval assigned to next approver.
                                {
                                    SmartDostDbContext.spAssignApprovalToApprover(Convert.ToInt32(EMS.CreatedBy), approvalPathTypeID, EMS.EMSExpenseTypeMasterID, EMS.EMSExpenseDetailID, AssignedToUser, ApproverTypeID);
                                }
                                //handled case when last approver is in notification then change main status.
                                var pendingUserNew = SmartDostDbContext.ApprovalStatusHistories.Where(a => a.EMSExpenseDetailID == EMS.EMSExpenseDetailID && a.IsActive && !a.IsDeleted).OrderByDescending(a => a.Sequence).FirstOrDefault(); //&& a.Status == (int)AspectEnums.ApprovalStatus.Pending
                                if (AssignedToUser == null || pendingUserNew.Status == (byte)AspectEnums.ApprovalStatus.Notification)
                                {
                                    EMS.Status = emslist.Status;
                                    SmartDostDbContext.Entry<EMSExpenseDetail>(EMS).State = System.Data.EntityState.Modified;
                                    SmartDostDbContext.SaveChanges();
                                }
                                //handled case when last approver is in notification then change main status.
                            }
                            else
                            {
                                //Handle notification condition here
                                EMS.Status = emslist.Status;
                                SmartDostDbContext.Entry<EMSExpenseDetail>(EMS).State = System.Data.EntityState.Modified;
                                SmartDostDbContext.SaveChanges();
                            }
                        }
                        else
                        {
                            //Handle notification condition here
                            EMS.Status = emslist.Status;
                            SmartDostDbContext.Entry<EMSExpenseDetail>(EMS).State = System.Data.EntityState.Modified;
                            SmartDostDbContext.SaveChanges();
                        }
                        //}


                        #endregion

                    }
                }
                scope.Complete();

            }

            return emsExpenseDetail;


        }

        #region Update Expense and corresponding tables
        /// <summary>
        /// UpdateAddEMSExpenseDetails
        /// </summary>
        /// <param name="expenseDetail"></param>
        /// <param name="userID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public int UpdateAddEMSExpenseDetails(EMSExpenseDetail expenseDetail, long userID, DateTime date)
        {
            int expenseDetailID = 0;

            var itemExpense = SmartDostDbContext.EMSExpenseDetails.FirstOrDefault(x => x.EMSExpenseDetailID == expenseDetail.EMSExpenseDetailID && x.IsActive == true && x.IsDeleted == false);

            #region Update Expense
            if (itemExpense != null)
            {
                itemExpense.Status = expenseDetail.Status;
                itemExpense.EMSExpenseTypeMasterID = expenseDetail.EMSExpenseTypeMasterID;
                itemExpense.Billable = expenseDetail.Billable;
                itemExpense.Comment = expenseDetail.Comment;
                itemExpense.BillableTo = expenseDetail.BillableTo;
                itemExpense.ModifiedBy = Convert.ToInt32(userID);
                itemExpense.ModifiedDate = date;
                SmartDostDbContext.Entry<EMSExpenseDetail>(itemExpense).State = System.Data.EntityState.Modified;
                SmartDostDbContext.SaveChanges();
                expenseDetailID = itemExpense.EMSExpenseDetailID;
            }
            #endregion

            #region Add Expense
            //else
            //{
            //    EMSExpenseDetail emsDetail = new EMSExpenseDetail();
            //    emsDetail.Status = expenseDetail.Status;
            //    emsDetail.IsActive = true;
            //    emsDetail.IsDeleted = false;
            //    emsDetail.EMSExpenseTypeMasterID = expenseDetail.EMSExpenseTypeMasterID;
            //    emsDetail.CreatedBy = Convert.ToInt32(userID);
            //    emsDetail.CreatedDate = date;
            //    emsDetail.Billable = expenseDetail.Billable;
            //    emsDetail.Comment = expenseDetail.Comment;
            //    emsDetail.BillableTo = expenseDetail.BillableTo;
            //    SmartDostDbContext.EMSExpenseDetails.Add(emsDetail);
            //    SmartDostDbContext.SaveChanges();
            //    //expenseDetailID = emsDetail.EMSExpenseDetailID;//expenseDetail.EMSExpenseDetailID;
            //}
            #endregion

            return expenseDetailID;
        }
        public int UpdateAddEMSExpenseDetails(EMSExpenseDetailDTO expenseDetail, long userID, DateTime date)
        {
            int expenseDetailID = 0;

            var itemExpense = SmartDostDbContext.EMSExpenseDetails.FirstOrDefault(x => x.EMSExpenseDetailID == expenseDetail.EMSExpenseDetailID && x.IsActive == true && x.IsDeleted == false);

            #region Update Expense
            if (itemExpense != null)
            {
                itemExpense.Status = expenseDetail.Status;
                itemExpense.EMSExpenseTypeMasterID = expenseDetail.EMSExpenseTypeMasterID;
                itemExpense.Billable = expenseDetail.Billable;
                itemExpense.Comment = expenseDetail.Comment;
                itemExpense.BillableTo = expenseDetail.BillableTo;
                itemExpense.ModifiedBy = Convert.ToInt32(userID);
                itemExpense.ModifiedDate = date;
                SmartDostDbContext.Entry<EMSExpenseDetail>(itemExpense).State = System.Data.EntityState.Modified;
                SmartDostDbContext.SaveChanges();
                expenseDetailID = itemExpense.EMSExpenseDetailID;
            }
            #endregion

            return expenseDetailID;
        }

        /// <summary>
        /// UpdateAddEMSBillDetail
        /// </summary>
        /// <param name="expenseDetail"></param>
        /// <param name="userID"></param>
        /// <param name="dat"></param>
        /// <returns></returns>
        public int UpdateAddEMSBillDetail(EMSBillDetail expenseDetail, long userID, DateTime date)
        {
            int expenseDetailID = 0;

            var itemBill = SmartDostDbContext.EMSBillDetails.Where(x => x.EMSExpenseDetailID == expenseDetail.EMSExpenseDetailID && x.IsActive == true && x.IsDeleted == false).ToList();

            foreach (var item in itemBill)
            {
                #region Update Expense

                var itemExpense = SmartDostDbContext.EMSBillDetails.FirstOrDefault(k => k.EMSBillDetailID == item.EMSBillDetailID);
                if (itemExpense != null)
                {
                    itemExpense.IsActive = expenseDetail.IsActive;
                    itemExpense.ModifiedBy = Convert.ToInt32(userID);
                    itemExpense.ModifiedDate = date;
                    // itemExpense.BillDate = Convert.ToDateTime(expenseDetail.BillDateStr);
                    itemExpense.BillNo = expenseDetail.BillNo;
                    itemExpense.Description = expenseDetail.Description;
                    itemExpense.Amount = expenseDetail.Amount;
                    itemExpense.IsDeleted = expenseDetail.IsDeleted;

                    SmartDostDbContext.Entry<EMSBillDetail>(itemExpense).State = System.Data.EntityState.Modified;

                    SmartDostDbContext.SaveChanges();

                    expenseDetailID = itemExpense.EMSBillDetailID;
                }

                #endregion

                #region Add Bill Details

                else
                {
                    EMSBillDetail emsDetail = new EMSBillDetail();
                    emsDetail.EMSExpenseDetailID = expenseDetail.EMSExpenseDetailID;
                    //emsDetail.BillDate = Convert.ToDateTime(expenseDetail.BillDateStr);
                    emsDetail.BillNo = expenseDetail.BillNo;
                    emsDetail.Description = expenseDetail.Description;
                    emsDetail.IsActive = true;
                    emsDetail.Amount = expenseDetail.Amount;
                    emsDetail.IsDeleted = false;
                    emsDetail.CreatedDate = date;
                    emsDetail.CreatedBy = expenseDetail.CreatedBy;

                    SmartDostDbContext.EMSBillDetails.Add(emsDetail);

                    SmartDostDbContext.SaveChanges();

                    expenseDetailID = emsDetail.EMSBillDetailID;
                }

                #endregion
            }

            #region Add Bill Details
            if (itemBill.Count == 0 || itemBill == null)
            {
                EMSBillDetail emsDetail = new EMSBillDetail();
                //itemExpense.ModifiedBy = Convert.ToInt32(userID);
                emsDetail.EMSExpenseDetailID = expenseDetail.EMSExpenseDetailID;
                // emsDetail.BillDate = Convert.ToDateTime(expenseDetail.BillDateStr);
                emsDetail.BillNo = expenseDetail.BillNo;
                emsDetail.Description = expenseDetail.Description;
                emsDetail.IsActive = true;
                emsDetail.Amount = expenseDetail.Amount;
                emsDetail.IsDeleted = false;
                emsDetail.CreatedDate = date;
                emsDetail.CreatedBy = expenseDetail.CreatedBy;

                SmartDostDbContext.EMSBillDetails.Add(emsDetail);

                SmartDostDbContext.SaveChanges();

                expenseDetailID = emsDetail.EMSBillDetailID;
            }
            #endregion


            return expenseDetailID;
        }
        public int UpdateAddEMSBillDetail(EMSBillDetailDTO expenseDetail, long userID, DateTime date)
        {
            int expenseDetailID = 0;

            var itemBill = SmartDostDbContext.EMSBillDetails.Where(x => x.EMSExpenseDetailID == expenseDetail.EMSExpenseDetailID && x.IsActive == true && x.IsDeleted == false).ToList();

            foreach (var item in itemBill)
            {
                #region Update Expense

                var itemExpense = SmartDostDbContext.EMSBillDetails.FirstOrDefault(k => k.EMSBillDetailID == item.EMSBillDetailID);
                if (itemExpense != null)
                {
                    itemExpense.IsActive = expenseDetail.IsActive;
                    itemExpense.ModifiedBy = Convert.ToInt32(userID);
                    itemExpense.ModifiedDate = date;
                    // itemExpense.BillDate = Convert.ToDateTime(expenseDetail.BillDateStr);
                    itemExpense.BillNo = expenseDetail.BillNo;
                    itemExpense.Description = expenseDetail.Description;
                    itemExpense.Amount = expenseDetail.Amount;
                    itemExpense.IsDeleted = expenseDetail.IsDeleted;

                    SmartDostDbContext.Entry<EMSBillDetail>(itemExpense).State = System.Data.EntityState.Modified;

                    SmartDostDbContext.SaveChanges();

                    expenseDetailID = itemExpense.EMSBillDetailID;
                }

                #endregion

                #region Add Bill Details

                else
                {
                    EMSBillDetail emsDetail = new EMSBillDetail();
                    emsDetail.EMSExpenseDetailID = expenseDetail.EMSExpenseDetailID;
                    //emsDetail.BillDate = Convert.ToDateTime(expenseDetail.BillDateStr);
                    emsDetail.BillNo = expenseDetail.BillNo;
                    emsDetail.Description = expenseDetail.Description;
                    emsDetail.IsActive = true;
                    emsDetail.Amount = expenseDetail.Amount;
                    emsDetail.IsDeleted = false;
                    emsDetail.CreatedDate = date;
                    emsDetail.CreatedBy = expenseDetail.CreatedBy;

                    SmartDostDbContext.EMSBillDetails.Add(emsDetail);

                    SmartDostDbContext.SaveChanges();

                    expenseDetailID = emsDetail.EMSBillDetailID;
                }

                #endregion
            }

            #region Add Bill Details
            if (itemBill.Count == 0 || itemBill == null)
            {
                EMSBillDetail emsDetail = new EMSBillDetail();
                //itemExpense.ModifiedBy = Convert.ToInt32(userID);
                emsDetail.EMSExpenseDetailID = expenseDetail.EMSExpenseDetailID;
                // emsDetail.BillDate = Convert.ToDateTime(expenseDetail.BillDateStr);
                emsDetail.BillNo = expenseDetail.BillNo;
                emsDetail.Description = expenseDetail.Description;
                emsDetail.IsActive = true;
                emsDetail.Amount = expenseDetail.Amount;
                emsDetail.IsDeleted = false;
                emsDetail.CreatedDate = date;
                emsDetail.CreatedBy = expenseDetail.CreatedBy;

                SmartDostDbContext.EMSBillDetails.Add(emsDetail);

                SmartDostDbContext.SaveChanges();

                expenseDetailID = emsDetail.EMSBillDetailID;
            }
            #endregion


            return expenseDetailID;
        }
        /// <summary>
        /// UpdateAddEMSBillDocument
        /// </summary>
        /// <param name="expenseDetail"></param>
        /// <returns></returns>
        public bool UpdateAddEMSBillDocument(EMSBillDocumentDetail expenseBillDocDetail, long userID, DateTime date)
        {
            bool isSuccess = false;

            var itemBillDocs = SmartDostDbContext.EMSBillDocumentDetails.Where(x => x.EMSBillDocumentDetailID == expenseBillDocDetail.EMSBillDocumentDetailID && x.IsActive == true && x.IsDeleted == false).ToList();

            foreach (var item in itemBillDocs)
            {
                #region Update Bill Document

                var itemBillDoc = SmartDostDbContext.EMSBillDocumentDetails.FirstOrDefault(k => k.EMSBillDocumentDetailID == item.EMSBillDocumentDetailID);
                if (itemBillDoc != null)
                {
                    itemBillDoc.IsActive = expenseBillDocDetail.IsActive;
                    itemBillDoc.IsDeleted = expenseBillDocDetail.IsDeleted;
                    itemBillDoc.ModifiedBy = Convert.ToInt32(userID);
                    itemBillDoc.DocumentName = expenseBillDocDetail.DocumentName;
                    itemBillDoc.ModifiedDate = date;
                    SmartDostDbContext.Entry<EMSBillDocumentDetail>(itemBillDoc).State = System.Data.EntityState.Modified;
                    isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;

                    //Delete uploaded files from application
                }
                #endregion

                #region Add Bill Documents

                else
                {
                    EMSBillDocumentDetail emsBilldoc = new EMSBillDocumentDetail();
                    emsBilldoc.DocumentName = expenseBillDocDetail.DocumentName;
                    emsBilldoc.EMSBillDetailID = expenseBillDocDetail.EMSBillDetailID;
                    emsBilldoc.IsActive = true;
                    emsBilldoc.IsDeleted = false;
                    emsBilldoc.CreatedBy = expenseBillDocDetail.CreatedBy;
                    emsBilldoc.CreatedDate = date;
                    SmartDostDbContext.EMSBillDocumentDetails.Add(emsBilldoc);
                    isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;

                    //Added new documents files 
                }

                #endregion
            }

            #region Add Bill Documents
            if (itemBillDocs.Count == 0 || itemBillDocs == null)
            {
                EMSBillDocumentDetail emsBilldoc = new EMSBillDocumentDetail();
                emsBilldoc.DocumentName = expenseBillDocDetail.DocumentName;
                emsBilldoc.EMSBillDetailID = expenseBillDocDetail.EMSBillDetailID;
                emsBilldoc.IsActive = true;
                emsBilldoc.IsDeleted = false;
                emsBilldoc.CreatedBy = expenseBillDocDetail.CreatedBy;
                emsBilldoc.CreatedDate = date;
                SmartDostDbContext.EMSBillDocumentDetails.Add(emsBilldoc);
                isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;
            }
            #endregion

            return isSuccess;
        }
        public bool UpdateAddEMSBillDocument(EMSBillDocumentDetailDTO expenseBillDocDetail, long userID, DateTime date)
        {
            bool isSuccess = false;

            var itemBillDocs = SmartDostDbContext.EMSBillDocumentDetails.Where(x => x.EMSBillDocumentDetailID == expenseBillDocDetail.EMSBillDocumentDetailID && x.IsActive == true && x.IsDeleted == false).ToList();

            foreach (var item in itemBillDocs)
            {
                #region Update Bill Document

                var itemBillDoc = SmartDostDbContext.EMSBillDocumentDetails.FirstOrDefault(k => k.EMSBillDocumentDetailID == item.EMSBillDocumentDetailID);
                if (itemBillDoc != null)
                {
                    itemBillDoc.IsActive = expenseBillDocDetail.IsActive;
                    itemBillDoc.IsDeleted = expenseBillDocDetail.IsDeleted;
                    itemBillDoc.ModifiedBy = Convert.ToInt32(userID);
                    itemBillDoc.DocumentName = expenseBillDocDetail.DocumentName;
                    itemBillDoc.ModifiedDate = date;
                    SmartDostDbContext.Entry<EMSBillDocumentDetail>(itemBillDoc).State = System.Data.EntityState.Modified;
                    isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;

                    //Delete uploaded files from application
                }
                #endregion

                #region Add Bill Documents

                else
                {
                    EMSBillDocumentDetail emsBilldoc = new EMSBillDocumentDetail();
                    emsBilldoc.DocumentName = expenseBillDocDetail.DocumentName;
                    emsBilldoc.EMSBillDetailID = expenseBillDocDetail.EMSBillDetailID;
                    emsBilldoc.IsActive = true;
                    emsBilldoc.IsDeleted = false;
                    emsBilldoc.CreatedBy = expenseBillDocDetail.CreatedBy;
                    emsBilldoc.CreatedDate = date;
                    SmartDostDbContext.EMSBillDocumentDetails.Add(emsBilldoc);
                    isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;

                    //Added new documents files 
                }

                #endregion
            }

            #region Add Bill Documents
            if (itemBillDocs.Count == 0 || itemBillDocs == null)
            {
                EMSBillDocumentDetail emsBilldoc = new EMSBillDocumentDetail();
                emsBilldoc.DocumentName = expenseBillDocDetail.DocumentName;
                emsBilldoc.EMSBillDetailID = expenseBillDocDetail.EMSBillDetailID;
                emsBilldoc.IsActive = true;
                emsBilldoc.IsDeleted = false;
                emsBilldoc.CreatedBy = expenseBillDocDetail.CreatedBy;
                emsBilldoc.CreatedDate = date;
                SmartDostDbContext.EMSBillDocumentDetails.Add(emsBilldoc);
                isSuccess = SmartDostDbContext.SaveChanges() > 0 ? true : false;
            }
            #endregion

            return isSuccess;
        }
        //public List<EMSExpenseDetail> GetExpenseList()
        //{
        //    List<EMSExpenseDetail> objEMSExpenseDetail = new List<EMSExpenseDetail>();

        //    //objEMSExpenseDetail =  (
        //    //                        from ed in SmartDostDbContext.EMSExpenseDetails
        //    //                        join ebd in SmartDostDbContext.EMSBillDetails on ed.EMSExpenseDetailID equals ebd.EMSExpenseDetailID
        //    //                        join ebd2 in SmartDostDbContext.EMSBillDocumentDetails on ebd.EMSBillDetailID equals ebd2.EMSBillDetailID
        //    //                        join etm in SmartDostDbContext.EMSExpenseTypeMasters on ed.EMSExpenseTypeMasterID equals etm.EMSExpenseTypeMasterId
        //    //                        join cs in SmartDostDbContext.CommonSetups on ed.Status equals cs.DisplayValue 
        //    //                        where ed.IsActive == true && ed.IsDeleted == false 
        //    //                        && ebd.IsActive ==true && ebd.IsDeleted == false
        //    //                        && ebd2.IsActive== true && ebd2.IsDeleted == false
        //    //                        && cs.MainType == "Expense Status"
        //    //                        select ed  //ed.EMSExpenseDetailID, ed.EMSExpenseTypeMasterID

        //    //).ToList();

        //    return SmartDostDbContext.EMSExpenseDetails.Where(x => x.EMSExpenseDetailID == 1 && x.Status==5  && x.EMSBillDetails.Any(y=>!y.IsDeleted && y.IsActive && y.EMSBillDocumentDetails.Any(z=>!z.IsDeleted && z.IsActive))).ToList();
        //}
        #endregion

        public List<EMSExpenseDetail> GetExpenseList(long userID, int? emsExpenseDetailID)
        {
            List<EMSExpenseDetail> expenses = new List<EMSExpenseDetail>();
            if (emsExpenseDetailID == null)
            {
                expenses = SmartDostDbContext.EMSExpenseDetails.Where(e => e.CreatedBy == userID && !e.IsDeleted && e.IsActive).Include("EMSBillDetails").Where(x => x.EMSBillDetails.Any(z => z.IsDeleted == false)).ToList();
                //List<EMSBillDetail> ems = SmartDostDbContext.EMSBillDetails.Where(x => !x.IsDeleted && x.IsActive && x.CreatedBy == userID).ToList();
                //var exp = SmartDostDbContext.EMSExpenseDetails.Where(e => e.CreatedBy == userID && !e.IsDeleted && e.IsActive).ToList();

                //expenses = exp.Where(k => ems.Any(s => s.EMSExpenseDetailID == k.EMSExpenseDetailID)).ToList();
                foreach (var item in expenses)
                {
                    item.EMSBillDetails = item.EMSBillDetails.Where(x => !x.IsDeleted && x.IsActive).ToList();
                    foreach (var doc in item.EMSBillDetails)
                    {
                        doc.EMSBillDocumentDetails = doc.EMSBillDocumentDetails.Where(x => !x.IsDeleted && x.IsActive).ToList();
                    }
                }

                expenses.AddRange(SmartDostDbContext.ApprovalStatusHistories.Where(x => x.AssignedToUser == userID && !x.IsDeleted && x.IsActive).Select(x => x.EMSExpenseDetail).ToList());
            }
            else
            {
                expenses = SmartDostDbContext.EMSExpenseDetails.Where(e => e.CreatedBy == userID && !e.IsDeleted && e.IsActive && e.EMSExpenseDetailID == emsExpenseDetailID).ToList();
                foreach (var item in expenses)
                {
                    item.EMSBillDetails = item.EMSBillDetails.Where(x => !x.IsDeleted && x.IsActive).ToList();
                    foreach (var doc in item.EMSBillDetails)
                    {
                        doc.EMSBillDocumentDetails = doc.EMSBillDocumentDetails.Where(x => !x.IsDeleted && x.IsActive).ToList();
                    }
                }
                expenses.AddRange(SmartDostDbContext.ApprovalStatusHistories.Where(x => x.AssignedToUser == userID && x.EMSExpenseDetailID == emsExpenseDetailID).Select(x => x.EMSExpenseDetail).ToList());

            }

            return expenses;

        }

        public string GetPendingWithName(int EMSExpenseDetailID, byte expenseMainStatus)
        {
            //var pendingwith = SmartDostDbContext.ApprovalStatusHistories.Where(p => p.EMSExpenseDetailID == EMSExpenseDetailID && 
            //    (p.Status == (int)AspectEnums.ApprovalStatus.Pending 
            //    || p.Status == (int)AspectEnums.ApprovalStatus.Rejected)).OrderByDescending(p=> p.ApprovalStatusHistoryID).FirstOrDefault();

            string Status = "";

            var pendingwith = SmartDostDbContext.ApprovalStatusHistories.Where(p => p.EMSExpenseDetailID == EMSExpenseDetailID && p.Status != (byte)AspectEnums.ApprovalStatus.Notification).OrderByDescending(p => p.ApprovalStatusHistoryID).FirstOrDefault();

            if (pendingwith != null)
            {
                UserMaster name = SmartDostDbContext.UserMasters.Where(u => u.UserID == pendingwith.AssignedToUser).FirstOrDefault();
                switch (pendingwith.Status)
                {
                    case 1:
                        if (expenseMainStatus == (byte)(AspectEnums.ApprovalStatus.Cancelled))
                        {
                            Status = "Cancelled";
                        }
                        else
                        {
                            Status = "Pending With({0})";
                            Status = string.Format(Status, name.FirstName);
                        }

                        break;
                    case 2:
                        Status = "Approved By({0})";
                        Status = string.Format(Status, name.FirstName);
                        break;
                    case 3:
                        Status = "Rejected By({0})";
                        Status = string.Format(Status, name.FirstName);
                        break;
                }
            }
            return Status;
        }

        public bool IsExpenseEditable(int emsExpenseDetailID)
        {
            bool isEditable = true;

            var pendingwith = SmartDostDbContext.ApprovalStatusHistories.Where(p => p.EMSExpenseDetailID == emsExpenseDetailID && p.Status == (byte)AspectEnums.ApprovalStatus.Approved).OrderByDescending(p => p.ApprovalStatusHistoryID).FirstOrDefault();

            if (pendingwith != null) isEditable = false;
            return isEditable;
        }


        public string GetCreatedByUsername(int emsExpenseDetailID)
        {
            //UserMaster name = 
            //    SmartDostDbContext.UserMasters.Where(u => u.UserID == userID && (u.IsActive == true && !u.IsDeleted)).FirstOrDefault();
            string createdByUserName = (from expense in SmartDostDbContext.EMSExpenseDetails
                                        join user in SmartDostDbContext.UserMasters on expense.CreatedBy equals user.UserID
                                        where expense.EMSExpenseDetailID == emsExpenseDetailID
                                        select user.FirstName + "(" + user.EmplCode + ")"
                            ).SingleOrDefault();

            return createdByUserName;
        }

        public bool UpdateBillIamge(List<EMSBillDocumentDetail> objEMSBillDocumentDetailDTO)
        {
            bool isSuccess = false;
            foreach (EMSBillDocumentDetail objBill in objEMSBillDocumentDetailDTO)
            {
                EMSBillDocumentDetail objEMSBillDocumentDetail = SmartDostDbContext.EMSBillDocumentDetails.FirstOrDefault(x => x.EMSBillDocumentDetailID == objBill.EMSBillDocumentDetailID && !x.IsDeleted && x.IsActive);
                if (objEMSBillDocumentDetail != null)
                {
                    objEMSBillDocumentDetail.DocumentName = objBill.DocumentName;
                    SmartDostDbContext.Entry<EMSBillDocumentDetail>(objEMSBillDocumentDetail).State = System.Data.EntityState.Modified;
                    SmartDostDbContext.SaveChanges();
                    isSuccess = true;
                }
            }
            return isSuccess;
        }

        public List<CommonSetup> GetApproverType(int companyID)
        {
            return SmartDostDbContext.CommonSetups.Where(x => x.CompanyID == companyID && !x.IsDeleted && x.MainType == "Expense" && x.SubType == "Approval Path"
                && x.DisplayText.ToLower() != "Approval Not required".ToLower()
                ).ToList();
        }


        #region "Approval Master data based on user Company"
        /// <summary>
        /// GetExpenseTypeMaster 
        /// </summary>
        /// <returns></returns>
        public List<EMSExpenseTypeMaster> GetExpenseTypeMaster(int companyID)
        {
            return SmartDostDbContext.EMSExpenseTypeMasters.Where(x => x.CompanyId == companyID && x.IsActive && !x.IsDeleted).ToList();

        }

        /// <summary>
        /// Method to get Approval data into Console
        /// </summary>
        /// <param name="userID">user ID</param>
        /// <param name="dataFrom">date from</param>
        /// <param name="dateTo">date To</param>
        /// <returns>returns Approval Data for based on user Company</returns>
        public List<GetExpenseApprovalPath> GetExpenseApprovalPathData(long? userID, int companyID, int? roleID, int approvalPathTypeID, int? emsExpenseTypeMasterId)
        {
            ObjectResult<spGetExpenseApprovalPath_Result> approvalList = SmartDostDbContext.spGetExpenseApprovalPath(userID, companyID, roleID, approvalPathTypeID, emsExpenseTypeMasterId);
            List<GetExpenseApprovalPath> result = new List<GetExpenseApprovalPath>();
            foreach (var item in approvalList)
            {
                result.Add(new GetExpenseApprovalPath()
                {
                    ApproverPathMasterID = item.ApproverPathMasterID
                    ,
                    ModuleName = item.ModuleName
                    ,
                    ExpenseType = item.ExpenseType
                    ,
                    Role = item.Role
                    ,
                    ApproverRole = item.ApproverRole

                    ,
                    ApproverUser = item.ApproverUser

                    ,
                    ApproverType = item.ApproverType
                    ,ApproverTypeValue = item.ApproverTypeValue
                    ,
                    Sequence = item.Sequence
                    ,
                    CreatedBy = item.CreatedBy
                    ,
                    CreatedDate = item.CreatedDate
                    ,
                    ModifiedBy = item.ModifiedBy
                    ,
                    ModifiedDate = item.ModifiedDate
                });
            }
            return result;
        }

        #endregion

        /// <summary>
        /// Create Approval EMS or LMS
        /// </summary>
        /// <param name="objapproverPathMaster"></param>
        /// <returns></returns>
        public int CreateApprovalPath(ApproverPathMaster objapproverPathMaster)
        {
            int status = 0;
            int? sequence = 0;
            objapproverPathMaster.CreatedDate = DateTime.Now;
            try
            {
                if (objapproverPathMaster.ApprovalPathTypeID == 1)
                {
                    sequence = SmartDostDbContext.ApproverPathMasters.Where(x => x.CompanyID == objapproverPathMaster.CompanyID &&
                        x.RoleID == objapproverPathMaster.RoleID && x.ApprovalPathTypeID == objapproverPathMaster.ApprovalPathTypeID && x.IsActive && !x.IsDeleted
                        && x.EMSExpenseTypeMasterId == objapproverPathMaster.EMSExpenseTypeMasterId
                        ).OrderByDescending(x => x.Sequence).FirstOrDefault().Sequence;
                }
                else if (objapproverPathMaster.ApprovalPathTypeID == 2)
                {
                    sequence = SmartDostDbContext.ApproverPathMasters.Where(x => x.CompanyID == objapproverPathMaster.CompanyID &&
                        x.RoleID == objapproverPathMaster.RoleID && x.ApprovalPathTypeID == objapproverPathMaster.ApprovalPathTypeID && x.IsActive && !x.IsDeleted
                        ).OrderByDescending(x => x.Sequence).FirstOrDefault().Sequence;
                }
                else if (objapproverPathMaster.ApprovalPathTypeID == 3)
                {
                    sequence = SmartDostDbContext.ApproverPathMasters.Where(x => x.CompanyID == objapproverPathMaster.CompanyID &&
                        x.RoleID == objapproverPathMaster.RoleID && x.ApprovalPathTypeID == objapproverPathMaster.ApprovalPathTypeID && x.IsActive && !x.IsDeleted
                        ).OrderByDescending(x => x.Sequence).FirstOrDefault().Sequence;
                }
                if (sequence == null) sequence = 0;
            }
            catch (Exception exe)
            {

            }
            var expenseData = new List<ApproverPathMaster>();
            if (objapproverPathMaster.ApprovalPathTypeID == Convert.ToInt32(AspectEnums.CheckModule.ExpenseManagementSystem))
            {
                expenseData = SmartDostDbContext.ApproverPathMasters.Where(x => x.CompanyID == objapproverPathMaster.CompanyID &&
                        x.RoleID == objapproverPathMaster.RoleID && x.ApprovalPathTypeID == 1 && x.IsActive && !x.IsDeleted
                        && x.EMSExpenseTypeMasterId == objapproverPathMaster.EMSExpenseTypeMasterId && x.ApproverRoleID == objapproverPathMaster.ApproverRoleID
                        && x.TeamID == objapproverPathMaster.TeamID && x.ApproverRoleID == objapproverPathMaster.ApproverRoleID).ToList();
            }
            else if (objapproverPathMaster.ApprovalPathTypeID == Convert.ToInt32(AspectEnums.CheckModule.LeaveManagementSystem))
            {
                expenseData = SmartDostDbContext.ApproverPathMasters.Where(x => x.CompanyID == objapproverPathMaster.CompanyID &&
                        x.RoleID == objapproverPathMaster.RoleID && x.ApprovalPathTypeID == 2 && x.IsActive && !x.IsDeleted
                         && x.ApproverRoleID == objapproverPathMaster.ApproverRoleID
                        && x.TeamID == objapproverPathMaster.TeamID && x.ApproverRoleID == objapproverPathMaster.ApproverRoleID).ToList();
            }
            else if (objapproverPathMaster.ApprovalPathTypeID == Convert.ToInt32(AspectEnums.CheckModule.SalesReturnSystem))
            {
                expenseData = SmartDostDbContext.ApproverPathMasters.Where(x => x.CompanyID == objapproverPathMaster.CompanyID &&
                        x.RoleID == objapproverPathMaster.RoleID && x.ApprovalPathTypeID == 3 && x.IsActive && !x.IsDeleted
                         && x.ApproverRoleID == objapproverPathMaster.ApproverRoleID
                        && x.TeamID == objapproverPathMaster.TeamID && x.ApproverRoleID == objapproverPathMaster.ApproverRoleID).ToList();
            }
            if (expenseData.Count > 0)
            {
                status = -1;
            }
            else
            {

                objapproverPathMaster.Sequence = Convert.ToInt32(++sequence);

                SmartDostDbContext.Entry<ApproverPathMaster>(objapproverPathMaster).State = System.Data.EntityState.Added;

                SmartDostDbContext.SaveChanges();

                if (objapproverPathMaster.ApproverPathMasterID > 0) status = objapproverPathMaster.ApproverPathMasterID;
            }
            return status;
        }

        /// <summary>
        /// Delete approver
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="approverPathMasterID"></param>
        /// <returns></returns>
        public bool DeleteApprover(long UserID, int approverPathMasterID)
        {
            bool isSuccess = false;

            ApproverPathMaster objApproverPathMaster = SmartDostDbContext.ApproverPathMasters.FirstOrDefault(k => k.ApproverPathMasterID == approverPathMasterID && k.IsActive && !k.IsDeleted);
            objApproverPathMaster.ModifiedDate = System.DateTime.Now;
            objApproverPathMaster.ModifiedBy = UserID;
            objApproverPathMaster.IsDeleted = true;
            objApproverPathMaster.IsActive = false;
            int result = SmartDostDbContext.SaveChanges();
            if (result > 0) isSuccess = true;
            return isSuccess;
        }

        /// <summary>
        /// GetUserByTeamRole
        /// </summary>
        /// <param name="TeamID"></param>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public List<spGetUserByTeamRole> GetUserByTeamRole(int? TeamID, int? RoleID)
        {
            ObjectResult<spGetUserByTeamRole_Result> approvalList = SmartDostDbContext.spGetUserByTeamRole(TeamID, RoleID);
            List<spGetUserByTeamRole> result = new List<spGetUserByTeamRole>();
            foreach (var item in approvalList)
            {
                result.Add(new spGetUserByTeamRole()
                {
                    UserID = item.UserID
                        ,
                    EmplCodeWithName = item.EmplCodeWithName
                        ,
                    TeamCodeWithName = item.TeamCodeWithName
                        ,
                    RoleCodeWithName = item.TeamCodeWithName
                        ,
                    TeamID = item.TeamID
                        ,
                    RoleID = item.RoleID
                });
            }
            return result;
        }
        #endregion


        #region Upload Master data from excel file created by Neeraj Singh

        public List<int> UploadMasterDataParking2Main(System.Data.DataTable dtDemoValidation, int enumMaster)
        {
            string consString = SmartDostDbContext.Database.Connection.ConnectionString;
            List<int> result = new List<int>();
            ObjectParameter objParamRowsInserted = new ObjectParameter("RowsInserted", typeof(int));
            ObjectParameter objParamRowUpdated = new ObjectParameter("RowUpdated", typeof(int));

            string masterName = Enum.GetName(typeof(AspectEnums.enumExcelType), enumMaster);
            string parkingTable = "Parking_" + masterName;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = System.Transactions.IsolationLevel.ReadUncommitted }))
            {
                // Delete existing data from the corresponding parking table
                SmartDostDbContext.Database.ExecuteSqlCommand("DELETE FROM " + parkingTable);
                SmartDostDbContext.SaveChanges();

                using (SqlConnection con = new SqlConnection(consString))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        try
                        {

                            if (parkingTable == "Parking_TeamSpocMaster")
                            {
                                #region TeamSpocMaster Mapping on bulk upload
                                SqlBulkCopyColumnMapping teamspocid = new SqlBulkCopyColumnMapping("TeamCode", "TeamCode");
                                sqlBulkCopy.ColumnMappings.Add(teamspocid);
                                SqlBulkCopyColumnMapping spoc = new SqlBulkCopyColumnMapping("SpocCode", "SpocCode");
                                sqlBulkCopy.ColumnMappings.Add(spoc);

                                SqlBulkCopyColumnMapping createdby = new SqlBulkCopyColumnMapping("CreatedBy", "CreatedBy");
                                sqlBulkCopy.ColumnMappings.Add(createdby);
                                SqlBulkCopyColumnMapping modifby = new SqlBulkCopyColumnMapping("ModifiedBy", "ModifiedBy");
                                sqlBulkCopy.ColumnMappings.Add(modifby);

                                SqlBulkCopyColumnMapping created = new SqlBulkCopyColumnMapping("CreatedOn", "CreatedOn");
                                sqlBulkCopy.ColumnMappings.Add(created);
                                SqlBulkCopyColumnMapping modif = new SqlBulkCopyColumnMapping("ModifiedOn", "ModifiedOn");
                                sqlBulkCopy.ColumnMappings.Add(modif);
                                SqlBulkCopyColumnMapping hod = new SqlBulkCopyColumnMapping("HODCode", "HODCode");
                                sqlBulkCopy.ColumnMappings.Add(hod);
                                SqlBulkCopyColumnMapping em2 = new SqlBulkCopyColumnMapping("EM2Code", "EM2Code");
                                sqlBulkCopy.ColumnMappings.Add(em2);
                                SqlBulkCopyColumnMapping em1 = new SqlBulkCopyColumnMapping("EM1Code", "EM1Code");
                                sqlBulkCopy.ColumnMappings.Add(em1);
                                SqlBulkCopyColumnMapping LocationLevel = new SqlBulkCopyColumnMapping("LocationLevel", "LocationLevel");
                                sqlBulkCopy.ColumnMappings.Add(LocationLevel);
                                SqlBulkCopyColumnMapping LocationValue = new SqlBulkCopyColumnMapping("LocationValue", "LocationValue");
                                sqlBulkCopy.ColumnMappings.Add(LocationValue);
                                SqlBulkCopyColumnMapping isdel = new SqlBulkCopyColumnMapping("IsDeleted", "IsDeleted");
                                sqlBulkCopy.ColumnMappings.Add(isdel);

                                #endregion
                            }
                            if (parkingTable == "Parking_SOSFMaster")
                            {
                                #region SOSFMaster Mapping on bulk upload
                                SqlBulkCopyColumnMapping year = new SqlBulkCopyColumnMapping("SaleYear", "SaleYear");
                                sqlBulkCopy.ColumnMappings.Add(year);
                                SqlBulkCopyColumnMapping sm = new SqlBulkCopyColumnMapping("SaleMonth", "SaleMonth");
                                sqlBulkCopy.ColumnMappings.Add(sm);
                                SqlBulkCopyColumnMapping sw = new SqlBulkCopyColumnMapping("SaleWeek", "SaleWeek");
                                sqlBulkCopy.ColumnMappings.Add(sw);
                                SqlBulkCopyColumnMapping sd = new SqlBulkCopyColumnMapping("SaleDate", "SaleDate");
                                sqlBulkCopy.ColumnMappings.Add(sd);
                                SqlBulkCopyColumnMapping sa = new SqlBulkCopyColumnMapping("SaleAmount", "SaleAmount");
                                sqlBulkCopy.ColumnMappings.Add(sa);

                                SqlBulkCopyColumnMapping teamspocid = new SqlBulkCopyColumnMapping("Percentage", "Percentage");
                                sqlBulkCopy.ColumnMappings.Add(teamspocid);
                                SqlBulkCopyColumnMapping spoc = new SqlBulkCopyColumnMapping("Forecast", "Forecast");
                                sqlBulkCopy.ColumnMappings.Add(spoc);


                                SqlBulkCopyColumnMapping created = new SqlBulkCopyColumnMapping("CreatedDate", "CreatedDate");
                                sqlBulkCopy.ColumnMappings.Add(created);
                                SqlBulkCopyColumnMapping createdby = new SqlBulkCopyColumnMapping("CreatedBy", "CreatedBy");
                                sqlBulkCopy.ColumnMappings.Add(createdby);
                                SqlBulkCopyColumnMapping modate = new SqlBulkCopyColumnMapping("ModifiedDate", "ModifiedDate");
                                sqlBulkCopy.ColumnMappings.Add(modate);
                                SqlBulkCopyColumnMapping modby = new SqlBulkCopyColumnMapping("ModifiedBy", "ModifiedBy");
                                sqlBulkCopy.ColumnMappings.Add(modby);
                                SqlBulkCopyColumnMapping isdel = new SqlBulkCopyColumnMapping("IsDeleted", "IsDeleted");
                                sqlBulkCopy.ColumnMappings.Add(isdel);
                                SqlBulkCopyColumnMapping company = new SqlBulkCopyColumnMapping("CompanyID", "CompanyID");
                                sqlBulkCopy.ColumnMappings.Add(company);
                                SqlBulkCopyColumnMapping sku = new SqlBulkCopyColumnMapping("SKUCode", "SKUCode");
                                sqlBulkCopy.ColumnMappings.Add(sku);
                                SqlBulkCopyColumnMapping stcode = new SqlBulkCopyColumnMapping("StoreCode", "StoreCode");
                                sqlBulkCopy.ColumnMappings.Add(stcode);

                                #endregion
                            }
                            sqlBulkCopy.BulkCopyTimeout = 180;
                            sqlBulkCopy.DestinationTableName = parkingTable;
                            con.Open();
                            sqlBulkCopy.WriteToServer(dtDemoValidation);
                            con.Close();
                        }
                        catch (Exception ex)
                        {

                        }

                    }

                }

                #region Call Entity SP based on master

                switch (enumMaster)
                {
                    case 1:
                        SmartDostDbContext.spUploadCompanyMaster(objParamRowsInserted, objParamRowUpdated);
                        break;
                    case 2:
                        SmartDostDbContext.spUploadGeoDefinitions(objParamRowsInserted, objParamRowUpdated);
                        break;
                    case 3:
                        SmartDostDbContext.spUploadGeoMaster(objParamRowsInserted, objParamRowUpdated);
                        break;
                    case 4:
                        SmartDostDbContext.spUploadProductMaster(objParamRowsInserted, objParamRowUpdated);
                        break;
                    case 5:
                        SmartDostDbContext.spUploadRoleMaster(objParamRowsInserted, objParamRowUpdated);
                        break;
                    case 6:
                        SmartDostDbContext.spUploadStoreMaster(objParamRowsInserted, objParamRowUpdated);
                        break;
                    case 7:
                        SmartDostDbContext.spUploadTeamMaster(objParamRowsInserted, objParamRowUpdated);
                        break;
                    case 8:
                        SmartDostDbContext.spUploadTeamSpocMaster(objParamRowsInserted, objParamRowUpdated);
                        break;
                    case 9:
                        SmartDostDbContext.spUploadUserMaster(objParamRowsInserted, objParamRowUpdated);
                        break;
                    case 10:
                        SmartDostDbContext.spUploadUserRoles(objParamRowsInserted, objParamRowUpdated);
                        break;

                    case 11:
                        SmartDostDbContext.spUploadSalesForecast(objParamRowsInserted, objParamRowUpdated);
                        break;
                }

                #endregion

                scope.Complete();

                result.Add(Convert.ToInt32(objParamRowsInserted.Value));
                result.Add(Convert.ToInt32(objParamRowUpdated.Value));
            }

            return result;
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
        public bool SumbitLeaveRequest(long userID, int roleID, string ApproverRemarks, List<LMSLeaveMaster> LMSLeaveMasterData)
        {
            bool status = false;
            DateTime dtModifiedDate = DateTime.Now;
            foreach (var item in LMSLeaveMasterData)
            {

                byte nextApproverType = 1;//1:Pending,6:Notification                
                int nextApproverID = 0;
                //no need to find next approver if staus is rejected
                if (item.CurrentStatus != (byte)AspectEnums.ApprovalStatus.Rejected)
                {
                    //int? LeaveMasterID = item.LeaveMasterID;
                    nextApproverID = GetNextApprover(Convert.ToInt32(userID), roleID, item.LeaveMasterID, out nextApproverType, out nextApproverID);

                }
                if (item.LeaveMasterID == 0)
                {
                    if (nextApproverID == 0)
                    {
                        nextApproverID = Convert.ToInt32(userID);
                        nextApproverType = 1; // if approveral path not exist then leave will be pending to self.
                    }
                    item.PendingWith = nextApproverID;
                    item.CreatedBy = userID;
                    item.ModifiedDate = dtModifiedDate;
                    item.ModifiedBy = null;

                    SmartDostDbContext.Entry<LMSLeaveMaster>(item).State = System.Data.EntityState.Added;
                    LMSStatusLogCreation(Convert.ToInt32(userID), roleID, item, item.CurrentStatus, nextApproverType, nextApproverID, "");
                }
                else
                {

                    var leaveData = SmartDostDbContext.LMSLeaveMasters.FirstOrDefault(x => x.LeaveMasterID == item.LeaveMasterID);
                    if (leaveData != null)
                    {
                        leaveData.ModifiedDate = dtModifiedDate;
                        foreach (var date in item.LMSLeaveDetails)
                        {
                            var dateDB = leaveData.LMSLeaveDetails.FirstOrDefault(x => x.LMSLeaveDetailID == date.LMSLeaveDetailID);
                            dateDB.CurrentStatus = date.CurrentStatus;
                            date.ModifiedDate = dtModifiedDate;
                        }
                        //leaveData.LMSLeaveDetails = item.LMSLeaveDetails;
                        LMSStatusLogCreation(Convert.ToInt32(userID), roleID, leaveData, item.CurrentStatus, nextApproverType, nextApproverID, ApproverRemarks);
                        SmartDostDbContext.Entry<LMSLeaveMaster>(leaveData).State = System.Data.EntityState.Modified;
                    }
                }
            }
            SmartDostDbContext.SaveChanges();
            status = true;
            return status;


        }

        private void LMSStatusLogCreation(int? userID, int? roleID, LMSLeaveMaster item, byte approvalAction, byte nextApproverType, int nextApproverID, string ApproverRemarks)
        {
            //Notifications entry
            while (nextApproverType == (byte)AspectEnums.ApprovalStatus.Notification)
            {
                //int? LeaveMasterID = item.LeaveMasterID;
                InsertLMSActionLog(item, nextApproverID, nextApproverType, approvalAction, ApproverRemarks);

                nextApproverID = GetNextApprover(userID, roleID, item.LeaveMasterID, out nextApproverType, out nextApproverID);
                //if(nextApproverType == (byte)AspectEnums.ApprovalStatus.Consent) 
                //    nextApproverType = (byte)AspectEnums.ApprovalStatus.Pending;
            }
            //Pending cases entry
            InsertLMSActionLog(item, nextApproverID, nextApproverType, approvalAction, ApproverRemarks);
        }

        private int GetNextApprover(int? userID, int? roleID, int? LMSMasterID, out byte nextApproverType, out int nextApproverID)
        {
            //TODO to find next approver and approval type            

            ObjectParameter AssignedToUser = new ObjectParameter("AssignedToUser", typeof(int));
            ObjectParameter ApproverTypeID = new ObjectParameter("ApproverTypeID", typeof(int));

            SmartDostDbContext.spGetLMSNextApprover(userID, roleID, LMSMasterID, AssignedToUser, ApproverTypeID);

            nextApproverID = int.Parse(AssignedToUser.Value.ToString());
            nextApproverType = Convert.ToByte(ApproverTypeID.Value);
            return nextApproverID;

        }

        private int GetSalesRequestNextApprover(int? userID, int? roleID, int? SRRequestID, out byte nextApproverType, out int nextApproverID)
        {
            //TODO to find next approver and approval type            

            ObjectParameter AssignedToUser = new ObjectParameter("AssignedToUser", typeof(int));
            ObjectParameter ApproverTypeID = new ObjectParameter("ApproverTypeID", typeof(int));

            SmartDostDbContext.spGetSalesRequestNextApprover(userID, roleID, SRRequestID, AssignedToUser, ApproverTypeID);

            nextApproverID = int.Parse(AssignedToUser.Value.ToString());
            nextApproverType = Convert.ToByte(ApproverTypeID.Value);
            return nextApproverID;

        }


        private void InsertLMSActionLog(LMSLeaveMaster item, long nextApproverID, byte nextApproverActionStatus, byte approvalAction, string ApproverRemarks)
        {
            //Below code will be used only in case of approval/reject action only.
            if (nextApproverActionStatus != (byte)AspectEnums.ApprovalStatus.Notification && approvalAction != (byte)AspectEnums.ApprovalStatus.Pending)
            {
                byte pendingStatus = (byte)AspectEnums.ApprovalStatus.Pending;
                var lastApprover = item.LMSStatusLogs.Where(x => x.CurrentStatus == pendingStatus).OrderByDescending(x => x.LMSStatusLogID).FirstOrDefault();
                if (lastApprover != null)
                {
                    lastApprover.CurrentStatus = approvalAction;
                    lastApprover.Remarks = ApproverRemarks;
                }
            }
            //Logic to send approval to next person
            if (nextApproverID == 0)//Apporval not required or person is having no reporting person
            {
                item.CurrentStatus = (byte)approvalAction;
            }
            else
            {

                item.LMSStatusLogs.Add(new LMSStatusLog { CreatedBy = nextApproverID, CreatedDate = DateTime.Now, Remarks = "", CurrentStatus = nextApproverActionStatus });

            }
            if (nextApproverID != 0)
                item.PendingWith = nextApproverID;
            SmartDostDbContext.SaveChanges();
            // item.LeaveMasterID = item.LeaveMasterID;


        }
        /// <summary>
        /// Get LeaveType Master based on UserID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<LMSLeaveTypeMaster> GetLeaveTypeMaster(long userID, int roleID)
        {

            var leaves = from leave in SmartDostDbContext.LMSLeaveTypeMasters
                         join user in SmartDostDbContext.UserMasters on leave.CompanyID equals user.CompanyID
                         //join config in SmartDostDbContext.LMSLeaveTypeConfigurations on leave.LMSLeaveTypeMasterID equals config.LMSLeaveTypeMasterID
                         //join leavesTaken in SmartDostDbContext.LMSLeaveMasters.Where(x => x.CreatedBy == userID && x.CreatedDate >= financialYearStart).GroupBy(x => x.LMSLeaveTypeMasterID).Select(y => new { LMSLeaveTypeMasterID = y.Key.Value, LeavesTaken = y.Sum(z => z.NumberOfLeave) }) on leave.LMSLeaveTypeMasterID equals leavesTaken.LMSLeaveTypeMasterID                         
                         //where user.UserID == userID && config.RoleID==roleID &&  config.CommonSetup.MainType=="LMSLeaveTypeConfiguration" && config.CommonSetup.SubType=="ConfigSetupID" && config.CommonSetup.DisplayValue==1
                         where user.UserID == userID && leave.IsDeleted == false //
                         select leave;
            return leaves.ToList();
        }
        /// <summary>
        /// Get LeaveType Master based on UserID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<LeaveBalanceEntity> GetLeavesTaken(long userID, int roleID, DateTime financialYearStart)
        {
            byte rejectedStatus = (byte)AspectEnums.ApprovalStatus.Rejected;

            List<LeaveBalanceEntity> objLeaveBalanceEntity = (List<LeaveBalanceEntity>)SmartDostDbContext.LMSLeaveMasters.Where(x => x.CreatedBy == userID && x.CreatedDate >= financialYearStart && x.CurrentStatus != rejectedStatus).GroupBy(x => x.LMSLeaveTypeMasterID).
                Select(y => new LeaveBalanceEntity { LMSLeaveTypeMasterID = y.Key.Value, LeavesTaken = (y.Sum(z => z.LMSLeaveDetails.Where(x => x.CurrentStatus != rejectedStatus && x.IsHalfDay == false).Count()) + y.Sum(z => z.LMSLeaveDetails.Where(x => x.CurrentStatus != rejectedStatus && x.IsHalfDay == true).Count()) / 2.0) }).ToList();

            return objLeaveBalanceEntity;
        }
        /// <summary>
        /// Get LeaveType Master based on RoleID
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<LMSLeaveTypeConfiguration> GetLeaveTypeCofiguration(long userID, int roleID)
        {

            var configuration = from config in SmartDostDbContext.LMSLeaveTypeConfigurations
                                join leaveType in SmartDostDbContext.LMSLeaveTypeMasters on config.LMSLeaveTypeMasterID equals leaveType.LMSLeaveTypeMasterID
                                where leaveType.IsDeleted == false && config.RoleID == roleID && config.IsDeleted == false
                                select config;
            return configuration.ToList();
        }

        /// <summary>
        /// Get Leave data based on request type or leaveTypeID
        /// </summary>
        /// <param name="userID">UserID for which leave needs to be fetched</param>
        /// <param name="LeaveTypeID"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        public List<LMSLeaveMaster> GetLeaves(long userID, LMSLeaveRequestDTO LeaveRequest)
        {
            List<LMSLeaveMaster> leaves = new List<LMSLeaveMaster>();
            //DateTime dtStart = Convert.ToDateTime(LeaveRequest.StartDate);
            //DateTime dtEnd = Convert.ToDateTime(LeaveRequest.EndDate);
            DateTime LastSyncDateTime;
            DateTime.TryParse(LeaveRequest.LastSyncDateTime, out LastSyncDateTime);

            leaves.AddRange(SmartDostDbContext.LMSLeaveMasters.Where(x => x.CreatedBy == userID && (LeaveRequest.LastSyncDateTime == null || x.ModifiedDate >= LastSyncDateTime)).ToList());
            leaves.AddRange(SmartDostDbContext.LMSStatusLogs.Where(x => x.CreatedBy == userID && (LeaveRequest.LastSyncDateTime == null || x.CreatedDate >= LastSyncDateTime)).Select(x => x.LMSLeaveMaster));
            return leaves;
        }

        public int SubmitLeavetypeConfig(LMSLeaveTypeConfiguration lmsconfig)
        {
            int status = 0;

            SmartDostDbContext.LMSLeaveTypeConfigurations.Add(lmsconfig);
            SmartDostDbContext.SaveChanges();
            status = 1;
            return status;
        }

        #endregion


        #region Sales Return System

        /// <summary>
        /// Get Approval Path Type for Console Approval Creation
        /// </summary>
        /// <returns></returns>
        public IList<ApprovalPathType> GetApprovalPathType()
        {
            IList<ApprovalPathType> result = new List<ApprovalPathType>();

            result = SmartDostDbContext.ApprovalPathTypes.Where(x => x.IsActive && !x.IsDeleted).ToList();
            return result;
        }

        /// <summary>
        /// APK Service to Get Sales return data
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <param name="ApproverRemarks"></param>
        /// <param name="SRRequestData"></param>
        /// <returns></returns>
        public bool SumbitSalesReturnRequest(long userID, int roleID, string ApproverRemarks, List<SRRequest> SRRequestData)
        {
            bool status = false;
            DateTime dtModifiedDate = DateTime.Now;
            foreach (var item in SRRequestData)
            {

                byte nextApproverType = 1;//1:Pending,6:Notification                
                int nextApproverID = 0;
                //no need to find next approver if staus is rejected
                if (item.CurrentStatus != (byte)AspectEnums.ApprovalStatus.Rejected)
                {
                    //int? LeaveMasterID = item.LeaveMasterID;
                    nextApproverID = GetNextApprover(Convert.ToInt32(userID), roleID, item.SRRequestID, out nextApproverType, out nextApproverID);

                }
                if (item.SRRequestID == 0)
                {
                    if (nextApproverID == 0)
                    {
                        nextApproverID = Convert.ToInt32(userID);
                        nextApproverType = 1; // if approveral path not exist then leave will be pending to self.
                    }
                    //item.StoreID = item.StoreID;
                    item.PendingWith = nextApproverID;
                    item.CreatedBy = userID;
                    //item.ModifiedDate = dtModifiedDate;
                    item.CreatedDate = dtModifiedDate;
                    item.ModifiedBy = null;
                    item.IsDeleted = false;

                    SmartDostDbContext.Entry<SRRequest>(item).State = System.Data.EntityState.Added;

                    SRApprovalStatusLogCreation(Convert.ToInt32(userID), roleID, item, item.CurrentStatus, nextApproverType, nextApproverID, "");
                }
                else
                {

                    var salesReturnRequestData = SmartDostDbContext.SRRequests.FirstOrDefault(x => x.SRRequestID == item.SRRequestID);
                    if (salesReturnRequestData != null)
                    {
                        salesReturnRequestData.ModifiedDate = dtModifiedDate;
                        foreach (var date in item.SRDetails)
                        {
                            var dateDB = salesReturnRequestData.SRDetails.FirstOrDefault(x => x.SRDetailID == date.SRRequestID);
                            dateDB.CurrentStatus = date.CurrentStatus;
                            date.ModifiedDate = dtModifiedDate;
                        }
                        //leaveData.LMSLeaveDetails = item.LMSLeaveDetails;
                        SRApprovalStatusLogCreation(Convert.ToInt32(userID), roleID, salesReturnRequestData, item.CurrentStatus, nextApproverType, nextApproverID, ApproverRemarks);
                        SmartDostDbContext.Entry<SRRequest>(salesReturnRequestData).State = System.Data.EntityState.Modified;
                    }
                }
            }
            SmartDostDbContext.SaveChanges();
            status = true;
            return status;
        }

        /// <summary>
        /// APK Service to Get Sales Return
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="SRRequest"></param>
        /// <returns></returns>
        public List<SRRequest> GetSalesReturn(long userID, GetSalesReturnRequestDTO SRRequest)
        {
            List<SRRequest> salesReturnRequest = new List<SRRequest>();

            DateTime LastSyncDateTime;
            DateTime.TryParse(SRRequest.LastSyncDateTime, out LastSyncDateTime);

            salesReturnRequest.AddRange(SmartDostDbContext.SRRequests.Where(x => x.CreatedBy == userID && (SRRequest.LastSyncDateTime == null || (x.ModifiedDate ?? x.CreatedDate) >= LastSyncDateTime)).ToList());
            salesReturnRequest.AddRange(SmartDostDbContext.SRApprovalStatusLogs.Where(x => x.AssignedTo == userID && (SRRequest.LastSyncDateTime == null || (x.ModifiedDate ?? x.CreatedDate) >= LastSyncDateTime)).Select(x => x.SRRequest));
            return salesReturnRequest;
        }

        /// <summary>
        /// APK method used to Assign Sales return according to defined approval path
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="roleID"></param>
        /// <param name="item"></param>
        /// <param name="approvalAction"></param>
        /// <param name="nextApproverType"></param>
        /// <param name="nextApproverID"></param>
        /// <param name="ApproverRemarks"></param>
        private void SRApprovalStatusLogCreation(int? userID, int? roleID, SRRequest item, byte approvalAction, byte nextApproverType, int nextApproverID, string ApproverRemarks)
        {
            //Notifications entry
            while (nextApproverType == (byte)AspectEnums.ApprovalStatus.Notification)
            {

                InsertSRActionLog(item, nextApproverID, nextApproverType, approvalAction, ApproverRemarks);

                nextApproverID = GetSalesRequestNextApprover(userID, roleID, item.SRRequestID, out nextApproverType, out nextApproverID);
            }

            //Pending cases entry
            InsertSRActionLog(item, nextApproverID, nextApproverType, approvalAction, ApproverRemarks);
        }

        /// <summary>
        /// APK method used to action from Approval end
        /// </summary>
        /// <param name="item"></param>
        /// <param name="nextApproverID"></param>
        /// <param name="nextApproverActionStatus"></param>
        /// <param name="approvalAction"></param>
        /// <param name="ApproverRemarks"></param>
        private void InsertSRActionLog(SRRequest item, long nextApproverID, byte nextApproverActionStatus, byte approvalAction, string ApproverRemarks)
        {
            //Below code will be used only in case of approval/reject action only.
            if (nextApproverActionStatus != (byte)AspectEnums.ApprovalStatus.Notification && approvalAction != (byte)AspectEnums.ApprovalStatus.Pending)
            {
                byte pendingStatus = (byte)AspectEnums.ApprovalStatus.Pending;
                var lastApprover = item.SRApprovalStatusLogs.Where(x => x.CurrentStatus == pendingStatus).OrderByDescending(x => x.SRApprovalStatusLogID).FirstOrDefault();
                if (lastApprover != null)
                {
                    lastApprover.CurrentStatus = approvalAction;
                    lastApprover.Remarks = ApproverRemarks;
                }
            }
            //Logic to send approval to next person
            if (nextApproverID == 0)//Apporval not required or person is having no reporting person
            {
                item.CurrentStatus = (byte)approvalAction;
            }
            else
            {
                item.SRApprovalStatusLogs.Add(new SRApprovalStatusLog
                {
                    SRRequestID = item.SRRequestID,
                    AssignedTo = nextApproverID,
                    CreatedDate = item.CreatedDate,
                    IsDeleted = false,
                    Remarks = "",
                    CurrentStatus = nextApproverActionStatus
                });

            }
            if (nextApproverID != 0)
                item.PendingWith = nextApproverID;
            SmartDostDbContext.SaveChanges();
            // item.LeaveMasterID = item.LeaveMasterID;
        }

        #endregion


        #region Syster of Sales Forecasting (SOSF)

        public long SubmitSOSFList(long UserID, List<SOSFMaster> SOSFList)
        {
            long result = 0;
            foreach (var item in SOSFList)
            {
                if (item.SOSFId == 0)
                {
                    item.CreatedDate = DateTime.Now;
                    SmartDostDbContext.SOSFMasters.Add(item);
                    SmartDostDbContext.SaveChanges();
                    result = 1;
                }
                else
                {
                    item.ModifiedDate = DateTime.Now;
                    item.ModifiedBy = UserID;
                    SmartDostDbContext.Entry<SOSFMaster>(item).State = System.Data.EntityState.Modified;
                    SmartDostDbContext.SaveChanges();
                    result = 1;
                }
               
            }
            return result;
        }

        public List<SOSFMaster> GetUserSOSFData(long UserID, int StoreID, int ProductID,out int type)
        {
            List<SOSFMaster> result = new List<SOSFMaster>();
            result = SmartDostDbContext.SOSFMasters.Where(m => m.SaleWeek > 0 && m.IsDeleted == false && m.CreatedBy == UserID && m.StoreID == StoreID && m.ProductID == ProductID).OrderByDescending(m => m.CreatedDate).ToList();

            if (result.Count > 0)
            {
                type = 1;
            }
            else if (result.Count > 0)
            {
                result = SmartDostDbContext.SOSFMasters.Where(m => m.SaleMonth > 0 && m.IsDeleted == false && m.CreatedBy == UserID && m.StoreID == StoreID && m.ProductID == ProductID).OrderByDescending(m => m.CreatedDate).ToList();
                type = 2;
            }
            else
            {
                result = SmartDostDbContext.SOSFMasters.Where(m => m.IsDeleted == false && m.CreatedBy == UserID && m.StoreID == StoreID && m.ProductID == ProductID).OrderByDescending(m => m.CreatedDate).ToList();
                type = 3;
            }
            
            return result;
        }

        #endregion
    }

}
