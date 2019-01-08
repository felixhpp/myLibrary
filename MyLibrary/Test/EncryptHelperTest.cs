using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonLib.Encrypts;

namespace Test
{
    [TestClass]
    public class EncryptHelperTest
    {
        [TestMethod]
        public void TestEncrypt()
        {
            string str = "test";
            string encStr = EncryptHelper.AESEncrypt(str);
            string decStr = EncryptHelper.AESDecrypt(encStr);
            Assert.AreEqual(str, decStr);

            string encStr1 = EncryptHelper.DESEncrypt(str);
            string decStr1 = EncryptHelper.DESDecrypt(encStr1);
            Assert.AreEqual(str, decStr1);

            string encStr2 = EncryptHelper.MD5(str);
            string hmaencStr = EncryptHelper.HMACMD5(str);
        }

    }
}
