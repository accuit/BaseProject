﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
  public  class OrderBookedReportBO
    {
        public string Name { get; set; }
        public string ProfilePictureFileName { get; set; }
        public long UserID { get; set; }
        public int OrderBookedCount { get; set; }
        public int TotalStoreCount { get; set; }
        public int Percentage { get; set; }
    }
}
