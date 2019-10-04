using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Samsung.SmartDost.CommonLayer.Aspects.DTO;
using Samsung.SmartDost.PresentationLayer.ServiceImpl;

namespace SmartDOSTServiceTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ComonLoginService_ValidInput_Success()
        {
            NewMethod("Object reference not set to an instance of an object.");
        }
        private static void NewMethod(string message)
        {
            //Arrange
            JsonResponse<ServiceOutputDTO> expected = new JsonResponse<ServiceOutputDTO>();
            expected.FailedValidations = null;
            expected.IsSuccess = false;
            expected.Message = message;
            expected.Result = null;
            expected.SingleResult = null;
            expected.StatusCode = null;

            //Action
            ISmartDost sdLogin = new SmartDost();
            ServiceInputDTO loginInputDTD = new ServiceInputDTO();
            loginInputDTD.apkVersion = "1.2.1";
            loginInputDTD.BrowserName = "Chrome";
            loginInputDTD.imei = "123456";
            loginInputDTD.IPAddress = "107.109.144.173";
            loginInputDTD.longitude = 45.545;
            loginInputDTD.longitude = 45.545;
            loginInputDTD.ModelName = "S3";
            loginInputDTD.password = "123456";
            var actual = sdLogin.CommonLoginService(loginInputDTD);

            //Assert
            Assert.AreEqual(expected.Message, actual.Message);
        }
        [TestMethod]
        public void ComonLoginService_ValidInput_Fail()
        {
            NewMethod("Object reference not set to an instance of an object.");         

        }
    }
}
