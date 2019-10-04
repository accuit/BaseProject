using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{

    public class StoreGeoTagDTO
    {
        public int GeoTagID { get; set; }
        public int StoreID { get; set; }
        public long UserID { get; set; }
        public System.DateTime GeoTagDate { get; set; }
        public System.DateTime CreatedDate { get; set; }        
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public string PictureFileName { get; set; }     
    }

    public class AuditSummarySearchDTO
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
        public Nullable<System.DateTime> AssessmentStartTime { get; set; }
        public Nullable<System.DateTime> AssessmentEndTime { get; set; }
    }

    public class AuditSearchDTO
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
    public class ProductAuditSummaryDTO
    {
        public int ProductID { get; set; }
        public string ProductType { get; set; }
        public string ProductGroup { get; set; }
        public string ProductCategory { get; set; }
        public string ModelName { get; set; }
        public Nullable<byte> WallNumber { get; set; }
        public Nullable<byte> RowNumber { get; set; }
        public string POSMCategory { get; set; }
        public string POSMCode { get; set; }
        public bool Topper { get; set; }
        public bool SwitchedOn { get; set; }
        public bool PriceTag { get; set; }
        public string DisplayArea { get; set; }
        public string DisplaySubArea { get; set; }
        public long StockAuditResponseID { get; set; }
        public int POSMID { get; set; }
        public string BrandName { get; set; }
        public string ProductSize { get; set; }
        public string WallBrandName { get; set; }
    }

    public class ProductAuditDTO
    {
        public int AuditID { get; set; }
    }

    public class AuditDetailsDTO
    {
        public List<ProductAuditSummaryDTO> productAuditSummary { get; set; }
        public StoreGeoTagDTO tagDetails { get; set; }
        public List<AuditQuestionDetailsDTO> auditDetails { get; set; }
        public List<AuditLogDetailsDTO> auditLogDetails { get; set; }
    }

    public class AuditQuestionDetailsDTO
    {
        public string Question { get; set; }
        public Nullable<int> QuestionTypeID { get; set; }
        public string UserResponse { get; set; }
        public int ModuleID { get; set; }
        public string NAME { get; set; }
    }
    public class SurveyAuditDTO
    {
        public List<SurveyModulesDTO> modules { get; set; }

        public List<AuditLogDetailsDTO> auditLogDetails { get; set; }
    }

    public class AuditLogDetailsDTO
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

    public class SurveyModulesDTO
    {

        public int SurveyResponseID { get; set; }
        public int ModuleCode { get; set; }
        public string ModuleName { get; set; }
        public int ModuleID { get; set; }
        public int AuditID { get; set; }

        public List<SurveyQuestionResponseDTO> surveyQuestionResponse { get; set; }
        public List<SurveyQuestionRepeatResponseDTO> surveyRepeatResponse { get; set; }
    }

    public class SurveyQuestionResponseDTO
    {

        public int ModuleID { get; set; }
        public int ModuleCode { get; set; }
        public string surveyQuestion { get; set; }
        public string RepeaterText { get; set; }

        public SurveyAuditUserResponseDTO surveyUserResponse { get; set; }

    }

    public class SurveyAuditUserResponseDTO
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
        public virtual ICollection<SurveyRepeatResponseDTO> SurveyRepeatResponses { get; set; }     
    }
  
    public class SurveyQuestionRepeatResponseDTO
    {
        public int ModuleID { get; set; }
        public int ModuleCode { get; set; }
        public string surveyQuestion { get; set; }
        public string userResponse { get; set; }
        public string imagePath { get; set; }
        public Nullable<int> SurveyQuestionRepeaterID { get; set; }
        
    }

        public class ReviewerResponseDTO
        {
            public int AuditLogID { get; set; }
            public int AuditID { get; set; }
            public long ReviewBy { get; set; }
            public string Remarks { get; set; }
            public byte Status { get; set; }
            public long userID { get; set; }
            public int RoleID { get; set; }  
        }

        public class GeoDefinitionDTO
        {
            public int GeoDefinitionID { get; set; }
            public string GeoDefCode { get; set; }
            public string GeoDefName { get; set; }
            public int GeoID { get; set; }
            public int ParentGeoDefinitionId { get; set; }
        }

        public class QCLoginResponseDTO
        {
            public string APIKey
            {
                get;
                set;
            }


            public string APIToken
            {
                get;
                set;
            }
            public long loginStatus { get; set; }

        }
}
