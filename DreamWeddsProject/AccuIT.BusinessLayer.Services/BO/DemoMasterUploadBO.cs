using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
     class DemoMasterUploadBO
    {

    }

    public class DemoProductMasterUploadBO
    {
        public Nullable<int> CompanyID { get; set; }
        public string ProductTypeCode { get; set; }
        public string ProductTypeName { get; set; }
        public string ProductGroupCode { get; set; }
        public string ProductGroupName { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string BasicModelCode { get; set; }
        public string BasicModelName { get; set; }
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public Nullable<decimal> MRP { get; set; }
        public Nullable<decimal> DealerPrice { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
