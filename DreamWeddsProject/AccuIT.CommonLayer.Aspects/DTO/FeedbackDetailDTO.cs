using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class FeedbackDetailDTO
    {
        [DataMember]
        public string FeedbackNumber { get; set; }
        [DataMember]
        public int? TeamID { get; set; }
        [DataMember]
        public int? FeedbackCatID { get; set; }
        [DataMember]
        public int? FeedbackTypeID { get; set; }
        [DataMember]
        public float TimeTaken { get; set; }
        [DataMember]
        public List<FeedbackDetailLogDTO> FeedbackDetailLog { get; set; }
        [DataMember]
        public long SpocID { get; set; }
        [DataMember]
        public long UserID { get; set; }
        [DataMember]
        public byte CurrentFeedbackStatusID { get; set; }
        [DataMember]
        public string ImageURL { get; set; } // will contain the image name only
    }
    public class FeedbackDetailLogDTO
    {
        [DataMember]
        public string ResponseDate { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public string CreatedBy { get; set; }
        [DataMember]
        public string CreatedOn { get; set; }
        [DataMember]
        public string PendingWith { get; set; }
        [DataMember]
        public byte FeedbackStatusID { get; set; }
        //[DataMember]
        //public string FeedbackStatusName { get; set; }

    }
}
