using System;

namespace AccuIT.BusinessLayer.Services.BO
{
    /// <summary>
    /// Business Object class to define system settings values
    /// </summary>
    public class SystemSettingBO
    {
        public int SettingID { get; set; }
        public int CompanyID { get; set; }
        public int CoverageApproveDays { get; set; }
        public int LogoutTime { get; set; }
        public int LoginFailedAttempt { get; set; }
        public int IdleSystemDay { get; set; }
        public int MaxStorePerDay { get; set; }
        public string CoveragePlanFirstWindow { get; set; }
        public string CoveragePlanSecondWndow { get; set; }
        public int DataSyncInterval { get; set; }
        public int CoverageRejectionEditHours { get; set; }
        public int MaxLeaveMarkDays { get; set; }
        public string WeeklyOffDays { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
    }
}
