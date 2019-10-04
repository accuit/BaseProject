using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class AnnouncementBO
    {
        public int AnnouncementID { get; set; }
        public System.DateTime StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string BannerContent { get; set; }
        public string BannerContentMobile { get; set; }
    }
}
