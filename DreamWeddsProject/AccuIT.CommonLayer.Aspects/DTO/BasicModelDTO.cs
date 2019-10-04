using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class BasicModelDTO
    {
        [DataMember]
        public string BasicModelCode { get; set; }
        [DataMember]
        public string BasicModelName { get; set; }
    }
}
