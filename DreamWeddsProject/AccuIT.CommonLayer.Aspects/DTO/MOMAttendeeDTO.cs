using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class MOMAttendeeDTO
    {
        [DataMember]
        public int AttendeeId { get; set; }
        [DataMember]
        public string AttendeeName{ get; set; }
        [DataMember]
        public long MOMId { get; set; }
    }
}
