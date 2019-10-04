using System;
using System.Runtime.Serialization;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    public class UserStoresDTO
    {
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public int StoreID { get; set; }
        public bool IsActive { get; set; }
        public long UserID { get; set; }
        public long UserRoleID { get; set; }
        public long StoreUserID { get; set; }
        public string BeatSummary { get; set; }
        public string City { get; set; }
        public string ChannelType { get; set; }
    }
}
