using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class FMSMastersDTO
    {
        [DataMember]
        public List<FeedbackStatusMasterDTO> Status { get; set; }
        [DataMember]
        public List<TeamMasterDTO> Teams { get; set; }
        [DataMember]
        public List<FeedbackCategoryMasterDTO> FeedbackCategories { get; set; }
        [DataMember]
        public List<FeedbackTypeMasterDTO> FeedbackTypes { get; set; }
        [DataMember]
        public bool HasMoreRows { get; set; }
    }

    [DataContract]
    public class SyncFeedbackStatusMasterDTO
    {

        [DataMember]
        public bool HasMoreRows;
        [DataMember]
        public string MaxModifiedDate;

        [DataMember]
        public List<FeedbackStatusMasterDTO> Result;
    }

    [DataContract]
    public class FeedbackStatusMasterDTO
    {
        [DataMember]
        public int FeedbackStatusID { get; set; }
        [DataMember]
        public string FeedbackStatusName { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }


    [DataContract]
    public class SyncTeamMasterDTO
    {

        [DataMember]
        public bool HasMoreRows;
        [DataMember]
        public string MaxModifiedDate;

        [DataMember]
        public List<TeamMasterDTO> Result;
    }

    [DataContract]
    public class TeamMasterDTO
    {
        [DataMember]
        public int TeamId { get; set; }
        [DataMember]
        public string TeamName { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }


    [DataContract]
    public class SyncFeedbackCategoryMasterDTO
    {

        [DataMember]
        public bool HasMoreRows;
        [DataMember]
        public string MaxModifiedDate;

        [DataMember]
        public List<FeedbackCategoryMasterDTO> Result;
    }

    [DataContract]
    public class FeedbackCategoryMasterDTO
    {
        [DataMember]
        public int FeedbackCatID { get; set; }
        [DataMember]
        public int TeamID { get; set; }
        [DataMember]
        public string FeedbackCategoryName { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }


    [DataContract]
    public class SyncFeedbackTypeMasterDTO
    {

        [DataMember]
        public bool HasMoreRows;
        [DataMember]
        public string MaxModifiedDate;

        [DataMember]
        public List<FeedbackTypeMasterDTO> Result;
    }


    [DataContract]
    public class FeedbackTypeMasterDTO
    {
        [DataMember]
        public int FeedbackTypeID { get; set; }
        [DataMember]
        public int FeedbackCatID { get; set; }
        [DataMember]
        public string FeedbackTypeName { get; set; }
        [DataMember]
        public string SampleImageName { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }

}
