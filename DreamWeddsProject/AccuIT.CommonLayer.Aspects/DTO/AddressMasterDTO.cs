using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class AddressMasterDTO
    {
        [DataMember]
        public int AddressID { get; set; }
        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public int Country { get; set; }
        [DataMember]
        public int PinCode { get; set; }
        [DataMember]
        public Nullable<int> AddressType { get; set; } // Officical, Venue , Residential
        [DataMember]
        public Nullable<int> AddressStatus { get; set; } // Permanent Temporary
        [DataMember]
        public int CreatedBy { get; set; }
        [DataMember]
        public Nullable<int> ModifiedBy { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public Nullable<int> AddressOwnerType { get; set; }
        [DataMember]
        public Nullable<int> AddressOwnerTypePKID { get; set; }
        [DataMember]
        public string Lattitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }

    }
}
