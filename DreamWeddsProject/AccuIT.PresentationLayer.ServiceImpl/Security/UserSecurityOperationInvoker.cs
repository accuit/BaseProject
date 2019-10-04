using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.CommonLayer.Aspects.Exceptions;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.CommonLayer.Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.PresentationLayer.ServiceImpl.Security
{
    public class UserSecurityOperationInvoker : IOperationInvoker
    {
        private IOperationInvoker InnerOperationInvoker { get; set; }
        private string operationName = string.Empty;

        #region Constructor

        /// <summary>
        /// Constructor to initialize class variables
        /// </summary>
        /// <param name="operationInvoker"></param>
        public UserSecurityOperationInvoker(IOperationInvoker operationInvoker, string operationName)
        {
            this.InnerOperationInvoker = operationInvoker;
            this.operationName = operationName;
        }

        #endregion


        /// <summary>
        /// Method to allocate input for the Invokable operation context requests
        /// </summary>
        /// <returns></returns>
        public Object[] AllocateInputs()
        {
            return InnerOperationInvoker.AllocateInputs();
        }

        /// <summary>
        /// Method to invoke Service Request
        /// </summary>
        /// <param name="instance">instance of the operation context request</param>
        /// <param name="inputs">input patameters array</param>
        /// <param name="outputs">output parameter arrays</param>
        /// <returns>returns execution context</returns>
        public Object Invoke(Object instance, Object[] inputs, out Object[] outputs)
        {

           
            outputs = null;
            var request = OperationContext.Current.IncomingMessageProperties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
            string apiKeyHeader = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.APIKeyHeader);
            string apiSecretHeader = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.APITokenHeader);
            string apiHeaderUserID = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.HeaderUserID);
            string headerValue = request.Headers[apiKeyHeader];
            string secretTokenKey = request.Headers[apiSecretHeader];
            string headerUserID = request.Headers[apiHeaderUserID];

            if (!String.IsNullOrEmpty(headerValue) && !String.IsNullOrEmpty(secretTokenKey) && !String.IsNullOrEmpty(headerUserID))
            {
                //call method to get the api key authorization from database
                bool isValid =  SystemBusinessInstance.IsValidServiceUser(RemoveUnwantedCharacters(headerValue), RemoveUnwantedCharacters(secretTokenKey), RemoveUnwantedCharacters(headerUserID));
                if (isValid)
                {
                    if (IsValidTokenUser(headerValue, secretTokenKey, operationName, Convert.ToInt32(headerUserID), inputs))
                    {   
                        return InnerOperationInvoker.Invoke(instance, inputs, out outputs);
                    }
                    else
                    {
                        //returns exception for missing API Credentials
                        throw new System.Security.VerificationException(Messages.ApiAccessDenied + operationName + " Unauthorized" + " UserID=" + headerUserID);
                    }
                }
                else
                {
                    //returns exception for missing API Credentials
                    throw new System.Security.SecurityException(Messages.ApiAccessDenied + operationName + " UserID=" + headerUserID + ";ApkiKey=" + headerValue + ";ApiToken=" + secretTokenKey);
                }
            }
            //returns exception for missing API Credentials
            throw new System.Security.SecurityException(Messages.CredentialsNotFound + operationName);
        }

     
        /// <summary>
        /// Sanitize HTML Code (Request Parameter)
        /// </summary>
        /// <param name="inputs"></param>
        private static void SanitizeChars(Object[] inputs)
        {
            foreach (var item in inputs)
            {
                if (item != null)
                {
                    if (item.GetType().GetInterface("System.Collections.IEnumerable") != null)
                    {
                        var collection = (IEnumerable)item;

                        foreach (var subitem in collection)
                        {
                            var typeMain = subitem.GetType();
                            foreach (var prop in typeMain.GetProperties().Where(x => x.PropertyType == typeof(String)))
                            {
                                if (prop.CanRead && prop.CanWrite)
                                {
                                    var ObjectPropertyValue = prop.GetValue(subitem);
                                    var SanitizedObject = System.Web.HttpUtility.HtmlEncode(ObjectPropertyValue);
                                    prop.SetValue(subitem, SanitizedObject);
                                }
                            }
                        }
                    }
                    else
                    {
                        var typeMain = item.GetType();
                        foreach (var prop in typeMain.GetProperties().Where(x => x.PropertyType == typeof(String)))
                        {
                            if (prop.CanRead && prop.CanWrite)
                            {
                                var ObjectPropertyValue = prop.GetValue(item);
                                var SanitizedObject = System.Web.HttpUtility.HtmlEncode(ObjectPropertyValue);
                                prop.SetValue(item, SanitizedObject);
                            }
                        }
                    }
                }
                else
                {

                }
            }
           

        }

        

        /// <summary>
        /// Method which fires at the time of Request invoke action begins
        /// </summary>
        /// <param name="instance">operation context instance</param>
        /// <param name="inputs">input parameter array</param>
        /// <param name="callback">call back method</param>
        /// <param name="state">request state</param>
        /// <returns>returns result status</returns>
        public IAsyncResult InvokeBegin(Object instance, Object[] inputs, AsyncCallback callback, Object state)
        {
            return InnerOperationInvoker.InvokeBegin(instance, inputs, callback, state);
        }

        /// <summary>
        /// Method which fires at the time of Request invoke action ends
        /// </summary>
        /// <param name="instance">operation context instance</param>
        /// <param name="outputs">output parameter array</param>
        /// <param name="result">result obtained</param>
        /// <returns>returns object</returns>
        public Object InvokeEnd(Object instance, out Object[] outputs, IAsyncResult result)
        {
            return InnerOperationInvoker.InvokeEnd(instance, out outputs, result);
        }

        /// <summary>
        /// Property to get Synchronous state boolean response
        /// </summary>
        public Boolean IsSynchronous
        {
            get { return InnerOperationInvoker.IsSynchronous; }
        }

        /// <summary>
        /// Method to remove unwanted slashes from string input
        /// </summary>
        /// <param name="inputText">input text</param>
        /// <returns>returns formatted string</returns>
        private string RemoveUnwantedCharacters(string inputText)
        {
            return inputText.Replace("\"", "");
        }

        ///// <summary>
        ///// Property to get the Api Manager business class instance
        ///// </summary>
        public ISystemService SystemBusinessInstance
        {
            get
            {
                //call library method to get instance of Api Manager business class
                return AopEngine.Resolve<ISystemService>(AspectEnums.AspectInstanceNames.ServiceManager, AspectEnums.ApplicationName.AccuIT);
            }
        }

        /// <summary>
        /// Method to validate token user credentials
        /// </summary>
        /// <param name="headerValue">header value</param>
        /// <param name="tokenValue">token value</param>
        /// <param name="operationName">operation name</param>
        /// <param name="inputs">input parameters</param>
        /// <returns></returns>
        private bool IsValidTokenUser(string headerValue, string tokenValue, string operationName, int userID, Object[] inputs)
        {

            bool isValidUser = false;
            try
            {
                ExceptionEngine.AppExceptionManager.Process(() =>
                {
                    /*By Passed Security<Begin>*/
                    isValidUser = false;

                    //return;
                    /*By Passed Security<End>*/
                   // int userID = SystemBusinessInstance.GetServiceTokenUserID(headerValue, tokenValue);
                    if (userID > 0)
                    {
                        switch (operationName)
                        {                           
                            case "GetMDMDistrict":
                                isValidUser = userID == Convert.ToInt64(inputs[0]);
                                break;
                        
                            default:
                                isValidUser = true;
                                break;
                        }
                    }
                }, AspectEnums.ExceptionPolicyName.ServiceExceptionPolicy.ToString());
            }
            catch (Exception)
            { }
            if (isValidUser == false)
            {

            }

            return isValidUser;
        }

    }
}
