using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class UpdateNotificationStatusDTO
    {
        [DataMember]
        public long? NotificationServiceID { get; set; }
        [DataMember]
        public byte ReadStatus { get; set; }
      
        [DataMember]
        public long? NotificationID { get; set; } 
        [DataMember]
        public long? UserID { get; set; } 
        [DataMember]
        public string IMEINumber { get; set; }
        
    }
}
