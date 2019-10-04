using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class PushNotificationDTO
    {
        [DataMember]
        public string AppID { get; set; }

        [DataMember]
        public string SenderID { get; set; }

        [DataMember]
        public string RegistrationKeyId { get; set; }

        [DataMember]
        public string ProjectURL { get; set; }
    }
}
