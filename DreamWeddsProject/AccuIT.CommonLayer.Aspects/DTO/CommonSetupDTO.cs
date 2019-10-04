using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class CommonSetupDTO
    {
        [DataMember]
        public int CommonSetupID { get; set; }
        [DataMember]
        public string DisplayText { get; set; }
        public string MainType { get; set; }
        public string SubType { get; set; }
        public int DisplayValue { get; set; }
        public int ParentID { get; set; }
        
    }
   
}
