using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class AttendanceExportBO
    {
        public Nullable<System.DateTime> Date { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public Nullable<System.DateTime> attendanceDate { get; set; }
        public string Attendance { get; set; }
        public Nullable<long> UserID { get; set; }
    }
}
