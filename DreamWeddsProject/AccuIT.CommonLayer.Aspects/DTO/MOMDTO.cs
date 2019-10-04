using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;


namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class MOMDTO
    {
        [DataMember]
        public long MOMId { get; set; }
        [DataMember]
        public long UserId { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string ActionItem { get; set; }
        [DataMember]
        public string MOMTitle { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public DateTime MomDate { get; set; }
        [DataMember]
        public string MomDateStr { get; set; }  
        [DataMember]
        public int MOMIdServer { get; set; }
        [DataMember]
        public int MOMIdClient { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }

        [DataMember]
        public List<MOMAttendeeDTO> MOMAttendees;
    }
    [DataContract]
    public class MOMOutputDTO
    {
        [DataMember]
        public long MOMIdServer { get; set; }
        [DataMember]
        public int MOMIdClient { get; set; }
    }
}
