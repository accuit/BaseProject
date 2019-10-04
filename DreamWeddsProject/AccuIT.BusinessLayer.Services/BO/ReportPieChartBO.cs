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
    public class ReportPieChartBO
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public List<PieChartDataStructure> Data { get; set; }
    }
    public class PieChartDataStructure
    {
        public string Name { get; set; }
        public int Value { get; set; }
        #region Changed for X-Axis Value by Dhiraj on 17-Dec-2013
        public int UserId{get;set; }
        #endregion
    }

}
