using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;

namespace AccuIT.CommonLayer.Aspects.Module
{
    /// <summary>
    /// Get the JOSN Message format for the given content type
    /// </summary>
    /// <param name="contentType">HttpContext Request type</param>
    /// <returns>JSOn Formatted data</returns>
    public class JsonContentMapper : WebContentTypeMapper
    {
        /// <summary>
        /// Method to override message format for content type in Json services
        /// </summary>
        /// <param name="contentType">content type</param>
        /// <returns>returns json format</returns>
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            return WebContentFormat.Json;
        }
    }
}
