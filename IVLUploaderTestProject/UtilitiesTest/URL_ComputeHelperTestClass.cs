using Cloud_Models.Models;
using REST_Helper.Utilities;
using NUnit.Framework;


namespace IVLUploaderTestProject
{
    public class URL_ComputeHelperTestClass
    {
        //URL_Model uRL_Model;
        //string Expec = @"TestData/TestData/TestData/TestData";
        [SetUp]
        public void Setup()
        {
            //uRL_Model = new URL_Model
            //{
            //    API_URL = "TestData/",
            //    API_URL_End_Point = "TestData",
            //    API_URL_Mid_Point = "TestData",
            //    API_URL_Start_Point = "TestData"
            //};
        }

        //[TestCase (uRL_Model,@"TestData\TestData\TestData\TestData" )]
        [TestCaseSource (typeof(URL_ComputeHelperTestClassData), "uRL_Model")]
        [Test]
        public void GetUrlTestMethod(URL_Model uRL_Model, string Expec)
        {
            var actualValue = URL_ComputeHelper.GetUrl(uRL_Model);
            Assert.AreEqual(Expec, actualValue);
        }
    }
}
