using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommonLib.File;

namespace Test
{
    [TestClass]
    public class FileHelperTest
    {
        [TestMethod]
        public void TestGetSuffix()
        {
            string currentDirectory = System.Environment.CurrentDirectory;  //XXXX\bin\Debug
            string path = currentDirectory + "\\directoryTest\\test.txt";
            string path2 = currentDirectory + "\\directoryTest\\test\\";
            // Assert
            Assert.AreEqual(".txt", FileHelper.GetSuffix(path));
            Assert.AreEqual("", FileHelper.GetSuffix(path2));
        }
        [TestMethod]
        public void TestCreateDirectory()
        {
            string currentDirectory = System.Environment.CurrentDirectory;  //XXXX\bin\Debug
            string path = currentDirectory + "\\directoryTest\\test.txt";
            string dicPath = currentDirectory + "\\directoryTest";

            Assert.IsFalse(System.IO.Directory.Exists(dicPath));
            Assert.AreEqual(true, FileHelper.CreateDirectory(path));
            Assert.IsTrue(System.IO.Directory.Exists(dicPath));
            Assert.AreEqual(true, FileHelper.DeleteDirectoryOrFile(path, true));
            Assert.IsFalse(System.IO.Directory.Exists(dicPath));
        }
    }
}
