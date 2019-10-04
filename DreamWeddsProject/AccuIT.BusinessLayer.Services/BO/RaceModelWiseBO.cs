using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
   public class RaceProductModelBO
    {
        public int ProductID { get; set; }
        public string ProductType { get; set; }
        public string ProductGroup { get; set; }
        public string ProductCategory { get; set; }
        public string ModelName { get; set; }
        public int BrandID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public string ProductSize { get; set; }
        public string BrandName { get; set; }
    }
}
