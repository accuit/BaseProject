using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccuIT.PersistenceLayer.Repository.Entities
{
    public class User
    {
        /// <summary>
        /// Property to get/set User ID
        /// </summary>
        public long UserID
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get/set First Name
        /// </summary>
        public string FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get/set Last Name
        /// </summary>
        public string LastName
        {
            get;
            set;
        }

        /// <summary>
        /// Property to get/set Middle Name
        /// </summary>
        public string MiddleName
        {
            get;
            set;
        }
    }
}
