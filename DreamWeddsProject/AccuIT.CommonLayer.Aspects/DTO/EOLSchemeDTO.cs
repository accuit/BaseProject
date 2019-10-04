using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SyncEOLSchemeDTO
    {
        [DataMember]
        public bool HasMoreRows;
        [DataMember]
        public string MaxModifiedDate;

        [DataMember]
        public List<EOLSchemeDTO> Result;
    }


    [DataContract]
    public class EOLSchemeDTO
    {
        [DataMember]
        public int SchemeID { get; set; }
        [DataMember]
        public string SchemeNumber { get; set; }
        [DataMember]
        public System.DateTime SchemeFrom { get; set; }
        [DataMember]
        public System.DateTime SchemeTo { get; set; }
        [DataMember]
        public System.DateTime OrderFrom { get; set; }
        [DataMember]
        public System.DateTime OrderTo { get; set; }
        [DataMember]
        public string PUMINumber { get; set; }
        
        public System.DateTime PUMIDate { get; set; }
        [DataMember]
        public string ProductType { get; set; }
        [DataMember]
        public string ProductGroup { get; set; }
        [DataMember]
        public string ProductCategory { get; set; }
        
        public long CreatedBy { get; set; }
        
        public System.DateTime CreatedDate { get; set; }
        
        public Nullable<long> ModifiedBy { get; set; }
        
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        [DataMember]
        public string strSchemeFrom { get; set; }
        [DataMember]
        public string strSchemeTo { get; set; }
        [DataMember]
        public string strOrderFrom { get; set; }
        [DataMember]
        public string strOrderTo { get; set; }
        [DataMember]
        public string strCreatedDate { get; set; }
        [DataMember]
        public string strModifiedDate { get; set; }
        [DataMember]
        public string strPUMIDate { get; set; }
        [DataMember]
        public ICollection<EOLSchemeDetailDTO> EOLSchemeDetails { get; set; }
        [DataMember]
        public ICollection<EOLOrderBookingDTO> EOLOrderBooking { get; set; }
        [DataMember]
        public bool SaveStatus { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }

    }

    [DataContract]
    public class EOLSchemeDetailDTO
    {
        [DataMember]
        public int SchemeDetailsID { get; set; }
        [DataMember]
        public int SchemeID { get; set; }
        [DataMember]
        public string BasicModelCode { get; set; }
        [DataMember]
        public int Quantity { get; set; }
        [DataMember]
        public int Support { get; set; }
    }

    [DataContract]
    public class EOLOrderBookingDTO
    {
        public int EOLOrderID { get; set; }        
        [DataMember]
        public int SchemeID { get; set; }
        [DataMember]
        public string BasicModelCode { get; set; }
        [DataMember]
        public int OrderQuantity { get; set; }
        [DataMember]
        public int ActualSupport { get; set; }
        public long CreatedBy { get; set; }
        public System.DateTime CreatedDateTime { get; set; }
        [DataMember]
        public int StoreID { get; set; }
        [DataMember]
        public string StoreName { get; set; }

    }

    [DataContract]
    public partial class EOLNotificationServiceLogDTO
    {
        [DataMember]
        public int EOLNotificationID { get; set; }
        [DataMember]
        public byte NotificationID { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int SchemeID { get; set; }
        [DataMember]
        public System.DateTime CreatedDate { get; set; }
    }
}
