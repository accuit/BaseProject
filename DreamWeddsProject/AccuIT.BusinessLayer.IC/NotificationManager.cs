using AccuIT.CommonLayer.Aspects.Logging;
using AccuIT.CommonLayer.Aspects.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using AccuIT.BusinessLayer.IC.Contracts;

namespace AccuIT.BusinessLayer.IC
{
    /// <summary>
    /// Notification class to send message to android mobile clients
    /// </summary>
    public class AndroiddNotificationManager : INotification
    {
        /// <summary>
        /// Method to authenticate message sender using url passed in input parameter
        /// </summary>
        /// <param name="senderID">google sender ID</param>
        /// <param name="password">password</param>
        /// <param name="loginURL">login URL</param>
        /// <returns>returns authentication status</returns>
        public string AuthenticateSender(string senderID, string password, string loginURL)
        {
            string webResponse = string.Empty;
            string urlWithParameters = String.Format("{0}Email={1}&Passwd={2}&accountType=GOOGLE&source=Company-App-Version&service=ac2dm", loginURL, senderID, password);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(urlWithParameters);
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                StreamReader reader;
                int index = 0;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream stream = response.GetResponseStream();
                    reader = new StreamReader(stream);
                    string output = reader.ReadToEnd();
                    reader.Close();
                    //stream.Close();
                    index = output.IndexOf("Auth=") + 5;
                    int length = output.Length - index;
                    webResponse = output.Substring(index, length);
                }
            }
            catch (Exception ex)
            {
                webResponse = ex.Message;
            }
            return webResponse;
        }

        /// <summary>
        /// Method to push message to device
        /// </summary>
        /// <param name="registrationID">registration ID</param>
        /// <param name="message">message to deliver</param>
        /// <param name="authToken">authorization token</param>
        /// <param name="messageURL">messaging service URL</param>
        /// <returns></returns>
        public string SendMessage(string registrationID, string message, string authToken, string messageURL)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://android.clients.google.com/c2dm/send");
            request.Method = "POST";
            request.KeepAlive = false;
            NameValueCollection postFileValues = new NameValueCollection();
            postFileValues.Add("registration_id", registrationID);
            postFileValues.Add("collapse_key", "1");
            postFileValues.Add("delay_while_idle", "0");
            postFileValues.Add("data.payload", message);
            string postData = GetPostStringFrom(postFileValues);
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);


            request.ContentType = "application/x-www-form-urlencoded;charset=UTF-8";
            request.ContentLength = byteArray.Length;

            request.Headers.Add(HttpRequestHeader.Authorization, "GoogleLogin auth=" + authToken);
            //-- Delegate Modeling to Validate Server Certificate --//
            ServicePointManager.ServerCertificateValidationCallback += delegate (
                        object
                        sender,
                        System.Security.Cryptography.X509Certificates.X509Certificate
                        pCertificate,
                        System.Security.Cryptography.X509Certificates.X509Chain pChain,
                        System.Net.Security.SslPolicyErrors pSSLPolicyErrors)
            {
                return true;
            };

            //-- Create Stream to Write Byte Array --// 
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            //-- Post a Message --//
            WebResponse Response = request.GetResponse();
            HttpStatusCode ResponseCode = ((HttpWebResponse)Response).StatusCode;
            if (ResponseCode.Equals(HttpStatusCode.Unauthorized) || ResponseCode.Equals(HttpStatusCode.Forbidden))
            {
                return "Unauthorized - need new token";

            }
            else if (!ResponseCode.Equals(HttpStatusCode.OK))
            {
                return "Response from web service isn't OK";
                //Console.WriteLine("Response from web service not OK :");
                //Console.WriteLine(((HttpWebResponse)Response).StatusDescription);
            }

            StreamReader Reader = new StreamReader(Response.GetResponseStream());
            string responseLine = Reader.ReadLine();
            Reader.Close();

            return responseLine;
        }

        /// <summary>
        /// Create Query String From Name Value Pair
        /// </summary>
        /// <param name="postFieldNameValue"></param>
        /// <returns></returns>
        private string GetPostStringFrom(NameValueCollection postFieldNameValue)
        {
            //throw new NotImplementedException();
            List<string> items = new List<string>();

            foreach (String name in postFieldNameValue)
                items.Add(String.Concat(name, "=", HttpUtility.UrlEncode(postFieldNameValue[name])));

            return String.Join("&", items.ToArray());
        }

        /// <summary>
        /// Method to send notification on the basis of device ID/RegistrationID and message
        /// </summary>
        /// <param name="deviceId">Registration Key</param>
        /// <param name="message">message</param>
        /// <returns>returns server response</returns>
        /// //Updated by VC 20140825
        /// 
        public string SendNotification(string Registration_IDsJSON, string PushNotificationMessage, int type)
        {
            String serverResponse = string.Empty;
            try
            {
                string GoogleAppID = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.GCMAndroidAppID);
                string senderID = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.GCMAndroidSenderID);
                string[] PushNotificationMessageSplited = PushNotificationMessage.Split('~');
                string NotificationTitle = PushNotificationMessageSplited[0];
                string NotificationBody = PushNotificationMessageSplited[1];

                WebRequest tRequest;
                tRequest = WebRequest.Create(AppUtil.GetAppSettings(AspectEnums.ConfigKeys.GoogleAndroidMessageURL));
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));

                tRequest.Headers.Add("Sender", senderID);
                string collapseKey = Guid.NewGuid().ToString();
                NotificationBody = NotificationBody.Replace("\\r\\n", "\r\n");
                string postData = "{ \"collapse_key\": \"" + collapseKey + "\"," +
                       "\"time_to_live\": 108, " +
                       "\"delay_while_idle\": true, " +
                       "\"data\": {\"message\":[{\"NotificationType\":1,\"NotificationData\":{\"Title\":\"" + NotificationTitle + "\", \"Body\":\"" + System.Web.HttpUtility.HtmlEncode(NotificationBody).Replace("\r\n", "<br>").Replace(" ", "&nbsp;") + "\"}}]," +
                       "\"time\":\"" + System.DateTime.Now.ToString() + "\"}," +
                       "\"registration_ids\":[" + Registration_IDsJSON + "]}";

                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                tRequest.ContentLength = byteArray.Length;

                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse tResponse = null;
                try
                {
                    tResponse = tRequest.GetResponse();
                    dataStream = tResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    serverResponse = reader.ReadToEnd();
                    reader.Close();
                    //dataStream.Close();
                    tResponse.Close();
                    ActivityLog.SetLog(serverResponse + " - " + AppVariables.AppLogTraceCategoryName.DiskFiles, LogLoc.DEBUG);
                    return serverResponse;
                }
                catch (WebException webEx)
                {
                    tResponse = webEx.Response;
                }
                if (tResponse == null) return null;
                System.IO.StreamReader sr = new System.IO.StreamReader(tResponse.GetResponseStream());
                serverResponse = sr.ReadToEnd().Trim();
                ActivityLog.SetLog(serverResponse + " - " + AppVariables.AppLogTraceCategoryName.DiskFiles, LogLoc.DEBUG);
            }
            catch (Exception ex)
            {
                ActivityLog.SetLog(ex.Message + " - "+ AppVariables.AppLogTraceCategoryName.DiskFiles, LogLoc.ERROR);
                serverResponse = ex.Message;
            }
            return serverResponse;
        }
        public string SendNotification(string deviceId, string message)
        {
            String serverResponse = string.Empty;
            try
            {
                string GoogleAppID = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.GCMAndroidAppID);
                string senderID = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.GCMAndroidSenderID);
                var value = message;
                WebRequest tRequest;
                tRequest = WebRequest.Create(AppUtil.GetAppSettings(AspectEnums.ConfigKeys.GoogleAndroidMessageURL));
                tRequest.Method = "POST";
                tRequest.ContentType = "application/x-www-form-urlencoded";

                tRequest.Headers.Add(string.Format("Authorization: key={0}", GoogleAppID));
                //tRequest.Headers.Add(string.Format("project_id: direct-shelter-478"));
                //tRequest.Headers.Add(string.Format("Sender:{0}", "716824367280"));
                //tRequest.Headers.Add(string.Format("Sender: id={0}", senderID));
                //tRequest.Headers.Add("Authorization", GoogleAppID);

                tRequest.Headers.Add("Sender", senderID);
                string collapseKey = Guid.NewGuid().ToString();
                string postData = "collapse_key=" + collapseKey + "&time_to_live=108&delay_while_idle=1&data.message=" + value + "&data.time=" + System.DateTime.Now.ToString() + "&registration_id=" + deviceId + "";

                Byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                tRequest.ContentLength = byteArray.Length;

                Stream dataStream = tRequest.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse tResponse = null;
                try
                {
                    tResponse = tRequest.GetResponse();
                    dataStream = tResponse.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    serverResponse = reader.ReadToEnd();
                    reader.Close();
                    //dataStream.Close();
                    tResponse.Close();
                    ActivityLog.SetLog(serverResponse + " - "+ AppVariables.AppLogTraceCategoryName.DiskFiles, LogLoc.DEBUG);
                    //serverResponse = "Success";
                    return serverResponse;
                }
                catch (WebException webEx)
                {
                    tResponse = webEx.Response;
                }
                if (tResponse == null) return null;
                System.IO.StreamReader sr = new System.IO.StreamReader(tResponse.GetResponseStream());
                serverResponse = sr.ReadToEnd().Trim();
                ActivityLog.SetLog(serverResponse +" - "+ AppVariables.AppLogTraceCategoryName.DiskFiles, LogLoc.DEBUG);
            }
            catch (Exception ex)
            {
                ActivityLog.SetLog(ex.Message +" - "+ AppVariables.AppLogTraceCategoryName.DiskFiles, LogLoc.DEBUG);
                serverResponse = ex.Message;
            }
            return serverResponse;
        }

        /// <summary>
        /// Validate Server Certificate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="policyErrors"></param>
        /// <returns></returns>
        private static bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors policyErrors)
        {
            return true;
        }

        public void QueueNotification(long userID, string notificationHeader, string notificationMessage, AspectEnums.NotificationType notificationType)
        {

        }
    }
}
