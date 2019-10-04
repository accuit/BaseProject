using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class ActivityApporverMasterDTO
    {
        [DataMember]
        public int ActivityApprverID { get; set; }
        [DataMember]
        public Nullable<int> ActivityID { get; set; }
        [DataMember]
        public Nullable<int> ApproverTypeID { get; set; }
        [DataMember]
        public string ApproverValue { get; set; }
        [DataMember]
        public string Branch { get; set; }

    }
}
