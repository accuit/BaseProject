using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class StockAuditDTO
    {  
        [DataMember]
        public ICollection<AuditResponseDTO> StockAuditSummary { get; set; }
    }

    public class AuditResponseDTO
    {

        [DataMember]
        public int FixtureID { get; set; }
        [DataMember]
        public byte WallNumber { get; set; }
        [DataMember]
        public byte RowNumber { get; set; }
        [DataMember]
        public byte BrandID { get; set; }
        
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public bool Topper { get; set; }
        [DataMember]
        public bool SwitchedOn { get; set; }
        [DataMember]
        public bool PriceTag { get; set; }
        [DataMember]
        public ICollection<StockAuditPOSMResponseDTO> StockAuditPOSMResponse { get; set; }
            
    }

    public partial class StockAuditPOSMResponseDTO
    {
        
        public long StockAuditPOSMResponseID { get; set; }
        
        public long StockAuditResponseID { get; set; }
        [DataMember]
        public int POSMID { get; set; }
        [DataMember]
        public Nullable<byte> POSMType { get; set; }
        
        public int ProductID { get; set; }
        
        public bool IsDeleted { get; set; }
        
        public System.DateTime CreatedDate { get; set; }
        
        public long CreatedBy { get; set; }
        
        public Nullable<System.DateTime> Modifieddate { get; set; }
        
        public Nullable<long> ModifiedBy { get; set; }       
        
    }
   
}
