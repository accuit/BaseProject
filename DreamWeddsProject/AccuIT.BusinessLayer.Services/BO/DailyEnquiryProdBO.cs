using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class DailyEnquiryProdBO
    {
        public long EndProdId { get; set; }
        public int CaseID { get; set; }
        public Nullable<long> ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual DailyEnquiryBO dailyEnquiryBO { get; set; }
        public virtual ProductMasterBO productMaster { get; set; }
    }
}
