using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SyncCompetitorsDTO
    {

        [DataMember]
        public bool HasMoreRows;
        [DataMember]
        public string MaxModifiedDate;

        [DataMember]
        public List<CompetitorsDTO> Result;
    }

    [DataContract]
   public class CompetitorsDTO
    {
        [DataMember]
       public int CompetitorID
        {
            get;
            set;
        }
        [DataMember]
        public int ProductTypeID
        {
            get;
            set;
        }

        [DataMember]
        public string Name
        {
            get;
            set;
        }

        [DataMember]
        public string Code
        {
            get;
            set;
        }


        [DataMember]
        public bool IsDeleted
        {
            get;
            set;
        }
    }
}
