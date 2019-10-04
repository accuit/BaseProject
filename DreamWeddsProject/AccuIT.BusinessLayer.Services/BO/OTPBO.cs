using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class OTPBO
    {
        public long OTPMasterID { get; set; }
        public int UserID { get; set; }
        public string GUID { get; set; }
        public string OTP { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> Attempts { get; set; }
    }
}
