using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace AccuIT.BusinessLayer.Services.BO
{
    public class ProductMasterBO
    {
        public long ProductId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string ProductName { get; set; }
        public string MeasureUnit { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<int> Availability { get; set; }
        public System.DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string Status { get; set; }
        public string ImagePath { get; set; }
    }
}
