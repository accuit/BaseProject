using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.IC.Entities
{
    /// <summary>
    /// Entity for order booking sync process
    /// </summary>
    public class OrderBooking
    {
        public long OrderID { get; set; }

        /// <summary>
        /// partner code
        /// </summary>
        public string DistyCode { get; set; }
        public string SRPCode { get; set; }
        /// <summary>
        /// Order number
        /// </summary>
        public string OrderKeyNo { get; set; }
        /// <summary>
        /// Order date
        /// </summary>
        public DateTime OrderDate { get; set; }
        /// <summary>
        /// Store code
        /// </summary>
        public string RTRCode { get; set; }

        /// <summary>
        /// Store code
        /// </summary>
        public string RouteCode { get; set; }
        /// <summary>
        /// Product code
        /// </summary>
        public string ProductCode { get; set; }
        /// <summary>
        /// Order Quantity
        /// </summary>
        public int? OrderQty { get; set; }
        public string DownloadFlag { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
    }
}
