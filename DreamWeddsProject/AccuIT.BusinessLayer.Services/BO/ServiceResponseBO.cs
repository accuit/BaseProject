using System.Runtime.Serialization;

namespace AccuIT.BusinessLayer.Services.BO
{
    /// <summary>
    /// This class is used to define the response data for service/business methods
    /// </summary>
    
    public class ServiceResponseBO
    {
        public string StatusCode
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }

        public string UserID
        {
            get;
            set;
        }

     
        public string UserCode
        {
            get;
            set;
        }

       
        public string CompanyID
        {
            get;
            set;
        }


        public string APIKey
        {
            get;
            set;
        }

        public string APIToken
        {
            get;
            set;
        }

        public int UserDeviceID
        {
            get;
            set;
        }

        public int RoleID
        {
            get;
            set;
        }
    }
}
