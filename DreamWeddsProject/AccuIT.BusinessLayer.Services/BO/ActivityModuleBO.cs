using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class ActivityModuleBO
    {
        public int ActivityModuleid { get; set; }
        public int RoleID { get; set; }
        public int ModuleID { get; set; }
        public bool IsMandatory { get; set; }
        public int Squence { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public Nullable<int> ModifyBy { get; set; }
    }
}
