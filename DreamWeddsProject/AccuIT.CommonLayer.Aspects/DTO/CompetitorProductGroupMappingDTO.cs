using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SyncCompetitorProductGroupMappingDTO
    {
        [DataMember]
        public bool HasMoreRows { get; set; }
        [DataMember]
        public string MaxModifiedDate { get; set; }
        [DataMember]
        public List<CompetitorProductGroupMappingDTO> Result;

    }

    [DataContract]
    public class CompetitorProductGroupMappingDTO
    {
        [DataMember]
        public int MappingID { get; set; }
        [DataMember]
        public int CompetitorID { get; set; }
        [DataMember]
        public int CompProductGroupID { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }

    }
}
