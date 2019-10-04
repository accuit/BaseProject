using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class AuthUserDTO
    {  

        [DataMember]
        public string UserID
        {
            get;
            set;
        }

        [DataMember]
        public string APIKey
        {
            get;
            set;
        }

        [DataMember]
        public string APIToken
        {
            get;
            set;
        }
        [DataMember]
        public string CompanyID
        {
            get;
            set;
        }
       
    }
}
