using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class PlanogramProductResponseDTO
    {        
        [DataMember]
        public string ProductCode { get; set; }

        [DataMember]
        public bool IsAvailable { get; set; }
    }
}
