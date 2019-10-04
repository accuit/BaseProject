using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.ReportBO
{
    public class SeniorBO
    {
        public long UserID { get; set; }
        public string FirstName { get; set; }
        public Nullable<int> RoleID { get; set; }
        //public Nullable<long> ReportingUserID { get; set; }
        //public Nullable<int> ReportingTeamID { get; set; }
    }
}
