using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.ReportBO
{
   public class OrderBookingSurveyBO
    {
        public long OrderBookingID { get; set; }
        public int ProductTypeID { get; set; }
        public int ProductGroupID { get; set; }
        public Nullable<int> ProductCategoryID { get; set; }
        public long SurveyResponseID { get; set; }
        public string OrderNo { get; set; }
        public Nullable<int> ProductID { get; set; }
        public int Quantity { get; set; }
        public int StoreID { get; set; }
        public long UserID { get; set; }
        public long UserRoleID { get; set; }
        public Nullable<long> CoverageID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int SyncStatus { get; set; }
    }
}
