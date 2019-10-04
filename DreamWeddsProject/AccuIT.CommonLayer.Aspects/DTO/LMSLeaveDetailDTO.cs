using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class LMSLeaveDetailDTO
    {
        [DataMember]
        public int LMSLeaveDetailID { get; set; }
        [DataMember]
        public int LeaveMasterID { get; set; }
        [DataMember]
        public string LeaveDate { get; set; }
        [DataMember]
        public bool IsHalfDay { get; set; }
        [DataMember]
        public long CreatedBy { get; set; }
        [DataMember]

        public byte CurrentStatus { get; set; }
        [DataMember]
        public string ModifiedDate { get; set; }
        [DataMember]
        public long? ModifiedBy { get; set; }
        [DataMember]
        public string ModifiedByUserName { get; set; }



    }
}

