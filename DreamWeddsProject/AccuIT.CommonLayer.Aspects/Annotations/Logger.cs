using System;
using System.Reflection;
using AccuIT.CommonLayer.Aspects.Logging;
using AccuIT.CommonLayer.Aspects.Utilities;
using Microsoft.Practices.Unity.InterceptionExtension;
using AccuIT.CommonLayer.Aspects.Utilities;

namespace AccuIT.CommonLayer.Aspects.Annotations
{
    public class Logger : System.Attribute
    {

    }

    public class LoggerRule : IMatchingRule
    {
        /// <summary>
        /// This interface implementation method is used to match the attribute internal logic to handle the transaction method
        /// </summary>
        /// <param name="member">method base input member details</param>
        /// <returns>returns boolean status</returns>
        public bool Matches(MethodBase member)
        {
            bool isMatched = true;

            //Iterate through attributes of the method invocation
            //foreach (var item in member.GetCustomAttributes(true))
            //{
            //    if (item.GetType().Name == "Logger")
            //    {
            //        isMatched = true;
            //        break;
            //    }
            //}
            return isMatched;
        }
    }

    /// <summary>
    /// This class is used to handle the transaction scope of method implementation as per unity interception logic
    /// </summary>
    public class LogWriteManager : ICallHandler
    {
        /// <summary>
        /// This interface implementation will be called as part of the transaction handling attribute invocation
        /// </summary>
        /// <param name="input">method execution as action input</param>
        /// <param name="getNext">next action to be executed</param>
        /// <returns></returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            bool isLogEnabled = AppUtil.GetAppSettings(AspectEnums.ConfigKeys.IsGlobalMethodLogging) == "1" ? true : false;
            if (isLogEnabled)
                LogTraceEngine.WriteLog(String.Format("Method {0} Execution started at {1}", input.MethodBase.Name, System.DateTime.Now));
            IMethodReturn result = null;
            result = getNext()(input, getNext);
            if (isLogEnabled)
                LogTraceEngine.WriteLog(String.Format("Method {0} Execution ended at {1}", input.MethodBase.Name, System.DateTime.Now));
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
