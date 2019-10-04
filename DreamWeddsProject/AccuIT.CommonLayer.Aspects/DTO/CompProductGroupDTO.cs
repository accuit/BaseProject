using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SyncCompProductGroupDTO
    {
        [DataMember]
        public bool HasMoreRows;
        [DataMember]
        public string MaxModifiedDate;

        [DataMember]
        public List<CompProductGroupDTO> Result;
    }
    [DataContract]
    public class CompProductGroupDTO
    {
        [DataMember]
        public int CompProductGroupID { get; set; }
        [DataMember]
        public string ProductGroupName { get; set; }
        [DataMember]
        public string ProductGroupCode { get; set; }
        [DataMember]
        public string IsDeleted { get; set; }
    }
}
