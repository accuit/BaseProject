using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [Serializable]
    public class GetSalesReturnRequestDTO
    {
        [DataMember]
        public int SRRequestID;

        [DataMember]
        public string StartDate;
        [DataMember]
        public string EndDate;
        [DataMember]
        public string LastSyncDateTime;
    }
}
