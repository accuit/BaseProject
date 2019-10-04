using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SRDetailDTO
    {
        [DataMember]
        public int SRDetailID { get; set; }

        [DataMember]
        public int SRRequestID { get; set; }

        [DataMember]
        public int ProductID { get; set; }

        [DataMember]
        public string BarCode { get; set; }

        [DataMember]
        public string ReasonForReturn { get; set; }

        [DataMember]
        public byte CurrentStatus { get; set; }

        [DataMember]
        public string Remarks { get; set; }

        [DataMember]
        public Nullable<long> CreatedBy { get; set; }

        [DataMember]
        public string CreatedDate { get; set; }

        [DataMember]
        public Nullable<long> ModifiedBy { get; set; }

        [DataMember]
        public string ModifiedByUserName { get; set; }

        [DataMember]
        public string ModifiedDate { get; set; }


    }
}
