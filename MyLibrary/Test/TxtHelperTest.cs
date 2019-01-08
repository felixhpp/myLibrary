using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonLib.Files;

namespace Test
{
    [TestClass]
    public class TxtHelperTest
    {
        [TestMethod]
        public void TestWrite()
        {
            string currentDirectory = System.Environment.CurrentDirectory;  //XXXX\bin\Debug
            string path = currentDirectory + "\\directoryTest\\test.txt";
            string content = "txthelper测试";
            TxtHelper.Write(path, content);

            Assert.AreEqual(content, TxtHelper.Read(path));
            FileHelper.DeleteDirectoryOrFile(path, true);
            Assert.IsFalse(System.IO.File.Exists(path));
        }

        [TestMethod]
        public void TestAppendWrite()
        {
            string currentDirectory = System.Environment.CurrentDirectory;  //XXXX\bin\Debug
            string path = currentDirectory + "\\directoryTest\\test.txt";
            string content = "txthelper测试";
            TxtHelper.Write(path, content, true, System.Text.Encoding.UTF8);
            TxtHelper.Write(path, content, true, System.Text.Encoding.UTF8);
            Assert.AreEqual(content + content, TxtHelper.Read(path, System.Text.Encoding.UTF8));
            FileHelper.DeleteDirectoryOrFile(path, true);
            Assert.IsFalse(System.IO.File.Exists(path));
        }
    }
}
