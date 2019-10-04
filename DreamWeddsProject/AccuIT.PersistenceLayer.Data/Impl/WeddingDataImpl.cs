using AccuIT.CommonLayer.Aspects.Security;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.PersistenceLayer.Repository.Contracts;
using AccuIT.PersistenceLayer.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Net.Mail;
using System.Transactions;
using System.Data.Objects;
using System.Web;
using System.Net.NetworkInformation;
using System.Net;
using AccuIT.BusinessLayer.Services.BO;
using AccuIT.CommonLayer.Log;

namespace AccuIT.PersistenceLayer.Data.Impl
{
    public class WeddingDataImpl : BaseDataImpl, IWeddingRepository
    {
        public List<Wedding> GetUserWeddingDetail(int UserID)
        {
            List<Wedding> wedding = new List<Wedding>();
            wedding = AccuitAdminDbContext.Weddings.Where(x => x.UserID == UserID && x.IsDeleted == false).ToList();
            return wedding;
        }

        public List<DreamWeddsBlog> GetDreamWeddsBlog()
        {

            return AccuitAdminDbContext.DreamWeddsBlogs.Where(x => !x.IsDeleted).ToList();
        }
        public DreamWeddsBlog GetBlogDetails(int blogID)
        {
            DreamWeddsBlog blog = new DreamWeddsBlog();
            return AccuitAdminDbContext.DreamWeddsBlogs.Where(x => x.BlogID == blogID).FirstOrDefault();
        }


        public List<FAQ> GetDreamWeddsFAQ()
        {
            List<FAQ> blogs = new List<FAQ>();
            return AccuitAdminDbContext.FAQs.ToList();
        }

        public Wedding GetWeddingDetailByID(int WeddingId)
        {
            AccuitAdminDbContext.Configuration.LazyLoadingEnabled = false;

            var subscriptions = AccuitAdminDbContext.UserWeddingSubscriptions
                .Include("Wedding")
                .Where(x => x.WeddingID == WeddingId)
                .OrderByDescending(x => x.UserWeddingSubscrptionID).FirstOrDefault();

            if (subscriptions.EndDate < DateTime.Now)
                subscriptions.Wedding.IsActive = false;

            return subscriptions.Wedding;
        }



        public List<BrideAndMaid> GetWeddingBrideMaids(int weddingID)
        {
            return AccuitAdminDbContext.BrideAndMaids.Where(x => x.WeddingID == weddingID && x.IsDeleted == false).ToList();
        }
        public List<GroomAndMan> GetWeddingGroomMen(int weddingID)
        {
            return AccuitAdminDbContext.GroomAndMen.Where(x => x.WeddingID == weddingID && x.IsDeleted == false).ToList();
        }
        public List<WeddingEvent> GetWeddingEvents(int weddingID)
        {
            return AccuitAdminDbContext.WeddingEvents.Where(x => x.WeddingID == weddingID && x.IsDeleted == false).ToList();
        }
        public List<TimeLine> GetWeddingTimeLines(int weddingID)
        {
            return AccuitAdminDbContext.TimeLines.Where(x => x.WeddingID == weddingID && x.IsDeleted == false).ToList();
        }
        public List<WeddingGallery> GetWeddingGallery(int weddingID)
        {
            return AccuitAdminDbContext.WeddingGalleries.Where(x => x.WeddingID == weddingID && x.IsDeleted == false).ToList();
        }



        public BrideAndMaid GetBrideDetailsByID(int brideMaidID)
        {
            return AccuitAdminDbContext.BrideAndMaids.Where(x => x.BrideAndMaidID == brideMaidID && x.IsDeleted == false).FirstOrDefault();
        }
        public GroomAndMan GetGroomDetailsByID(int groomManID)
        {
            return AccuitAdminDbContext.GroomAndMen.Where(x => x.GroomAndMenID == groomManID && x.IsDeleted == false).FirstOrDefault();
        }
        public WeddingEvent GetEventDetailsByID(int eventID)
        {
            var eventbyid = AccuitAdminDbContext.WeddingEvents.Where(x => x.WeddingEventID == eventID && x.IsDeleted == false).FirstOrDefault();
            return eventbyid;
        }
        public TimeLine GetTimeLineDetailsByID(int timeLineID)
        {
            return AccuitAdminDbContext.TimeLines.Where(x => x.TimeLineID == timeLineID && x.IsDeleted == false).FirstOrDefault();
        }
        public AddressMaster GetVenueAddress(int typeID, int PKID)
        {
            return AccuitAdminDbContext.AddressMasters.Where(x => x.AddressOwnerType == typeID && x.AddressOwnerTypePKID == PKID).FirstOrDefault();
        }

        public bool DeleteEventDetailsByID(int eventID, int UserID)
        {
            var venues = AccuitAdminDbContext.Venues.Where(x => x.WeddingEventID == eventID).ToList();
            foreach (var venue in venues)
            {
                venue.IsDeleted = true;
                venue.ModifiedBy = UserID;
                venue.IsActive = false;
                venue.ModifiedDate = DateTime.Now;
                AccuitAdminDbContext.Entry<Venue>(venue).State = System.Data.EntityState.Modified;
            }
            var weddingEvent = AccuitAdminDbContext.WeddingEvents.Where(x => x.WeddingEventID == eventID).FirstOrDefault();
            weddingEvent.IsDeleted = true;
            weddingEvent.IsActive = false;
            weddingEvent.ModifiedBy = UserID;
            weddingEvent.ModifiedDate = DateTime.Now;
            AccuitAdminDbContext.Entry<WeddingEvent>(weddingEvent).State = System.Data.EntityState.Modified;
            return AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
        }

        public List<UserWeddingSubscription> GetUserWeddingSubscriptions(int userID)
        {
            ActivityLog.SetLog("WeddingDataImpl > GetUserWeddingSubscriptions initiated for userID: ." + userID, LogLoc.INFO);
            List<UserWeddingSubscription> result = new List<UserWeddingSubscription>();
            AccuitAdminDbContext.Configuration.LazyLoadingEnabled = false;
            result = AccuitAdminDbContext.UserWeddingSubscriptions
                .Include("OrderMaster")
                .Include("Wedding")
                .Where(x => !x.IsDeleted && x.UserId == userID)
                .ToList();

            return result;
        }

      
        public List<OrderMaster> GetUserOrders(int UserID)
        {
            ActivityLog.SetLog("WeddingDataImpl > GetUserOrders initiated for userID: ." + UserID, LogLoc.INFO);
            return AccuitAdminDbContext.OrderMasters.Where(x => x.UserID == UserID && !x.IsDeleted).ToList();
        }

        public OrderMaster GetOrderByID(int OrderID)
        {
            ActivityLog.SetLog("WeddingDataImpl > GetOrderByID initiated for orderID: ." + OrderID, LogLoc.INFO);
            OrderMaster OM = new OrderMaster();
            try
            {

                OM = AccuitAdminDbContext.OrderMasters.Where(x => x.OrderID == OrderID && !x.IsDeleted).FirstOrDefault();
            }
            catch (Exception e)
            {
                ActivityLog.SetLog("WeddingDataImpl > Exception: " + e, LogLoc.ERROR);
            }
            return OM;
        }

        public int SubmitUserOrder(OrderMaster order)
        {
            ActivityLog.SetLog("WeddingDataImpl > SubmitUserOrder initiated for User: " + order.UserID, LogLoc.INFO);
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
            }))
            {

                if (order.OrderID == 0)
                {
                    order.UserWeddingSubscriptions = new List<UserWeddingSubscription>();

                    order.CreatedDate = DateTime.Now;
                    order.CreatedBy = order.UserID;
                    AccuitAdminDbContext.OrderMasters.Add(order);
                    AccuitAdminDbContext.SaveChanges();
                    ActivityLog.SetLog("WeddingDataImpl > Main Order is Saved. ", LogLoc.INFO);
                    foreach (var item in order.OrderDetails)
                    {
                        for (int i = 1; i <= item.Quantity; i++)
                        {
                            AddUserWeddingSubscription(order.UserID, item);
                        }
                    }
                    scope.Complete();
                }
                else
                {
                    OrderMaster myOrder = AccuitAdminDbContext.OrderMasters.Where(x => x.OrderID == order.OrderID).FirstOrDefault();
                    AccuitAdminDbContext.Entry<OrderMaster>(myOrder).State = System.Data.EntityState.Modified;
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
            }

            return order.OrderID;
        }
        private void AddUserWeddingSubscription(int UserID, OrderDetail order)
        {
            ActivityLog.SetLog("WeddingDataImpl > Going to add user subscription for this order. " + order.OrderID, LogLoc.INFO);
            UserWeddingSubscription uws = new UserWeddingSubscription();
            uws.InvoiceNo = order.OrderID;
            uws.TemplateID = order.TemplateID;
            uws.UserId = UserID;
            uws.WeddingID = null;
            SubscriptionMaster subs = AccuitAdminDbContext.SubscriptionMasters.Where(x => x.SubscriptionID == order.SubscrptionID).FirstOrDefault();
            uws.StartDate = DateTime.Now;
            uws.EndDate = DateTime.Now.AddDays(subs.Days);
            uws.IsDeleted = false;
            uws.SubscriptionType = subs.SubscriptionID;
            uws.SubscriptionStatus = (int)AspectEnums.SubscriptionStatus.Active;
            AccuitAdminDbContext.UserWeddingSubscriptions.Add(uws);
            AccuitAdminDbContext.SaveChanges();
        }


        public int SubmitUserWeddingDetail(int UserID, Wedding wedding)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
             new TransactionOptions
             {
                 IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
             }))
            {
                if (wedding.WeddingID == 0)
                {
                    wedding.CreatedDate = DateTime.Now;
                    wedding.UserID = UserID;
                    wedding.CreatedBy = UserID;
                    wedding.IsActive = true;
                    wedding.IsDeleted = false;
                    AccuitAdminDbContext.Weddings.Add(wedding);
                    AccuitAdminDbContext.SaveChanges();

                    // Add default wedding event`
                    WeddingEvent newevent = new WeddingEvent();
                    newevent.CreatedBy = UserID;
                    newevent.CreatedDate = DateTime.Now;
                    newevent.StartTime = wedding.WeddingDate;
                    newevent.EndTime = wedding.WeddingDate.AddHours(10); // TBD Implementation
                    newevent.EventDate = wedding.WeddingDate;
                    newevent.IsActive = true;
                    newevent.IsDeleted = false;
                    newevent.Title = wedding.Title;
                    newevent.BackGroundImage = wedding.BackgroundImage;
                    newevent.WeddingID = wedding.WeddingID;
                    AccuitAdminDbContext.WeddingEvents.Add(newevent);
                    AccuitAdminDbContext.SaveChanges();
                    //newevent.WeddingEventID = SubmitWeddingEvent(UserID, newevent);

                    scope.Complete();
                }
                else
                {
                    Wedding mywedding = AccuitAdminDbContext.Weddings.Where(x => x.WeddingID == wedding.WeddingID).FirstOrDefault();
                    mywedding.ModifiedDate = DateTime.Now;
                    mywedding.ModifiedBy = UserID;
                    mywedding.WeddingDate = wedding.WeddingDate;
                    mywedding.IsLoveMarriage = wedding.IsLoveMarriage;
                    mywedding.IconUrl = wedding.IconUrl;
                    mywedding.BackgroundImage = wedding.BackgroundImage;
                    mywedding.Quote = wedding.Quote;
                    mywedding.IsDeleted = wedding.IsDeleted;
                    wedding.IsActive = wedding.IsActive;
                    mywedding.videoUrl = wedding.videoUrl;
                    mywedding.Title = wedding.Title;
                    mywedding.fbPageUrl = wedding.fbPageUrl;
                    mywedding.WeddingStyle = wedding.WeddingStyle;
                    AccuitAdminDbContext.Entry<Wedding>(mywedding).State = System.Data.EntityState.Modified;
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
            }

            return wedding.WeddingID;
        }

        public bool UpdateWeddingSubscription(UserWeddingSubscription subscription)
        {
            bool success = false;

            UserWeddingSubscription uws = new UserWeddingSubscription();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
          new TransactionOptions
          {
              IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
          }))
            {
                uws = AccuitAdminDbContext.UserWeddingSubscriptions.Where(x => x.UserWeddingSubscrptionID == subscription.UserWeddingSubscrptionID).First();
                uws.WeddingID = subscription.WeddingID;
                AccuitAdminDbContext.Entry<UserWeddingSubscription>(uws).State = System.Data.EntityState.Modified;
                success = AccuitAdminDbContext.SaveChanges() > 0 ? true : false;
                scope.Complete();
            }
            return success;
        }

        public int SubmitWeddingEvent(int UserID, WeddingEvent weddevent)
        {
            WeddingEvent weddingevent = new WeddingEvent();
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
             new TransactionOptions
             {
                 IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
             }))
            {
                if (weddevent.WeddingEventID == 0)
                {
                    weddevent.CreatedDate = DateTime.Now;
                    weddevent.CreatedBy = UserID;
                    weddevent.IsActive = true;
                    weddevent.IsDeleted = false;
                    AccuitAdminDbContext.WeddingEvents.Add(weddevent);
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
                else
                {
                    WeddingEvent myEvent = AccuitAdminDbContext.WeddingEvents.Where(x => x.WeddingEventID == weddevent.WeddingEventID).FirstOrDefault();
                    myEvent.ModifiedDate = DateTime.Now;
                    myEvent.EventDate = weddevent.EventDate;
                    myEvent.StartTime = weddevent.StartTime;
                    myEvent.EndTime = weddevent.EndTime;
                    myEvent.ImageUrl = weddevent.ImageUrl;
                    myEvent.Title = weddevent.Title;
                    myEvent.Aboutevent = weddevent.Aboutevent;
                    myEvent.BackGroundImage = weddevent.BackGroundImage;
                    myEvent.IsActive = weddevent.IsActive;
                    myEvent.IsDeleted = weddevent.IsDeleted;
                    //myevent.About = weddevent.About;
                    AccuitAdminDbContext.Entry<WeddingEvent>(myEvent).State = System.Data.EntityState.Modified;
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
            }

            return weddevent.WeddingEventID;
        }
        public int SubmitVenue(int userID, Venue venue)
        {

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
             new TransactionOptions
             {
                 IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
             }))
            {
                if (venue.VenueID == 0)
                {
                    venue.CreatedDate = DateTime.Now;
                    venue.CreatedBy = userID;
                    venue.IsActive = true;
                    AccuitAdminDbContext.Venues.Add(venue);
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
                else
                {
                    Venue myVenue = AccuitAdminDbContext.Venues.Where(x => x.VenueID == venue.VenueID).FirstOrDefault();
                    myVenue.ModifiedDate = DateTime.Now;
                    myVenue.VenueImageUrl = venue.VenueImageUrl;
                    myVenue.VenueMobile = venue.VenueMobile;
                    myVenue.VenuePhone = venue.VenuePhone;
                    myVenue.VenueWebsite = venue.VenueWebsite;
                    myVenue.VenueBannerImageUrl = venue.VenueBannerImageUrl;
                    myVenue.VenuePhone = venue.VenuePhone;
                    myVenue.OwnerName = venue.OwnerName;
                    myVenue.Name = venue.Name;
                    myVenue.googleMapUrl = venue.googleMapUrl;
                    myVenue.IsActive = venue.IsActive;
                    myVenue.IsDeleted = venue.IsDeleted;
                    AccuitAdminDbContext.Entry<Venue>(myVenue).State = System.Data.EntityState.Modified;
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
            }

            return venue.VenueID;

        }
        public int SubmitAddress(int userID, AddressMaster address)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
            }))
            {
                if (address.AddressID == 0)
                {
                    address.CreatedDate = DateTime.Now;
                    address.CreatedBy = userID;
                    address.IsDeleted = false;
                    AccuitAdminDbContext.AddressMasters.Add(address);
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
                else
                {
                    AddressMaster myaddress = AccuitAdminDbContext.AddressMasters.Where(x => x.AddressID == address.AddressID).FirstOrDefault();// GetWeddingDetailByID(weddingId).WeddingEvents.FirstOrDefault().Venues.Where(x => x.VenueID == venue.VenueID).FirstOrDefault();
                    myaddress.ModifiedDate = DateTime.Now;
                    myaddress.Address1 = address.Address1;
                    myaddress.Address2 = address.Address2;
                    myaddress.City = address.City;
                    myaddress.State = address.State;
                    myaddress.PinCode = address.PinCode;
                    myaddress.AddressOwnerType = address.AddressOwnerType;
                    myaddress.AddressOwnerTypePKID = address.AddressOwnerTypePKID;
                    myaddress.VenueID = address.VenueID;
                    myaddress.AddressType = address.AddressType;
                    myaddress.IsDeleted = address.IsDeleted;
                    AccuitAdminDbContext.Entry<AddressMaster>(myaddress).State = System.Data.EntityState.Modified;
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
            }
            return address.AddressID;
        }
        public int SubmitBrideMaids(int UserID, BrideAndMaid bridemaid)
        {
            int WeddingId = bridemaid.WeddingID;
            if (bridemaid.DateofBirth.Value.Year == 1)
                bridemaid.DateofBirth = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
            new TransactionOptions
            {
                IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
            }))
            {
                if (bridemaid.BrideAndMaidID == 0)
                {
                    bridemaid.CreatedDate = DateTime.Now;
                    bridemaid.IsDeleted = false;
                    bridemaid.CreatedBy = UserID;

                    AccuitAdminDbContext.BrideAndMaids.Add(bridemaid);
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
                else
                {
                    BrideAndMaid bridemaids = AccuitAdminDbContext.BrideAndMaids.Where(x => x.BrideAndMaidID == bridemaid.BrideAndMaidID).FirstOrDefault();
                    bridemaids.ModifiedDate = DateTime.Now;
                    bridemaids.AboutBrideMaid = bridemaid.AboutBrideMaid;
                    bridemaids.DateofBirth = bridemaid.DateofBirth;
                    bridemaids.FirstName = bridemaid.FirstName;
                    bridemaids.Imageurl = bridemaid.Imageurl;
                    bridemaids.IsBride = bridemaid.IsBride;
                    bridemaids.LastName = bridemaid.LastName;
                    bridemaids.RelationWithBride = bridemaid.RelationWithBride;
                    bridemaids.fbUrl = bridemaid.fbUrl;
                    bridemaids.googleUrl = bridemaid.googleUrl;
                    bridemaids.instagramUrl = bridemaid.instagramUrl;
                    bridemaids.ModifiedBy = bridemaid.ModifiedBy;
                    bridemaids.IsDeleted = bridemaid.IsDeleted;
                    AccuitAdminDbContext.Entry<BrideAndMaid>(bridemaids).State = System.Data.EntityState.Modified;
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
            }

            return bridemaid.BrideAndMaidID;
        }
        public int SubmitGroomMen(int UserID, GroomAndMan groom)
        {
            if (groom.DateofBirth.Value.Year == 1)
                groom.DateofBirth = null;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
             new TransactionOptions
             {
                 IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
             }))
            {
                if (groom.GroomAndMenID == 0)
                {
                    groom.CreatedDate = DateTime.Now;
                    AccuitAdminDbContext.GroomAndMen.Add(groom);
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
                else
                {
                    GroomAndMan groomMen = AccuitAdminDbContext.GroomAndMen.Where(x => x.GroomAndMenID == groom.GroomAndMenID).First();
                    groomMen.ModifiedDate = DateTime.Now;
                    groomMen.AboutMen = groom.AboutMen;
                    groomMen.DateofBirth = groom.DateofBirth;
                    groomMen.FirstName = groom.FirstName;
                    groomMen.Imageurl = groom.Imageurl;
                    groomMen.IsGroom = groom.IsGroom;
                    groomMen.LastName = groom.LastName;
                    groomMen.RelationWithGroom = groom.RelationWithGroom;
                    groomMen.fbUrl = groom.fbUrl;
                    groomMen.googleUrl = groom.googleUrl;
                    groomMen.instagramUrl = groom.instagramUrl;
                    groomMen.IsDeleted = groom.IsDeleted;
                    AccuitAdminDbContext.Entry<GroomAndMan>(groomMen).State = System.Data.EntityState.Modified;
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
            }

            return groom.GroomAndMenID;
        }
        public int SubmitTimeLine(int UserID, TimeLine timeline)
        {
            int weddingID = timeline.WeddingID;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
             new TransactionOptions
             {
                 IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
             }))
            {
                if (timeline.TimeLineID == 0)
                {
                    timeline.CreatedDate = DateTime.Now;
                    timeline.CreatedBy = UserID;
                    AccuitAdminDbContext.TimeLines.Add(timeline);
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
                else
                {
                    TimeLine tLine = AccuitAdminDbContext.TimeLines.Where(x => x.TimeLineID == timeline.TimeLineID).First();
                    tLine.ModifiedDate = DateTime.Now;
                    tLine.StoryDate = timeline.StoryDate;
                    tLine.Title = timeline.Title;
                    tLine.ImageUrl = timeline.ImageUrl;
                    tLine.Story = timeline.Story;
                    tLine.IsDeleted = timeline.IsDeleted;
                    tLine.ModifiedBy = UserID;
                    tLine.Location = timeline.Location;
                    AccuitAdminDbContext.Entry<TimeLine>(tLine).State = System.Data.EntityState.Modified;
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
            }

            return timeline.TimeLineID;
        }
        public int SubmitGallery(int UserID, WeddingGallery gallery)
        {

            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required,
             new TransactionOptions
             {
                 IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted
             }))
            {
                if (gallery.WeddingGalleryID == 0)
                {
                    gallery.CreatedDate = DateTime.Now;
                    gallery.CreatedBy = UserID;
                    AccuitAdminDbContext.WeddingGalleries.Add(gallery);
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
                else
                {
                    WeddingGallery gallry = AccuitAdminDbContext.WeddingGalleries.Where(x => x.WeddingGalleryID == gallery.WeddingGalleryID).First();
                    gallry.ImageName = gallery.ImageName;
                    gallry.ImageTitle = gallery.ImageTitle;
                    gallry.ImageUrl = gallery.ImageUrl;
                    gallry.IsDeleted = gallery.IsDeleted;
                    gallry.Place = gallery.Place;
                    AccuitAdminDbContext.Entry<WeddingGallery>(gallry).State = System.Data.EntityState.Modified;
                    AccuitAdminDbContext.SaveChanges();
                    scope.Complete();
                }
            }
            return gallery.WeddingGalleryID;
        }
    }
}
