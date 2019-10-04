using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class UserWeddingSubscriptionDTO
    {
        [DataMember]
        public int UserWeddingSubscrptionID { get; set; }
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public int TemplateID { get; set; }
        [DataMember]
        public int InvoiceNo { get; set; }
        [DataMember]
        public Nullable<int> WeddingID { get; set; }
        [DataMember]
        public string SubscriptionType { get; set; }
        [DataMember]
        public string StartDate { get; set; }
        [DataMember]
        public string EndDate { get; set; }
        [DataMember]
        public string SubscriptionStatus { get; set; }
        
        [DataMember]
        public OrderMasterDTO OrderMaster { get; set; }
        //[DataMember]
        public virtual TemplateMasterDTO TemplateMaster { get; set; }
        [DataMember]
        public virtual WeddingDTO Wedding { get; set; }
    }
}
