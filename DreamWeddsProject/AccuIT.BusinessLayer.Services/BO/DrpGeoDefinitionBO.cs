using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class DrpGeoDefinitionBO
    {
        public int GeoId{ get; set; }
        public string Name{ get; set; }
        public int NextGeoId { get; set; }
        public IList<GeoDefinitionBO> geoDefinition
        {
            get;
            set;
        }
    }
}
