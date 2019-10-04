using System;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    /// <summary>
    /// Class to list the properties for store schemes
    /// </summary>
    [DataContract]
    public class SchemeDTO
    {
        public partial class Scheme
        {
            [DataMember]
            public int SchemeID { get; set; }
            [DataMember]
            public string Title { get; set; }            
            public string Description { get; set; }
            public bool IsActive { get; set; }
            public System.DateTime SchemeStartDate { get; set; }
            public Nullable<System.DateTime> SchemeExpiryDate { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public long CreatedBy { get; set; }
            public Nullable<System.DateTime> ModifiedDate { get; set; }
            public Nullable<long> ModifiedBy { get; set; }
            public bool IsDeleted { get; set; }
        }
    }
}
