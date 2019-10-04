using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccuIT.PersistenceLayer.Repository.Entities;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.BusinessLayer.Services.BO;

namespace AccuIT.PersistenceLayer.Repository.Contracts
{
    public interface IWeddingRepository
    {
        List<Wedding> GetUserWeddingDetail(int UserID);

        List<DreamWeddsBlog> GetDreamWeddsBlog();
        DreamWeddsBlog GetBlogDetails(int blogID);

        List<FAQ> GetDreamWeddsFAQ();
        Wedding GetWeddingDetailByID(int WeddingId);


        List<BrideAndMaid> GetWeddingBrideMaids(int weddingID);
        List<GroomAndMan> GetWeddingGroomMen(int weddingID);
        List<WeddingEvent> GetWeddingEvents(int weddingID);
        List<TimeLine> GetWeddingTimeLines(int weddingID);
        List<WeddingGallery> GetWeddingGallery(int weddingID);

        WeddingEvent GetEventDetailsByID(int eventID);
        BrideAndMaid GetBrideDetailsByID(int brideMaidID);
        GroomAndMan GetGroomDetailsByID(int groomMenID);
        TimeLine GetTimeLineDetailsByID(int timeLineID);
        AddressMaster GetVenueAddress(int typeID, int PKID);

        bool DeleteEventDetailsByID(int eventID, int UserID);



        List<UserWeddingSubscription> GetUserWeddingSubscriptions(int userID);

        List<OrderMaster> GetUserOrders(int UserID);
        OrderMaster GetOrderByID(int OrderID);
        int SubmitUserOrder(OrderMaster order);

        int SubmitUserWeddingDetail(int UserID, Wedding wedding);
        bool UpdateWeddingSubscription(UserWeddingSubscription subscription);
        int SubmitWeddingEvent(int UserID, WeddingEvent events);
        int SubmitVenue(int WeddingID, Venue venue);
        int SubmitAddress(int userID, AddressMaster address);
        int SubmitBrideMaids(int WeddingID, BrideAndMaid bride);
        int SubmitGroomMen(int WeddingID, GroomAndMan groom);
        int SubmitTimeLine(int weddingID, TimeLine timeline);
        int SubmitGallery(int UserID, WeddingGallery gallery);
    }
}
