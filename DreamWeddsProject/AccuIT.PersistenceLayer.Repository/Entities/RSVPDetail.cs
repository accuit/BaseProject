namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RSVPDetail
    {
        [Key]
        public int RSVPID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public bool IsComing { get; set; }

        public DateTime? FromDate { get; set; }

        public DateTime? ToDate { get; set; }

        public int? GuestCount { get; set; }

        public byte PreferredFood { get; set; }

        [StringLength(150)]
        public string Email { get; set; }

        [StringLength(15)]
        public string Phone { get; set; }

        [StringLength(15)]
        public string Mobile { get; set; }

        [StringLength(500)]
        public string SpecialNote { get; set; }

        [Required]
        [StringLength(500)]
        public string ImageUrl { get; set; }

        public bool IsDeleted { get; set; }

        public int WeddingID { get; set; }

        [StringLength(20)]
        public string ParticipatingInEvents { get; set; }

        [StringLength(150)]
        public string ComingFromCity { get; set; }

        public int? PreferredStayIn { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public int? CreatedBy { get; set; }

        public virtual Wedding Wedding { get; set; }
    }
}
