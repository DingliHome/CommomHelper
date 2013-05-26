using System.Collections.Generic;
using ClassLibraryHelper.C_;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TestCommonHelper
{


    /// <summary>
    ///This is a test class for EmployeeCloneTest and is intended
    ///to contain all EmployeeCloneTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EmployeeCloneTest
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
        ///A test for EmployeeClone Constructor
        ///</summary>
        [TestMethod()]
        public void EmployeeCloneConstructorTest()
        {
            EmployeeClone target = new EmployeeClone() { Age = 21, Name = "dingli" ,Department = new Department(){DepartmentName = "dep1"}};
            var clone = target.Clone() as EmployeeClone;
            Console.WriteLine(clone.Department.DepartmentName);
            target.Department.DepartmentName = "dep2";
            Console.WriteLine(clone.Department.DepartmentName);

            EmployeeClone dClone=new EmployeeClone(){Department = new Department(){DepartmentName = "dep1"}};
            var deepEmployeeClone = dClone.DeepEmployeeClone();
            Console.WriteLine(deepEmployeeClone.Department.DepartmentName);
            dClone.Department.DepartmentName = "dep2";
            Console.WriteLine(deepEmployeeClone.Department.DepartmentName);

            EmployeeClone employeeClone=new EmployeeClone(){Department = new Department(){DepartmentName = "dep1"}};
            var employeeClone1 = employeeClone.Clone(employeeClone);
            Console.WriteLine(employeeClone1.Department.DepartmentName);
            employeeClone.Department.DepartmentName = "dep2";
            Console.WriteLine(employeeClone1.Department.DepartmentName);
            //Assert.Inconclusive("TODO: Implement code to verify target");
        }

        /// <summary>
        ///A test for Clone
        ///</summary>
        [TestMethod()]
        public void CloneTest()
        {
            EmployeeClone target = new EmployeeClone(); // TODO: Initialize to an appropriate value
            object expected = null; // TODO: Initialize to an appropriate value
            object actual;
            actual = target.Clone();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Age
        ///</summary>
        [TestMethod()]
        public void AgeTest()
        {
            EmployeeClone target = new EmployeeClone(); // TODO: Initialize to an appropriate value
            int expected = 0; // TODO: Initialize to an appropriate value
            int actual;
            target.Age = expected;
            actual = target.Age;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        /// <summary>
        ///A test for Name
        ///</summary>
        [TestMethod()]
        public void NameTest()
        {
            EmployeeClone target = new EmployeeClone(); // TODO: Initialize to an appropriate value
            string expected = string.Empty; // TODO: Initialize to an appropriate value
            string actual;
            target.Name = expected;
            actual = target.Name;
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
