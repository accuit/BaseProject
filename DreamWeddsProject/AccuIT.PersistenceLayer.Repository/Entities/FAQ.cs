namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FAQ")]
    public partial class FAQ
    {
        public int FAQID { get; set; }

        [Required]
        [StringLength(500)]
        public string Question { get; set; }

        [Required]
        [StringLength(1500)]
        public string Answer { get; set; }

        [Required]
        [StringLength(250)]
        public string Website { get; set; }

        public bool IsMainQue { get; set; }

        public int? sequence { get; set; }
    }
}
