using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{ /// <summary>
    /// Business object to fetch user Beat details
    /// </summary>
   public class UserBeatBO
    {
        /// <summary>
        /// Property to get/set CompanyID
        /// </summary>
       
        public int CompanyID
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get/set UserID
        /// </summary>
       
        public long UserID
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get/set DealerID
        /// </summary>

        public int StoreID
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get/set CoverageDate
        /// </summary>
       
        public DateTime CoverageDate
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get/set IsCoverage
        /// </summary>
       
        public bool IsCoverage
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get/set StatusID
        /// </summary>
       
        public int StatusID
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get/set Remarks
        /// </summary>
       
        public string Remarks
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get/set CreatedDate
        /// </summary>
       
        public DateTime CreatedDate
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get/set CreatedBy
        /// </summary>
       
        public int CreatedBy
        {
            get;
            set;
        }
    }
}
