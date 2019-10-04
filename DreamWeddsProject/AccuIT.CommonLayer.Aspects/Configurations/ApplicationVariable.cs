using System.Globalization;
using System.Threading;

namespace AccuIT.CommonLayer.Aspects.Configurations
{
    /// <summary>
    /// Class to hold the global/application variables such as localization attributes
    /// </summary>
    public class ApplicationVariable
    {
        private static string _regionCode;
        
        /// <summary>
        /// Property to get application region code
        /// </summary>
        public static string RegionCode
        {
            get { return ApplicationVariable._regionCode; }
            set { _regionCode = value; }
        }

        /// <summary>
        /// Method to set the application variables
        /// </summary>
        /// <param name="regionCode">region code of application</param>
        public static void SetApplicationConstants(string regionCode)
        {
            _regionCode = regionCode;
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(regionCode);
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(regionCode);
        }
    }
}
