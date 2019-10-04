using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class CoverageWindowDTO
    {
        
        [DataMember]
        public string CoverageDate { get; set; }
        [DataMember]
        public bool ExceptionFlag { get; set; }
        [DataMember]
        public bool IsCurrentMonth { get; set; }
    }
}
