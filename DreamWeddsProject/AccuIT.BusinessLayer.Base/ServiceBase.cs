using System;
using AccuIT.CommonLayer.EntityMapper;


namespace AccuIT.BusinessLayer.Base
{
     /// <summary>
    /// Base class to define the common service through out the business layer of application
    /// </summary>
    public abstract class ServiceBase : MarshalByRefObject
    {
        /// <summary>
        /// Property to get set object mapping instance
        /// </summary>
        [Microsoft.Practices.Unity.Dependency]
        public Mapper ObjectMapper
        {
            get;
            set;
        }

         /// <summary>
        /// Struct to get the container instance names for Data/Persistence layer registrations
        /// </summary>
        public struct ContainerDataLayerInstanceNames
        {
            public const string EMP_REPOSITORY = "AccuIT_EmpDataImpl";
            public const string SYSTEM_REPOSITORY = "AccuIT_SystemDataImpl";
            public const string STORE_REPOSITORY = "AccuIT_StoreDataImpl";
            public const string EMAIL_REPOSITORY = "AccuIT_EmailDataImpl";
            public const string ACTIVITY_REPOSITORY = "AccuIT_ActivityDataImpl";
            public const string SECURITY_REPOSITORY = "AccuIT_SecurityDataImpl";
            public const string Report_REPOSITORY = "AccuIT_ReportDataImpl";
            public const string WEDDING_REPOSITORY = "AccuIT_WeddingDataImpl";

        }

          /// <summary>
        /// Struct to get the container instance names for Business layer registrations
        /// </summary>
        public struct ContainerBusinessLayerInstanceNames
        {
            public const string EMP_MANAGER = "AccuIT_EmpManager";
            public const string SYSTEM_MANAGER = "AccuIT_SystemManager";
            public const string STORE_MANAGER = "AccuIT_StoreManager";
            public const string EMAIL_MANAGER = "AccuIT_EmailManager";
            public const string ACTIVITY_MANAGER = "AccuIT_ActivityManager";
            public const string SECURITY_MANAGER = "AccuIT_SecurityManager";
            public const string Report_MANAGER = "AccuIT_ReportManager";
            public const string WEDDING_MANAGER = "AccuIT_WeddingManager";
        }

        /// <summary>
        /// 
        /// </summary>
        public struct IntegrationComponentInstanceNames
        {
            public const string NOTIFICATION_MANAGER = "AccuIT_AndroiddNotificationManager";
        }
    }
}
