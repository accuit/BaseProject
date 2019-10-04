using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using AccuIT.CommonLayer.Aspects.Logging;

namespace AccuIT.CommonLayer.Aspects.Exceptions
{
    /// <summary>
    /// Exception engine class for the enterprise library exception manager services and mappings
    /// </summary>
    public static class ExceptionEngine
    {
        //private variable for exception manager instance
        private static ExceptionManager _exceptionManager;


        #region Public Properties
        /// <summary>
        /// Property to get set exception manager instance
        /// </summary>
        public static ExceptionManager AppExceptionManager
        {
            get { return _exceptionManager; }
            private set { _exceptionManager = value; }
        }

        #endregion


        /// <summary>
        /// Method to initialize the enterprise library exception handler instance under the Aop
        /// </summary>
        public static void InitializeExceptionAopFramework()
        {
            try
            {
                //get the immediate application exception manager instance from Enterprise Library Container
                _exceptionManager = EnterpriseLibraryContainer.Current.GetInstance<ExceptionManager>();
            }
            catch (Exception ex)
            {
                LogTraceEngine.WriteLog(ex.Message);
            }
        }

        /// <summary>
        /// Method to execute action within scope of enterprise library exception handling manager
        /// </summary>
        /// <param name="action">Action to be executed</param>
        /// <param name="exceptionPolicyName">exception handling policy name</param>
        public static void ProcessAction(Action action, string exceptionPolicyName)
        {
            _exceptionManager.Process(action, exceptionPolicyName);
        }
    }
}
