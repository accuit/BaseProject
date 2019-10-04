using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AccuIT.BusinessLayer.Services.BO
{
    /// <summary>
    /// List of GCMids 
    /// </summary>
    class GCMRegistrationIDs
    {        
        public string CoverageNotificationServiceID { get; set; }
        public string AndroidID { get; set; }
        public string UserID { get; set; }                
    }

    [DataContract]
    class GCmResponse
    {
        [DataMember(Name = "multicast_id")]
        public string multicast_id { get; set; }
        [DataMember(Name = "success")]
        public string success { get; set; }
        [DataMember(Name = "failure")]
        public string failure { get; set; }
        [DataMember(Name = "canonical_ids")]
        public string canonical_ids { get; set; }
        [DataMember(Name = "results")]
        public IList<GCMresult> results { get; set; }
    }

    [DataContract]
    class GCMresult
    {
        [DataMember(Name = "registration_id")]
        public string registration_id { get; set; }
        [DataMember(Name = "error")]
        public string error { get; set; }
        [DataMember(Name = "message_id")]
        public string message_id { get; set; }
    }
}
