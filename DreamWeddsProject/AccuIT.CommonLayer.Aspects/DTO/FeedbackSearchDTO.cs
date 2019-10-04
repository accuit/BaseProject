using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class FeedbackSearchDTO
    {
        [DataMember]
        public string FeedbackTeamIDs { get; set; }
        [DataMember]
        public string FeedbackCatIDs { get; set; }
        [DataMember]
        public string FeedbackTypeIDs { get; set; }
        [DataMember]
        public long LastFeedbackID { get; set; }
        [DataMember]
        public int Rowcounter { get; set; }
        [DataMember]
        public byte PendingWithType { get; set; }

        //[DataMember]
        //public byte StatusID { get; set; }

        [DataMember]
        public string StatusIDs { get; set; }

        [DataMember]
        public string DateFrom { get; set; }
        [DataMember]
        public string DateTo { get; set; }

    }
}
