using System;
using System.Runtime.Remoting.Messaging;
using AccuIT.CommonLayer.Aspects.Utilities;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using log4net;

namespace AccuIT.CommonLayer.Aspects.Logging
{
    /// <summary>
    /// Class to invoke the Log & Trace engine of enterprise library framework
    /// </summary>
    public static class ActivityLog
    {

        private static readonly ILog logger = LogManager.GetLogger("EShot");
        public static void SetLog(string message, LogLoc location)
        {

            switch (location)
            {
                case LogLoc.ERROR:
                    logger.Error(message);
                    break;
                case LogLoc.DEBUG:
                    logger.Debug(message);
                    break;
                case LogLoc.INFO:
                    logger.Info(message);
                    break;
                //case Hays.Log.LogLoc.SQLDB:
                //    using (IDBManager dbManager = new DBManager(DataProvider.SqlServer))
                //    {
                //        dbManager.AddParameters(0, "@error", message, DbType.String);
                //        dbManager.ExecuteNonQuery(CommandType.StoredProcedure, System.Configuration.ConfigurationSettings.AppSettings["SQlAuditSPNmame"]);
                //    }
                //    break;
                //case Hays.Log.LogLoc.ORACLEDB:
                //    using (IDBManager dbManager = new DBManager(DataProvider.Oracle))
                //    {
                //        dbManager.AddParameters(0, "@error", message, DbType.String);
                //        dbManager.ExecuteNonQuery(CommandType.StoredProcedure, System.Configuration.ConfigurationSettings.AppSettings["OracleAuditSPNmame"]);
                //    }
                //    break;
                default:
                    logger.Debug(message);
                    break;
            }
        }
    }

    public enum LogLoc
    {
        ERROR,
        DEBUG,
        INFO,
        SQLDB,
        ORACLEDB
    }
}
