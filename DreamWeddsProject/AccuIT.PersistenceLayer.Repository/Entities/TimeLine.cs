namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TimeLine")]
    public partial class TimeLine
    {
        public int TimeLineID { get; set; }

        public DateTime StoryDate { get; set; }

        [Required]
        [StringLength(150)]
        public string Title { get; set; }

        [StringLength(2000)]
        public string Story { get; set; }

        [StringLength(1000)]
        public string ImageUrl { get; set; }

        public int WeddingID { get; set; }

        public int CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [StringLength(500)]
        public string Location { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Wedding Wedding { get; set; }

        public virtual Wedding Wedding1 { get; set; }
    }
}
