using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class EMSBillDocumentDetailDTO
    {
        [DataMember]
        public int EMSBillDocumentDetailID { get; set; }

        [DataMember]
        public int EMSBillDocumentDetailIDClient { get; set; }

        [DataMember]
        public int EMSBillDocumentDetailIDServer { get; set; }

        [DataMember]
        public int EMSBillDetailID { get; set; }

        [DataMember]
        public string DocumentName { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        //[DataMember]
        //public System.DateTime CreatedDate { get; set; }
        //public string CreatedDate { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        [DataMember]
        public int? ModifiedBy { get; set; }

    }



    public class EMSBillDocumentDetailOutputDTO
    {
        [DataMember]
        public int EMSBillDocumentDetailIDServer { get; set; }

        [DataMember]
        public int EMSBillDocumentDetailIDClient { get; set; }


    }

}
