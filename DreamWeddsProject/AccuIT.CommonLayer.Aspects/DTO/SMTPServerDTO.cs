using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    public class SMTPServerDTO
    {
        public int SMTPInfoID { get; set; }
        public string ServerName { get; set; }
        public Nullable<int> PortName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public bool IsSSL { get; set; }
        public string PortNumber { get; set; }
    }
}
