using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class UserStoreMappingBO
    {
        public string EmplCode { get; set; }
        public string Username { get; set; }
        public string UserProfile { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string StoreStatus { get; set; }
        public string MappingStatus { get; set; }
        public string MappingDate { get; set; }      
    }
}
