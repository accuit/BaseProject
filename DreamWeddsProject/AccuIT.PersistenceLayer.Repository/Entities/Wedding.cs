namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Wedding
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Wedding()
        {
            BrideAndMaids = new HashSet<BrideAndMaid>();
            BrideAndMaids1 = new HashSet<BrideAndMaid>();
            GroomAndMen = new HashSet<GroomAndMan>();
            GroomAndMen1 = new HashSet<GroomAndMan>();
            RSVPDetails = new HashSet<RSVPDetail>();
            TimeLines = new HashSet<TimeLine>();
            TimeLines1 = new HashSet<TimeLine>();
            UserWeddingSubscriptions = new HashSet<UserWeddingSubscription>();
            UserWeddingSubscriptions1 = new HashSet<UserWeddingSubscription>();
            WeddingEvents = new HashSet<WeddingEvent>();
            WeddingEvents1 = new HashSet<WeddingEvent>();
            WeddingGalleries = new HashSet<WeddingGallery>();
            WeddingGalleries1 = new HashSet<WeddingGallery>();
        }

        public int WeddingID { get; set; }

        public DateTime WeddingDate { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        public int WeddingStyle { get; set; }

        [StringLength(500)]
        public string IconUrl { get; set; }

        public int? TemplateID { get; set; }

        public bool IsLoveMarriage { get; set; }

        public int? UserID { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }

        [StringLength(1000)]
        public string BackgroundImage { get; set; }

        [StringLength(500)]
        public string Quote { get; set; }

        [StringLength(1500)]
        public string fbPageUrl { get; set; }

        [StringLength(1500)]
        public string videoUrl { get; set; }

        public int CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BrideAndMaid> BrideAndMaids { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BrideAndMaid> BrideAndMaids1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroomAndMan> GroomAndMen { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GroomAndMan> GroomAndMen1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RSVPDetail> RSVPDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeLine> TimeLines { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TimeLine> TimeLines1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserWeddingSubscription> UserWeddingSubscriptions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserWeddingSubscription> UserWeddingSubscriptions1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WeddingEvent> WeddingEvents { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WeddingEvent> WeddingEvents1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WeddingGallery> WeddingGalleries { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WeddingGallery> WeddingGalleries1 { get; set; }
    }
}
