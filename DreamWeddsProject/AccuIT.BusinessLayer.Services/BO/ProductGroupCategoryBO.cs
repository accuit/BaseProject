using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    [Serializable]
    public class ProductGroupCategoryBO
    {
        public int ProductGroupCategoryId { get; set; }
        public string ProductGroupCategoryName { get; set; }
        public string ProductGroupCategoryDescription { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public Nullable<int> ModifyBy { get; set; }
        public bool IsDeleted { get; set; }
        public int Sequence { get; set; }
    }
}
