using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    /// <summary>
    /// This class is used to define the response data for Dealer list /business methods.
    /// </summary>


    public class SyncDealerListDTO
    {
        [DataMember]
        public bool HasMoreRows;
        [DataMember]
        public string MaxModifiedDate;

        [DataMember]
        public List<DealerListDTO> Result;
    }
    public class DealerListDTO
    {
        //[DataMember]
        //public long DealerID
        //{
        //    get;
        //    set;
        //}
        //[DataMember]
        //public string DealerName
        //{
        //    get;
        //    set;
        //}
        //[DataMember]
        //public string StoreCode
        //{
        //    get;
        //    set;
        //}

        //[DataMember]
        //public string City
        //{
        //    get;
        //    set;
        //}


        [DataMember]
        public int StoreID { get; set; }
        [DataMember]
        public string ChannelType { get; set; }
        [DataMember]
        public string StoreSize { get; set; }
        [DataMember]
        public string ContactPerson { get; set; }
        [DataMember]
        public string MobileNo { get; set; }
        [DataMember]
        public string EmailID { get; set; }
        [DataMember]
        public string StoreCode { get; set; }
        [DataMember]
        public string StoreName { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string PictureFileName { get; set; }
        [DataMember]
        public bool IsDisplayCounterShare { get; set; }
        [DataMember]
        public bool IsPlanogram { get; set; }
        [DataMember]
        public string StoreClass { get; set; }
        [DataMember]
        public Nullable<bool> IsFreeze { get; set; }
        [DataMember]
        public Nullable<double> FreezeLattitude { get; set; }
        [DataMember]
        public Nullable<double> FreezeLongitude { get; set; }
        [DataMember]
        public string StoreAddress { get; set; }
        [DataMember]
        public Nullable<bool> IsDeleted { get; set; }
    }
}
