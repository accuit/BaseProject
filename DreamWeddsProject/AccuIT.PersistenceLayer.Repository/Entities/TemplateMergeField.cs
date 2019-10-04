namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class TemplateMergeField
    {
        [Key]
        public int PK_ID { get; set; }

        [Required]
        [StringLength(50)]
        public string MERGEFIELD_NAME { get; set; }

        [StringLength(150)]
        public string SRC_FIELD { get; set; }

        [StringLength(200)]
        public string SRC_FIELD_VALUE { get; set; }

        public bool IsDeleted { get; set; }

        public int? TemplateID { get; set; }

        public int CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? Sequence { get; set; }

        public int? TemplateCode { get; set; }

        public virtual TemplateMaster TemplateMaster { get; set; }

        public virtual TemplateMaster TemplateMaster1 { get; set; }
    }
}
