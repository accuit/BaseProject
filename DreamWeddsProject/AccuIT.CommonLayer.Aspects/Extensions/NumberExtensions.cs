using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using AccuIT.CommonLayer.Aspects.Configurations;

namespace AccuIT.CommonLayer.Aspects.Extensions
{

    /// <summary>
    /// Number extensions method for conversion process and culture specific conversions
    /// </summary>
    public static class NumberExtensions
    {
        /// <summary>
        /// Method to convert string value into number as per region code provided
        /// </summary>
        /// <param name="value">string input value</param>
        /// <param name="regionCode">region code specified</param>
        /// <returns>returns numeric equivalent</returns>
        public static int ToNumeric(this string value, string regionCode)
        {
            int number = 0;
            bool isValid = int.TryParse(value, System.Globalization.NumberStyles.Currency, new CultureInfo(regionCode).NumberFormat, out number);
            return number;
        }

        /// <summary>
        /// Method to convert the numeric value into culture specific string value
        /// </summary>
        /// <param name="number">number</param>
        /// <param name="regionCode">region code</param>
        /// <returns>returns string value</returns>
        public static string ToNumericString(this int number, string regionCode)
        {
            return number.ToString(new CultureInfo(regionCode).NumberFormat);
        }

        /// <summary>
        /// Method to convert string value into decimal as per region code provided
        /// </summary>
        /// <param name="value">string input value</param>
        /// <param name="regionCode">region code specified</param>
        /// <returns>returns decimal equivalent</returns>
        public static double ToDouble(this string value, string regionCode)
        {
            double number = 0;
            bool isValid = double.TryParse(value, System.Globalization.NumberStyles.Currency, new CultureInfo(regionCode).NumberFormat, out number);
            return number;
        }

        /// <summary>
        /// Method to convert the numeric value into culture specific string value
        /// </summary>
        /// <param name="number">number</param>
        /// <param name="regionCode">region code</param>
        /// <returns>returns string value</returns>
        public static string ToDoubleString(this double number, string regionCode)
        {
            return number.ToString(new CultureInfo(regionCode).NumberFormat);
        }
    }
}
