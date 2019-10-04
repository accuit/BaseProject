using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class EMSExpenseApprovalPathDTO
    {
        [DataMember]
        public int EMSExpenseApprovalPathID { get; set; }

        [DataMember]
        public int EMSExpenseTypeMasterID { get; set; }

        [DataMember]
        public bool IsActive { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public int RoleID { get; set; }

        [DataMember]
        public byte ApprovalType { get; set; }

        //[DataMember]
        //public System.DateTime CreatedDate { get; set; }

        [DataMember]
        public int CreatedBy { get; set; }

        //[DataMember]
        //public System.DateTime ModifiedDate { get; set; }

        [DataMember]
        public int ModifiedBy { get; set; }
    }
}
