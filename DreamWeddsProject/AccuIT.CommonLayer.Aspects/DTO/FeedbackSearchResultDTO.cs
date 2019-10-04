using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class FeedbackSearchResultOutputDTO
    {
        [DataMember]
        public List<FeedbackSearchResultDTO> FeedbacksearchList { get; set; }
        [DataMember]
        public bool HasMoreRows { get; set; }
        //[DataMember]
        //public List<SearchFeedbackStatusCountDTO> SearchFeedbackStatusCountList { get; set; }

        [DataMember]
        public string StatusCountStr { get; set; }

        
    }

    [DataContract]
    public class FeedbackSearchResultDTO
    {
        [DataMember]
        public long FeedbackID { get; set; }
        [DataMember]
        public int FeedbackTeamID { get; set; }
        [DataMember]
        public int FeedbackCatID { get; set; }
        [DataMember]
        public int FeedbackTypeID { get; set; }
        [DataMember]
        public long UserID { get; set; }
        [DataMember]
        public long SpocID { get; set; }
        [DataMember]
        public int CurrentStatusID { get; set; }
        [DataMember]
        public string CreatedBy { get; set; } //User Name
        [DataMember]
        public string CreatedOn { get; set; }
        [DataMember]
        public int PendingSince { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public string LastUpdatedOn { get; set; }
        [DataMember]
        public string FeedbackTypeName { get; set; }
        [DataMember]
        public string TeamName { get; set; }
        [DataMember]
        public string FeedbackCategoryName { get; set; }
    }
}
