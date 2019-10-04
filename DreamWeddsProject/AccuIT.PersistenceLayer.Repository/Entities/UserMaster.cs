namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("UserMaster")]
    public partial class UserMaster
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserMaster()
        {
            DailyLoginHistories = new HashSet<DailyLoginHistory>();
            LoginAttemptHistories = new HashSet<LoginAttemptHistory>();
            OrderMasters = new HashSet<OrderMaster>();
            UserDevices = new HashSet<UserDevice>();
            UserRoles = new HashSet<UserRole>();
            UserServiceAccesses = new HashSet<UserServiceAccess>();
            UserSystemSettings = new HashSet<UserSystemSetting>();
        }

        [Key]
        public int UserID { get; set; }

        [Required]
        [StringLength(40)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public bool isDeleted { get; set; }

        [StringLength(150)]
        public string LoginName { get; set; }

        [StringLength(30)]
        public string Password { get; set; }

        [StringLength(750)]
        public string ImagePath { get; set; }

        [StringLength(20)]
        public string EmpCode { get; set; }

        [StringLength(250)]
        public string Email { get; set; }

        public int? AccountStatus { get; set; }

        public bool isActive { get; set; }

        public int? SeniorEmpID { get; set; }

        public bool? IsEmployee { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Mobile { get; set; }

        public int CompanyId { get; set; }

        public virtual CompanyMaster CompanyMaster { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DailyLoginHistory> DailyLoginHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LoginAttemptHistory> LoginAttemptHistories { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderMaster> OrderMasters { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserDevice> UserDevices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRole> UserRoles { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserServiceAccess> UserServiceAccesses { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserSystemSetting> UserSystemSettings { get; set; }
    }
}
