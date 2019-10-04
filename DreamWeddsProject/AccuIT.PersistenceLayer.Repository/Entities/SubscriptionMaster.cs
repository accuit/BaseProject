namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SubscriptionMaster")]
    public partial class SubscriptionMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SubscriptionMaster()
        {
            OrderDetails = new HashSet<OrderDetail>();
            UserWeddingSubscriptions = new HashSet<UserWeddingSubscription>();
            UserWeddingSubscriptions1 = new HashSet<UserWeddingSubscription>();
        }

        [Key]
        public int SubscriptionID { get; set; }

        public int SubsType { get; set; }

        [Required]
        [StringLength(50)]
        public string SubsName { get; set; }

        [Required]
        [StringLength(5)]
        public string SubsCode { get; set; }

        public int Days { get; set; }

        public bool IsDeleted { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserWeddingSubscription> UserWeddingSubscriptions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserWeddingSubscription> UserWeddingSubscriptions1 { get; set; }
    }
}
