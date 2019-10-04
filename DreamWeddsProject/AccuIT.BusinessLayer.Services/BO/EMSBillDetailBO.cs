using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class EMSBillDetailBO
    {
        public int EMSBillDetailID { get; set; }

        public int EMSExpenseDetailID { get; set; }

        //public System.DateTime BillDate { get; set; }

        public string BillDate { get; set; }

        public string BillNo { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public System.DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public List<EMSBillDocumentDetailBO> EMSBillDocumentDetails { get; set; }


    }
}
