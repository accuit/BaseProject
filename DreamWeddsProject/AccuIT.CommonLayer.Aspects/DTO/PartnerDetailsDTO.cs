using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    public class PartnerDetailsDTO
    {

        [DataMember]
        public string StoreCode
        {
            get;
            set;
        }
        [DataMember]
        public string ShipToRegion
        {
            get;
            set;
        }

        [DataMember]
        public string ShipToBranch
        {
            get;
            set;
        }

        [DataMember]
        public string SoldToCode
        {
            get;
            set;
        }

        [DataMember]
        public string ShipToCode
        {
            get;
            set;
        }

        [DataMember]
        public string ShipToName
        {
            get;
            set;
        }

        [DataMember]
        public string AccId
        {
            get;
            set;
        }


        [DataMember]
        public string StoreName
        {
            get;
            set;
        }


        [DataMember]
        public string AccountName
        {
            get;
            set;
        }


        [DataMember]
        public int StoreID
        {
            get;
            set;
        }

        [DataMember]
        public string City
        {
            get;
            set;
        }

        [DataMember]
        public string State
        {
            get;
            set;
        }

        [DataMember]
        public long userID
        {
            get;
            set;
        }

        [DataMember]
        public string PartnerContactPerson
        {
            get;
            set;
        }
        [DataMember]
        public string PartnerMobileNo
        {
            get;
            set;
        }

        [DataMember]
        public string PartnerEmailID
        {
            get;
            set;
        }

        [DataMember]
        public string LastVisitedDate
        {
            get;
            set;
        }

        [DataMember]
        public Nullable<decimal> AVMTDPurchase { get; set; }
         [DataMember]
        public Nullable<decimal> AVMTDSale { get; set; }
         [DataMember]
        public Nullable<decimal> MTDSellThru { get; set; }
         [DataMember]
        public Nullable<decimal> Target { get; set; }
         [DataMember]
        public Nullable<double> ACH { get; set; }
         [DataMember]
         public string VisitSummary { get; set; }

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
