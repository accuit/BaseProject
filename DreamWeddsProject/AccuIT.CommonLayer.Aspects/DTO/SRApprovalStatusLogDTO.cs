using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SRApprovalStatusLogDTO
    {
        [DataMember]
        public int SRApprovalStatusLogID { get; set; }

        [DataMember]
        public int SRRequestID { get; set; }

        [DataMember]
        public byte CurrentStatus { get; set; }

        [DataMember]
        public long AssignedTo { get; set; }

        public string AssignedToUserName { get; set; }

        [DataMember]
        public string Remarks { get; set; }

        [DataMember]
        public string CreatedDate { get; set; }

        [DataMember]
        public string ModifiedDate { get; set; }

    }
}
