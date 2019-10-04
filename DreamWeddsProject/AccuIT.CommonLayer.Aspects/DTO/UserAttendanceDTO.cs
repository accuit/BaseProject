using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    // <summary>
    /// DTO class to get User Attendance info
    /// </summary>
     [DataContract]
   public class UserAttendanceDTO
   { 
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
         /// Property to get IsAttendance
         /// </summary>
         [DataMember]
         public bool IsAttendance
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

         /// <summary>
         /// Property to get lattitude
         /// </summary>
         [DataMember]
         public string Lattitude
         {
             get;
             set;
         }

         /// <summary>
         /// Property to get longitude
         /// </summary>
         [DataMember]
         public string Longitude
         {
             get;
             set;
         }
    }
}
