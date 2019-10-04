using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.ReportBO
{
    public class CompetitionSurveyResponseBO
    {
        public string userType { get; set; }
        public string userCode { get; set; }
        public long UserID { get; set; }
        public string userName { get; set; }
        public string userMobile { get; set; }
        public string AccountName { get; set; }
        public string Disty { get; set; }
        public string ShipToBranch { get; set; }
        public string prd_Category { get; set; }
        public string Brand { get; set; }
        public string Question { get; set; }
        public int UserResponse { get; set; }
        public string ShipToRegion { get; set; }
        public string STATE { get; set; }
        public string City { get; set; }
        public Nullable<System.DateTime> SurveyDate { get; set; }
        public byte CompetitionType { get; set; }
        public long SurveyResponseID { get; set; }
        public string ChannelType { get; set; }
        public Nullable<int> SurveyQuestionID { get; set; }
        public int StoreID { get; set; }
        public string StoreName { get; set; }
        public string StoreCode { get; set; }
    }
}
