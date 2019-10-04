using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.DTO
{
    [DataContract]
    public class WeddingDTO
    {
        [DataMember]
        public int WeddingID { get; set; }

        [DisplayFormat(DataFormatString = "{dd MM yyyy}")]
        [DataMember]
        public string WeddingDate { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string WeddingStyle { get; set; }

        [DataMember]
        public string IconUrl { get; set; }
        [DataMember]
        public int TemplateID { get; set; }
        [DataMember]

        public bool IsDeleted { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string BackgroundImage { get; set; }
        [DataMember]
        public string Quote { get; set; }
        [DataMember]
        public string FbPageUrl { get; set; }
        [DataMember]
        public string VideoUrl { get; set; }
        [DataMember]
        public string TemplateName { get; set; }
        [DataMember]
        public string TemplateImageUrl { get; set; }
        [DataMember]
        public string TemplatePreviewUrl { get; set; }
        [DataMember]
        public string SubscriptionEndDate { get; set; }
        [DataMember]
        public int UserWeddingSubscriptionID { get; set; }
                   
        [DataMember]
        public bool IsLoveMarriage { get; set; }
        public UserWeddingSubscriptionDTO UserWeddingSubscription { get; set; }
    }

    [DataContract]
    public class BrideAndMaidDTO
    {
        [DataMember]
        public int BrideAndMaidID { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        [DisplayFormat(DataFormatString = "{0:mm dd yyyy}", ApplyFormatInEditMode = true)]
        public string DateofBirth { get; set; }
        [DataMember]
        public int WeddingID { get; set; }

        [DataMember]
        public bool IsBride { get; set; }

        [DataMember]
        public int RelationWithBride { get; set; }
        [DataMember]
        public string strRelationWithBride { get; set; }
        [DataMember]
        public string Imageurl { get; set; }
        //[DataMember]
        //public int CreatedBy { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        //[DataMember]
        //public Nullable<int> ModifiedBy { get; set; }
        [DataMember]
        public string AboutBrideMaid { get; set; }
        [DataMember]
        public string FbUrl { get; set; }
        [DataMember]
        public string GoogleUrl { get; set; }
        [DataMember]
        public string InstagramUrl { get; set; }
        [DataMember]
        public string LinkedinUrl { get; set; }
    }
    [DataContract]
    public class GroomAndMenDTO
    {
        [DataMember]
        public int GroomAndMenID { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string DateofBirth { get; set; }
        [DataMember]
        public int WeddingID { get; set; }

        [DataMember]
        public bool IsGroom { get; set; }
        //[DataMember]
        //public Nullable<int> RelationWithGroom { get; set; }
        [DataMember]
        public int RelationWithGroom { get; set; }
        [DataMember]
        public string strRelationWithGroom { get; set; }
        [DataMember]
        public string Imageurl { get; set; }
        //[DataMember]
        //public int CreatedBy { get; set; }
        //[DataMember]
        //public bool IsDeleted { get; set; }
        //[DataMember]
        //public bool IsActive { get; set; }
        //[DataMember]
        //public Nullable<int> ModifiedBy { get; set; }
        [DataMember]
        public string AboutMen { get; set; }
        [DataMember]
        public string FbUrl { get; set; }
        [DataMember]
        public string GoogleUrl { get; set; }
        [DataMember]
        public string InstagramUrl { get; set; }
        [DataMember]
        public string LinkedinUrl { get; set; }
    }
    [DataContract]
    public class WeddingEventDTO
    {
        [DataMember]
        public int WeddingEventID { get; set; }
        [DataMember]
        public string EventDate { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public int WeddingID { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }
        [DataMember]
        public  List<VenueDTO> Venues { get; set; }
    }
    [DataContract]
    public class VenueDTO
    {
        [DataMember]
        public int VenueID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string VenueImageUrl { get; set; }
        [DataMember]
        public string VenueWebsite { get; set; }
        [DataMember]
        public string OwnerName { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string VenuePhone { get; set; }
        [DataMember]
        public string VenueMobile { get; set; }
        [DataMember]
        public int WeddingEventID { get; set; }
        [DataMember]
        public AddressMasterDTO VenueAddress { get; set; }
    }

    [DataContract]
    public class TimeLineDTO
    {
        [DataMember]
        public int TimeLineID { get; set; }
        [DataMember]
        public string StoryDate { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Story { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public int WeddingID { get; set; }
        [DataMember]
        public string Location { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }

    }

    [DataContract]
    public class WeddingGalleryDTO
    {
        [DataMember]
        public int WeddingGalleryID { get; set; }
        [DataMember]
        public string ImageTitle { get; set; }
        [DataMember]
        public string ImageName { get; set; }
       // [DataMember]
        public string DateTaken { get; set; }
       // [DataMember]
        //public string Place { get; set; }
        [DataMember]
        public int WeddingID { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }


    }
    [DataContract]
    public class RSVPDetailDTO
    {
        public int RSVPID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsComing { get; set; }
        public Nullable<System.DateTime> FromDate { get; set; }
        public Nullable<System.DateTime> ToDate { get; set; }
        public Nullable<int> GuestCount { get; set; }
        public byte PreferredFood { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string SpecialNote { get; set; }
        public string ImageUrl { get; set; }
        public bool IsDeleted { get; set; }
        public int WeddingID { get; set; }
        public string ParticipatingInEvents { get; set; }
        public string ComingFromCity { get; set; }
        public Nullable<int> PreferredStayIn { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<int> CreatedBy { get; set; }

    }


    [DataContract]
    public class EventAndVenueDTO
    {
        [DataMember]
        public int WeddingEventID { get; set; }
        [DataMember]
        public string EventDate { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public int WeddingID { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public bool IsDeleted { get; set; }
        [DataMember]
        public bool IsActive { get; set; }
        [DataMember]
        public string StartTime { get; set; }
        [DataMember]
        public string EndTime { get; set; }

        [DataMember]
        public int VenueID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int AddressID { get; set; }
        [DataMember]
        public string VenueImageUrl { get; set; }
        [DataMember]
        public string VenueWebsite { get; set; }
        [DataMember]
        public string OwnerName { get; set; }
        [DataMember]
        public string VenuePhone { get; set; }
        [DataMember]
        public string VenueMobile { get; set; }

        [DataMember]
        public string Address1 { get; set; }
        [DataMember]
        public string Address2 { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public int Country { get; set; }
        [DataMember]
        public int PinCode { get; set; }
        [DataMember]
        public int AddressType { get; set; }
        [DataMember]
        public int AddressStatus { get; set; }
        [DataMember]
        public string Lattitude { get; set; }
        [DataMember]
        public string Longitude { get; set; }


    }
}
