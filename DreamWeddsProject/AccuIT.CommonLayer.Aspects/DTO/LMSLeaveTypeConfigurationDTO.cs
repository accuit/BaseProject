using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class LMSLeaveTypeConfigurationDTO
    {
        [DataMember]
        public int LMSLeaveTypeConfigurationID { get; set; }
        [DataMember]
        public int LMSLeaveTypeMasterID { get; set; }
        [DataMember]
        public string ConfigurationText { get; set; }
        [DataMember]
        public int ConfigurationValue { get; set; }
        [DataMember]
        public int ConfigValue { get; set; }
        
    }
}
