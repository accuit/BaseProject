using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class ServiceOutputDTO
    {
        [DataMember]
        public bool IsUpdated
        {
            get;
            set;
        }

        [DataMember]
        public string StatusCode
        {
            get;
            set;
        }

        [DataMember]
        public int UserDeviceID
        {
            get;
            set;
        }


        /* Jira ID : SDCE-198<End>*/
        [DataMember]
        public UserProfileDTO UserProfileData
        {
            get;
            set;
        }


        #region UserData
        [DataMember]
        public string Message
        {
            get;
            set;
        }
        [DataMember]
        public int UserID
        {
            get;
            set;
        }
        [DataMember]
        public int CompanyID
        {
            get;
            set;
        }
        [DataMember]
        public int RoleID
        {
            get;
            set;
        }
        [DataMember]
        public string UserCode
        {
            get;
            set;
        }
        [DataMember]
        public string FirstName { get; set; }

        [DataMember]
        public string LoginName { get; set; }

        [DataMember]
        public string LastName { get; set; }

        [DataMember]
        public string Mobile { get; set; }

        [DataMember]
        public long UserRoleID { get; set; }
        #endregion
       

        #region Security
        [DataMember]
        public string APIKey
        {
            get;
            set;
        }

        [DataMember]
        public string APIToken
        {
            get;
            set;
        }
        #endregion


    }
}
