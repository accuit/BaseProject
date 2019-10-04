using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    [Serializable]
    public class ChannelTypeDisplayBO
    {
        public int ChannelTypeDisplayID { get; set; }
        public string ChannelType { get; set; }
        public bool IsDisplayCounterShare { get; set; }
        public bool IsPlanogram { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifyDate { get; set; }
        public Nullable<int> ModifyBy { get; set; }

    }



}
