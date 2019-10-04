using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    [Serializable]
    public class FeedbackTypeMasterBO
    {
        public int FeedbackTypeID { get; set; }
        public int FeedbackCatID { get; set; }
        public string FeedbackTypeName { get; set; }
        public int TAT { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<DateTime> ModifiedOn { get; set; }
        public string SampleImageName { get; set; }

        public Nullable<long> ModifiedBy { get; set; }
    }
}
