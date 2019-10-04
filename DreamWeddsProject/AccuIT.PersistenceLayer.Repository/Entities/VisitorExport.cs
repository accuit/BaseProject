using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.PersistenceLayer.Repository.Entities
{
    public class VisitorExport
    {
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Mobile_Calling { get; set; }
        public int AccountStatus { get; set; }
        public long UserID { get; set; }
        public string VisitorDesignation { get; set; }
        public string VisitorName { get; set; }
        public string VisitorID { get; set; }
        public int TotalStoreCount { get; set; }
        public int TotalVisitorCount { get; set; }
        public int Percentage { get; set; }
        public string Branch { get; set; }
        public string Region { get; set; }
        public string VisitDate { get; set; }
        public string Disty { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
    }
}
