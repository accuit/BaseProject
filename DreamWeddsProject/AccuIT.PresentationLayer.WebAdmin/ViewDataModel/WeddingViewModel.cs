using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.BusinessLayer.Services.BO;
using System.Web.Mvc;
using System.Runtime.Serialization;

namespace AccuIT.PresentationLayer.WebAdmin.ViewDataModel
{
    [DataContract]
    public class WeddingViewModel
    {
        public UserProfileBO userProfile { get; set; }
        public TemplateMasterBO userTemplate { get; set; }
        public List<TemplateMasterBO> listTemplates { get; set; }
        public List<UserWeddingSubscriptionBO> userSubscriptions { get; set; }
        [DataMember]
        public WeddingBO WeddingBO { get; set; }
        public BrideAndMaidBO BrideAndMaidsBO { get; set; }
        public GroomAndManBO GroomAndMenBO { get; set; }
        [DataMember]
        public WeddingEventBO WeddingEventsBO { get; set; }
        public WeddingEventDTO WeddingEventsDTO { get; set; }
        public RSVPDetailBO rsvpDetailsBO { get; set; }
        public TimeLineBO TimeLineBO { get; set; }
        public VenueBO venueBO { get; set; }
    }

}