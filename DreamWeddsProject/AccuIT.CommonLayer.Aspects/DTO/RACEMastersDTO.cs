using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class RACEMastersDTO
    {
        [DataMember]
        public List<RaceBrandMasterDTO> BrandMaster;
        [DataMember]
        public List<RaceProductCategoryDTO> ProductCategory;
        [DataMember]
        public List<RaceBrandCategoryMappingDTO> BrandCategoryMapping;
        [DataMember]
        public List<RacePOSMMasterDTO> POSMMaster;
        [DataMember]
        public List<RaceFixtureMasterDTO> FixtureMaster;
        [DataMember]
        public List<RacePOSMProductMappingDTO> POSMProductMapping;     
    }


    [DataContract]
    public class SyncRacePOSMProductMappingDTO
    {

        [DataMember]
        public bool HasMoreRows;
        [DataMember]
        public string MaxModifiedDate;

        [DataMember]
        public List<RacePOSMProductMappingDTO> Result;
    }


    [DataContract]
    public class RacePOSMProductMappingDTO
    {
        [DataMember]
        public int POSMProductMappingID { get; set; }
        [DataMember]
        public int POSMID { get; set; }
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    
    }

    [DataContract]
    public class SyncRaceBrandMasterDTO
    {
        [DataMember]
        public bool HasMoreRows { get; set; }
        [DataMember]
        public string MaxModifiedDate { get; set; }
        [DataMember]
        public List<RaceBrandMasterDTO> Result;
    }

    [DataContract]
    public class RaceBrandMasterDTO
    {
        [DataMember]
        public byte BrandID { get; set; }
        [DataMember]
        public string BrandCode { get; set; }
        [DataMember]
        public string BrandName { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        //[DataMember]
        //public ICollection<RaceBrandCategoryMappingDTO> RaceBrandCategoryMappings { get; set; }
        //[DataMember]
        //public ICollection<RaceProductMasterDTO> RaceProductMasters { get; set; }
    }

    [DataContract]
    public class SyncRaceBrandCategoryMappingDTO
    {
        [DataMember]
        public bool HasMoreRows { get; set; }
        [DataMember]
        public string MaxModifiedDate { get; set; }
        [DataMember]
        public List<RaceBrandCategoryMappingDTO> Result;
    }

    [DataContract]
    public class RaceBrandCategoryMappingDTO
    {
        [DataMember]
        public int BrandCategoryMappingID { get; set; }
        [DataMember]
        public byte BrandID { get; set; }
        [DataMember]
        public int CompProductGroupID { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        //[DataMember]
        //public virtual RaceBrandMasterDTO RaceBrandMaster { get; set; }
        //[DataMember]
        //public virtual RaceProductCategoryDTO RaceProductCategory { get; set; }
    }

    [DataContract]
    public class SyncRaceProductCategoryDTO
    {
        [DataMember]
        public bool HasMoreRows { get; set; }
        [DataMember]
        public string MaxModifiedDate { get; set; }
        [DataMember]
        public List<RaceProductCategoryDTO> Result;
    }

    [DataContract]
    public class RaceProductCategoryDTO
    {
        [DataMember]
        public int CompProductGroupID { get; set; }
        [DataMember]
        public string ProductGroupName { get; set; }
        [DataMember]
        public string ProductGroupCode { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        //public virtual ICollection<RaceBrandCategoryMappingDTO> RaceBrandCategoryMappings { get; set; }
    }

    [DataContract]
    public class SyncRacePOSMMasterDTO
    {

        [DataMember]
        public bool HasMoreRows;
        [DataMember]
        public string MaxModifiedDate;

        [DataMember]
        public List<RacePOSMMasterDTO> Result;
    }

    [DataContract]
    public class RacePOSMMasterDTO
    {
       
        [DataMember]
        public int POSMID { get; set; }
        [DataMember]
        public string POSMCode { get; set; }
        [DataMember]
        public string POSMName { get; set; }
        public string POSMDescription { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        [DataMember]
        public string POSMType { get; set; }
        public string POSMCategory { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }


    [DataContract]
    public class SyncRaceFixtureMasterDTO
    {

        [DataMember]
        public bool HasMoreRows;
        [DataMember]
        public string MaxModifiedDate;

        [DataMember]
        public List<RaceFixtureMasterDTO> Result;
    }

    [DataContract]
    public partial class RaceFixtureMasterDTO
    {

        [DataMember]
        public int FixtureID { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string SubCategory { get; set; }
        [DataMember]
        public string CategoryGroups { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        [DataMember]
        public bool IsCompetitorAvailable { get; set; }
        [DataMember]
        public bool IsRowAvailable { get; set; }
        [DataMember]
        public bool IsColumnAvailable { get; set; }
        public List<RaceFixtureCategoryMappingDTO> RaceFixtureCategoryMappings { get; set; }

        //[DataMember]
        //public int FixtureID { get; set; }
        //[DataMember]
        //public string Category { get; set; }
        // [DataMember]
        //public string SubCategory { get; set; }
        // [DataMember]
        // public string CategoryGroups { get; set; }
        //[DataMember]
        //public bool IsDeleted { get; set; }
        //public System.DateTime CreatedDate { get; set; }
        //[DataMember]
        //public bool IsCompetitorAvailable { get; set; }
        //[DataMember]
        //public bool IsRowAvailable { get; set; }
        //[DataMember]
        //public bool IsColumnAvailable { get; set; }
        //public List<RaceFixtureCategoryMappingDTO> RaceFixtureCategoryMappings { get; set; }

        
    }
    [DataContract]
    public partial class RaceFixtureCategoryMappingDTO
    {
        public int FixtureCategoryMappingID { get; set; }
        public int FixtureID { get; set; }
        public int CompProductGroupID { get; set; }
        public bool IsDeleted { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }

    [DataContract]
    public class RaceProductMasterOutputDTO
    {
        [DataMember]
        public List<RaceProductMasterDTO> Products;
        [DataMember]
        public bool HasMoreRows;
        [DataMember]
        public string MaxModifiedDate { get; set; }

    }
    [DataContract]
    public class RaceProductMasterDTO
    {
        [DataMember]
        public int ProductID { get; set; }
        [DataMember]
        public string ProductType { get; set; }
        [DataMember]
        public string ProductGroup { get; set; }
        [DataMember]
        public string ProductCategory { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public byte BrandID { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

    }

}
