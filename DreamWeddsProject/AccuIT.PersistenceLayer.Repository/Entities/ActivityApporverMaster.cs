//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Samsung.SmartDost.PersistenceLayer.Repository.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class ActivityApporverMaster
    {
        public int ActivityApprverID { get; set; }
        public Nullable<int> ActivityID { get; set; }
        public Nullable<int> ApproverTypeID { get; set; }
        public string ApproverValue { get; set; }
        public string Branch { get; set; }
    
        public virtual ApproverTypeMaster ApproverTypeMaster { get; set; }
        public virtual ActivityMaster ActivityMaster { get; set; }
    }
}
