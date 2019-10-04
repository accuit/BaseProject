using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class PartnerMeetingDTO
    {
        [DataMember]
        public int StoreID { get; set; }
        [DataMember]
        public long UserID { get; set; }
        [DataMember]
        public long UserDetailID { get; set; }
        [DataMember]
        public string Remarks { get; set; }
        [DataMember]
        public string ShipToCode { get; set; }
    }
}
