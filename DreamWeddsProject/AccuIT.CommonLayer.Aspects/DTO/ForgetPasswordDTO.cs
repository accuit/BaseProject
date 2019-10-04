using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    /// <summary>
    /// This class is used to define the response data for Forget Password method
    /// </summary>

    [DataContract]
  public  class ForgotPasswordDTO
    {
        [DataMember]
        public string StatusCode
        {
            get;
            set;
        }

        [DataMember]
        public string Message
        {
            get;
            set;
        }
    }
}
