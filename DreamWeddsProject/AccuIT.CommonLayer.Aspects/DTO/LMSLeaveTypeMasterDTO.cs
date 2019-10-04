using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class LMSLeaveTypeMasterDTO
    {
        [DataMember]
        public int LMSLeaveTypeMasterID { get; set; }        
        [DataMember]
        public string LeaveType { get; set; }
        [DataMember]
        public string LeaveTypeCode { get; set; }
        [DataMember]
        public double LeavesTaken { get; set; }

    }
}
