using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    public class CheckUserAuthenticationDTO
    {
        public long userID { get; set; }
        public Nullable<byte> AuthStatus { get; set; }
    }
}
