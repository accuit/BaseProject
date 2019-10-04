using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.ReportBO
{
    public class CollectionSurveyBO
    {
        public long CollectionSurveyID { get; set; }
        public long UserID { get; set; }
        public Nullable<long> CoverageID { get; set; }
        public int StoreID { get; set; }
        public long SurveyResponseID { get; set; }
        public long UserRoleID { get; set; }
        public int PaymentModeID { get; set; }
        public double Amount { get; set; }
        public string Comments { get; set; }
        public string TransactionID { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
