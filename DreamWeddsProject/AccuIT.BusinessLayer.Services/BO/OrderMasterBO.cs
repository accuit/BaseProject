using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public  class OrderMasterBO
    {

        public int OrderID { get; set; }
        public int UserID { get; set; }
        public System.DateTime OrderDate { get; set; }
        public Nullable<System.DateTime> RequiredDate { get; set; }
        public string OrderStatus { get; set; }
        public string AddressID { get; set; }
        public Nullable<int> CGST { get; set; }
        public Nullable<int> SGST { get; set; }
        public Nullable<int> Discount { get; set; }
        public decimal Amount { get; set; }
        public Nullable<decimal> ReceivedAmount { get; set; }
        public Nullable<int> PaymentMode { get; set; }
        public string PaymentTerms { get; set; }
        public string OderNote { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }


        public virtual ICollection<OrderDetailBO> OrderDetails { get; set; }
        public virtual ICollection<UserWeddingSubscriptionBO> UserWeddingSubscriptions { get; set; }
        public virtual UserMasterBO UserMaster { get; set; }
    }
    public partial class OrderDetailBO
    {
        public int OrderDetailID { get; set; }
        public Nullable<int> Discount { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int OrderID { get; set; }
        public int TemplateID { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> SubscrptionID { get; set; }

       // public virtual OrderMasterBO OrderMaster { get; set; }
        //public virtual SubscriptionMasterBO SubscriptionMaster { get; set; }
        //public virtual TemplateMaster TemplateMaster { get; set; }
    }
}
