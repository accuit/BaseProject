using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class DownloadDataMasterBO
    {
        public int DownloadDataMasterID { get; set; }
        public string DataName { get; set; }
        public string DataDescription { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifyBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
