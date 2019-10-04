using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class RoleModuleQuestionBO
    {
        public int SurveyQuestionID { get; set; }
        public string Question { get; set; }
        public Nullable<int> RoleID { get; set; }
        public Nullable<bool> IsMandatory { get; set; }
        public string RecurrenceExpression { get; set; }
        public Nullable<int> ModuleID { get; set; }
        public bool IsSelected { get; set; }
    }
}
