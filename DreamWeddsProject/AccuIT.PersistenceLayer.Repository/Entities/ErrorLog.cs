namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ErrorLog")]
    public partial class ErrorLog
    {
        [Key]
        public int LogID { get; set; }

        public int? EventID { get; set; }

        public int Priority { get; set; }

        public int Severity { get; set; }

        [StringLength(150)]
        public string Title { get; set; }

        [StringLength(150)]
        public string MachineName { get; set; }

        [StringLength(150)]
        public string AppDomainName { get; set; }

        [StringLength(150)]
        public string ProcessID { get; set; }

        [StringLength(150)]
        public string ProcessName { get; set; }

        [StringLength(150)]
        public string ThreadName { get; set; }

        [StringLength(150)]
        public string Win32ThreadId { get; set; }

        public string Message { get; set; }

        public DateTime Timestamp { get; set; }

        [StringLength(1000)]
        public string FormattedMessage { get; set; }
    }
}
