using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class DisplayCounterShareResponseBO
    {
        public string Brand { get; set; }
        public string PrdocutCategory { get; set; }
        public string region { get; set; }
        public string branch { get; set; }
        public long Response { get; set; }
        public int storecount { get; set; }
    }
}
