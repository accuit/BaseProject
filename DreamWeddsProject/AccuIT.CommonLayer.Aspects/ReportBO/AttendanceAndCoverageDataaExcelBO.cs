using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.ReportBO
{
   public class AttendanceAndCoverageDataaExcelBO
    {
        public int CovergeExportID { get; set; }
        public System.DateTime Date { get; set; }
        public string Region { get; set; }
        public string Branch { get; set; }
        public string Disty { get; set; }
        public string UserType { get; set; }
        public string UserCode { get; set; }
        public Nullable<int> userid { get; set; }
        public Nullable<int> roleid { get; set; }
        public string UserName { get; set; }
        public int TotalOutletAssigned { get; set; }
        public string Attendance { get; set; }
        public int NormTargetForDay { get; set; }
        public int VisitsPlanned { get; set; }
        public Nullable<int> TotalVisitsDoneSPP { get; set; }
        public Nullable<int> TotalVisitsDoneNonSPP { get; set; }
        public int TotalVisitsDone { get; set; }
        public int VisitsAgainstPlanSPP { get; set; }
        public Nullable<int> VisitsAgainstPlanNONSPP { get; set; }
        public Nullable<int> VisitsAgainstPlanTotal { get; set; }
        public int MTDUniqueCoverageSPP { get; set; }
        public Nullable<int> MTDUniqueCoverageNONSPP { get; set; }
        public Nullable<int> MTDUniqueCoverageTotal { get; set; }
        public Nullable<System.DateTime> LastLoginTime { get; set; }
        public Nullable<System.DateTime> attendanceDate { get; set; }
    }
}
