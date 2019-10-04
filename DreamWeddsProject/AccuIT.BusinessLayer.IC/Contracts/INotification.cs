
namespace AccuIT.BusinessLayer.IC.Contracts
{
    /// <summary>
    /// Interface to define the methods for android push notification
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// Method to authenticate message sender using url passed in input parameter
        /// </summary>
        /// <param name="senderID">google sender ID</param>
        /// <param name="password">password</param>
        /// <param name="loginURL">login URL</param>
        /// <returns>returns authentication status</returns>
        string AuthenticateSender(string senderID, string password, string loginURL);

        /// <summary>
        /// Method to push message to device
        /// </summary>
        /// <param name="registrationID">registration ID</param>
        /// <param name="message">message to deliver</param>
        /// <param name="authToken">authorization token</param>
        /// <param name="messageURL">messaging service URL</param>
        /// <returns></returns>
        string SendMessage(string registrationID, string message, string authToken, string messageURL);

        /// <summary>
        /// Method to send notification on the basis of device ID/RegistrationID and message
        /// </summary>
        /// <param name="deviceId">Registration Key</param>
        /// <param name="message">message</param>
        /// <returns>returns server response</returns>
        string SendNotification(string deviceId, string message);

        /// <summary>
        /// Overload SendNotification to send notification with JSON string
        /// </summary>
        /// <param name="Registration_IDsJSON">Android Ids seperated with comma</param>
        /// <param name="PushNotificationMessage">Notification message</param>
        /// /// <param name="type">Notification Type</param>
        /// <returns>returns server response</returns>
        string SendNotification(string Registration_IDsJSON, string PushNotificationMessage, int type);
    }
}
