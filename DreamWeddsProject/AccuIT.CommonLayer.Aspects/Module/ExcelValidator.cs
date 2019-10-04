using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Threading.Tasks;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.CommonLayer.Aspects.ReportBO;

/*
=========================================CHANGE HISTORY========================================================
Create Date           Created By                 Purpose
=============================================================================================================== 
01-Oct-2014           Vaishali Choudhary         Validate Excel with template
===============================================================================================================
 */
namespace Samsung.SmartDost.CommonLayer.Aspects.Module
{
    public class ExcelValidator
    {

       
        public ExcelValidator()
        {
        }

        /*Constructor overloading done to pass the type of UI for excel validation
         * so that accordingly null columns can be checked.
         * */
        int uploadfor = 0; // flag for geotagging
        /// <summary>
        /// Pass uploadfor to identify the UI for Excel validation
        /// </summary>
        /// <param name="Uploadfor"></param>
        public ExcelValidator(int Uploadfor)
        {
            uploadfor = Uploadfor;
        }

        Dictionary<int, string> ValidationMessage = new Dictionary<int, string>();
        DataTable ExcelData = new DataTable();
        DataTable TemplateTable = new DataTable();
        int ErrorType = 0;


        #region Process File
        public KeyValuePair<int, string> ValidateExcel(string filePath, string TemplatePath, string SheetName, int ModuleCode, string excelType, string Uploadfor = null)
        {
            ExcelData = LoadDatafromExcel(filePath, SheetName, excelType);
            TemplateTable = LoadDatafromExcel(TemplatePath, SheetName, ".xlsx");
            GetValidationMessage();
            if (ExcelData == null || ExcelData.Rows.Count == 0)
            {
                ErrorType = 1;
            }
            else
            {
                //compare with template
                if (CompareExcelFormat(ExcelData, TemplateTable))
                {
                    //validate for null and duplicate records
                    ChooseValidate(uploadfor, ExcelData);
                }

            }
            return ValidationMessage.Where(x => x.Key == ErrorType).FirstOrDefault();
            //return ErrorType;
        }
        #endregion

        private bool ChooseValidate(int uploadfor, DataTable ExcelData)
        {
            switch (uploadfor)
            {
                case 0:
                    return ValidateData(ExcelData);
                case 1:
                    return true;
                default:
                    return false;
            }
        }

        #region List of Error Message
        private void GetValidationMessage()
        {
            ValidationMessage.Add(0, "");
            ValidationMessage.Add(1, "No Data available in Uploaded File!");
            ValidationMessage.Add(2, "Column Mismatch with Template!");
            ValidationMessage.Add(3, "Column name Mismatch with Template!");
            ValidationMessage.Add(4, "Null value(s) found!");
            ValidationMessage.Add(5, "Duplicate records!");
            ValidationMessage.Add(6, "Store Code not correct!");

        }
        #endregion

        #region Validate Data
        private string excelTypeConnection(string excelType, string filePath)
        {
            //return String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0\"", filePath);                
            if (excelType == ".xlsx")
                return String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;IMEX=1;HDR=YES\"", filePath);
            else
                return String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0;IMEX=1;\"", filePath);
        }
        // load excel from folder and convert into DataTable
        public DataTable LoadDatafromExcel(string filePath, string SheetName, string excelType)
        {
            String strConnection = System.Configuration.ConfigurationManager.ConnectionStrings["SmartDostLog_DB_Connection"].ConnectionString;
            String excelConnString = excelTypeConnection(excelType, filePath);
            string excelQuery = "SELECT * from [" + SheetName + "$]";
            DataTable dt = new DataTable();

            //Create Connection to Excel work book 
            using (OleDbConnection excelConnection = new OleDbConnection(excelConnString))
            {
                //Create OleDbCommand to fetch data from Excel 
                using (OleDbCommand cmd = new OleDbCommand(excelQuery, excelConnection))
                {
                    excelConnection.Open();
                    OleDbDataAdapter DA = new OleDbDataAdapter();
                    DA.SelectCommand = cmd;
                    DA.Fill(dt);
                }
            }
            return dt;
        }

        // Compare Uploaded format with Template
        private bool CompareExcelFormat(DataTable ExcelData, DataTable Template)
        {
            bool Validate;
            Validate = ExcelData.Columns.Count == Template.Columns.Count ? true : false;

            if (Validate == true)
            {
                //check excel format
                int counter = 0;
                foreach (DataColumn TCol in Template.Columns)
                {
                    if (!(ExcelData.Columns[counter].ColumnName == TCol.ColumnName))
                    {
                        Validate = false;
                        ErrorType = 3;
                        break;
                    }
                    counter++;
                }
            }
            else
            {
                ErrorType = 2;
            }

            return Validate;
        }



        //Validate data for GEO Tag Freezing
        private bool ValidateData(DataTable Excel)
        {
            bool IsValidated;
            //Save data in Collection            
            var StoreList = (from r in ExcelData.AsEnumerable()
                             select r.Field<string>("StoreCode")).ToList();


            //Check for Null values
            IsValidated = StoreList.Where(item => item == null).Count() == 0 ? true : false;
            ErrorType = IsValidated == false ? 4 : ErrorType;

            //check for duplicate rows
            if (IsValidated)
            {
                IsValidated = StoreList.GroupBy(item => item).Any(x => x.Count() > 1) == true ? false : true;
                ErrorType = IsValidated == false ? 5 : ErrorType;
            }

            return IsValidated;

        }

        #endregion

    }
}
