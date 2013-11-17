using System;
using ClassLibraryHelper.C_;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestCommonHelper
{
    [TestClass]
    public class MultiPartDataSenderTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var sender = new MultiPartDataSender();
            sender.AddTextField("key", "nihao");
            sender.AddLocalFile("File");
            if (sender.Connect())
            {
                var send = sender.Send();
                if (send)
                {
                    Assert.IsTrue(send);
                }
                sender.Close();
            }
        }
    }
}
