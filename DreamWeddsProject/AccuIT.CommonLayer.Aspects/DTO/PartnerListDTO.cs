using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
   public class PartnerListDTO
    {
        [DataMember]
       public string City
       {
           get;
           set;
       }
        [DataMember]
        public int StoreID
        {
            get;
            set;
        }
        [DataMember]
        public string ShipToName
        {
            get;
            set;
        }
       [DataMember]
        public string ShipToCode
        {
            get;
            set;
        }

    }
}
