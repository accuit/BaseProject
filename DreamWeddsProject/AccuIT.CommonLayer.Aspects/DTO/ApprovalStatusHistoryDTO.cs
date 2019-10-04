using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
     public class ApprovalStatusHistoryDTO
    {
        public int ApprovalStatusHistoryID { get; set; }
        public int EMSExpenseDetailID { get; set; }
        public Nullable<long> AssignedToUser { get; set; }
        public Nullable<int> AssignedRoleID { get; set; }
        public Nullable<byte> ApproverTypeID { get; set; }
        public Nullable<byte> Sequence { get; set; }
        public byte Status { get; set; }
        public string Remark { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        //public virtual RoleMaster RoleMaster { get; set; }
        //public virtual EMSExpenseDetail EMSExpenseDetail { get; set; }
        //public virtual UserMaster UserMaster { get; set; }
        //public virtual UserMaster UserMaster1 { get; set; }
    }
}
