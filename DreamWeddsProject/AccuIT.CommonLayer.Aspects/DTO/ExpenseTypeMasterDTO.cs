using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class SyncExpenseTypeMasterDTO
    {

        [DataMember]
        public bool HasMoreRows;

        [DataMember]
        public string MaxModifiedDate;

        [DataMember]
        public List<ExpenseTypeMasterDTO> Result;
    }
    
    /// <summary>
    /// ExenseMasterDTO for EMSExpenseTypeMaster table
    /// </summary>
    [DataContract]
    public class ExpenseTypeMasterDTO
    {
        [DataMember]
        public int EMSExpenseTypeMasterId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Code { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public int CompanyId { get; set; }
        [DataMember]
        public int Sequence { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }
}
