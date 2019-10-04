using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class RaceReportModelWiseBO
    {
        public int auditid { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string ShipToRegion { get; set; }
        public string ShipToBranch { get; set; }
        public string ChannelType { get; set; }
        public string City { get; set; }
        public string STATE { get; set; }
        public System.DateTime AuditTime { get; set; }
        public int ProductID { get; set; }
        public Nullable<int> ModelCounter { get; set; }
        public string ProductType { get; set; }
        public string ProductGroup { get; set; }
        public string ProductCategory { get; set; }
        public byte BrandID { get; set; }
        public int TransType { get; set; }
        public string StoreCode { get; set; }
    }
}
