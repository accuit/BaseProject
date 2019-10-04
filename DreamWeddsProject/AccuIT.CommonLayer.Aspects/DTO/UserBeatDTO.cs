using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    /// <summary>
    /// DTO class to get user Beat information
    /// </summary>
    [DataContract]
    public class UserBeatDTO
    {
        /// <summary>
        /// Property to get UserID
        /// </summary>
        [DataMember]
        public int CompanyID
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get UserID
        /// </summary>
        [DataMember]
        public long UserID
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get DealerID
        /// </summary>
        [DataMember]
        public int StoreID
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get CoverageDate
        /// </summary>
        [DataMember]
        public string PlanDate
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get IsCoverage
        /// </summary>
        [DataMember]
        public bool IsCoverage
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get StatusID
        /// </summary>
        [DataMember]
        public int StatusID
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get Remarks
        /// </summary>
        [DataMember]
        public string Remarks
        {
            get;
            set;
        }

        [DataMember]
        public string CoverageType { get; set; }

        [DataMember]
        public Nullable<int> TargetOutlets { get; set; }

        [DataMember]
        public string MarketOffDays { get; set; }
    }
}
