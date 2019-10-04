using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SyncTableDTO
    {
        [DataMember]
        public int SyncTableID { get; set; }
        [DataMember]
        public string EntityName { get; set; }
        [DataMember]
        public System.DateTime SyncDate { get; set; }
    }
}
