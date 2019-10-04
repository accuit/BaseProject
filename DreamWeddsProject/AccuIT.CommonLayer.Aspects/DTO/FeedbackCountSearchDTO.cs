using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class FeedbackCountSearchDTO
    {
        [DataMember]
        public string FeedbackTeamIDs { get; set; }
        [DataMember]
        public string FeedbackCatIDs { get; set; }
        [DataMember]
        public string FeedbackTypeIDs { get; set; }

    }
}
