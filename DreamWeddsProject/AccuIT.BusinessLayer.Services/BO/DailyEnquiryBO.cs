using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
       [Serializable]
    public class DailyEnquiryBO
    {
        public long EnqID { get; set; }
        public int CaseID { get; set; }
        public System.DateTime EnqDate { get; set; }
        public int EmployeeId { get; set; }
        public string Deal { get; set; }
        public string UnqCustId { get; set; }
        public string Action { get; set; }
        public string Status { get; set; }
        public Nullable<decimal> Subtotal { get; set; }
        public Nullable<decimal> Freight { get; set; }
        public Nullable<decimal> Tax { get; set; }
        public Nullable<decimal> Payable { get; set; }
        public string PayTerms { get; set; }
        public string LeadTime { get; set; }
        public string Remarks { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool isDeleted { get; set; }
        public Nullable<int> CustomerID { get; set; }
    


    }
}
