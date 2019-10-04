using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    public class UserBeatDetailsDTO
    {
        [DataMember]
        public IList<UserBeatDetailDTO> UserBeatDetails { get; set; }
        [DataMember]
        public string TotalWorkingDays { get; set; }
        [DataMember]
        public string TotalOutletPlanned { get; set; }
        [DataMember]
        public string TotalOff { get; set; }
        [DataMember]
        public string LeaveDetail { get; set; }
        [DataMember]
        public Int32 TotalAssignedOutlet { get; set; }

    }
}
