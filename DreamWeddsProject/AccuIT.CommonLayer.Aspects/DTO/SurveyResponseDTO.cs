using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SurveyResponseDTO
    {
        [DataMember]
        public long SurveyResponseID { get; set; }
        [DataMember]
        public long UserID { get; set; }
        [DataMember]
        public Nullable<int> StoreID { get; set; }
        [DataMember]
        public Nullable<int> ModuleID { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public string PictureFileName { get; set; }
        [DataMember]
        public string Lattitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }
        [DataMember]
        public bool IsOffline { get; set; }
        [DataMember]
        public Nullable<int> ModuleCode { get; set; }
        [DataMember]
        public Nullable<System.DateTime> BeatPlanDate { get; set; }
        [DataMember]
        public Nullable<long> CoverageID { get; set; }

        [DataMember]
        public Nullable<int> UserDeviceID
        {
            get;
            set;
        }

        //VC20140925        
        [DataMember]
        public Nullable<bool> UserOption { get; set; }
        //VC20140925
        //VC20140925        
        [DataMember]
        public Nullable<bool> RaceProfile { get; set; }
        //VC20140925
        [DataMember]
        public string strAssesmentStartTime { get; set; }
        [DataMember]
        public string strAssesmentEndTime { get; set; }
        [DataMember]
        public DateTime AssessmentStartTime { get; set; }
        [DataMember]
        public DateTime AssessmentEndTime { get; set; }
    }
}
