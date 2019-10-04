using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class SOSFMasterBO
    {
        public long SOSFId { get; set; }
        public System.DateTime SaleYear { get; set; }
        public Nullable<System.DateTime> SaleMonth { get; set; }
        public Nullable<int> SaleWeek { get; set; }
        public Nullable<System.DateTime> SaleDate { get; set; }
        public decimal SaleAmount { get; set; }
        public Nullable<decimal> Percentage { get; set; }
        public Nullable<decimal> Forecast { get; set; }
        public Nullable<int> StoreID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int CompanyID { get; set; }
    }

    [DataContract]
    public class ParkingSOSFMasterBO
    {
        public int SaleYear { get; set; }
        public Nullable<int> SaleMonth { get; set; }
        public Nullable<int> SaleWeek { get; set; }
        public Nullable<System.DateTime> SaleDate { get; set; }
        public decimal SaleAmount { get; set; }
        public Nullable<int> Percentage { get; set; }
        public Nullable<decimal> Forecast { get; set; }
        public string StoreCode { get; set; }
        public string SKUCode { get; set; }
        public int CompanyID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }

    }

}
