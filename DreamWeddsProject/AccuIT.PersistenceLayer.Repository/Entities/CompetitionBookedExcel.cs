using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.PersistenceLayer.Repository.Entities
{
   public class CompetitionBookedExcel
    {
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string Mobile_Calling { get; set; }
        public int AccountStatus { get; set; }
        public long UserID { get; set; }
        public string Designation { get; set; }
        public int UserCompetitionCount { get; set; }
        public int TotalStoreCount { get; set; }
        public int Percentage { get; set; }
        public string Date { get; set; }
    }
}
