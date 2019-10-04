using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    [Serializable]
    public class ExpenseTypeMasterBO
    {
        public int EMSExpenseTypeMasterId { get; set; }
        
        public string Name { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public int CompanyId { get; set; }
        //public DateTime CreatedDate { get; set; }
        //public int CreatedBy { get; set; }
        public int Sequence { get; set; }
    
    }
}
