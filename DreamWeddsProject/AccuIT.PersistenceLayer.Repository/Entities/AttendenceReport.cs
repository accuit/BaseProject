using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.PersistenceLayer.Repository.Entities
{
    public class AttendenceReport
    {
        public long UserID
        {
            get;
            set;
        }
        public string PersonName
        {
            get;
            set;
        }
        public int Percentage
        {
            get;
            set;
        }
        public int PresentUserCount
        {
            get;
            set;
        }
        public int PlanUserCount
        {
            get;
            set;
        }
    }
    public class TeamLevel
    {
        public int RoleID { get; set; }
        public string TeamCode { get; set; }
        public string RoleCode { get; set; }
        public int ProfileLevelCount { get; set; }
    }

}
