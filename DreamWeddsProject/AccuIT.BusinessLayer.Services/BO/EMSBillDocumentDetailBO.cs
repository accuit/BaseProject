using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class EMSBillDocumentDetailBO
    {
        public int EMSBillDocumentDetailID { get; set; }

        public int EMSBillDetailID { get; set; }

        public string DocumentName { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public System.DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public System.DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }
    }
}
