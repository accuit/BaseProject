namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Venue")]
    public partial class Venue
    {
        public int VenueID { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [StringLength(350)]
        public string VenueImageUrl { get; set; }

        [StringLength(1000)]
        public string VenueBannerImageUrl { get; set; }

        [StringLength(500)]
        public string VenueWebsite { get; set; }

        [StringLength(50)]
        public string OwnerName { get; set; }

        [StringLength(15)]
        public string VenuePhone { get; set; }

        [StringLength(15)]
        public string VenueMobile { get; set; }

        public int? WeddingEventID { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        [StringLength(1500)]
        public string googleMapUrl { get; set; }

        public int CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public virtual WeddingEvent WeddingEvent { get; set; }
    }
}
