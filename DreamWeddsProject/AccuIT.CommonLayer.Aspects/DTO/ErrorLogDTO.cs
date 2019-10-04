using System;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    /// <summary>
    /// This DTO class purpose is to provide the data members to log message and error details into database
    /// </summary>
    [DataContract]
    public class ErrorLogDTO
    {
        [DataMember]
        public int LogID { get; set; }
        [DataMember]
        public Nullable<int> EventID { get; set; }
        [DataMember]
        public int Priority { get; set; }
        [DataMember]
        public string Severity { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public System.DateTime Timestamp { get; set; }
        [DataMember]
        public string MachineName { get; set; }
        [DataMember]
        public string AppDomainName { get; set; }
        [DataMember]
        public string ProcessID { get; set; }
        [DataMember]
        public string ProcessName { get; set; }
        [DataMember]
        public string ThreadName { get; set; }
        [DataMember]
        public string Win32ThreadId { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string FormattedMessage { get; set; }
    }
}
