using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class spGetRoleMasterBO
    {
        public int RoleID { get; set; }
        public string RoleNameWithProfileTeam { get; set; }
        public string RoleIDWithTeamID { get; set; }
        public string Code { get; set; }
        public string NAME { get; set; }
        public int ProfileLevel { get; set; }
    }
}
