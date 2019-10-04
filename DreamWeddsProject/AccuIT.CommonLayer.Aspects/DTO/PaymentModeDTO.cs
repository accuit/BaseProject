using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{

    [DataContract]
    public class SyncPaymentModeDTO
    {

        [DataMember]
        public bool HasMoreRows;
        [DataMember]
        public string MaxModifiedDate;

        [DataMember]
        public List<PaymentModeDTO> Result;
    }

    [DataContract]
    public class PaymentModeDTO
    {
        [DataMember]
        public int PaymentModeID { get; set; }
        [DataMember]
        public string ModeName { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }
}
