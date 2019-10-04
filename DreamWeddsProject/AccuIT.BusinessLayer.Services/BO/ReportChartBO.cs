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
    public class ReportChartBO
    {
        public string Title { get; set; }
        public string ReportText { get; set; }
        public string SubTitle { get; set; }
        public bool IsLastLevel { get; set; }
        public List<string> XAxisValues { get; set; }//Used for stack dril
        public List<XAxisDataStructure> XAxisHiddenValues { get; set; }//Used for Stack Dril with Axis as employee
        public List<ChartDataStructure> Data { get; set; }
        public ReportChartBO(string Title, string ReportText, string SubTitle)
        {
            this.Title=Title;
            this.ReportText=ReportText;
            this.SubTitle = SubTitle;
        }
        public ReportChartBO()
        {

        }
    }
    public class ActivityQuestionsBO
    {
        private int questionID;
        private string questionText;

        public string QuestionText
        {
            get { return questionText; }
            set { questionText = value; }
        }

        public int QuestionID
        {
            get { return questionID; }
            set { questionID = value; }
        }
    }
    public class ChartDataStructure
    {
        public string Name { get; set; }
        public double Value { get; set; }        
        public List<double> Values { get; set; }//Used for stack dril
        public long UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string ProfilePictureFileName { get; set; }
        public string Mobile_Calling { get; set; }
        public string GeoDefName { get; set; }
        public int StoreCount { get; set; }
        public List<string> StoresCount { get; set; }
    }
    public class XAxisDataStructure
    {               
        public long UserId { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string UserName { get; set; }
        public string ProfilePictureFileName { get; set; }
        public string Mobile_Calling { get; set; }
        public string GeoDefName { get; set; }
        public double NoOFStores { get; set; }
        public string BrandNames { get; set; }
        public string ProductNames { get; set; }
    }

}
