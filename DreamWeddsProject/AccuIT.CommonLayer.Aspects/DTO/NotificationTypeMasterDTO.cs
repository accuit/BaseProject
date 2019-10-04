using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class NotificationTypeMasterDTO
    {        
        [DataMember]
        public byte NotificationType { get; set; }
        [DataMember]
        public string NotificationTypeDescription { get; set; }
        [DataMember]
        public int UnreadCount { get; set; }
        [DataMember]
        public int TotalCount { get; set; }
    }
}
