namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TemplateImage
    {
        [Key]
        public int ImageID { get; set; }

        [Required]
        [StringLength(150)]
        public string ImageName { get; set; }

        [StringLength(250)]
        public string ImageTitle { get; set; }

        public string ImageTagLine { get; set; }

        [StringLength(1000)]
        public string ImageUrl { get; set; }

        [Required]
        [StringLength(1000)]
        public string ImageFolderPath { get; set; }

        public bool IsBannerImage { get; set; }

        public int TemplateID { get; set; }

        public int CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ImageType { get; set; }

        public virtual TemplateMaster TemplateMaster { get; set; }

        public virtual TemplateMaster TemplateMaster1 { get; set; }
    }
}
