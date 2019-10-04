using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class CoverageUserDTO
    {
        [DataMember]
        public long UserID { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string CoverageType { get; set; }
        [DataMember]
        public string MarketOffDays { get; set; }
        [DataMember]
        public int StatusID { get; set; }
        
    }
}
