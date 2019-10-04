using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    /// <summary>
    /// DTO class to get user names
    /// </summary>
    [DataContract]
    public class UserDTO
    {
        #region Primitive Properties
        /// <summary>
        /// Property to get primary key
        /// </summary>
        [DataMember]
        public virtual long UserID
        {
            get;
            set;
        }
        /// <summary>
        /// Property to get First Name
        /// </summary>
        [DataMember]
        public virtual string FirstName
        {
            get;
            set;
        }
        /// <summary>
        /// Property to get Last Name
        /// </summary>
        [DataMember]
        public virtual string LastName
        {
            get;
            set;
        }
        /// <summary>
        /// Property to get Middle Name
        /// </summary>
        [DataMember]
        public virtual string MiddleName
        {
            get;
            set;
        }

        [DataMember]
        public string UserCode { get; set; }

        [DataMember]
        public bool IsOfflineProfile { get; set; }

        [DataMember]
        public string Pincode { get; set; }

        [DataMember]
        public long UserRoleID { get; set; }

        [DataMember]
        public bool IsRoamingProfile { get; set; }
               
        #endregion
    }
}
