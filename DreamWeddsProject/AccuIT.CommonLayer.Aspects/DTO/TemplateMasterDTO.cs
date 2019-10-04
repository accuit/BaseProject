using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    public class TemplateMasterDTO
    {
        [DataMember]
        public int TemplateID { get; set; }
        [DataMember]
        public string TemplateName { get; set; }
        [DataMember]
        public string TemplateTags { get; set; }
        [DataMember]
        public string TemplateUrl { get; set; }
        [DataMember]
        public string TemplateFolderPath { get; set; }
        [DataMember]
        public string ThumbnailImageUrl { get; set; }
        [DataMember]
        public string TemplateScreenshotUrl { get; set; }
        [DataMember]
        public string TemplatePreviewUrl { get; set; }
        [DataMember]
        public string TagLine { get; set; }
        [DataMember]
        public string UrlIdentifier { get; set; }
       // [DataMember]
       // public List<WeddingDTO> Weddings { get; set; }
        //[DataMember]
        //public string TemplateTypeText { get; set; }
        //[DataMember]
        //public string TemplateTagName { get; set; }
        //[DataMember]
        //public string TemplateFeatureText { get; set; }
    }
}
