namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GroomAndMan
    {
        [Key]
        public int GroomAndMenID { get; set; }

        [Required]
        [StringLength(150)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(150)]
        public string LastName { get; set; }

        public DateTime? DateofBirth { get; set; }

        public int WeddingID { get; set; }

        public bool IsGroom { get; set; }

        public int? RelationWithGroom { get; set; }

        [StringLength(500)]
        public string Imageurl { get; set; }

        public int CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(500)]
        public string AboutMen { get; set; }

        public bool? IsDeleted { get; set; }

        [StringLength(1500)]
        public string fbUrl { get; set; }

        [StringLength(1500)]
        public string googleUrl { get; set; }

        [StringLength(1500)]
        public string instagramUrl { get; set; }

        [StringLength(1500)]
        public string lnkedinUrl { get; set; }

        public virtual Wedding Wedding { get; set; }

        public virtual Wedding Wedding1 { get; set; }
    }
}
