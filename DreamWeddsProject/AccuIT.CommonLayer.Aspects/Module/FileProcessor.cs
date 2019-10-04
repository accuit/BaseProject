using System;
using System.IO;
using System.Web;
using System.Web.Configuration;

namespace AccuIT.CommonLayer.Aspects.Module
{
    /// <summary>
    /// File Processor class to provide requested file from file server
    /// </summary>
    public class FileProcessor : IHttpHandler
    {
        #region Constants
        //DealerCreation
        private const string CLIENT_BIN_NAME = @"ClientBin\";
        private const string CSV_RESPONSE_CONTENT_TYPE = "application/CSV";
        private const string HTML_RESPONSE_CONTENT_TYPE = "text/HTML";
        private const string CONTENT_DISPOSITION_HEADER = "Content-Disposition";
        private const string ATTACHMENT_FORMAT = "attachment;filename={0}";
        private const string CONTENT_LENGTH_HEADER = "Content-Length";
        private const string IMAGE_RESPONSE_CONTENT_TYPE = "image/jpeg";
        private const string FILE_SERVER_DEALERCREATION_IMAGE_FOLDER = @"Contents\DealerCreation\";
        private const string FILE_SERVER_IMAGE_FOLDER = @"Contents\SDImages\";
        private const string FILE_SERVER_USER_IMAGE_FOLDER = @"Contents\UserFiles\";
        private const string FILE_SERVER_STORE_IMAGE_FOLDER = @"Contents\StoreFiles\";
        private const string FILE_SERVER_GENERAL_IMAGE_FOLDER = @"Contents\GeneralFiles\";
        private const string FILE_SERVER_FMS_IMAGE_FOLDER = @"Contents\FMS\";
        private const string FILE_SERVER_APK_FOLDER = @"Contents\APKFiles\";
        private const string FILE_SERVER_QC_FOLDER = @"Contents\QCTool\";
        private const string FILE_SERVER_REPORT_FOLDER = @"ReportFiles\";
        private const string FILE_SERVER_SCHEMES_FOLDER = @"ReportFiles\Schemes\";
        private const string FILE_SERVER_SCHEMES_IMAGES_FOLDER = @"ReportFiles\Schemes\Images\";
        private const string FILE_SERVER_APKICON_FOLDER = @"APKICON\";
        private const string FILE_SERVER_FMSICON_FOLDER = @"APKICON\FeedbackTypeIcon\";
        private const string FILE_SERVER_QUESTIONICON_FOLDER = @"APKICON\APKQuestionIcon\";
        private const string OTHER_QUERYSTRING_KEY = "other";
        private const string SIZE_QUERYSTRING_KEY = "size";
        private const string FILENAME_QUERYSTRING_KEY = "id";
        private const string DEFAULT_IMAGE_NAME = "18K.jpg";
        private const string No_IMAGE_NAME = "NOIMAGE.jpg";
        private const string FILE_TYPE_QUERYSTRING_KEY = "type";


        #endregion
        private FileInfo requestedFileInfo = null;
        private string fileType = string.Empty;
        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        /// <summary>
        /// Http Handler interface to process the http web context request
        /// </summary>
        /// <param name="context">http context</param>
        public void ProcessRequest(HttpContext context)
        {
            string fileName = String.Empty;
            FileStream stream = null;
            if (context.Request.QueryString[FILENAME_QUERYSTRING_KEY] != null && context.Request.QueryString[FILE_TYPE_QUERYSTRING_KEY] != null)
            {
                fileName = Convert.ToString(context.Request.QueryString[FILENAME_QUERYSTRING_KEY]);
                fileType = Convert.ToString(context.Request.QueryString[FILE_TYPE_QUERYSTRING_KEY]);
            }
            else
            {
                throw new ArgumentException("No Image ID parameter specified");
            }
            GetRequestedFile(context, fileName, stream);
        }

        /// <summary>
        /// Method to handle the call back event
        /// </summary>
        /// <returns>returns boolean status</returns>
        public bool ThumbnailCallback()
        {
            return true;
        }

        /// <summary>
        /// Method to get requested file by http handler
        /// </summary>
        /// <param name="context">http context</param>
        /// <param name="fileName">file name</param>
        /// <param name="sizeValue">size specification</param>
        /// <param name="stream">file stream</param>
        private void GetRequestedFile(HttpContext context, string fileName, FileStream stream)
        {

            string filePath = String.Empty;
            filePath = GetImagePath(fileName, context);
            if (!String.IsNullOrEmpty(fileName))
            {
                //GET THE FILE STREAM
                stream = GetFileStream(filePath, context);

                if (stream != null)
                {
                    //GET THE REQUESTED FILE INFO
                    requestedFileInfo = GetFileInfo(filePath, context);

                    GetRequestedFile(stream, context);
                }
            }
        }

        /// <summary>
        /// This function returns the file path or null string 
        /// </summary>
        /// <param name="fileName">The name of image file</param>
        /// <param name="context">The context of http</param>
        /// <param name="isRootFiles">check for root folder images</param>
        /// <returns></returns>
        private string GetImagePath(string fileName, HttpContext context)
        {
            string filePath = string.Empty;
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    //get the file stream
                    return filePath;
                }
                switch (fileType)
                {
                    case "User":
                        filePath = context.Server.MapPath(FILE_SERVER_USER_IMAGE_FOLDER + fileName);
                        break;
                    case "Store":
                        filePath = context.Server.MapPath(FILE_SERVER_STORE_IMAGE_FOLDER + fileName);
                        break;
                    case "Survey":
                        filePath = context.Server.MapPath(FILE_SERVER_IMAGE_FOLDER + fileName);
                        break;
                    case "General":
                        filePath = context.Server.MapPath(FILE_SERVER_GENERAL_IMAGE_FOLDER + fileName);
                        break;
                    case "Dealer":
                        filePath = context.Server.MapPath(FILE_SERVER_DEALERCREATION_IMAGE_FOLDER + fileName);
                        break;
                    case "FMS":
                        filePath = context.Server.MapPath(FILE_SERVER_FMS_IMAGE_FOLDER + fileName);
                        break;
                    case "APK":
                        filePath = context.Server.MapPath(FILE_SERVER_APK_FOLDER + fileName);
                        break;
                    case "QC":
                        filePath = context.Server.MapPath(FILE_SERVER_QC_FOLDER + fileName);
                        break;
                    case "Schemes":
                        filePath = context.Server.MapPath(FILE_SERVER_SCHEMES_FOLDER + fileName);
                        break;
                    case "SchemesImage":
                        filePath = context.Server.MapPath(FILE_SERVER_SCHEMES_IMAGES_FOLDER + fileName);
                        break;
                    case "APKIcon":
                        filePath = context.Server.MapPath(FILE_SERVER_APKICON_FOLDER + fileName);
                        break;
                    case "FMSIcon":
                        filePath = context.Server.MapPath(FILE_SERVER_FMSICON_FOLDER + fileName);
                        break;
                    case "QuestionIcon":
                        filePath = context.Server.MapPath(FILE_SERVER_QUESTIONICON_FOLDER + fileName);
                        break;
                    case "Report":
                        filePath = context.Server.MapPath(FILE_SERVER_REPORT_FOLDER + fileName);
                        break;
                    default:
                        filePath = context.Server.MapPath(FILE_SERVER_IMAGE_FOLDER + fileName);
                        break;
                }

                if (File.Exists(filePath))
                    return filePath;
                else
                    return string.Empty;

            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Method to get file info details
        /// </summary>
        /// <param name="filePath">complete file path</param>
        /// <param name="context">http context</param>
        /// <returns>returns file info details</returns>
        private FileInfo GetFileInfo(string filePath, HttpContext context)
        {
            FileInfo fileInfo = null;
            if (System.IO.File.Exists(filePath))
            {
                fileInfo = new FileInfo(filePath);
            }
            else
            {
                fileInfo = new FileInfo(context.Server.MapPath(FILE_SERVER_IMAGE_FOLDER + DEFAULT_IMAGE_NAME));
            }
            return fileInfo;
        }

        /// <summary>
        /// Method to get file stream
        /// </summary>
        /// <param name="filePath">file complete path</param>
        /// <param name="context">http context</param>
        /// <returns>returns file stream</returns>
        private FileStream GetFileStream(string filePath, HttpContext context)
        {
            FileStream stream = null;
            if (System.IO.File.Exists(filePath))
            {
                stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            else
            {
                //stream = new FileStream(context.Server.MapPath(FILE_SERVER_IMAGE_FOLDER + DEFAULT_IMAGE_NAME), FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                stream = null;
            }
            return stream;
        }

        /// <summary>
        /// Method to get image file in actual size
        /// </summary>
        /// <param name="context">http context</param>
        /// <param name="stream">file stream</param>
        /// <returns>returns byte array</returns>
        private byte[] GetImageWithoutSizeSpecification(HttpContext context, FileStream stream)
        {
            byte[] browsedFile;
            System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(stream);
            long _TotalBytes = requestedFileInfo.Length;
            browsedFile = binaryReader.ReadBytes((Int32)_TotalBytes);
            binaryReader.Close();
            binaryReader.Dispose();
            stream.Close();
            stream.Dispose();
            context.Response.Clear();
            context.Response.ContentType = IMAGE_RESPONSE_CONTENT_TYPE;
            context.Response.BinaryWrite(browsedFile);
            context.Response.End();
            return browsedFile;
        }
        /// <summary>
        /// Method to get image file in actual size
        /// </summary>
        /// <param name="context">http context</param>
        /// <param name="stream">file stream</param>
        /// <returns>returns byte array</returns>
        private byte[] GetFile(HttpContext context, FileStream stream)
        {
            byte[] browsedFile;
            System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(stream);
            long _TotalBytes = requestedFileInfo.Length;
            browsedFile = binaryReader.ReadBytes((Int32)_TotalBytes);
            binaryReader.Close();
            binaryReader.Dispose();
            stream.Close();
            stream.Dispose();
            context.Response.Clear();
            context.Response.ContentType = HTML_RESPONSE_CONTENT_TYPE;
            context.Response.BinaryWrite(browsedFile);
            context.Response.End();
            return browsedFile;
        }

        /// <summary>
        /// Method to get requested file by file handler http context class
        /// </summary>
        /// <param name="stream">file stream</param>
        /// <param name="context">http context</param>
        private void GetRequestedFile(FileStream stream, HttpContext context)
        {
            if (fileType == "Schemes")
                GetFile(stream, context);
            else
                GetImageFile(stream, context);
        }

        /// <summary>
        /// Method to get the Image file as specified 
        /// </summary>
        /// <param name="stream">file stream</param>
        /// <param name="context">http context</param>
        /// <returns>returns byte array</returns>
        private byte[] GetImageFile(FileStream stream, HttpContext context)
        {
            return GetImageWithoutSizeSpecification(context, stream);
        }
        /// <summary>
        /// Method to get the Image file as specified 
        /// </summary>
        /// <param name="stream">file stream</param>
        /// <param name="context">http context</param>
        /// <returns>returns byte array</returns>
        private byte[] GetFile(FileStream stream, HttpContext context)
        {
            return GetFile(context, stream);
        }


    }
}
