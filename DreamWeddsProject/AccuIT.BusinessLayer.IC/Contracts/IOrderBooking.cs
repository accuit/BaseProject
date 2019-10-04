using AccuIT.BusinessLayer.IC.Entities;

namespace AccuIT.BusinessLayer.IC.Contracts
{
    /// <summary>
    /// Order booking interface to sync into DMS
    /// </summary>
    public interface IOrderBooking
    {
        /// <summary>
        /// Method to save order into dms database
        /// </summary>
        /// <param name="order">order entity</param>
        /// <returns>returns boolean status</returns>
        bool SaveOrderInDMS(Entities.OrderBooking order);
    }
}
