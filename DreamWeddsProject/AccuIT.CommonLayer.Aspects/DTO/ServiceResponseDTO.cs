using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    /// <summary>
    /// This class is used to define the response data for service/business methods
    /// </summary>

    [DataContract]
    public class ServiceResponseDTO
    {
        
        [DataMember]
        public string StatusCode
        {
            get;
            set;
        }

        [DataMember]
        public string Message
        {
            get;
            set;
        }
    
        [DataMember]
        public string UserID
        {
            get;
            set;
        }

        [DataMember]
        public string UserCode
        {
            get;
            set;
        }

        [DataMember]
        public string CompanyId
        {
            get;
            set;
        }


        [DataMember]
        public string APIKey
        {
            get;
            set;
        }

        [DataMember]
        public string APIToken
        {
            get;
            set;
        }

        [DataMember]
        public int UserDeviceID
        {
            get;
            set;
        }

        [DataMember]
        public int RoleID
        {
            get;
            set;
        }

    }
}
