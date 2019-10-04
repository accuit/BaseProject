using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    /// <summary>
    /// DTO class to get User Profile Detail
    /// </summary>
    [DataContract]
    public class UserProfileDTO
    {
        #region Primitive Properties

        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public string UserCode { get; set; }
        [DataMember]
        public string LoginName { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Mobile { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string JoiningDate { get; set; }
        [DataMember]
        public string PurchaseDate { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
        [DataMember]
        public AddressMasterDTO Address { get; set; }
        [DataMember]
        public int AccountStatus { get; set; }
        [DataMember]
        public bool IsAdmin { get; set; }
        [DataMember]
        public int RoleID { get; set; }
        //[DataMember]
        //public string AlternateEmailID { get; set; }
        [DataMember]
        public int UserRoleID { get; set; }

        [DataMember]
        public List<UserWeddingSubscriptionDTO> UserWeddingSubscriptions { get; set; }

        #endregion
    }
}
