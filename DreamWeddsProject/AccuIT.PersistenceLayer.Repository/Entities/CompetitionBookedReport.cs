using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.PersistenceLayer.Repository.Entities
{
  public  class CompetitionBookedReport
    {
        public long UserID
        {
            get;
            set;
        }
        public string PersonName
        {
            get;
            set;
        }

        public int Percentage
        {
            get;
            set;
        }

        public int CompetitionBookedCount
        {
            get;
            set;
        }
        public int TotalCompetitionCount
        {
            get;
            set;
        }
        public string ProfilePictureFileName 
        { 
            get; 
            set; 
        }

    }
}
