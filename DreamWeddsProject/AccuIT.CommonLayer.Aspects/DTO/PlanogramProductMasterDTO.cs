using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SyncPlanogramProductMasterDTO
    {
        [DataMember]
        public bool HasMoreRows { get; set; }
        [DataMember]
        public string MaxModifiedDate { get; set; }
        [DataMember]
        public List<PlanogramProductMasterDTO> Result;
    }

    [DataContract]
    public class PlanogramProductMasterOutputDTO
    {
        [DataMember]
        public List<PlanogramProductMasterDTO> PlanogramProductMasterList { get; set; }
        [DataMember]
        public int TotalRow { get; set; }
    }
    [DataContract]
    public class PlanogramProductMasterDTO
    {
        [DataMember]
        public int PlanogramProductMasterID { get; set; }
        [DataMember]
        public string Class { get; set; }
        [DataMember]
        public string ChannelType { get; set; }
        [DataMember]
        public string CompProductGroup { get; set; }
        [DataMember]
        public string ProductCode { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }
}
