using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class OrderBookingSurveyDTO
    {
        [DataMember]
        public long OrderBookingID { get; set; }
        [DataMember]
        public int ProductTypeID { get; set; }
        [DataMember]
        public int ProductGroupID { get; set; }
        [DataMember]
        public Nullable<int> ProductCategoryID { get; set; }
        [DataMember]
        public long SurveyResponseID { get; set; }
        [DataMember]
        public string OrderNo { get; set; }
        [DataMember]
        public Nullable<int> ProductID { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public int StoreID { get; set; }
        [DataMember]
        public long UserID { get; set; }
        [DataMember]
        public long UserRoleID { get; set; }
        [DataMember]
        public Nullable<long> CoverageID { get; set; }
        [DataMember]
        public  byte OrderBookingType { get; set; }
    }
}
