using REST_Helper.Utilities;
using NUnit.Framework;
using Common;
using System.IO;
using MD5Helper = Common.MD5Helper;

namespace IVLUploaderTestProject
{

    public  class MD5HelperTestClass
    {
        [SetUp]
        public void Setup()
        {
            MD5HelperTestDataClass md5 = new MD5HelperTestDataClass();
        }
        private  TestContext testContext;

        public  TestContext TestContext
        {
            get => testContext;
            set => testContext = value;
        }

        //[TestCase ("e10adc3949ba59abbe56e057f20f883e", "123456")]
        [Test]
        [TestCaseSource (typeof(MD5HelperTestDataClass), "stringTestDataArrays")]
        public static void GetMD5HashFromStringTestMethod(string expectedResult, string input)
        {
            var actualResult = MD5Helper.GetMd5Hash(input);
            Assert.AreEqual(expectedResult, actualResult);

        }

        [TestCaseSource(typeof(MD5HelperTestDataClass), "fileInfoCorrectTestDataArrays")]
        [Test]
        public static void GetMD5HashFromFileTestMethod(MD5ReturnResponse expectedResult, FileInfo input)
        {
            var actualResult = MD5Helper.GetMd5Hash(input);
            expectedResult.status = MD5responseStatusEnum.Success;
            Assert.AreEqual(expectedResult.status, actualResult.status);
        }

        [TestCaseSource(typeof(MD5HelperTestDataClass), "fileInfoWrongTestDataArrays")]
        [Test]
        public static void GetMD5WithWrongDataTestMethod(MD5ReturnResponse expectedResult, FileInfo input)
        {
            FileStream fs = new FileStream(input.FullName,FileMode.Open);
            
            var actualResult = MD5Helper.GetMd5Hash(input);
            Assert.AreEqual(expectedResult.status, actualResult.status);
        }

        
    }
}
