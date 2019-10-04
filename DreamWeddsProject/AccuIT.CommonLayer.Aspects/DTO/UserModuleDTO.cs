using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace AccuIT.CommonLayer.Aspects.DTO
{
    /// <summary>
    /// Class to get user module data contract objects
    /// </summary>
    /// 
    [DataContract]
    public class SyncUserModuleDTO
    {
        [DataMember]
        public bool HasMoreRows;
        [DataMember]
        public string MaxModifiedDate;

        [DataMember]
        public List<UserModuleDTO> Result;
    }

    [DataContract]
    public class UserModuleDTO
    {
        [DataMember]
        public int ModuleID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public Nullable<int> ParentModuleID { get; set; }
        [DataMember]
        public bool IsMandatory { get; set; }
        [DataMember]
        public int Sequence { get; set; }
        [DataMember]
        public int EmpID { get; set; }
        [DataMember]
        public long EmpRoleID { get; set; }

        [DataMember]
        public Nullable<int> ModuleCode { get; set; }
        [DataMember]
        public string Icon { get; set; }
        [DataMember]
        public bool IsQuestionModule { get; set; }
        [DataMember]
        public Nullable<bool> IsStoreWise { get; set; }
        [DataMember]
        public string PageURL { get; set; }
        [DataMember]
        public byte ModuleType { get; set; }
        
        [DataMember]
        public bool IsDeleted { get; set; }
    }

}
