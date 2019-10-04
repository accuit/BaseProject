using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class AuditSummarySearchBO
    {
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string ShipToRegion { get; set; }
        public string ChannelType { get; set; }
        public string STATE { get; set; }
        public string City { get; set; }
        public Nullable<int> AuditMonth { get; set; }
        public string AuditorName { get; set; }
        public string Supervisor { get; set; }
        public string UserLocation { get; set; }
        public string CurrentStatus { get; set; }
        public string SupStatus { get; set; }
        public string QC1Status { get; set; }
        public string Qc2Status { get; set; }
        public string QC1Comments { get; set; }
        public string QC2Comments { get; set; }
        public long UserID { get; set; }
        public string EmplCode { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public System.DateTime AssesmentDate { get; set; }
        public string StoreAddress { get; set; }
        public string StartStatus { get; set; }
        public long SurveyResponseID { get; set; }
        public int AuditID { get; set; }
    }

    public class SurveyAuditBO
    {
        public List<SurveyModuleBO> modules { get; set; }

        public List<AuditLogDetailsBO> auditLogDetails { get; set; }
    }

    public class AuditLogDetailsBO
    {
        public int AuditLogID { get; set; }
        public int AuditID { get; set; }
        public long ReviewBy { get; set; }
        public string ReviewByName { get; set; }
        public string Remarks { get; set; }
        public byte Status { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public string Days { get; set; }
    }

    public class SurveyModuleBO
    {
        public SurveyModuleBO()
        {
            surveyQuestionResponse = new List<SurveyQuestionResponseBO>();
        }

        public int SurveyResponseID { get; set; }
        public int ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public int ModuleID { get; set; }
        public int AuditID { get; set; }

        public List<SurveyQuestionResponseBO> surveyQuestionResponse { get; set; }
        public List<SurveyQuestionRepeatResponseBO> surveyRepeatResponse { get; set; }
    }

    public class SurveyQuestionRepeatResponseBO
    {
        public int ModuleID { get; set; }
        public int ModuleCode { get; set; }
        public string surveyQuestion { get; set; }
        public string userResponse { get; set; }
        public string imagePath { get; set; }
        public Nullable<int> SurveyQuestionRepeaterID { get; set; }        
    }

    public class SurveyQuestionResponseBO
    {
        public int ModuleID { get; set; }
        public int ModuleCode { get; set; }
        public string surveyQuestion { get; set; }
        public string RepeaterText { get; set; }

        public SurveyUserResponseBO surveyUserResponse { get; set; }
    }

    public class SurveyUserResponseBO
    {
        public long SurveyUserResponseID { get; set; }
        public long SurveyResponseID { get; set; }
        public int SurveyQuestionID { get; set; }
        public string UserResponse { get; set; }
        public int SurveyTypeID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<int> SurveyOptionID { get; set; }
        public virtual ICollection<SurveyRepeatResponseBO> SurveyRepeatResponses { get; set; }
    }

    public partial class SurveyRepeatResponseBO
    {
        public long SurveyRepeatResponseID { get; set; }
        public Nullable<long> SurveyUserResponseID { get; set; }
        public string UserResponse { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public Nullable<int> SurveyQuestionRepeaterID { get; set; }

    }

    public class AuditSearchBO
    {
        public DateTime? Assessment_From_Date { get; set; }
        public DateTime? Assessment_To_Date { get; set; }
        
        public Nullable<DateTime> QClevel1datefrom { get; set; }
        public DateTime? QClevel1dateto { get; set; }
        public string QC1Review { get; set; }

        public DateTime? QClevel2DateFrom { get; set; }
        public DateTime? QClevel2DateTo { get; set; }
        public string QC2Review { get; set; }

        public DateTime? SupdateFrom { get; set; }
        public DateTime? SupdateTo { get; set; }
        public string SupReview { get; set; }

        public Boolean advanceFilterRequired { get; set; }
        public string storeCode { get; set; }
        public int? qC1Status { get; set; }
        public int? qC2Status { get; set; }
        public int? supStatus { get; set; }
        public int? currentStatus { get; set; }

        public string supervisiorreview { get; set; }
        public string QCLevel1 { get; set; }
        public string QCLevel2 { get; set; }
        public string Unit { get; set; }
        public string Region { get; set; }
    }
}
