using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
   public  class GetVOCReportBO
    {
        public string Header { get; set; }
        public int? Month1 { get; set; }
        public int? Month2 { get; set; }
        public Nullable<int> Month1_Percentage { get; set; }
        public Nullable<int> Month2_Percentage { get; set; }
    }
}
