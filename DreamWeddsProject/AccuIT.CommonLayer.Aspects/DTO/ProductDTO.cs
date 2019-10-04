using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{

    [DataContract]
    public class SyncProductDTO
    {

        [DataMember]
        public bool HasMoreRows;
        [DataMember]
        public string MaxModifiedDate;

        [DataMember]
        public List<ProductDTO> Result;
    }

    
    public class ProductDTO
    {
        [DataMember]
        public string ProductTypeCode { get; set; }
        [DataMember]
        public string ProductTypeName { get; set; }
        [DataMember]
        public int ProductTypeID { get; set; }
        [DataMember]
        public string ProductGroupCode { get; set; }
        [DataMember]
        public string ProductGroupName { get; set; }
        [DataMember]
        public int ProductGroupID { get; set; }
        [DataMember]
        public string CategoryCode { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
        [DataMember]
        public int ProductCategoryID { get; set; }
        [DataMember]
        public int ModelTypeID { get; set; }
        [DataMember]
        public string BasicModelCode { get; set; }
        [DataMember]
        public string BasicModelName { get; set; }
        [DataMember]
        public string SKUCode { get; set; }
        [DataMember]
        public string SKUName { get; set; }
        [DataMember]
        public Nullable<decimal> DealerPrice { get; set; }
        [DataMember]
        public int ProductID { get; set; }
        //[DataMember]
        //public System.DateTime CreatedDate { get; set; }
        [DataMember]
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }
}
