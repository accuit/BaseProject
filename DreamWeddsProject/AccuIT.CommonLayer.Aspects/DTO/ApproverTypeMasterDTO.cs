using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class ApproverTypeMasterDTO
    {
        [DataMember]
        public int ApproverTypeID { get; set; }
        [DataMember]
        public string ApproverType { get; set; }
        [DataMember]
        public Nullable<bool> IsDeleted { get; set; }
    }
}
