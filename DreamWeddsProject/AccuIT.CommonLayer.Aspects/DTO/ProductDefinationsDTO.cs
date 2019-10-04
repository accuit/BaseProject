using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
  public  class ProductDefinationsDTO
  {
      #region Primitive Properties

      [DataMember]
      public int ProductDefID { get; set; }

      [DataMember]
      public string DefCode { get; set; }

      [DataMember]
      public string DefName { get; set; }

      [DataMember]
      public Nullable<int> MasterID { get; set; }

      #endregion
  }
}
