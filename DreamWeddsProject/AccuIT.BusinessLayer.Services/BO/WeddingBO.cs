using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.BusinessLayer.Services.BO
{
    [DataContract]
    public class WeddingBO
    {
        public int WeddingID { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime WeddingDate { get; set; }
        public string Title { get; set; }
        public int WeddingStyle { get; set; }
        public string strWeddingStyle { get; set; }
        public string IconUrl { get; set; }
        public int TemplateID { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string TemplateName { get; set; }
        public string TemplateImageUrl { get; set; }
        public string TemplatePreviewUrl { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsLoveMarriage { get; set; }
        public string BackgroundImage { get; set; }
        public string Quote { get; set; }
        public string FbPageUrl { get; set; }
        public string VideoUrl { get; set; }
        public int? UserWeddingSubscriptionID { get; set; }

        public DateTime SubscriptionEndDate { get; set; }
        public List<BrideAndMaidBO> BrideAndMaids { get; set; }
        public List<GroomAndManBO> GroomAndMen { get; set; }
        public List<WeddingEventBO> WeddingEvents { get; set; }
        public List<RSVPDetailBO> RSVPDetails { get; set; }
        public List<WeddingGalleryBO> WeddingGalleries { get; set; }
        public List<TimeLineBO> TimeLines { get; set; }
        public WeddingEventBO WeddingEvent { get; set; }
        public List<UserWeddingSubscriptionBO> UserWeddingSubscriptions { get; set; }
    }
    [DataContract]
    public class TimeLineBO
    {
        public int TimeLineID { get; set; }
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}")]
        public System.DateTime StoryDate { get; set; }
        [DisplayFormat(DataFormatString = "{dd-MM-yyyy}")]
        public string strStoryDate { get; set; }
        public string Title { get; set; }
        public string Story { get; set; }
        public string ImageUrl { get; set; }
        public string Location { get; set; }
        public int WeddingID { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

    }
    [DataContract]
    public class BrideAndMaidBO
    {
        // [Display("Bride ID")]
        public int BrideAndMaidID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DisplayFormat(DataFormatString = "{0:mm dd yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateofBirth { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public string strDateofBirth { get; set; }

        //[DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        //public string modalDateofBirth { get; set; }
        public int WeddingID { get; set; }
        public bool IsBride { get; set; }
        public int RelationWithBride { get; set; }
        public string strRelationWithBride { get; set; }
        public string Imageurl { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string AboutBrideMaid { get; set; }
        public string fbUrl { get; set; }
        public string googleUrl { get; set; }
        public string instagramUrl { get; set; }
        public string lnkedinUrl { get; set; }
    }
    [DataContract]
    public class GroomAndManBO
    {
        public int GroomAndMenID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // [DisplayFormat(DataFormatString = "{0:mm dd yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateofBirth { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public string strDateofBirth { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}", ApplyFormatInEditMode = true)]
        public string modalDateofBirth { get; set; }
        public int WeddingID { get; set; }
        public bool IsGroom { get; set; }
        public int RelationWithGroom { get; set; }
        public string strRelationWithGroom { get; set; }
        public string Imageurl { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string AboutMen { get; set; }
        public string fbUrl { get; set; }
        public string googleUrl { get; set; }
        public string instagramUrl { get; set; }
        public string lnkedinUrl { get; set; }
    }
    [DataContract]
    public class WeddingEventBO
    {
        public int WeddingEventID { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public System.DateTime EventDate { get; set; }
        public string Title { get; set; }
        public int WeddingID { get; set; }
        public string ImageUrl { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        [DataType(DataType.Time), DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public System.DateTime StartTime { get; set; }
        [DataType(DataType.Time), DisplayFormat(DataFormatString = "{0:hh:mm tt}", ApplyFormatInEditMode = true)]
        public System.DateTime EndTime { get; set; }
        public string Aboutevent { get; set; }
        public string BackGroundImage { get; set; }
        public VenueBO Venue { get; set; }
        public virtual ICollection<VenueBO> Venues { get; set; }
        //public virtual WeddingBO Wedding { get; set; }
        public string strEventDate { get; set; }
        public string strStartTime { get; set; }
        public string strEndTime { get; set; }


    }
    [DataContract]
    public class WeddingGalleryBO
    {
        public int WeddingGalleryID { get; set; }
        public string ImageTitle { get; set; }
        public string ImageName { get; set; }
        public DateTime? DateTaken { get; set; }
        public string Place { get; set; }
        public int WeddingID { get; set; }
        public string ImageUrl { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public virtual WeddingBO Wedding { get; set; }
    }
    [DataContract]
    public class VenueBO
    {
        public int VenueID { get; set; }
        public string Name { get; set; }
        public int WeddingID { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string VenueImageUrl { get; set; }
        public string VenueBannerImageUrl { get; set; }
        public string VenueWebsite { get; set; }
        public string OwnerName { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string VenuePhone { get; set; }
        public string VenueMobile { get; set; }
        public Nullable<int> WeddingEventID { get; set; }
        public AddressMasterBO VenueAddress { get; set; }
        public string googleMapUrl { get; set; }
        public IEnumerable<AddressMasterBO> AddressMasters { get; set; }

    }
    [DataContract]
    public class RSVPDetailBO
    {
        public int RSVPID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsComing { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public Nullable<System.DateTime> FromDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
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

    public class DreamWeddsBlogBO
    {
        public int BlogID { get; set; }
        public string BlogName { get; set; }
        public string Title { get; set; }
        public string BlogSubject { get; set; }
        public string Quote { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public string SpecialNote { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
    public class FAQBO
    {
        public int FAQID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Website { get; set; }
        public bool IsMainQue { get; set; }
        public Nullable<int> sequence { get; set; }
    }

}
