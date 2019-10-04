using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AccuIT.BusinessLayer.Services.BO
{
   public class AttendenceReportBO
    {
        public int UserID
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

   public class TeamLevelBO
   {
       public int RoleID { get; set; }
       public string TeamCode { get; set; }
       public int ProfileLevelCount { get; set; }
       public int TeamID { get; set; }
   }
}
