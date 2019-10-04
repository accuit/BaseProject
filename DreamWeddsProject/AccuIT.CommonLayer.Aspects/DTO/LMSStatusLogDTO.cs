using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class LMSStatusLogDTO
    {
        [DataMember]
        public int LMSStatusLogID { get; set; }
        [DataMember]
        public int LeaveMasterID { get; set; }
        [DataMember]
        public byte CurrentStatus { get; set; }
        [DataMember]
        public string CreatedByUserName { get; set; }
        [DataMember]
        public long CreatedBy { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public string Remarks { get; set; }

    }
}
