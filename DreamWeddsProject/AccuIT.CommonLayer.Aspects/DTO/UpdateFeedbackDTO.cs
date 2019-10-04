using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class UpdateFeedbackDTO
    {
        [DataMember]
        public int FeedbackID { get; set; }
        [DataMember]
        public byte NewStatusID { get; set; }
        [DataMember]
        public string ETRDate { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public long UserIdPendingWith { get; set; }
    }
}
