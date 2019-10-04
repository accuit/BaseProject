
using AccuIT.BusinessLayer.IC.Data;
using System;
using AccuIT.BusinessLayer.IC.Contracts;

namespace AccuIT.BusinessLayer.IC
{
    /// <summary>
    /// 
    /// </summary>
    public class OrderManager : IOrderBooking
    {
        private DataImpl DataFactory { get; set; }


        public OrderManager()
        {
            DataFactory = new DataImpl();
        }
        /// <summary>
        /// Method to save order into dms database
        /// </summary>
        /// <param name="order">order entity</param>
        /// <param name="isUpdate">is data to be updated</param>
        /// <returns>returns boolean status</returns>
        public bool SaveOrderInDMS(AccuIT.BusinessLayer.IC.Entities.OrderBooking order)
        {
            return DataFactory.SaveOrderInDMS(order);
        }
    }
}
