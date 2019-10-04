using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class CheckUserAuthenticationBO
    {
        public int userID { get; set; }
        public Nullable<byte> AuthStatus { get; set; }
    }
}
