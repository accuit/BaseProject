using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    /// <summary>
    /// DTO class to get User Stores List For Today's Beat
    /// </summary>
    public class UserStoresTodayBeatDTO
    {
        [DataMember]
        public string StoreCode { get; set; }
        [DataMember]
        public string ShipToRegion { get; set; }
        [DataMember]
        public string ShipToBranch { get; set; }
        [DataMember]
        public string SoldToCode { get; set; }
        [DataMember]
        public string ShipToCode { get; set; }
        [DataMember]
        public string AccId { get; set; }
        [DataMember]
        public string StoreName { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public int StoreID { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string ContactPerson { get; set; }
        [DataMember]
        public string MobileNo { get; set; }
        [DataMember]
        public string EmailID { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public long UserID { get; set; }
        [DataMember]
        public long UserRoleID { get; set; }
        [DataMember]
        public long StoreUserID { get; set; }
        [DataMember]
        public Nullable<System.DateTime> LastVisitDate { get; set; }
        [DataMember]
        public System.DateTime CoverageDate { get; set; }
         [DataMember]
        public long CoverageID { get; set; }
         [DataMember]
         public bool IsCoverage { get; set; }

         [DataMember]
         public decimal AVMTDPurchase { get; set; }
         /// <summary>
         /// MDT purchase
         /// </summary>
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

         [DataMember]
         public decimal AVLMTDSale { get; set; }
         [DataMember]
         public decimal AVLMTDPurchase { get; set; }
         [DataMember]
         public decimal HALMTDSale { get; set; }
         [DataMember]
         public decimal HALMTDPurchase { get; set; }
         [DataMember]
         public decimal ACLMTDSale { get; set; }
         [DataMember]
         public decimal ACLMTDPurchase { get; set; }
         /// <summary>
         /// MDT purchase
         /// </summary>
         [DataMember]
         public decimal Target { get; set; }

         /// <summary>
         /// MDT purchase
         /// </summary>
         [DataMember]
         public double ACH { get; set; }

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
         public string LastVisitedDate { get; set; }

         [DataMember]
         public string VisitSummary { get; set; }

         [DataMember]
         public string PictureFileName { get; set; }

         [DataMember]
         public bool IsCovered { get; set; }

         //Added by vaishali for SDCE-576(Send Frozen Geo tag information into APK) :: VC20141010
         [DataMember]
         public bool IsFreeze { get; set; }

         [DataMember]
         public double? FreezeLattitude { get; set; }

         [DataMember]
         public double? FreezeLongitude { get; set; }
        //VC20141010

    }
}
