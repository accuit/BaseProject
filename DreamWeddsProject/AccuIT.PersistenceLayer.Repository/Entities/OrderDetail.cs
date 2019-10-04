namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        public int OrderDetailID { get; set; }

        public int? Discount { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public int OrderID { get; set; }

        public int TemplateID { get; set; }

        public bool IsDeleted { get; set; }

        public int? SubscrptionID { get; set; }

        public virtual OrderMaster OrderMaster { get; set; }

        public virtual SubscriptionMaster SubscriptionMaster { get; set; }

        public virtual TemplateMaster TemplateMaster { get; set; }
    }
}
