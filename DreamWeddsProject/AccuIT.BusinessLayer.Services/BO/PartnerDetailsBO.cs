using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace AccuIT.BusinessLayer.Services.BO
{
   public class PartnerDetailsBO
    {

        
        public string StoreCode
        {
            get;
            set;
        }
        
        public string ShipToRegion
        {
            get;
            set;
        }

        
        public string ShipToBranch
        {
            get;
            set;
        }

        
        public string SoldToCode
        {
            get;
            set;
        }

        
        public string ShipToCode
        {
            get;
            set;
        }

        
        public string AccId
        {
            get;
            set;
        }


        
        public string StoreName
        {
            get;
            set;
        }


        
        public string AccountName
        {
            get;
            set;
        }


        
        public int StoreID
        {
            get;
            set;
        }

        
        public string City
        {
            get;
            set;
        }

        
        public string State
        {
            get;
            set;
        }


        public long UserID
        {
            get;
            set;
        }


        
        public string PartnerContactPerson
        {
            get;
            set;
        }
        
        public string PartnerMobileNo
        {
            get;
            set;
        }

        
        public string PartnerEmailID
        {
            get;
            set;
        }


        public DateTime? LastVisitDate
        {
            get;
            set;
        }

        public Nullable<decimal> AVMTDPurchase { get; set; }
        public Nullable<decimal> AVMTDSale { get; set; }
        public Nullable<decimal> MTDSellThru { get; set; }
        public Nullable<decimal> Target { get; set; }
        public Nullable<double> ACH { get; set; }
        public string VisitSummary { get; set; }
        
        public decimal HAMTDSale { get; set; }
       
        public decimal HAMTDPurchase { get; set; }
        
        public decimal ACMTDSale { get; set; }
        
        public decimal ACMTDPurchase { get; set; }
     
    }
}
