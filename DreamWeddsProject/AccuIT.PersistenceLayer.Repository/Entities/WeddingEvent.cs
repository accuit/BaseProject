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
    
    public partial class WeddingEvent
    {
        public WeddingEvent()
        {
            this.Venues = new HashSet<Venue>();
        }
    
        public int WeddingEventID { get; set; }
        public System.DateTime EventDate { get; set; }
        public string Title { get; set; }
        public int WeddingID { get; set; }
        public string ImageUrl { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime StartTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public string Aboutevent { get; set; }
        public string BackGroundImage { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
        public virtual Wedding Wedding { get; set; }
        public virtual ICollection<Venue> Venues { get; set; }
    }
}