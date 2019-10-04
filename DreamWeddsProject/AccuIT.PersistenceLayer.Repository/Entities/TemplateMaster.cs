namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TemplateMaster")]
    public partial class TemplateMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TemplateMaster()
        {
            OrderDetails = new HashSet<OrderDetail>();
            TemplateImages = new HashSet<TemplateImage>();
            TemplateImages1 = new HashSet<TemplateImage>();
            TemplateMergeFields = new HashSet<TemplateMergeField>();
            TemplateMergeFields1 = new HashSet<TemplateMergeField>();
            TemplatePages = new HashSet<TemplatePage>();
            TemplatePages1 = new HashSet<TemplatePage>();
            UserWeddingSubscriptions = new HashSet<UserWeddingSubscription>();
            UserWeddingSubscriptions1 = new HashSet<UserWeddingSubscription>();
        }

        [Key]
        public int TemplateID { get; set; }

        [Required]
        [StringLength(150)]
        public string TemplateName { get; set; }

        public int TemplateType { get; set; }

        public int TemplateStatus { get; set; }

        [Required]
        public string TemplateContent { get; set; }

        [StringLength(250)]
        public string TemplateSubject { get; set; }

        [StringLength(15)]
        public string TemplateTags { get; set; }

        [StringLength(1000)]
        public string TemplateUrl { get; set; }

        [StringLength(500)]
        public string TemplateFolderPath { get; set; }

        [StringLength(1000)]
        public string ThumbnailImageUrl { get; set; }

        [StringLength(500)]
        public string TagLine { get; set; }

        public int? COST { get; set; }

        [StringLength(100)]
        public string AuthorName { get; set; }

        [StringLength(1000)]
        public string AboutTemplate { get; set; }

        [StringLength(20)]
        public string Features { get; set; }

        public bool IsDeleted { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public int? ModifiedBY { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? TemplateCode { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TemplateImage> TemplateImages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TemplateImage> TemplateImages1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TemplateMergeField> TemplateMergeFields { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TemplateMergeField> TemplateMergeFields1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TemplatePage> TemplatePages { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TemplatePage> TemplatePages1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserWeddingSubscription> UserWeddingSubscriptions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserWeddingSubscription> UserWeddingSubscriptions1 { get; set; }
    }
}
