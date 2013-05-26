using System.Data;
using System.Data.SqlClient;
using System.Threading;
using ClassLibraryHelper.SQL;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestCommonHelper
{


    /// <summary>
    ///This is a test class for SqlHelperTest and is intended
    ///to contain all SqlHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SqlHelperTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
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

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for AsyncExecuteReader
        ///</summary>
        [TestMethod()]
        public void AsyncExecuteReaderTest()
        {
            SqlHelper target = new SqlHelper(); // TODO: Initialize to an appropriate value
            string sql = "select * from Employee"; // TODO: Initialize to an appropriate value
            var sqlHelperTest = new SqlHelperTest();
            Action<IAsyncResult> action = sqlHelperTest.EmployeeLoaded; // TODO: Initialize to an appropriate value
            target.AsyncExecuteReader(sql, action);
            Thread.Sleep(100000);// 主线程终止，会关闭子线程(异步sqlcommand)
            //Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }

        public void EmployeeLoaded(IAsyncResult result)
        {
            try
            {
                var cmd = (SqlCommand)result.AsyncState;
                if (cmd.Connection.State == ConnectionState.Closed)
                    cmd.Connection.Open();
                var endExecuteReader = cmd.EndExecuteReader(result);


                DataTable dt = new DataTable();
                dt.Load(endExecuteReader);

            }
            catch (Exception er)
            {

                throw;
            }

        }
    }
}
