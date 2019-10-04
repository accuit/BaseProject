using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{       
         [Serializable]
    public class APKMaintainanceBO
    {
        public int APKID { get; set; }
        public string APKVersion { get; set; }
        public string APKURL { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public bool IsLatest { get; set; }

        
    }
}
