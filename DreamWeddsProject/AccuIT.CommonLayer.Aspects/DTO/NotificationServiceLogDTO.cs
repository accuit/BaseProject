using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class NotificationServiceLogDTO
    {
        [DataMember]
        public int NotificationServiceID { get; set; }
        [DataMember]
        public int NotificationType { get; set; }
        [DataMember]
        public string PushNotificationMessage { get; set; }
        [DataMember]
        public string NotificationDate { get; set; }
        [DataMember]
        public byte ReadStatus { get; set; }
        [DataMember]
        public int NotificationID { get; set; }
    }
}
