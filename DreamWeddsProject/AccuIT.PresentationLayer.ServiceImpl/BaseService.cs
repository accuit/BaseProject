using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.CommonLayer.EntityMapper;
//using Samsung.SmartDost.ExternalServiceLayer.MDMServices.Contracts;

namespace AccuIT.PresentationLayer.ServiceImpl
{
    /// <summary>
    /// Class to provide base class instance to other class members and methods
    /// </summary>
    public class BaseService
    {
        private IUserService userBusinessInstance;
        private ISystemService systemBusinessInstance;
        private IWeddingService weddingBusinessInstance;
        private IMapper entityMapper;
        private ISecurityService securityBusinessInstance;

        /// <summary>
        /// Property to get instance of Store Manager business class
        /// </summary>
        /// 

        public IWeddingService WeddingBusinessInstance
        {
            get
            {
                if (weddingBusinessInstance == null)
                {
                    weddingBusinessInstance = AopEngine.Resolve<IWeddingService>(AspectEnums.AspectInstanceNames.WeddingManager, AspectEnums.ApplicationName.AccuIT);
                }
                return weddingBusinessInstance;
            }
        }

        /// <summary>
        /// Property to get instance of User Manager business class
        /// </summary>
        public IUserService UserBusinessInstance
        {
            get
            {
                if (userBusinessInstance == null)
                {
                    userBusinessInstance = AopEngine.Resolve<IUserService>(AspectEnums.AspectInstanceNames.UserManager, AspectEnums.ApplicationName.AccuIT);
                }
                return userBusinessInstance;
            }
        }

        /// <summary>
        /// Property to get instance of System manager business class
        /// </summary>
        public ISystemService SystemBusinessInstance
        {
            get
            {
                if (systemBusinessInstance == null)
                {
                    systemBusinessInstance = AopEngine.Resolve<ISystemService>(AspectEnums.AspectInstanceNames.ServiceManager, AspectEnums.ApplicationName.AccuIT);
                }

                return systemBusinessInstance;
            }
        }


        #region for SDCE -991 (FMS) by vaishali on 27 Nov 2014
        /// <summary>
        /// Property to get instance of Report manager business class
        /// </summary>
        //public IFeedbackService FeedbackBusinessInstance
        //{
        //    get
        //    {
        //        if (feedbackinstance == null)
        //        {
        //            feedbackinstance = AopEngine.Resolve<IFeedbackService>(AspectEnums.AspectInstanceNames.FeedbackManager, AspectEnums.ApplicationName.Samsung);
        //        }

        //        return feedbackinstance;
        //    }
        //}
        #endregion


        #region SALESCATALYST added by Neeraj
        /// <summary>
        /// Added by Neeraj Singh on 26th July 2015
        /// </summary>
        public ISecurityService SecurityBusinessInstance
        {
            get
            {
                if (securityBusinessInstance == null)
                {
                    securityBusinessInstance = AopEngine.Resolve<ISecurityService>(AspectEnums.AspectInstanceNames.SecurityManager, AspectEnums.ApplicationName.AccuIT);
                }
                return securityBusinessInstance;
            }
        }
        #endregion

        /// <summary>
        /// Property to get Object Mapper instance
        /// </summary>
        public IMapper EntityMapper
        {
            get
            {
                if (entityMapper == null)
                {
                    entityMapper = AopEngine.Resolve<IMapper>();
                }
                return entityMapper;
            }
        }
    }
}
