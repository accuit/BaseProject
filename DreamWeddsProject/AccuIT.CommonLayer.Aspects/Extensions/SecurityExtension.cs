//using System;
//using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Text.RegularExpressions;
//using System.Threading.Tasks;

namespace AccuIT.CommonLayer.Aspects.Extensions
{
    public static class SecurityExtension
    {

        public static string SanitizeHtml(this string html)
        {
            //string acceptable = "script|link|title";
            string acceptable = "strong|em|p|font|u|b|i|blockquote|br|span|h1|h2|h3|h4|h5|h6|sub|sup|ul|ol|li|hr|table|tr|td|th";
            string stringPattern = @"</?(?(?=" + acceptable + @")notag|[a-zA-Z0-9]+)(?:\s[a-zA-Z0-9\-]+=?(?:(["",']?).*?\1?)?)*\s*/?>";
            return System.Text.RegularExpressions.Regex.Replace(html, stringPattern, "");
        }

        #region Validate Image(JPG, PNG) Content Type
        public static bool IsImageValid(HttpPostedFile value)
        {
            bool isValid = false;
            var file = value;

            if (file == null || file.ContentLength > 1 * 1024 * 1024)
            {
                return isValid;
            }

            if (IsFileTypeValid(file))
            {
                isValid = true;
            }

            return isValid;
        }

        private static bool IsFileTypeValid(HttpPostedFile value)
        {
            bool isValid = false;

            try
            {
                using (var img = System.Drawing.Image.FromStream(value.InputStream))
                {
                    if (IsOneOfValidFormats(img.RawFormat))
                    {
                        isValid = true;
                    }
                }
            }
            catch
            {
                isValid = false;
            }
            return isValid;
        }

        private static bool IsOneOfValidFormats(ImageFormat rawFormat)
        {
            List<ImageFormat> formats = getValidFormats();

            foreach (ImageFormat format in formats)
            {
                if (rawFormat.Equals(format))
                {
                    return true;
                }
            }
            return false;
        }

        private static List<ImageFormat> getValidFormats()
        {
            List<ImageFormat> formats = new List<ImageFormat>();
            formats.Add(ImageFormat.Png);
            formats.Add(ImageFormat.Jpeg);
            return formats;
        }
        #endregion

        public static bool IsComplexPassword(this string Password, ref List<string> ErrorMessage)
        {
            bool IsSuccess = true;
            //ErrorMessage = new List<string>();
            if (Password.Length < 6)
            {
                ErrorMessage.Add("Password must have at least 10 characters");
                IsSuccess = false;
                return false;
            }
            if (!Regex.IsMatch(Password, "[A-Z]", RegexOptions.Singleline))
            {
                ErrorMessage.Add("Password must have at least 1 uppercase character (A-Z)");
                IsSuccess = false;
                return false;
            }
            if (!Regex.IsMatch(Password, "[a-z]", RegexOptions.Singleline))
            {
                ErrorMessage.Add("Password must have at least 1 lowercase character (a-z) ");
                IsSuccess = false;
                return false;
            }
            if (!Regex.IsMatch(Password, "[0-9]", RegexOptions.Singleline))
            {
                ErrorMessage.Add("Password must have at least 1 digit (0-9)");
                IsSuccess = false;
                return false;
            }
            if (Regex.IsMatch(Password, @"([a-zA-Z0-9!@#$%^&*()_+}{"":;'?<;.>;,-=|~`\]\[\\\s])\1\1", RegexOptions.Singleline))
            {
                ErrorMessage.Add("Password cannot have more than 2 identical characters in a row (e.g., 111 not allowed)");
                IsSuccess = false;
                return false;
            }
            return IsSuccess;
        }
    }
}
