using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SOSFMasterDTO
    {
        [DataMember]
        public long SOSFId { get; set; }
        [DataMember]
        public int SaleYear { get; set; }
        [DataMember]
        public Nullable<int> SaleMonth { get; set; }
        [DataMember]
        public Nullable<int> SaleWeek { get; set; }
        [DataMember]
        public string SaleDate { get; set; }
        [DataMember]
        public decimal SaleAmount { get; set; }
        [DataMember]
        public decimal? Percentage { get; set; }
        [DataMember]
        public Nullable<decimal> Forecast { get; set; }
        [DataMember]
        public string StoreCode { get; set; }
        [DataMember]
        public string SKUCode { get; set; }
        [DataMember]
        public int CompanyID { get; set; }
        [DataMember]
        public string CreatedDate { get; set; }
        [DataMember]
        public long CreatedBy { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public string ModifiedDate { get; set; }
        [DataMember]
        public Nullable<long> ModifiedBy { get; set; }
       
    }

    [DataContract]
    public class SOSFOutPutDTO
    {
        [DataMember]
        public int ReqType { get; set; } // Year: 1, Month: 2, Week: 3
        [DataMember]
        public List<SOSFMasterDTO> SOSFList { get; set; }

    }


}
