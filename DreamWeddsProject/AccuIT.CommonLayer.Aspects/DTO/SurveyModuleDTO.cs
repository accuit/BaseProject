using AccuIT.CommonLayer.Aspects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    [DataContract]
    public class SyncSurveyModuleDTO
    {
        [DataMember]
        public bool HasMoreRows;
        [DataMember]
        public string MaxModifiedDate;

        [DataMember]
        public List<SurveyModuleDTO> Result;
    }

    [DataContract]
    public class SurveyModuleDTO
    {
        [DataMember]
        public int ModuleID { get; set; }

        [DataMember]
        public int ModuleCode { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public List<SurveyQuestionDTO> Questions { get; set; }

        [DataMember]
        public int Sequence { get; set; }
    }
}
