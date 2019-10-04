using AccuIT.PersistenceLayer.Repository.Entities;

namespace AccuIT.PersistenceLayer.Data.Impl
{
    /// <summary>
    /// Base class
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable")]
    public abstract class BaseDataImpl
    {
        private AccuITAdminEntities accuitAdminDbContext;

        #region Constructors

        /// <summary>
        /// Constructor to intialize database instance for EF
        /// </summary>
        public BaseDataImpl()
        {
            accuitAdminDbContext = new AccuITAdminEntities();
        }

        #endregion

        /// <summary>
        /// Property to get db context instance of Entity Framework Database
        /// </summary>
        public AccuITAdminEntities AccuitAdminDbContext
        {
            get
            {
                return accuitAdminDbContext;
            }
        }
    }
}
