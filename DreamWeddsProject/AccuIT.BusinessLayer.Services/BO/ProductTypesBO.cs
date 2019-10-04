using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class ProductTypesBO
    {
        public int ProductTypeID { get; set; }
        public int CompanyID { get; set; }
        public string ProductType { get; set; }
        public bool IsActive { get; set; }
        public string Code { get; set; }
    }
}
