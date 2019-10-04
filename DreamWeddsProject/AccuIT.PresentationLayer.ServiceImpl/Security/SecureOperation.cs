using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace AccuIT.PresentationLayer.ServiceImpl.Security
{
    /// <summary>
    /// Class to define the security attribute over the WCF methods
    /// </summary>
    internal class SecureOperation : Attribute, IOperationBehavior
    {
        #region IOperationBehavior Members
        /// <summary>
        /// Method which fires at the time of add binding parameters
        /// </summary>
        /// <param name="operationDescription">operation description</param>
        /// <param name="bindingParameters">binding parameters</param>
        public void AddBindingParameters(OperationDescription operationDescription, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {

        }

        /// <summary>
        /// Method which fires at the time of apply client behaviour override action
        /// </summary>
        /// <param name="operationDescription">operation description</param>
        /// <param name="clientOperation">client operation dispatcher</param>
        public void ApplyClientBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.ClientOperation clientOperation)
        {

        }

        /// <summary>
        /// Method to apply dispatcher behaviour for existing operation context
        /// </summary>
        /// <param name="operationDescription">operation description</param>
        /// <param name="dispatchOperation">dispatcher operation</param>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, System.ServiceModel.Dispatcher.DispatchOperation dispatchOperation)
        {
            dispatchOperation.Invoker = new SecurityOperationInvoker(dispatchOperation.Invoker);
        }

        /// <summary>
        /// Method to validate Operation context action
        /// </summary>
        /// <param name="operationDescription">operation description</param>
        public void Validate(OperationDescription operationDescription)
        {

        }

        #endregion
    }
}
