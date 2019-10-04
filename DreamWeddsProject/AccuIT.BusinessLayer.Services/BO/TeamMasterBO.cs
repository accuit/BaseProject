using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
   public class TeamMasterBO
    {
        public int TeamID { get; set; }
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int MaxLevel { get; set; }
        public int Status { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
