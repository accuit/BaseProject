using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SurveyUserResponseDTO
    {
        [DataMember]
        public long SurveyUserResponseID { get; set; }
        [DataMember]
        public long SurveyResponseID { get; set; }
        [DataMember]
        public int SurveyQuestionID { get; set; }
        [DataMember]
        public string UserResponse { get; set; }
        [DataMember]
        public int SurveyTypeID { get; set; }
        [DataMember]
        public List<SurveyRepeatResponseDTO> SurveyRepeatResponses { get; set; }

    }
    public class SurveyRepeatResponseDTO
    {
        public int SurveyQuestionRepeaterID { get; set; }
        public string UserResponse { get; set; }

    }
}
