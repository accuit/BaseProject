using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class TargetACHReportBO
    {
        public string Name { get; set; }
        public string ProfilePictureFileName { get; set; }
        public long UserID { get; set; }
        public int TargetCount { get; set; }
        public int ACHCount { get; set; }
        public int Percentage { get; set; }
    }
}
