using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.CommonLayer.Resources;
using System;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace AccuIT.PresentationLayer.ServiceImpl.Security
{
    /// <summary>
    /// Class to invoke service methods if provided token values matches
    /// </summary>
    public class SecurityOperationInvoker : IOperationInvoker
    {
        private IOperationInvoker InnerOperationInvoker { get; set; }

        #region Constructor

        /// <summary>
        /// Constructor to initialize class variables
        /// </summary>
        /// <param name="operationInvoker"></param>
        public SecurityOperationInvoker(IOperationInvoker operationInvoker)
        {
            this.InnerOperationInvoker = operationInvoker;
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
            //+ authorization
            outputs = null;
            var request = OperationContext.Current.IncomingMessageProperties[HttpRequestMessageProperty.Name] as HttpRequestMessageProperty;
            string apiKeyHeader = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.APIKeyHeader);
            string apiSecretHeader = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.APITokenHeader);
            string headerValue = request.Headers[apiKeyHeader];
            string secretTokenKey = request.Headers[apiSecretHeader];

            if (!String.IsNullOrEmpty(headerValue) && !String.IsNullOrEmpty(secretTokenKey))
            {
                //call method to get the api key authorization from database
                bool isValid = true;// SystemBusinessInstance.IsValidServiceUser(RemoveUnwantedCharacters(headerValue), RemoveUnwantedCharacters(secretTokenKey), "");
                if (isValid)
                {
                    return InnerOperationInvoker.Invoke(instance, inputs, out outputs);
                }
                else
                {
                    //returns exception for missing API Credentials
                    throw new System.Security.SecurityException(Messages.ApiAccessDenied);
                }
            }
            //returns exception for missing API Credentials
            throw new System.Security.SecurityException(Messages.CredentialsNotFound);
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
                return AopEngine.Resolve<ISystemService>(AspectEnums.AspectInstanceNames.ServiceManager,AspectEnums.ApplicationName.AccuIT);
            }
        }
    }
}
