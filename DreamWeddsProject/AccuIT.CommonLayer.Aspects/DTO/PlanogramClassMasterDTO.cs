using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SyncPlanogramClassMasterDTO
    {
        [DataMember]
        public bool HasMoreRows { get; set; }
        [DataMember]
        public string MaxModifiedDate { get; set; }
        [DataMember]
        public List<PlanogramClassMasterDTO> Result;

    }
    [DataContract]
    public class PlanogramClassMasterDTO
    {
        [DataMember]
        public int ClassID { get; set; }
        [DataMember]
        public int StartRange { get; set; }
        [DataMember]
        public int EndRange { get; set; }
        [DataMember]
        public string Class { get; set; }
        [DataMember]
        public string ChannelType { get; set; }
        [DataMember]
        public Nullable<int> CompProdGroupID { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }
}
