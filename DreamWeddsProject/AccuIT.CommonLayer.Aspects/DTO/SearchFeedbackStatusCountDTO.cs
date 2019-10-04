using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    
    [DataContract]
    public class SearchFeedbackStatusCountDTO
    {
        [DataMember]
        public byte FeedbackStatusID { get; set; }
        [DataMember]
        public string FeedbackStatusName { get; set; }
        [DataMember]
        public int FeedbackCount { get; set; }        
    }
}
