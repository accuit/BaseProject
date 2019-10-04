using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SDSchemeDTO
    {
        [DataMember]
        public int SDSchemeID { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string strDateValidFrom { get; set; }
        [DataMember]
        public string strDateValidTo { get; set; }
        [DataMember]
        public string HTMLFilename { get; set; }

        
        public System.DateTime DateValidFrom { get; set; }        
        public System.DateTime DateValidTo { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
