using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class AddressMasterBO
    {

        public int AddressID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int Country { get; set; }
        public int PinCode { get; set; }
        public Nullable<int> AddressType { get; set; }
        public Nullable<int> AddressStatus { get; set; }
        public System.DateTime Created_Date { get; set; }
        public int Created_By { get; set; }
        public Nullable<System.DateTime> Modified_Date { get; set; }
        public Nullable<int> Modified_By { get; set; }
        public Nullable<bool> Is_Deleted { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> AddressOwnerType { get; set; }
        public Nullable<int> AddressOwnerTypePKID { get; set; }
        public Nullable<int> VenueID { get; set; }

        public virtual VenueBO Venue { get; set; }

    }
}
