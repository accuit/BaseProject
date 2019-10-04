using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    [Serializable]
    public class ModuleMasterBO
    {
        public int ModuleID { get; set; }
        public string Name { get; set; }
        public Nullable<int> ParentModuleID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public int ModuleCode { get; set; }
        public bool IsMobile { get; set; }
        public bool IsMandatory { get; set; }
        public bool IsDeleted { get; set; }
        public int Sequence { get; set; }
        public bool IsSystemModule { get; set; }
        public string ParentName { get; set; }
        public string Icon { get; set; }
        public int Status { get; set; }

        // Add ModuleType by Navneet Sharma on 3-Dec-2014 //
        public int ModuleType { get; set; }
        public string ModuleDescription { get; set; }
        public string PageURL { get; set; }

        public bool IsSelected { get; set; }

    }

 
}
