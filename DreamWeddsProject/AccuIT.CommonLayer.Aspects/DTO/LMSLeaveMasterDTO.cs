using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class LMSLeaveMasterDTO
    {
        [DataMember]
        public int LeaveMasterID { get; set; }
        [DataMember]
        public string LeaveSubject { get; set; }
        [DataMember]
        public int NumberOfLeave { get; set; }
        [DataMember]
        public long CreatedBy { get; set; }        
        [DataMember]
        public string CreatedByUserName { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string AppliedDate { get; set; }
        [DataMember]
        public byte CurrentStatus { get; set; }
        [DataMember]
        public long PendingWith { get; set; }
        [DataMember]
        public string PendingWithUserName { get; set; }
        [DataMember]
        public string ModifiedDate { get; set; }
        [DataMember]
        public long? ModifiedBy { get; set; }
        [DataMember]
        public string ModifiedByUserName { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public int LMSLeaveTypeMasterID { get; set; }
        [DataMember]
        public List<LMSLeaveDetailDTO> LMSLeaveDetails { get; set; }
        [DataMember]
        public List<LMSStatusLogDTO> LMSStatusLogs { get; set; }

    }
}

