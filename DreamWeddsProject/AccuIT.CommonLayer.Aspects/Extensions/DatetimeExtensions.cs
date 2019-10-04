using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using AccuIT.CommonLayer.Aspects.Configurations;

namespace AccuIT.CommonLayer.Aspects.Extensions
{
    /// <summary>
    /// Date Time extension class to provide additional conversion and formatting methods for Date Time objects
    /// </summary>
    public static class DatetimeExtensions
    {
        /// <summary>
        /// Extension method to get date time on the basis of user culture
        /// </summary>
        /// <param name="date">input date value</param>
        /// <returns>returns formatted date time value</returns>
        public static string GetCultureDate(this DateTime date)
        {
            if (String.IsNullOrEmpty(ApplicationVariable.RegionCode))
                ApplicationVariable.RegionCode = "en-US";
            return date.ToString(new CultureInfo(ApplicationVariable.RegionCode).DateTimeFormat);
        }


        /// <summary>
        /// Extension method to get only date on the basis of user culture
        /// </summary>
        /// <param name="date">input date value</param>
        /// <returns>returns formatted date time value</returns>
        public static string GetCultureShortDate(this DateTime date)
        {
            if (String.IsNullOrEmpty(ApplicationVariable.RegionCode))
                ApplicationVariable.RegionCode = "en-US";
            return date.ToString(new CultureInfo(ApplicationVariable.RegionCode).DateTimeFormat.ShortDatePattern);
        }


        /// <summary>
        /// Extension method to parse the date time string into application date time format
        /// </summary>
        /// <param name="dateValue">date value</param>
        /// <param name="inputFormat">input format value</param>
        /// <returns>returns converted date</returns>
        public static DateTime ConvertToAppDate(this string dateValue, string inputFormat)
        {
            if (String.IsNullOrEmpty(ApplicationVariable.RegionCode))
                ApplicationVariable.RegionCode = "en-US";
            DateTime newDate;
            newDate = DateTime.ParseExact(dateValue, inputFormat, new CultureInfo(ApplicationVariable.RegionCode).DateTimeFormat, DateTimeStyles.None);
            return newDate;
        }

        /// <summary>
        /// Extension method to parse the date time string into application date time format
        /// </summary>
        /// <param name="dateValue">date value</param>
        /// <returns>returns converted date</returns>
        public static DateTime ConvertToAppDate(this string dateValue)
        {
            if (String.IsNullOrEmpty(ApplicationVariable.RegionCode))
                ApplicationVariable.RegionCode = "en-US";
            CultureInfo culture = new CultureInfo(ApplicationVariable.RegionCode);
            DateTime newDate;
            newDate = DateTime.ParseExact(dateValue, culture.DateTimeFormat.ShortDatePattern, culture.DateTimeFormat, DateTimeStyles.None);
            return newDate;
        }

        public static DateTime? ConvertToNullableDateTime(this string dateValue)
        {
            return string.IsNullOrEmpty(dateValue) ? (DateTime?)null : Convert.ToDateTime(dateValue);
        }

       
    }
}
