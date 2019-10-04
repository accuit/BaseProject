namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;

    public partial class AccuITAdminEntities : DbContext
    {
        public AccuITAdminEntities()
            : base("name=AccuITAdminEntities")
        {
        }

        public virtual DbSet<AddressMaster> AddressMasters { get; set; }
        public virtual DbSet<BrideAndMaid> BrideAndMaids { get; set; }
        public virtual DbSet<CommonSetup> CommonSetups { get; set; }
        public virtual DbSet<CompanyMaster> CompanyMasters { get; set; }
        public virtual DbSet<DailyLoginHistory> DailyLoginHistories { get; set; }
        public virtual DbSet<DreamWeddsBlog> DreamWeddsBlogs { get; set; }
        public virtual DbSet<ECampaign> ECampaigns { get; set; }
        public virtual DbSet<EmailService> EmailServices { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<FAQ> FAQs { get; set; }
        public virtual DbSet<GroomAndMan> GroomAndMen { get; set; }
        public virtual DbSet<LoginAttemptHistory> LoginAttemptHistories { get; set; }
        public virtual DbSet<ModuleMaster> ModuleMasters { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderMaster> OrderMasters { get; set; }
        public virtual DbSet<OTPMaster> OTPMasters { get; set; }
        public virtual DbSet<Permission> Permissions { get; set; }
        public virtual DbSet<RoleMaster> RoleMasters { get; set; }
        public virtual DbSet<RoleModule> RoleModules { get; set; }
        public virtual DbSet<RSVPDetail> RSVPDetails { get; set; }
        public virtual DbSet<SubscriptionMaster> SubscriptionMasters { get; set; }
        public virtual DbSet<SystemSetting> SystemSettings { get; set; }
        public virtual DbSet<TemplateImage> TemplateImages { get; set; }
        public virtual DbSet<TemplateMaster> TemplateMasters { get; set; }
        public virtual DbSet<TemplateMergeField> TemplateMergeFields { get; set; }
        public virtual DbSet<TemplatePage> TemplatePages { get; set; }
        public virtual DbSet<TimeLine> TimeLines { get; set; }
        public virtual DbSet<UserDevice> UserDevices { get; set; }
        public virtual DbSet<UserMaster> UserMasters { get; set; }
        public virtual DbSet<UserRoleModulePermission> UserRoleModulePermissions { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<UserServiceAccess> UserServiceAccesses { get; set; }
        public virtual DbSet<UserSystemSetting> UserSystemSettings { get; set; }
        public virtual DbSet<UserWeddingSubscription> UserWeddingSubscriptions { get; set; }
        public virtual DbSet<Venue> Venues { get; set; }
        public virtual DbSet<WeddingEvent> WeddingEvents { get; set; }
        public virtual DbSet<WeddingGallery> WeddingGalleries { get; set; }
        public virtual DbSet<Wedding> Weddings { get; set; }
        public virtual DbSet<vwGetUserRoleModule> vwGetUserRoleModules { get; set; }
        public virtual DbSet<vwGetUserWeddingTemplate> vwGetUserWeddingTemplates { get; set; }

        public virtual ObjectResult<SPCheckAuthentication_Result> SPCheckAuthentication(string loginName, string password, string iMEINumber, string lattitude, string longitude, string ipAddress, string browserName, string apkDeviceName, string aPKVersion, Nullable<byte> loginType)
        {
            var loginNameParameter = loginName != null ?
                new ObjectParameter("LoginName", loginName) :
                new ObjectParameter("LoginName", typeof(string));

            var passwordParameter = password != null ?
                new ObjectParameter("Password", password) :
                new ObjectParameter("Password", typeof(string));

            var iMEINumberParameter = iMEINumber != null ?
                new ObjectParameter("IMEINumber", iMEINumber) :
                new ObjectParameter("IMEINumber", typeof(string));

            var lattitudeParameter = lattitude != null ?
                new ObjectParameter("lattitude", lattitude) :
                new ObjectParameter("lattitude", typeof(string));

            var longitudeParameter = longitude != null ?
                new ObjectParameter("longitude", longitude) :
                new ObjectParameter("longitude", typeof(string));

            var ipAddressParameter = ipAddress != null ?
                new ObjectParameter("IpAddress", ipAddress) :
                new ObjectParameter("IpAddress", typeof(string));

            var browserNameParameter = browserName != null ?
                new ObjectParameter("BrowserName", browserName) :
                new ObjectParameter("BrowserName", typeof(string));

            var apkDeviceNameParameter = apkDeviceName != null ?
                new ObjectParameter("ApkDeviceName", apkDeviceName) :
                new ObjectParameter("ApkDeviceName", typeof(string));

            var aPKVersionParameter = aPKVersion != null ?
                new ObjectParameter("APKVersion", aPKVersion) :
                new ObjectParameter("APKVersion", typeof(string));

            var loginTypeParameter = loginType.HasValue ?
                new ObjectParameter("LoginType", loginType) :
                new ObjectParameter("LoginType", typeof(byte));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SPCheckAuthentication_Result>("SPCheckAuthentication", loginNameParameter, passwordParameter, iMEINumberParameter, lattitudeParameter, longitudeParameter, ipAddressParameter, browserNameParameter, apkDeviceNameParameter, aPKVersionParameter, loginTypeParameter);
        }

        public virtual ObjectResult<SPGetLoginDetails_Result> SPGetLoginDetails(Nullable<int> userID, Nullable<bool> showAnnouncment, string aPIKey, string aPIToken, string aPKVersion)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("userID", userID) :
                new ObjectParameter("userID", typeof(int));

            var showAnnouncmentParameter = showAnnouncment.HasValue ?
                new ObjectParameter("showAnnouncment", showAnnouncment) :
                new ObjectParameter("showAnnouncment", typeof(bool));

            var aPIKeyParameter = aPIKey != null ?
                new ObjectParameter("APIKey", aPIKey) :
                new ObjectParameter("APIKey", typeof(string));

            var aPITokenParameter = aPIToken != null ?
                new ObjectParameter("APIToken", aPIToken) :
                new ObjectParameter("APIToken", typeof(string));

            var aPKVersionParameter = aPKVersion != null ?
                new ObjectParameter("APKVersion", aPKVersion) :
                new ObjectParameter("APKVersion", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SPGetLoginDetails_Result>("SPGetLoginDetails", userIDParameter, showAnnouncmentParameter, aPIKeyParameter, aPITokenParameter, aPKVersionParameter);
        }

        public virtual int SPGetUserLoginDetails(Nullable<int> userID, Nullable<bool> showAnnouncment, string aPIKey, string aPIToken, string aPKVersion)
        {
            var userIDParameter = userID.HasValue ?
                new ObjectParameter("userID", userID) :
                new ObjectParameter("userID", typeof(int));

            var showAnnouncmentParameter = showAnnouncment.HasValue ?
                new ObjectParameter("showAnnouncment", showAnnouncment) :
                new ObjectParameter("showAnnouncment", typeof(bool));

            var aPIKeyParameter = aPIKey != null ?
                new ObjectParameter("APIKey", aPIKey) :
                new ObjectParameter("APIKey", typeof(string));

            var aPITokenParameter = aPIToken != null ?
                new ObjectParameter("APIToken", aPIToken) :
                new ObjectParameter("APIToken", typeof(string));

            var aPKVersionParameter = aPKVersion != null ?
                new ObjectParameter("APKVersion", aPKVersion) :
                new ObjectParameter("APKVersion", typeof(string));

            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SPGetUserLoginDetails", userIDParameter, showAnnouncmentParameter, aPIKeyParameter, aPITokenParameter, aPKVersionParameter);
        }

        public virtual int SpClearLogs()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SpClearLogs");
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressMaster>()
                .Property(e => e.Address1)
                .IsUnicode(false);

            modelBuilder.Entity<AddressMaster>()
                .Property(e => e.Address2)
                .IsUnicode(false);

            modelBuilder.Entity<AddressMaster>()
                .Property(e => e.Lattitude)
                .IsUnicode(false);

            modelBuilder.Entity<AddressMaster>()
                .Property(e => e.Longitude)
                .IsUnicode(false);

            modelBuilder.Entity<BrideAndMaid>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<BrideAndMaid>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<BrideAndMaid>()
                .Property(e => e.Imageurl)
                .IsUnicode(false);

            modelBuilder.Entity<BrideAndMaid>()
                .Property(e => e.AboutBrideMaid)
                .IsUnicode(false);

            modelBuilder.Entity<BrideAndMaid>()
                .Property(e => e.fbUrl)
                .IsUnicode(false);

            modelBuilder.Entity<BrideAndMaid>()
                .Property(e => e.googleUrl)
                .IsUnicode(false);

            modelBuilder.Entity<BrideAndMaid>()
                .Property(e => e.instagramUrl)
                .IsUnicode(false);

            modelBuilder.Entity<BrideAndMaid>()
                .Property(e => e.lnkedinUrl)
                .IsUnicode(false);

            modelBuilder.Entity<CommonSetup>()
                .Property(e => e.MainType)
                .IsUnicode(false);

            modelBuilder.Entity<CommonSetup>()
                .Property(e => e.SubType)
                .IsUnicode(false);

            modelBuilder.Entity<CommonSetup>()
                .Property(e => e.DisplayText)
                .IsUnicode(false);

            modelBuilder.Entity<CompanyMaster>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<CompanyMaster>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<CompanyMaster>()
                .HasMany(e => e.SystemSettings)
                .WithRequired(e => e.CompanyMaster)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CompanyMaster>()
                .HasMany(e => e.UserMasters)
                .WithRequired(e => e.CompanyMaster)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<DailyLoginHistory>()
                .Property(e => e.SessionID)
                .IsUnicode(false);

            modelBuilder.Entity<DailyLoginHistory>()
                .Property(e => e.IpAddress)
                .IsUnicode(false);

            modelBuilder.Entity<DailyLoginHistory>()
                .Property(e => e.BrowserName)
                .IsUnicode(false);

            modelBuilder.Entity<DailyLoginHistory>()
                .Property(e => e.ApkDeviceName)
                .IsUnicode(false);

            modelBuilder.Entity<DailyLoginHistory>()
                .Property(e => e.APKVersion)
                .IsUnicode(false);

            modelBuilder.Entity<DailyLoginHistory>()
                .Property(e => e.Lattitude)
                .IsUnicode(false);

            modelBuilder.Entity<DailyLoginHistory>()
                .Property(e => e.Longitude)
                .IsUnicode(false);

            modelBuilder.Entity<DreamWeddsBlog>()
                .Property(e => e.BlogName)
                .IsUnicode(false);

            modelBuilder.Entity<DreamWeddsBlog>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<DreamWeddsBlog>()
                .Property(e => e.BlogSubject)
                .IsUnicode(false);

            modelBuilder.Entity<DreamWeddsBlog>()
                .Property(e => e.Quote)
                .IsUnicode(false);

            modelBuilder.Entity<DreamWeddsBlog>()
                .Property(e => e.AuthorName)
                .IsUnicode(false);

            modelBuilder.Entity<DreamWeddsBlog>()
                .Property(e => e.Content)
                .IsUnicode(false);

            modelBuilder.Entity<DreamWeddsBlog>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<DreamWeddsBlog>()
                .Property(e => e.SpecialNote)
                .IsUnicode(false);

            modelBuilder.Entity<ECampaign>()
                .Property(e => e.ECampaignName)
                .IsUnicode(false);

            modelBuilder.Entity<ECampaign>()
                .Property(e => e.ECampaignSubject)
                .IsUnicode(false);

            modelBuilder.Entity<ECampaign>()
                .Property(e => e.ECampaignLogoUrl)
                .IsUnicode(false);

            modelBuilder.Entity<ECampaign>()
                .Property(e => e.ECampaignContent)
                .IsUnicode(false);

            modelBuilder.Entity<EmailService>()
                .Property(e => e.FromEmail)
                .IsUnicode(false);

            modelBuilder.Entity<EmailService>()
                .Property(e => e.FromName)
                .IsUnicode(false);

            modelBuilder.Entity<EmailService>()
                .Property(e => e.ToName)
                .IsUnicode(false);

            modelBuilder.Entity<EmailService>()
                .Property(e => e.ToEmail)
                .IsUnicode(false);

            modelBuilder.Entity<EmailService>()
                .Property(e => e.CcEmail)
                .IsUnicode(false);

            modelBuilder.Entity<EmailService>()
                .Property(e => e.BccEmail)
                .IsUnicode(false);

            modelBuilder.Entity<EmailService>()
                .Property(e => e.Subject)
                .IsUnicode(false);

            modelBuilder.Entity<EmailService>()
                .Property(e => e.Body)
                .IsUnicode(false);

            modelBuilder.Entity<EmailService>()
                .Property(e => e.Mobile)
                .IsUnicode(false);

            modelBuilder.Entity<EmailService>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<EmailService>()
                .Property(e => e.Message)
                .IsUnicode(false);

            modelBuilder.Entity<EmailService>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<EmailService>()
                .Property(e => e.AttachmentFileName)
                .IsUnicode(false);

            modelBuilder.Entity<EmailService>()
                .Property(e => e.Remarks)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorLog>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorLog>()
                .Property(e => e.MachineName)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorLog>()
                .Property(e => e.AppDomainName)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorLog>()
                .Property(e => e.ProcessID)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorLog>()
                .Property(e => e.ProcessName)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorLog>()
                .Property(e => e.ThreadName)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorLog>()
                .Property(e => e.Win32ThreadId)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorLog>()
                .Property(e => e.Message)
                .IsUnicode(false);

            modelBuilder.Entity<ErrorLog>()
                .Property(e => e.FormattedMessage)
                .IsUnicode(false);

            modelBuilder.Entity<FAQ>()
                .Property(e => e.Question)
                .IsUnicode(false);

            modelBuilder.Entity<FAQ>()
                .Property(e => e.Answer)
                .IsUnicode(false);

            modelBuilder.Entity<FAQ>()
                .Property(e => e.Website)
                .IsUnicode(false);

            modelBuilder.Entity<GroomAndMan>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<GroomAndMan>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<GroomAndMan>()
                .Property(e => e.Imageurl)
                .IsUnicode(false);

            modelBuilder.Entity<GroomAndMan>()
                .Property(e => e.AboutMen)
                .IsUnicode(false);

            modelBuilder.Entity<GroomAndMan>()
                .Property(e => e.fbUrl)
                .IsUnicode(false);

            modelBuilder.Entity<GroomAndMan>()
                .Property(e => e.googleUrl)
                .IsUnicode(false);

            modelBuilder.Entity<GroomAndMan>()
                .Property(e => e.instagramUrl)
                .IsUnicode(false);

            modelBuilder.Entity<GroomAndMan>()
                .Property(e => e.lnkedinUrl)
                .IsUnicode(false);

            modelBuilder.Entity<LoginAttemptHistory>()
                .Property(e => e.Lattitude)
                .IsUnicode(false);

            modelBuilder.Entity<LoginAttemptHistory>()
                .Property(e => e.Longitude)
                .IsUnicode(false);

            modelBuilder.Entity<LoginAttemptHistory>()
                .Property(e => e.IpAddress)
                .IsUnicode(false);

            modelBuilder.Entity<LoginAttemptHistory>()
                .Property(e => e.Browser)
                .IsUnicode(false);

            modelBuilder.Entity<ModuleMaster>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<ModuleMaster>()
                .Property(e => e.Icon)
                .IsUnicode(false);

            modelBuilder.Entity<ModuleMaster>()
                .Property(e => e.ModuleDescription)
                .IsUnicode(false);

            modelBuilder.Entity<ModuleMaster>()
                .Property(e => e.PageURL)
                .IsUnicode(false);

            modelBuilder.Entity<ModuleMaster>()
                .HasMany(e => e.RoleModules)
                .WithRequired(e => e.ModuleMaster)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderDetail>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrderMaster>()
                .Property(e => e.OrderStatus)
                .IsUnicode(false);

            modelBuilder.Entity<OrderMaster>()
                .Property(e => e.AddressID)
                .IsUnicode(false);

            modelBuilder.Entity<OrderMaster>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrderMaster>()
                .Property(e => e.ReceivedAmount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<OrderMaster>()
                .Property(e => e.PaymentTerms)
                .IsUnicode(false);

            modelBuilder.Entity<OrderMaster>()
                .Property(e => e.OderNote)
                .IsUnicode(false);

            modelBuilder.Entity<OrderMaster>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.OrderMaster)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<OrderMaster>()
                .HasMany(e => e.UserWeddingSubscriptions)
                .WithOptional(e => e.OrderMaster)
                .HasForeignKey(e => e.InvoiceNo);

            modelBuilder.Entity<OrderMaster>()
                .HasMany(e => e.UserWeddingSubscriptions1)
                .WithOptional(e => e.OrderMaster1)
                .HasForeignKey(e => e.InvoiceNo);

            modelBuilder.Entity<OTPMaster>()
                .Property(e => e.OTP)
                .IsUnicode(false);

            modelBuilder.Entity<OTPMaster>()
                .Property(e => e.GUID)
                .IsUnicode(false);

            modelBuilder.Entity<Permission>()
                .Property(e => e.PermissionValue)
                .IsUnicode(false);

            modelBuilder.Entity<RoleMaster>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<RoleMaster>()
                .Property(e => e.Code)
                .IsUnicode(false);

            modelBuilder.Entity<RoleMaster>()
                .Property(e => e.RoleDescription)
                .IsUnicode(false);

            modelBuilder.Entity<RoleMaster>()
                .HasMany(e => e.RoleModules)
                .WithRequired(e => e.RoleMaster)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RoleMaster>()
                .HasMany(e => e.UserRoles)
                .WithRequired(e => e.RoleMaster)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RoleModule>()
                .HasMany(e => e.UserRoleModulePermissions)
                .WithOptional(e => e.RoleModule)
                .WillCascadeOnDelete();

            modelBuilder.Entity<RSVPDetail>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<RSVPDetail>()
                .Property(e => e.MiddleName)
                .IsUnicode(false);

            modelBuilder.Entity<RSVPDetail>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<RSVPDetail>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<RSVPDetail>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<RSVPDetail>()
                .Property(e => e.Mobile)
                .IsUnicode(false);

            modelBuilder.Entity<RSVPDetail>()
                .Property(e => e.SpecialNote)
                .IsUnicode(false);

            modelBuilder.Entity<RSVPDetail>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<RSVPDetail>()
                .Property(e => e.ParticipatingInEvents)
                .IsUnicode(false);

            modelBuilder.Entity<RSVPDetail>()
                .Property(e => e.ComingFromCity)
                .IsUnicode(false);

            modelBuilder.Entity<SubscriptionMaster>()
                .Property(e => e.SubsName)
                .IsUnicode(false);

            modelBuilder.Entity<SubscriptionMaster>()
                .Property(e => e.SubsCode)
                .IsUnicode(false);

            modelBuilder.Entity<SubscriptionMaster>()
                .HasMany(e => e.OrderDetails)
                .WithOptional(e => e.SubscriptionMaster)
                .HasForeignKey(e => e.SubscrptionID);

            modelBuilder.Entity<SubscriptionMaster>()
                .HasMany(e => e.UserWeddingSubscriptions)
                .WithRequired(e => e.SubscriptionMaster)
                .HasForeignKey(e => e.SubscriptionType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SubscriptionMaster>()
                .HasMany(e => e.UserWeddingSubscriptions1)
                .WithRequired(e => e.SubscriptionMaster1)
                .HasForeignKey(e => e.SubscriptionType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SystemSetting>()
                .Property(e => e.WeeklyOffDays)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateImage>()
                .Property(e => e.ImageName)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateImage>()
                .Property(e => e.ImageTitle)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateImage>()
                .Property(e => e.ImageTagLine)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateImage>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateImage>()
                .Property(e => e.ImageFolderPath)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateMaster>()
                .Property(e => e.TemplateContent)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateMaster>()
                .Property(e => e.TemplateSubject)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateMaster>()
                .Property(e => e.TemplateTags)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateMaster>()
                .Property(e => e.TemplateUrl)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateMaster>()
                .Property(e => e.TemplateFolderPath)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateMaster>()
                .Property(e => e.ThumbnailImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateMaster>()
                .Property(e => e.TagLine)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateMaster>()
                .Property(e => e.AuthorName)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateMaster>()
                .Property(e => e.AboutTemplate)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateMaster>()
                .Property(e => e.Features)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateMaster>()
                .HasMany(e => e.OrderDetails)
                .WithRequired(e => e.TemplateMaster)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TemplateMaster>()
                .HasMany(e => e.TemplateImages)
                .WithRequired(e => e.TemplateMaster)
                .HasForeignKey(e => e.TemplateID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TemplateMaster>()
                .HasMany(e => e.TemplateImages1)
                .WithRequired(e => e.TemplateMaster1)
                .HasForeignKey(e => e.TemplateID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TemplateMaster>()
                .HasMany(e => e.TemplateMergeFields)
                .WithOptional(e => e.TemplateMaster)
                .HasForeignKey(e => e.TemplateID);

            modelBuilder.Entity<TemplateMaster>()
                .HasMany(e => e.TemplateMergeFields1)
                .WithOptional(e => e.TemplateMaster1)
                .HasForeignKey(e => e.TemplateID);

            modelBuilder.Entity<TemplateMaster>()
                .HasMany(e => e.TemplatePages)
                .WithRequired(e => e.TemplateMaster)
                .HasForeignKey(e => e.TemplateID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TemplateMaster>()
                .HasMany(e => e.TemplatePages1)
                .WithRequired(e => e.TemplateMaster1)
                .HasForeignKey(e => e.TemplateID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TemplateMaster>()
                .HasMany(e => e.UserWeddingSubscriptions)
                .WithRequired(e => e.TemplateMaster)
                .HasForeignKey(e => e.TemplateID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TemplateMaster>()
                .HasMany(e => e.UserWeddingSubscriptions1)
                .WithRequired(e => e.TemplateMaster1)
                .HasForeignKey(e => e.TemplateID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TemplateMergeField>()
                .Property(e => e.MERGEFIELD_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateMergeField>()
                .Property(e => e.SRC_FIELD)
                .IsUnicode(false);

            modelBuilder.Entity<TemplateMergeField>()
                .Property(e => e.SRC_FIELD_VALUE)
                .IsUnicode(false);

            modelBuilder.Entity<TemplatePage>()
                .Property(e => e.PageName)
                .IsUnicode(false);

            modelBuilder.Entity<TemplatePage>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<TemplatePage>()
                .Property(e => e.PageContent)
                .IsUnicode(false);

            modelBuilder.Entity<TemplatePage>()
                .Property(e => e.PageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<TemplatePage>()
                .Property(e => e.PageFolderPath)
                .IsUnicode(false);

            modelBuilder.Entity<TemplatePage>()
                .Property(e => e.ThumbnailImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<TimeLine>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<TimeLine>()
                .Property(e => e.Story)
                .IsUnicode(false);

            modelBuilder.Entity<TimeLine>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<TimeLine>()
                .Property(e => e.Location)
                .IsUnicode(false);

            modelBuilder.Entity<UserDevice>()
                .Property(e => e.IMEINumber)
                .IsUnicode(false);

            modelBuilder.Entity<UserDevice>()
                .Property(e => e.LoginName)
                .IsUnicode(false);

            modelBuilder.Entity<UserDevice>()
                .Property(e => e.SenderKey)
                .IsUnicode(false);

            modelBuilder.Entity<UserMaster>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<UserMaster>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<UserMaster>()
                .Property(e => e.LoginName)
                .IsUnicode(false);

            modelBuilder.Entity<UserMaster>()
                .Property(e => e.Password)
                .IsUnicode(false);

            modelBuilder.Entity<UserMaster>()
                .Property(e => e.ImagePath)
                .IsUnicode(false);

            modelBuilder.Entity<UserMaster>()
                .Property(e => e.EmpCode)
                .IsUnicode(false);

            modelBuilder.Entity<UserMaster>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<UserMaster>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<UserMaster>()
                .Property(e => e.Mobile)
                .IsUnicode(false);

            modelBuilder.Entity<UserMaster>()
                .HasMany(e => e.LoginAttemptHistories)
                .WithRequired(e => e.UserMaster)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserMaster>()
                .HasMany(e => e.OrderMasters)
                .WithRequired(e => e.UserMaster)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserMaster>()
                .HasMany(e => e.UserDevices)
                .WithRequired(e => e.UserMaster)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserMaster>()
                .HasMany(e => e.UserServiceAccesses)
                .WithRequired(e => e.UserMaster)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserMaster>()
                .HasMany(e => e.UserSystemSettings)
                .WithRequired(e => e.UserMaster)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UserRoleModulePermission>()
                .Property(e => e.PermissionValue)
                .IsUnicode(false);

            modelBuilder.Entity<UserServiceAccess>()
                .Property(e => e.APIKey)
                .IsUnicode(false);

            modelBuilder.Entity<UserServiceAccess>()
                .Property(e => e.APIToken)
                .IsUnicode(false);

            modelBuilder.Entity<UserWeddingSubscription>()
                .Property(e => e.ReasonOfUpdate)
                .IsUnicode(false);

            modelBuilder.Entity<Venue>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Venue>()
                .Property(e => e.VenueImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<Venue>()
                .Property(e => e.VenueBannerImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<Venue>()
                .Property(e => e.VenueWebsite)
                .IsUnicode(false);

            modelBuilder.Entity<Venue>()
                .Property(e => e.OwnerName)
                .IsUnicode(false);

            modelBuilder.Entity<Venue>()
                .Property(e => e.VenuePhone)
                .IsUnicode(false);

            modelBuilder.Entity<Venue>()
                .Property(e => e.VenueMobile)
                .IsUnicode(false);

            modelBuilder.Entity<Venue>()
                .Property(e => e.googleMapUrl)
                .IsUnicode(false);

            modelBuilder.Entity<WeddingEvent>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<WeddingEvent>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<WeddingEvent>()
                .Property(e => e.Aboutevent)
                .IsUnicode(false);

            modelBuilder.Entity<WeddingEvent>()
                .Property(e => e.BackGroundImage)
                .IsUnicode(false);

            modelBuilder.Entity<WeddingGallery>()
                .Property(e => e.ImageTitle)
                .IsUnicode(false);

            modelBuilder.Entity<WeddingGallery>()
                .Property(e => e.ImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<WeddingGallery>()
                .Property(e => e.ImageName)
                .IsUnicode(false);

            modelBuilder.Entity<WeddingGallery>()
                .Property(e => e.Place)
                .IsUnicode(false);

            modelBuilder.Entity<Wedding>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<Wedding>()
                .Property(e => e.IconUrl)
                .IsUnicode(false);

            modelBuilder.Entity<Wedding>()
                .Property(e => e.BackgroundImage)
                .IsUnicode(false);

            modelBuilder.Entity<Wedding>()
                .Property(e => e.Quote)
                .IsUnicode(false);

            modelBuilder.Entity<Wedding>()
                .Property(e => e.fbPageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<Wedding>()
                .Property(e => e.videoUrl)
                .IsUnicode(false);

            modelBuilder.Entity<Wedding>()
                .HasMany(e => e.BrideAndMaids)
                .WithRequired(e => e.Wedding)
                .HasForeignKey(e => e.WeddingID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Wedding>()
                .HasMany(e => e.BrideAndMaids1)
                .WithRequired(e => e.Wedding1)
                .HasForeignKey(e => e.WeddingID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Wedding>()
                .HasMany(e => e.GroomAndMen)
                .WithRequired(e => e.Wedding)
                .HasForeignKey(e => e.WeddingID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Wedding>()
                .HasMany(e => e.GroomAndMen1)
                .WithRequired(e => e.Wedding1)
                .HasForeignKey(e => e.WeddingID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Wedding>()
                .HasMany(e => e.RSVPDetails)
                .WithRequired(e => e.Wedding)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Wedding>()
                .HasMany(e => e.TimeLines)
                .WithRequired(e => e.Wedding)
                .HasForeignKey(e => e.WeddingID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Wedding>()
                .HasMany(e => e.TimeLines1)
                .WithRequired(e => e.Wedding1)
                .HasForeignKey(e => e.WeddingID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Wedding>()
                .HasMany(e => e.UserWeddingSubscriptions)
                .WithOptional(e => e.Wedding)
                .HasForeignKey(e => e.WeddingID);

            modelBuilder.Entity<Wedding>()
                .HasMany(e => e.UserWeddingSubscriptions1)
                .WithOptional(e => e.Wedding1)
                .HasForeignKey(e => e.WeddingID);

            modelBuilder.Entity<Wedding>()
                .HasMany(e => e.WeddingEvents)
                .WithRequired(e => e.Wedding)
                .HasForeignKey(e => e.WeddingID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Wedding>()
                .HasMany(e => e.WeddingEvents1)
                .WithRequired(e => e.Wedding1)
                .HasForeignKey(e => e.WeddingID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Wedding>()
                .HasMany(e => e.WeddingGalleries)
                .WithRequired(e => e.Wedding)
                .HasForeignKey(e => e.WeddingID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Wedding>()
                .HasMany(e => e.WeddingGalleries1)
                .WithRequired(e => e.Wedding1)
                .HasForeignKey(e => e.WeddingID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<vwGetUserRoleModule>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<vwGetUserRoleModule>()
                .Property(e => e.Icon)
                .IsUnicode(false);

            modelBuilder.Entity<vwGetUserRoleModule>()
                .Property(e => e.PageURL)
                .IsUnicode(false);

            modelBuilder.Entity<vwGetUserWeddingTemplate>()
                .Property(e => e.Amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<vwGetUserWeddingTemplate>()
                .Property(e => e.OrderStatus)
                .IsUnicode(false);

            modelBuilder.Entity<vwGetUserWeddingTemplate>()
                .Property(e => e.TemplateFolderPath)
                .IsUnicode(false);

            modelBuilder.Entity<vwGetUserWeddingTemplate>()
                .Property(e => e.ThumbnailImageUrl)
                .IsUnicode(false);

            modelBuilder.Entity<vwGetUserWeddingTemplate>()
                .Property(e => e.Title)
                .IsUnicode(false);

            modelBuilder.Entity<vwGetUserWeddingTemplate>()
                .Property(e => e.BackgroundImage)
                .IsUnicode(false);
        }


    }

    public partial class SPCheckAuthentication_Result
    {
        public Nullable<byte> AuthStatus { get; set; }
        public Nullable<int> userID { get; set; }
    }

    public partial class SPGetLoginDetails_Result
    {
        public Nullable<int> RoleId { get; set; }
        public Nullable<int> UserID { get; set; }
        public string RoleName { get; set; }
        public Nullable<int> UserRoleID { get; set; }
        public string EmpCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string LoginName { get; set; }
        public string ProfilePictureFileName { get; set; }
    }


}
