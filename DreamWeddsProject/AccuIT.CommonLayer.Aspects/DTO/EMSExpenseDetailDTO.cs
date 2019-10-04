using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class EMSExpenseDetailDTO
    {
        [DataMember]
        public int EMSExpenseDetailID { get; set; }
        [DataMember]
        public int EMSExpenseTypeMasterID { get; set; }
        [DataMember]
        public bool Billable { get; set; }
        [DataMember]
        public string BillableTo { get; set; }

        [DataMember]
        public string ExpenseType { get; set; }

        //  [DataMember]
        //  public byte ExpenseStatus { get; set; }

        [DataMember]
        public string ModifiedDateStr { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public byte Status { get; set; }

        [DataMember]
        public DateTime CreatedDate { get; set; }

        //created date in string
        [DataMember]
        public string ExpenseDate { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public string CreatedByUsername { get; set; }

        public int? ModifiedBy { get; set; }
        [DataMember]
        public List<EMSBillDetailDTO> EMSBillDetails { get; set; }

        public List<ApprovalStatusHistoryDTO> ApprovalStatusHistories { get; set; }

        [DataMember]
        public int EMSExpenseDetailIDClient { get; set; }

        [DataMember]
        public int EMSExpenseDetailIDServer { get; set; }

        //new returns

        [DataMember]
        public string PendingWith { get; set; }

        [DataMember]
        public long? AssignedToUser { get; set; }

        [DataMember]
        public byte AssignedStatus { get; set; }

        [DataMember]
        public bool IsExpenseEditable { get; set; }
    }

    [DataContract]
    public class EMSExpenseDetailOutputDTO
    {
        [DataMember]
        public int EMSExpenseDetailIDServer { get; set; }
        [DataMember]
        public int EMSExpenseDetailIDClient { get; set; }

        [DataMember]
        public List<EMSBillDetailOutputDTO> EMSBillDetails { get; set; }
    }
}
