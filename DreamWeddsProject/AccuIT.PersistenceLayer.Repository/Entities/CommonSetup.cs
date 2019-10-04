namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CommonSetup")]
    public partial class CommonSetup
    {
        public int CommonSetupID { get; set; }

        [Required]
        [StringLength(50)]
        public string MainType { get; set; }

        [Required]
        [StringLength(50)]
        public string SubType { get; set; }

        [Required]
        [StringLength(150)]
        public string DisplayText { get; set; }

        public int DisplayValue { get; set; }

        public int ParentID { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public bool? isDeleted { get; set; }
    }
}
