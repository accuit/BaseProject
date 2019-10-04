namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserWeddingSubscription
    {
        [Key]
        public int UserWeddingSubscrptionID { get; set; }

        public int UserId { get; set; }

        public int TemplateID { get; set; }

        public int? InvoiceNo { get; set; }

        public int? WeddingID { get; set; }

        public int SubscriptionType { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? UpdatedDate { get; set; }

        [StringLength(500)]
        public string ReasonOfUpdate { get; set; }

        public int SubscriptionStatus { get; set; }

        public virtual OrderMaster OrderMaster { get; set; }

        public virtual OrderMaster OrderMaster1 { get; set; }

        public virtual SubscriptionMaster SubscriptionMaster { get; set; }

        public virtual SubscriptionMaster SubscriptionMaster1 { get; set; }

        public virtual TemplateMaster TemplateMaster { get; set; }

        public virtual TemplateMaster TemplateMaster1 { get; set; }

        public virtual Wedding Wedding { get; set; }

        public virtual Wedding Wedding1 { get; set; }
    }
}
