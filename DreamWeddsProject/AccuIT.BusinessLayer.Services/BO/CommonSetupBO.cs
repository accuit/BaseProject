using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class CommonSetupBO
    {
        public int CommonSetupID { get; set; }
        public string MainType { get; set; }
        public string SubType { get; set; }
        public string DisplayText { get; set; }
        public int DisplayValue { get; set; }
        public int ParentID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> isDeleted { get; set; }
    }
}
