using System;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class SystemSettingsDTO
    {
        [DataMember]
        public int SettingID { get; set; }
        [DataMember]
        public int CompanyID { get; set; }
        [DataMember]
        public int CoverageApproveDays { get; set; }
        [DataMember]
        public int LogoutTime { get; set; }
        [DataMember]
        public int LoginFailedAttempt { get; set; }
        [DataMember]
        public int IdleSystemDay { get; set; }
        [DataMember]
        public int MaxStorePerDay { get; set; }
        [DataMember]
        public string CoveragePlanFirstWindow { get; set; }
        [DataMember]
        public string CoveragePlanSecondWndow { get; set; }
        [DataMember]
        public int DataSyncInterval { get; set; }
        [DataMember]
        public int CoverageRejectionEditHours { get; set; }
        [DataMember]
        public int MaxLeaveMarkDays { get; set; }
        [DataMember]
        public string WeeklyOffDays { get; set; }
        [DataMember]
        public System.DateTime CreatedDate { get; set; }
        [DataMember]
        public long CreatedBy { get; set; }
        [DataMember]
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        [DataMember]
        public Nullable<long> ModifiedBy { get; set; }
    }
}
