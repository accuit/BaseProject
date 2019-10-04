using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    /// <summary>
    /// DTO class to get Store details
    /// </summary>
    [DataContract]
    public class StoreDTO
    {
        #region Old Fields

        //#region Primitive Properties

        //[DataMember]
        //public int StoreID { get; set; }
        //[DataMember]
        //public int CompanyID { get; set; }
        //[DataMember]
        //public string ShipToRegion { get; set; }
        //[DataMember]
        //public string ShipToBranch { get; set; }
        //[DataMember]
        //public string State { get; set; }
        //[DataMember]
        //public string SoldToCode { get; set; }
        //[DataMember]
        //public string ShipToCode { get; set; }
        //[DataMember]
        //public string AccId { get; set; }
        //[DataMember]
        //public string ChannelType { get; set; }
        //[DataMember]
        //public Nullable<int> PartnerID { get; set; }
        //[DataMember]
        //public Nullable<int> PartnerDetailID { get; set; }
        //[DataMember]
        //public string ContactPerson { get; set; }
        //[DataMember]
        //public string MobileNo { get; set; }
        //[DataMember]
        //public string EmailID { get; set; }
        //[DataMember]
        //public string StoreCode { get; set; }
        //[DataMember]
        //public string StoreName { get; set; }
        //[DataMember]
        //public string AccountName { get; set; }
        //[DataMember]
        //public string ParentCode { get; set; }
        //[DataMember]
        //public string City { get; set; }
        //[DataMember]
        //public string ShipToName { get; set; }
        //[DataMember]
        //public bool IsActive { get; set; }
        //[DataMember]
        //public bool IsDeleted { get; set; }

        //[DataMember]
        //public long UserRoleID { get; set; }
        //[DataMember]
        //public string PictureFileName { get; set; }

        ///// <summary>
        ///// MDT purchase
        ///// </summary>
        //[DataMember]
        //public decimal AVMTDPurchase { get; set; }

        ///// <summary>
        ///// MDT purchase
        ///// </summary>
        //[DataMember]
        //public decimal AVMTDSale { get; set; }

        //[DataMember]
        //public decimal HAMTDSale { get; set; }
        //[DataMember]
        //public decimal HAMTDPurchase { get; set; }
        //[DataMember]
        //public decimal ACMTDSale { get; set; }
        //[DataMember]
        //public decimal ACMTDPurchase { get; set; }

        //[DataMember]
        //public decimal AVLMTDSale { get; set; }
        //[DataMember]
        //public decimal AVLMTDPurchase { get; set; }
        //[DataMember]
        //public decimal HALMTDSale { get; set; }
        //[DataMember]
        //public decimal HALMTDPurchase { get; set; }
        //[DataMember]
        //public decimal ACLMTDSale { get; set; }
        //[DataMember]
        //public decimal ACLMTDPurchase { get; set; }

        ///// <summary>
        ///// MDT purchase
        ///// </summary>
        //[DataMember]
        //public decimal Target { get; set; }

        ///// <summary>
        ///// MDT purchase
        ///// </summary>
        //[DataMember]
        //public double ACH { get; set; }

        ///// <summary>
        ///// Property to determine geo tag required or not
        ///// </summary>
        //[DataMember]
        //public bool IsGeoTagRequired { get; set; }

        ///// <summary>
        ///// Property to determine geo tag count
        ///// </summary>
        //[DataMember]
        //public int GeoTagCount { get; set; }

        //[DataMember]
        //public string LastVisitedDate { get; set; }

        //[DataMember]
        //public string VisitSummary { get; set; }

        //#endregion


        ////VC20140925 to add fields for Freeze GeoTag
        //[DataMember]
        //public bool IsFreeze { get; set; }

        //[DataMember]
        //public double? FreezeLattitude { get; set; }

        //[DataMember]
        //public double? FreezeLongitude { get; set; }
        ////VC20140925
        //#region SDCE-684 New add Modifiy by Niranjan (Channel Type Status) 21-10-2014
        ///// <summary>
        ///// Property to determine Display Counter Share required or not
        ///// </summary>
        //[DataMember]
        //public bool IsDisplayCounterShare { get; set; }

        ///// <summary>
        ///// Property to determine IsPlanogram required or not
        ///// </summary>
        //[DataMember]
        //public bool IsPlanogram { get; set; }

        //#endregion
        #endregion

        #region New Fields added and old removed after Change Display Store Profile and Today Beat into SP [Amit Mishra]
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
        public bool IsGeoTagRequired { get; set; }
        [DataMember]
        public bool IsDisplayCounterShare { get; set; }
        [DataMember]
        public bool IsPlanogram { get; set; }
        [DataMember]
        public string LastVisitedDate { get; set; }
        [DataMember]
        public string VisitSummary { get; set; }
        [DataMember]
        public bool IsFreeze { get; set; }
        [DataMember]
        public Nullable<double> FreezeLattitude { get; set; }
        [DataMember]
        public Nullable<double> FreezeLongitude { get; set; }
        [DataMember]
        public decimal Target { get; set; }
        [DataMember]
        public double ACH { get; set; }
        [DataMember]
        public decimal ACMTDSale { get; set; }
        [DataMember]
        public decimal ACMTDPurchase { get; set; }
        [DataMember]
        public decimal AVMTDSale { get; set; }
        [DataMember]
        public decimal AVMTDPurchase { get; set; }
        [DataMember]
        public decimal HAMTDSale { get; set; }
        [DataMember]
        public decimal HAMTDPurchase { get; set; }
        [DataMember]
        public decimal ACLMTDSale { get; set; }
        [DataMember]
        public decimal ACLMTDPurchase { get; set; }
        [DataMember]
        public decimal AVLMTDSale { get; set; }
        [DataMember]
        public decimal AVLMTDPurchase { get; set; }
        [DataMember]
        public decimal HALMTDSale { get; set; }
        [DataMember]
        public string StoreClass { get; set; }
        [DataMember]
        public decimal HALMTDPurchase { get; set; }
        [DataMember]
        public long CoverageID { get; set; }
        //[DataMember]
        //public System.DateTime CoverageDate { get; set; }
        [DataMember]
        public bool IsCoverage { get; set; }
        [DataMember]
        public long UserRoleID { get; set; }
        [DataMember]
        public bool IsStoreProfileVisible { get; set; }

        [DataMember]
        public string StoreAddress { get; set; }
        #endregion
    }
}
