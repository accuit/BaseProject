using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    [Serializable]
    public class TemplateMasterBO
    {
        public int TemplateID { get; set; }
        public int UserID { get; set; }
        public string TemplateName { get; set; }
        public int TemplateType { get; set; }
        public int TemplateStatus { get; set; }
        public int TemplateCode { get; set; }
        public string TemplateContent { get; set; }
        public int CREATED_BY { get; set; }
        public System.DateTime Created_Date { get; set; }
        public Nullable<int> Modified_BY { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public string TemplateSubject { get; set; }
        public string TemplateTags { get; set; }
        public string TemplateUrl { get; set; }
        public string TemplateFolderPath { get; set; }
        public string ThumbnailImageUrl { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string TemplateScreenshotUrl { get; set; }
        public string TemplatePreviewUrl { get; set; }
        public string TagLine { get; set; }
        public Nullable<int> COST { get; set; }
        public string AuthorName { get; set; }
        public string AboutTemplate { get; set; }
        public string Features { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsTrial { get; set; }

        public string UrlIdentifier { get; set; }
        public string TemplateTypeText { get; set; }
        public string TemplateTagName { get; set; }
        public string TemplateFeatureText { get; set; }
        public List<WeddingBO> Weddings { get; set; }
        public virtual List<TemplateImageBO> templateImages { get; set; }
        public virtual List<TemplatePageBO> templatePages { get; set; }

    }

    [Serializable]
    public class TemplateImageBO
    {
        public int ImageID { get; set; }
        public string ImageName { get; set; }
        public string ImageTitle { get; set; }
        public string ImageTagLine { get; set; }
        public string ImageUrl { get; set; }
        public string ImageFolderPath { get; set; }
        public bool IsBannerImage { get; set; }
        public int TemplateID { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ImageType { get; set; }

    }

    [Serializable]
    public class TemplatePageBO
    {
        public int PageID { get; set; }
        public string PageName { get; set; }
        public string Title { get; set; }
        public string PageContent { get; set; }
        public string PageUrl { get; set; }
        public string PageFolderPath { get; set; }
        public string ThumbnailImageUrl { get; set; }
        public int TemplateID { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

    }

    public class TemplateMergeFieldBO
    {
        public int PK_ID { get; set; }
        public string MERGEFIELD_NAME { get; set; }
        public string SRC_FIELD { get; set; }
        public string SRC_FIELD_VALUE { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> TemplateID { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

       // public virtual TemplateMasterBO TemplateMaster { get; set; }
    }
}
