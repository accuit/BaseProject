namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ECampaign")]
    public partial class ECampaign
    {
        public int ECampaignID { get; set; }

        [StringLength(100)]
        public string ECampaignName { get; set; }

        [StringLength(250)]
        public string ECampaignSubject { get; set; }

        [StringLength(550)]
        public string ECampaignLogoUrl { get; set; }

        public int TemplateID { get; set; }

        public string ECampaignContent { get; set; }

        public int? Status { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime Created_Date { get; set; }

        public int Created_By { get; set; }

        public DateTime? Modified_Date { get; set; }

        public int? Modified_By { get; set; }

        public bool? Is_Deleted { get; set; }

        public int? CategoryId { get; set; }
    }
}
