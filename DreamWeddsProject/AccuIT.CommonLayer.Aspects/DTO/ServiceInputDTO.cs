using System;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class ServiceInputDTO
    {
        [DataMember]
        public string imei
        {
            get;
            set;
        }
        [DataMember]
        public string username
        {
            get;
            set;
        }
        [DataMember]
        public string password
        {
            get;
            set;
        }
        [DataMember]
        public double lattitude
        {
            get;
            set;
        }
        [DataMember]
        public double longitude
        {
            get;
            set;
        }
        [DataMember]
        public string apkVersion
        {
            get;
            set;
        }

        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public string BrowserName { get; set; }
        [DataMember]
        public string IPAddress { get; set; }
        [DataMember]
        public bool RequireAnnouncement { get; set; }

    }
}
