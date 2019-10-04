using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
     [Serializable]
    public class MOMAttendeeBO
    {
        public int AttendeeId { get; set; }
        
        public string AttendeeName { get; set; }
        
        public long MOMId { get; set; }
    }
}
