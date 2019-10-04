using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class EMSBillDetailDTO
    {
     
        [DataMember]
        public int EMSBillDetailID { get; set; }

        [DataMember]
        public int EMSExpenseDetailID { get; set; }

        [DataMember]
        public int EMSBillDetailIDServer { get; set; }

        [DataMember]
        public int EMSBillDetailIDClient { get; set; }

        [DataMember]
        public string BillDateStr { get; set; }

        [DataMember]
        public DateTime BillDate { get; set; }

        [DataMember]
        public string BillNo { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public decimal Amount { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public int? ModifiedBy { get; set; }

        [DataMember]
        public List<EMSBillDocumentDetailDTO> EMSBillDocumentDetails { get; set; }


    }

    public class EMSBillDetailOutputDTO
    {
     
        [DataMember]
        public int EMSBillDetailIDServer { get; set; }

        [DataMember]
        public int EMSBillDetailIDClient { get; set; }

        [DataMember]
        public List<EMSBillDocumentDetailOutputDTO> EMSBillDocumentDetails { get; set; }
    }

}
