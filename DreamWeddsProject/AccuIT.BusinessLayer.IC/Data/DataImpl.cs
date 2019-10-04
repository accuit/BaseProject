using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using AccuIT.BusinessLayer.IC.Resources;
using AccuIT.CommonLayer.Aspects.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccuIT.BusinessLayer.IC.Resources;

namespace AccuIT.BusinessLayer.IC.Data
{
    public class DataImpl
    {
        private Database dataAccess;

        /// <summary>
        /// Property to get set database container instance of METLB
        /// </summary>
        public Database DataAccess
        {
            get
            {
                return dataAccess;
            }
            private set
            {
                dataAccess = value;
            }
        }

        public DataImpl()
        {
            InstantiateDatabaseAccess();
        }

        /// <summary>
        /// Method to save order into dms database
        /// </summary>
        /// <param name="order">order entity</param>
        /// <param name="isUpdate">is data to be updated</param>
        /// <returns>returns boolean status</returns>
        public bool SaveOrderInDMS(AccuIT.BusinessLayer.IC.Entities.OrderBooking order)
        {
            InstantiateDatabaseAccess();
            bool isSaved = false;
            DbCommand command = DataAccess.GetStoredProcCommand(DBConstants.SaveOrderBooking_SP_Name);
            DataAccess.AddInParameter(command, "orderID", DbType.Int64, order.OrderID);
            DataAccess.AddInParameter(command, "distCode", DbType.String, order.DistyCode);
            DataAccess.AddInParameter(command, "srpCode", DbType.String, order.SRPCode);
            DataAccess.AddInParameter(command, "orderKeyNo", DbType.String, order.OrderKeyNo);
            DataAccess.AddInParameter(command, "orderDate", DbType.DateTime, order.OrderDate);
            DataAccess.AddInParameter(command, "rtrCode", DbType.String, order.RTRCode);
            DataAccess.AddInParameter(command, "prdCode", DbType.String, order.ProductCode);
            DataAccess.AddInParameter(command, "orderQty", DbType.Int32, order.OrderQty);
            DataAccess.AddInParameter(command, "downloadFlag", DbType.String, order.DownloadFlag);
            DataAccess.AddInParameter(command, "createdDate", DbType.DateTime, System.DateTime.Now);
            DataAccess.AddInParameter(command, "routeCode", DbType.String, order.RouteCode);
            DataAccess.AddInParameter(command, "createdby", DbType.String, order.CreatedBy);
            DataAccess.AddInParameter(command, "modifieddate", DbType.DateTime, System.DateTime.Now);
            DataAccess.AddInParameter(command, "modifiedby", DbType.String, order.ModifiedBy);
            DataAccess.AddInParameter(command, "isUpdate", DbType.Boolean, false);
            DataAccess.ExecuteNonQuery(command);
            isSaved = Convert.ToBoolean(DataAccess.GetParameterValue(command, "isUpdate"));
            return isSaved;
        }

        /// <summary>
        /// Method to initialize enterprise library database container instance
        /// </summary>
        private void InstantiateDatabaseAccess()
        {
            //if (dataAccess == null)
            //{
                dataAccess = EnterpriseLibraryContainer.Current.GetInstance<Database>(AppUtil.GetAppSettings(AspectEnums.ConfigKeys.SamsungDMSConnection)) as SqlDatabase;
            //}
            

        } 
    }
}
