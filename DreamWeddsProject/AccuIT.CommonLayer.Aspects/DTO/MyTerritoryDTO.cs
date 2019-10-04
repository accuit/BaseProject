using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;


namespace AccuIT.CommonLayer.Aspects.DTO
{
    /// <summary>
    /// This class is used to define the response data for SPGetMyTerritory_Result
    /// </summary>
    [DataContract]
    public class MyTerritoryDTO
    {
        [DataMember]
        public int TotalND { get; set; }
        [DataMember]
        public int ActiveND { get; set; }
        [DataMember]
        public int TTLSPPDLRs { get; set; }
        [DataMember]
        public int NotBilledSPPDLRs { get; set; }
        [DataMember]
        public double SPPSellThru { get; set; }
        [DataMember]
        public double TTLSellThru { get; set; }
        [DataMember]
        public double TTLSellOut { get; set; }
        [DataMember]
        public double SPPSellOut { get; set; }
        [DataMember]
        public double DistyWOS { get; set; }
        [DataMember]
        public string DistyCode { get; set; }
        [DataMember]
        public string DistyName { get; set; }
    }
}
