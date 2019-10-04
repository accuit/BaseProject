using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    /// <summary>
    /// Business object to fetch user attendance details
    /// </summary>
   
   public class UserAttendanceBO
    {
       /// <summary>
       /// Property to get/set user id
       /// </summary>       
          
            public long UserID
            {
                get;
                set;
            }
            /// <summary>
            /// Property to get/set IsAttendance
            /// </summary>       
            public bool IsAttendance
            {
                get;
                set;
            }
            /// <summary>
            /// Property to get/set AttendanceDate
            /// </summary>       
            public DateTime AttendanceDate
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
            /// Property to get/set IsDeleted
            /// </summary>       
            public bool IsDeleted
            {
                get;
                set;
            }
            /// <summary>
            /// Property to get/set StatusCode
            /// </summary>       
            public string StatusCode
            {
                get;
                set;
            }
        
    }
}
