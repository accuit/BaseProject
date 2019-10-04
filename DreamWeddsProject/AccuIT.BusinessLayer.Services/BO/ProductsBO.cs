using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class ProductTypeBO
    {
        public string ProductTypeCode { get; set; }
        public string ProductTypeName { get; set; }
        
       
    }

    public class ProductGroupsBO
    {
        public string ProductTypeCode { get; set; }
        public string ProductGroupCode { get; set; }
        public string ProductGroupName { get; set; }
    }

    public class ProductCategoryBO
    {
        public string ProductTypeCode { get; set; }
        public string ProductGroupCode { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
    }

    public class ProductModelBO
    {
        public string ProductTypeCode { get; set; }
        public string ProductGroupCode { get; set; }
        public string CategoryCode { get; set; }
        public string BasicModelCode { get; set; }
        public string BasicModelName { get; set; }
        public string SKUCode { get; set; }
        public string SKUName { get; set; }
        public Nullable<decimal> MRP { get; set; }
        public Nullable<decimal> DealerPrice { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
    }
}
