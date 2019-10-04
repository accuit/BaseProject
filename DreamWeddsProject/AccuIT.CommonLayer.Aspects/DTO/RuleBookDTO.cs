using AccuIT.CommonLayer.Aspects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class RuleBookDTO
    {
        public RuleBookDTO()
        {
            ApproverTypes = new List<ApproverTypeMasterDTO>();
            ActivityMasters = new List<ActivityMasterDTO>();
            ActivityApporverMasters = new List<ActivityApporverMasterDTO>();

        }

        [DataMember]
        public List<ApproverTypeMasterDTO> ApproverTypes { get; set; }
        [DataMember]
        public List<ActivityMasterDTO> ActivityMasters { get; set; }
        [DataMember]
        public List<ActivityApporverMasterDTO> ActivityApporverMasters { get; set; }

    }
}
