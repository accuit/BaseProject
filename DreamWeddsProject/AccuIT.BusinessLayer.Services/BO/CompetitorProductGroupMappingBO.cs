using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
   public  class CompetitorProductGroupMappingBO
    {
        public int MappingID { get; set; }
        public int CompetitorID { get; set; }
        public int CompProductGroupID { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
