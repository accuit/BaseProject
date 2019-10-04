using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    /// <summary>
    /// Class to provide the services exception details in form fault contract members
    /// </summary>
    [DataContract]
    public class ServiceFaultDTO
    {
        /// <summary>
        /// Property to get set fault message of service method execution
        /// </summary>
        [DataMember]
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get set fault code of service method execution
        /// </summary>
        [DataMember]
        public string FaultCode
        {
            get;
            set;
        }
    }
}
