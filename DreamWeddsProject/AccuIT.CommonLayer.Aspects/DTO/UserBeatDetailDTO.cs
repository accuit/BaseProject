using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class UserBeatDetailDTO
    {
        [DataMember]
        public string CoverageDate { get; set; }

        [DataMember]
        public List<UserBeatStoreDetailDTO> StoreData { get; set; }
    }
    [DataContract]
    public class UserBeatStoreDetailDTO
    {
        [DataMember]
        public int StoreID { get; set; }
        [DataMember]
        public string StoreCode { get; set; }
        [DataMember]
        public string StoreName { get; set; }
        [DataMember]
        public string City { get; set; }
        //[DataMember]
        //public int StatusID { get; set; }
        //[DataMember]
        //public string DateRange { get; set; }
        //[DataMember]
        //public string PlanMonth { get; set; }
        //[DataMember]
        //public long UserID { get; set; }
        ////[DataMember]
        ////public string TotalWorkingDays { get; set; }
        ////[DataMember]
        ////public string TotalOutletPlanned { get; set; }
        ////[DataMember]
        ////public string TotalOff { get; set; }
        //[DataMember]
        //public string Day { get; set; }
    }
}
