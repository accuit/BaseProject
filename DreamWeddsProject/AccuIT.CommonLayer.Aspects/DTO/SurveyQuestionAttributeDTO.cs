using System;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SurveyQuestionAttributeDTO
    {
        [DataMember]
        public int SurveyOptionID { get; set; }
        [DataMember]
        public int SurveyQuestionID { get; set; }
        [DataMember]
        public string OptionValue { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public int Sequence { get; set; }
        [DataMember]
        public bool IsAffirmative { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; } 

    }
}
