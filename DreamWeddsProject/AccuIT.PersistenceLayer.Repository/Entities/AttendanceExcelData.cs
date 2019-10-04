using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.PersistenceLayer.Repository.Entities
{
    public class AttendanceExcelData
    {
        public DateTime Date { get; set; }
        public long UserID { get; set; }
        public string ShipToName { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Mobile_Calling { get; set; }
        public int AccountStatus { get; set; }
        public Nullable<System.DateTime> AttenTime { get; set; }
        public string AttenType { get; set; }
        public Nullable<System.DateTime> LastLoginDate { get; set; }
       
    }
}
