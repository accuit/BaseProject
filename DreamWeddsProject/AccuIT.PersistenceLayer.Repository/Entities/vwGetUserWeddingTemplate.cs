namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class vwGetUserWeddingTemplate
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserID { get; set; }

        public int? AccountStatus { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderID { get; set; }

        public int? WeddingID { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int TemplateID { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime OrderDate { get; set; }

        public int? UserWeddingSubscrptionID { get; set; }

        [Key]
        [Column(Order = 4, TypeName = "money")]
        public decimal Amount { get; set; }

        [StringLength(40)]
        public string OrderStatus { get; set; }

        public int? PaymentMode { get; set; }

        public int? Discount { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? SubscriptionType { get; set; }

        public int? SubscriptionStatus { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(150)]
        public string TemplateName { get; set; }

        [StringLength(500)]
        public string TemplateFolderPath { get; set; }

        [StringLength(1000)]
        public string ThumbnailImageUrl { get; set; }

        public DateTime? WeddingDate { get; set; }

        [StringLength(150)]
        public string Title { get; set; }

        [StringLength(1000)]
        public string BackgroundImage { get; set; }
    }
}
