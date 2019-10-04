using System.Reflection;
using System.Transactions;
using Microsoft.Practices.Unity.InterceptionExtension;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.CommonLayer.Aspects.Utilities;


namespace AccuIT.CommonLayer.Aspects.Annotations
{
    /// <summary>
    /// This class is used to define the definitions of the Transaction attributes
    /// </summary>
    public class RunTransaction : System.Attribute
    {
    }

    /// <summary>
    /// This class's purpose is to define the rule for the transaction handler identification
    /// </summary>
    public class TransactionRule : IMatchingRule
    {
        /// <summary>
        /// This interface implementation method is used to match the attribute internal logic to handle the transaction method
        /// </summary>
        /// <param name="member">method base input member details</param>
        /// <returns>returns Boolean status</returns>
        public bool Matches(MethodBase member)
        {
            bool isMatched = false;

            //Iterate through attributes of the method invocation
            foreach (var item in member.GetCustomAttributes(true))
            {
                if (item.GetType().Name == AppConstants.RUN_TRANSACTION)
                {
                    isMatched = true;
                    break;
                }
            }
            return isMatched;
        }
    }
    /// <summary>
    /// This class is used to handle the transaction scope of method implementation as per unity interception logic
    /// </summary>
    public class TransactionManager : ICallHandler
    {
        /// <summary>
        /// This interface implementation will be called as part of the transaction handling attribute invocation
        /// </summary>
        /// <param name="input">method execution as action input</param>
        /// <param name="getNext">next action to be executed</param>
        /// <returns></returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn result = null;
            using (TransactionScope transaction = new TransactionScope())
            {
                result = getNext()(input, getNext);
                if (result.Exception != null)
                {
                    throw result.Exception;
                }
                else
                {
                    transaction.Complete();
                }
            }
            return result;
        }
        /// <summary>
        /// This property is implemented to get the order sequence of method execution
        /// </summary>
        public int Order
        {
            get;
            set;
        }
    }
}
