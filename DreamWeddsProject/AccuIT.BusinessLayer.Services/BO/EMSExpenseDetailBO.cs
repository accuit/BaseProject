using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class EMSExpenseDetailBO
    {
        public int EMSExpenseDetailID { get; set; }

        public int EMSExpenseTypeMasterID { get; set; }

        public bool Billable { get; set; }

        public string BillableTo { get; set; }

        public string Comment { get; set; }

        public byte Status { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public string CreatedDateStr { get; set; }

        public int CreatedBy { get; set; }

        public System.DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public List<EMSBillDetailBO> EMSBillDetails { get; set; }
    }
}
