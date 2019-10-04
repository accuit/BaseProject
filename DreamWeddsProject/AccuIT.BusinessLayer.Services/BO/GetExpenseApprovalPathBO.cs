using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class GetExpenseApprovalPathBO
    {
        public int ApproverPathMasterID { get; set; }
        public string ModuleName { get; set; }
        public string ExpenseType { get; set; }
        public string Role { get; set; }
        public string ApproverRole { get; set; }
        public string ApproverUser { get; set; }
        public string ApproverType { get; set; }
        public int? ApproverTypeValue { get; set; }
        public int Sequence { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
