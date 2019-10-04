using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    
    public class MOMMasterBO 
    {
        
        public long MOMId { get; set; }
        
        public long UserId { get; set; }
        
        public string Description { get; set; }
        
        public string ActionItem { get; set; }
        
        public string MOMTitle { get; set; }
        
        public string Location { get; set; }
        
        public DateTime MomDate { get; set; }
        
      //  public DateTime DateCreated { get; set; }
        
      //  public DateTime DateModified { get; set; }
        
        public bool IsDeleted { get; set; }

        public List<MOMAttendeeBO> MOMAttendees;

        public string MomDateStr { get; set; }
   
        public int MOMIdServer { get; set; }
    
        public int MOMIdClient { get; set; }

  
    }
}
