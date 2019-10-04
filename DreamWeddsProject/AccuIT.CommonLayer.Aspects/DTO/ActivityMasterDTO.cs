using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class ActivityMasterDTO
    {
        [DataMember]
        public int ActivityID { get; set; }
        [DataMember]
        public string Deptt { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public string Activity { get; set; }
        [DataMember]
        public string SubActivity { get; set; }
        [DataMember]
        public string RBCNo { get; set; }
        [DataMember]
        public string RFA_EP { get; set; }
        [DataMember]
        public string BudgetRequired { get; set; }
        [DataMember]
        public string FinancialLimits { get; set; }

    }
}
