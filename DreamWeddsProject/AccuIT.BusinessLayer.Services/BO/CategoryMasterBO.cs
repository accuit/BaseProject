using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class CategoryMasterBO
    {
        public int CategoryID { get; set; }
        public int ParentCatgID { get; set; }

        
        public string CategoryName { get; set; }
        public string CategoryCode { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> Created_BY { get; set; }
        public Nullable<int> Modified_BY { get; set; }
        public Nullable<System.DateTime> Created_Date { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
    
    }
}
