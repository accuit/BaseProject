using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.PersistenceLayer.Repository.Entities
{
    public class SecurityAspect
    {
        public int PermissionID { get; set; }
        public string PermissionValue { get; set; }
        public int RoleID { get; set; }
        public int ModuleID { get; set; }
        public int UserID { get; set; }
        public long UserRolePermissionID { get; set; }
        public int ModuleCode { get; set; }
    }
}
