using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class ProductTypeHierarchyDTO
    {
        public ProductTypeHierarchyDTO()
        {
            ProductGroups = new List<ProductGroupDTO>();
        }
        [DataMember]
        public string ProductTypeCode { get; set; }
        [DataMember]
        public string ProductTypeName { get; set; }
        [DataMember]
        public List<ProductGroupDTO> ProductGroups
        {
            get;
            set;
        }
    }

    [DataContract]
    public class ProductGroupDTO
    {
        public ProductGroupDTO()
        {
            ProductCategories = new List<ProductCategoryDTO>();
        }
        [DataMember]
        public string ProductGroupCode { get; set; }
        [DataMember]
        public string ProductGroupName { get; set; }
        [DataMember]
        public List<ProductCategoryDTO> ProductCategories
        {
            get;
            set;
        }
    }

    [DataContract]
    public class ProductCategoryDTO
    {
        [DataMember]
        public string CategoryCode { get; set; }
        [DataMember]
        public string CategoryName { get; set; }
    }
}
