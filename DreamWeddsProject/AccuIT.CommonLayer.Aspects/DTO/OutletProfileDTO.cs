using System;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class OutletProfileDTO
    {
        /// <summary>
        /// User role ID
        /// </summary>
        [DataMember]
        public long UserRoleID { get; set; }

        /// <summary>
        /// Role ID
        /// </summary>
        [DataMember]
        public int RoleID { get; set; }

        /// <summary>
        /// Outlet name
        /// </summary>
        [DataMember]
        public string OutletName { get; set; }

        /// <summary>
        /// Outlet code
        /// </summary>
        [DataMember]
        public string OutletCode { get; set; }

        /// <summary>
        /// MDT purchase
        /// </summary>
        

        /// <summary>
        /// MDT purchase
        /// </summary>
        [DataMember]
        public decimal Target { get; set; }

        /// <summary>
        /// MDT purchase
        /// </summary>
        [DataMember]
        public string ACH { get; set; }

        /// <summary>
        /// Property to determine geo tag required or not
        /// </summary>
        [DataMember]
        public bool IsGeoTagRequired { get; set; }

        /// <summary>
        /// Property to determine geo tag count
        /// </summary>
        [DataMember]
        public int GeoTagCount { get; set; }
        [DataMember]
        public decimal AVMTDPurchase { get; set; }
        [DataMember]
        public decimal AVMTDSale { get; set; }

        [DataMember]
        public decimal HAMTDSale { get; set; }
        [DataMember]
        public decimal HAMTDPurchase { get; set; }
        [DataMember]
        public decimal ACMTDSale { get; set; }
        [DataMember]
        public decimal ACMTDPurchase { get; set; }
    }
}
