using ClassLibraryHelper.C_.HttpHelpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestCommonHelper
{
    
    
    /// <summary>
    ///这是 HttpHelperTest 的测试类，旨在
    ///包含所有 HttpHelperTest 单元测试
    ///</summary>
    [TestClass()]
    public class HttpHelperTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///获取或设置测试上下文，上下文提供
        ///有关当前测试运行及其功能的信息。
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region 附加测试特性
        // 
        //编写测试时，还可使用以下特性:
        //
        //使用 ClassInitialize 在运行类中的第一个测试前先运行代码
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //使用 ClassCleanup 在运行完类中的所有测试后再运行代码
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //使用 TestInitialize 在运行每个测试前先运行代码
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //使用 TestCleanup 在运行完每个测试后运行代码
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///GetHtml 的测试
        ///</summary>
        [TestMethod()]
        public void GetHtmlTest()
        {
            string url = "https://www.google.com.hk/search?hl=zh-CN&newwindow=1&safe=strict&biw=1280&bih=685&q=httpwebrequest+%E5%B0%81%E8%A3%85%E7%B1%BB&oq=httpwebrequest+%E5%B0%81%E8%A3%85%E7%B1%BB&aq=f&aqi=&aql=&gs_l=serp.3...1670.4592.0.4720.11.10.0.0.0.0.269.965.2-4.4.0...0.0.FAET4xqd1ww"; // TODO: 初始化为适当的值
            string expected = string.Empty; // TODO: 初始化为适当的值
            string actual;
            actual = HttpHelper.GetHtml(url);
          //  Assert.AreEqual(expected, actual);
          //  Assert.Inconclusive("验证此测试方法的正确性。");
        }
    }
}
