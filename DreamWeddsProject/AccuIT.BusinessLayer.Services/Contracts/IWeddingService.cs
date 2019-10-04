using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AccuIT.BusinessLayer.Services.BO;
using AccuIT.CommonLayer.Aspects.DTO;
using AccuIT.CommonLayer.Aspects.ReportBO;
using AccuIT.CommonLayer.Aspects.Utilities;

namespace AccuIT.BusinessLayer.Services.Contracts
{
    public interface IWeddingService
    {
        List<WeddingBO> GetUserWeddingDetail(int UserID);

        WeddingBO GetWeddingDetailByID(int WeddingId);

        List<DreamWeddsBlogBO> GetDreamWeddsBlog();
        DreamWeddsBlogBO GetBlogDetails(int blogID);
        List<FAQBO> GetDreamWeddsFAQ();

        

        List<BrideAndMaidBO> GetWeddingBrideMaids(int weddingID);
        List<GroomAndManBO> GetWeddingGroomMen(int weddingID);
        List<WeddingEventBO> GetWeddingEvents(int weddingID);
        List<TimeLineBO> GetWeddingTimeLines(int weddingID);
        List<WeddingGalleryBO> GetWeddingGallery(int weddingID);

        BrideAndMaidBO GetBrideDetailsByID(int brideMaidID);
        GroomAndManBO GetGroomDetailsByID(int groomMenID);
        TimeLineBO GetTimeLineDetailsByID(int timeLineID);
        WeddingEventBO GetEventDetailsByID(int eventID);

        bool DeleteEventDetailsByID(int eventID, int UserID);

        int SubmitUserWeddingDetail(int UserID, WeddingBO wedding);
        List<UserWeddingSubscriptionBO> GetUserWeddingSubscriptions(int userID);

        List<OrderMasterBO> GetUserOrders(int UserID);
        OrderMasterBO GetOrderByID(int OrderID);
        int SubmitUserOrder(OrderMasterBO order);

        int SubmitWeddingEvent(int UserId, WeddingEventBO eventBo);
        int SubmitVenue(int WeddingID, VenueBO venueBO);
        int SubmitAddress(int userID, AddressMasterBO address);
        int SubmitBrideMaids(int WeddingID, BrideAndMaidBO brideBO);
        int SubmitGroomMen(int WeddingID, GroomAndManBO groomBO);
        int SubmitTimeLine(int weddingID, TimeLineBO timelineBO);
        int SubmitGallery(int UserID, WeddingGalleryBO galleryBO);
    }
}
