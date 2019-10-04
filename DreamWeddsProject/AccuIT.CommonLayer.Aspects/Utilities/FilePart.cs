// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FilePart.cs" company="Jake Woods">
//   Copyright (c) 2013 Jake Woods
//   
//   Permission is hereby granted, free of charge, to any person obtaining a copy of this software 
//   and associated documentation files (the "Software"), to deal in the Software without restriction, 
//   including without limitation the rights to use, copy, modify, merge, publish, distribute, 
//   sublicense, and/or sell copies of the Software, and to permit persons to whom the Software 
//   is furnished to do so, subject to the following conditions:
//   
//   The above copyright notice and this permission notice shall be included in all copies 
//   or substantial portions of the Software.
//   
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//   INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR 
//   PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR 
//   ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, 
//   ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <author>Jake Woods</author>
// <summary>
//   Represents a single file extracted from a multipart/form-data
//   stream.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using System.Linq;

namespace AccuIT.CommonLayer.Aspects.Utilities.HttpMultipartParser
{
    using System;
    using System.IO;
    using System.Web;

    /// <summary>
    ///     Represents a single file extracted from a multipart/form-data
    ///     stream.
    /// </summary>
    public class FilePart
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePart"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the input field used for the upload.
        /// </param>
        /// <param name="fileName">
        /// The name of the file.
        /// </param>
        /// <param name="data">
        /// The file data.
        /// </param>
        public FilePart(string name, string fileName, Stream data) :
            this(name, fileName, data, "text/plain", "form-data")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePart"/> class.
        /// </summary>
        /// <param name="name">
        /// The name of the input field used for the upload.
        /// </param>
        /// <param name="fileName">
        /// The name of the file.
        /// </param>
        /// <param name="data">
        /// The file data.
        /// </param>
        /// <param name="contentType">
        /// The content type.
        /// </param>
        /// <param name="contentDisposition">
        /// The content disposition.
        /// </param>
        public FilePart(string name, string fileName, Stream data, string contentType, string contentDisposition)
        {
            this.Name = name;
            this.FileName = fileName.Split(Path.GetInvalidFileNameChars()).Last();
            this.Data = data;
            this.ContentType = contentType;
            this.ContentDisposition = contentDisposition;
        }

        #endregion

      
        #region Public Properties

        /// <summary>
        ///     Gets the data.
        /// </summary>
        public Stream Data { get; private set; }

        /// <summary>
        ///     Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the content-type. Defaults to text/plain if unspecified.
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        ///  Gets or sets the content-disposition. Defaults to form-data if unspecified.
        /// </summary>
        public string ContentDisposition { get; set; }

        #endregion
    }

    public static class FileUpload
    {
        public static string GetFileName(string FileNameWithOutExtension, ref HttpPostedFileBase fpFileUpload)
        {
            if ((fpFileUpload != null))
            {
                string strFileExt = System.IO.Path.GetExtension(fpFileUpload.FileName.ToString());
                if (strFileExt == null)
                {
                    return string.Empty;
                }
                else
                {
                    return FileNameWithOutExtension + "" + strFileExt;
                }
            }
            else
            {
                return string.Empty;
            }

        }
        public static string UploadFile(string FileNameWithPathOnServer, ref HttpPostedFileBase fpFileUpload, string[] extensionAllowed = null)
        {
            string strFilename1 = System.IO.Path.GetFileName(fpFileUpload.FileName.ToString());
            string strPath = string.Empty;
            string ApplicationPath = HttpContext.Current.Request.ApplicationPath;

            if (FileNameWithPathOnServer.Split(new char[] { ':', '\\' }).Length > 1)
            {
                strPath = FileNameWithPathOnServer;
            }
            else if (FileNameWithPathOnServer.Split(new char[] { '~' }).Length > 1)
            {
                strPath = HttpContext.Current.Server.MapPath(FileNameWithPathOnServer);
            }
            else
            {
                if (ApplicationPath.Split(new char[] { '/' }).Length > 1)
                {
                    if (FileNameWithPathOnServer.IndexOf(ApplicationPath.Split(new char[] { '/' })[1]) > 0)
                    {
                        FileNameWithPathOnServer = FileNameWithPathOnServer.Replace(ApplicationPath.Split(new char[] { '/' })[1], "");
                    }
                }
                strPath = HttpContext.Current.Server.MapPath("~\\" + FileNameWithPathOnServer);
            }

            string strFileExt = System.IO.Path.GetExtension(fpFileUpload.FileName.ToString());
            string strServerFileExt = System.IO.Path.GetExtension(FileNameWithPathOnServer);

            if (fpFileUpload == null || string.IsNullOrEmpty(strFilename1))
            {
                return AppConstants.InvalidFileName;
            }
            if (strFileExt == null | strFileExt.ToLower() != strServerFileExt.ToLower())
            {
                return AppConstants.InvalidFileFormatxls;
            }
            if (extensionAllowed != null && extensionAllowed.Count() > 0)
            {
                bool isValidFileType = false;
                foreach (string s in extensionAllowed)
                {
                    if (strFileExt.ToLower() == s.ToLower())
                    {
                        isValidFileType = true;
                        break;
                    }
                }
                if (!isValidFileType)// if extension not exists in list then return error
                {
                    return AppConstants.InvalidFileFormatxls;
                }

            }
            else if (!CheckUploadFileNameAndExtensoin(strFilename1, strFileExt.ToLower()))
            {
                return AppConstants.InvalidFileFormatxls;
            }

            if (string.IsNullOrEmpty(strPath))
            {
                return AppConstants.InvalidFilePath;
            }
            //string temp = System.Web.HttpContext.Current.Server.MapPath(strPath);
            //fpFileUpload.PostedFile.SaveAs(strPath);
            string temp = System.Web.HttpContext.Current.Server.MapPath(strPath);
            fpFileUpload.SaveAs(temp);

            return "Success";

        }
        public static bool CheckUploadFileNameAndExtensoin(string FileName, string FileExtension)
        {
            //Check File Name
            string[] InvalidChar = null;
            bool CheckValue = true;
            InvalidChar = new string[] { "!", "^", ";", "&", "%", ".", "\\", "/", "$", "@", "*", "#", "~", "`", "-", "+", "=", "(", ")" };
            foreach (char strchar in FileName)
            {
                for (Int16 charIndex = 0; charIndex <= InvalidChar.Length - 1; charIndex++)
                {
                    if (strchar.Equals((InvalidChar[charIndex].ToString())))
                    {
                        CheckValue = false;
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }
            if (CheckValue == true)
            {
                //Check File Extension
                switch (FileExtension.ToLower())
                {
                    case ".xls":
                        return true;
                    case ".xlsx":
                        return true;
                    case ".xlsb":
                        return true;
                    default:
                        return false;
                }
            }
            else
            {
                return false;
            }
        }

    }
}