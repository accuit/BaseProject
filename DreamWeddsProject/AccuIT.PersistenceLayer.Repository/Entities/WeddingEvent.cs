namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class WeddingEvent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public WeddingEvent()
        {
            Venues = new HashSet<Venue>();
        }

        public int WeddingEventID { get; set; }

        public DateTime EventDate { get; set; }

        [Required]
        [StringLength(250)]
        public string Title { get; set; }

        public int WeddingID { get; set; }

        [StringLength(500)]
        public string ImageUrl { get; set; }

        public int CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [StringLength(1000)]
        public string Aboutevent { get; set; }

        [StringLength(1000)]
        public string BackGroundImage { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Venue> Venues { get; set; }

        public virtual Wedding Wedding { get; set; }

        public virtual Wedding Wedding1 { get; set; }
    }
}
