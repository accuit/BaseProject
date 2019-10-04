
namespace AccuIT.CommonLayer.Aspects.Utilities
{
    public static class SessionVariables
    {
        public const string CurrentUserHierarchyForReport = "CurrentUserHierarchyForReport";
        public const string CurrentUserLevelForReport = "CurrentUserLevelForReport";
        public const string CurrentUserBreadCumForReport = "CurrentUserBreadCumForReport";
        public const string SelectedReportDateFrom = "SelectedReportDateFrom";
        public const string SelectedReportDateTo = "SelectedReportDateTo";
        public const string CurrentSelectedRoleForReport = "CurrentSelectedRoleForReport";
        public const string EmployeeListUnderCurrentUser = "EmployeeListUnderCurrentUser";
        public const string EmployeeListGeoUnderCurrentUser = "EmployeeListGeoUnderCurrentUser";
        public const string SurveyResponseUnderCurrentUser = "SurveyResponseUnderCurrentUser";
        public const string SurveyUserResponseUnderCurrentUser = "SurveyUserResponseUnderCurrentUser";
        public const string CollectionSurveyResponseUnderCurrentUser = "CollectionSurveyResponseUnderCurrentUser";
        public const string OrderBookingSurveyResponseUnderCurrentUser = "OrderBookingSurveyResponseUnderCurrentUser";
        public const string CompetitionSurveyResponseUnderCurrentUser = "CompetitionSurveyResponseUnderCurrentUser";
        public const string CoveragePlanUnderCurrentUser = "CoveragePlanUnderCurrentUser";
        public const string AllCoveragePlanUnderCurrentUser = "AllCoveragePlanUnderCurrentUser";
        public const string AttendanceDataUnderCurrentUser = "AttendanceDataUnderCurrentUser";
        public const string CounterShareResponseUnderCurrentUser = "CounterShareResponseUnderCurrentUser";
        public const string DisplayShareResponseUnderCurrentUser = "DisplayShareResponseUnderCurrentUser";
        public const string CounterShareResponseUnderCurrentUserComplete = "CounterShareResponseUnderCurrentUserComplete";
        public const string DisplayShareResponseUnderCurrentUserComplete = "DisplayShareResponseUnderCurrentUserComplete";
        public const string CounterShareDate = "CounterShareDate";
        public const string CounterShareDateChannelTypes = "CounterShareChannelTypes";
        public const string RoleMasters = "RoleMasters";
        public const string StoreUsers = "StoreUsers";
        public const string GeoTags = "GeoTags";
        public const string StoreMasters = "StoreMasters";
        public const string ProductMasters = "ProductMasters";
        public const string PaymentModes = "PaymentModes";        
        public const string StoreMastersCount = "StoreMastersCount";
        public const string AttendanceAndCoverageDataUnderCurrentUser = "AttendanceAndCoverageDataUnderCurrentUser";
        public const string DistintRoles = "DistintRoles";
        public const string AuthToken = "AuthToken";
        public const string SessionUserID = "EmpID";
        
    }

    public static class CacheVariables
    {
        public const string ddmmyyyyAttendanceAndCoverageReportCache = "ddmmyyyyAttendanceAndCoverageReportCache";
        public const string ddmmyyyyStoreUsers = "ddmmyyyyStoreUsers";
        public const string ddmmyyyyStores = "ddmmyyyyStores";
        public const string ddmmyyyyUserGeo = "ddmmyyyyUserGeo";
        public const string ddmmyyyyCoverageBatchNotification = "ddmmyyyyCoverageBatchNotification";
        public const string ddmmyyyyDisplayCounterShareData = "ddmmyyyyDisplayCounterShareData";
    }

    public static class CookieVariables
    {
        public const string AuthToken = "AuthToken";
    }
   
}
