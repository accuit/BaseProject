using Samsung.SmartDost.BusinessLayer.Services.BO;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.CommonLayer.Aspects.Exceptions;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using Samsung.SmartDost.CommonLayer.Resources;
using Samsung.SmartDost.PresentationLayer.ServiceImpl.Security;
using System;
using System.Collections;
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
        /// Displays the SO Details based on SO Number.
        /// </summary>
        /// <param name="storeId">SO Number of user.</param>
        /// <param name="storeId">The store identifier.</param>
        /// <returns></returns>
        [UserSecureOperation]
        public JsonResponse<SORLSDTO> GetRLSSODetails(string soNumber, long userID)
        {
            JsonResponse<SORLSDTO> response = new JsonResponse<SORLSDTO>();
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    SORLSDTO objSORLSDTO = new SORLSDTO();
                    string stringFormat = "dd-MMM-yyyy";
                    soNumber = System.Web.HttpUtility.HtmlEncode(soNumber);
                    DataSet ds = ReportBusinessInstance.GetSODetails(soNumber, userID);
                    if (ds.Tables.Count > 0)
                    {
                        DataTable dtData = ds.Tables[0];
                        var ssoList = dtData.AsEnumerable().Select(row =>
                               new SORLSDTO
                               {
                                   ClaimNo = row.Field<string>("CLAIM_NO"),
                                   BPNAME = row.Field<string>("BPNAME"),
                                   ClaimDate = row.Field<DateTime>("CLAIM_DATE").ToString(stringFormat),
                                   BranchCode = row.Field<string>("BRANCH_CODE"),
                                   BranchName = row.Field<string>("BRANCH_NAME"),
                                   AscCode = row.Field<string>("ASC_CODE"),
                                   GcicStatus = row.Field<string>("GCIC_STATUS"),
                                   GCICReasonDescription = row.Field<string>("GCIC Reason Description"),
                                   RlsStatus = row.Field<string>("RLS Status"),
                                   Product = row.Field<string>("PRODUCT"),
                                   SerialNoIMEI = row.Field<string>("SerialNo/IMEI"),
                                   RECEIVED_DT = row.Field<DateTime?>("RECEIVED_DT") == null ? "" : row.Field<DateTime?>("RECEIVED_DT").Value.ToString(stringFormat),
                                   CLOSE_DATE = row.Field<DateTime?>("CLOSE_DATE") == null ? "" : row.Field<DateTime?>("CLOSE_DATE").Value.ToString(stringFormat),
                                   SawNo = row.Field<string>("SAW_NO"),
                                   SawStatus = row.Field<string>("SAW_STATUS"),
                                   SawDateTime = row.Field<string>("SAWDATETIME"),
                                   DEFECT_DESC = row.Field<string>("DEFECT_DESC"),
                                   RepairDesc = row.Field<string>("REPAIR_DESC"),
                                   StatusID = row.Field<string>("RLS DT/GRMS Status"),
                                   RejectReason = row.Field<string>("Reject_Reason"),
                                   RejectRemarks = row.Field<string>("REJECT_REMARKS")

                               }).ToList();
                        response.SingleResult = ssoList.FirstOrDefault();
                        response.IsSuccess = true;
                    }




                    //response.Result = StoreBusinessInstance.DisplayStoreProfile(15,15);
                    //response.IsSuccess = true;

                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
