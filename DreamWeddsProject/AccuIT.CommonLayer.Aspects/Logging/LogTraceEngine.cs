using System;
using System.Runtime.Remoting.Messaging;
using AccuIT.CommonLayer.Aspects.Utilities;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace AccuIT.CommonLayer.Aspects.Logging
{
    /// <summary>
    /// Class to invoke the Log & Trace engine of enterprise library framework
    /// </summary>
    public static class LogTraceEngine
    {
        #region Private Variables

        private static LogWriter _defaultLogWriter;
        private static TraceManager _traceManager;
        private static object sync = new object();
        private delegate void WriteLogAsync(string logMessage);
        private delegate void WriteLogWithCategoryAsync(string logMessage, AppVariables.AppLogTraceCategoryName category);
        private delegate void WriteLogArgumentsWithCategoryAsync(string logMessage, AppVariables.AppLogTraceCategoryName category, params string[] arguments);
        #endregion

        #region Public Methods

        /// <summary>
        /// Method to initialize services for logging in application
        /// </summary>
        public static void InitializeLoggingService()
        {
            try
            {
                //get the immediate application log writer and trace manager instance from Enterprise Library Container
                _defaultLogWriter = EnterpriseLibraryContainer.Current.GetInstance<LogWriter>();
                _traceManager = EnterpriseLibraryContainer.Current.GetInstance<TraceManager>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Method to log message details in asynchronous process
        /// </summary>
        /// <param name="message">message to log</param>
        public static void LogAsync(string message)
        {
            WriteLogAsync logDefinition = new WriteLogAsync(WriteLog);
            AsyncCallback logDefinitionCompletedCallback = new AsyncCallback(LogDefinitionCompletedCallBack);
            lock (sync)
            {
                logDefinition.BeginInvoke(message, logDefinitionCompletedCallback, null);
            }
        }

        /// <summary>
        /// Method to log messages with log category mentioned
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="category">log category</param>
        public static void LogAsync(string message, AppVariables.AppLogTraceCategoryName category)
        {
            WriteLogWithCategoryAsync logDefinition = new WriteLogWithCategoryAsync(WriteLogWithCategory);
            AsyncCallback logDefinitionWithCategoryCompletedCallBack = new AsyncCallback(LogDefinitionWithCategoryCompletedCallBack);
            lock (sync)
            {
                logDefinition.BeginInvoke(message, category, logDefinitionWithCategoryCompletedCallBack, null);
            }
        }

        /// <summary>
        /// Method to log messages with log category mentioned
        /// </summary>
        /// <param name="message">message to log</param>
        /// <param name="category">log category</param>
        /// <param name="arguments">arguments</param>
        public static void LogAsync(string message, AppVariables.AppLogTraceCategoryName category, params string[] arguments)
        {
            WriteLogArgumentsWithCategoryAsync logDefinition = new WriteLogArgumentsWithCategoryAsync(WriteLogParametersWithCategory);
            AsyncCallback logDefinitionWithParameterCategoryCompletedCallBack = new AsyncCallback(LogDefinitionWithParameterCategoryCompletedCallBack);
            lock (sync)
            {
                logDefinition.BeginInvoke(message, category, arguments, logDefinitionWithParameterCategoryCompletedCallBack, null);
            }
        }


        /// <summary>
        /// Method to write log in flat file of application directory location specified
        /// </summary>
        /// <param name="logMessage">log message</param>
        /// <param name="categoryName">category name</param>
        public static void WriteLog(string logMessage)
        {
            if (_defaultLogWriter != null)
                _defaultLogWriter.Write(logMessage, AppVariables.AppLogTraceCategoryName.General.ToString());
        }

        /// <summary>
        /// Method to write log information on the basis of category and message provided
        /// </summary>
        /// <param name="logMessage">log message</param>
        /// <param name="category">log category</param>
        public static void WriteLogWithCategory(string logMessage, AppVariables.AppLogTraceCategoryName category)
        {
            if (_defaultLogWriter != null)
                _defaultLogWriter.Write(logMessage, category.ToString());
        }

        /// <summary>
        /// Method to write log information on the basis of category and message provided
        /// </summary>
        /// <param name="logMessage">log message</param>
        /// <param name="category">log category</param>
        public static void WriteLogParametersWithCategory(string logMessage, AppVariables.AppLogTraceCategoryName category, params object[] arguments)
        {
            if (_defaultLogWriter != null)
            {
                if (arguments != null && arguments.Length > 0)
                {
                    logMessage += "\r\nWith Field(s)/Value(s): ";

                    foreach (var item in arguments)
                    {
                        if (item != null)
                        {
                            if (arguments.Length > 1)
                            {
                                logMessage += string.Format("{0}~ ", item.ToString());
                            }
                            else
                            {
                                logMessage += string.Format("{0}", item.ToString());
                            }
                        }
                    }
                }

                if (logMessage.Contains("~"))
                {
                    logMessage = logMessage.Substring(0, logMessage.LastIndexOf('~'));
                }

                _defaultLogWriter.Write(logMessage, AppVariables.AppLogTraceCategoryName.DiskFiles.ToString());
                _defaultLogWriter.Write(logMessage, category.ToString());
            }
        }

        /// <summary>   
        /// Method to write trace log in flat file of application directory location specified
        /// </summary>
        /// <param name="traceMessage">trace message</param>
        public static void WriteTrace(string traceMessage)
        {
            if (_defaultLogWriter != null)
                _defaultLogWriter.Write(traceMessage, AppVariables.AppLogTraceCategoryName.Tracing.ToString());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Method to handle the log completed call back method
        /// </summary>
        /// <param name="result">result obtained</param>
        private static void LogDefinitionCompletedCallBack(IAsyncResult result)
        {
            WriteLogAsync importStock = (WriteLogAsync)((AsyncResult)result).AsyncDelegate;
        }

        /// <summary>
        /// Method to handle the log with category completed call back method
        /// </summary>
        /// <param name="result">result obtained</param>
        private static void LogDefinitionWithCategoryCompletedCallBack(IAsyncResult result)
        {
            WriteLogWithCategoryAsync importStock = (WriteLogWithCategoryAsync)((AsyncResult)result).AsyncDelegate;
        }

        /// <summary>
        /// Method to handle the log with category completed call back method
        /// </summary>
        /// <param name="result">result obtained</param>
        private static void LogDefinitionWithParameterCategoryCompletedCallBack(IAsyncResult result)
        {
            WriteLogArgumentsWithCategoryAsync importStock = (WriteLogArgumentsWithCategoryAsync)((AsyncResult)result).AsyncDelegate;
        }

        #endregion
    }
}
