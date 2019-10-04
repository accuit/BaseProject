using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
   public class CollectionReportBO
    {
        public string Name { get; set; }
        public string ProfilePictureFileName { get; set; }
        public long UserID { get; set; }
        public int CollectionBookedCount { get; set; }
        public int TotalStoreCount { get; set; }
        public int Percentage { get; set; }
        public string Disty { get; set; }
        public string StroreCode { get; set; }
        public string StoreName { get; set; }
        public string TransactionNo { get; set; }
        public string TransactionDate { get; set; }
        public string CollectionAmount { get; set; }
        public string PaymentMode { get; set; }
    }
}
