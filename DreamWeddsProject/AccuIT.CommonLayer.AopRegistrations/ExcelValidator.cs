using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;
using System.Data.OleDb;
using System.Threading.Tasks;
using Samsung.SmartDost.CommonLayer.Aspects.Utilities;
using Samsung.SmartDost.PersistenceLayer.Repository.Contracts;
using Samsung.SmartDost.CommonLayer.AopContainer;

/*
=========================================CHANGE HISTORY========================================================
Create Date           Created By                 Purpose
=============================================================================================================== 
01-Oct-2014           Vaishali Choudhary         Validate and Upload Excel data to Unfreeze GeoTag 
===============================================================================================================
 */
namespace Samsung.SmartDost.CommonLayer.AopRegistrations
{
    public class ExcelValidator
    {
        
        Dictionary<int, string> ValidationMessage = new Dictionary<int, string>();
        DataTable ExcelData = new DataTable();
        DataTable TemplateTable = new DataTable();
        int ErrorType = 0;

        #region Define Repository Instance
        private ISystemRepository systemRepositoryInstance;        
        private ISystemRepository SystemRepositoryInstance
        {
            get
            {
                if (systemRepositoryInstance == null)
                {
                    systemRepositoryInstance = AopEngine.Resolve<ISystemRepository>(AspectEnums.PeristenceInstanceNames.SystemDataImpl, AspectEnums.ApplicationName.Samsung);
                }
                return systemRepositoryInstance;
            }
        }
        #endregion

        #region Process File
        public KeyValuePair<int,string> SaveFileToDatabase(string filePath,string TemplatePath, string SheetName, int ModuleCode)
        {
            ExcelData = LoadDatafromExcel(filePath, SheetName);
            TemplateTable = LoadDatafromExcel(TemplatePath, SheetName);
            GetValidationMessage();
            if (ExcelData == null || ExcelData.Rows.Count == 0)
            {
                ErrorType = 1;                
            }
            else
            {
                if (CompareExcelFormat(ExcelData, TemplateTable))
                {
                    if (ValidateData(ExcelData))
                    {
                        ProcessGeoTagUnFreezFile(ExcelData, ModuleCode);
                    }
                }
            }
            if (File.Exists(filePath))
                File.Delete(filePath);
            return ValidationMessage.Where(x => x.Key == ErrorType).FirstOrDefault();
            //return ErrorType;
        }
        #endregion

        #region List of Error Message
        private void GetValidationMessage()
        {
            ValidationMessage.Add(0, "");
            ValidationMessage.Add(1, "No Data in Excel!");                                                            
            ValidationMessage.Add(2, "Column Mismatch!");
            ValidationMessage.Add(3, "Column name Mismatch!");
            ValidationMessage.Add(4, "Null value found!");
            ValidationMessage.Add(5, "Duplicate records!");
            ValidationMessage.Add(6, "Store Code not correct!");

        }
        #endregion

        #region Validate Data
        // load excel from folder and convert into DataTable
        private DataTable LoadDatafromExcel(string filePath, string SheetName)
        {
            String strConnection = System.Configuration.ConfigurationManager.ConnectionStrings["SmartDostLog_DB_Connection"].ConnectionString;
            String excelConnString = String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0\"", filePath);
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

        // Validate data 
        private bool ValidateData(DataTable Excel)
        {
            bool IsValidated;
            //Save data in Collection            
            List<string> StoreList = (from r in ExcelData.AsEnumerable()
                                      select r.Field<string>("StoreCode")).ToList();


            //Check for Null values
            IsValidated = StoreList.Where(item => item == null).Count() == 0 ? true : false;
            ErrorType = IsValidated == false ? 4 : ErrorType;

            //check for duplicate rows
            if (IsValidated)
            {
                IsValidated = StoreList.GroupBy(item => item).Any(x => x.Count() > 1)== true ? false : true;
                ErrorType = IsValidated == false ? 5 : ErrorType;
            }

            return IsValidated;

        }
        #endregion

        #region Process successfully validated data
        private bool ProcessGeoTagUnFreezFile(DataTable ExcelData, int modulecode)
        {
            systemRepositoryInstance = SystemRepositoryInstance;

            List<string> StoreCode = (from r in ExcelData.AsEnumerable()
                                      select r.Field<string>("StoreCode")).ToList();           

            var result=systemRepositoryInstance.ValidateStoreCode(StoreCode);
            ErrorType = result.Count() == 0 ? ErrorType : 6;

            return ErrorType == 0 ? systemRepositoryInstance.UnfreezeGeoTag(StoreCode) : false;

        }
        #endregion

    }
}
