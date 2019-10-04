using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
   public class TotalCoverageExportBO
    {
        public string UserCode { get; set; }
        public string Mobile_Calling { get; set; }
        public int AccountStatus { get; set; }
        public long UserID { get; set; }
        public string Designation { get; set; }
        public string UserName { get; set; }
        public int UserStoreCount { get; set; }
        public int TotalStoreCount { get; set; }
        public int Percentage { get; set; }
        public string ProfilePictureFileName { get; set; }
        public string Date { get; set; }
        public string Region { get; set; }
        public string Branch { get; set; }
        public int POSMCount { get; set; }
        public int CompCount { get; set; }
        public int OrderCount { get; set; }
        public int HygeineCount { get; set; }
        public int CollectionCount { get; set; }
        public int GeoTagCount { get; set; }
        public string Disty { get; set; }
        public int DealerBoards { get; set; }
        public int UniqueOlCount { get; set; }

    }
}
