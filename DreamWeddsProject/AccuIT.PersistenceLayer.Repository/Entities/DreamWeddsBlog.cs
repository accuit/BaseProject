namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DreamWeddsBlog")]
    public partial class DreamWeddsBlog
    {
        [Key]
        public int BlogID { get; set; }

        [Required]
        [StringLength(150)]
        public string BlogName { get; set; }

        [Required]
        [StringLength(500)]
        public string Title { get; set; }

        [StringLength(250)]
        public string BlogSubject { get; set; }

        [StringLength(500)]
        public string Quote { get; set; }

        [StringLength(50)]
        public string AuthorName { get; set; }

        [Required]
        public string Content { get; set; }

        [StringLength(1500)]
        public string ImageUrl { get; set; }

        [StringLength(500)]
        public string SpecialNote { get; set; }

        public bool IsDeleted { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
