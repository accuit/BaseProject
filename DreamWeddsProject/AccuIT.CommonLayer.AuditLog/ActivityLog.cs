using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using log4net;

namespace AccuIT.CommonLayer.Log
{
    public class ActivityLog
    {
        private static readonly ILog logger = LogManager.GetLogger("EShot");
        public static void SetLog(string message, LogLoc location)
        {

            switch (location)
            {
                case AccuIT.CommonLayer.Log.LogLoc.ERROR:
                    logger.Error(message);
                    break;
                case AccuIT.CommonLayer.Log.LogLoc.DEBUG:
                    logger.Debug(message);
                    break;
                case AccuIT.CommonLayer.Log.LogLoc.INFO:
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
}
