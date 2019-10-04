using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class SDSchemeBO
    {
        
        public int SDSchemeID { get; set; }        
        public string Title { get; set; }                   
        public string HTMLFilename { get; set; }
        public System.DateTime DateValidFrom { get; set; }
        public System.DateTime DateValidTo { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public virtual List<SchemeRoleMappingBO> SchemeRoleMappings { get; set; }
    }

    public partial class SchemeRoleMappingBO
    {
        
        public Nullable<int> SDSchemeID { get; set; }
        public Nullable<int> RoleID { get; set; }        

    }
}
