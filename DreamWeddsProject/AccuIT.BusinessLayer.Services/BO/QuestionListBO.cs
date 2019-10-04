using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
  public class QuestionListBO
    {

        public int SurveyQuestionID { get; set; }
        public int CompanyID { get; set; }
        public string Question { get; set; }
        public int QuestionTypeID { get; set; } 
        public Nullable<int> ProductTypeID { get; set; }
        public Nullable<int> ProductGroupID { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> ModuleID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedBy { get; set; }
        
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDailyQuote { get; set; }
        public string QuestionType { get; set; }

        public Nullable<System.DateTime> QuoteDate { get; set; }
        public string HintImageName { get; set; }
        public bool IsMandatory { get; set; }
        public int Sequence { get; set; }
        public int TextLength { get; set; }
        public int DependentOptionID { get; set; }
        public string QuestionImage { get; set; }
        public int? RepeaterTypeID { get; set; }
        public string RepeaterText { get; set; }
        public int? RepeatMaxTimes { get; set; }
        public bool IsMendatory { get; set; }
        public Nullable<int> ProductGroupCategoryId { get; set; }
        public Nullable<long> ModifiedByUserID { get; set; }

    }
}
