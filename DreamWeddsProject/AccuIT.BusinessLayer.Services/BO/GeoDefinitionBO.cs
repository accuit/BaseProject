using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class GeoDefinitionBO
    {
        public int GeoDefinitionID { get; set; }
        public string GeoDefCode { get; set; }
        public string GeoDefName { get; set; }
        public int GeoID { get; set; }
        public int ParentGeoDefinitionId { get; set; }
    }
}
