using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.ReportBO
{
    public class SchemeReportBO
    {
        public string SchemeNumber { get; set; }
        public string PUMINumber { get; set; }
        public System.DateTime PUMIDate { get; set; }
        public System.DateTime SchemeFrom { get; set; }
        public System.DateTime schemeTo { get; set; }
        public System.DateTime OrderPlaced { get; set; }
        public string NAME { get; set; }
        public string UserCode { get; set; }
        public string ProductType { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string BasicModelCode { get; set; }
        public int OrderQuantity { get; set; }
        public int MaxSupport { get; set; }
        public int SupportRequired { get; set; }
        public long createdBy { get; set; }
        public string UserType { get; set; }
    }
}
