using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    [Serializable]
    public class FeedbackCategoryMasterBO
    {
        public int FeedbackCatID { get; set; }
        public int TeamID { get; set; }
        public string FeedbackCategoryName { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public long ModifiedBy { get; set; }
    }
}
