using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Samsung.SmartDost.PersistenceLayer.Repository.Entities
{
    public class EMSExpense
    {
        public int EMSExpenseDetailID { get; set; }

        public int EMSExpenseTypeMasterID { get; set; }

        public bool Billable { get; set; }

        public string BillableTo { get; set; }
   
        public string Comment { get; set; }

        public byte Status { get; set; }

        public int CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public List<EmsBill> EMSBillDetails { get; set; }
    }
}
