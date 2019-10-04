using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
  public  class AnswerListBO
    {
        public int SurveyOptionID { get; set; }
        public int SurveyQuestionID { get; set; }
        public string OptionValue { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public int Sequence { get; set; }
        public bool IsAffirmative { get; set; }// Manoranjan 
    }
}
