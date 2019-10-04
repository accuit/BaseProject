#region Namespace declaration
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Data.SqlClient;
using System.Configuration;
using AccuIT.CommonLayer.Aspects.Exceptions;
using AccuIT.CommonLayer.Aspects.Utilities;
using AccuIT.CommonLayer.Aspects.Extensions;
using AccuIT.PresentationLayer.WebAdmin.Core;
using AccuIT.CommonLayer.Resources;
using AccuIT.BusinessLayer.Services.Contracts;
using AccuIT.CommonLayer.AopContainer;
using AccuIT.BusinessLayer.Services.BO;
using System.IO;
using ClosedXML.Excel;
using AccuIT.PresentationLayer.WebAdmin.CustomFilter;
using AccuIT.PresentationLayer.WebAdmin.Models;
using AccuIT.CommonLayer.Aspects.Logging;
using AccuIT.PresentationLayer.WebAdmin.ViewDataModel;
using System.Web.Script.Serialization;
using System.Threading.Tasks;
#endregion

namespace AccuIT.PresentationLayer.WebAdmin.Controllers
{
    [AuthorizePage((int)AspectEnums.WebModules.UploadCSV, (int)AspectEnums.RoleType.Admin)]
    [SessionTimeOut]

    public class AdminController : BaseController
    {
        List<ModuleMasterBO> modulesBO = new List<ModuleMasterBO>();
        RoleModulesViewModel MVM = new RoleModulesViewModel();
        DropDown DD = new DropDown();
        // CSV Upload Action
        #region CSV Upload supporting Methods and Action Results

        DataSet ds = null;
        bool isSuccess = false;
        string StrFileName = string.Empty;
        string StrFilePath = string.Empty;
        string masterName = string.Empty;
        string parkingTable = string.Empty;
        int selectedMasterEnum = -1;
        bool isError = false;
        public string MyPath = ConfigurationManager.AppSettings["MasterFiles"].ToString();
        string[] arrProductMasterExcelColumn = new string[] { "ProductName", "CategoryID", "MeasureUnit", "UnitPrice", "Availability", "IsDeleted", "Status", "ImagePath", "Product_Description", "SKUCode", "SKUName", "Color", "Size", "Brand", "Type", "UserType" };
        string[] arrProductImagesExcelColumn = new string[] { "Name", "Caption", "ImageUrl", "ProductID", "CategoryID", "IsPrimeImage" };
        string[] arrProductSpecificsExcelColumn = new string[] { "Header", "HeaderContent", "SKUCode", "CategoryCode", "IsPrimeHeader" };
        string[] arrCategoryMasterExcelColumn = new string[] { "CategoryName", "CategoryCode", "Description", "ImageUrl", "ParentCatgID" };



        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Index()
        {
            // if (this.HasPermission((int)AspectEnums.WebModules.Home_UploadCSV))
            //{
            try
            {
                UserProfileBO empProfile = (UserProfileBO)Session[PageConstants.SESSION_PROFILE_KEY];
                DropDown model = new DropDown();
                var masterlist = model.GetCSVMastersList();
                ViewBag.MasterDataList = masterlist;
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrMessage = "Error: " + ex;
                return View();
            }
            //}
            // else
            //{
            //  return RedirectToAction("UnAuthorizedUser", "Account");
            //}
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(ProductModelBO model, HttpPostedFileBase uploadcsv, FormCollection collection)
        {
            AdminModels admodel = new AdminModels();
            DropDown ddmodel = new DropDown();
            var masterlist = ddmodel.GetCSVMastersList();
            ViewBag.MasterDataList = masterlist;
            try
            {
                ExceptionEngine.ProcessAction(() =>
                {
                    if ((uploadcsv.FileName.Contains(".xls") || uploadcsv.FileName.Contains(".csv")) && uploadcsv.ContentLength > 0)
                    {
                        int contentLength = uploadcsv.ContentLength;
                        if (contentLength > 5000000)
                        {
                            DisplayErrorMessage(AppConstants.FileMaxSize5MB);
                            return;
                        }

                        //StrFileName = hdMasterName.Value;
                        string serverPath = Server.MapPath(MyPath);
                        if (!Directory.Exists(serverPath))
                        {
                            Directory.CreateDirectory(serverPath);
                        }
                        selectedMasterEnum = Convert.ToInt32(collection["ddlselectedvalue"]);
                        #region Get selected master enum description
                        switch (selectedMasterEnum)
                        {

                            case 2:
                                StrFileName = AspectEnums.enumExcelType.ProductMaster.GetDescription();
                                break;
                            case 3:
                                StrFileName = AspectEnums.enumExcelType.CategoryMaster.GetDescription();
                                break;
                            case 4:
                                StrFileName = AspectEnums.enumExcelType.ProductImages.GetDescription();
                                break;
                            case 5:
                                StrFileName = AspectEnums.enumExcelType.ProdSpecification.GetDescription();
                                break;
                        }
                        #endregion

                        model.CreatedBy = (int)HttpContext.Session[PageConstants.SESSION_USER_ID];

                        StrFileName = StrFileName + "_" + model.CreatedBy + "_" + System.DateTime.Now.ToString("yyyyMMddhhmmss");
                        StrFilePath = StrFileName;
                        mGetSetFileName = StrFilePath;
                        StrFileName = GetFileName(StrFileName, ref uploadcsv);
                        StrFileName = MyPath + "\\" + StrFileName;
                        mGetSetFilePath = StrFileName;
                        string strMessage = null;

                        if (string.IsNullOrEmpty(StrFileName))
                        {
                            DisplayErrorMessage(AppConstants.InvalidFile);
                            return;
                        }
                        else
                        {
                            strMessage = UploadFile(StrFileName, ref uploadcsv);
                            if (strMessage.ToUpper() != "SUCCESS")
                            {
                                DisplayErrorMessage(strMessage);
                                return;
                            }
                            ds = new DataSet();
                            ds = ImportExcelXLS(StrFileName);
                            if (ds == null)
                            {
                                return;
                            }
                            else
                            {
                                ds = UpdateTableColumn(ds);
                                if (ValidationOnExcelHeader(ds))
                                {
                                    if (ds.Tables[0].Rows.Count <= 0)
                                    {
                                        // DisplayErrorMessage("No data found to upload.");
                                        ViewBag.ShowPopup = true;
                                        @ViewBag.ErrMessage = "No data found to upload.";
                                        isError = true;
                                        return;
                                    }

                                    mGetSetExcelDataSet = ds;
                                    DataTable finalDT = ProcessExcelData(mGetSetExcelDataSet);

                                    if (isError == false)
                                    {

                                        isSuccess = true;
                                        ViewBag.lblExcelCount = ds.Tables[0].Rows.Count.ToString();
                                        List<int> result = SystemBusinessInstance.UploadMasterDataParking2Main(finalDT, selectedMasterEnum);

                                        if (result.Count > 0)
                                        {
                                            ViewBag.ShowPopup = true;
                                            @ViewBag.ErrMessage = "Data Uploaded Successfully.";
                                            return;
                                        }
                                        else
                                        {
                                            ViewBag.ShowPopup = true;
                                            @ViewBag.ErrMessage = "No data found to update.";
                                            // DisplayErrorMessage("No data found to update.");
                                            return;
                                        }
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        ViewBag.ShowPopup = true;
                        @ViewBag.ErrMessage = "File format is not valid.";
                        isError = true;
                        return;
                    }
                }, AspectEnums.ExceptionPolicyName.AssistingAdministrators.ToString());
            }
            catch (Exception ex)
            {
                // ClientScript.RegisterStartupScript(GetType(), "myScript",
                //                                                      "<script>HidePopup();</script>");
                ViewBag.ShowPopup = true;
                @ViewBag.ErrMessage = ex.Message;
                DisplayErrorMessage(ex.Message);
            }
            return View(admodel);
        }


        #region Validate Excel sheet
        private bool ValidationOnExcelHeader(DataSet Ds)
        {
            string[] arrMainReportColumn = null;
            arrMainReportColumn = arrProductMasterExcelColumn;

            #region case on array selection
            switch (selectedMasterEnum)
            {

                case 2:
                    arrMainReportColumn = arrProductMasterExcelColumn;
                    break;
                case 3:
                    arrMainReportColumn = arrCategoryMasterExcelColumn;
                    break;
                case 4:
                    arrMainReportColumn = arrProductImagesExcelColumn;
                    break;
                case 5:
                    arrMainReportColumn = arrProductSpecificsExcelColumn;
                    break;
            }

            #endregion

            foreach (string column in arrMainReportColumn)
            {
                if (!Ds.Tables[0].Columns.Contains(column))
                {
                    DisplayErrorMessage(AppConstants.InvalidColumnSpellingAllColumn);
                    return false;
                }
            }
            return true;
        }

        private bool ValidationOnBlankRow(DataSet Ds)
        {
            var blankRows = from table in Ds.Tables[0].AsEnumerable()
                            where Convert.ToString(table["Store Code"]) == null || Convert.ToString(table["Store Code"]) == ""
                            select table;
            if (blankRows != null && blankRows.Count() > 0)
            {
                DisplayErrorMessage(AppConstants.BlankRows);
                return false;
            }
            return true;
        }

        private bool FindDuplicateDistCode(DataSet Ds)
        {
            var duplicates = Ds.Tables[0].AsEnumerable()
                .GroupBy(row => row[0])
                .Where(group => group.Count() > 1)
                .Select(g => g.Key);
            string _distcode = "";
            foreach (var item in duplicates)
            {
                _distcode = _distcode + item + ", ";
            }
            if (_distcode != "")
            {
                DisplayErrorMessage(AppConstants.DuplicateData + _distcode.Substring(0, _distcode.Length - 2));
                //ClientScript.RegisterStartupScript(GetType(), "myScript", "<script>ShowMessage('Duplicate record found in excel for the following StoreCode : " + _distcode.Substring(0, _distcode.Length - 2) + "');</script>");
                return false;
            }
            return true;
        }
        #endregion

        #region Excel Upload Methods

        [NonAction]
        private string GetFileName(string FileNameWithOutExtension, ref HttpPostedFileBase fpFileUpload)
        {
            if ((fpFileUpload != null))
            {
                string strFileExt = System.IO.Path.GetExtension(fpFileUpload.FileName.ToString());
                if (strFileExt == null | (strFileExt.ToLower() != ".xls" & strFileExt.ToLower() != ".xlsx" & strFileExt.ToLower() != ".xlsb"))
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
        [NonAction]
        private string UploadFile(string FileNameWithPathOnServer, ref HttpPostedFileBase fpFileUpload, string[] extensionAllowed = null)
        {
            string strFilename1 = System.IO.Path.GetFileName(fpFileUpload.FileName.ToString());
            string strPath = string.Empty;
            string ApplicationPath = HttpContext.Request.ApplicationPath;

            if (FileNameWithPathOnServer.Split(new char[] { ':', '\\' }).Length > 1)
            {
                strPath = FileNameWithPathOnServer;
            }
            else if (FileNameWithPathOnServer.Split(new char[] { '~' }).Length > 1)
            {
                strPath = HttpContext.Server.MapPath(FileNameWithPathOnServer);
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
                strPath = HttpContext.Server.MapPath("~\\" + FileNameWithPathOnServer);
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
        public bool CheckUploadFileNameAndExtensoin(string FileName, string FileExtension)
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
        public DataSet ImportExcelXLS(string FileNameWithPath, bool deleteFileAfterLoad = true)
        {
            try
            {
                string HDR = "No";
                string strConn;
                string strPath = string.Empty;

                string ApplicationPath = HttpContext.Request.ApplicationPath;

                if (FileNameWithPath.Split(new char[] { ':', '\\' }).Length > 1)
                {
                    strPath = HttpContext.Server.MapPath(FileNameWithPath);
                }
                else if (FileNameWithPath.Split(new char[] { '~' }).Length > 1)
                {
                    strPath = HttpContext.Server.MapPath(FileNameWithPath);
                }
                else
                {
                    if (ApplicationPath.Split(new char[] { '/' }).Length > 1)
                    {
                        if (FileNameWithPath.IndexOf(ApplicationPath.Split(new char[] { '/' })[1]) > 0)
                        {
                            FileNameWithPath = FileNameWithPath.Replace(ApplicationPath.Split(new char[] { '/' })[1], "");
                        }
                    }
                    strPath = HttpContext.Server.MapPath("~\\" + FileNameWithPath);
                }

                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strPath + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text\"";


                DataSet output = null;
                using (OleDbConnection conn = new OleDbConnection(strConn))
                {
                    conn.Open();
                    DataTable schemaTable = conn.GetOleDbSchemaTable(
                        OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                    output = new DataSet();
                    foreach (DataRow schemaRow in schemaTable.Rows)
                    {
                        string sheet = schemaRow["TABLE_NAME"].ToString();
                        if (!sheet.EndsWith("_"))
                        {
                            OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + sheet + "]", conn);
                            cmd.CommandType = CommandType.Text;
                            DataTable outputTable = new DataTable(sheet);
                            output.Tables.Add(outputTable);
                            new OleDbDataAdapter(cmd).Fill(outputTable);
                        }
                    }
                    conn.Close();
                }
                return output;
            }
            catch (Exception ex)
            {
                isError = true;
                ViewBag.ShowPopup = true;
                ViewBag.ErrMsg = ex.Message;
                DisplayErrorMessage(AppConstants.NascaProtectedFile);
                DisplayErrorMessage("Selected File is either corrupted or protected.");
                return null;
            }
        }
        #endregion

        public DataSet UpdateTableColumn(DataSet ds)
        {
            if (ds.Tables.Count > 0)
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataColumn item in ds.Tables[0].Columns)
                    {
                        item.ColumnName = ds.Tables[0].Rows[0][item.Caption].ToString();
                    }
                    ds.Tables[0].Rows[0].Delete();
                    ds.Tables[0].AcceptChanges();
                }
            return ds;
        }

        //Masters file Path
        private string mGetSetFilePath
        {
            get
            {
                return ViewBag.FilePath as string;
            }
            set
            {
                if (ViewBag.FilePath == null)
                {
                    ViewBag.FilePath = StrFilePath;
                }
            }
        }
        //Masters file Name
        private string mGetSetFileName
        {
            get
            {
                return ViewBag.FileName as string;
            }
            set
            {
                if (ViewBag.FileName == null)
                {
                    ViewBag.FileName = StrFileName;
                }
            }
        }

        /// <summary>
        /// Conversion of SQL Database in to Excel sample file
        /// </summary>
        /// <param name="xlstable"></param>
        /// <param name="ExportFileName"></param>
        /// <returns></returns>
        public bool ExportDatasetToExcel(DataSet xlstable, string ExportFileName)
        {

            try
            {
                string xlsFileName = ExportFileName;
                if ((xlstable != null) & xlstable.Tables[0].Rows.Count > 0)
                {
                    DataTable dt = xlstable.Tables[0];
                    // dt.TableName = "SecInfoData";
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt);
                        Response.Clear();
                        Response.Buffer = true;
                        Response.Charset = "";
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.AddHeader("content-disposition", "attachment;filename=" + xlsFileName);
                        using (MemoryStream MyMemoryStream = new MemoryStream())
                        {
                            wb.SaveAs(MyMemoryStream);
                            MyMemoryStream.WriteTo(Response.OutputStream);
                            Response.Flush();
                            Response.End();
                        }
                    }

                    //ClientScript.RegisterStartupScript(GetType(), "myScript",
                    //                                 "<script>ShowMessage('Sample Master excel file has been downloaded successfully.');</script>");

                }

            }

            catch
            {

            }
            return true;
        }

        private DataTable CreateDataTable()
        {
            DataTable dt = new DataTable();

            #region Product Master
            if (selectedMasterEnum == (int)AspectEnums.enumExcelType.ProductMaster)
            {
                ParkingProductMasterBO objDemoValidation = new ParkingProductMasterBO();
                foreach (var prop in objDemoValidation.GetType().GetProperties())
                {
                    if (!prop.PropertyType.FullName.ToLower().Contains("nullable"))
                    {
                        // if ((prop.Name.ToLower() == "SkuCode")) continue;
                        DataColumn dc1 = new DataColumn(prop.Name, prop.PropertyType);
                        dc1.AllowDBNull = false;
                        dt.Columns.Add(dc1);
                    }
                    else
                    {
                        DataColumn dc1 = new DataColumn(prop.Name);
                        dc1.AllowDBNull = true;
                        dt.Columns.Add(dc1);
                    }
                }
            }
            #endregion

            #region Category Master
            else if (selectedMasterEnum == (int)AspectEnums.enumExcelType.CategoryMaster)
            {
                CategoryMasterBO objCategoryMasterBO = new CategoryMasterBO();
                foreach (var prop in objCategoryMasterBO.GetType().GetProperties())
                {
                    if (!prop.PropertyType.FullName.ToLower().Contains("nullable"))
                    {
                        //if ((prop.Name.ToLower() == "SkuCode")) continue;
                        DataColumn dc1 = new DataColumn(prop.Name, prop.PropertyType);
                        dc1.AllowDBNull = false;
                        dt.Columns.Add(dc1);
                    }
                    else
                    {
                        DataColumn dc1 = new DataColumn(prop.Name);
                        dc1.AllowDBNull = true;
                        dt.Columns.Add(dc1);
                    }
                }
            }
            #endregion

            #region Product Images
            else if (selectedMasterEnum == (int)AspectEnums.enumExcelType.ProductImages)
            {
                ProductImagesBO objProductImagesBO = new ProductImagesBO();
                foreach (var prop in objProductImagesBO.GetType().GetProperties())
                {
                    if (!prop.PropertyType.FullName.ToLower().Contains("nullable"))
                    {
                        //if ((prop.Name.ToLower() == "SkuCode")) continue;
                        DataColumn dc1 = new DataColumn(prop.Name, prop.PropertyType);
                        dc1.AllowDBNull = false;
                        dt.Columns.Add(dc1);
                    }
                    else
                    {
                        DataColumn dc1 = new DataColumn(prop.Name);
                        dc1.AllowDBNull = true;
                        dt.Columns.Add(dc1);
                    }
                }
            }
            #endregion

            #region Product Specification
            else if (selectedMasterEnum == (int)AspectEnums.enumExcelType.ProdSpecification)
            {
                ProductSepecificationsBO objParkingUserMasterBO = new ProductSepecificationsBO();
                foreach (var prop in objParkingUserMasterBO.GetType().GetProperties())
                {
                    if (!prop.PropertyType.FullName.ToLower().Contains("nullable"))
                    {
                        //if ((prop.Name.ToLower() == "SkuCode")) continue;
                        DataColumn dc1 = new DataColumn(prop.Name, prop.PropertyType);
                        dc1.AllowDBNull = false;
                        dt.Columns.Add(dc1);
                    }
                    else
                    {
                        DataColumn dc1 = new DataColumn(prop.Name);
                        dc1.AllowDBNull = true;
                        dt.Columns.Add(dc1);
                    }
                }
            }
            #endregion

            return dt;
        }

        private DataTable ProcessExcelData(DataSet ds)
        {
            DataTable dt = CreateDataTable();
            try
            {
                DateTime CurrentDate = DateTime.Now;

                #region Upload Product Master

                if (selectedMasterEnum == (int)AspectEnums.enumExcelType.ProductMaster)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DataRow dr1 = dt.NewRow();

                        dr1["ProductID"] = 1;
                        dr1["ProductName"] = Convert.ToString(dr["ProducTName"]);
                        dr1["MeasureUnit"] = Convert.ToString(dr["MeasureUnit"]);
                        dr1["Availability"] = Convert.ToString(dr["Availability"]);
                        dr1["Product_Description"] = Convert.ToString(dr["Product_Description"]);
                        dr1["CategoryID"] = Convert.ToString(dr["CategoryID"]);
                        dr1["SKUCode"] = Convert.ToString(dr["SKUCode"]);
                        dr1["SKUName"] = Convert.ToString(dr["SKUName"]);
                        dr1["Brand"] = Convert.ToString(dr["Brand"]);
                        dr1["Color"] = Convert.ToString(dr["Color"]);
                        dr1["Size"] = Convert.ToString(dr["Size"]);
                        dr1["ImagePath"] = Convert.ToString(dr["ImagePath"]);
                        dr1["UnitPrice"] = Convert.ToDecimal(dr["UnitPrice"]);
                        dr1["Status"] = Convert.ToString(dr["Status"]);
                        dr1["DateCreated"] = CurrentDate;
                        dr1["CreatedBy"] = (int)HttpContext.Session[PageConstants.SESSION_USER_ID];
                        dr1["DateModified"] = CurrentDate;
                        dr1["ModifiedBy"] = (int)HttpContext.Session[PageConstants.SESSION_USER_ID];

                        if (dr["IsDeleted"].ToString().ToLower().Equals("yes") || dr["IsDeleted"].ToString().ToLower().Equals("Y"))
                        {
                            dr1["IsDeleted"] = true;
                        }
                        else
                        {
                            dr1["IsDeleted"] = false;
                        }


                        dt.Rows.Add(dr1);
                    }
                }
                #endregion

                #region Upload Category Master

                else if (selectedMasterEnum == (int)AspectEnums.enumExcelType.CategoryMaster)
                {
                    //Parking User Master Code goes here Confirmed from Neeraj
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DataRow dr1 = dt.NewRow();
                        dr1["CategoryID"] = 0;

                        dr1["CategoryName"] = Convert.ToString(dr["CategoryName"]);
                        dr1["CategoryCode"] = Convert.ToString(dr["CategoryCode"]);
                        dr1["Description"] = Convert.ToString(dr["Description"]);
                        dr1["Picture"] = Convert.ToString(dr["ImageUrl"]);
                        dr1["ParentCatgID"] = Convert.ToInt32(dr["ParentCatgID"]);
                        dr1["Created_By"] = UserID;
                        dr1["Created_Date"] = CurrentDate;
                        dr1["Modified_Date"] = CurrentDate;
                        dr1["Modified_By"] = UserID;
                        dr1["ISDeleted"] = false;
                        //if (dr["ISDeleted"].ToString().ToLower().Contains("y"))
                        //{
                        //    dr1["ISDeleted"] = true;
                        //}
                        //else
                        //{
                        //    dr1["ISDeleted"] = false;
                        //}

                        dt.Rows.Add(dr1);
                    }

                }

                #endregion

                #region Upload Product Images

                else if (selectedMasterEnum == (int)AspectEnums.enumExcelType.ProductImages)
                {
                    //Parking User Master Code goes here Confirmed from Neeraj
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DataRow dr1 = dt.NewRow();
                        dr1["ProductImageID"] = 0;
                        dr1["Name"] = Convert.ToString(dr["Name"]);
                        dr1["Caption"] = Convert.ToString(dr["Caption"]);
                        dr1["ImageUrl"] = Convert.ToString(dr["ImageUrl"]);
                        dr1["SKUCode"] = "SKUCode";
                        dr1["CategoryID"] = Convert.ToInt32(dr["CategoryID"]);
                        dr1["CreatedBy"] = UserID;
                        dr1["CreatedDate"] = CurrentDate;
                        dr1["ModifiedDate"] = CurrentDate;
                        dr1["ModifiedBy"] = UserID;

                        if (dr["IsPrimeImage"].ToString().ToLower().Contains("y"))
                        {
                            dr1["IsPrimeImage"] = true;
                        }
                        else
                        {
                            dr1["IsPrimeImage"] = false;
                        }

                        dr1["ISDeleted"] = false;

                        //if (dr["ISDeleted"].ToString().ToLower().Contains("y"))
                        //{
                        //    dr1["ISDeleted"] = true;
                        //}
                        //else
                        //{
                        //    dr1["ISDeleted"] = false;
                        //}

                        dt.Rows.Add(dr1);
                    }

                }

                #endregion

                #region Upload Product Specifications

                else if (selectedMasterEnum == (int)AspectEnums.enumExcelType.ProdSpecification)
                {
                    //Parking User Master Code goes here Confirmed from Neeraj
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        DataRow dr1 = dt.NewRow();
                        dr1["ProductSpecificID"] = 0;
                        dr1["Header"] = Convert.ToString(dr["Header"]);
                        dr1["HeaderContent"] = Convert.ToString(dr["HeaderContent"]);
                        dr1["SKUCode"] = Convert.ToString(dr["SKUCode"]);
                        dr1["CategoryID"] = Convert.ToInt32(dr["CategoryID"]);
                        dr1["CreatedBy"] = UserID;
                        dr1["CreatedDate"] = CurrentDate;
                        dr1["ModifiedDate"] = CurrentDate;
                        dr1["ModifiedBy"] = UserID;

                        if (dr["IsPrimeHeader"].ToString().ToLower().Contains("y"))
                        {
                            dr1["IsPrimeHeader"] = true;
                        }
                        else
                        {
                            dr1["IsPrimeHeader"] = false;
                        }

                        dr1["IsDeleted"] = false;

                        //if (dr["IsDeleted"].ToString().ToLower().Contains("y"))
                        //{
                        //    dr1["IsDeleted"] = true;
                        //}
                        //else
                        //{
                        //    dr1["IsDeleted"] = false;
                        //}

                        dt.Rows.Add(dr1);
                    }

                }

                #endregion

            }
            catch (Exception ex)
            {
                ViewBag.ShowPopup = true;
                isError = true;
                @ViewBag.ErrMessage = ex.Message; // "Something went wrong. Please contact administrator.";
                DisplayErrorMessage(AppConstants.uploadMasterError);
            }
            return dt;
        }

        public ActionResult DownloadSampleXls(DropDown model, string ddlSelectedMasterText, int myid = 0)
        {
            try
            {
                ExceptionEngine.ProcessAction(() =>
                {
                    String fileName = "";
                    DataSet _Ds = new DataSet();
                    DataTable _Dt = new DataTable();
                    DataColumn _Dc = new DataColumn();
                    // _Dt.Columns.Add("S.No.");

                    # region add columns in sample excel file during download

                    if (myid == (int)AspectEnums.enumExcelType.ProductMaster)
                    {
                        foreach (var columnName in arrProductMasterExcelColumn)
                        {
                            _Dt.Columns.Add(columnName);
                        }
                        fileName = "ProductMaster_Sample.xls";
                    }
                    else if (myid == (int)AspectEnums.enumExcelType.CategoryMaster)
                    {
                        foreach (var columnName in arrCategoryMasterExcelColumn)
                        {
                            _Dt.Columns.Add(columnName);
                        }
                        fileName = "CategoryMaster_Sample.xls";
                    }
                    else if (myid == (int)AspectEnums.enumExcelType.ProductImages)
                    {
                        foreach (var columnName in arrProductImagesExcelColumn)
                        {
                            _Dt.Columns.Add(columnName);
                        }
                        fileName = "ProductImages_Sample.xls";
                    }
                    else if (myid == (int)AspectEnums.enumExcelType.ProdSpecification)
                    {
                        foreach (var columnName in arrProductSpecificsExcelColumn)
                        {
                            _Dt.Columns.Add(columnName);
                        }
                        fileName = "ProductSpecifications_Sample.xls";
                    }

                    #endregion


                    DataRow _Dr = _Dt.NewRow();
                    _Dt.Rows.Add(_Dr);
                    _Ds.Tables.Add(_Dt);
                    ExportDatasetToExcel(_Ds, fileName);

                }, AspectEnums.ExceptionPolicyName.AssistingAdministrators.ToString());



            }
            catch
            {

            }
            return View();

        }
        private DataSet mGetSetExcelDataSet
        {
            get
            {
                return Session["ExcelData"] as DataSet;
            }
            set
            {
                if (Session["ExcelData"] == null)
                    Session["ExcelData"] = ds;
            }
        }

        #endregion

        //Manage Master Roles and User Roles
        #region Create View and Update Master Roles

        [HttpGet]
        public ActionResult RoleManager()
        {
            RoleManagerBO manager = new RoleManagerBO();
            manager.roleMasterBO = new RoleMasterBO();
            manager.lstRoleMaster = EmpBusinessInstance.GetRoleMaster().ToList();
            //(int)AspectEnums.PermissionEnum.View;

            return View(manager);
        }

        [HttpPost]
        public ActionResult RoleManager(RoleManagerBO model)
        {
            RoleMasterBO rm = model.roleMasterBO;

            return View();
        }

        [HttpPost]
        public ActionResult UpdateRole(RoleMasterBO model)
        {
            return View();
        }

        #endregion

        #region Assign and UnAssign Modules to a Role

        [HttpGet]
        public ActionResult ManageRoleModules(int roleID = 0)
        {
            if (roleID == 0)
                roleID = USERRoleID;

            MVM.ListRoles = DD.GetAllRolesList();

            MVM.ListAssignedModules = DD.GetAssignedModulesList(roleID);
            MVM.ListUnAssignedModules = DD.GetUnAssignedModulesList(roleID);

            return View(MVM);
        }


        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetAssignedModules(int roleID)
        {
            MVM.ListAssignedModules = DD.GetAssignedModulesList(roleID);
            return Json(MVM.ListAssignedModules, JsonRequestBehavior.AllowGet);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult GetUnAssignedModules(int roleID)
        {
            MVM.ListUnAssignedModules = DD.GetUnAssignedModulesList(roleID);
            return Json(MVM.ListUnAssignedModules, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult ManageRoleModules(RoleModulesViewModel MVM)
        {
            List<RoleModuleBO> listNewRoleModules = new List<RoleModuleBO>();
            RoleModuleBO roleModule = new RoleModuleBO();
            bool isSuccess = false;
            int rolemodulecount = 0;
            int roleid = MVM.RoleID;
            var NewAssignedModulesID = MVM.selectedAssignedModules;
            var UnAssignedModulesID = MVM.selectedUnassignedModules;
            #region Delete existing unassigned Modules
            if (UnAssignedModulesID != null)
            {
                foreach (var item in UnAssignedModulesID)
                {
                    rolemodulecount = GetRoleModules(roleid, Convert.ToInt32(item)).Count();
                    if (rolemodulecount > 0)
                    {

                        roleModule = GetRoleModules(roleid, Convert.ToInt32(item)).First();
                        if (roleModule.IsDeleted == false)
                        {
                            roleModule.IsDeleted = true;
                            isSuccess = SystemBusinessInstance.DeleteRoleModuleByRoleModule(roleModule);
                        }
                    }
                }
            }
            #endregion

            #region Add New assigned Module to a Role
            if (NewAssignedModulesID != null)
            {
                foreach (var moduleid in NewAssignedModulesID)
                {
                    rolemodulecount = GetRoleModules(roleid, Convert.ToInt32(moduleid)).Count();
                    if (rolemodulecount > 0)
                    {
                        roleModule = GetRoleModules(roleid, Convert.ToInt32(moduleid)).First();
                    }
                    if (rolemodulecount == 0)
                    {
                        // if 0, means assigned new module to this role
                        roleModule.RoleID = MVM.RoleID;
                        roleModule.ModuleID = Convert.ToInt32(moduleid);
                        roleModule.CreatedDate = DateTime.Now;
                        roleModule.CreatedBy = UserID;
                        roleModule.Sequence = NewAssignedModulesID.Count();
                        roleModule.IsMandatory = false;
                        roleModule.IsDeleted = false;
                        listNewRoleModules.Add(roleModule);
                        isSuccess = SystemBusinessInstance.InsertRoleModules(listNewRoleModules);
                    }
                    if (roleModule.IsDeleted == true)
                    {

                        roleModule.IsDeleted = false;  // updating to undelete
                        isSuccess = SystemBusinessInstance.DeleteRoleModuleByRoleModule(roleModule);
                    }
                }

                roleModule = new RoleModuleBO();
            }


            #endregion

            if (isSuccess)
            {
                ViewBag.Message = "New Modules Assigned Successfully.";
                ViewBag.ShowPopup = true;
            }
            else
            {
                ViewBag.Message = "Failed to assign new module/s. Please try again later.";
                ViewBag.ShowPopup = true;
            }
            MVM.ListRoles = DD.GetAllRolesList();
            MVM.ListAssignedModules = DD.GetAssignedModulesList(MVM.RoleID);
            MVM.ListUnAssignedModules = DD.GetUnAssignedModulesList(MVM.RoleID);
            return View(MVM);

        }
        #endregion
        //Create Employees
        #region Create and View Employees


        public ActionResult GetEmployeeInfo(int empID)
        {
            UserProfileBO emp = new UserProfileBO();
            emp = EmpBusinessInstance.DisplayUserProfile(empID);

            return View(emp);
        }

        #endregion

        #region Modules Manager

        public ActionResult ModulesManager(int? ModuleID, bool? isDeleteReq)
        {

            ModulesViewModel manager = new ModulesViewModel();
            manager.moduleMasterBO = new ModuleMasterBO();
            if (ModuleID != null || isDeleteReq != null)
            {
                var ModulesList = SystemBusinessInstance.GetModuleList().ToList();
                var Module = ModulesList.Where(x => x.ModuleID == ModuleID && x.IsDeleted == false).First();

                manager.ListParentModules = DD.GetParentModuleNamesList(ModulesList);
                //Assign new module code
                int? maxModuleCode = SystemBusinessInstance.GetModuleList().Max(k => k.ModuleCode) + 1;
                manager.moduleMasterBO = Module;

                manager.ListModuleMaster = ModulesList;

                ViewBag.ShowUpdatePopup = "Y";
            }
            else
            {
                var ModulesList = SystemBusinessInstance.GetModuleList().Distinct().ToList();
                manager.ListParentModules = DD.GetParentModuleNamesList(ModulesList);
                //Assign new module code
                int? maxModuleCode = SystemBusinessInstance.GetModuleList().Max(k => k.ModuleCode) + 1;
                manager.moduleMasterBO.ModuleCode = Convert.ToInt32(maxModuleCode);
                foreach (var item in ModulesList)
                {
                    if (item.ParentModuleID != null)
                    {
                        var parentList = ModulesList.Where(k => k.ModuleID == item.ParentModuleID).FirstOrDefault();
                        if (parentList != null)
                            item.ParentName = parentList.Name;
                    }
                    else
                    {
                        item.ParentName = string.Empty;
                    }
                }
                manager.ListModuleMaster = ModulesList;
            }



            return View(manager);
        }

        [HttpPost]
        public ActionResult ModulesManager(ModulesViewModel MVM, int moduleID, bool isupdated)
        {

            if (SystemBusinessInstance.GetModuleList().Select(x => x.Name).First() == MVM.moduleMasterBO.Name)
            {
                ViewBag.Message = "This Module Name already exists.";
                ViewBag.ShowPopup = true;
            }
            else
            {
                bool isSuccess = false;
                //Create New Module
                ModuleMasterBO newModule = MVM.moduleMasterBO;
                // MVM.moduleMasterBO.ModuleCode = Convert.ToInt32(collection["NewModuleCode"]);
                string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
                string controllerName = this.ControllerContext.RouteData.Values["controller"].ToString();
                newModule.PageURL = "~/" + controllerName + "/" + actionName + "/";
                newModule.Icon = "NA";
                isSuccess = SystemBusinessInstance.IsModuleInsert(MVM.moduleMasterBO);

                var ModulesList = SystemBusinessInstance.GetModuleList().ToList();
                MVM.ListParentModules = DD.GetParentModuleNamesList(ModulesList);
                MVM.ListModuleMaster = ModulesList;
                if (isSuccess == true)
                {
                    ViewBag.Message = "New Module Created Successfully.";
                    ViewBag.ShowPopup = true;
                    MVM.moduleMasterBO = new ModuleMasterBO();
                    MVM.moduleMasterBO.ModuleCode = SystemBusinessInstance.GetModuleList().Max(k => k.ModuleCode) + 1;
                    return View(MVM);
                }
                else
                {
                    ViewBag.ErrMessage = "Something went wrong. Please try again.";
                    ViewBag.ShowPopup = true;
                }

            }

            return View(MVM);

        }

        public ActionResult UpdateModules(int ModuleID, bool isDeleteReq)
        {
            ModulesViewModel manager = new ModulesViewModel();
            manager.moduleMasterBO = new ModuleMasterBO();
            var ModulesList = SystemBusinessInstance.GetModuleList().ToList();
            var Module = ModulesList.Where(x => x.ModuleID == ModuleID && x.IsDeleted == false).First();

            manager.ListParentModules = DD.GetParentModuleNamesList(ModulesList);
            //Assign new module code
            int? maxModuleCode = SystemBusinessInstance.GetModuleList().Max(k => k.ModuleCode) + 1;
            manager.moduleMasterBO = Module;

            manager.ListModuleMaster = ModulesList;

            ViewBag.ShowUpdatePopup = "Y";
            return View("ModulesManager", manager);
        }
        #endregion

        #region Manager Permissions of Roles and Modules for User/Employee

        public ActionResult ManageRolePermissions(int RoleID = 0)
        {
            if (RoleID == 0)
                RoleID = USERRoleID;
            RolePermissionsViewModel RPMV = new RolePermissionsViewModel();
            RoleModuleBO currentRoleModule = new RoleModuleBO();
            currentRoleModule = GetRoleModules(RoleID, null).Where(x => x.IsDeleted == false).First();
            List<UserRoleModulePermissionBO> userRoleModulePermissions = new List<UserRoleModulePermissionBO>();
            RPMV.ListRoles = DD.GetAllRolesList();
            RPMV.ListModules = DD.GetAssignedModulesList(RoleID);
            //List<ModuleMasterBO> ListModuleMaster = SystemBusinessInstance.GetModulesListByRole(RoleID).ToList();

            var hasCreateAccess = userRoleModulePermissions.Where(x => x.PermissionID == (int)AspectEnums.RolePermissionEnums.CRUD && x.RoleModuleID == currentRoleModule.RoleModuleID).SingleOrDefault();
            var hasDeleteAccess = userRoleModulePermissions.Where(x => x.PermissionID == (int)AspectEnums.RolePermissionEnums.Delete && x.RoleModuleID == currentRoleModule.RoleModuleID).SingleOrDefault();
            var hasUpdateAccess = userRoleModulePermissions.Where(x => x.PermissionID == (int)AspectEnums.RolePermissionEnums.Update && x.RoleModuleID == currentRoleModule.RoleModuleID).SingleOrDefault();
            var hasUploadAccess = userRoleModulePermissions.Where(x => x.PermissionID == (int)AspectEnums.RolePermissionEnums.Upload && x.RoleModuleID == currentRoleModule.RoleModuleID).SingleOrDefault();
            var hasEmailAccess = userRoleModulePermissions.Where(x => x.PermissionID == (int)AspectEnums.RolePermissionEnums.Email && x.RoleModuleID == currentRoleModule.RoleModuleID).SingleOrDefault();

            return View(RPMV);
        }

        [HttpPost]
        public ActionResult ManageRolePermissions(RolePermissionsViewModel model)
        {
            bool status;


            return View();
        }

        #endregion


        #region Private and void methods

        private void LoadModuleMasters(int RoleID)
        {

            List<ModuleMasterBO> modulesBO = new List<ModuleMasterBO>();
            var roleModules = GetRoleModules(RoleID, null).Where(x => x.IsDeleted == false).ToList();
            modulesBO = FillModules().Where(m => m.ModuleType == Convert.ToInt32(AspectEnums.ModuleTypeEnum.DashBoard)).ToList();

            var AssignedModules = (from roleModule in roleModules
                                   join module in modulesBO on roleModule.ModuleID equals module.ModuleID
                                   select module).ToList();

            var UnAssignedModules = modulesBO.Except(AssignedModules);
        }

        private void LoadWebDashboard(int RoleID)
        {
            List<ModuleMasterBO> LstFillCpl = new List<ModuleMasterBO>();
            List<int> Dshbrd = new List<int>();
            List<RoleModuleBO> Fn = new List<RoleModuleBO>();
            var roleModules = GetRoleModules(RoleID, null).ToList();
            //userRoleModulePermissions = GetUserRoleModulePermissions(roleModules);

            modulesBO = FillModules().Where(m => m.ModuleType == Convert.ToInt32(AspectEnums.ModuleTypeEnum.ControlPanel)).ToList();


            var AssignedModules = (from roleModule in roleModules
                                   join module in modulesBO on roleModule.ModuleID equals module.ModuleID
                                   select module).ToList();

            var UnAssignedModules = modulesBO.Except(AssignedModules).ToList(); ;


        }

        private List<RoleModuleBO> GetRoleModules(int RoleID, int? ModuleID)
        {
            return SystemBusinessInstance.GetRoleModulesByRoleID(RoleID, ModuleID);
        }
        private List<RoleMasterBO> GetRoleMaster()
        {
            return SystemBusinessInstance.GetRoleMasters();
        }
        private List<ModuleMasterBO> FillModules()
        {
            return SystemBusinessInstance.GetModuleList().ToList();
        }

        private void DisplayErrorMessage(string message)
        {
            ExceptionEngine.ProcessAction(() =>
            {
                //ClientScript.RegisterStartupScript(GetType(), "myScript", "<script>ShowMessage('" + message + "');</script>");
            }, AspectEnums.ExceptionPolicyName.AssistingAdministrators.ToString());
        }

        #endregion

    }
}
