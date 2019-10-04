using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net;
using System.IO;
using System.Text;

namespace ServiceUnitTest
{
    [TestClass]
    public class SecurityIDORTest
    {
        private static string _URL = "http://localhost/smartdostservice/smartdost.svc/";
        private static WebHeaderCollection headers = new WebHeaderCollection();
        private static long _userID = 1903;

        public static long UserID
        {
            get { return SecurityIDORTest._userID; }
            set { SecurityIDORTest._userID = value; }
        }

        public static WebHeaderCollection Headers
        {
            get { return headers; }
            set { headers = value; }
        }
        private static string requestbody;

        public static string Requestbody
        {
            get { return requestbody; }
            set { requestbody = value; }
        }
        public static string URL
        {
            get { return _URL; }
            set { _URL = value; }
        }


        #region Common Functions
        private static void KeyTokenGenerator()
        {
            Headers.Clear();
            Headers.Add("APIKey", "dGLTEbZMLV");
            Headers.Add("APIToken", "acffb3e7");
            Headers.Add("UserID", UserID.ToString());
        }

        private static string SendHttpRequest(WebHeaderCollection headers, string body, string servicename)
        {
            try
            {
                HttpWebRequest req = WebRequest.Create(URL + servicename) as HttpWebRequest;

                req.Method = "POST";
                // Create POST data and convert it to a byte array.
                string postData = body;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                req.Headers = headers;
                req.ContentType = "Application/json";
                // Set the ContentLength property of the WebRequest.
                req.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = req.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
                // Get the response.
                WebResponse wr = req.GetResponse();

                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(wr.GetResponseStream());
                // Read the content.
                string responseFromServer = reader.ReadToEnd();

                reader.Close();

                wr.Close();
                return responseFromServer;
            }
            catch (WebException wex)
            {
                var pageContent = new StreamReader(wex.Response.GetResponseStream())
                                      .ReadToEnd();
                return pageContent;
            }
        }
        #endregion

        #region GetUserModules
        [TestMethod]
        public void GetUserModules_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID) + ",\"RoleID\":24}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetUserModules");

            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));


        }
        [TestMethod]
        public void GetUserModules_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID + 1) + ",\"RoleID\":24}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetUserModules");

            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));


        }
        #endregion

        #region IsUserHasAttendance
        [TestMethod]
        public void IsUserHasAttendance_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID)+"}";
            //Act
            var response = SendHttpRequest(Headers, requestbody, "IsUserHasAttendance");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
         }

        [TestMethod]
        public void IsUserHasAttendance_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID+1)+"}";
            //Act
            var response = SendHttpRequest(Headers, requestbody, "IsUserHasAttendance");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region OfficeTrainingDataInfo
        [TestMethod]
        public void OfficeTrainingDataInfo_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":"+(UserID)+",\"trainingDetails\":HelloTest}";
           // Act
            var response = SendHttpRequest(Headers, requestbody, "OfficeTrainingDataInfo");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }

        [TestMethod]
        public void OfficeTrainingDataInfo_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":" + (UserID+1) +"}";
            // Act
            var response = SendHttpRequest(Headers, requestbody, "OfficeTrainingDataInfo");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  PartnerList
        [TestMethod]
        public void PartnerList_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID) + "}";
            //Act
            var response = SendHttpRequest(Headers, requestbody, "PartnerList");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }

        [TestMethod]
        public void PartnerList_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID + 1) + "}";
            //Act
            var response = SendHttpRequest(Headers, requestbody, "PartnerList");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  PartnerDetails

        [TestMethod]
        public void PartnerDetails_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":"+(UserID)+",\"PartnerID\":2,\"shipToCode\": 1234}";
            //Act
            var response = SendHttpRequest(Headers, requestbody, "PartnerDetails");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }

        [TestMethod]
        public void PartnerDetails_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID+1) + ",\"PartnerID\":2,\"shipToCode\": 1234}";
            //Act
            var response = SendHttpRequest(Headers, requestbody, "PartnerDetails");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region DealersListBasedOnCity
        [TestMethod]
        public void DealersListBasedOnCity_IsHeaderBodyMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"roleID\":24,\"userID\":"+(UserID)+"}";

            //Act
            var response = SendHttpRequest(headers, requestbody, "DealersListBasedOnCity");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void DealersListBasedOnCity_IsHeaderBodyIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"roleID\":24,\"userID\":" + (UserID+1) + "}";

            //Act
            var response = SendHttpRequest(headers, requestbody, "DealersListBasedOnCity");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region DisplayRaceUserProfile
        [TestMethod]
        public void DisplayRaceUserProfile_IsHeaderBodyMatched_IsTrue()
        {
            // arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":"+(UserID)+"}";
            // Act 
            var response = SendHttpRequest(headers, requestbody, "DisplayRaceUserProfile");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void DisplayRaceUserProfile_IsHeaderBodyIDNotMatched_IsTrue()
        {
            // arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":" + (UserID + 1) + "}";
            // Act 
            var response = SendHttpRequest(headers, requestbody, "DisplayRaceUserProfile");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region GetAuditDetails
        [TestMethod]
        public void GetAuditDetails_IsHeaderBodyMatched_IsTrue()
        {
            // arrange
            KeyTokenGenerator();
            requestbody = "{\"surveyResponseID\":160395,\"AuditID\":,\"UserID\":"+(UserID)+",\"RoleID\":24}";
            // Act 
            var response = SendHttpRequest(headers, requestbody, "GetAuditDetails");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetAuditDetails_IsHeaderBodyIDNotMatched_IsTrue()
        {
            // arrange
            KeyTokenGenerator();
            requestbody = "{\"surveyResponseID\":160395,\"AuditID\":4,\"UserID\":" + (UserID+1) + ",\"RoleID\":24}";
            // Act 
            var response = SendHttpRequest(headers, requestbody, "GetAuditDetails");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region GetCompetitors
        [TestMethod]
        public void GetCompetitors_IsHeaderBodyMatched_IsTrue()
        {
            // arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":"+(UserID)+"}";
            // Act 
            var response = SendHttpRequest(headers, requestbody, "GetCompetitors");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetCompetitors_IsHeaderBodyIDNotMatched_IsTrue()
        {
            // arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":" + (UserID+1) + "}";
            // Act 
            var response = SendHttpRequest(headers, requestbody, "GetCompetitors");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region GetEOLSchemes
        [TestMethod]
        public void GetEOLSchemes_IsHeaderBodyMatched_IsTrue()
        {
            // arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":" + UserID + ",\"roleID\":24}";
            // Act 
            var response = SendHttpRequest(headers, requestbody, "GetEOLSchemes");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetEOLSchemes_IsHeaderBodyIDNotMatched_IsTrue()
        {
            // arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":"+(UserID+1)+",\"roleID\":24}";
            // Act 
            var response = SendHttpRequest(headers, requestbody, "GetEOLSchemes");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region GetFeedbackDetails
        [TestMethod]
        public void GetFeedbackDetails_IsHeaderBodyMatche_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":"+UserID+",\"roleID\":24,\"FeedbackID\":10}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetFeedbackDetails");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetFeedbackDetails_IsHeaderBodyIDNotMatche_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":" + (UserID+1) + ",\"roleID\":24,\"FeedbackID\":10}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetFeedbackDetails");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetFMSMasters
        [TestMethod]
        public void GetFMSMasters_IsHeaderBodyMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":"+UserID+",\"roleID\":24,\"LastTeamID\":10,\"LastCategoryID\":10,\"LastTypeID\":10,\"rowcounter\":10}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetFeedbackDetails");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetFMSMasters_IsHeaderBodyIDNotMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":" + (UserID+1) + ",\"roleID\":24,\"LastTeamID\":10,\"LastCategoryID\":10,\"LastTypeID\":10,\"rowcounter\":10}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetFeedbackDetails");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion 

        #region  GetGeoDefinitions
        [TestMethod]
        public void GetGeoDefinitions_IsHeaderBodyMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":" + (UserID) + ",\"roleID\":24}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetGeoDefinitions");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetGeoDefinitions_IsHeaderBodyIDNotMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":"+(UserID+1)+",\"roleID\":24}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetGeoDefinitions");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetMyTerritory
        [TestMethod]
        public void GetMyTerritory_IsHeaderBodyMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":"+(UserID)+"}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetMyTerritory");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetMyTerritory_IsHeaderBodyIDNotMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":" + (UserID+1) + "}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetMyTerritory");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region GetNotifications
        [TestMethod]
        public void GetNotifications_IsHeaderBodyMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":" + (UserID) + ",\"roleID\":24,\"NotificationType\":1,\"LastNotificationServiceID\":1014,\"rowcounter\":10}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetNotifications");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetNotifications_IsHeaderBodyIDNotMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":"+(UserID+1)+",\"roleID\":24,\"NotificationType\":1,\"LastNotificationServiceID\":1014,\"rowcounter\":10}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetNotifications");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetPlanogramClassMasters
        [TestMethod]
        public void GetPlanogramClassMasters_IsHeaderBodyMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"companyID\":1,\"userID\":" + (UserID) + ",\"roleID\":24}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetPlanogramClassMasters");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetPlanogramClassMasters_IsHeaderBodyIDNotMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"companyID\":1,\"userID\":"+(UserID+1)+",\"roleID\":24}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetPlanogramClassMasters");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion
       
        #region  GetPlanogramProductMasters
        [TestMethod]
        public void GetPlanogramProductMasters_IsHeaderBodyMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"companyID\":1,\"userID\":"+UserID+",\"roleID\":24,\"PlanogramProductMasterID\":1,\"rowcounter\":10}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetPlanogramProductMasters");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetPlanogramProductMasters_IsHeaderBodyIDNotMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"companyID\":1,\"userID\":" + (UserID+1) + ",\"roleID\":24,\"PlanogramProductMasterID\":1,\"rowcounter\":10}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetPlanogramProductMasters");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetProductAuditdata
        [TestMethod]
        public void GetProductAuditdata_IsHeaderBodyMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"companyID\":1,\"userID\":" + UserID + ",\"roleID\":24,\"PlanogramProductMasterID\":1,\"rowcounter\":10}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetProductAuditdata");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetProductAuditdata_IsHeaderBodyIDNotMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"companyID\":1,\"userID\":" + (UserID+1) + ",\"roleID\":24,\"PlanogramProductMasterID\":1,\"rowcounter\":10}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetProductAuditdata");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetProductDefinitions
        [TestMethod]
        public void GetProductDefinitions_IsHeaderBodyMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"companyID\":1,\"userID\":"+UserID+"}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetProductDefinitions");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetProductDefinitions_IsHeaderBodyIDNotMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"companyID\":1,\"userID\":" + (UserID+1) + "}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetProductDefinitions");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetRaceMasters
        [TestMethod]
        public void GetRaceMasters_IsHeaderBodyMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":"+UserID+",\"roleID\":24}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetRaceMasters");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetRaceMasters_IsHeaderBodyIDNotMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":" + (UserID+1) + ",\"roleID\":24}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetRaceMasters");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetRaceProductMasters
        [TestMethod]
        public void GetRaceProductMasters_IsHeaderBodyMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":"+(UserID)+",\"roleID\":24,\"LastProductID\":12,\"rowcounter\":5}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetRaceProductMasters");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetRaceProductMasters_IsHeaderBodyIDNotMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":"+(UserID+1)+",\"roleID\":24,\"LastProductID\":12,\"rowcounter\":5}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetRaceProductMasters");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetRLSSODetails
        [TestMethod]
        public void GetRLSSODetails_IsHeaderBodyMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"soNumber\":17,\"userID\":"+UserID+"}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetRLSSODetails");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetRLSSODetails_IsHeaderBodyIDNotMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"soNumber\":17,\"userID\":" + (UserID+1)+"}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetRLSSODetails");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region GetRuleBook
        [TestMethod]
        public void GetRuleBook_IsHeaderBodyMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":" + UserID + "}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetRuleBook");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetRuleBook_IsHeaderBodyIDNotMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":" + (UserID+1) + "}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetRuleBook");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetSearchAuditdata
        [TestMethod]
        public void GetSearchAuditdata_IsHeaderBodyMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"auditSearchDTO\":{},\"userID\":"+UserID+",\"RoleID\":24}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetSearchAuditdata");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetSearchAuditdata_IsHeaderBodyIDNotMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"auditSearchDTO\":{},\"userID\":"+(UserID+1)+",\"RoleID\":24}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetSearchAuditdata");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetSurveyModulesList
        [TestMethod]
        public void GetSurveyModulesList_IsHeaderBodyMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":"+UserID+",\"surveyResponseID\":1010,\"RoleID\":24}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetSurveyModulesList");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetSurveyModulesList_IsHeaderBodyIDNotMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":" + (UserID+1) + ",\"surveyResponseID\":1010,\"RoleID\":24}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetSurveyModulesList");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetTodaySchemes
        [TestMethod]
        public void GetTodaySchemes_IsHeaderBodyMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":"+UserID+",\"userRoleID\":24}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetTodaySchemes");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetTodaySchemes_IsHeaderBodyIDNotMatched_IsTrue()
        {
            //Arrange
            KeyTokenGenerator();
            requestbody = "{\"userID\":" + (UserID + 1) + ",\"userRoleID\":24}";
            //Act
            var response = SendHttpRequest(headers, requestbody, "GetTodaySchemes");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        //#region  LogoutUser
        //[TestMethod]
        //public void LogoutUser_IsHeaderBodyMatched_IsTrue()
        //{
        //    //Arrange
        //    KeyTokenGenerator();
        //    requestbody = "{\"userID\":" + UserID + "}";
        //    //Act
        //    var response = SendHttpRequest(headers, requestbody, "LogoutUser");
        //    // Assert
        //    Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        //}
        //[TestMethod]
        //public void LogoutUser_IsHeaderBodyIDNotMatched_IsTrue()
        //{
        //    //Arrange
        //    KeyTokenGenerator();
        //    requestbody = "{\"userID\":" + (UserID + 1) + "}";
        //    //Act
        //    var response = SendHttpRequest(headers, requestbody, "LogoutUser");
        //    // Assert
        //    Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        //}
        //#endregion

        #region  NotificationTypeMaster
        [TestMethod]
        public void NotificationTypeMaster_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID) + ",\"RoleID\":24}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "NotificationTypeMaster");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void NotificationTypeMaster_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID + 1) + ",\"RoleID\":24}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "NotificationTypeMaster");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  SaveStoreSurveyResponses
        [TestMethod]
        public void SaveStoreSurveyResponses_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"storeSurvey\":[{\"SurveyResponseID\":166,\"UserID\":" + (UserID) + ",\"StoreID\":663378,\"ModuleID\":9813,\"Comments\":\"ghkj\",\"PictureFileName\":\"gf\",\"Lattitude\":\"ff\",\"Longitude\":\"dfs\",\"IsOffline\":true,\"ModuleCode\":1,\"CoverageID\":123,\"UserDeviceID\":234,\"UserOption\":true,\"RaceProfile\":true,\"strAssesmentStartTime\":\"28-May-2015\",\"strAssesmentEndTime\":\"28-May-2015\",}]}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SaveStoreSurveyResponses");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void SaveStoreSurveyResponses_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"storeSurvey\":[{\"SurveyResponseID\":166,\"UserID\":" + (UserID + 1) + ",\"StoreID\":663378,\"ModuleID\":9813,\"Comments\":\"ghkj\",\"PictureFileName\":\"gf\",\"Lattitude\":\"ff\",\"Longitude\":\"dfs\",\"IsOffline\":true,\"ModuleCode\":1,\"CoverageID\":123,\"UserDeviceID\":234,\"UserOption\":true,\"RaceProfile\":true,\"strAssesmentStartTime\":\"28-May-2015\",\"strAssesmentEndTime\":\"28-May-2015\",}]}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SaveStoreSurveyResponses");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region SearchFeedbacks
        [TestMethod]
        public void SearchFeedbacks_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"searchFeedBacks\":{\"FeedbackTeamIDs\":\"34\",\"FeedbackCatIDs\":\"r\",\"FeedbackTypeIDs\":\"Present\",\"LastFeedbackID\":2344, \"Rowcounter\":34,\"PendingWithType\":1,\"StatusIDs\":\"1\",\"DateFrom\":\"28-May-2015\",\"DateTo\":\"28-May-2015\"},{\"storeID\":2,\"userID\":"+UserID+"}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SearchFeedbacks");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void SearchFeedbacks_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"searchFeedBacks\":{\"FeedbackTeamIDs\":\"34\",\"FeedbackCatIDs\":\"r\",\"FeedbackTypeIDs\":\"Present\",\"LastFeedbackID\":2344, \"Rowcounter\":34,\"PendingWithType\":1,\"StatusIDs\":\"1\",\"DateFrom\":\"28-May-2015\",\"DateTo\":\"28-May-2015\"},{\"storeID\":2,\"userID\":"+(UserID+1)+"}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SearchFeedbacks");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  SearchFeedbackStatusCount
        [TestMethod]
        public void SearchFeedbackStatusCount_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"feedbackCountSearch\":{\"FeedbackTeamIDs\":\"123\",\"FeedbackCatIDs\":\"34\",\"FeedbackTypeIDs\":\"ew\"},{\"userID\":"+UserID+",\"roleID\":24}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SearchFeedbackStatusCount");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void SearchFeedbackStatusCount_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"feedbackCountSearch\":{\"FeedbackTeamIDs\":\"123\",\"FeedbackCatIDs\":\"34\",\"FeedbackTypeIDs\":\"ew\"},{\"userID\":" + (UserID+1) + ",\"roleID\":24}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SearchFeedbackStatusCount");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  SubmitAuditResponse Need to pass DTO in parameter
        [TestMethod]
        public void SubmitAuditResponse_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":"+UserID+",\"roleID\":24,\"SurveyResponseID\":12345}";
            //requestbody = "{\"userID\":1903,\"roleID\":24,\"SurveyResponseID\":12345,\"auditResponse\":{}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SubmitAuditResponse");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void SubmitAuditResponse_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID+1) + ",\"roleID\":24,\"SurveyResponseID\":12345}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SubmitAuditResponse");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  SubmitEOLOrder
        [TestMethod]
        public void SubmitEOLOrder_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"eolOrders\":{\"EOLOrderID\":12,\"SchemeID\":1,\"BasicModelCode\":\"345\",\"OrderQuantity\":2,\"ActualSupport\":34,\"CreatedDateTime\":\"12-May-2014\",\"StoreID\":123,\"StoreName\":\"TestStore\"},{\"userID\":"+UserID+"}}";
            //requestbody = "{\"userID\":1903,\"roleID\":24,\"SurveyResponseID\":12345,\"auditResponse\":{}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SubmitEOLOrder");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void SubmitEOLOrder_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"eolOrders\":{\"EOLOrderID\":12,\"SchemeID\":1,\"BasicModelCode\":\"345\",\"OrderQuantity\":2,\"ActualSupport\":34,\"CreatedDateTime\":\"12-May-2014\",\"StoreID\":123,\"StoreName\":\"TestStore\"},{\"userID\":" + (UserID+1) + "}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SubmitEOLOrder");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  SubmitFeedbacks
        [TestMethod]
        public void SubmitFeedbacks_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"FeedBacks\":{\"FeedbackTeamID\":1903,\"FeedbackCatID\":1,\"FeedbackTypeID\":23,\"ImageBytes\":\"283445\",\"Remarks\":\"remarksdata\",\"storeID\":1234},{\"userID\":"+UserID+"}}";
            //requestbody = "{\"userID\":1903,\"roleID\":24,\"SurveyResponseID\":12345,\"auditResponse\":{}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SubmitFeedbacks");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void SubmitFeedbacks_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"FeedBacks\":{\"FeedbackTeamID\":1903,\"FeedbackCatID\":1,\"FeedbackTypeID\":23,\"ImageBytes\":\"283445\",\"Remarks\":\"remarksdata\",\"storeID\":1234},{\"userID\":" + (UserID+1) + "}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SubmitFeedbacks");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }

        #endregion

        #region  SubmitPlanogram Need to pass DTO in parameter
        [TestMethod]
        public void SubmitPlanogram_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"PlanogramResponse\":{},{\"companyID\":1,\"userID\":"+UserID+",\"roleID\":24}}";
            //requestbody = "{\"userID\":1903,\"roleID\":24,\"SurveyResponseID\":12345,\"auditResponse\":{}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SubmitPlanogram");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void SubmitPlanogram_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"PlanogramResponse\":{},{\"companyID\":1,\"userID\":" + (UserID+1) + ",\"roleID\":24}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SubmitPlanogram");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }

        #endregion 

        #region  UpdateFeedbacks
        [TestMethod]
        public void UpdateFeedbacks_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"Feedback\":{},{\"userID\":"+UserID+",\"roleID\":24}}";
            //requestbody = "{\"userID\":1903,\"roleID\":24,\"SurveyResponseID\":12345,\"auditResponse\":{}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "UpdateFeedbacks");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void UpdateFeedbacks_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"Feedback\":{},{\"userID\":" + (UserID+1) + ",\"roleID\":24}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "UpdateFeedbacks");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion 

        #region  UpdateNotificationStatus
        [TestMethod]
        public void UpdateNotificationStatus_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":"+UserID+",\"roleID\":24,{\"Notifications\":{}}";
            
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "UpdateNotificationStatus");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void UpdateNotificationStatus_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID+1) + ",\"roleID\":24,{\"Notifications\":{}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "UpdateNotificationStatus");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  submitReviewerResponse
        [TestMethod]
        public void submitReviewerResponse_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"reviewerResponse\":{},{\"userID\":"+UserID+",\"roleID\":24}}";
            
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "submitReviewerResponse");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void submitReviewerResponse_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"reviewerResponse\":{},{\"userID\":" + (UserID+1) + ",\"roleID\":24}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "submitReviewerResponse");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  InsertUserBeatDetailsInfo
        [TestMethod]
        public void InsertUserBeatDetailsInfo_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":"+UserID+",{\"userBeatCollection\":{},\"MarketOffDays\":\"gh\",\"CoverageType\":\"jk\"}}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "InsertUserBeatDetailsInfo");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void InsertUserBeatDetailsInfo_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID+1) + ",{\"userBeatCollection\":{},\"MarketOffDays\":\"gh\",\"CoverageType\":\"jk\"}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "InsertUserBeatDetailsInfo");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetStoresForTodayBeat
        [TestMethod]
        public void GetStoresForTodayBeat_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":"+UserID+"}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetStoresForTodayBeat");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetStoresForTodayBeat_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID+1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetStoresForTodayBeat");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetUserStores
        [TestMethod]
        public void GetUserStores_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + UserID + "}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetUserStores");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetUserStores_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID + 1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetUserStores");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetOutletProfile
        [TestMethod]
        public void GetOutletProfile_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":"+UserID+",\"storeID\":1024}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetOutletProfile");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetOutletProfile_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID+1) + ",\"storeID\":1024}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetOutletProfile");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetUserSystemSettings
        [TestMethod]
        public void GetUserSystemSettings_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + UserID + "}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetUserSystemSettings");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetUserSystemSettings_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID + 1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetUserSystemSettings");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetCoverageUsers
        [TestMethod]
        public void GetCoverageUsers_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + UserID + "}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetCoverageUsers");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetCoverageUsers_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID + 1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetCoverageUsers");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetUserBeatDetails
        [TestMethod]
        public void GetUserBeatDetails_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + UserID + "}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetUserBeatDetails");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetUserBeatDetails_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID + 1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetUserBeatDetails");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region   IsCoverageFirstWindow
        [TestMethod]
        public void IsCoverageFirstWindow_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":"+UserID+",\"RoleID\":24}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "IsCoverageFirstWindow");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void IsCoverageFirstWindow_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID+1) + ",\"RoleID\":24}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "IsCoverageFirstWindow");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion 

        #region  SendPushNotification
        [TestMethod]
        public void SendPushNotification_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":"+UserID+",\"message\":\"SuccessResult\",\"registrationKey\":12345}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SendPushNotification");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void SendPushNotification_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID+1) + ",\"message\":\"SuccessResult\",\"registrationKey\":12345}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SendPushNotification");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  DisplayUserProfile
        [TestMethod]
        public void DisplayUserProfile_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + UserID + "}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "DisplayUserProfile");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void DisplayUserProfile_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID + 1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "DisplayUserProfile");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetPushNotificationData
        [TestMethod]
        public void GetPushNotificationData_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + UserID + "}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetPushNotificationData");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetPushNotificationData_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID + 1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetPushNotificationData");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetReportDashboardURL
        [TestMethod]
        public void GetReportDashboardURL_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + UserID + "}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetReportDashboardURL");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetReportDashboardURL_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID + 1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetReportDashboardURL");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetSurveyQuestionAttributes
        [TestMethod]
        public void GetSurveyQuestionAttributes_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + UserID + "}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetSurveyQuestionAttributes");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetSurveyQuestionAttributes_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID + 1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetSurveyQuestionAttributes");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  UpdatePartnerDetails
        [TestMethod]
        public void UpdatePartnerDetails_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody ="{\"userID\":"+UserID+",\"storeID\":33029,\"contactPerson\":\"Retesh Agarwal\",\"mobileNo\":\"9777477707\",\"emailID\":\"riteshagrawal@gmail.com\",\"imageName\":\"myimg\",\"storeAddress\":\"Test Address\"}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "UpdatePartnerDetails");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void UpdatePartnerDetails_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID+1) + ",\"storeID\":33029,\"contactPerson\":\"Retesh Agarwal\",\"mobileNo\":\"9777477707\",\"emailID\":\"riteshagrawal@gmail.com\",\"imageName\":\"myimg\",\"storeAddress\":\"Test Address\"}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "UpdatePartnerDetails");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  UpdatePendingCoverage
        [TestMethod]
        public void UpdatePendingCoverage_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID) + ",\"userIDList\":{},\"status\":1}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "UpdatePendingCoverage");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void UpdatePendingCoverage_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID+1) + ",\"userIDList\":{},\"status\":1}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "UpdatePendingCoverage");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region UpdateStoreProfile
        [TestMethod]
        public void UpdateStoreProfile_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + UserID + ",\"storeID\":33029,\"contactPerson\":\"Retesh Agarwal\",\"mobileNo\":\"9777477707\",\"emailID\":\"riteshagrawal@gmail.com\",\"imageName\":\"myimg\",\"storeAddress\":\"Test Address\"}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "UpdateStoreProfile");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void UpdateStoreProfile_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userID\":" + (UserID+1) + ",\"storeID\":33029,\"contactPerson\":\"Retesh Agarwal\",\"mobileNo\":\"9777477707\",\"emailID\":\"riteshagrawal@gmail.com\",\"imageName\":\"myimg\",\"storeAddress\":\"Test Address\"}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "UpdateStoreProfile");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  DisplayStoreProfile
        [TestMethod]
        public void DisplayStoreProfile_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"StoreID\":1311,\"userID\":"+UserID+"}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "DisplayStoreProfile");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void DisplayStoreProfile_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"StoreID\":1311,\"userID\":" + (UserID+1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "DisplayStoreProfile");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetSurveyQuestions
        [TestMethod]
        public void GetSurveyQuestions_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userRoleID\":24,\"userID\":"+UserID+"}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetSurveyQuestions");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetSurveyQuestions_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userRoleID\":24,\"userID\":" + (UserID+1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetSurveyQuestions");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetSurveyPartnerQuestions
        [TestMethod]
        public void GetSurveyPartnerQuestions_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userRoleID\":24,\"userID\":" + UserID + "}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetSurveyPartnerQuestions");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetSurveyPartnerQuestions_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userRoleID\":24,\"userID\":" + (UserID + 1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetSurveyPartnerQuestions");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetPendingSyncEntities
        [TestMethod]
        public void GetPendingSyncEntities_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"companyID\":1,\"userID\":"+UserID+"}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetPendingSyncEntities");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetPendingSyncEntities_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"companyID\":1,\"userID\":" + (UserID+1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetPendingSyncEntities");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  UpdateSyncEntity
        [TestMethod]
        public void UpdateSyncEntity_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"syncTableID\":1,\"userID\":"+UserID+"}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "UpdateSyncEntity");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void UpdateSyncEntity_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"syncTableID\":1,\"userID\":" + (UserID+1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "UpdateSyncEntity");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region   UpdateAndroidRegistrationId
        [TestMethod]
        public void UpdateAndroidRegistrationId_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"registrationId\":\"535\",\"userID\":"+UserID+"}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "UpdateAndroidRegistrationId");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void UpdateAndroidRegistrationId_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"registrationId\":\"535\",\"userID\":" + (UserID+1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "UpdateAndroidRegistrationId");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetCompanySystemSettings
        [TestMethod]
        public void GetCompanySystemSettings_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"companyID\":1,\"userID\":"+UserID+"}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetCompanySystemSettings");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetCompanySystemSettings_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"companyID\":1,\"userID\":" + (UserID+1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetCompanySystemSettings");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region GetCompetitionProductGroup
        [TestMethod]
        public void GetCompetitionProductGroup_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"companyID\":1,\"userID\":" + UserID + "}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetCompetitionProductGroup");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetCompetitionProductGroup_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"companyID\":1,\"userID\":" + (UserID + 1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetCompetitionProductGroup");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  GetPaymentModes
        [TestMethod]
        public void GetPaymentModes_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"companyID\":1,\"userID\":" + UserID + "}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetPaymentModes");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetPaymentModes_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"companyID\":1,\"userID\":" + (UserID + 1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetPaymentModes");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region   GetProductList
        [TestMethod]
        public void GetProductList_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"companyID\":1,\"userID\":" + UserID + "}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetProductList");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetProductList_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"companyID\":1,\"userID\":" + (UserID + 1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetProductList");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region   GetStoreSchemes
        [TestMethod]
        public void GetStoreSchemes_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"companyID\":1,\"userID\":" + UserID + "}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetStoreSchemes");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void GetStoreSchemes_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"companyID\":1,\"userID\":" + (UserID + 1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "GetStoreSchemes");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  IsGeoTagRequired
        [TestMethod]
        public void IsGeoTagRequired_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"storeID\":1014,\"userID\":"+UserID+"}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "IsGeoTagRequired");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void IsGeoTagRequired_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"storeID\":1014,\"userID\":" + (UserID+1) + "}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "IsGeoTagRequired");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
           #endregion

        #region  SaveSurveyUserResponse
        [TestMethod]
        public void SaveSurveyUserResponse_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"activities\":{}, {\"userID\":"+UserID+"}}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SaveSurveyUserResponse");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void SaveSurveyUserResponse_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"activities\":{}, {\"userID\":" + (UserID+1) + "}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SaveSurveyUserResponse");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  MarkAttendance
        [TestMethod]
        public void MarkAttendance_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userAttendance\":{\"UserID\":"+UserID+",\"IsAttendance\":1,\"Remarks\":\"Present\",\"Lattitude\":\"28.5251536\",\"Longitude\":\"77.2900144\"},{\"numberOfDays\":2}}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "MarkAttendance");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void MarkAttendance_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userAttendance\":{\"UserID\":" + (UserID+1) + ",\"IsAttendance\":1,\"Remarks\":\"Present\",\"Lattitude\":\"28.5251536\",\"Longitude\":\"77.2900144\"},{\"numberOfDays\":2}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "MarkAttendance");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  SaveGeoTag
        //[TestMethod]
        //public void SaveGeoTag_IsHeaderBodyUserIDMatched_ReturnTrue()
        //{
        //    // Arrange
        //    KeyTokenGenerator();
        //    Requestbody = "{\"storeTag\":[{\"SurveyResponseID\":166,\"UserID\":"+UserID+",\"StoreID\":663378,\"ModuleID\":9813,\"Comments\":\"TestComment\",\"PictureFileName\":\"testfile\",\"Lattitude\":\"rr\",\"Longitude\":\"hf\",\"IsOffline\":1,\"ModuleCode\":1,\"BeatPlanDate\":\"28-May-2015\",\"CoverageID\":123,\"UserDeviceID\":234,\"UserOption\":1,\"RaceProfile\":1,\"strAssesmentStartTime\":\"28-May-2015\",\"strAssesmentEndTime\":\"28-May-2015\",\"AssessmentStartTime\":\"28-May-2015\",\"AssessmentEndTime\":\"28-May-2015\"}]}";

        //    //Act
        //    var response = SendHttpRequest(Headers, Requestbody, "SaveGeoTag");
        //    // Assert
        //    Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        //}
        //[TestMethod]
        //public void SaveGeoTag_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        //{
        //    // Arrange
        //    KeyTokenGenerator();
        //    Requestbody = "{\"storeTag\":[{\"SurveyResponseID\":166,\"UserID\":" + (UserID+1) + ",\"StoreID\":663378,\"ModuleID\":9813,\"Comments\":\"TestComment\",\"PictureFileName\":\"testfile\",\"Lattitude\":\"rr\",\"Longitude\":\"hf\",\"IsOffline\":1,\"ModuleCode\":1,\"BeatPlanDate\":\"28-May-2015\",\"CoverageID\":123,\"UserDeviceID\":234,\"UserOption\":1,\"RaceProfile\":1,\"strAssesmentStartTime\":\"28-May-2015\",\"strAssesmentEndTime\":\"28-May-2015\",\"AssessmentStartTime\":\"28-May-2015\",\"AssessmentEndTime\":\"28-May-2015\"}]}";
        //    //Act
        //    var response = SendHttpRequest(Headers, Requestbody, "SaveGeoTag");
        //    // Assert
        //    Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        //}
        #endregion

        #region  SubmitPartnerMeeting
        [TestMethod]
        public void SubmitPartnerMeeting_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"partnerEntity\":{\"StoreID\":59724,\"UserID\":"+UserID+",\"UserDetailID\":23,\"Remarks\":\"test\",\"ShipToCode\":\"4257388\"}}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SubmitPartnerMeeting");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void SubmitPartnerMeeting_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"partnerEntity\":{\"StoreID\":59724,\"UserID\":"+(UserID+1)+",\"UserDetailID\":23,\"Remarks\":\"test\",\"ShipToCode\":\"4257388\"}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SubmitPartnerMeeting");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region SaveStoreSurveyResponse
        [TestMethod]
        public void SaveStoreSurveyResponse_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"storeSurvey\":[{\"SurveyResponseID\":166,\"UserID\":"+UserID+",\"StoreID\":663378,\"ModuleID\":9813,\"Comments\":\"TestComments\",\"PictureFileName\":\"TestFilename\",\"Lattitude\":\"3456\",\"Longitude\":\"789\",\"IsOffline\":true,\"ModuleCode\":1,\"CoverageID\":123,\"UserDeviceID\":234,\"UserOption\":true,\"RaceProfile\":true,\"strAssesmentStartTime\":\"28-May-2015\",\"strAssesmentEndTime\":\"28-May-2015\"}]}";

            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SaveStoreSurveyResponse");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void SaveStoreSurveyResponse_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
           // Requestbody = "{\"storeSurvey\":[{\"SurveyResponseID\":166,\"UserID\":" + (UserID + 1) + ",\"StoreID\":663378,\"ModuleID\":9813,\"Comments\":\"TestComments\",\"PictureFileName\":\"TestFilename\",\"Lattitude\":\"3456\",\"Longitude\":\"789\",\"IsOffline\":true,\"ModuleCode\":1,\"CoverageID\":123,\"UserDeviceID\":234,\"UserOption\":true,\"RaceProfile\":true,\"strAssesmentStartTime\":\"28-May-2015\",\"strAssesmentEndTime\":\"28-May-2015\"}]}";
            Requestbody = "{\"storeSurvey\":[{\"SurveyResponseID\":166,\"UserID\":" + (UserID + 1) + ",\"StoreID\":663378,\"ModuleID\":9813,\"Comments\":\"TestComments\",\"PictureFileName\":\"TestFilename\",\"Lattitude\":\"3456\",\"Longitude\":\"789\",\"IsOffline\":true,\"ModuleCode\":1,\"BeatPlanDate\":\"10-May-2015\",\"CoverageID\":123,\"UserDeviceID\":234,\"UserOption\":true,\"RaceProfile\":true,\"strAssesmentStartTime\":\"28-May-2015\",\"strAssesmentEndTime\":\"28-May-2015\"}]}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SaveStoreSurveyResponse");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region UpdateUserProfile
        [TestMethod]
        public void UpdateUserProfile_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userProfile\":{\"UserID\":"+UserID+",\"UserCode\":\"102\",\"EmplCode\":\"22\",\"FirstName\":\"test\",\"LastName\":\"test\",\"Mobile_Calling\":\"9873306227\",\"Mobile_SD\":\"4567\",\"EmailID\":\"a@gmail.com\",\"IsOfflineProfile\":0,\"ProfilePictureFileName\":\"TestPictureName\",\"Address\":\"Delhi\",\"Pincode\":\"110040\",\"AlternateEmailID\":\"test@gmail.com\",\"UserRoleID\":1,\"UserRoleIDActual\":0,\"IsRoamingProfile\":1}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "UpdateUserProfile");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void UpdateUserProfile_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"userProfile\":{\"UserID\":" + (UserID+1) + ",\"UserCode\":\"102\",\"EmplCode\":\"22\",\"FirstName\":\"test\",\"LastName\":\"test\",\"Mobile_Calling\":\"9873306227\",\"Mobile_SD\":\"4567\",\"EmailID\":\"a@gmail.com\",\"IsOfflineProfile\":0,\"ProfilePictureFileName\":\"TestPictureName\",\"Address\":\"Delhi\",\"Pincode\":\"110040\",\"AlternateEmailID\":\"test@gmail.com\",\"UserRoleID\":1,\"UserRoleIDActual\":0,\"IsRoamingProfile\":1}}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "UpdateUserProfile");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion


        #region  SubmitCompetitionBooked
        [TestMethod]
        public void SubmitCompetitionBooked_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"competitions\":[{\"CompSurveyID\":2863972,\"ProductTypeID\":1,\"ProductGroupID\":1,\"CompetitorID\":1,\"ProductID\":,\"UserID\":"+UserID+",\"UserRoleID\":8610,\"StoreID\":9817,\"SurveyResponseID\":355908,\"SurveyQuestionID\":751,\"UserResponse\":12,\"CoverageID\":1,\"CompetitionType\":1,\"Sellout\":0}]}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SubmitCompetitionBooked");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void SubmitCompetitionBooked_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"competitions\":[{\"CompSurveyID\":2863972,\"ProductTypeID\":1,\"ProductGroupID\":1,\"CompetitorID\":1,\"ProductID\":1,\"UserID\":" + (UserID+1) + ",\"UserRoleID\":8610,\"StoreID\":9817,\"SurveyResponseID\":355908,\"SurveyQuestionID\":751,\"UserResponse\":12,\"CoverageID\":1,\"CompetitionType\":1,\"Sellout\":0}]}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SubmitCompetitionBooked");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  SubmitCollectionSurvey
        [TestMethod]
        public void SubmitCollectionSurvey_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"collection\":[{\"CollectionSurveyID\":166,\"UserID\":"+UserID+",\"CoverageID\":663378,\"StoreID\":9813,\"SurveyResponseID\":682691,\"UserRoleID\":8610,\"PaymentModeID\":1,\"Amount\":400,\"Comments\":\"tstsg\",\"TransactionID\":\"23\",\"PaymentDate\":\"28-May-2015\"}]}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SubmitCollectionSurvey");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void SubmitCollectionSurvey_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"collection\":[{\"CollectionSurveyID\":166,\"UserID\":" + (UserID+1) + ",\"CoverageID\":663378,\"StoreID\":9813,\"SurveyResponseID\":682691,\"UserRoleID\":8610,\"PaymentModeID\":1,\"Amount\":400,\"Comments\":\"tstsg\",\"TransactionID\":\"23\",\"PaymentDate\":\"28-May-2015\"}]}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SubmitCollectionSurvey");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion

        #region  SubmitOrderBooking
        [TestMethod]
        public void SubmitOrderBooking_IsHeaderBodyUserIDMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"orders\":[{\"OrderBookingID\":1904,\"ProductTypeID\":3,\"ProductGroupID\":2988,\"ProductCategoryID\":29,\"SurveyResponseID\":467317,\"OrderNo\":\"_240065201_24_12_1\",\"ProductID\":22,\"Quantity\":55,\"StoreID\":377,\"UserID\":"+UserID+",\"UserRoleID\":8610,\"CoverageID\":3,\"OrderBookingType\":1}]}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SubmitOrderBooking");
            // Assert
            Assert.IsFalse(response.Contains("UnAuthorized access detected"));
        }
        [TestMethod]
        public void SubmitOrderBooking_IsHeaderBodyUserIDNotMatched_ReturnTrue()
        {
            // Arrange
            KeyTokenGenerator();
            Requestbody = "{\"orders\":[{\"OrderBookingID\":1904,\"ProductTypeID\":3,\"ProductGroupID\":2988,\"ProductCategoryID\":29,\"SurveyResponseID\":467317,\"OrderNo\":\"_240065201_24_12_1\",\"ProductID\":22,\"Quantity\":55,\"StoreID\":377,\"UserID\":" + (UserID+1) + ",\"UserRoleID\":8610,\"CoverageID\":3,\"OrderBookingType\":1}]}";
            //Act
            var response = SendHttpRequest(Headers, Requestbody, "SubmitOrderBooking");
            // Assert
            Assert.IsTrue(response.Contains("UnAuthorized access detected"));
        }
        #endregion
    }
}
