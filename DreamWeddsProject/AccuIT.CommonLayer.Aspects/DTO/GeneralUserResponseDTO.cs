using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class GeneralUserResponseDTO
    {
        [DataMember]
        public long GeneralUserResponseID { get; set; }
        [DataMember]
        public long UserID { get; set; }
        [DataMember]
        public int SurveyQuestionID { get; set; }
        [DataMember]
        public string UserResponse { get; set; }
        [DataMember]
        public int SurveyTypeID { get; set; }
        [DataMember]
        public Nullable<System.DateTime> SurveyDate { get; set; }

    }
}
