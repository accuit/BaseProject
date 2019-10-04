using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class UserDashboardDTO
    {
        [DataMember]
        public List<UserWeddingSubscriptionDTO> UserDashboard { get; set; }
        //public int UserID { get; set; }
        //[DataMember]
        //public List<UserWeddingSubscriptionDTO> userWeddingSubscriptions { get; set; }
        //[DataMember]
        //public List<TemplateMasterDTO> userTemplates { get; set; }

    }

    [DataContract]
    public class UserWeddingTemplateSubscriptionsDTO
    {
        [DataMember]
        public int UserID { get; set; }

        [DataMember]
        public int UserWeddingSubscrptionID { get; set; }
        [DataMember]
        public int InvoiceNo { get; set; }
        [DataMember]
        public int SubscriptionType { get; set; }
        [DataMember]
        public AccuIT.CommonLayer.Aspects.Utilities.AspectEnums.SubscriptionType SubscriptionTypeList { get; set; }
        [DataMember]
        public System.DateTime StartDate { get; set; }
        [DataMember]
        public System.DateTime EndDate { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        [DataMember]
        public string ReasonOfUpdate { get; set; }
        [DataMember]
        public int? SubscriptionStatus { get; set; }
        [DataMember]
        public List<TemplateMasterDTO> Templates { get; set; }
    }
}
