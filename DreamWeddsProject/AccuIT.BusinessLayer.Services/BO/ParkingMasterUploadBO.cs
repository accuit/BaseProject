using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    public class ParkingRoleMasterBO
    {
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
        public string Description { get; set; }
        public Nullable<int> TeamID { get; set; }
        public Nullable<int> GeoID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public int ProfileLevel { get; set; }
        public bool IsAdmin { get; set; }
        public string CoverageType { get; set; }
        public Nullable<int> TargetOutlets { get; set; }
        public string MarketOffDays { get; set; }
        public bool IsReportProfile { get; set; }
        public bool IsEffectiveProfile { get; set; }
        public bool IsGeoTagMandate { get; set; }
        public bool IsGeoPhotoMandate { get; set; }
        public bool IsStoreProfileVisible { get; set; }
        public bool IsOfflineAccess { get; set; }
        public bool ShowPerformanceTab { get; set; }
        public bool IsRaceProfile { get; set; }
        public bool IsAttendanceMandate { get; set; }
        public bool IsGeoFencingApplicable { get; set; }
    }

    public class ParkingTeamMasterBO
    {
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int MaxLevel { get; set; }
        public int Status { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public long CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ParkingTeamSpocMasterBO
    {

        public string TeamCode { get; set; }
        public string LocationLevel { get; set; }
        public string LocationValue { get; set; }
        public string SpocCode { get; set; }
        public string EM1Code { get; set; }
        public string EM2Code { get; set; }
        public string HODCode { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<long> CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    }

    public class ParkingProductMasterBO
    {
        public long ProductId { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public string ProductName { get; set; }
        public string MeasureUnit { get; set; }
        public Nullable<decimal> UnitPrice { get; set; }
        public Nullable<int> Availability { get; set; }
        public System.DateTime DateCreated { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> DateModified { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public string Status { get; set; }
        public string ImagePath { get; set; }
        public string Product_Description { get; set; }
        public string SKUCode { get; set; }
        public string Brand { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<int> UserType { get; set; }
        public string SKUName { get; set; }

    }

    public class ProductImagesBO
    {
        public long ProductImageID { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string ImageUrl { get; set; }
        public string SKUCode { get; set; }
        public int ProductID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public bool IsPrimeImage { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }

    }

    public class ProductSepecificationsBO
    {
        public long SpecificID { get; set; }
        public string Header { get; set; }
        public string HeaderContent { get; set; }
        public string SKUCOde { get; set; }
        public long ProductID { get; set; }
        public Nullable<int> CategoryID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class ParkingStoreMasterBO
    {
        public int CompanyID { get; set; }
        public string State { get; set; }
        public string AccId { get; set; }
        public string ChannelType { get; set; }
        public string ContactPerson { get; set; }
        public string MobileNo { get; set; }
        public string EmailID { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string City { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string Lattitude { get; set; }
        public string Longitude { get; set; }
        public string PictureFileName { get; set; }
        public bool IsSynced { get; set; }
        public Nullable<System.DateTime> SyncDate { get; set; }
        public int GeoTagCount { get; set; }
        public Nullable<System.DateTime> LastGeoTagDate { get; set; }
        public string GeoTagFileName { get; set; }
        public string StoreClass { get; set; }
        public string StoreAddress { get; set; }
    }

    public  class ParkingRoleNormMasterBO
    {
        public int RoleID { get; set; }
        public System.DateTime CoverageDate { get; set; }
        public int TargetOutlets { get; set; }
        public bool IsDeleted { get; set; }
    }

    public  class ParkingCompanyMasterBO
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int DivisionID { get; set; }
    }

    public class ParkingGeoMasterBO
    {
        public int CompanyID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Nullable<int> Level { get; set; }
        public Nullable<int> ParentGeoID { get; set; }
    }

    public class ParkingGeoDefinitionsBO
    {
        public string GeoDefCode { get; set; }
        public string GeoDefName { get; set; }
        public string GeoCode { get; set; }
        public string ParentGeoDefCode { get; set; }
        public bool IsDeleted { get; set; }
    }


}
