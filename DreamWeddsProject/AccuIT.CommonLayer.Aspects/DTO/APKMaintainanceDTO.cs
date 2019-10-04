using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class APKMaintainanceDTO
    {
        [DataMember]
        public bool IsUpdated { get; set; }
        [DataMember]
        public string APKURL { get; set; }
       
    }
}
