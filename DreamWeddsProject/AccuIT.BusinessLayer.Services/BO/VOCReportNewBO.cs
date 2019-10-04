using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
   public class VOCReportNewBO
    {
        public string Header { get; set; }
        public string Question { get; set; }
        public string Response { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public long? CEVOCResponseID { get; set; }
    }
}
