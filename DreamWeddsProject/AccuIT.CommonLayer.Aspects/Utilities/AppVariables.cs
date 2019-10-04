
namespace AccuIT.CommonLayer.Aspects.Utilities
{
    public static class AppVariables
    {
        /// <summary>
        /// Enum to define category names for logging and tracing in application
        /// </summary>
        public enum AppLogTraceCategoryName
        {
            General,
            Tracing,
            DiskFiles,
            Important,
            EmailLog,
            BlockedByFilter,
            NotificationListener,
            EmailListener,
        }


    }
    public static class StoredProcedureVariables
    {
        public const string ActivityReport = "SPGetActivityReportNew";
        public const string SORLS = "spGetClaimDetailsByClaimNoForSMD";
    }
}
