using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{

    [DataContract]
    public class SRRequestDTO
    {
        [DataMember]
        public int SRRequestID { get; set; }

        [DataMember]
        public int StoreID { get; set; }

        [DataMember]
        public byte CurrentStatus { get; set; }

        [DataMember]
        public Nullable<long> PendingWith { get; set; }

        [DataMember]
        public string PendingWithUserName { get; set; }

        [DataMember]
        public string Remarks { get; set; }

        [DataMember]
        public long CreatedBy { get; set; }

        [DataMember]
        public string CreatedByUserName { get; set; }

        [DataMember]
        public string CreatedDate { get; set; }

        [DataMember]
        public Nullable<long> ModifiedBy { get; set; }

        [DataMember]
        public string ModifiedByUserName { get; set; }

        [DataMember]
        public string ModifiedDate { get; set; }

        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public List<SRApprovalStatusLogDTO> SRApprovalStatusLogs { get; set; }

        [DataMember]
        public List<SRDetailDTO> SRDetails { get; set; }

    }
}
