using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    /// <summary>
    /// This class is used to provide data for PIE Chart
    /// </summary>
    public class ReportBarChartBO
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string ReportText { get; set; }
        #region Changed for X-Axis Value by Dhiraj on 17-Dec-2013
        public List<BarChartDataXAxisCategory> XAxisCategory { get; set; }
        #endregion
        public List<BarChartDataStructure> Data { get; set; }
    }
    public class BarChartDataStructure
    {
        public string Name { get; set; }
        public List<int> Value { get; set; }
    }
    #region Changed for X-Axis Value by Dhiraj on 17-Dec-2013              
    public class BarChartDataXAxisCategory
    {
        public string Name { get; set; }
        public long UserId { get; set; }
    }
    #endregion

   

}
