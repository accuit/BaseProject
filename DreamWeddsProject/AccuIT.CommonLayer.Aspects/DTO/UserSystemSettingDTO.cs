using System;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    /// <summary>
    /// DTO class to get user system settings value
    /// </summary>
    public class UserSystemSettingDTO
    {
        [DataMember]
        public long UserSystemID { get; set; }
        [DataMember]
        public long UserID { get; set; }
        [DataMember]
        public bool IsAPKLoggingEnabled { get; set; }
        [DataMember]
        public DateTime? CoverageExceptionWindow { get; set; }
        [DataMember]
        public bool IsCoverageException { get; set; }
    }
}
