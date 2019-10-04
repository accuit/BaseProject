using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class OrderBookedExportBO
    {
        public string UserCode { get; set; }
        public string Mobile_Calling { get; set; }
        public int AccountStatus { get; set; }
        public long UserID { get; set; }
        public string Designation { get; set; }
        public string UserName { get; set; }
        public int OrderBookedCount { get; set; }
        public int TotalStoreCount { get; set; }
        public int Percentage { get; set; }
        public string ProfilePictureFileName { get; set; }
        public string ShipToName { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string OrderNo { get; set; }
        public string OrderQuantity { get; set; }
        public string ProductCode { get; set; }
        public string Branch { get; set; }
        public string Region { get; set; }
        public string BasicModelName { get; set; }
        public string CategoryName { get; set; }
        public string VisitDate { get; set; }
    }
}
