using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccuIT.CommonLayer.Aspects.ReportBO
{
    /// <summary>
    /// Business Object to fetch store details only
    /// </summary>
    public class StoreBO
    {
        public StoreBO()
        {
            IsGeoTagRequired = true;
        }
        public int StoreID { get; set; }

        public int CompanyID { get; set; }

        public string ShipToRegion { get; set; }

        public string ShipToBranch { get; set; }

        public string STATE { get; set; }

        public string SoldToCode { get; set; }

        public string ShipToCode { get; set; }

        public string AccId { get; set; }

        public string ChannelType { get; set; }

        public Nullable<int> PartnerID { get; set; }

        public Nullable<int> PartnerDetailID { get; set; }

        public string ContactPerson { get; set; }

        public string MobileNo { get; set; }

        public string EmailID { get; set; }

        public string StoreCode { get; set; }

        public string StoreName { get; set; }

        public string Dealer_Display
        {
            get
            {
                if (!string.IsNullOrEmpty(StoreCode))
                    return StoreName + " (" + StoreCode + ")";
                else
                    return string.Empty;
            }
        }

        public string AccountName { get; set; }

        public string ParentCode { get; set; }

        public string City { get; set; }

        public string ShipToName { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string ModifiedBy { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public long UserRoleID { get; set; }

        public string PictureFileName { get; set; }

        public int GeoTagCount { get; set; }

        public bool IsGeoTagRequired { get; set; }


        public decimal AVMTDSale { get; set; }
        public decimal AVMTDPurchase { get; set; }
        public decimal HAMTDSale { get; set; }
        public decimal HAMTDPurchase { get; set; }
        public decimal ACMTDSale { get; set; }
        public decimal ACMTDPurchase { get; set; }

        public decimal AVLMTDSale { get; set; }
        public decimal AVLMTDPurchase { get; set; }
        public decimal HALMTDSale { get; set; }
        public decimal HALMTDPurchase { get; set; }
        public decimal ACLMTDSale { get; set; }
        public decimal ACLMTDPurchase { get; set; }

        public decimal Target { get; set; }

        public double ACH { get; set; }

        public DateTime? LastVisitDate { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public Nullable<System.DateTime> LastGeoTagDate { get; set; }
        public virtual ICollection<StoreGeoTagBO> StoreGeoTags { get; set; }

        //VC20140925 to add fields for Freeze GeoTag
        public bool IsFreeze { get; set; }
        public double? FreezeLattitude { get; set; }
        public double? FreezeLongitude { get; set; }
        //VC20140925
        public string StoreAddress { get; set; }



        #region SDCE-684 New add Modifiy by Niranjan (Channel Type Status) 21-10-2014
        public bool IsDisplayCounterShare { get; set; }
        public bool IsPlanogram { get; set; }
        #endregion
    }
}
