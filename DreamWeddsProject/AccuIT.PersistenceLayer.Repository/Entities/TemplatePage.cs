namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TemplatePage
    {
        [Key]
        public int PageID { get; set; }

        [Required]
        [StringLength(150)]
        public string PageName { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        [Required]
        public string PageContent { get; set; }

        [StringLength(500)]
        public string PageUrl { get; set; }

        [Required]
        [StringLength(500)]
        public string PageFolderPath { get; set; }

        [StringLength(500)]
        public string ThumbnailImageUrl { get; set; }

        public int TemplateID { get; set; }

        public int CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual TemplateMaster TemplateMaster { get; set; }

        public virtual TemplateMaster TemplateMaster1 { get; set; }
    }
}
