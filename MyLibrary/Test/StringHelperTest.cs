using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonLib.Tools;

namespace Test
{
    [TestClass]
    public class StringHelperTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            string str = "001";
   
            Assert.AreEqual("00000001", StringHelper.PadLeft(str,"0", 5));    //左补齐
            Assert.AreEqual("00100000", StringHelper.PadRight(str, "0", 5));   //右补齐
        }
    }
}
