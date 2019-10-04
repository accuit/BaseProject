using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SubmitFeedbacksDTO
    {
        [DataMember]
        public int FeedbackTeamID { get; set; }
        [DataMember]
        public int FeedbackCatID { get; set; }
        [DataMember]
        public int FeedbackTypeID { get; set; }
        [DataMember]
        public string ImageBytes { get; set; }        
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public int storeID { get; set; }   
    }
}
