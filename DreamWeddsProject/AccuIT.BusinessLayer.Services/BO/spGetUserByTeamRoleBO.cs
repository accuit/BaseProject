using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class spGetUserByTeamRoleBO
    {
        public long UserID { get; set; }
        public string EmplCodeWithName { get; set; }
        public string TeamCodeWithName { get; set; }
        public string RoleCodeWithName { get; set; }
        public int TeamID { get; set; }
        public int RoleID { get; set; }
    }
}
