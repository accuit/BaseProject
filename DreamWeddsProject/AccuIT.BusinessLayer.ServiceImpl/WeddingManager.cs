using AccuIT.BusinessLayer.Base;
using AccuIT.BusinessLayer.Services.BO;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.CommonLayer.Aspects.ReportBO;
using AccuIT.CommonLayer.Aspects.Security;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.PersistenceLayer.Repository.Contracts;
using AccuIT.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using AccuIT.CommonLayer.EntityMapper;
using AccuIT.CommonLayer.Aspects.Logging;

namespace AccuIT.BusinessLayer.ServiceImpl
{
    public class WeddingManager : WeddingBaseService, IWeddingService
    {
        #region Properties

        /// <summary>
        /// Property to inject the user persistence layer object invocation
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.EMP_REPOSITORY)]
        public IUserRepository UserRepository { get; set; }

        /// <summary>
        /// Property to inject the user persistence layer object invocation
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.SYSTEM_REPOSITORY)]
        public ISystemRepository SystemRepository { get; set; }

        /// <summary>
        /// Property to inject the user persistence layer object invocation
        /// </summary>
        [Microsoft.Practices.Unity.Dependency(ContainerDataLayerInstanceNames.WEDDING_REPOSITORY)]
        public IWeddingRepository WeddingRepository { get; set; }

        public static List<CommonSetupBO> CommonFields { get; set; }

        #endregion
        string weddingFolder = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.WeddingFolder.ToString());

        public string GetWeddingCommonFieldDisplayText(int DisplayValue, string SubType)
        {
            List<CommonSetupBO> commonValues = new List<CommonSetupBO>();
            if (CommonFields == null)
            {
                ObjectMapper.Map(SystemRepository.GetCommonSetup(0, null, Convert.ToString((int)AspectEnums.CommonTableMainType.Wedding)), commonValues);
                CommonFields = commonValues;
            }
            return CommonFields.Where(x => x.DisplayValue == DisplayValue && x.SubType == SubType).FirstOrDefault().DisplayText;

        }
        public List<WeddingBO> GetUserWeddingDetail(int UserID)
        {
            List<WeddingBO> weddings = new List<WeddingBO>();
            ObjectMapper.Map(WeddingRepository.GetUserWeddingDetail(UserID), weddings);
            foreach (var wedding in weddings)
            {
                FormatWeddingData(wedding);
            }
            return weddings;
        }

        private WeddingBO FormatWeddingData(WeddingBO wedding)
        {
            wedding.strWeddingStyle = GetWeddingCommonFieldDisplayText(wedding.WeddingStyle, AspectEnums.CommonFieldSubType.Style.ToString());
            wedding.WeddingEvents.Where(x => x.IsDeleted == false);
            foreach (var bride in wedding.BrideAndMaids.Where(x => !x.IsDeleted))
            {
                if (bride.RelationWithBride > 0)
                    bride.strRelationWithBride = GetWeddingCommonFieldDisplayText(Convert.ToInt32(bride.RelationWithBride), AspectEnums.CommonFieldSubType.Relation.ToString());
                else
                    bride.strRelationWithBride = "Bride";
            }

            foreach (var groom in wedding.GroomAndMen.Where(x => !x.IsDeleted))
            {
                if (groom.IsGroom == false && groom.RelationWithGroom > 0)
                    groom.strRelationWithGroom = GetWeddingCommonFieldDisplayText(Convert.ToInt32(groom.RelationWithGroom), "Relation");
                else
                    groom.strRelationWithGroom = "Groom";
            }


            return wedding;
        }


        public WeddingBO GetWeddingDetailByID(int WeddingId)
        {
            WeddingBO weddingBO = new WeddingBO();
            ObjectMapper.Map(WeddingRepository.GetWeddingDetailByID(WeddingId), weddingBO);
            if (weddingBO.UserWeddingSubscriptions.Count > 0)
            {
                weddingBO.SubscriptionEndDate = weddingBO.UserWeddingSubscriptions.Where(x => x.WeddingID == WeddingId).First().EndDate;
                weddingBO.UserWeddingSubscriptionID = weddingBO.UserWeddingSubscriptions.Last().UserWeddingSubscrptionID;
            }

            weddingBO.BackgroundImage = string.IsNullOrEmpty(weddingBO.BackgroundImage) ? "../../assets/img/image_placeholder.jpg" : weddingBO.BackgroundImage.Replace("{D}", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.USERFOLDER).ToString()).Replace("{0}", weddingBO.WeddingID.ToString());
            return weddingBO;
        }

        public List<DreamWeddsBlogBO> GetDreamWeddsBlog()
        {
            List<DreamWeddsBlogBO> blogs = new List<DreamWeddsBlogBO>();
            ObjectMapper.Map(WeddingRepository.GetDreamWeddsBlog(), blogs);
            return blogs;
        }

        public List<FAQBO> GetDreamWeddsFAQ()
        {
            List<FAQBO> faq = new List<FAQBO>();
            ObjectMapper.Map(WeddingRepository.GetDreamWeddsFAQ(), faq);
            return faq;
        }
        public DreamWeddsBlogBO GetBlogDetails(int blogID)
        {
            DreamWeddsBlogBO blog = new DreamWeddsBlogBO();
            ObjectMapper.Map(WeddingRepository.GetBlogDetails(blogID), blog);
            return blog;
        }


        public List<BrideAndMaidBO> GetWeddingBrideMaids(int weddingID)
        {
            List<BrideAndMaidBO> bridemaids = new List<BrideAndMaidBO>();
            ObjectMapper.Map(WeddingRepository.GetWeddingBrideMaids(weddingID), bridemaids);
            foreach (var item in bridemaids)
            {
                item.strDateofBirth = Convert.ToDateTime(item.DateofBirth).ToString("dd-MMM-yyyy");
                item.Imageurl = item.Imageurl.Replace("{D}", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.USERFOLDER).ToString())
                    .Replace("{0}", item.BrideAndMaidID.ToString());
                if (item.IsBride == false && item.RelationWithBride > 0)
                    item.strRelationWithBride = GetWeddingCommonFieldDisplayText(Convert.ToInt32(item.RelationWithBride), "Relation");
                else
                    item.strRelationWithBride = "Bride";
            }
            return bridemaids;
        }
        public List<GroomAndManBO> GetWeddingGroomMen(int weddingID)
        {
            List<GroomAndManBO> groommen = new List<GroomAndManBO>();
            ObjectMapper.Map(WeddingRepository.GetWeddingGroomMen(weddingID), groommen);
            foreach (var item in groommen)
            {
                item.strDateofBirth = Convert.ToDateTime(item.DateofBirth).ToString("dd-MMM-yyyy");
                item.Imageurl = item.Imageurl.Replace("{D}", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.USERFOLDER).ToString())
                    .Replace("{0}", item.GroomAndMenID.ToString());
                if (item.IsGroom == false && item.RelationWithGroom > 0)
                    item.strRelationWithGroom = GetWeddingCommonFieldDisplayText(Convert.ToInt32(item.RelationWithGroom), "Relation");
                else
                    item.strRelationWithGroom = "Groom";
            }
            return groommen;
        }
        public List<WeddingEventBO> GetWeddingEvents(int weddingID)
        {
            List<WeddingEventBO> events = new List<WeddingEventBO>();
            ObjectMapper.Map(WeddingRepository.GetWeddingEvents(weddingID), events);
            if (events.Count > 0)
            {
                foreach (var myevent in events)
                {
                    myevent.ImageUrl = myevent.ImageUrl.Replace("{D}", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.USERFOLDER).ToString())
                   .Replace("{0}", myevent.WeddingEventID.ToString());

                    foreach (var venue in myevent.Venues)
                    {
                        venue.VenueImageUrl = venue.VenueImageUrl.Replace("{D}", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.USERFOLDER).ToString())
                   .Replace("{0}", venue.VenueID.ToString());

                        AddressMasterBO address = new AddressMasterBO();
                        ObjectMapper.Map(WeddingRepository.GetVenueAddress((int)AspectEnums.AddressOwnerType.Venue, venue.VenueID), address);
                        venue.VenueAddress = address;
                    }
                }
            }
            return events;
        }
        public List<TimeLineBO> GetWeddingTimeLines(int weddingID)
        {
            List<TimeLineBO> timelines = new List<TimeLineBO>();
            ObjectMapper.Map(WeddingRepository.GetWeddingTimeLines(weddingID), timelines);
            foreach (var item in timelines)
            {
                item.ImageUrl = item.ImageUrl.Replace("{D}", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.USERFOLDER).ToString())
                   .Replace("{0}", item.TimeLineID.ToString());
                item.ImageUrl = item.ImageUrl.Replace("{0}", item.TimeLineID.ToString());
            }
            return timelines;
        }
        public List<WeddingGalleryBO> GetWeddingGallery(int weddingID)
        {
            List<WeddingGalleryBO> gallery = new List<WeddingGalleryBO>();
            ObjectMapper.Map(WeddingRepository.GetWeddingGallery(weddingID), gallery);
            foreach (var image in gallery)
            {
                image.ImageUrl = image.ImageUrl.Replace("{D}", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.USERFOLDER).ToString())
                    .Replace("{0}", image.WeddingGalleryID.ToString());
            }
            return gallery;
        }

        public WeddingEventBO GetEventDetailsByID(int eventID)
        {
            WeddingEventBO weddingevent = new WeddingEventBO();
            ObjectMapper.Map(WeddingRepository.GetEventDetailsByID(eventID), weddingevent);
            weddingevent.ImageUrl = weddingevent.ImageUrl.Replace("{D}", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.USERFOLDER).ToString())
                    .Replace("{0}", weddingevent.WeddingEventID.ToString());
            foreach (var venue in weddingevent.Venues)
            {
                AddressMasterBO address = new AddressMasterBO();
                ObjectMapper.Map(WeddingRepository.GetVenueAddress((int)AspectEnums.AddressOwnerType.Venue, venue.VenueID), address);
                venue.VenueAddress = address;
                venue.VenueImageUrl = venue.VenueImageUrl.Replace("{D}", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.USERFOLDER).ToString())
                    .Replace("{0}", venue.VenueID.ToString());
            }
            return weddingevent;
        }
        public BrideAndMaidBO GetBrideDetailsByID(int brideMaidID)
        {
            BrideAndMaidBO bride = new BrideAndMaidBO();
            ObjectMapper.Map(WeddingRepository.GetBrideDetailsByID(brideMaidID), bride);
            bride.strDateofBirth = Convert.ToDateTime(bride.DateofBirth).ToString("dd-MMM-yyyy");
            bride.Imageurl = bride.Imageurl.Replace("{D}", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.USERFOLDER).ToString())
                   .Replace("{0}", bride.BrideAndMaidID.ToString());
            //if (bride.IsBride == false && bride.RelationWithBride > 0)
            //    bride.strRelationWithBride = GetWeddingCommonFieldDisplayText(Convert.ToInt32(bride.RelationWithBride), "Relation");
            //else
            //    bride.strRelationWithBride = "Bride";

            return bride;
        }
        public GroomAndManBO GetGroomDetailsByID(int groomManID)
        {
            GroomAndManBO groom = new GroomAndManBO();
            ObjectMapper.Map(WeddingRepository.GetGroomDetailsByID(groomManID), groom);
            groom.strDateofBirth = Convert.ToDateTime(groom.DateofBirth).ToString("dd-MMM-yyyy");
            groom.Imageurl = groom.Imageurl.Replace("{D}", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.USERFOLDER).ToString())
                   .Replace("{0}", groom.GroomAndMenID.ToString());
            if (groom.IsGroom == false && groom.RelationWithGroom > 0)
                groom.strRelationWithGroom = GetWeddingCommonFieldDisplayText(Convert.ToInt32(groom.RelationWithGroom), "Relation");
            else
                groom.strRelationWithGroom = "Groom";

            return groom;
        }
        public TimeLineBO GetTimeLineDetailsByID(int timeLineID)
        {
            TimeLineBO timeline = new TimeLineBO();
            ObjectMapper.Map(WeddingRepository.GetTimeLineDetailsByID(timeLineID), timeline);
            timeline.ImageUrl = timeline.ImageUrl.Replace("{D}", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.USERFOLDER).ToString()).Replace("{0}", timeline.TimeLineID.ToString());
            return timeline;
        }
        public List<UserWeddingSubscriptionBO> GetUserWeddingSubscriptions(int userID)
        {
            ActivityLog.SetLog("WeddingManager > GetUserWeddingSubscriptions initiated.", LogLoc.INFO);
            List<UserWeddingSubscriptionBO> userData = new List<UserWeddingSubscriptionBO>();
            ObjectMapper.Map(WeddingRepository.GetUserWeddingSubscriptions(userID), userData);

            foreach (var item in userData)
            {
                item.TemplateMaster = SystemRepository.GetBasicTemplateInfo(item.TemplateID);

                if (item.WeddingID == 0 || item.Wedding == null)
                {
                    item.Wedding = new WeddingBO
                    {
                        WeddingDate = DateTime.Now,
                        WeddingID = 0,
                        Title = "Wedding not created.",
                        BackgroundImage = item.TemplateMaster.ThumbnailImageUrl,
                        TemplateImageUrl = item.TemplateMaster.ThumbnailImageUrl,
                        TemplateName = item.TemplateMaster.TemplateName,
                        TemplatePreviewUrl = item.TemplateMaster.TemplateUrl
                    };
                }
                else
                {
                    item.Wedding.BackgroundImage = item.Wedding.BackgroundImage.Replace("{D}", AppUtil.GetAppSettings(AspectEnums.ConfigKeys.USERFOLDER).ToString())
                   .Replace("{0}", item.WeddingID.ToString());

                    item.Wedding.TemplateImageUrl = item.TemplateMaster.ThumbnailImageUrl;
                    item.Wedding.TemplateName = item.TemplateMaster.TemplateName;
                    item.Wedding.TemplatePreviewUrl = item.TemplateMaster.TemplatePreviewUrl;
                }
            }

            return userData;
        }


        public bool DeleteEventDetailsByID(int eventID, int UserID)
        {
            return WeddingRepository.DeleteEventDetailsByID(eventID, UserID);

        }

        public List<OrderMasterBO> GetUserOrders(int UserID)
        {
            List<OrderMasterBO> orders = new List<OrderMasterBO>();
            ObjectMapper.Map(WeddingRepository.GetUserOrders(UserID), orders);
            return orders;
        }
        public OrderMasterBO GetOrderByID(int OrderID)
        {
            OrderMasterBO order = new OrderMasterBO();
            ObjectMapper.Map(WeddingRepository.GetOrderByID(OrderID), order);
            return order;
        }
        public int SubmitUserOrder(OrderMasterBO orderBO)
        {
            OrderMaster order = new OrderMaster();
            ObjectMapper.Map(orderBO, order);
            return WeddingRepository.SubmitUserOrder(order);
        }

        public int SubmitUserWeddingDetail(int UserID, WeddingBO weddingbo)
        {
            Wedding wedding = new Wedding();
            ObjectMapper.Map(weddingbo, wedding);
            weddingbo.WeddingID = WeddingRepository.SubmitUserWeddingDetail(UserID, wedding);

            //Update wedding Subscriptions
            UserWeddingSubscriptionBO BO = new UserWeddingSubscriptionBO();
            BO.UserWeddingSubscrptionID = weddingbo.UserWeddingSubscriptionID;
            BO.WeddingID = weddingbo.WeddingID;

            UserWeddingSubscription entity = new UserWeddingSubscription();
            ObjectMapper.Map(BO, entity);
            bool success = WeddingRepository.UpdateWeddingSubscription(entity);

            return weddingbo.WeddingID;

        }
        public int SubmitWeddingEvent(int userID, WeddingEventBO eventBO)
        {
            WeddingEvent weddEvent = new WeddingEvent();
            ObjectMapper.Map(eventBO, weddEvent);
            int eventID = WeddingRepository.SubmitWeddingEvent(userID, weddEvent);
            //if (eventID > 0)
            //{
            //    Venue venue = new Venue();
            //    ObjectMapper.Map(eventBO.Venue, venue);
            //    venue.WeddingEventID = eventID;
            //    int venueID = WeddingRepository.SubmitVenue(eventBO.WeddingID, venue);
            //    if (venueID > 0)
            //    {
            //        AddressMaster address = new AddressMaster();
            //        eventBO.Venue.VenueAddress.AddressOwnerType = (int)AspectEnums.AddressOwnerType.Venue;
            //        eventBO.Venue.VenueAddress.AddressOwnerTypePKID = venueID;
            //        ObjectMapper.Map(eventBO.Venue.VenueAddress, address);
            //        eventID = WeddingRepository.SubmitAddress(userID, address) > 0 ? eventID : 0;
            //    }
            //    else
            //        eventID = 0;
            //}
            return eventID;

        }
        public int SubmitVenue(int weddingID, VenueBO weddingbo)
        {
            Venue wedding = new Venue();
            ObjectMapper.Map(weddingbo, wedding);
            return WeddingRepository.SubmitVenue(weddingID, wedding);
        }
        public int SubmitAddress(int userID, AddressMasterBO addressBO)
        {
            AddressMaster address = new AddressMaster();
            ObjectMapper.Map(addressBO, address);
            return WeddingRepository.SubmitAddress(userID, address);
        }
        public int SubmitBrideMaids(int UserID, BrideAndMaidBO bridemaid)
        {

            BrideAndMaid bridemaids = new BrideAndMaid();
            ObjectMapper.Map(bridemaid, bridemaids);
            return WeddingRepository.SubmitBrideMaids(UserID, bridemaids);
        }
        public int SubmitGroomMen(int UserID, GroomAndManBO groomBO)
        {
            GroomAndMan groom = new GroomAndMan();
            ObjectMapper.Map(groomBO, groom);
            return WeddingRepository.SubmitGroomMen(UserID, groom);
        }
        public int SubmitTimeLine(int UserID, TimeLineBO timelineBO)
        {
            TimeLine tline = new TimeLine();
            ObjectMapper.Map(timelineBO, tline);
            return WeddingRepository.SubmitTimeLine(UserID, tline);
        }
        public int SubmitGallery(int UserID, WeddingGalleryBO galleryBO)
        {
            WeddingGallery gallery = new WeddingGallery();
            ObjectMapper.Map(galleryBO, gallery);
            return WeddingRepository.SubmitGallery(UserID, gallery);
        }
    }
}
