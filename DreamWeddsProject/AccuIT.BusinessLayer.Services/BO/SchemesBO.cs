using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    /// <summary>
    /// Business object to fetch dealer schemes details only
    /// </summary>
    public class SchemesBO
    {
        public int SchemeID { get; set; }
        public int DealerID { get; set; }
        public string Title { get; set; }
        public int CompanyID { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime SchemeStartDate { get; set; }
        public string SchemeStartDate_Display
        {
            get
            {
                if (!string.IsNullOrEmpty(SchemeStartDate.ToString()))
                    return SchemeStartDate.ToString("dd/MM/yyyy");
                else
                    return string.Empty;
            }
        }
        public Nullable<System.DateTime> SchemeExpiryDate { get; set; }
        public string SchemeExpiryDate_Display
        {
            get
            {
                if (!string.IsNullOrEmpty(SchemeExpiryDate.ToString()))
                    return SchemeExpiryDate.Value.ToString("dd/MM/yyyy");
                else
                    return string.Empty;
            }
        }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}
