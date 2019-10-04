//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AccuIT.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class SubscriptionMaster
    {
        public SubscriptionMaster()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
            this.UserWeddingSubscriptions = new HashSet<UserWeddingSubscription>();
        }
    
        public int SubscriptionID { get; set; }
        public int SubsType { get; set; }
        public string SubsName { get; set; }
        public string SubsCode { get; set; }
        public int Days { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<UserWeddingSubscription> UserWeddingSubscriptions { get; set; }
    }
}
