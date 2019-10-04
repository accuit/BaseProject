using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    /// <summary>
    /// This class is used to define the response data for Dealer list based on city name.
    /// </summary>
   public class DishCompReportBO
    {     
        public string CompetitorName
        {
            get;
            set;
        }
        public string SecondLevelColumn
        {
            get;
            set;
        }
        public double Value
        {
            get;
            set;
        }
        public double Stores
        {
            get;
            set;
        }
        public string Region
        {
            get;
            set;
        }
        public string Branch
        {
            get;
            set;
        }
      
      
        
    }
}
