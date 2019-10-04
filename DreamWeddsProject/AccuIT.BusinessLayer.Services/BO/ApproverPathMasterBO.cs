using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class ApproverPathMasterBO
    {
        public int ApproverPathMasterID { get; set; }
        public int CompanyID { get; set; }
        public Nullable<int> EMSExpenseTypeMasterId { get; set; }
        public int ApprovalPathTypeID { get; set; }
        public int RoleID { get; set; }
        public Nullable<int> ApproverRoleID { get; set; }
        public Nullable<long> ApproverUserID { get; set; }
        public byte ApproverTypeID { get; set; }
        public int Sequence { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> TeamID { get; set; }
        public string TeamCode { get; set; }

        public Nullable<int> ApproverTeamID { get; set; }

        public string ApproverTeamCode { get; set; }

    }
}
