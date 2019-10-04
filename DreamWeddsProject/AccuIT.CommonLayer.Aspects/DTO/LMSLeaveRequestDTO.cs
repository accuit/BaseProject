using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class LMSLeaveRequestDTO
    {
        [DataMember]
        public int LeaveTypeID;
        [DataMember]
        public int RequestType;

        [DataMember]
        public string StartDate;
        [DataMember]
        public string EndDate;
        [DataMember]
        public string LastSyncDateTime;
        
    }
}
