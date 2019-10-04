using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class CollectionSurveyDTO
    {
        [DataMember]
        public long CollectionSurveyID { get; set; }
        [DataMember]
        public long UserID { get; set; }
        [DataMember]
        public Nullable<long> CoverageID { get; set; }
        [DataMember]
        public int StoreID { get; set; }
        [DataMember]
        public long SurveyResponseID { get; set; }
        [DataMember]
        public long UserRoleID { get; set; }
        [DataMember]
        public int PaymentModeID { get; set; }
        [DataMember]
        public double Amount { get; set; }
        [DataMember]
        public string Comments { get; set; }
        [DataMember]
        public string TransactionID { get; set; }
        [DataMember]
        public string PaymentDate { get; set; }
    }
}
