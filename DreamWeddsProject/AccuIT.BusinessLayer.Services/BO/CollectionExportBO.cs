using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
   public class CollectionExportBO
    {
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Mobile_Calling { get; set; }
        public int AccountStatus { get; set; }
        public long UserID { get; set; }
        public string Designation { get; set; }
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
        public string Branch { get; set; }
        public string Region { get; set; }
        public string VisitDate { get; set; }
    }
}
