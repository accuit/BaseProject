using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class CompetitionSurveyDTO
    {
        [DataMember]
        public long CompSurveyID { get; set; }
        [DataMember]
        public Nullable<int> ProductTypeID { get; set; }
        [DataMember]
        public Nullable<int> ProductGroupID { get; set; }
        [DataMember]
        public int CompetitorID { get; set; }
        [DataMember]
        public Nullable<int> ProductID { get; set; }
        [DataMember]
        public long UserID { get; set; }
        [DataMember]
        public long UserRoleID { get; set; }
        [DataMember]
        public int StoreID { get; set; }
        [DataMember]
        public long SurveyResponseID { get; set; }
        [DataMember]
        public int SurveyQuestionID { get; set; }
        [DataMember]
        public int UserResponse { get; set; }
        [DataMember]
        public Nullable<long> CoverageID { get; set; }
        [DataMember]
        public byte CompetitionType { get; set; }
        [DataMember]
        public Nullable<double> Sellout { get; set; }
    }
}
