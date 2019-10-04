using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class NotificationServiceBO
    {
        public long NotificationServiceID { get; set; }
        public long NotificationID { get; set; }
        public string AndroidID { get; set; }
        public int Frequency { get; set; }
        public string Notification { get; set; }
        public DateTime NotificationDate { get; set; }
        public DateTime EndDate { get; set; }
        public long UserID { get; set; }
    }
}
