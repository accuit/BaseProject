using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public  class UserWeddingSubscriptionBO
    {
        public int? UserWeddingSubscrptionID { get; set; }
        public int UserId { get; set; }
        public int TemplateID { get; set; }
        public int InvoiceNo { get; set; }
        public Nullable<int> WeddingID { get; set; }
        public int? SubscriptionType { get; set; }
        public AccuIT.CommonLayer.Aspects.Utilities.AspectEnums.SubscriptionType SubscriptionTypeList { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string ReasonOfUpdate { get; set; }
        public int? SubscriptionStatus { get; set; }

        public OrderMasterBO OrderMaster { get; set; }
        public  TemplateMasterBO TemplateMaster { get; set; }
        public  WeddingBO Wedding { get; set; }
    }
}
