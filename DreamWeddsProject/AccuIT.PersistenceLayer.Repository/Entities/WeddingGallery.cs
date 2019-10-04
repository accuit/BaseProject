namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WeddingGallery")]
    public partial class WeddingGallery
    {
        public int WeddingGalleryID { get; set; }

        [StringLength(250)]
        public string ImageTitle { get; set; }

        public int WeddingID { get; set; }

        public string ImageUrl { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool IsDeleted { get; set; }

        [Required]
        [StringLength(50)]
        public string ImageName { get; set; }

        public DateTime? DateTaken { get; set; }

        [StringLength(100)]
        public string Place { get; set; }

        public virtual Wedding Wedding { get; set; }

        public virtual Wedding Wedding1 { get; set; }
    }
}
