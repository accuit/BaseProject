using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class PlanogramCompititorResponseDTO
    {
        [DataMember]
        public int CompetitorID { get; set; }

        [DataMember]
        public int Value { get; set; }
    }
}
