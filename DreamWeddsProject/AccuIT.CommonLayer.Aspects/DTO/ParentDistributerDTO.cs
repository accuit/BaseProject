using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
   public class ParentDistributerDTO
    {
       
       [DataMember]
        public string DistributorCode { get; set; }
       [DataMember]
       public string DistributorName { get; set; }
       /// <summary>
       /// Added by Gourav Vishnoi on 15 June 2015 , For SRD Geo Tag
       /// </summary>
       //[DataMember]
       //public double Lattitude { get; set; }
       //[DataMember]
       //public double Longitude { get; set; }
       //[DataMember]
       //public bool IsMasterTagged { get; set; }
   }
}
