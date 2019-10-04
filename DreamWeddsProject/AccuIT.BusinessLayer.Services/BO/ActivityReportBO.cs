using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class ActivityReportBO
    {
        public long ID { get; set; }
        public Nullable<System.DateTime> SurveyDate { get; set; }
        public string Region { get; set; }
        public string Branch { get; set; }
        public string UserType { get; set; }
        public string UserCode { get; set; }
        public long UserID { get; set; }
        public string UserName { get; set; }
        public string MainModule { get; set; }
        public string SubModule { get; set; }
        public string OLID { get; set; }
        public string OLName { get; set; }
        public string Question { get; set; }
        public string UserResponse { get; set; }
        public string FreezeGeoTag { get; set; }
        public string Deviation { get; set; }
        public string Distance { get; set; }
        public string UserOption { get; set; }

    }
}
