using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
  public class DesignationDTO
    {
      [DataMember]
      public int DesignationID { get; set; }

      [DataMember]
      public string DesignName { get; set; }

    }
}
