using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class PlanogramResponseDTO
    {        
        [DataMember]
        public double Adherence { get; set; }

        [DataMember]
        public long SurveyResponseID { get; set; }

        [DataMember]
        public int CompProductGroupID { get; set; }

        [DataMember]
        public int ClassID { get; set; }

        [DataMember]
        public string Class { get; set; }

        [DataMember]
        public ICollection<PlanogramCompititorResponseDTO> PlanogramCompititorResponses { get; set; }

        [DataMember]
        public ICollection<PlanogramProductResponseDTO> PlanogramProductResponses { get; set; }
     

    }
}
