using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccuIT.CommonLayer.Aspects.Extensions
{
    /// <summary>
    /// Extension class to provide add-in methods for string data types
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Extension method to check whether passed value is null or not and in case of null/empty value
        /// replace with input value
        /// </summary>
        /// <param name="value">value to validate</param>
        /// <param name="valueToReplace">value to replace</param>
        /// <returns>returns updated value</returns>
        public static string IsNull(this string value, string valueToReplace)
        {
            if (String.IsNullOrEmpty(value))
            {
                return valueToReplace;
            }
            return value;
        }

        /// <summary>
        /// Extension method to convert string into integer array separated by specified separator 
        /// </summary>
        /// <param name="value">value to convert into integer array</param>
        /// <param name="separator">value of separator</param>
        /// <returns>return integer array</returns>
        public static int[] ToIntArray(this string value, char separator)
        {
            return Array.ConvertAll(value.Split(separator), s => int.Parse(s));
        }

        /// <summary>
        /// Method to format the phone number as per defined region code
        /// </summary>
        /// <param name="phone">phone number</param>
        /// <param name="regionCode">region code</param>
        /// <returns>returns formatted string</returns>
        public static string FormatPhoneNumber(this string phone, string regionCode)
        {
            return String.Format("{0:##-##-##-##-##}", double.Parse(phone));
        }

        public static string FindReplace(this string input, string replaceTag, string replaceVal)
        {
            StringBuilder b = new StringBuilder(input);
            string FinalVal = String.Empty;
            FinalVal = b.Replace(replaceTag, replaceVal).ToString();


            return FinalVal;
        }
    }
}
