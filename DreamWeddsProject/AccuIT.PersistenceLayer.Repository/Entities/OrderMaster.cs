namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderMaster")]
    public partial class OrderMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OrderMaster()
        {
            OrderDetails = new HashSet<OrderDetail>();
            UserWeddingSubscriptions = new HashSet<UserWeddingSubscription>();
            UserWeddingSubscriptions1 = new HashSet<UserWeddingSubscription>();
        }

        [Key]
        public int OrderID { get; set; }

        public int UserID { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? RequiredDate { get; set; }

        [StringLength(40)]
        public string OrderStatus { get; set; }

        [StringLength(60)]
        public string AddressID { get; set; }

        public int? CGST { get; set; }

        public int? SGST { get; set; }

        public int? Discount { get; set; }

        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Column(TypeName = "money")]
        public decimal? ReceivedAmount { get; set; }

        public int? PaymentMode { get; set; }

        [StringLength(50)]
        public string PaymentTerms { get; set; }

        [StringLength(250)]
        public string OderNote { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserWeddingSubscription> UserWeddingSubscriptions { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserWeddingSubscription> UserWeddingSubscriptions1 { get; set; }

        public virtual UserMaster UserMaster { get; set; }
    }
}
