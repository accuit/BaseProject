using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Samsung.SmartDost.CommonLayer.Aspects.IO
{
    /// <summary>
    /// Class to handle Input/Output operations in application
    /// </summary>
    public static class FileManager
    {
        /// <summary>
        /// Method to read text from provided file path in application
        /// </summary>
        /// <param name="filePath">file complete qualified path</param>
        /// <returns>returns file text as string</returns>
        public static string ReadFile(string filePath)
        {
            string fileText = string.Empty;
            FileInfo fileInfo = new FileInfo(filePath);
            fileText = fileInfo.OpenText().ReadToEnd();
            fileInfo = null;
            return fileText;
        }
    }
}
