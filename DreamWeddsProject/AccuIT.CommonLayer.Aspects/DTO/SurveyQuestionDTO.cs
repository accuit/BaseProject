using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SurveyQuestionDTO
    {
        [DataMember]
        public int SurveyQuestionID { get; set; }
        [DataMember]
        public string Question { get; set; }
        [DataMember]
        public int QuestionTypeID { get; set; }
        [DataMember]
        public Nullable<int> ProductTypeID { get; set; }
        [DataMember]
        public Nullable<int> ProductGroupID { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public Nullable<int> ModuleID { get; set; }
        //[DataMember]
        //public bool IsDailyQuote { get; set; }
        //[DataMember]
        //public Nullable<System.DateTime> QuoteDate { get; set; }
        //[DataMember]
        //public string HintImageName { get; set; }
        [DataMember]
        public Nullable<int> ModuleCode { get; set; }
        //[DataMember]
        //public long UserRoleID { get; set; }
        //[DataMember]
        //public int RoleID { get; set; }
        [DataMember]
        public IList<SurveyQuestionAttributeDTO> Options { get; set; }
        [DataMember]
        public int Sequence { get; set; }
        [DataMember]
        public int TextLength { get; set; }
        [DataMember]
        public int DependentOptionID { get; set; }
        //VC20140905
        [DataMember]
        public string QuestionImage { get; set; }
        //VC20140905
        [DataMember]
        public Nullable<int> RepeaterTypeID { get; set; }
        [DataMember]
        public string RepeaterText { get; set; }
        [DataMember]
        public Nullable<int> RepeatMaxTimes { get; set; }
        [DataMember]
        public bool IsMandatory { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }

        
    }
}
