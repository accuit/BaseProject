using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AccuIT.CommonLayer.Aspects.Utilities
{
    /// <summary>
    /// Class to hold constant values through out assemblies and applications
    /// </summary>
    public static class AppConstants
    {
        /// <summary>
        /// Constant to store the key name of transaction attribute name
        /// </summary>
        public const string RUN_TRANSACTION = "RunTransaction";

        // Added new properties to set message content for Master Excel Upload added by Neeraj Singh on 21st Dec 2015
        public const string FileMaxSize5MB = "Max size of uploaded file should not be more then 5 MB.";
        public const string InvalidFile = "File is invalid.";
        public const string InvalidData = "Data in uploaded file is invalid.";
        public const string ExcelColimnMismatch = "Excel Sheet Column is different than what is required.Please Try again.";
        public const string UpdateSuccessfully = "{0} records inserted and {1} records updated successfully in {2}";
        public const string uploadMasterError = "Error occure while uploading master data, this is due to excel may be contain incorrect data.";
        public const string NoDataFound = "No data found to update.";
        public const string BlankRows = "File should not have blank rows.";
        public const string Error = "Error occured,Please contact to Administrator.";
        public const string InvalidColumnSpelling = "Excel Sheet Column Header is different than what is required.It must be : Store Code";
        public const string InvalidColumnSpellingAllColumn = "Excel Sheet Column Header is different than what is required.It must be same as sample file.";
        public const string DuplicateData = "Duplicate record found in excel for the following StoreCode : ";

        public const string InvalidFileFormatxls = "Please Upload xls or xlsx file only.";
        public const string InvalidFileName = "File Name is Invalid.";
        public const string InvalidFilePath = "File Path does not exist";
        public const string NascaProtectedFile = "Selected File is either corrupted or NASCA protected.";
    }
}
