using System;
using Unity;
using Unity.Lifetime;
using Unity.Injection;
using AccuIT.BusinessLayer.IC;
using AccuIT.BusinessLayer.ServiceImpl;
using AccuIT.BusinessLayer.Services.BO;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.CommonLayer.Aspects.Annotations;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.CommonLayer.Aspects.Exceptions;
using AccuIT.CommonLayer.Aspects.Logging;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.CommonLayer.EntityMapper;
using AccuIT.PersistenceLayer.Data.Impl;
using AccuIT.PersistenceLayer.Repository.Contracts;
using AccuIT.PersistenceLayer.Repository.Entities;
using AccuIT.BusinessLayer.IC.Contracts;
using AccuIT.CommonLayer.Aspects.ReportBO;
using Unity.Interception;
using Unity.Interception.Interceptors.InstanceInterceptors.TransparentProxyInterception;

namespace AccuIT.CommonLayer.AopRegistrations
{
    /// <summary>
    /// Class to register the objects for unity container
    /// </summary>
    public static class UnityRegistration
    {
        public const string DATE_FORMAT = "MM/dd/yyyy";
        public const string DATETIME_FORMAT = "dd-MMM-yyyy HH:mm:ss";
        public const string DATETIMELONG_FORMAT = "dd-MMM-yyyy hh:mm:ss.fff";

        /// <summary>
        /// Method to initialize the AOP Container of application
        /// </summary>
        public static void InitializeAopContainer()
        {
            AccuIT.CommonLayer.AopContainer.AopEngine.Initialize();
            InitializeLibrary();
            AopEngine.Container.RegisterType<IMapper, Mapper>(new InjectionMember[] { });
            MapEntities();
            InitializeTransactionInterceptor();
            ExceptionEngine.InitializeExceptionAopFramework();
        }

        #region Register Methods

        /// <summary>
        /// Overload method to register the mapping of instances in IoC Container
        /// </summary>
        /// <typeparam name="T">Generic Type of object to map</typeparam>
        /// <typeparam name="U">Generic Type of object from which to map</typeparam>
        /// <param name="instanceName">instance name of container</param>
        private static void RegisterType<T, U>(string instanceName)
        {
            AopEngine.Container.RegisterType(typeof(T), typeof(U), instanceName, new ContainerControlledLifetimeManager() { }, new InjectionMember[] { });
        }

        /// <summary>
        /// Method to get container instance name on basis of aspect name and application name
        /// </summary>
        /// <param name="aspectName">aspect name</param>
        /// <param name="application">application name</param>
        /// <returns>returns registration instance name</returns>
        private static string GetPersistenceRegisterInstanceName(AspectEnums.PeristenceInstanceNames aspectName, AspectEnums.ApplicationName application)
        {
            return String.Format("{0}_{1}", application.ToString(), aspectName.ToString());
        }

        /// <summary>
        /// Method to get container instance name on basis of aspect name and application name
        /// </summary>
        /// <param name="aspectName">aspect name</param>
        /// <param name="application">application name</param>
        /// <returns>returns registration instance name</returns>
        private static string GetICRegisterInstanceName(AspectEnums.IntCompInstanceNames aspectName, AspectEnums.ApplicationName application)
        {
            return String.Format("{0}_{1}", application.ToString(), aspectName.ToString());
        }

        /// <summary>
        /// Method to get container instance name on basis of aspect name and application name
        /// </summary>
        /// <param name="aspectName">aspect name</param>
        /// <param name="application">application name</param>
        /// <returns>returns registration instance name</returns>
        private static string GetBusinessRegisterInstanceName(AspectEnums.AspectInstanceNames aspectName, AspectEnums.ApplicationName application)
        {
            return String.Format("{0}_{1}", application.ToString(), aspectName.ToString());
        }

        #endregion

        #region Initialize Accuit Library

        /// <summary>
        /// Method to initialize the IoC container for Data Path Persistence layer objects and business layer objects
        /// </summary>
        private static void InitializeLibrary()
        {
            InitializeLibraryPersistenceLayer();
            InitializeLibraryBusinessLayer();
            InitializeIntegrationComponents();
        }

        /// <summary>
        /// Method to initialize the IoC container for Integration component instances
        /// </summary>
        private static void InitializeIntegrationComponents()
        {
            AopEngine.Container.RegisterType<INotification, AndroiddNotificationManager>(GetICRegisterInstanceName(AspectEnums.IntCompInstanceNames.AndroiddNotificationManager, AspectEnums.ApplicationName.AccuIT));
            AopEngine.Container.RegisterType<IOrderBooking, OrderManager>(GetICRegisterInstanceName(AspectEnums.IntCompInstanceNames.OrderManager, AspectEnums.ApplicationName.AccuIT));
        }

        /// <summary>
        /// Method to initialize the IoC container for Persistence layer objects
        /// </summary>
        private static void InitializeLibraryPersistenceLayer()
        {
            AopEngine.Container.RegisterType<IUserRepository, UserDataImpl>(GetPersistenceRegisterInstanceName(AspectEnums.PeristenceInstanceNames.EmpDataImpl, AspectEnums.ApplicationName.AccuIT));
            AopEngine.Container.RegisterType<ISystemRepository, SystemDataImpl>(GetPersistenceRegisterInstanceName(AspectEnums.PeristenceInstanceNames.SystemDataImpl, AspectEnums.ApplicationName.AccuIT));
            AopEngine.Container.RegisterType<ISecurityRepository, SecurityDataImpl>(GetPersistenceRegisterInstanceName(AspectEnums.PeristenceInstanceNames.SecurityDataImpl, AspectEnums.ApplicationName.AccuIT));
            AopEngine.Container.RegisterType<IEmailRepository, EmailDataImpl>(GetPersistenceRegisterInstanceName(AspectEnums.PeristenceInstanceNames.EmailDataImpl, AspectEnums.ApplicationName.AccuIT));
            AopEngine.Container.RegisterType<IWeddingRepository, WeddingDataImpl>(GetPersistenceRegisterInstanceName(AspectEnums.PeristenceInstanceNames.WeddingDataImpl, AspectEnums.ApplicationName.AccuIT));

        }

        /// <summary>
        /// Method to initialize the IoC container for Business layer objects
        /// </summary>
        private static void InitializeLibraryBusinessLayer()
        {
            //  AopEngine.Container.RegisterType<IBatchService, BatchManager>(GetBusinessRegisterInstanceName(AspectEnums.AspectInstanceNames.BatchManager, AspectEnums.ApplicationName.Samsung));
            AopEngine.Container.RegisterType<IUserService, UserManager>(GetBusinessRegisterInstanceName(AspectEnums.AspectInstanceNames.UserManager, AspectEnums.ApplicationName.AccuIT));
            AopEngine.Container.RegisterType<ISystemService, ServiceManager>(GetBusinessRegisterInstanceName(AspectEnums.AspectInstanceNames.ServiceManager, AspectEnums.ApplicationName.AccuIT));
            AopEngine.Container.RegisterType<ISecurityService, SecurityManager>(GetBusinessRegisterInstanceName(AspectEnums.AspectInstanceNames.SecurityManager, AspectEnums.ApplicationName.AccuIT));
            AopEngine.Container.RegisterType<IEmailService, EmailManager>(GetBusinessRegisterInstanceName(AspectEnums.AspectInstanceNames.EmailManager, AspectEnums.ApplicationName.AccuIT));
            AopEngine.Container.RegisterType<IWeddingService, WeddingManager>(GetBusinessRegisterInstanceName(AspectEnums.AspectInstanceNames.WeddingManager, AspectEnums.ApplicationName.AccuIT));
        }

        #endregion

        #region Map Entities

        /// <summary>
        /// Method to register the mapping for auto mapper between the business objects, data objects and Data Transfer Objects
        /// </summary>
        private static void MapEntities()
        {
            AccuIT.CommonLayer.EntityMapper.Mapper mapper = AopEngine.Container.Resolve<EntityMapper.Mapper>();
            MapBOEntities(mapper);
            MapDTOEntities(mapper);
        }

        /// <summary>
        /// Method to register the mapping for auto mapper between the DataPath business objects with DataPath data objects
        /// </summary>
        /// <param name="mapper">EntityMapper object</param>
        private static void MapBOEntities(EntityMapper.Mapper mapper)
        {
            mapper.CreateMap<UserMaster, UserBO>();
            mapper.CreateMap<UserBO, UserMaster>();

            mapper.CreateMap<UserMaster, UserMasterBO>();
            mapper.CreateMap<UserMasterBO, UserMaster>();

            // mapper.CreateMap<UserMaster, UserMasterBO>();
            //mapper.CreateMap<UserMasterDTO, UserMaster>();

            mapper.CreateMap<UserMaster, UserProfileBO>();
            mapper.CreateMap<UserProfileBO, UserMaster>();

            mapper.CreateMap<DailyLoginHistory, DailyLoginHistoryBO>();
            mapper.CreateMap<DailyLoginHistoryBO, DailyLoginHistory>();

            //mapper.CreateMap<ProductMaster, ProductMasterBO>();
            //mapper.CreateMap<ProductMasterBO, ProductMaster>();

            //mapper.CreateMap<CategoryMaster, CategoryMasterBO>();
            //mapper.CreateMap<CategoryMasterBO, CategoryMaster>();

            //mapper.CreateMap<DailyEnquiry, DailyEnquiryBO>();
            //mapper.CreateMap<DailyEnquiryBO, DailyEnquiry>();

            //mapper.CreateMap<DailyEnquiry, QuotationViewDataBO>();
            //mapper.CreateMap<QuotationViewDataBO, DailyEnquiry>();

            //mapper.CreateMap<DailyEnquiryProd, DailyEnquiryProdBO>();
            //mapper.CreateMap<DailyEnquiryProdBO, DailyEnquiryProd>();

            mapper.CreateMap<UserRole, UserRoleBO>();
            mapper.CreateMap<UserRoleBO, UserRole>();

            mapper.CreateMap<RoleMaster, RoleMasterBO>();
            mapper.CreateMap<RoleMasterBO, UserRole>();

            mapper.CreateMap<DailyLoginHistoryBO, DailyLoginHistory>();
            mapper.CreateMap<DailyLoginHistory, DailyLoginHistoryBO>();

            mapper.CreateMap<TemplateMaster, TemplateMasterBO>();
            mapper.CreateMap<TemplateMasterBO, TemplateMaster>();

            mapper.CreateMap<TemplateMergeField, TemplateMergeFieldBO>();
            mapper.CreateMap<TemplateMergeFieldBO, TemplateMaster>();

            mapper.CreateMap<AddressMaster, AddressMasterBO>();
            mapper.CreateMap<AddressMasterBO, AddressMaster>();

            mapper.CreateMap<AddressMasterDTO, AddressMasterBO>();
            mapper.CreateMap<AddressMasterBO, AddressMasterDTO>();

            mapper.CreateMap<TemplateImage, TemplateImageBO>();
            mapper.CreateMap<TemplateImageBO, TemplateImage>();

            mapper.CreateMap<TemplatePage, TemplatePageBO>();
            mapper.CreateMap<TemplatePageBO, TemplatePage>();

            mapper.CreateMap<SecurityAspect, SecurityAspectBO>();
            mapper.CreateMap<SecurityAspectBO, SecurityAspect>();

            mapper.CreateMap<Wedding, WeddingBO>();
            mapper.CreateMap<WeddingBO, Wedding>();

            mapper.CreateMap<DreamWeddsBlog, DreamWeddsBlogBO>();
            mapper.CreateMap<DreamWeddsBlogBO, DreamWeddsBlog>();

            mapper.CreateMap<FAQ, FAQBO>();
            mapper.CreateMap<FAQBO, FAQ>();

            //AutoMapper.Mapper.CreateMap<Wedding, WeddingBO>()
            //    .ForMember(x => x.WeddingDate, opt => opt.MapFrom(y => y.WeddingDate == null ? "" : Convert.ToDateTime(y.WeddingDate).ToString("dd-MMM-yyyy")));

            //AutoMapper.Mapper.CreateMap<WeddingBO, Wedding>()
            //  .ForMember(x => x.WeddingDate, opt => opt.MapFrom(y => y.WeddingDate == null ? Convert.ToDateTime("") : Convert.ToDateTime(y.WeddingDate)));

            mapper.CreateMap<WeddingEventBO, WeddingEvent>();

            AutoMapper.Mapper.CreateMap<WeddingEvent, WeddingEventBO>()
            .ForMember(x => x.strStartTime, opt => opt.MapFrom(y => y.StartTime == null ? "" : y.StartTime.ToShortTimeString()))
            .ForMember(x => x.strEndTime, opt => opt.MapFrom(y => y.EndTime == null ? "" : y.EndTime.ToShortTimeString()))
            .ForMember(x => x.strEventDate, opt => opt.MapFrom(y => y.EventDate == null ? DateTime.Now.ToString(DATE_FORMAT) : y.EventDate.ToString(DATE_FORMAT)));


            mapper.CreateMap<WeddingGallery, WeddingGalleryBO>();
            mapper.CreateMap<WeddingGalleryBO, WeddingGallery>();


            AutoMapper.Mapper.CreateMap<WeddingGalleryDTO, WeddingGalleryBO>()
            .ForMember(x => x.DateTaken, opt => opt.MapFrom(y => y.DateTaken == "" ? DateTime.Now : Convert.ToDateTime(y.DateTaken)));

            AutoMapper.Mapper.CreateMap<WeddingGalleryBO, WeddingGalleryDTO>()
               .ForMember(x => x.DateTaken, opt => opt.MapFrom(y => y.DateTaken == null ? "" : Convert.ToDateTime(y.DateTaken).ToString(DATE_FORMAT)));

            mapper.CreateMap<WeddingGalleryDTO, WeddingGalleryBO>();
            mapper.CreateMap<WeddingGalleryBO, WeddingGalleryDTO>();


            mapper.CreateMap<Venue, VenueBO>();
            mapper.CreateMap<VenueBO, Venue>();

            AutoMapper.Mapper.CreateMap<AddressMaster, VenueBO>()
                .ForMember(x => x.VenueAddress, opt => opt.MapFrom(y => y));



            AutoMapper.Mapper.CreateMap<TimeLine, TimeLineBO>()
                .ForMember(x => x.strStoryDate, opt => opt.MapFrom(y => y.StoryDate == null ? "" : Convert.ToDateTime(y.StoryDate).ToString(DATE_FORMAT)));
            mapper.CreateMap<TimeLineBO, TimeLine>();

            mapper.CreateMap<vwGetUserWeddingTemplate, UserWeddingTemplateSubscriptionsBO>();
            mapper.CreateMap<UserWeddingTemplateSubscriptionsBO, vwGetUserWeddingTemplate>();

            mapper.CreateMap<UserWeddingTemplateSubscriptionsDTO, UserWeddingTemplateSubscriptionsBO>();
            mapper.CreateMap<UserWeddingTemplateSubscriptionsBO, UserWeddingTemplateSubscriptionsDTO>();

            mapper.CreateMap<UserDashboardDTO, UserWeddingTemplateSubscriptionsBO>();
            mapper.CreateMap<UserWeddingTemplateSubscriptionsBO, UserDashboardDTO>();

            mapper.CreateMap<vwGetUserWeddingTemplate, UserWeddingSubscriptionBO>();
            mapper.CreateMap<UserWeddingSubscriptionBO, vwGetUserWeddingTemplate>();

            mapper.CreateMap<UserWeddingSubscription, UserWeddingSubscriptionBO>();
            mapper.CreateMap<UserWeddingSubscriptionBO, UserWeddingSubscription>();

            mapper.CreateMap<UserWeddingSubscriptionDTO, UserWeddingSubscriptionBO>();
            mapper.CreateMap<UserWeddingSubscriptionBO, UserWeddingSubscriptionDTO>();

            // Wedding DTOs

            mapper.CreateMap<TemplateMasterDTO, TemplateMasterBO>();
            mapper.CreateMap<TemplateMasterBO, TemplateMasterDTO>();

            AutoMapper.Mapper.CreateMap<WeddingBO, WeddingDTO>()
                .ForMember(x => x.WeddingDate, opt => opt.MapFrom(y => y.WeddingDate == null ? DateTime.Now.ToString(DATE_FORMAT) : y.WeddingDate.ToString(DATE_FORMAT)))
                .ForMember(x => x.SubscriptionEndDate, opt => opt.MapFrom(y =>  y.SubscriptionEndDate.ToString(DATE_FORMAT)))
                .ForMember(x => x.WeddingStyle, opt => opt.MapFrom(y => Convert.ToString((AspectEnums.WeddingStyle)y.WeddingStyle)));

            AutoMapper.Mapper.CreateMap<WeddingDTO, WeddingBO>()
                 .ForMember(x => x.WeddingDate, opt => opt.MapFrom(y => y.WeddingDate == null ? DateTime.Now : Convert.ToDateTime(y.WeddingDate)))
                .ForMember(x => x.WeddingStyle, opt => opt.MapFrom(y => (AspectEnums.WeddingStyle)Enum.Parse(typeof(AspectEnums.WeddingStyle), y.WeddingStyle)));


            AutoMapper.Mapper.CreateMap<UserWeddingSubscriptionDTO, UserWeddingSubscriptionBO>()
                .ForMember(x => x.StartDate, opt => opt.MapFrom(y => y.StartDate == null ? DateTime.Now : Convert.ToDateTime(y.StartDate)))
                .ForMember(x => x.EndDate, opt => opt.MapFrom(y => y.EndDate == null ? DateTime.Now : Convert.ToDateTime(y.EndDate)))
                 // .ForMember(x => x.UpdatedDate, opt => opt.MapFrom(y => y.UpdatedDate == "" ? DateTime.Now : Convert.ToDateTime(y.UpdatedDate)))
                 .ForMember(x => x.SubscriptionStatus, opt => opt.MapFrom(y => (int)(AspectEnums.SubscriptionStatus)Enum.Parse(typeof(AspectEnums.SubscriptionStatus), y.SubscriptionStatus)))
               .ForMember(x => x.SubscriptionType, opt => opt.MapFrom(y => (int)(AspectEnums.SubscriptionType)Enum.Parse(typeof(AspectEnums.SubscriptionType), y.SubscriptionType)));

            AutoMapper.Mapper.CreateMap<UserWeddingSubscriptionBO, UserWeddingSubscriptionDTO>()
                .ForMember(x => x.StartDate, opt => opt.MapFrom(y => y.StartDate == null ? "" : y.StartDate.ToString(DATE_FORMAT)))
                .ForMember(x => x.EndDate, opt => opt.MapFrom(y => y.EndDate == null ? "" : y.EndDate.ToString(DATE_FORMAT)))
              // .ForMember(x => x.UpdatedDate, opt => opt.MapFrom(y => y.UpdatedDate == null ? "" : y.UpdatedDate.ToString()))
              .ForMember(x => x.SubscriptionStatus, opt => opt.MapFrom(y => Convert.ToString((AspectEnums.SubscriptionStatus)Convert.ToInt32(y.SubscriptionStatus))))
              .ForMember(x => x.SubscriptionType, opt => opt.MapFrom(y => Convert.ToString((AspectEnums.SubscriptionType)Convert.ToInt32(y.SubscriptionType))));


            AutoMapper.Mapper.CreateMap<WeddingEventBO, WeddingEventDTO>()
                .ForMember(x => x.StartTime, opt => opt.MapFrom(y => y.StartTime == null ? "" : y.StartTime.ToShortTimeString()))
            .ForMember(x => x.EndTime, opt => opt.MapFrom(y => y.StartTime == null ? "" : y.EndTime.ToShortTimeString()))
            .ForMember(x => x.EventDate, opt => opt.MapFrom(y => y.EventDate == null ? DateTime.Now.ToString(DATE_FORMAT) : y.EventDate.ToString(DATE_FORMAT)));

            AutoMapper.Mapper.CreateMap<WeddingEventDTO, WeddingEventBO>()
                .ForMember(x => x.StartTime, opt => opt.MapFrom(y => y.EventDate == null ? DateTime.Now : Convert.ToDateTime(y.StartTime)))
                .ForMember(x => x.EndTime, opt => opt.MapFrom(y => y.EventDate == null ? DateTime.Now : Convert.ToDateTime(y.EndTime)))
                .ForMember(x => x.EventDate, opt => opt.MapFrom(y => y.EventDate == null ? DateTime.Now : Convert.ToDateTime(y.EventDate)));


            AutoMapper.Mapper.CreateMap<EventAndVenueDTO, WeddingEventBO>()
             .ForMember(x => x.StartTime, opt => opt.MapFrom(y => y.EventDate == null ? DateTime.Now : Convert.ToDateTime(y.StartTime)))
             .ForMember(x => x.EndTime, opt => opt.MapFrom(y => y.EventDate == null ? DateTime.Now : Convert.ToDateTime(y.EndTime)))
                  .ForMember(x => x.EventDate, opt => opt.MapFrom(y => y.EventDate == null ? DateTime.Now : DateTime.ParseExact(y.EventDate, "yyyy-MM-ddTHH:mm:ss.000Z", System.Globalization.CultureInfo.InvariantCulture)));

            mapper.CreateMap<EventAndVenueDTO, WeddingEventBO>();

            AutoMapper.Mapper.CreateMap<EventAndVenueDTO, VenueBO>()
                 .ForMember(x => x.Name, opt => opt.MapFrom(y => y.Name));

            mapper.CreateMap<EventAndVenueDTO, AddressMasterBO>();


            mapper.CreateMap<VenueDTO, VenueBO>();
            AutoMapper.Mapper.CreateMap<VenueBO, VenueDTO>()
                .ForMember(x => x.VenueAddress, opt => opt.MapFrom(y => y.VenueAddress));

            AutoMapper.Mapper.CreateMap<AddressMasterBO, VenueDTO>()
                .ForMember(x => x.VenueAddress, opt => opt.MapFrom(y => y));


            AutoMapper.Mapper.CreateMap<BrideAndMaid, BrideAndMaidBO>()
                  .ForMember(x => x.strDateofBirth, opt => opt.MapFrom(y => y.DateofBirth == null ? "" : Convert.ToDateTime(y.DateofBirth).ToString(DATE_FORMAT)));
            mapper.CreateMap<BrideAndMaidBO, BrideAndMaid>();

            AutoMapper.Mapper.CreateMap<BrideAndMaidDTO, BrideAndMaidBO>()
             .ForMember(x => x.DateofBirth, opt => opt.MapFrom(y => Convert.ToDateTime(y.DateofBirth)));

            AutoMapper.Mapper.CreateMap<BrideAndMaidBO, BrideAndMaidDTO>()
               .ForMember(x => x.DateofBirth, opt => opt.MapFrom(y => y.DateofBirth == null ? "" : Convert.ToDateTime(y.DateofBirth).ToString(DATE_FORMAT)))
               .ForMember(x => x.RelationWithBride, opt => opt.MapFrom(y => y.RelationWithBride));


            AutoMapper.Mapper.CreateMap<GroomAndMan, GroomAndManBO>()
                .ForMember(x => x.strDateofBirth, opt => opt.MapFrom(y => y.DateofBirth == null ? "" : Convert.ToDateTime(y.DateofBirth).ToString(DATE_FORMAT)));

            mapper.CreateMap<GroomAndManBO, GroomAndMan>();

            AutoMapper.Mapper.CreateMap<GroomAndMenDTO, GroomAndManBO>()
               .ForMember(x => x.DateofBirth, opt => opt.MapFrom(y => Convert.ToDateTime(y.DateofBirth)));

            AutoMapper.Mapper.CreateMap<GroomAndManBO, GroomAndMenDTO>()
              .ForMember(x => x.DateofBirth, opt => opt.MapFrom(y => y.DateofBirth == null ? "" : Convert.ToDateTime(y.DateofBirth).ToString(DATE_FORMAT)))
             .ForMember(x => x.RelationWithGroom, opt => opt.MapFrom(y => y.RelationWithGroom));



            AutoMapper.Mapper.CreateMap<TimeLineDTO, TimeLineBO>()
               .ForMember(x => x.StoryDate, opt => opt.MapFrom(y => Convert.ToDateTime(y.StoryDate)));

            AutoMapper.Mapper.CreateMap<TimeLineBO, TimeLineDTO>()
    .ForMember(x => x.StoryDate, opt => opt.MapFrom(y => y.StoryDate == null ? "" : Convert.ToDateTime(y.StoryDate).ToString(DATE_FORMAT)));



            mapper.CreateMap<AddressMasterDTO, AddressMasterBO>();
            mapper.CreateMap<AddressMasterBO, AddressMasterDTO>();

            mapper.CreateMap<CommonSetup, CommonSetupDTO>();
            mapper.CreateMap<CommonSetupDTO, CommonSetup>();

            mapper.CreateMap<OrderDetailBO, OrderDetail>();
            mapper.CreateMap<OrderDetail, OrderDetailBO>();

            mapper.CreateMap<OrderDetailBO, OrderDetailDTO>();
            mapper.CreateMap<OrderDetailDTO, OrderDetailBO>();

            mapper.CreateMap<OrderMasterBO, OrderMaster>();
            mapper.CreateMap<OrderMaster, OrderMasterBO>();


            AutoMapper.Mapper.CreateMap<OrderMasterDTO, OrderMasterBO>()
                .ForMember(x => x.OrderDate, opt => opt.MapFrom(y => y.OrderDate == null ? DateTime.Now : Convert.ToDateTime(y.OrderDate)))
                .ForMember(x => x.RequiredDate, opt => opt.MapFrom(y => y.RequiredDate == "" ? DateTime.Now : Convert.ToDateTime(y.RequiredDate)));
            // .ForMember(x => x.RequiredDate, opt => opt.MapFrom(y => (int)(AspectEnums.SubscriptionStatus)Enum.Parse(typeof(AspectEnums.SubscriptionStatus), y.SubscriptionStatus)))
            //  .ForMember(x => x.SubscriptionType, opt => opt.MapFrom(y => (int)(AspectEnums.SubscriptionType)Enum.Parse(typeof(AspectEnums.SubscriptionType), y.SubscriptionType)));

            AutoMapper.Mapper.CreateMap<OrderMasterBO, OrderMasterDTO>()
                .ForMember(x => x.OrderDate, opt => opt.MapFrom(y => y.OrderDate == null ? "" : y.OrderDate.ToString(DATE_FORMAT)))
                .ForMember(x => x.RequiredDate, opt => opt.MapFrom(y => y.RequiredDate == null ? "" : Convert.ToDateTime(y.RequiredDate).ToString(DATE_FORMAT)));
            // .ForMember(x => x.UpdatedDate, opt => opt.MapFrom(y => y.UpdatedDate == null ? "" : y.UpdatedDate.ToString()))
            // .ForMember(x => x.SubscriptionStatus, opt => opt.MapFrom(y => Convert.ToString((AspectEnums.SubscriptionStatus)Convert.ToInt32(y.SubscriptionStatus))))
            //.ForMember(x => x.SubscriptionType, opt => opt.MapFrom(y => Convert.ToString((AspectEnums.SubscriptionType)Convert.ToInt32(y.SubscriptionType))));




            //   #region Methods Added for Role Master :Dhiraj 3-Dec-2013
            mapper.CreateMap<RoleMaster, RoleMasterBO>();
            mapper.CreateMap<RoleMasterBO, RoleMaster>();
            mapper.CreateMap<RoleModule, RoleModuleBO>();
            mapper.CreateMap<RoleModuleBO, RoleModule>();
            mapper.CreateMap<PermissionBO, PermissionBO>();
            mapper.CreateMap<UserRoleModulePermissionBO, UserRoleModulePermission>();
            mapper.CreateMap<UserRoleModulePermission, UserRoleModulePermissionBO>();
            mapper.CreateMap<ModuleMasterBO, ModuleMaster>();
            mapper.CreateMap<ModuleMaster, ModuleMasterBO>();

            mapper.CreateMap<CommonSetup, CommonSetupBO>();
            mapper.CreateMap<CommonSetupBO, CommonSetup>();


        }

        /// <summary>
        /// Method to map DTO Data Transfer Objects entities with BO entities
        /// </summary>
        /// <param name="mapper">EntityMapper</param>
        private static void MapDTOEntities(EntityMapper.Mapper mapper)
        {
            mapper.CreateMap<OTPMaster, OTPBO>();
            mapper.CreateMap<OTPBO, OTPMaster>();

            mapper.CreateMap<EmailService, EmailServiceBO>();
            mapper.CreateMap<EmailServiceBO, EmailService>();

            mapper.CreateMap<EmailService, EmailServiceDTO>();
            mapper.CreateMap<EmailServiceDTO, EmailService>();

            //mapper.CreateMap<SMTPServer, SMTPServerDTO>();
            //mapper.CreateMap<SMTPServerDTO, SMTPServer>();


            //mapper.CreateMap<UserBO, UserDTO>();
            //mapper.CreateMap<UserDTO, UserBO>();

            //mapper.CreateMap<PartnerMeetingDTO, PartnerMeeting>();
            //mapper.CreateMap<PartnerMeeting, PartnerMeetingDTO>();

            //mapper.CreateMap<CompProductGroupDTO, CompProductGroup>();
            //mapper.CreateMap<CompProductGroup, CompProductGroupDTO>();

            mapper.CreateMap<SystemSettingBO, SystemSettingsDTO>();
            mapper.CreateMap<SystemSettingsDTO, SystemSettingBO>();

            mapper.CreateMap<ServiceResponseBO, ServiceResponseDTO>();
            mapper.CreateMap<ServiceResponseDTO, ServiceResponseBO>();

            mapper.CreateMap<ForgotPasswordBO, ForgotPasswordDTO>();
            mapper.CreateMap<ForgotPasswordDTO, ForgotPasswordBO>();

            //   AutoMapper.Mapper.CreateMap<SRRequest, SRRequestDTO>().ForMember(x => x.CreatedByUserName, opt => opt.MapFrom(y => y.UserMaster.FirstName))
            //      .ForMember(x => x.CreatedDate, opt => opt.MapFrom(y => y.CreatedDate))
            //      .ForMember(x => x.CreatedByUserName, opt => opt.MapFrom(y => y.UserMaster.FirstName))
            //      .ForMember(x => x.ModifiedDate, opt => opt.MapFrom(y => y.ModifiedDate == null ? "" : y.ModifiedDate.Value.ToString()))
            //      .ForMember(x => x.ModifiedByUserName, opt => opt.MapFrom(y => y.ModifiedBy == null ? "" : y.UserMaster1.FirstName))
            //      .ForMember(x => x.PendingWithUserName, opt => opt.MapFrom(y => y.UserMaster2.FirstName));


            AutoMapper.Mapper.CreateMap<UserProfileBO, UserProfileDTO>()
                .ForMember(x => x.JoiningDate, opt => opt.MapFrom(y => y.JoiningDate == null ? "" : y.JoiningDate.Value.ToString(DATE_FORMAT)));
            //.ForMember(x => x.PurchaseDate, opt => opt.MapFrom(y => y.PurchaseDate == null ? "" : y.PurchaseDate.Value.ToString(DATE_FORMAT)));

            AutoMapper.Mapper.CreateMap<UserProfileDTO, UserProfileBO>()
                .ForMember(x => x.JoiningDate, opt => opt.MapFrom(y => y.JoiningDate == null ? DateTime.Now : Convert.ToDateTime(y.JoiningDate)));
            //.ForMember(x => x.PurchaseDate, opt => opt.MapFrom(y => y.PurchaseDate == null ? DateTime.Now : Convert.ToDateTime(y.PurchaseDate)));


            mapper.CreateMap<UserMaster, UserProfileDTO>();
            mapper.CreateMap<UserProfileDTO, UserMaster>();


            mapper.CreateMap<ErrorLogDTO, ErrorLog>();
            mapper.CreateMap<ErrorLog, ErrorLogDTO>();

            // mapper.CreateMap<UserModuleDTO, vwUserRoleModule>();
            //  mapper.CreateMap<vwUserRoleModule, UserModuleDTO>();

            mapper.CreateMap<UserModuleDTO, vwGetUserRoleModule>();
            mapper.CreateMap<vwGetUserRoleModule, UserModuleDTO>();

            mapper.CreateMap<UserSystemSetting, UserSystemSettingDTO>();
            mapper.CreateMap<UserSystemSettingDTO, UserSystemSetting>();


            mapper.CreateMap<CommonSetup, CommonSetupDTO>();
            mapper.CreateMap<CommonSetupDTO, CommonSetup>();

            mapper.CreateMap<SPGetLoginDetails_Result, UserLoginDetailsBO>();
            mapper.CreateMap<UserLoginDetailsBO, SPGetLoginDetails_Result>();

            mapper.CreateMap<UserLoginDetailDTO, UserLoginDetailsBO>();
            mapper.CreateMap<UserLoginDetailsBO, UserLoginDetailDTO>();

            mapper.CreateMap<SPCheckAuthentication_Result, CheckUserAuthenticationBO>();
            mapper.CreateMap<CheckUserAuthenticationBO, SPCheckAuthentication_Result>();

            mapper.CreateMap<CheckUserAuthenticationDTO, CheckUserAuthenticationBO>();
            mapper.CreateMap<CheckUserAuthenticationBO, CheckUserAuthenticationDTO>();


        }

        #endregion

        #region Initialize Transaction Interceptor

        /// <summary>
        /// This method is used to configure the assembly services for the transaction handling scope
        /// </summary>
        private static void InitializeTransactionInterceptor()
        {
            //add extension for interception in unity container
            AopEngine.Container.AddNewExtension<Interception>();

            //Define the interface name to deal with transaction rule execution
            AopEngine.Container.Configure<Interception>().SetInterceptorFor<IUserService>(new TransparentProxyInterceptor());

            AopEngine.Container.AddNewExtension<Interception>();
           //  AopEngine.Container.Configure<Interception>().AddPolicy("Logger").AddMatchingRule(new LoggerRule()).AddCallHandler(new LogWriteManager());
        }

        #endregion



    }
}

